using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImLazy.Data;
using log4net;

namespace ImLazy.Runtime
{
    public class Executor
    {
        public readonly static Executor Instance = new Executor();

        public delegate void ImLazyAction(string arg1, SerializableDictionary<string, object> arg2, out string path);
        public delegate bool ImLazyFunc(string arg1, SerializableDictionary<string, object> arg2);

        private readonly CacheMap<ImLazyFunc> _conditionCacheMap = new CacheMap<ImLazyFunc>();
        private readonly CacheMap<ImLazyAction> _actionCacheMap = new CacheMap<ImLazyAction>();
        private readonly CacheMap<Rule> _ruleCacheMap = new CacheMap<Rule>();

        private readonly HashSet<string> _execludsions = new HashSet<string>
        {
            "desktop.ini",
            "Thumbs.db",
            ".td"
        };

        public CacheMap<ImLazyFunc> ConditionCacheMap
        {
            get { return _conditionCacheMap; }
        }

        public CacheMap<ImLazyAction> ActionCacheMap
        {
            get { return _actionCacheMap; }
        }

        public CacheMap<Rule> RuleCacheMap
        {
            get { return _ruleCacheMap; }
        }


        /// <summary>
        /// Used to store LastWriteTime for files scanned.
        /// <para>When executing, the time stored will be compared to the actual time of </para>
        /// <para>the file-rule, which is a combination of file path and <see cref="RuleProperty"/>.</para>
        /// <para>Followed procedures will be skipped if these two time are the same.</para>
        /// </summary>
        private readonly static FileRuleCombinationCache Records = new FileRuleCombinationCache();

        private static readonly ILog Log = LogManager.GetLogger(typeof(Executor));

        static Executor()
        {
        }

        public static void ClearCache(Guid ruleGuid)
        {
            Records.Remove(ruleGuid);
        }

        public Task<IEnumerable<WalkthroughResult>> Walkthrough(params Folder[] folders)
        {
            // 每个目录都在各自的线程中运行
            return Task.Factory.StartNew<IEnumerable<WalkthroughResult>>(() =>
            {
                var results = new List<WalkthroughResult>();
                folders.ForEach(_ => results.AddRange(Do(_, true)));
                return results;
            });
        }

