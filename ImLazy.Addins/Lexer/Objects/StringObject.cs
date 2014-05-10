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
        public string NextElementType { get { return null; } }

        public string ElementType
        {
            get { return "string"; }
        }

        public string Name
        {
            get { return "StringObject"; }
        }

        public object GetObject(string value)
        {
            return value;
        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new TextContent {Configuration = config};
        }
    }
}
