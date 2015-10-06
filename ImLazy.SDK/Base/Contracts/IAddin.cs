using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ImLazy.SDK.Base.Contracts
{
    /// <summary>
    /// 插件的基本接口
    /// </summary>
    [InheritedExport]
    public interface IAddin
    {
        /// <summary>
        /// 获取插件的视图
        /// </summary>
        IEditView CreateMainView(SerializableDictionary<string, object> config);
        string LocalName { get; }
    }
}
