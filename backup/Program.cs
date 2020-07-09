using System;

namespace backup
{
    class Program
    {
        static void Main(string[] args)
        {
            // Credits
            PrintTitle();
            
            // Validate input params
            if (ValidateArgs(args))
            {
                try
                {
                    var backupProvider = new BackupProvider(args[0]);

                    // TODO: Determine if we are decrypting or not based on args[1] if it exists...

                    backupProvider.PerformBackup();
                }
                catch (Exception e)
                {
                    PrintException(e);
                }
            }

            // End of program
            Console.Write("\n\n");
            PrintLn();
            Console.WriteLine("\nPress any key to exit . . .");
            Console.ReadKey();
        }

        static void PrintTitle()
        {
            Console.Title = "System Backup";
            Console.WriteLine("System Backup Script 1.0"); 
            Console.WriteLine("\nDevelopers:");
            Console.WriteLine("\tJordan Hook (JordanHook.com)\n");
            PrintLn();
        }

        static bool ValidateArgs(string[] args)
        {
            if (args.Length == 0 || args.Length > 2 || (args.Length == 2 && (args[1] != "-d" || args[1] != "--decrypt")))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nUssage: backup.exe [config_input_location] [-d | --decrypt | optional]");
                Console.WriteLine("ex:");
                Console.WriteLine("\tbackup.exe ./config.json -d\n\n");
                Console.ResetColor();

                return false;
            }

            return true;
        }


        public static void PrintLn()
        {
            for (int i = 0; i < Console.BufferWidth; i++)
            {
                Console.Write("═");
            }
            Console.Write("\n");
        }

        public static void PrintException(Exception e)
        {
            PrintError(e.Message);
        }

        public static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n\n[ERROR] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PrintInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[INFO] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
