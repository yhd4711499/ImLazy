using System.Collections.Generic;
using IEditView = ImLazy.SDK.Base.Contracts.IEditView;

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
