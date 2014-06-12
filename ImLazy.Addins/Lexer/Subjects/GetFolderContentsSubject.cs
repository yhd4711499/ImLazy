using System.ComponentModel.Composition;
using System.IO;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Subjects
{
    [Export(typeof(ISubject))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Subjects.GetFolderContentsSubject")]
    class GetFolderContentsSubject:ISubject
    {
        public string Name
        {
            get { return "GetFolderContentsSubject"; }
        }

        public LexerType GetVerbType()
        {
            return LexerTypes.List;
        }

        public object GetProperty(string filePath)
        {
            var dinfo = new DirectoryInfo(filePath);
            return !dinfo.Exists ? null : dinfo.GetFileSystemInfos();
        }
    }
}
