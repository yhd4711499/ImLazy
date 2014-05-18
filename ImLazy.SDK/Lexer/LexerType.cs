using System;
using System.Collections.Generic;

namespace ImLazy.SDK.Lexer
{
    public class LexerType : IEquatable<LexerType>
    {
        private static readonly Dictionary<string, LexerType> Cache = new Dictionary<string, LexerType>(); 
        public string Name { get; private set; }
        public string CanonicalName { get; private set; }

        public static LexerType FromFullName(string fullName)
        {
            return Cache[fullName];
        }

        public LexerType(LexerType deviredType, string name, string canonicalNamePrefix)
        {
            CanonicalName = String.Format("{0}.{1}", canonicalNamePrefix, name);
            Name = name;
            DeviredType = deviredType;
            Cache[CanonicalName] = this;
        }

        private LexerType DeviredType { get; set; }
        public bool Is(LexerType type)
        {
            if (type.Equals(this))
                return true;
            if (DeviredType != null)
                return DeviredType.Equals(type);
            return false;
        }

        public bool Equals(LexerType other)
        {
            return CanonicalName.Equals(other.CanonicalName);
        }
    }
}