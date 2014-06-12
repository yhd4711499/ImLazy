using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using ImLazy.SDK.Exceptions;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.ContainsFileInfoVerb")]
    class ContainsFileInfoVerb : IVerb
    {
        private static readonly LexerType[] SupportedSubjectTypes = { LexerTypes.List };

        public virtual string Name { get { return "ContainsFileInfoVerb"; } }
        public LexerType GetObjectType(LexerType verbType)
        {
            return LexerTypes.String;
        }

        public IEnumerable<LexerType> GetSupportedSubjectTypes()
        {
            return SupportedSubjectTypes;
        }

        public virtual bool IsMatch(object subject, object value)
        {
            /*var dirInfo = subject as DirectoryInfo;
            if (dirInfo == null)
            {
                return false;
            }
            var entries = dirInfo.GetFileSystemInfos();*/
            var entries = subject as IEnumerable<FileSystemInfo>;
            if (entries == null)
            {
                throw new CheckedException(ErrorCodeDefinitions.ErrUnknown);
            }
            var strValue = value as string;
            if (strValue == null) throw new NotSupportedException("StringVerbBase only accepts string value!");
            var values = strValue.Split('|');
            return entries.Any(fileSystemInfo => values.Any(_ => fileSystemInfo.Name.Contains(strValue)));
        }
    }
}
