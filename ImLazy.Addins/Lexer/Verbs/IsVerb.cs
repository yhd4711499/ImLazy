using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.IsVerb")]
    public class IsVerb:IVerb
    {
        public string Name { get { return "IsVerb"; } }
        public LexerType ElementType { get { return LexerTypes.TypeType; } }
        public LexerType GetObjectType(LexerType verbType)
        {
            return verbType;
        }

        public bool IsMatch(object property, object value)
        {
            var actualType = (LexerType) property;
            var configType = (LexerType) value;
            return actualType.Is(configType);
        }
    }
}
