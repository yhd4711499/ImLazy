using System;
using System.Xml.Serialization;

namespace ImLazy.Data
{
    [Serializable]
    public class RuleProperty: DataItemBase ,IEquatable<RuleProperty>
    {
        [XmlAttribute]
        public int Priority { get; set; }
        /// <summary>
        /// Unique id
        /// </summary>
        [XmlAttribute]
        public Guid RuleGuid { get; set; }
        [XmlAttribute]
        public bool Enabled { get; set; }

        /// <summary>
        /// refers to <see cref="Guid.Equals(Guid)"/>
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true if RuleGUID equals</returns>
        public bool Equals(RuleProperty other)
        {
            return other.RuleGuid.Equals(RuleGuid);
        }
    }
}
