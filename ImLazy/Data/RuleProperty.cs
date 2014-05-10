using System;
using System.Xml.Serialization;

namespace ImLazy.Data
{
    [Serializable]
    public class RuleProperty: DataItemBase ,IEquatable<RuleProperty>, IComparable<RuleProperty>
    {
        /// <summary>
        /// Item with lower Priority value is actual prior. 
        /// </summary>
        [XmlAttribute]
        public int Priority { get; set; }
        /// <summary>
        /// Unique id
        /// </summary>
        [XmlAttribute]
        public Guid RuleGuid { get; set; }
        [XmlAttribute]
        public bool Enabled { get; set; }
        [Obsolete("Don't use it in code. Use RuleProperty.Create(int) instead.")]
        public RuleProperty()
        {
            
        }

        public static RuleProperty Create(int priority, Guid guid)
        {
// ReSharper disable once CSharpWarnings::CS0618
            return new RuleProperty()
            {
                Enabled = true,
                RuleGuid = guid,
                Priority = priority
            };
        }

        /// <summary>
        /// refers to <see cref="Guid.Equals(Guid)"/>
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true if RuleGUID equals</returns>
        public bool Equals(RuleProperty other)
        {
            return other.RuleGuid.Equals(RuleGuid);
        }

        /// <summary>
        /// Get the hash code of RuleGuid property
        /// </summary>
        /// <returns>The hash code of RuleGuid property</returns>
        public override int GetHashCode()
        {
            return RuleGuid.GetHashCode();
        }

        public int CompareTo(RuleProperty other)
        {
            return Priority.CompareTo(other.Priority);
        }
    }
}
