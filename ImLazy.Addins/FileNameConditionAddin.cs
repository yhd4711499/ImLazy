//using ImLazy.Contracts;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.Composition;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;

//namespace ImLazy.Addins
//{
//    //[ExportMetadata("Parent", "ImLazy.Addins.FilePropertyConditionAddin")]
//    //[ExportMetadata("Name", "ImLazy.Addins.FileNameConditionAddin")]
//    //[Export(typeof(IConditionAddin))]
//    class FileNameConditionAddin : IConditionAddin
//    {
//        private String TargetString;

//        public bool IsMatch(string filePath)
//        {
//            var name = Path.GetFileNameWithoutExtension(filePath);
//            return _availCheckers[Symbol](name, TargetString);
//        }

//        public void Configurate(Dictionary<string,object> arg)
//        {
//            Symbol = arg["Symbol"].ToString();
//            TargetString = arg["Param"].ToString();
//        }

//        public string Symbol
//        {
//            get;
//            set;
//        }

//        private static readonly Dictionary<string, Func<string, string, bool>> _availCheckers = new Dictionary<string, Func<string, string, bool>> { 
//            {"contains",(a,b)=>{return a.Contains(b);}},
//            {"not contains",(a,b)=>{return !a.Contains(b);}},
//            {"starts with",(a,b)=>{return !a.StartsWith(b);}},
//            {"ends with",(a,b)=>{return !a.EndsWith(b);}},
//        };

//        private static readonly IEnumerable<string> _availSymbols = _availCheckers.Select(_ => _.Key);

//        public IEditView GetMainView(Dictionary<string, object> config)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
