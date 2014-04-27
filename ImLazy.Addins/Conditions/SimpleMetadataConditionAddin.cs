using System.IO;
using ImLazy.Contracts;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ImLazy.Util;
using ImLazy.RunTime;
using LogManager = ImLazy.RunTime.LogManager;

namespace ImLazy.Addins.Conditions
{
    [ExportMetadata("Name", "文件信息")]
    [ExportMetadata("Type", typeof(SimpleMetadataConditionAddin))]
    [Export(typeof(IConditionAddin))]
    public class SimpleMetadataConditionAddin : IConditionAddin
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(SimpleMetadataConditionAddin));

        #region Class Initiation

        public static readonly Dictionary<string, PropertyOpeartion> PropertyOpeartions = new Dictionary<string, PropertyOpeartion>
        {
            {"System.FileName", GetPropertyOpeartion<string>(Path.GetFileName)},
            {"System.FileExtension", GetPropertyOpeartion<string>(Path.GetExtension)},
            {"System.Size", GetPropertyOpeartion<long>(_=> File.Exists(_)? new FileInfo(_).Length.ToString():"0")},
        };

        /// <summary>
        /// Recents used keys
        /// </summary>
        /*static readonly List<PropertyKey> RecentUsedKeys = new List<PropertyKey>();*/

        /// <summary>
        /// Stores all the operation
        /// </summary>
        private static readonly Dictionary<string, ConditionOpeartion> Opeartions = new Dictionary
            <string, ConditionOpeartion>
        {
            {
                "equals",
                GetOperation<string>((a, b) => a.Equals(b, StringComparison.InvariantCultureIgnoreCase))
            },
            {
                "contains",
                GetOperation<string>((a, b) => a.Contains(b))
            },
            {
                "starts with",
                GetOperation<string>((a, b) => a.StartsWith(b, StringComparison.InvariantCultureIgnoreCase))
            },
            {
                "ends with",
                GetOperation<string>((a, b) => a.EndsWith(b, StringComparison.InvariantCultureIgnoreCase))
            },
            {
                "not equals",
                GetOperation<string>((a, b) => !a.Equals(b, StringComparison.InvariantCultureIgnoreCase))
            },
            {
                "not contains",
                GetOperation<string>((a, b) => !a.Contains(b))
            },
            {
                "not starts with",
                GetOperation<string>((a, b) => !a.StartsWith(b, StringComparison.InvariantCultureIgnoreCase))
            },
            {
                "not ends with",
                GetOperation<string>((a, b) => !a.EndsWith(b, StringComparison.InvariantCultureIgnoreCase))
            },
            {
                ">",
                GetOperation<long>((a,b)=>long.Parse(a) > long.Parse(b))
            },
            {
                ">=",
                GetOperation<long>((a,b)=>long.Parse(a) >= long.Parse(b))
            },
            {
                "=",
                GetOperation<long>((a,b)=>long.Parse(a) == long.Parse(b))
            },
            {
                "<",
                GetOperation<long>((a,b)=>long.Parse(a) < long.Parse(b))
            },
            {
                "<=",
                GetOperation<long>((a,b)=>long.Parse(a) <= long.Parse(b))
            },
            {
                "!=",
                GetOperation<long>((a,b)=>long.Parse(a) != long.Parse(b))
            },
        };

        static SimpleMetadataConditionAddin()
        {
            Log.Debug("Initiating ...");
            try
            {
                /*LoadAllProperties(typeof(SystemProperties));
                Log.Debug("Sorting system properties...");
                Properties.Sort((a, b) => 
                {
                    var indexA = PriorityMap.FindIndex(_ => _.Equals(a.PropertyKey));
                    var indexB = PriorityMap.FindIndex(_ => _.Equals(b.PropertyKey));
                    if (indexA == indexB)
                        return 0;
                    if (indexA > indexB)
                        return -1;
                    return 1;
                });
                Log.Debug("SimpleMetadataConditionAddin Initiated.");*/
            }
            catch (Exception ex)
            {
                Log.Error("Failed in initiating.", ex);
            }
        }
        #endregion

        #region Constaints
                private const char Spliter = ',';
        #endregion

        #region Utils
        /// <summary>
        /// Convinience for creating a new Opeartion
        /// </summary>
        /// <typeparam name="T">Param type</typeparam>
        /// <param name="func">Match function</param>
        /// <returns></returns>
        static ConditionOpeartion GetOperation<T>(Func<string, string, bool> func)
        {
            return new ConditionOpeartion(typeof(T), func);
        }

        /// <summary>
        /// Convinience for creating a new Opeartion
        /// </summary>
        /// <typeparam name="T">Param type</typeparam>
        /// <param name="func">Get value function</param>
        /// <returns></returns>
        static PropertyOpeartion GetPropertyOpeartion<T>(Func<string, string> func)
        {
            return new PropertyOpeartion(typeof(T), func);
        }

        internal static IEnumerable<KeyValuePair<string, ConditionOpeartion>> GetAvailSymbols(PropertyOpeartion propertyOperation)
        {
            return Opeartions.Where(_ => _.Value.TargetType == propertyOperation.TargetType);
        }

        internal static string GetPropertyOperationName(object propertyOperation)
        {
            return ((KeyValuePair<string, PropertyOpeartion>)propertyOperation).Key;
        }

        internal static Type GetPropertyOperationType(object propertyOperation)
        {
            return ((KeyValuePair<string, PropertyOpeartion>)propertyOperation).Value.TargetType;
        }
        #endregion

        #region API
        public bool IsMatch(string filePath, SerializableDictionary<string, object> arg)
        {
            Log.Debug("Begin match.");

            var symbol = arg.TryGetValue<string>(ConfigNames.Symbol);
            var matchObjects = arg.TryGetValue(ConfigNames.TargetObject);
            var targetProperty = arg.TryGetValue<string>(ConfigNames.TargetProperty);

            // get PropertyOpeartion
            PropertyOpeartion po;
            if (!PropertyOpeartions.TryGetValue(targetProperty, out po))
            {
                Log.ErrorFormat("Can't find a correct property for [{0}]. Return false instead.", targetProperty);
                return false;
            }
            ConditionOpeartion co;
            if (!Opeartions.TryGetValue(symbol, out co))
            {
                Log.WarnFormat("Can't find a correct operation for [{0}. Return false instead.", po.TargetType);
                return false;
            }
            var value = po.Get(filePath);
            if (value == null)
            {
                Log.DebugFormat("The value of [{0}] in [{1}] is null!", targetProperty, filePath);
                return false;
            }

            // dose the value matcth any of the given matchObjects ?
            var matchStrings = ((string) matchObjects).Split(Spliter);
            Log.DebugFormat("Matching {0} {1} [{2}]", value, symbol, String.Join(",", matchStrings));
            var result = matchStrings.Any(_ => co.IsMatch(value, _)); //operationCache.IsMatch(value, matchObjects);

            Log.DebugFormat("Done matching : {0}", result);
            return result;

        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new SimpleMetadataConditionAddinView { Configuration = config };
        }
        #endregion

        /// <summary>
        /// An extension for Symbol
        /// </summary>
        internal class ConditionOpeartion
        {
            public Type TargetType { get; private set; }
            public Func<string, string, bool> IsMatch { get; private set; }

            public ConditionOpeartion(Type type, Func<string, string, bool> matchFunc)
            {
                TargetType = type;
                IsMatch = matchFunc;
            }
        }

        /// <summary>
        /// An extension for Symbol
        /// </summary>
        public class PropertyOpeartion
        {
            public Type TargetType { get; private set; }
            public Func<string, string> Get { get; private set; }

            public PropertyOpeartion(Type targetType, Func<string, string> getFunc)
            {
                Get = getFunc;
                TargetType = targetType;
            }
        }
    }
}
