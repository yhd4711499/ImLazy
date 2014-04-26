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
    public class CopyAction : IActionAddin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (CopyAction));
        public void DoAction(string filePath, SerializableDictionary<string, object> dic)
        {
            var targetPath = dic.TryGetValue<string>("TargetObject");
            if (File.Exists(targetPath))
            {
                throw new Exception(String.Format("Target path exist : {0}. Unable to proceed !", targetPath));
            }
            Log.DebugFormat("Copying : {0} -> {1}",filePath, targetPath);
            File.Copy(filePath, targetPath);
        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new TextContent {Configuration = config};
        }
    }
}
