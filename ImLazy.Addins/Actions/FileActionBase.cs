﻿using System.Collections.Generic;
using System.IO;
using ImLazy.Addins.ContentViews;
using ImLazy.SDK.Base.Contracts;
using System;
using ImLazy.Runtime;
using ImLazy.Util;
using log4net;
using LogManager = log4net.LogManager;

namespace ImLazy.Addins.Actions
{
    public abstract class FileActionBase : IActionAddin
    {
        private string _actionName;
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileActionBase));

        /// <summary>
        /// convert to abs path if necessary.
        /// </summary>
        /// <param name="filePath">the file path of source file which you want do something with.
        /// <para>e.g. "d:/Downloads"</para></param>
        /// <param name="targetPath">the target folder or file path
        /// <para>e.g. "/tmp"</para></param>
        /// <returns>Abs path
        /// <para>e.g. "d:/Downloads/tmp"</para></returns>
        private static string GetAbsPath(string filePath, string targetPath)
        {
            if (targetPath.StartsWith("/") || targetPath.StartsWith("\\"))
                // ReSharper disable once AssignNullToNotNullAttribute
                return Path.Combine(Path.GetDirectoryName(filePath), targetPath.Remove(0, 1));
            return targetPath;
        }

        public void DoAction(string filePath, SerializableDictionary<string, object> dic, out string updatedPath)
        {
            var targetPath = dic.TryGetValue<string>(ConfigNames.ObjectValue);
            if (File.Exists(targetPath))
            {
                throw new Exception(String.Format("Target path exist : {0}. Unable to proceed !", targetPath));
            }

            string newPath;

            // convert to abs path if necessary.
            targetPath = GetAbsPath(filePath, targetPath);

            Action<string, string> action;

            if (String.IsNullOrEmpty(Path.GetExtension(targetPath)))
            {
                // targetPath is dir. So copy filePath to targetPath folder
                action = (f, t) => FolderUtil.ToFolder(f, t, _action);

                // dirs should be made first if needed.
                FolderUtil.MakeDirs(targetPath);

                // update path
                newPath = Path.Combine(targetPath, Path.GetFileName(filePath));
            }
            else
            {
                // now targetPath is a full path.(ie. "D:\temp\a.ext")
                // so copy from filePath directly to targetPath
                action = _action;

                // update path
                newPath = targetPath;
            }

            // ready? go!
            updatedPath = TryExecuteFileAction(filePath, targetPath, action) ? newPath : null;
        }

        private bool TryExecuteFileAction(string filePath, string targetPath, Action<string,string> action)
        {
            // there is no need to check both path.
            /*if (FileSystemUtil.IsFileLocked(filePath))
            {
                Log.InfoFormat("Source:[{0}] seems to be locked. Try it next time.", filePath);
                return;
            }
            if (FileSystemUtil.IsFileLocked(targetPath))
            {
                Log.InfoFormat("Target:[{0}] seems to be locked. Try it next time.", targetPath);
                return;
            }*/
            var result = true;
            try
            {
                action(filePath, targetPath);
                Log.InfoFormat("{2} : [{0}] -> [{1}]", filePath, targetPath, _actionName);
            }
            catch
            {
                Log.WarnFormat("\"{0}\" seems to be locked. Try it next time.", filePath);
                result = false;
            }
            return result;
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
