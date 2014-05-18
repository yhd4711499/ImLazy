namespace ImLazy.SDK.Lexer
{
    public interface ISubject : ILexer
    {
        LexerType GetVerbType();
        object GetProperty(string filePath);
    }
}
