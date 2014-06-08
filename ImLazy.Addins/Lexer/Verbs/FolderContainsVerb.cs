using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.FolderContainsVerb")]
    class FolderContainsVerb : IVerb
    {
        private static readonly LexerType[] SupportedSubjectTypes = { LexerTypes.Folder };

        public virtual string Name { get { return "FolderContainsVerb"; } }
        public LexerType GetObjectType(LexerType verbType)
        {
            return LexerTypes.String;
        }

        public LexerType[] GetSupportedSubjectTypes()
        {
            return SupportedSubjectTypes;
        }

        public virtual bool IsMatch(object subject, object value)
        {
            var dirInfo = subject as DirectoryInfo;
            if (dirInfo == null)
            {
                return false;
            }
            var entries = dirInfo.GetFileSystemInfos();
            var strValue = value as string;
            if (strValue == null) throw new NotSupportedException("StringVerbBase only accepts string value!");
            var values = strValue.Split('|');
            return entries.Any(fileSystemInfo => values.Any(_ => fileSystemInfo.Name.Contains(strValue)));
        }
    }
}
