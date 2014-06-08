using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ImLazy.Util
{
    public static class FolderUtil
    {
        /// <summary>
        /// 创建目录与其所有的父目录
        /// </summary>
        /// <param name="path">路径，如果是文件路径，则取其目录</param>
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

        /// <summary>
        /// 将filePath的文件名组合到targetPath目录之下，并执行action
        /// </summary>
        /// <param name="filePath">源文件的路径</param>
        /// <param name="targetPath">目标目录的路径</param>
        /// <param name="action">动作</param>
        public static void ToFolder(string filePath, string targetPath, Action<string,string> action)
        {
            var name = Path.GetFileName(filePath);
            Debug.Assert(name != null, "name != null");
            var fullPath = Path.Combine(targetPath, name);
            action(filePath, fullPath);
        }
    }
}