using System.Collections.Generic;

namespace ImLazy.SDK.Base.Contracts
{
    /// <summary>
    /// 提供设置接口的视图
    /// </summary>
    public interface IEditView
    {
        //IAddin Addin { get; }
        /// <summary>
        /// 读取或写入设置
        /// </summary>
        SerializableDictionary<string, object> Configuration { set; get; }
    }
}
