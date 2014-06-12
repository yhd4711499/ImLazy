using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using ImLazy.Annotations;
using ImLazy.SDK.Lexer;
using log4net;

namespace ImLazy.Runtime
{
    public class LexerAddinHost
    {
        private static readonly object LockObj = new object();
        private static readonly ILog Log = LogManager.GetLogger(typeof(LexerAddinHost));

        public readonly CacheMap<Func<string, object>> SubjectsCacheMap = new CacheMap<Func<string, object>>();

        public readonly CacheMap<Func<object, object, bool>> VerbsCacheMap = new CacheMap<Func<object, object, bool>>();

        public readonly CacheMap<Func<string, object>> ObjectsCacheMap = new CacheMap<Func<string, object>>();

        #region Singleton

        private static readonly LexerAddinHost _instance = new LexerAddinHost();

        public static LexerAddinHost Instance
        {
            get { return _instance; }
        }

        #endregion

        #region Addins
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        [ImportMany]
        public IEnumerable<Lazy<ISubject, ILexerData>> Subjects { get; [UsedImplicitly] set; }

        [ImportMany]

        public IEnumerable<Lazy<IVerb, ILexerData>> Verbs { get; [UsedImplicitly] set; }

        [ImportMany]
        public IEnumerable<Lazy<IObject, ILexerData>> Objects { get; [UsedImplicitly] set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local
        #endregion

        static LexerAddinHost()
        {
            Log.Info("LexerAddinHost initiating...");
            _instance.LoadAddins();
            _instance.BuildCache();
            Log.Info("LexerAddinHost initiated.");
        }

        ~LexerAddinHost()
        {
            Log.Info("LexerAddinHost finalized.");
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
                Log.Info("Loading lexer addins from '\\Addins' subfolder and self (ImLazy.dll)...");
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
                Log.InfoFormat("Loading finished. Available addins are listed below:\n\t{0} Subjects\n\t\t{1}\n\t{2} Verbs\n\t\t{3}\n\t{4} Objects\n\t\t{5}",
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
            if (nextType == null) return null;
            return Verbs.Where(_ => _.Value.GetSupportedSubjectTypes().Any(__ => __.Equals(nextType)));
        }

        public IEnumerable<Lazy<IObject, ILexerData>> GetSupportedObjectsByVerbType(LexerType verbType)
        {
            if (verbType == null) return null;
            return Objects.Where(_ => verbType.Equals(_.Value.ElementType));
        }

        /// <summary>
        /// Build caches for view creators and addin functions for both conditions and actions.
        /// <para>TS:true</para>
        /// </summary>
        public void BuildCache()
        {
            lock (LockObj)
            {
                #region Cache lexer items and ViewCreator func to CacheMap<object>

                Log.Info("Building view creators, subjects and verbs caches...");

                Objects.ForEach(
                    _ => ObjectsCacheMap.Put(_.Value.GetType().FullName, _.Value.GetObject));
                Subjects.ForEach(
                    _ => SubjectsCacheMap.Put(_.Value.GetType().FullName, _.Value.GetProperty));
                Verbs.ForEach(
                    _ => VerbsCacheMap.Put(_.Value.GetType().FullName, _.Value.IsMatch));
                Log.Info("Done caching.");

                #endregion

            }
        }
    }
}
