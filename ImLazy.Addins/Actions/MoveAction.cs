using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using System.ComponentModel.Composition;

namespace ImLazy.Addins.Actions
{
    [ExportMetadata("Type", typeof(MoveAction))]
    [Export(typeof(IActionAddin))]
    public class MoveAction : FileActionBase
    {
        public MoveAction()
        {
            SetAction(FileSystemUtil.MoveFileOrFolder);
        }

        protected override string GetName()
        {
            return "MoveAction".Local();
        }
    }
}
