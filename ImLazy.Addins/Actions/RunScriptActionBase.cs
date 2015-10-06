using System;
using System.Collections.Generic;
using System.IO;
using ImLazy.Runtime;
using ImLazy.SDK.Base.Contracts;
using ImLazy.Util;
using log4net;
using LogManager = ImLazy.Runtime.LogManager;
using ImLazy.Addins.ContentViews;

namespace ImLazy.Addins.Actions
{
    abstract class RunScriptActionBase : IActionAddin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (RunScriptActionBase));
        public virtual IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new ScriptContent { Configuration = config, ScriptExt = GetScriptExt()};
        }

        protected abstract string GetScriptExt();
        protected abstract string GetSyntaxName();

        public abstract string LocalName { get; }

        protected abstract string Execute(string scriptFilePath, string targetFilePath);

        public void DoAction(string filePath, SerializableDictionary<string, object> dic, out string updatedFilePath)
        {
            // 1.Get script string from config.
            var script = dic.TryGetValue<string>(ConfigNames.ObjectValue);

            // 2.Save to a temp file.
            var scriptFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + GetScriptExt());
            using (TextWriter writer = new StreamWriter(scriptFilePath))
            {
                writer.Write(script);
            }

            // 3.Run this script file with filePath as argument.
            Log.DebugFormat("Executing script {0} ...", scriptFilePath);
            var output = Execute(scriptFilePath, filePath);
            Log.DebugFormat("Done. Output is {0}", output);

            // 4.Delete it.
            File.Delete(scriptFilePath);

            if (Directory.Exists(output) || File.Exists(output))
                updatedFilePath = output;
            else
                updatedFilePath = null;
        }
    }
}
