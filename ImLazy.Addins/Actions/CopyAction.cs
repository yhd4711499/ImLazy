using ImLazy.Addins.Utils;
using ImLazy.Contracts;
using System.IO;
using System.ComponentModel.Composition;

namespace ImLazy.Addins.Actions
{
    [ExportMetadata("Name", "复制")]
    [ExportMetadata("Type", typeof(CopyAction))]
    [Export(typeof(IActionAddin))]
    public class CopyAction : FileActionBase
    {
        public CopyAction()
        {
            SetAction(FileSystemUtil.CopyFileOrFolder);
        }
    }
}
