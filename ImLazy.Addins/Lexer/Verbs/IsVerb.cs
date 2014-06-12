using System.Collections.Generic;
using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.IsVerb")]
    public class IsVerb:IVerb
    {
        private static readonly LexerType[] SupportedSubjectTypes = { LexerTypes.FileType };
        public virtual string Name { get { return "IsVerb"; } }
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
            var actualType = (LexerType) subject;
            var configType = (LexerType) value;
            return actualType.Is(configType);
        }
    }
}
