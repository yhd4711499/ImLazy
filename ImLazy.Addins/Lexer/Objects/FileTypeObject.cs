using System.Collections.Generic;
using System.ComponentModel.Composition;
using ImLazy.Addins.ContentViews;
using ImLazy.Addins.Lexer.Subjects;
using ImLazy.Runtime;
using ImLazy.SDK.Base.Contracts;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Objects
{
    [Export(typeof(IObject))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Objects.FileTypeObject")]
    class FileTypeObject : IObject
    {
        public string Name
        {
            get { return "FileTypeObject"; }
        }

        public LexerType ElementType
        {
            get { return LexerTypes.FileType; }
        }

        public object GetObject(string value)
        {
            return LexerType.FromFullName(value);
        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config, LexerType type)
        {
            config[ConfigNames.AvailableItems] = FileTypeSubject.SupportedTypes;//.Select(_ => _.Name);
            return new ComboxContent { Configuration = config, MinWidth = 50};
        }
    }
}
