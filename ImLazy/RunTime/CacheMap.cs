using System;
using System.Collections.Generic;
using ImLazy.Contracts;
using ImLazy.Data;

namespace ImLazy.RunTime
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
        #region Singleton

// ReSharper disable StaticFieldInGenericType
        private static readonly CacheMap<Func<string, SerializableDictionary<string, object>, bool>> _conditionCacheMap = new CacheMap<Func<string, SerializableDictionary<string, object>, bool>>();

        /// <summary>
        /// 条件
        /// </summary>
        public static CacheMap<Func<string, SerializableDictionary<string, object>, bool>> ConditionCacheMap
        {
            get { return _conditionCacheMap; }
        }

        private static readonly CacheMap<Action<string, SerializableDictionary<string, object>>> _actionCacheMap = new CacheMap<Action<string, SerializableDictionary<string, object>>>();

        /// <summary>
        /// 动作
        /// </summary>
        public static CacheMap<Action<string, SerializableDictionary<string, object>>> ActionCacheMap
        {
            get { return _actionCacheMap; }
        }

        private static readonly CacheMap<Rule> _ruleCacheMap = new CacheMap<Rule>();

        /// <summary>
        /// 规则
        /// </summary>
        public static CacheMap<Rule> RuleCacheMap
        {
            get { return _ruleCacheMap; }
        }

        private static readonly CacheMap<Func<SerializableDictionary<string, object>, IEditView>> _viewCreatorCacheMap = new CacheMap<Func<SerializableDictionary<string, object>, IEditView>>();

        /// <summary>
        /// 视图创建
        /// </summary>
        public static CacheMap<Func<SerializableDictionary<string, object>, IEditView>> ViewCreatorCacheMap
        {
            get { return _viewCreatorCacheMap; }
        }
// ReSharper restore StaticFieldInGenericType
        #endregion

        public static readonly CacheMap<Func<string,object>> SubjectsCacheMap = new CacheMap<Func<string, object>>();

        public static readonly CacheMap<Func<object,object,bool>> VerbsCacheMap = new CacheMap<Func<object, object, bool>>();
 
        public static readonly CacheMap<Func<string, object>> ObjectsCacheMap = new CacheMap<Func<string, object>>(); 

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