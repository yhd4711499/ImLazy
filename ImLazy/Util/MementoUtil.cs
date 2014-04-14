using System;

namespace ImLazy.Util
{
    public static class MementoUtil
    {
        /// <summary>
        /// 获取Memento对象
        /// </summary>
        /// <typeparam name="T">memento的类型</typeparam>
        /// <param name="mementoObj">memento的Object形式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">如果mementoObj与类型不符</exception>
        public static T GetMemento<T>(object mementoObj) where T:class
        {
            var memento = mementoObj as T;
            if (memento == null)
            {
                throw new ArgumentException(String.Format("The type of should be {0}",
                    typeof(T)));
            }
            return memento;
        }
    }
}