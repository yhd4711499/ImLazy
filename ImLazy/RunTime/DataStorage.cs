using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ImLazy.SDK.Base.Contracts;
using ImLazy.Data;
using ImLazy.Util;

namespace ImLazy.RunTime
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
                        _instance = DataCreationUtil.TryCreateFromFile<DataStorage>(FilePath);
                    }
                    if (_instance != null)
                    {
                        _instance.Rules.ForEach(_ => CacheMap<object>.RuleCacheMap.Put(_.Key, _.Value));
                        _instance.Folders.ForEach(f=>f.RuleProperties.Sort());
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
// ReSharper disable MemberCanBePrivate.Global
        public List<ConditionCorp> Conditions { get; set; }

        [XmlIgnore]
        public List<ActionAddinInfo> Actions { get; set; }

        public SerializableDictionary<Guid, Rule> Rules { get; set; }

        public List<Folder> Folders { get; set; }
// ReSharper restore MemberCanBePrivate.Global

        #endregion

        /// <summary>
        /// 保存至文件
        /// </summary>
        public void Save()
        {
            DataCreationUtil.TrySaveToFile(this, FilePath);
            AddinHost.Instance.BuildCache();
        }
    }
}
