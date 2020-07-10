using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace backup
{
    class BackupProvider
    {
        public Config Config { get; private set; }

        // Hide Default Constructor
        private BackupProvider() { }

        public BackupProvider(string configPath)
        {
            this.LoadConfiguration(configPath);
        }

        private void LoadConfiguration(string configPath)
        {
            if (!File.Exists(configPath))
            {
                throw new Exception($"The configuration file path provided does not exist: \n'{configPath}'");
            }
            else
            {
                var rawText = File.ReadAllText(configPath);
                var config = JsonSerializer.Deserialize<Config>(rawText);

                // Validate all input directories exist
                foreach (var input in config.InputDirectories)
                {
                    if (!Directory.Exists(input))
                        throw new Exception($"Invalid input directory detected: \n'{input}'");
                }

                // Validate output directory exists, if not create it
                if (!Directory.Exists(config.Output))
                    Directory.CreateDirectory(config.Output);

                this.Config = config;
            }
        }

        public void PerformBackup()
        {
            // Generate timestamp to tag backup
            var now = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-s");
            var outpath = Path.Combine(Config.Output, now) + ".zip";
            long fileBackupCount = 0;

            Console.Write("\n");
            Program.PrintInfo($"Backing up data to: {outpath}");

            // Create new zip file
            using (FileStream fs = new FileStream(outpath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Create, false))
                {
                    // Iterate the targetted directories to store in the zip file
                    foreach (var input in Config.InputDirectories)
                    {
                        Program.PrintInfo($"Backing up input directory '{input}'");
                        fileBackupCount += zipFolder(input, input, zip, fileBackupCount);
                    }
                }
            }

            Program.PrintInfo($"\n{fileBackupCount} files were backed up.");
        }

        private long zipFolder(string root, string directory, ZipArchive zip, long count = 0)
        {
            foreach (var file in Directory.EnumerateFiles(directory))
            {
                count++;
                Program.PrintInfo(file);

                // Strip the root directory from the file path and switch to unix style folders
                string path = file.Replace(root + "\\", "").Replace("\\", "/");
                var entry = zip.CreateEntry(path);
                using (var entryStream = entry.Open())
                {
                    using (var inputFs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        inputFs.CopyTo(entryStream);
                    }
                }
            }

            foreach (var folder in Directory.EnumerateDirectories(directory))
            {
                zipFolder(root, folder, zip, count);
            }

            return count;
        }
    }
}
