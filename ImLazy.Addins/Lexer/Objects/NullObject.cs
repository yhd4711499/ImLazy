using System.Collections.Generic;
using System.ComponentModel.Composition;
using ImLazy.SDK.Base.Contracts;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Objects
{
    [Export(typeof(IObject))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Objects.NullObject")]
    class NullObject : IObject
    {
        public string Name
        {
            get { return "NullObject"; }
        }

        public LexerType ElementType
        {
            get { return LexerTypes.Null; }
        }

        public object GetObject(string value)
        {
            return null;
        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config, LexerType type)
        {
            return null;
        }
    }
}
