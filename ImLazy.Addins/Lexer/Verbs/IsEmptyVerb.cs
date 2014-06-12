using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.IsEmptyVerb")]
    class IsEmptyVerb : IVerb
    {
        private static readonly LexerType[] SupportedSubjectTypes = { LexerTypes.List };

        public virtual string Name
        {
            get { return "IsEmptyVerb"; }
        }

        public LexerType GetObjectType(LexerType verbType)
        {
            return LexerTypes.Null;
        }

        public IEnumerable<LexerType> GetSupportedSubjectTypes()
        {
            return SupportedSubjectTypes;
        }

        public virtual bool IsMatch(object subject, object value)
        {
            var list = subject as IEnumerable;
            if (list == null) return false;
            return list.Sum() == 1;
        }
    }
}
