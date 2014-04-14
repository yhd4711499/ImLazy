//using ImLazy.Contracts;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.Composition;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Xml.Serialization;

//namespace ImLazy.Addins
//{
//    //[ExportMetadata("Name", NAME)]
//    //[Export(typeof(IConditionAddin))]
//    class FilePropertyConditionAddin : IConditionAddin
//    {
//        public const String NAME = "ImLazy.Addins.FilePropertyConditionAddin";

//        private List<Lazy<IConditionAddin, IConditionAddinMetadata>> _availConditions;
//        public List<Lazy<IConditionAddin, IConditionAddinMetadata>> AvailConditions
//        {
//            get
//            {
//                if (_availConditions == null)
//                {
//                    _availConditions = new List<Lazy<IConditionAddin, IConditionAddinMetadata>>();
//                    AddinHost.Instance.ConditionAddins.Where(l => l.Metadata.Parent.Equals(typeof(FilePropertyConditionAddin).FullName)).ToList().ForEach(_ => AvailConditions.Add(_));
//                }
//                return _availConditions;
//            }
//        }

//        private IConditionAddin _selectedConditionAddin;

//        public bool IsMatch(string filePath)
//        {
//            return _selectedConditionAddin.IsMatch(filePath);
//        }

//        public void Configurate(Dictionary<string,object> arg)
//        {
//            object selectedName = null;
//            if (arg.TryGetValue("Selected", out selectedName)) {
//                _selectedConditionAddin = AvailConditions.Where(l => l.Metadata.Name.Equals(selectedName.ToString())).FirstOrDefault().Value;
//                _selectedConditionAddin.Configurate(arg);
//            }
//        }

//        public string Symbol
//        {
//            get;
//            set;
//        }

//        public IEditView GetMainView(Dictionary<string, object> config)
//        {
//            var v = new FilePropertyConditionAddinView(this);
//            if (config != null)
//                v.Configuration = config;
//            return v;
//        }
//    }
//}
