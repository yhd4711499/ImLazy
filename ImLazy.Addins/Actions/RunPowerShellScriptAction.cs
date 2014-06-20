using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ImLazy.Addins.ContentViews;
using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using ImLazy.SDK.Util;

namespace ImLazy.Addins.Actions
{
    [ExportMetadata("Type", typeof(RunPowerShellScriptAction))]
    [Export(typeof(IActionAddin))]
    class RunPowerShellScriptAction : RunScriptActionBase
    {
        public override IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new ScriptContent {Configuration = config};
        }

        protected override string GetScriptExt()
        {
            return ".ps1";
        }

        public override string LocalName
        {
            get { return "RunPowerShellScriptAction".Local(); }
        }

        protected override string Execute(string scriptFilePath, string targetFilePath)
        {
            return ShellUtil.ExecuteCommandSync(String.Format("\"{0}\" \"{1}\"", scriptFilePath, targetFilePath), "powershell");
        }
    }
}
