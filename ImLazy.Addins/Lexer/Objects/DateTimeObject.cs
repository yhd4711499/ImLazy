using System.Collections.Generic;
using System.ComponentModel.Composition;
using ImLazy.Addins.ContentViews;
using ImLazy.SDK.Base.Contracts;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Objects
{
    [Export(typeof(IObject))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Objects.DateTimeObject")]
    class DateTimeObject : IObject
    {
        public string Name { get { return "DateTimeObject"; } }
        public LexerType ElementType { get { return LexerTypes.TimeSpan; } }
        public object GetObject(string value)
        {
            return value;
        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config, LexerType type)
        {
            return new TimeSpanContent { Configuration = config, MinWidth = 50 };
        }
    }
}
