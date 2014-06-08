using System.ComponentModel.Composition;
using System.IO;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Subjects
{
    [Export(typeof(ISubject))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Subjects.GetFolderSubject")]
    class GetFolderSubject:ISubject
    {
        public string Name
        {
            get { return "GetFolderSubject"; }
        }

        public LexerType GetVerbType()
        {
            return LexerTypes.Folder;
        }

        public object GetProperty(string filePath)
        {
            return new DirectoryInfo(filePath);
        }
    }
}
