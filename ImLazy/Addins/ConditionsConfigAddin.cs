using System.Collections.Generic;
using System.ComponentModel.Composition;
using ImLazy.Contracts;

namespace ImLazy.Addins
{
    [ExportMetadata("Name", "条件组")]
    [ExportMetadata("Type", typeof(ConditionsConfigAddin))]
    [Export(typeof(IAddin))]
    class ConditionsConfigAddin:IAddin
    {
        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new ConditionsConfigAddinView();
        }
    }
}
