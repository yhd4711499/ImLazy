using System;
using System.ComponentModel.Composition;

namespace ImLazy.SDK.Base.Contracts
{
    /// <summary>
    /// 插件元数据
    /// </summary>
    [InheritedExport]
    public interface IAddinMetadata
    {
        /// <summary>
        /// 插件的类型
        /// </summary>
        Type Type { get; }
    }
}
