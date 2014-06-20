using System;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using ImLazy.Entities;
using Microsoft.Win32;

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
                // Create directory for the database.
                string dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ImLazy");
                if (!Directory.Exists(dataDirectory))
                {
                    Directory.CreateDirectory(dataDirectory);
                }

                // Set |DataDirectory| macro to our own path. This macro is used within the connection string.
                AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);

                Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ModelContainer>()); 

            }
            catch (Exception)
            {
                try
                {
                    InitRegistry(AppDomain.CurrentDomain.BaseDirectory);
                }
                catch (Exception)
                {
                    LocalStorageFolder = "C:\\";
                }
            }
        }

        private static void ReadRegistry()
        {
            var r = Registry.LocalMachine.OpenSubKey("SOFTWARE", false).OpenSubKey("Ornithopter").OpenSubKey("ImLazy");
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
    }
    // ReSharper restore PossibleNullReferenceException
}
