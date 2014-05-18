using System.Collections.Generic;
using System.ComponentModel.Composition;
using ImLazy.Addins.ContentViews;
using ImLazy.SDK.Base.Contracts;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Objects
{
    [Export(typeof(IObject))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Objects.StringObject")]
    class StringObject : IObject
    {
        public string Name
        {
            get { return "StringObject"; }
        }

        public LexerType ElementType
        {
            get { return LexerTypes.StringType; }
        }

        public object GetObject(string value)
        {
            return value;
        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new TextContent {Configuration = config, MinWidth = 50};
        }
    }
}
