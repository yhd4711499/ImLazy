using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using System.ComponentModel.Composition;

namespace ImLazy.Addins.Actions
{
    [ExportMetadata("Type", typeof(CopyAction))]
    [Export(typeof(IActionAddin))]
    public class CopyAction : FileActionBase
    {
        public CopyAction()
        {
            SetAction(FileSystemUtil.CopyFileOrFolder);
        }

        protected override string GetName()
        {
            return "CopyAction".Local();
        }
    }
}
