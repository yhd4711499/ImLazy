using System;
using Microsoft.Win32;

namespace ImLazy.RunTime
{
    // ReSharper disable PossibleNullReferenceException
    public static class AppEnvironment
    {
        public static string LocalStorageFolder { get; private set; }

        static AppEnvironment()
        {
            try
            {
                var r = Registry.LocalMachine.OpenSubKey("SOFTWARE", false).OpenSubKey("Ornithopter").OpenSubKey("ImLazy");
                LocalStorageFolder = (string)r.GetValue("mainExePath");

            }
            catch (Exception ex)
            {
                try
                {
                    InitRegistry(System.AppDomain.CurrentDomain.BaseDirectory);
                }
                catch (Exception)
                {
                    LocalStorageFolder = "C:\\";
                }
            }
        }

        public static void InitRegistry(string mainExeFilePath)
        {
            var r = Registry.LocalMachine.OpenSubKey("SOFTWARE", true).CreateSubKey("Ornithopter").CreateSubKey("ImLazy");
            r.SetValue("mainExePath", mainExeFilePath);
            LocalStorageFolder = mainExeFilePath;
        }
    }
    // ReSharper restore PossibleNullReferenceException
}
