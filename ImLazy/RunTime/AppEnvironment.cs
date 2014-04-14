using System;
using System.IO;

namespace ImLazy.RunTime
{
    static class AppEnvironment
    {
        public static readonly String LocalStorageFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ImLazy");

        static AppEnvironment()
        {
            Directory.CreateDirectory(LocalStorageFolder);
        }
    }
}
