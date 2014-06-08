using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.IsVerb")]
    public class IsVerb:IVerb
    {
        private static readonly LexerType[] SupportedSubjectTypes = { LexerTypes.FileType };
        public string Name { get { return "IsVerb"; } }
        public LexerType GetObjectType(LexerType verbType)
        {
            return verbType;
        }

        public LexerType[] GetSupportedSubjectTypes()
        {
            return SupportedSubjectTypes;
        }

        public bool IsMatch(object subject, object value)
        {
            var actualType = (LexerType) subject;
            var configType = (LexerType) value;
            return actualType.Is(configType);
        }
    }
}
