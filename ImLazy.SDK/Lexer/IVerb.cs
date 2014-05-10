using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImLazy.Contracts;

namespace ImLazy.SDK.Lexer
{
    public interface IVerb : ILexer
    {
        string GetObjectType(string verbType);
        /// <summary>
        /// 是否符合条件
        /// </summary>
        /// <param name="property">由Subject获取的属性</param>
        /// <param name="value">由Object获取的属性</param>
        /// <returns>是否满足</returns>
        bool IsMatch(object property, object value);
    }
}
