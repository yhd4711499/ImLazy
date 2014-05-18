using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ImLazy.Data
{
    public class Folder : DataItemBase
    {
        private string _folderPath;

        /// <summary>
        /// An <see cref="List{RuleProperty}"/> to store properties of rules.
        /// </summary>
// ReSharper disable once MemberCanBePrivate.Global
        public List<RuleProperty> RuleProperties { get; set; }

        [XmlAttribute]
        public String FolderPath
        {
            get { return _folderPath; }
            set
            {
                _folderPath = value;
                var splits = _folderPath.Split('\\');
                _displayPath = splits.Length > 2 ? splits.Last() : _folderPath;
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
                RuleProperties = new List<RuleProperty>()
            };
        }

        private string _displayPath;
        /// <summary>
        /// 
        /// </summary>
        /// <returns><see cref="FolderPath"/></returns>
        public override string ToString()
        {
            return _displayPath;
        }
    }
}
