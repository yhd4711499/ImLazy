using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CuttingEdge.Conditions;
using ImLazy.Data;
using log4net;

namespace ImLazy.Runtime
{
    public class Executor
    {
        public readonly static Executor Instance = new Executor();

        private readonly CacheMap<Func<string, SerializableDictionary<string, object>, bool>> _conditionCacheMap = new CacheMap<Func<string, SerializableDictionary<string, object>, bool>>();
        private readonly CacheMap<Action<string, SerializableDictionary<string, object>>> _actionCacheMap = new CacheMap<Action<string, SerializableDictionary<string, object>>>();
        private readonly CacheMap<Rule> _ruleCacheMap = new CacheMap<Rule>();

        private readonly HashSet<string> _execludsions = new HashSet<string>
        {
            "desktop.ini",
            "Thumbs.db",
            ".td"
        };

        public CacheMap<Func<string, SerializableDictionary<string, object>, bool>> ConditionCacheMap
        {
            get { return _conditionCacheMap; }
        }

        public CacheMap<Action<string, SerializableDictionary<string, object>>> ActionCacheMap
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

        public void Execute(IEnumerable<Folder> folders)
        {
            Condition.Requires(folders, "folders").IsNotNull();
            
            // 每个目录都在各自的线程中运行
            var enumerable = folders as Folder[] ?? folders.ToArray();
            enumerable.AsParallel().ForAll(Do);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        private void Do(Folder folder)
        {
            var entries = from entry in Directory.EnumerateFileSystemEntries(folder.FolderPath)
                let fileName = Path.GetFileName(entry)
                where fileName != null && !_execludsions.Any(fileName.Contains)
                select entry;
            
            entries.AsParallel().ForEach(fe =>
            {
                foreach (var rp in folder.RuleProperties.Where(_=>_.Enabled))
                {
                    // Is the file-rule untouched since last execution?
                    int lastExeTime, lastWriteTime = File.GetLastWriteTime(fe).Millisecond;
                    if (Records.TryGetValue(rp.RuleGuid, fe, out lastExeTime) && lastExeTime == lastWriteTime)
                    {
                        // If so, skip it.
                        Log.DebugFormat("{0} : {1} has no changes since last exectution and is skipped.", rp.RuleGuid, fe);
                        continue;
                    }
                    Records[rp.RuleGuid, fe] = lastWriteTime;

                    // Get the rule by guid
                    Log.DebugFormat("Searching for rule with GUID:{0}...", rp.RuleGuid);
                    var rule = RuleCacheMap.Get(rp.RuleGuid);
                    if (rule == null)
                    {
                        Log.WarnFormat("Target rule [{0}] not found!", rp.RuleGuid);
                        continue;
                    }
                    Log.DebugFormat("Target rule founded with name:[{0}]", rule.Name);

                    // Check file by the conditions of the rule
                    Log.DebugFormat("Checking [{0}] ...", fe);
                    if (IsMatch(rule.ConditionBranch, fe))
                    {
                        // Execution the actions
                        Log.Debug("ConditionBranch matched. Performing actions...");
                        int passed = 0, failed = 0;
                        rule.Actions.ForEach(action =>
                        {
                            Log.DebugFormat("Searching action {0} (name : {1}) ...", action.AddinType, action.Name);
                            var actionMethod = ActionCacheMap.Get(action.AddinType);
                            if (actionMethod == null)
                            {
                                Log.Warn("Action not found!");
                                return;
                            }
                            Log.Debug("Try performing action ...");
                            try
                            {
                                actionMethod(fe, action.Config);
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
                    else
                    {
                        Log.Debug("Condition not match and was Skipped.");
                    }
                }
            });
        }

        private bool IsMatch(ConditionCorp corp ,string filePath)
        {
            if (corp == null) throw new ArgumentNullException("corp");
            while (true)
            {
                var leaf = corp as ConditionLeaf;
                if (leaf != null)
                {
                    return ConditionCacheMap.Get(leaf.AddinType)(filePath, leaf.Config);
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

            /// <summary>
            /// Get the computed hash code
            /// </summary>
            /// <param name="propertyGuid"></param>
            /// <param name="filePath"></param>
            /// <returns></returns>
            /*private static int CalcHash(RuleProperty p, string filePath)
            {
                return p.GetHashCode() + 31 * filePath.GetHashCode();
            }*/

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