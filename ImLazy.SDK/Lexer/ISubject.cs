using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImLazy.SDK.Lexer
{
    public interface ISubject : ILexer
    {
        string GetVerbType();
        object GetProperty(string filePath);
    }
}
