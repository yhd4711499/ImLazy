using System;
using System.Collections.Generic;
using System.Linq;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    public abstract class StringVerbBase:IVerb
    {
        private static readonly LexerType[] SupportedSubjectTypes = { LexerTypes.String };

        public abstract string Name { get; }

        public LexerType GetObjectType(LexerType verbType)
        {
            return verbType;
        }

        public IEnumerable<LexerType> GetSupportedSubjectTypes()
        {
            return SupportedSubjectTypes;
        }

        public bool IsMatch(object subject, object value)
        {
            var strValue = value as string;
            if (strValue == null) throw new NotSupportedException("StringVerbBase only accepts string value!");
            var values = strValue.Split('|');
            return values.Any(_ => GetResult((string)subject, _));
        }

        protected abstract bool GetResult(string a, string b);
    }
}
