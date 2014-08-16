using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ImLazy.Entities;
using ImLazy.Util;

namespace ImLazy.Data
{
    [Serializable]
    public class Rule : DataItemBase<RuleEntity>, IEquatable<Rule>
    {
        [XmlAttribute]
        public Guid Guid { get; set; }
        /*[XmlIgnore]
        public int Id { get; private set; }*/
        [XmlAttribute]
        public String Name { get; set; }

// ReSharper disable MemberCanBePrivate.Global
        public ConditionBranch ConditionBranch { get; set; }
// ReSharper restore MemberCanBePrivate.Global
// ReSharper disable once MemberCanBePrivate.Global
        public List<ActionAddinInfo> Actions { get; set; }

        /// <summary>
        /// To avoid initiate properties twice in Deserialzation.
        /// </summary>
        /// <returns>A new instance of <see cref="Rule"/></returns>
        public static Rule Create()
        {
            var r = new Rule
            {
                Guid = Guid.NewGuid(),
                ConditionBranch = new ConditionBranch(),
                Actions = new List<ActionAddinInfo>(),
            };
            r.ConditionBranch.Add(new ConditionLeaf());
            r.Actions.Add(new ActionAddinInfo());
            return r;
        }

        public bool Equals(Rule other)
        {
            return Guid.Equals(other.Guid);
        }

        public override RuleEntity GetEntity()
        {
            var rule = new RuleEntity
            {
                Id = Guid,
                Name = Name
            };
            Actions.ForEach(ac => rule.Actions.Add((ActionAddinInfoEntity)ac.GetEntity()));

            rule.ConditionBranch = (ConditionBranchEntity)ConditionBranch.GetEntity();
            return rule;
        }

        public override void Save(ModelContainer container)
        {
            container.RuleEntitySet.Add(GetEntity());
        }

        public override void FromEntity(RuleEntity entity, ModelContainer context)
        {
            Name = entity.Name;
            Guid = entity.Id;
            var actionEntities = entity.Actions;
            var addinInfos = actionEntities.Select(_ =>
            {
                var addinInfo = new ActionAddinInfo();
                addinInfo.FromEntity(_, context);
                return addinInfo;
            });
            Actions = new List<ActionAddinInfo>();
            Actions.AddRange(addinInfos);

            ConditionBranch = new ConditionBranch();
            ConditionBranch.FromEntity(entity.ConditionBranch, context);
        }
    }
}
