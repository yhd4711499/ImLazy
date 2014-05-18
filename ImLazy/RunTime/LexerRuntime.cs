using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using ImLazy.SDK.Lexer;
using log4net;

namespace ImLazy.RunTime
{
    public class LexerRuntime
    {
        private static readonly object LockObj = new object();
        private static readonly ILog Log = LogManager.GetLogger(typeof(LexerRuntime));

        #region Singleton

        private static readonly LexerRuntime _instance = new LexerRuntime();

        public static LexerRuntime Instance
        {
            get { return _instance; }
        }

        #endregion

        #region Addins
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        [ImportMany]
        public IEnumerable<Lazy<ISubject, ILexerData>> Subjects { get; set; }

        [ImportMany]

        public IEnumerable<Lazy<IVerb, ILexerData>> Verbs { get; set; }

        [ImportMany]
        public IEnumerable<Lazy<IObject, ILexerData>> Objects { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local
        #endregion

        static LexerRuntime()
        {
            Log.Info("LexerRuntime initiating...");
            _instance.LoadAddins();
            _instance.BuildCache();
            Log.Info("LexerRuntime initiated.");
        }

        ~LexerRuntime()
        {
            Log.Info("LexerRuntime finalized.");
        }

        /// <summary>
        /// Load addins from \Addins folder and this (ImLazy.dll) assembly
        /// <para>TS:true</para>
        /// </summary>
        private void LoadAddins()
        {
            lock (LockObj)
            {
                #region Load addins
                Log.Info("Loading addins from '\\Addins' subfolder and self (AddinHost.dll)...");
                var catalog = new AggregateCatalog();
                catalog.Catalogs.Add(new DirectoryCatalog("Addins"));
                catalog.Catalogs.Add(new AssemblyCatalog(GetType().Assembly));
                var container = new CompositionContainer(catalog);
                container.ComposeParts(this);
                #endregion

                #region Print loaded addins to log
                var f = new Func<IEnumerable<ILexer>, String>(c =>
                {
                    var addinMetadatas = c as ILexer[] ?? c.ToArray();
                    return !addinMetadatas.Any() ? String.Empty : String.Join("\n\t\t", addinMetadatas.Select(_ => _.GetType().FullName));
                });
                Log.InfoFormat("Load addins finished.\n\tSubjects has {0}\n\t\t{1}\n\tVerbs has {2}\n\t\t{3}\n\tObjects has {4}\n\t\t{5}",
                    Subjects.Count(),
                    f(Subjects.Select(_=>_.Value)),
                    Verbs.Count(),
                    f(Verbs.Select(_ => _.Value)),
                    Objects.Count(),
                    f(Objects.Select(_ => _.Value)));
                #endregion
            }
        }

        public IEnumerable<Lazy<IVerb, ILexerData>> GetSupportedVerbsByType(LexerType nextType)
        {
            return Verbs.Where(_ => nextType.Is(_.Value.ElementType));
        }

        public IEnumerable<Lazy<IObject, ILexerData>> GetSupportedObjectsByVerbType(LexerType verbType)
        {
            return Objects.Where(_ => verbType.Is(_.Value.ElementType));
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

                Log.Info("Building view creators, subjects and verbs caches...");

                Objects.ForEach(
                    _ =>
                    {
                        CacheMap<object>.ViewCreatorCacheMap.Put(_.Value.GetType().FullName, _.Value.CreateMainView);
                        CacheMap<object>.ObjectsCacheMap.Put(_.Value.GetType().FullName, _.Value.GetObject);
                    });
                Subjects.ForEach(
                    _ => CacheMap<object>.SubjectsCacheMap.Put(_.Value.GetType().FullName, _.Value.GetProperty));
                Verbs.ForEach(
                    _ => CacheMap<object>.VerbsCacheMap.Put(_.Value.GetType().FullName, _.Value.IsMatch));

                Log.Info("Done caching.");

                #endregion

            }
        }
    }
}
