using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImLazy.Addins.Utils;
using ImLazy.Contracts;
using ImLazy.RunTime;
using ImLazy.Util;
using log4net;
using WpfLocalization;
using ConfigurationHelper = ImLazy.Util.ConfigurationHelper;
using LogManager = ImLazy.RunTime.LogManager;

namespace ImLazy.Addins.Conditions
{
    [ExportMetadata("Type", typeof(FileTypeConditionAddin))]
    [Export(typeof(IConditionAddin))]
    class FileTypeConditionAddin:IConditionAddin
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(FileTypeConditionAddin));

        public static readonly Dictionary<string, Func<string, bool>> FileTypes = new Dictionary<string, Func<string, bool>>
            {
                {"File", File.Exists},
                {"Folder", Directory.Exists},
                {"Any", (s) => true},
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
