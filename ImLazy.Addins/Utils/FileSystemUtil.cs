using System.Collections.Generic;
using System.IO;

namespace ImLazy.Addins.Utils
{
    internal static class FileSystemUtil
    {
        public static void CopyFileOrFolder(string source, string dest)
        {
            if (Directory.Exists(source))
            {
                DirectoryCopy(source, dest, true);
            }
            else
            {
                File.Copy(source, dest);
            }
        }

        public static void DeleteFileOrFolder(string source)
        {
            if (Directory.Exists(source))
            {
                Directory.Delete(source, true);
            }
            else
            {
                File.Delete(source);
            }
        }

        public static void MoveFileOrFolder(string source, string dest)
        {
            if (Directory.Exists(source))
            {
                if (Path.GetPathRoot(source) != Path.GetPathRoot(dest))
                {
                    DirectoryCopy(source, dest, true);
                    Directory.Delete(source, true);
                }
                else
                {
                    Directory.Move(source, dest);
                }
                
            }
            else
            {
                File.Move(source, dest);
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);
            var dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (!copySubDirs) return;
            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, true);
            }
        }

        private static IEnumerable<FileInfo> WalkDirectoryTree(DirectoryInfo root)
        {
            // First, process all the files directly under this folder
            var files = root.GetFiles("*.*");

            foreach (var fi in files)
            {
                // In this example, we only access the existing FileInfo object. If we
                // want to open, delete or modify the file, then
                // a try-catch block is required here to handle the case
                // where the file has been deleted since the call to TraverseTree().
                yield return fi;
            }

            // Now find all the subdirectories under this directory.
            var subDirs = root.GetDirectories();

            foreach (var dirInfo in subDirs)
            {
                // Resursive call for each subdirectory.
                WalkDirectoryTree(dirInfo);
            }
        }

        public static bool IsFileLocked(string path)
        {
            if (!File.Exists(path))
                return false;
            var flag = true;
            try
            {
                using (new FileStream(path, FileMode.Open))
                {
                    flag = false;
                }
            }
            catch (IOException)
            {
                
            }
            return flag;
        }
    }
}
