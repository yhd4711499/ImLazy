using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ImLazy.Addins.ContentViews;
using ImLazy.Contracts;
using ImLazy.RunTime;
using ImLazy.SDK.Lexer;
using ImLazy.Util;

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
