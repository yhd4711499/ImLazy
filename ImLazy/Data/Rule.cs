using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ImLazy.Data
{
    [Serializable]
    public class Rule : DataItemBase, IEquatable<Rule>
    {
        [XmlAttribute]
        public Guid Guid { get; set; }
        [XmlAttribute]
        public String Name { get; set; }
// ReSharper disable MemberCanBePrivate.Global
        public ConditionBranch ConditionBranch { get; set; }
// ReSharper restore MemberCanBePrivate.Global
        public List<ActionAddinInfo> Actions { get; set; }

        /// <summary>
        /// To avoid initiate properties twice in Deserialzation.
        /// </summary>
        /// <returns>A new instance of <see cref="Rule"/></returns>
        public static Rule Create()
        {
            return new Rule
            {
                Guid = Guid.NewGuid(),
                ConditionBranch = new ConditionBranch(),
                Actions = new List<ActionAddinInfo>(),
            };
        }

        public bool Equals(Rule other)
        {
            return Guid.Equals(other.Guid);
        }
    }
}
