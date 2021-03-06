﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using ImLazy.SDK.Base.Contracts;
using log4net;

namespace ImLazy.Runtime
{
    /// <summary>
    /// Host all addins
    /// </summary>
    public class AddinHost
    {
        private static readonly object LockObj = new object();
        private static readonly ILog Log = LogManager.GetLogger(typeof(AddinHost));

        #region Singleton

        private static readonly AddinHost _instance = new AddinHost();

        public static AddinHost Instance
        {
            get { return _instance; }
        }

        #endregion


        static AddinHost()
        {
            Log.Info("AddinHost initiating...");
            _instance.LoadAddins();
            _instance.BuildCache();
            Log.Info("AddinHost initiated.");
        }

        ~AddinHost()
        {
            Log.Info("AddinHost finalized.");
        }

        public readonly CacheMap<Func<SerializableDictionary<string, object>, IEditView>> ViewCreatorCacheMap = new CacheMap<Func<SerializableDictionary<string, object>, IEditView>>();

        #region Addins
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        [ImportMany]
        public IEnumerable<Lazy<IActionAddin, IActionAddinMetadata>> ActionAddins { get; private set; }

        [ImportMany]
        public IEnumerable<Lazy<IConditionAddin, IConditionAddinMetadata>> ConditionAddins { get; private set; }

        [ImportMany]
        private IEnumerable<Lazy<IAddin, IAddinMetadata>> OtherAddins { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local
        #endregion

        /// <summary>
        /// Load addins from \Addins folder and this (ImLazy.dll) assembly
        /// <para>TS:true</para>
        /// </summary>
        private void LoadAddins()
        {
            lock (LockObj)
            {
                #region Load addins
                Log.Info("Loading addins from '\\Addins' subfolder and self (ImLazy.dll)...");
                var catalog = new AggregateCatalog();
                catalog.Catalogs.Add(new DirectoryCatalog("Addins"));
                catalog.Catalogs.Add(new AssemblyCatalog(typeof (AddinHost).Assembly));
                var container = new CompositionContainer(catalog);
                container.ComposeParts(this);
                #endregion

                #region Print loaded addins to log
                var f = new Func<IEnumerable<IAddinMetadata>, String>(c =>
                {
                    var addinMetadatas = c as IAddinMetadata[] ?? c.ToArray();
                    return !addinMetadatas.Any() ? String.Empty: String.Join("\n\t\t", addinMetadatas.Select(_=>_.Type.Name));
                });
                Log.InfoFormat("Loading finished. Available addins are listed below:\n\t{0} ConditionAddins\n\t\t{1}\n\t{2} ActionAddins\n\t\t{3}\n\t{4} Otherddins\n\t\t{5}",
                    ConditionAddins.Count(),
                    f(ConditionAddins.Select(_ => _.Metadata)),
                    ActionAddins.Count(),
                    f(ActionAddins.Select(_ => _.Metadata)),
                    OtherAddins.Count(),
                    f(OtherAddins.Select(_ => _.Metadata)));
                #endregion
            }
        }

        /// <summary>
        /// Build caches for view creators and addin functions for both conditions and actions.
        /// <para>TS:true</para>
        /// </summary>
        public void BuildCache()
        {
            lock (LockObj)
            {
                #region Cache ViewCreator func to CacheMap<object>

                Log.Info("Building view creators caches...");
                
                ConditionAddins.ForEach(
                    _ => ViewCreatorCacheMap.Put(_.Metadata.Type.FullName, _.Value.CreateMainView));
                ActionAddins.ForEach(
                    _ => ViewCreatorCacheMap.Put(_.Metadata.Type.FullName, _.Value.CreateMainView));
                OtherAddins.ForEach(
                    _ => ViewCreatorCacheMap.Put(_.Metadata.Type.FullName, _.Value.CreateMainView));
                
                #endregion

                #region Cache addins main func to according cache map

                Log.Info("Building functions caches...");

                ConditionAddins.ForEach(
                    _ => Executor.Instance.ConditionCacheMap.Put(_.Metadata.Type.FullName, _.Value.IsMatch));
                ActionAddins.ForEach(
                    _ => Executor.Instance.ActionCacheMap.Put(_.Metadata.Type.FullName, _.Value.DoAction));

                Log.Info("Done caching.");

                #endregion
            }
        }
    }
}
