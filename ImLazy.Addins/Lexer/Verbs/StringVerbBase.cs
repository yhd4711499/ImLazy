using System;
using System.Linq;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    public abstract class StringVerbBase:IVerb
    {
        public abstract string Name { get; }

        public LexerType ElementType
        {
            get { return LexerTypes.StringType; }
        }

        public LexerType GetObjectType(LexerType verbType)
        {
            return verbType;
        }

        public bool IsMatch(object property, object value)
        {
            var strValue = value as string;
            if (strValue == null) throw new NotSupportedException("StringVerbBase only accepts string value!");
            var values = strValue.Split('|');
            return values.Any(_ => GetResult((string)property, _));
        }

        protected abstract bool GetResult(string a, string b);
    }
}
