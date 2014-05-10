using System.ComponentModel;

namespace ImLazy.Contracts
{
    /// <summary>
    /// 条件插件的元数据
    /// </summary>
    public interface IConditionAddinMetadata:IAddinMetadata
    {
        /// <summary>
        /// 父条件，默认为空字符串（""）
        /// <para>只读</para>
        /// </summary>
        [DefaultValue("")]
        string Parent { get; }
    }
}
