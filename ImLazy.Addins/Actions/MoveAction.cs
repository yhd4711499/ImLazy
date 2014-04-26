using ImLazy.Contracts;
using System.Collections.Generic;
using System.IO;
using ImLazy.Util;
using System.ComponentModel.Composition;
using ImLazy.Addins.ContentViews;
using ImLazy.RunTime;
using System;

namespace ImLazy.Addins.Actions
{
    [ExportMetadata("Name", "移动")]
    [ExportMetadata("Type", typeof(MoveAction))]
    [Export(typeof(IActionAddin))]
    public class MoveAction : FileActionBase
    {
        public MoveAction()
        {
            SetAction(File.Move);
        }
    }
}
