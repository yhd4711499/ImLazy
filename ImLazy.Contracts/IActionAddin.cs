using System.Collections.Generic;

namespace ImLazy.Contracts
{
    /// <summary>
    /// 动作插件，用于对路径执行动作
    /// </summary>
    public interface IActionAddin : IAddin
    {
        /// <summary>
        /// 执行动作
        /// </summary>
        /// <param name="filePath">路径（目录或文件）</param>
        /// <param name="dic">设置</param>
        void DoAction(string filePath, SerializableDictionary<string, object> dic);
    }
}
