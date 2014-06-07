using System.ComponentModel.Composition;
using System.IO;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Subjects
{
    [Export(typeof(ISubject))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Subjects.ModifyDateSubject")]
    class ModifyDateSubject : ISubject
    {
        public string Name { get { return "ModifyDateSubject"; } }
        public LexerType ElementType { get; private set; }
        public LexerType GetVerbType()
        {
            return LexerTypes.DateType;
        }

        public object GetProperty(string filePath)
        {
            return File.GetLastWriteTime(filePath);
        }
    }
}
