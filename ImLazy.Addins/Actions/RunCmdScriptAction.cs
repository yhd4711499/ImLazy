﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ImLazy.Addins.ContentViews;
using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using ImLazy.SDK.Util;

namespace ImLazy.Addins.Actions
{
    [ExportMetadata("Type", typeof(RunCmdScriptAction))]
    [Export(typeof(IActionAddin))]
    class RunCmdScriptAction : RunScriptActionBase
    {
        protected override string GetScriptExt()
        {
            return ".bat";
        }

        public override string LocalName
        {
            get { return "RunCmdScriptAction".Local(); }
        }

        protected override string Execute(string scriptFilePath, string targetFilePath)
        {
            return ShellUtil.ExecuteCommandSync(String.Format("\"{0}\" {1}", scriptFilePath, targetFilePath));
        }

        protected override string GetSyntaxName()
        {
            return "CMD";
        }
    }
}
