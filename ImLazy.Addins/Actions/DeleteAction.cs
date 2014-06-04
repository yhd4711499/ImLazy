using System.Collections.Generic;
using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using System.ComponentModel.Composition;

namespace ImLazy.Addins.Actions
{
    [ExportMetadata("Type", typeof(DeleteAction))]
    [Export(typeof(IActionAddin))]
    public class DeleteAction:IActionAddin
    {
        public void DoAction(string filePath, SerializableDictionary<string, object> dic, out string updatedPath)
        {
            FileSystemUtil.DeleteFileOrFolder(filePath);
            updatedPath = null;
        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return null;
        }

        public string LocalName
        {
            get { return "DeleteAction".Local(); }
        }
    }
}
