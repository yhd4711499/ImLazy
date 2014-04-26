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
    }
}