namespace ImLazy.SDK.Lexer
{
    public static class LexerTypes
    {
        public static readonly LexerType ObjectType = new LexerType(null, "Object", "ImLazy.SDK.Lexer");
        public static readonly LexerType StringType = new LexerType(ObjectType, "String", "ImLazy.SDK.Lexer");
        public static readonly LexerType TypeType = new LexerType(ObjectType, "Type", "ImLazy.SDK.Lexer");
        public static readonly LexerType DateType = new LexerType(ObjectType, "DateType", "ImLazy.SDK.Lexer");
        public static readonly LexerType TimeSpanType = new LexerType(DateType, "TimeSpanType", "ImLazy.SDK.Lexer");
    }
}
