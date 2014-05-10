using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ImLazy.Contracts;

namespace ImLazy.SDK.Lexer
{
    public interface IObject : ILexer
    {
        object GetObject(string value);

        /// <summary>
        /// 获取插件的视图
        /// </summary>
        IEditView CreateMainView(SerializableDictionary<string, object> config);
    }
}
