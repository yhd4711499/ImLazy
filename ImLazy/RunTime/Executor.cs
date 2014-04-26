using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImLazy.Contracts;
using ImLazy.Data;
using log4net;

namespace ImLazy.RunTime
{
    public class Executor
    {
        private readonly CacheMap<Func<string, SerializableDictionary<string, object>, bool>> _conditionCacheMap;
        private readonly CacheMap<Action<string, SerializableDictionary<string, object>>> _actionCacheMap;
        private readonly CacheMap<Rule> _ruleCacheMap;

        /// <summary>
        /// 初始化Executor
        /// </summary>
        /// <param name="conditionCacheMap">条件</param>
        /// <param name="actionCacheMap">动作</param>
        /// <param name="ruleCacheMap">规则</param>
        public Executor(
            CacheMap<Func<string, SerializableDictionary<string, object>, bool>> conditionCacheMap
            , CacheMap<Action<string, SerializableDictionary<string, object>>> actionCacheMap
            , CacheMap<Rule> ruleCacheMap)
        {
            _conditionCacheMap = conditionCacheMap;
            _actionCacheMap = actionCacheMap;
            _ruleCacheMap = ruleCacheMap;
        }

        public void Execute(IEnumerable<Folder> folders)
        {
            // 每个目录都在各自的线程中运行
            var enumerable = folders as Folder[] ?? folders.ToArray();
            enumerable.AsParallel().ForAll(Do);
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(Executor));

        private void Do(Folder folder)
        {
            foreach (var rp in folder.RuleProperties)
            {
                Log.DebugFormat("Searching for rule with GUID:{0}...", rp.RuleGuid);
                var rule = _ruleCacheMap.Get(rp.RuleGuid);
                if (rule == null)
                {
                    Log.Warn("Target rule not found!");
                    continue;
                }
                Log.DebugFormat("Target rule founded with name:[{0}]", rule.Name);
                
                Directory.EnumerateFileSystemEntries(folder.FolderPath).ForEach(fe =>
                {
                    Log.DebugFormat("Checking [{0}] ...", fe);
                    if (IsMatch(rule.ConditionBranch, fe))
                    {
                        Log.Debug("ConditionBranch matched. Performing actions...");
                        int successed = 0, failed = 0;
                        rule.Actions.ForEach(action =>
                        {
                            Log.DebugFormat("Searching action {0} (name : {1}) ...", action.AddinType, action.Name);
                            var actionMethod = _actionCacheMap.Get(action.AddinType);
                            if (actionMethod == null)
                            {
                                Log.Warn("Action not found!");
                                return;
                            }
                            Log.Debug("Try performing action ...");
                            try
                            {
                                actionMethod(fe, action.Config);
                                successed++;
                                Log.Debug("Succeed!");
                                    
                            }
                            catch (Exception ex)
                            {
                                failed++;
                                Log.Error("Action failed!",ex);
                            }
                        });
                        Log.DebugFormat("Actions all done. Successed:{0}, failed:{1}", successed, failed);
                    }
                    else
                    {
                        Log.Debug("Condition not match and was Skipped.");
                    }
                });
            }
        }

        private bool IsMatch(ConditionCorp corp ,string filePath)
        {
            if (corp == null) throw new ArgumentNullException("corp");
            while (true)
            {
                var leaf = corp as ConditionLeaf;
                if (leaf != null)
                {
                    return _conditionCacheMap.Get(leaf.AddinType)(filePath, leaf.Config);
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
    }
}