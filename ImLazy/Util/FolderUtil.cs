using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ImLazy.Util
{
    public static class FolderUtil
    {
        public static void MakeDirs(string path)
        {
            const char pathSplitter = '\\';
            path = path.Replace('/', pathSplitter);
            var splits = path.Split('\\');
            var acc = new StringBuilder();
            acc.Append(splits[0]);
            acc.Append(pathSplitter);
            foreach (var str in splits.Skip(1))
            {
                acc.Append(str);
                acc.Append(pathSplitter);
                if (!Directory.Exists(acc.ToString()))
                    Directory.CreateDirectory(acc.ToString());
            }
        }

        public static void ToFolder(string filePath, string targetPath, Action<string,string> action)
        {
            var name = Path.GetFileName(filePath);
            Debug.Assert(name != null, "name != null");
            var fullPath = Path.Combine(targetPath, name);
            action(filePath, fullPath);
        }
    }
}