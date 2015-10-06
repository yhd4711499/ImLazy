using System;
using Microsoft.Win32;
using System.IO;

namespace ImLazy.Runtime
{
    // ReSharper disable PossibleNullReferenceException
    public static class AppEnvironment
    {
        public static string LocalStorageFolder { get; private set; }

        static AppEnvironment()
        {
            try
            {
                ReadRegistry();
                /*// Create directory for the database.
                var dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ImLazy");
                if (!Directory.Exists(dataDirectory))
                {
                    Directory.CreateDirectory(dataDirectory);
                }

                // Set |DataDirectory| macro to our own path. This macro is used within the connection string.
                AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);

                Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ModelContainer>()); */

            }
            catch (Exception)
            {
                try
                {
                    InitRegistry(AppDomain.CurrentDomain.BaseDirectory);
                }
                catch (Exception)
                {
                    LocalStorageFolder = AppDomain.CurrentDomain.BaseDirectory;
                }
            }
        }

        private static void ReadRegistry()
        {
            var r = Registry.LocalMachine.OpenSubKey("SOFTWARE", false).OpenSubKey("Ornithopter", false).OpenSubKey("ImLazy", false);
            LocalStorageFolder = (string)r.GetValue("mainExePath");
        }

        public static void InitRegistry(string mainExeFilePath)
        {
            try
            {
                ReadRegistry();
            }
            catch
            {
                try
                {
                    var r = Registry.LocalMachine.OpenSubKey("SOFTWARE", true).CreateSubKey("Ornithopter").CreateSubKey("ImLazy");
                    r.SetValue("mainExePath", mainExeFilePath);
                    LocalStorageFolder = mainExeFilePath;
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(typeof(AppEnvironment)).Error("Failed in initiating registry!", ex);
                    throw;
                }
            }
            
        }

        /// <summary>
        /// get the syntax rule file path for given syntax
        /// </summary>
        /// <param name="syntax"></param>
        /// <returns></returns>
        public static string GetSyntaxHighlightingRulePath(string syntax)
        {
            return Path.Combine("syntax", syntax);
        }
    }
    // ReSharper restore PossibleNullReferenceException
}
