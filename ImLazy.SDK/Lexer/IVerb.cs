using System.Collections.Generic;

namespace ImLazy.SDK.Lexer
{
    public interface IVerb : ILexer
    {
        LexerType GetObjectType(LexerType verbType);

        IEnumerable<LexerType> GetSupportedSubjectTypes();
        /// <summary>
        /// 是否符合条件
        /// </summary>
        /// <param name="subject">由Subject获取的属性</param>
        /// <param name="value">由Object获取的属性</param>
        /// <returns>是否满足</returns>
        bool IsMatch(object subject, object value);
    }
}
