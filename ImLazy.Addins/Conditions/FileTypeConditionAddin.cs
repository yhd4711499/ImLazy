using System;
using System.Collections.Generic;
using System.IO;
using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using ImLazy.Runtime;
using ImLazy.SDK.Util;
using ImLazy.Util;
using log4net;
using LogManager = ImLazy.Runtime.LogManager;

namespace ImLazy.Addins.Conditions
{
    //[ExportMetadata("Type", typeof(FileTypeConditionAddin))]
    //[Export(typeof(IConditionAddin))]
    class FileTypeConditionAddin:IConditionAddin
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(FileTypeConditionAddin));

        public static readonly Dictionary<string, Func<string, bool>> FileTypes = new Dictionary<string, Func<string, bool>>
            {
                {"File", File.Exists},
                {"Folder", Directory.Exists},
                {"Any", s => true},
            };

        static FileTypeConditionAddin()
        {
        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new FileTypeConditionAddinView {Configuration = config};
        }

        public string LocalName
        {
            get { return "FileTypeConditionAddin".Local(); }
        }

        public bool IsMatch(string filePath, SerializableDictionary<string, object> dic)
        {
            var type = dic.TryGetValue<string>(ConfigNames.FileType);
            if (!Log.CheckParam(type, "Can't parse a correct file type from [{0}]. Return false instead.", type))
                return false;

            Func<string, bool> matchTypeFunc;
            if (FileTypes.TryGetValue(type, out matchTypeFunc)) return matchTypeFunc(filePath);

            Log.ErrorFormat("Can't parse a correct file type from [{0}]. Return false instead.", type);
            return false;
        }
    }
}
