using System.ComponentModel.Composition;
using System.Linq;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.EqualVerb")]
    public class EqualVerb : IVerb
    {
        public string Name
        {
            get { return "EqualVerb"; }
        }

        public LexerType ElementType
        {
            get { return LexerTypes.ObjectType; }
        }

        public LexerType GetObjectType(LexerType verbType)
        {
            return verbType;
        }

        public bool IsMatch(object property, object value)
        {
            var strValue = value as string;
            if (strValue == null) return property.Equals(value);
            var values = strValue.Split('|');
            return values.Any(_ => _.Equals((string)property));
        }
    }
}
