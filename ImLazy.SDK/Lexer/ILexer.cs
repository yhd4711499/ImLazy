using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace ImLazy.SDK.Lexer
{
    [InheritedExport]
    public interface ILexer
    {
        string ElementType { get; }
        string Name { get; }
    }
}
