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
                    return !addinMetadatas.Any() ? String.Empty : String.Join("\n\t\t", addinMetadatas.Select(_ => _.Name));
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

        public IEnumerable<Lazy<IVerb, ILexerData>> GetSupportedVerbs(string name)
        {
            var firstOrDefault = Subjects.FirstOrDefault(_ => _.Value.Name.Equals(name));
            if (firstOrDefault == null) return null;
            var nextType = firstOrDefault.Value.GetVerbType();
            if (nextType.Equals("none"))
                return null;
            var items = nextType.Equals("object")
                ? Verbs
                : Verbs.Where(_ => _.Value.ElementType.Equals("object") || _.Value.ElementType.Equals(nextType));
            return items;
        }

        public IEnumerable<Lazy<IVerb, ILexerData>> GetSupportedVerbsByType(string nextType)
        {
            if (nextType.Equals("none"))
                return null;
            var items = nextType.Equals("object")
                ? Verbs
                : Verbs.Where(_ => _.Value.ElementType.Equals("object") || _.Value.ElementType.Equals(nextType));
            return items;
        }

        public IEnumerable<Lazy<IObject, ILexerData>> GetSupportedObjectsByVerbType(string verbType)
        {
            var items = verbType.Equals("object")
                ? Objects
                : Objects.Where(
                    _ => _.Value.ElementType.Equals("object") || _.Value.ElementType.Equals(verbType));
            return items;
        }

        public IEnumerable<Lazy<IObject, ILexerData>> GetSupportedObjects(string subjectName, string verbName)
        {
            var firstOrDefault = Verbs.FirstOrDefault(_ => _.Value.Name.Equals(verbName));
            var fodSubject = Subjects.FirstOrDefault(_ => _.Value.Name.Equals(subjectName));

            if (firstOrDefault == null) return null;

            var nextType = firstOrDefault.Value.GetObjectType(fodSubject == null ? null : fodSubject.Value.GetVerbType());

            var items = nextType.Equals("object")
                ? Objects
                : Objects.Where(
                    _ => _.Value.ElementType.Equals("object") || _.Value.ElementType.Equals(nextType));
            return items;
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

                #endregion

                #region Cache addins main func to according cache map

                /*Log.Info("Building functions caches...");

                ConditionAddins.ForEach(
                    _ => CacheMap<object>.ConditionCacheMap.Put(_.Metadata.Type.FullName, _.Value.IsMatch));
                ActionAddins.ForEach(
                    _ => CacheMap<object>.ActionCacheMap.Put(_.Metadata.Type.FullName, _.Value.DoAction));

                Log.Info("Done caching.");*/

                #endregion
            }
        }
    }
}
