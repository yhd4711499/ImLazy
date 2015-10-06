using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ImLazy.Addins.ContentViews;
using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using ImLazy.SDK.Util;

namespace ImLazy.Addins.Actions
{
    [ExportMetadata("Type", typeof(RunCSharpScriptAction))]
    [Export(typeof(IActionAddin))]
    class RunCSharpScriptAction : RunScriptActionBase
    {
        protected override string GetScriptExt()
        {
            return ".cs";
        }

        public override string LocalName
        {
            get { return "RunCSharpScriptAction".Local(); }
        }

        protected override string Execute(string scriptFilePath, string targetFilePath)
        {
            // TODO: csharp parser.
            return ShellUtil.ExecuteCommandSync(String.Format("\"{0}\" {1}", scriptFilePath, targetFilePath));
        }

        protected override string GetSyntaxName()
        {
            return "csharp";
        }
    }
}
