namespace ImLazy.SDK.Lexer
{
    public static class LexerTypes
    {
        private const string Prefix = "ImLazy.SDK.Lexer";
        public static readonly LexerType Null = new LexerType(null, "Null", Prefix);
        public static readonly LexerType Object = new LexerType(null, "Object", Prefix);
        public static readonly LexerType List = new LexerType(Object, "List", Prefix);
        public static readonly LexerType String = new LexerType(Object, "String", Prefix);
        public static readonly LexerType FileType = new LexerType(Object, "FileType", Prefix);
        public static readonly LexerType Date = new LexerType(Object, "Date", Prefix);
        public static readonly LexerType TimeSpan = new LexerType(Object, "TimeSpan", Prefix);
        public static readonly LexerType Folder = new LexerType(Object, "Folder", Prefix);
    }
}
