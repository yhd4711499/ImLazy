namespace ImLazy.SDK.Lexer
{
    public interface ISubject : ILexer
    {
        string GetVerbType();
        object GetProperty(string filePath);
    }
}
