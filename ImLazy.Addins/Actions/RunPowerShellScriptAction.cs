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

        protected override string GetSyntaxName()
        {
            return "power_shell";
        }
    }
}
