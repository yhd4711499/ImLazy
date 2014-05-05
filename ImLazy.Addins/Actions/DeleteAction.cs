using ImLazy.Addins.Utils;
using ImLazy.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;

namespace ImLazy.Addins.Actions
{
    [ExportMetadata("Type", typeof(DeleteAction))]
    [Export(typeof(IActionAddin))]
    public class DeleteAction:IActionAddin
    {
        public void DoAction(string filePath, SerializableDictionary<string, object> dic)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
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
