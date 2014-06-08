namespace ImLazy.SDK.Lexer
{
    public static class LexerTypes
    {
        public static readonly LexerType Object = new LexerType(null, "Object", "ImLazy.SDK.Lexer");
        public static readonly LexerType String = new LexerType(Object, "String", "ImLazy.SDK.Lexer");
        public static readonly LexerType FileType = new LexerType(Object, "FileType", "ImLazy.SDK.Lexer");
        public static readonly LexerType Date = new LexerType(Object, "Date", "ImLazy.SDK.Lexer");
        public static readonly LexerType TimeSpan = new LexerType(Object, "TimeSpan", "ImLazy.SDK.Lexer");
        public static readonly LexerType Folder = new LexerType(Object, "Folder", "ImLazy.SDK.Lexer");
    }
}
