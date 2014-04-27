using ImLazy.Addins.Utils;
using ImLazy.Contracts;
using System.IO;
using System.ComponentModel.Composition;

namespace ImLazy.Addins.Actions
{
    [ExportMetadata("Name", "移动")]
    [ExportMetadata("Type", typeof(MoveAction))]
    [Export(typeof(IActionAddin))]
    public class MoveAction : FileActionBase
    {
        public MoveAction()
        {
            SetAction(FileSystemUtil.MoveFileOrFolder);
        }
    }
}
