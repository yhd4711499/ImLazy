using ImLazy.Contracts;
using System;
using System.Collections.Generic;

namespace ImLazy.Addins.Actions
{
    class FileActionBase : IActionAddin
    {
        public void DoAction(string filePath, SerializableDictionary<string, object> dic)
        {
            throw new NotImplementedException();
        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            throw new NotImplementedException();
        }
    }
}
