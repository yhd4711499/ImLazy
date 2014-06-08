using System;
using System.ComponentModel.Composition;
using System.IO;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Subjects
{
    [Export(typeof(ISubject))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Subjects.FileExtensionSubject")]
    public class FileExtensionSubject : ISubject
    {
        public string Name { get { return "FileExtensionSubject"; } }
        public LexerType ElementType { get; private set; }

        public LexerType GetVerbType()
        {
            return LexerTypes.String;
        }

        public object GetProperty(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            return String.IsNullOrEmpty(extension) ? "" : extension.Substring(1);
        }
    }
}
