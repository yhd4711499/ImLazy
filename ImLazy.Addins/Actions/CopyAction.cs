using System;
using ImLazy.Contracts;
using System.Collections.Generic;
using System.IO;
using ImLazy.Util;
using System.ComponentModel.Composition;
using ImLazy.Addins.ContentViews;
using log4net;

namespace ImLazy.Addins.Actions
{
    [ExportMetadata("Name", "复制")]
    [ExportMetadata("Type", typeof(CopyAction))]
    [Export(typeof(IActionAddin))]
    public class CopyAction : FileActionBase
    {
        public CopyAction()
        {
            SetAction(File.Copy);
        }
    }
}
