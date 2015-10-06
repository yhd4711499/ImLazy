using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ImLazy.Entities;

namespace ImLazy.Data
{
    public class Folder : DataItemBase<FolderEntity>
    {
        private string _folderPath;

        /// <summary>
        /// An <see cref="List{RuleProperty}"/> to store properties of rules.
        /// </summary>
// ReSharper disable once MemberCanBePrivate.Global
        public List<RuleProperty> RuleProperties { get; set; }

        [XmlAttribute]
        public bool Enabled { get; set; }

        [XmlAttribute]
        public String FolderPath
        {
            get { return _folderPath; }
            set
            {
                _folderPath = value;
            }
        }

        /// <summary>
        /// Use this static method to create an instance of <see cref="Folder"/>
        /// </summary>
        /// <returns></returns>
        public static Folder Create()
        {
            return new Folder
            {
                RuleProperties = new List<RuleProperty>(),
                Enabled = true
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns><see cref="FolderPath"/></returns>
        public override string ToString()
        {
            return FolderPath;
        }

        public override FolderEntity GetEntity()
        {
            var entity = new FolderEntity
            {
                Enabled = Enabled,
                FolderPath = FolderPath,
            };
            RuleProperties.ForEach(rp => entity.RuleProperties.Add(rp.GetEntity()));
            return entity;
        }

        public override void Save(ModelContainer container)
        {
            /*container.FolderEntitySet.Add(GetEntity());
            container.SaveChanges();*/
        }

        public override void FromEntity(FolderEntity entity, ModelContainer context)
        {
            Enabled = entity.Enabled;
            FolderPath = entity.FolderPath;

            var properties = entity.RuleProperties.Select(_ =>
            {
                var rp = new RuleProperty();
                rp.FromEntity(_, context);
                return rp;
            });
            RuleProperties = new List<RuleProperty>();
            RuleProperties.AddRange(properties);
        }
    }
}
