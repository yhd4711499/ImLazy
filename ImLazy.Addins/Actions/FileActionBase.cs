using System.IO;
using ImLazy.Addins.ContentViews;
using ImLazy.Addins.Utils;
using ImLazy.Contracts;
using System;
using System.Collections.Generic;
using ImLazy.RunTime;
using ImLazy.Util;
using log4net;
using LogManager = log4net.LogManager;

namespace ImLazy.Addins.Actions
{
    public abstract class FileActionBase : IActionAddin
    {
        private string _actionName;
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileActionBase));
        public void DoAction(string filePath, SerializableDictionary<string, object> dic)
        {
            var targetPath = dic.TryGetValue<string>(ConfigNames.ObjectValue);
            if (File.Exists(targetPath))
            {
                throw new Exception(String.Format("Target path exist : {0}. Unable to proceed !", targetPath));
            }
            if (targetPath.StartsWith("/") || targetPath.StartsWith("\\"))
// ReSharper disable once AssignNullToNotNullAttribute
                targetPath = Path.Combine(Path.GetDirectoryName(filePath), targetPath.Remove(0,1));
            if (Directory.Exists(targetPath))
            {
                // targetPath is dir. So copy filePath to targetPath folder
                ExecuteFileAction(filePath, targetPath, (f, t) => FolderUtil.ToFolder(f, t, _action));
            }
            else
            {
                // targetPath is not dir or is non-existed dir
                if (String.IsNullOrEmpty(Path.GetExtension(targetPath)))
                {
                    // no extension means the targetPath is a dir path.
                    // so the dirs should be made first then followed by copying.
                    FolderUtil.MakeDirs(targetPath);
                    ExecuteFileAction(filePath, targetPath, (f, t) => FolderUtil.ToFolder(f, t, _action));
                }
                else
                {
                    // now targetPath is a full path.(ie. "D:\temp\a.ext")
                    // so copy from filePath directly to targetPath
                    ExecuteFileAction(filePath, targetPath, _action);
                }
            }
        }

        private void ExecuteFileAction(string filePath, string targetPath, Action<string,string> action)
        {
            if (FileSystemUtil.IsFileLocked(filePath))
            {
                Log.InfoFormat("Source:[{0}] seems to be locked. Try it next time.", filePath);
                return;
            }
            if (FileSystemUtil.IsFileLocked(targetPath))
            {
                Log.InfoFormat("Target:[{0}] seems to be locked. Try it next time.", targetPath);
                return;
            }

            action(filePath, targetPath);
            Log.InfoFormat("{2} : {0} -> {1}", filePath, targetPath, _actionName);
        }

        private Action<string, string> _action;

        protected void SetAction(Action<string, string> action)
        {
            _action = action;
            _actionName = action.Method.Name;
        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new TextContent { Configuration = config };
        }

        protected abstract string GetName();
        public string LocalName { get { return GetName(); } }
    }
}
