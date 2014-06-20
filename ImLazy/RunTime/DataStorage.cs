using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ImLazy.SDK.Base.Contracts;
using ImLazy.Data;
using ImLazy.Util;
using Rule = ImLazy.Data.Rule;

namespace ImLazy.Runtime
{
    /// <summary>
    /// 在运行时储存所有条件和动作
    /// </summary>
    public class DataStorage
    {
        private static readonly object LockObj = new object();
        //private static readonly ILog Log = LogManager.GetLogger(typeof (DataStorage));
        private static readonly String FilePath = Path.Combine(AppEnvironment.LocalStorageFolder, "data.xml");

        #region Singleton
        /// <summary>
        /// 获取实例，如果是第一次获取，将会从文件中读取并创建。
        /// </summary>
        public static DataStorage Instance
        {
            get
            {
                lock (LockObj)
                {
                    if (_instance == null)
                    {
                        try
                        {
                            _instance = DataCreationUtil.TryCreateFromFile<DataStorage>(FilePath);
                            /*_instance = Create();
                            var container = new ModelContainer();
                            foreach (var ruleEntity in container.RuleEntitySet)
                            {
                                var rule = DataCreationUtil.FromEntity<Rule, RuleEntity>(ruleEntity);
                                _instance.Rules[rule.Guid] = rule;
                            }
                            foreach (var folderEntity in container.FolderEntitySet)
                            {
                                _instance.Folders.Add(DataCreationUtil.FromEntity<Folder, FolderEntity>(folderEntity));
                            }*/
                            
                            
                            /*_instance = DataCreationUtil.TryCreateFromFile<DataStorage>(FilePath);
                            var container = new ModelContainer();
                            _instance.Rules.ForEach(_ => _.Value.Save(container));
                            _instance.Folders.ForEach(_ => _.Save(container));
                            container.SaveChanges();*/
                        }
                        catch (DbEntityValidationException e)
                        {
                            Debug.WriteLine(e.StackTrace);
                        }
                        
                    }
                    if (_instance != null)
                    {
                        _instance.Rules.ForEach(_ => Executor.Instance.RuleCacheMap.Put(_.Key, _.Value));
                    }
                    else
                        _instance = Create();
                }
                return _instance;
            }
        }

        private static DataStorage _instance;

        /// <summary>
        /// Construtor only for Xml serialization
        /// </summary>
        [Obsolete("Construtor only for Xml serialization or in Create() method. Don't use it programmly!")]
        private DataStorage()
        {
            Conditions = new List<ConditionCorp>();
            Actions = new List<ActionAddinInfo>();
            Conditions.AddRange(AddinHost.Instance.ConditionAddins
                .Where(_ => String.IsNullOrEmpty(_.Metadata.Parent))    // only add root condition
                .Select(DataCreationUtil.FromLazy<ConditionCorp, IConditionAddin, IConditionAddinMetadata>));   // create from lazy<T,TMetadata>
            Actions.AddRange(AddinHost.Instance.ActionAddins
                .Select(DataCreationUtil.FromLazy<ActionAddinInfo, IActionAddin, IActionAddinMetadata>));
        }

        /// <summary>
        /// Create instance programmly
        /// </summary>
        /// <returns></returns>
        private static DataStorage Create()
        {
#pragma warning disable 618
            var store = new DataStorage
#pragma warning restore 618
            {
                Folders = new List<Folder>(),
                Rules = new SerializableDictionary<Guid, Rule>()
            };
            return store;
        }
        #endregion

        #region Data

        [XmlIgnore]
        [NotMapped]
        public List<ConditionCorp> Conditions { get; set; }

        [XmlIgnore]
        [NotMapped]
        public List<ActionAddinInfo> Actions { get; set; }

        public SerializableDictionary<Guid, Rule> Rules { get; set; }

        public List<Folder> Folders { get; set; }

        #endregion

        /// <summary>
        /// 保存至文件
        /// </summary>
        public void Save()
        {
            DataCreationUtil.TrySaveToFile(this, FilePath);
            AddinHost.Instance.BuildCache();
            LexerAddinHost.Instance.BuildCache();
        }
    }
}
