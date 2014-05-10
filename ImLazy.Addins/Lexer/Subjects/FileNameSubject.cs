using System.ComponentModel.Composition;
using System.IO;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Subjects
{
    [Export(typeof(ISubject))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Subjects.FileNameSubject")]
    public class FileNameSubject : ISubject
    {
        public string ElementType { get; private set; }

        public string Name
        {
            get { return "FileNameSubject"; }
        }

        public string GetVerbType()
        {
            return "string";
        }

        public object GetProperty(string filePath)
        {
            return Path.GetFileName(filePath);
        }
    }
}
