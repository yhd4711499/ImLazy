using System;
using System.IO;
using ImLazy.Util;
using Microsoft.Win32;

namespace ImLazy.RunTime
{
    public static class AppEnvironment
    {
        public static string LocalStorageFolder { get; private set; }

        public static void InitRegistry(string mainExeFilePath)
        {
// ReSharper disable PossibleNullReferenceException
            var r = Registry.LocalMachine.OpenSubKey("SOFTWARE", true).CreateSubKey("Ornithopter").CreateSubKey("ImLazy");
            r.SetValue("mainExePath", mainExeFilePath);
// ReSharper restore PossibleNullReferenceException
            LocalStorageFolder = mainExeFilePath;
        }
    }
}
