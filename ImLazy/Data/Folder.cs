using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ImLazy.Data
{
    public class Folder : DataItemBase
    {
        /// <summary>
        /// An <see cref="List{RuleProperty}"/> to store properties of rules.
        /// </summary>
// ReSharper disable once MemberCanBePrivate.Global
        public List<RuleProperty> RuleProperties { get; set; }
        [XmlAttribute]
        public String FolderPath { get; set; }

        /// <summary>
        /// Use this static method to create an instance of <see cref="Folder"/>
        /// </summary>
        /// <returns></returns>
        public static Folder Create()
        {
            return new Folder
            {
                RuleProperties = new List<RuleProperty>()
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
    }
}
