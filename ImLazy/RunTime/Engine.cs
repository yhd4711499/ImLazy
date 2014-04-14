//using System;
//using System.Collections.Generic;
//using System.IO;
//using ImLazy.Data;
//using log4net;
//
//namespace ImLazy.RunTime
//{
//    public class Engine
//    {
//        private static readonly ILog Log = LogManager.GetLogger(typeof(Engine));
//        public static void Do(Folder folder)
//        {
//            foreach (var rule in DataStorage.Instance.Rules)
//            {
//                foreach (var rp in folder.RuleProperties)
//                {
//                    Log.DebugFormat("Finding rule with GUID:{0}...", rp.RuleGuid);
//                    if (!rp.RuleGuid.Equals(rule.Guid)) continue;
//                    Log.DebugFormat("Target rule was found, which is named [{0}]", rule.Name);
//                    var rule1 = rule;
//                    Directory.EnumerateFileSystemEntries(folder.FolderPath).ForEach(fe =>
//                    {
//                        try
//                        {
//                            Log.DebugFormat((string) "Checking [{0}] ...", (object) fe);
//                            if (rule1.ConditionBranch.IsMatch(fe))
//                            {
//                                Log.Debug("ConditionBranch matched. Performing actions...");
//                                rule1.Actions.ForEach(action=>{
//                                                                 Log.DebugFormat("Performing {0} ...", action);
//                                                                 action.Addin.DoAction(fe, action.Config);
//                                                                 Log.Debug("Succeed!");
//                                });
//                                Log.Debug("Actions completed with success!");
//                            }
//                            else
//                            {
//                                Log.Debug("ConditionBranch not match. Skip.");
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            Log.Error(String.Format((string) "Failed in checking match or performing action in [{0}]" ,(object) fe), ex);
//                        }
//                    });
//                }
//            }
//        }
//    }
//}
