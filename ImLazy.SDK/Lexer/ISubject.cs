namespace ImLazy.SDK.Lexer
{
    public interface ISubject : ILexer
    {
        LexerType GetVerbType();
        /// <summary>
        /// get property value from filePath
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        object GetProperty(string filePath);
    }
}
