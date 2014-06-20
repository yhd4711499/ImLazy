using System;
using System.Data.Entity;
using ImLazy.Entities;
using ImLazy.Util;

namespace ImLazy.Data
{
    /// <summary>
    /// An abstract class which implemented<see cref="IEquatable{DataItem}"/><para/>
    /// All the data classes should be derived from this class.<para/> 
    /// It's designed to provide an easy way to make some fundermental changes in the future.
    /// </summary>
    [Serializable]
    public abstract class DataItemBase<T> : IEquatable<DataItemBase<T>>, ICloneable
    {
        /// <summary>
        /// Equals to "=="
        /// </summary>
        /// <param name="other"></param>
        /// <returns>== or not</returns>
        public bool Equals(DataItemBase<T> other)
        {
            return this == other;
        }

        public object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        public abstract T GetEntity();

        public abstract void Save(ModelContainer container);
        public abstract void FromEntity(T entity, ModelContainer context);
    }
}
