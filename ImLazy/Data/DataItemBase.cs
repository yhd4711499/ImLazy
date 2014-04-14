using System;
using ImLazy.Util;

namespace ImLazy.Data
{
    /// <summary>
    /// An abstract class which implemented<see cref="IEquatable{DataItem}"/><para/>
    /// All the data classes should be derived from this class.<para/> 
    /// It's designed to provide an easy way to make some fundermental changes in the future.
    /// </summary>
    [Serializable]
    public abstract class DataItemBase : IEquatable<DataItemBase>, ICloneable
    {
        /// <summary>
        /// Equals to "=="
        /// </summary>
        /// <param name="other"></param>
        /// <returns>== or not</returns>
        public bool Equals(DataItemBase other)
        {
            return this == other;
        }

        public object Clone()
        {
            return ObjectCopier.Clone(this);
        }
    }
}
