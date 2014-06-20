using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using System.ComponentModel.Composition;

namespace ImLazy.Addins.Actions
{
    [ExportMetadata("Type", typeof(RenameAction))]
    [Export(typeof(IActionAddin))]
    public class RenameAction : FileActionBase
    {
        public RenameAction()
        {
            SetAction(FileSystemUtil.RenameFileOrFolder);
        }

        protected override string GetName()
        {
            return "RenameAction".Local();
        }
    }
}
