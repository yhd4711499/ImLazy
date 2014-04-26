using System.IO;
using ImLazy.Addins.ContentViews;
using ImLazy.Contracts;
using System;
using System.Collections.Generic;
using ImLazy.Util;
using log4net;

namespace ImLazy.Addins.Actions
{
    public abstract class FileActionBase : IActionAddin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileActionBase));
        public void DoAction(string filePath, SerializableDictionary<string, object> dic)
        {
            var targetPath = dic.TryGetValue<string>("TargetObject");
            if (File.Exists(targetPath))
            {
                throw new Exception(String.Format("Target path exist : {0}. Unable to proceed !", targetPath));
            }
            if (Directory.Exists(targetPath))
            {
                // targetPath is dir. So copy filePath to targetPath folder
                Log.DebugFormat("Copying : {0} -> {1}", filePath, targetPath);
                FolderUtil.ToFolder(filePath, targetPath, _action);
            }
            else
            {
                // targetPath is not dir or is non-existed dir
                if (String.IsNullOrEmpty(Path.GetExtension(targetPath)))
                {
                    // no extension means the targetPath is a dir path.
                    // so the dirs should be made first then followed by copying.
                    FolderUtil.MakeDirs(targetPath);
                    Log.DebugFormat("Copying : {0} -> {1}", filePath, targetPath);
                    FolderUtil.ToFolder(filePath, targetPath, _action);
                }
                else
                {
                    // now targetPath is a full path.(ie. "D:\temp\a.ext")
                    // so copy from filePath directly to targetPath
                    Log.DebugFormat("Copying : {0} -> {1}", filePath, targetPath);
                    _action(filePath, targetPath);
                }
            }
        }

        private Action<string, string> _action;

        protected void SetAction(Action<string, string> action)
        {
            _action = action;
        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new TextContent { Configuration = config };
        }
    }
}
