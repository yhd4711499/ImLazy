using System.Collections.Generic;
using System.ComponentModel.Composition;
using ImLazy.SDK.Base.Contracts;
using ImLazy.Util;

namespace ImLazy.Addins
{
    [ExportMetadata("Type", typeof(ConditionsConfigAddin))]
    [Export(typeof(IAddin))]
    class ConditionsConfigAddin:IAddin
    {
        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new ConditionsConfigAddinView { Configuration = config };
        }

        public string LocalName
        {
            get { return "ConditionsConfigAddin".Local(); }
        }
    }
}
