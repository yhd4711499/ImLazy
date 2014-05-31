using System;
using System.Collections.Generic;
using ImLazy.SDK.Base.Contracts;

namespace ImLazy.Runtime
{
    /// <summary>
    /// 缓存，使用
    /// <see>
    ///     <cref>CacheMap{object}</cref>
    /// </see>
    ///     来获取静态属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CacheMap<T> where T:class 
    {
        private readonly Dictionary<string, T> _cache = new Dictionary<string, T>();
        /// <summary>
        /// 放入缓存，将会替换已有的同名缓存
        /// </summary>
        /// <param name="type"><see cref="IAddin"/>的类型</param>
        /// <param name="matchObj">方法的委托</param>
        public void Put(string type, T matchObj)
        {
            _cache[type] = matchObj;
        }

        /// <summary>
        /// 放入缓存，将会替换已有的同名缓存
        /// </summary>
        /// <param name="guid">GUID</param>
        /// <param name="obj">对象</param>
        public void Put(Guid guid, T obj)
        {
            _cache[guid.ToString()] = obj;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="type"><see cref="IAddin"/>的类型</param>
        /// <returns>方法的委托</returns>
        public T Get(string type)
        {
            T result;
            return _cache.TryGetValue(type, out result) ? result : null;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="guid">Guid</param>
        /// <returns>对象</returns>
        public T Get(Guid guid)
        {
            T result;
            return _cache.TryGetValue(guid.ToString(), out result) ? result : null;
        }

        public void Remove(Guid guid)
        {
            _cache.Remove(guid.ToString());
        }
    }
}