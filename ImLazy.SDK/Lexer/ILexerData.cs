using System.ComponentModel.Composition;

namespace ImLazy.SDK.Lexer
{
    [InheritedExport]
    public interface ILexerData
    {
        string Name { get; }
    }
}
