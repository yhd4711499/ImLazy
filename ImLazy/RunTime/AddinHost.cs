using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using ImLazy.Contracts;
using log4net;

namespace ImLazy.RunTime
{
    /// <summary>
    /// Host all addins
    /// </summary>
    public class AddinHost
    {
        private static readonly object lockObj = new object();
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
            Log.Info("AddinHost loaded.");
            LoadAllAddins();
        }

        ~AddinHost()
        {
            Log.Info("AddinHost finalized.");
        }



        #region Addins

        [ImportMany]
        public IEnumerable<Lazy<IActionAddin, IActionAddinMetadata>> ActionAddins { get; private set; }

        [ImportMany]
        public IEnumerable<Lazy<IConditionAddin, IConditionAddinMetadata>> ConditionAddins { get; private set; }

        [ImportMany]
        private IEnumerable<Lazy<IAddin, IAddinMetadata>> OtherAddins { get; set; }

        #endregion

        static void LoadAllAddins()
        {
            lock (lockObj)
            {
            
                #region Load addins

                // Load addins from \Addins folder and this (ImLazy.dll) assembly
                Log.Info("Loading addins from '\\Addins' subfolder ...");
                var catalog = new AggregateCatalog();
                catalog.Catalogs.Add(new DirectoryCatalog("Addins"));
                catalog.Catalogs.Add(new AssemblyCatalog(typeof (AddinHost).Assembly));
                var container = new CompositionContainer(catalog);
                container.ComposeParts(_instance);

                #endregion


                #region Print loaded addins to log
                var f = new Func<IEnumerable<IAddinMetadata>, String>(c =>
                    {
                        var addinMetadatas = c as IAddinMetadata[] ?? c.ToArray();
                        if (addinMetadatas.Count() != 0)
                        {
                            var sb = new StringBuilder("");
                            foreach (var item in addinMetadatas)
                            {
                                sb.Append("\t\t");
                                sb.AppendLine(item.Type.Name);
                            }
                            return sb.ToString();
                        }
                        return String.Empty;
                    });
                Log.InfoFormat("Load addins finished.\n\tConditionAddins has {0}\n{1}\tActionAddins has {2}\n{3}\tOtherddins has {4}\n{5}",
                    _instance.ConditionAddins.Count(),
                    f(_instance.ConditionAddins.Select(_=>_.Metadata)),
                    _instance.ActionAddins.Count(),
                    f(_instance.ActionAddins.Select(_=>_.Metadata)),
                    _instance.OtherAddins.Count(),
                    f(_instance.OtherAddins.Select(_ => _.Metadata)));
                #endregion


                _instance.BuildCache();
            }
        }

        public void BuildCache()
        {
            #region Cache ViewCreator func to CacheMap<object>

            Log.Debug("Building cache...");

            ConditionAddins.ForEach(
                _ => CacheMap<object>.ViewCreatorCacheMap.Put(_.Metadata.Type.FullName, _.Value.CreateMainView));
            ActionAddins.ForEach(
                _ => CacheMap<object>.ViewCreatorCacheMap.Put(_.Metadata.Type.FullName, _.Value.CreateMainView));
            OtherAddins.ForEach(
                _ => CacheMap<object>.ViewCreatorCacheMap.Put(_.Metadata.Type.FullName, _.Value.CreateMainView));

            #endregion

            #region Cache addins main func to according cache map

            ConditionAddins.ForEach(
                _ => CacheMap<object>.ConditionCacheMap.Put(_.Metadata.Type.FullName, _.Value.IsMatch));
            ActionAddins.ForEach(
                _ => CacheMap<object>.ActionCacheMap.Put(_.Metadata.Type.FullName, _.Value.DoAction));

            Log.Debug("Done caching.");
            #endregion
        }
    }
}
