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
//    //[ExportMetadata("Name", "ImLazy.Addins.ExtensionConditionAddin")]
//    //[Export(typeof(IConditionAddin))]
//    class ExtensionConditionAddin:IConditionAddin
//    {
//        private IEnumerable<string> _extensions;
//        public bool IsMatch(string filePath)
//        {
//            var ext = Path.GetExtension(filePath);
//            return _extensions.Any(a => _availCheckers[Symbol](ext, a));
//        }

//        public void Configurate(Dictionary<string,object> arg)
//        {
//            _extensions = arg["Param"].ToString().Split(',');
//            Symbol = arg["Symbol"].ToString();
//        }

//        public string Symbol
//        {
//            get;
//            set;
//        }

//        private static readonly Dictionary<string, Func<string, string, bool>> _availCheckers = new Dictionary<string, Func<string, string, bool>> { 
//            {"==",(a,b)=>{return String.Compare(a, b, true) == 0;}},
//            {"!=",(a,b)=>{return String.Compare(a, b, true) != 0;}},
//        };

//        private static readonly IEnumerable<string> _availSymbols = _availCheckers.Select(_ => _.Key);

//        public IEditView GetMainView(Dictionary<string, object> config)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
