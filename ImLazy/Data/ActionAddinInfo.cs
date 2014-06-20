using System;
using System.Collections.Generic;
using ImLazy.Entities;

namespace ImLazy.Data
{
    [Serializable]
    public class ActionAddinInfo : AddinInfo
    {
        public override void Save(ModelContainer container)
        {
            var entity = container.AddinInfoEntitySet.Add(new ActionAddinInfoEntity
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
        }

        protected override AddinInfoEntity GetDerivedEntity()
        {
            return new ActionAddinInfoEntity();
        }
    }
}
