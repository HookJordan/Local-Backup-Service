using System;
using System.Collections.Generic;
using System.IO;
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
        }
    }
}
