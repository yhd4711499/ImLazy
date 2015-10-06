using System.Collections.Generic;

namespace ImLazy.Data
{
    /// <summary>
    /// 含有子节点的节点
    /// </summary>
    public interface IBranch : ICorp
    {
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="corp"></param>
        void Add(ICorp corp);
        /// <summary>
        /// 获取子节点
        /// </summary>
        List<ICorp> SubConditions { get; set; }
    }
}