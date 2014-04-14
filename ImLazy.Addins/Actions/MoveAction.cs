using ImLazy.Contracts;
using System.Collections.Generic;
using System.IO;
using ImLazy.Util;
using System.ComponentModel.Composition;
using ImLazy.Addins.ContentViews;
using ImLazy.RunTime;

namespace ImLazy.Addins.Actions
{
    [ExportMetadata("Name", "移动")]
    [ExportMetadata("Type", typeof(MoveAction))]
    [Export(typeof(IActionAddin))]
    public class MoveAction : IActionAddin
    {
        public void DoAction(string filePath, SerializableDictionary<string, object> dic)
        {
            var targetPath = dic.TryGetValue<string>(ConfigNames.TargetObject);
            if (File.Exists(targetPath))
            {
                // TODO:
            }
            else
            {
                File.Move(filePath, targetPath);
            } 
        }
        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new TextContent { Configuration = config };
        }
    }
}
