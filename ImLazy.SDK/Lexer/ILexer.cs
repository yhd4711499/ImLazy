using System.ComponentModel.Composition;

namespace ImLazy.SDK.Lexer
{
    [InheritedExport]
    public interface ILexer
    {
        string ElementType { get; }
        string Name { get; }
    }
}
