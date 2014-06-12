using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.EqualVerb")]
    public class EqualVerb : IVerb
    {
        private static readonly LexerType[] SupportedSubjectTypes = { LexerTypes.String };
        public virtual string Name
        {
            get { return "EqualVerb"; }
        }

        public LexerType GetObjectType(LexerType verbType)
        {
            return verbType;
        }

        public IEnumerable<LexerType> GetSupportedSubjectTypes()
        {
            return SupportedSubjectTypes;
        }

        public virtual bool IsMatch(object subject, object value)
        {
            var strValue = value as string;
            if (strValue == null) return subject.Equals(value);
            var values = strValue.Split('|');
            return values.Any(_ => _.Equals((string)subject));
        }
    }
}
