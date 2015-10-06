using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ImLazy.Addins;
using System.Collections.Generic;
using System.Xml.Serialization;
using ImLazy.Entities;
using ImLazy.Util;
using ImLazy.Runtime;

namespace ImLazy.Data
{
    [XmlInclude(typeof(ConditionLeaf))]
    [Serializable]
    public class ConditionBranch : ConditionCorp
    {
        [Required]
        public ConditionMode Mode { get; set; }

        public ConditionBranch()
        {
            SubConditions = new List<ConditionCorp>();
            AddinType = typeof(ConditionsConfigAddin).FullName;
        }

// ReSharper disable once MemberCanBePrivate.Global
        public List<ConditionCorp> SubConditions { get; set; }

        public override void Save(SerializableDictionary<string, object> config)
        {
            base.Save(config);
            Mode = GetMode(config);
        }

        private static ConditionMode GetMode(Dictionary<string, object> config)
        {
            var modeStr = config.TryGetValue<string>(ConfigNames.Symbol);
            return modeStr != null ? EnumHelper.Parse<ConditionMode>(modeStr) : ConditionMode.All;
        }

        public void Add(ConditionCorp corp)
        {
            SubConditions.Add(corp);
        }

        protected override AddinInfoEntity GetDerivedEntity()
        {
            var branch = new ConditionBranchEntity
            {
                AddinType = AddinType,
                Mode = Mode.ToString()
            };

            foreach (var condition in SubConditions.OfType<ConditionBranch>())
            {
                branch.SubConditions.Add((ConditionBranchEntity)condition.GetEntity());
            }

            foreach (var condition in SubConditions.OfType<ConditionLeaf>())
            {
                branch.SubConditions.Add((ConditionLeafEntity)condition.GetEntity());
            }
            return branch;
        }

        public override void FromEntity(AddinInfoEntity entity, ModelContainer context)
        {
            base.FromEntity(entity, context);
            var cbe = entity as ConditionBranchEntity;
            if (cbe == null)
            {
                throw new NotSupportedException();
            }

            Mode = EnumHelper.Parse<ConditionMode>(cbe.Mode);

            foreach (var s in cbe.SubConditions.OfType<ConditionBranchEntity>())
            {
                var se = new ConditionBranch();
                se.FromEntity(s, context);
                SubConditions.Add(se);
            }
            foreach (var s in cbe.SubConditions.OfType<ConditionLeafEntity>())
            {
                var se = new ConditionLeaf();
                se.FromEntity(s, context);
                SubConditions.Add(se);
            }
        }

        public override void Save(ModelContainer container)
        {
            /*var branch = (ConditionBranchEntity) container.AddinInfoEntitySet.Add(new ConditionBranchEntity
            {
                AddinType = AddinType,
                Mode = Mode.ToString()
            });

            container.SaveChanges();

            foreach (var condition in SubConditions)
            {
                branch.SubConditions.Add((ConditionCorpEntity)condition.GetEntity());
            }

            branch.Configs = Config.ToEntities();

            container.SaveChanges();

            SubConditions.ForEach(_=>_.Save(container));*/
        }

        public int Sum()
        {
            var sum = 1;

            SubConditions.ForEach(_ =>
            {
                sum++;
                var branch = _ as ConditionBranch;
                if (branch != null)
                {
                    sum += branch.Sum();
                }
            });
            return sum;
        }
    }
}