        public void Execute(IEnumerable<Folder> folders)
        {
            // 每个目录都在各自的线程中运行
            var enumerable = folders as Folder[] ?? folders.ToArray();
            enumerable.AsParallel().ForAll(_=>Do(_));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="walkthrough"></param>
        private IEnumerable<WalkthroughResult> Do(Folder folder, bool walkthrough = false)
        {
            var entries = from entry in Directory.EnumerateFileSystemEntries(folder.FolderPath)
                let fileName = Path.GetFileName(entry)
                where fileName != null && !_execludsions.Any(fileName.Contains)
                select entry;
            var results = new List<WalkthroughResult>();
            entries.AsParallel().ForEach(fe =>
            {
                foreach (var rp in folder.RuleProperties.Where(_=>_.Enabled))
                {
                    if (!walkthrough)
                    {
                        // Is the file-rule untouched since last execution?
                        int lastExeTime, lastWriteTime = File.GetLastWriteTime(fe).Millisecond;
                        if (Records.TryGetValue(rp.RuleGuid, fe, out lastExeTime) && lastExeTime == lastWriteTime)
                        {
                            // If so, skip it.
                            Log.DebugFormat("{0} : {1} has no changes since last execution and is skipped.", rp.RuleGuid, fe);
                            continue;
                        }
                        Records[rp.RuleGuid, fe] = lastWriteTime;
                    }

                    // Get the rule by guid
                    var rule = RuleCacheMap.Get(rp.RuleGuid);
                    if (rule == null)
                    {
                        Log.WarnFormat("Target rule [{0}] not found!", rp.RuleGuid);
                        continue;
                    }

                    // Check file by the conditions of the rule
                    if (!IsMatch(rule.ConditionBranch, fe)) continue;

                    results.Add(new WalkthroughResult(fe, rule.Name, folder.FolderPath));

                    if (walkthrough) return;
                    
                    Log.Debug("Condition matched. Performing actions...");
                    // Execution the actions
                    ExecuteAction(rule, fe, rp);
                }
            });
            return results;
        }

        private void ExecuteAction(Rule rule, string fe, RuleProperty rp)
        {
            int passed = 0, failed = 0;
            rule.Actions.ForEach(action =>
            {
                var actionMethod = ActionCacheMap.Get(action.AddinType);
                if (actionMethod == null)
                {
                    Log.WarnFormat("Action [name:{0}, type:{1}] not found!", action.Name, action.AddinType);
                    return;
                }
                Log.Debug("Try performing action ...");
                try
                {
                    string updatedPath;
                    actionMethod(fe, action.Config, out updatedPath);
                    if (updatedPath != null)
                    {
                        Log.DebugFormat("Path has been updated to [{0}]", updatedPath);
                        fe = updatedPath;
                    }
                    passed++;
                    Log.Debug("Passed!");
                }
                catch (Exception ex)
                {
                    failed++;
                    Log.Error("Action failed!", ex);
                }
                finally
                {
                    // Remove record to avoid memory leak
                    Records.Remove(rp.RuleGuid, fe);
                }
            });
            Log.DebugFormat("Actions all done. Passed:{0}, failed:{1}", passed, failed);
        }

        private bool IsMatch(ConditionCorp corp ,string filePath)
        {
            if (corp == null) throw new ArgumentNullException("corp");
            while (true)
            {
                var leaf = corp as ConditionLeaf;
                if (leaf != null)
                {
                    var match = false;
                    try
                    {
                        match = ConditionCacheMap.Get(leaf.AddinType)(filePath, leaf.Config);
                    }
                    catch (Exception e)
                    {
                        Log.Error(String.Format("Failed in matching {0}, config: {1}", filePath, leaf.Config), e);
                    }
                    return match;
                }
                var branch = corp as ConditionBranch;
                if (branch == null || branch.SubConditions == null || !branch.SubConditions.Any())
                    return false;
                switch (branch.Mode)
                {
                    case ConditionMode.All:
                        return branch.SubConditions.All(c => IsMatch(c, filePath));
                    case ConditionMode.None:
                        return !branch.SubConditions.Any(c => IsMatch(c, filePath));
                    case ConditionMode.Any:
                        return branch.SubConditions.Any(c => IsMatch(c, filePath));
                    default:
                        Log.Error("Can't get " + ConfigNames.Symbol + " for ConditionBranch : Mode is not specified !");
                        return false;
                }
            }
        }

        /// <summary>
        /// A <see cref="Dictionary{T,V}"/> which takes a computed hashcode of
        /// <para> <see cref="RuleProperty"/> and <see cref="string"/> as key and <see cref="int"/> as value.</para>
        /// </summary>
        private class FileRuleCombinationCache : Dictionary<Guid, Dictionary<string, int>>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="propertyGuid"><see cref="Guid"/></param>
            /// <param name="filePath">File path</param>
            /// <returns></returns>
            public int this[Guid propertyGuid, string filePath]
            {
                set
                {
                    Dictionary<string, int> pairs;
                    if (!TryGetValue(propertyGuid, out pairs))
                    {
                        pairs = new Dictionary<string, int>();
                        this[propertyGuid] = pairs;
                    }
                    pairs[filePath] = value;
                }
            }

            public void Remove(Guid propertyGuid, string filePath)
            {
                this[propertyGuid].Remove(filePath);
            }

            /// <summary>
            /// Same as TryGetValue(int, out int) but taks a computed hashcode of the first two params as key.
            /// </summary>
            /// <param name="propertyGuid"></param>
            /// <param name="filePath"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool TryGetValue(Guid propertyGuid, string filePath, out int value)
            {
                value = -1;
                Dictionary<string, int> pairs;
                return TryGetValue(propertyGuid, out pairs) && pairs.TryGetValue(filePath, out value);
            }
            
        }
    }
}