using System;
using System.Collections.Generic;
using ImLazy.Entities;

namespace ImLazy.Data
{
    [Serializable]
    public class ConditionCorp : AddinInfo
    {
        // TODO: This property would result in Xml serialzation loop.
        //public ConditionCorp Parent { get; set; }
        public override void Save(ModelContainer container)
        {
            var entity = container.AddinInfoEntitySet.Add(new ConditionCorpEntity
            {
                AddinType = AddinType,
            });
            container.SaveChanges();

            Config.ForEach(_ => entity.Configs.Add(new ConfigEntity
            {
                Key = _.Key,
                Value = _.Value == null ? null : _.Value.ToString()
            }));
            container.SaveChanges();
            //base.Save(container, parentId);
        }

        protected override AddinInfoEntity GetDerivedEntity()
        {
            return new ConditionCorpEntity();
        }
    }
}
