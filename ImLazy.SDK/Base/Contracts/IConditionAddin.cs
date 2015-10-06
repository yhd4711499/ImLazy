using System.Collections.Generic;

namespace ImLazy.SDK.Base.Contracts
{
    /// <summary>
    /// 条件插件，用于判断路径是否符合条件
    /// </summary>
    public interface IConditionAddin:IAddin
    {
        /// <summary>
        /// 是否符合条件
        /// </summary>
        /// <param name="filePath">路径（目录或文件）</param>
        /// <param name="dic">设置</param>
        /// <returns>是否满足</returns>
        bool IsMatch(string filePath, SerializableDictionary<string, object> dic);
    }
}
