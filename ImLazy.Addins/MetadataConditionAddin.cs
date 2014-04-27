using ImLazy.Contracts;
using log4net;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ImLazy.Util;
using ImLazy.RunTime;
using LogManager = ImLazy.RunTime.LogManager;

namespace ImLazy.Addins
{
    [ExportMetadata("Name", "元数据")]
    [ExportMetadata("Type", typeof(MetadataConditionAddin))]
    [Export(typeof(IConditionAddin))]
    public class MetadataConditionAddin : IConditionAddin
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(MetadataConditionAddin));

        #region Class Initiation
        /// <summary>
        /// Stores all the metadata properties
        /// </summary>
        public static readonly List<ShellPropertyDescription> Properties = new List<ShellPropertyDescription>();
        /// <summary>
        /// To map a property key to a predefined priority which is actually the index of the property key in this list.
        /// </summary>
        static readonly List<PropertyKey> PriorityMap = new List<PropertyKey>
        {
            SystemProperties.System.Size,
            SystemProperties.System.DateCreated,
            SystemProperties.System.DateAccessed,
            SystemProperties.System.FileExtension,
            SystemProperties.System.FileName,
        };
        /// <summary>
        /// Recents used keys
        /// </summary>
        //static readonly List<PropertyKey> RecentUsedKeys = new List<PropertyKey>();

        /// <summary>
        /// Stores all the operation
        /// </summary>
        static readonly List<ConditionOpeartion> Opeartions = new List<ConditionOpeartion>
        {
            GetOperation<string>("equals",(a,b)=>a.ToString().Equals(b.ToString(), StringComparison.InvariantCultureIgnoreCase)),
            GetOperation<string>("contains",(a,b)=>a.ToString().Contains(b.ToString())),
            GetOperation<string>("starts with",(a,b)=>a.ToString().StartsWith(b.ToString(), StringComparison.InvariantCultureIgnoreCase)),
            GetOperation<string>("ends with",(a,b)=>a.ToString().EndsWith(b.ToString(), StringComparison.InvariantCultureIgnoreCase)),
            GetOperation<string>("not equals",(a,b)=>!a.ToString().Equals(b.ToString(), StringComparison.InvariantCultureIgnoreCase)),
            GetOperation<string>("not contains",(a,b)=>!a.ToString().Contains(b.ToString())),
            GetOperation<string>("not starts with",(a,b)=>!a.ToString().StartsWith(b.ToString(), StringComparison.InvariantCultureIgnoreCase)),
            GetOperation<string>("not ends with",(a,b)=>!a.ToString().EndsWith(b.ToString(), StringComparison.InvariantCultureIgnoreCase)),
            GetOperation<long>(">",(a,b)=>(long)a > (long)b),
            GetOperation<long>(">=",(a,b)=>(long)a >= (long)b),
            GetOperation<long>("=",(a,b)=>(long)a == (long)b),
            GetOperation<long>("<",(a,b)=>(long)a < (long)b),
            GetOperation<long>("<=",(a,b)=>(long)a <= (long)b),
            GetOperation<long>("!=",(a,b)=>(long)a != (long)b),
        };

        static MetadataConditionAddin()
        {
            Log.Debug("Initiating ...");
            try
            {
                LoadAllProperties(typeof(SystemProperties));
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
                Log.Debug("MetadataConditionAddin Initiated.");
            }
            catch (Exception ex)
            {
                Log.Error("Failed in initiating.", ex);
            }
        }

        static void LoadAllProperties(Type root)
        {
            Log.DebugFormat("Loading properties in {0}...", root);
            foreach (var item in root.GetNestedTypes())
            {
                foreach (var key in item.GetProperties())
                {
                    Properties.Add(SystemProperties.GetPropertyDescription((PropertyKey)key.GetValue(null)));
                }
                LoadAllProperties(item);
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
        /// <param name="symbol"></param>
        /// <param name="func">Match function</param>
        /// <returns></returns>
        static ConditionOpeartion GetOperation<T>(string symbol, Func<object, object, bool> func)
        {
            return new ConditionOpeartion(typeof(T), symbol, func);
        }

        internal static IEnumerable<ConditionOpeartion> GetAvailSymbols(ShellPropertyDescription sd)
        {
            return Opeartions.Where(_ => _.TargetType == sd.ValueType);
        }

        #endregion

        #region API
        public bool IsMatch(string filePath, SerializableDictionary<string, object> arg)
        {
            Log.Debug("Begin match.");

            var symbol = arg.TryGetValue<string>(ConfigNames.Symbol);
            var matchObjects = arg.TryGetValue(ConfigNames.TargetObject);
            var targetProperty = arg.TryGetValue<string>(ConfigNames.TargetProperty);

            // get property object
            var so = ShellObject.FromParsingName(filePath);
            IShellProperty p;
            try
            {
                p = so.Properties.GetProperty(targetProperty);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("Failed in IShellProperty.GetProperty({0}) on {1}!", targetProperty, filePath), ex);
                return false;
            }
            // get operation coresponding to the type
            var t = p.ValueType;
            var operationCache = GetAvailSymbols(p.Description).FirstOrDefault(_ => _.Name.Equals(symbol));
            if (operationCache == null)
            {
                Log.WarnFormat("Can't find a correct operation for {0}. Return false instead.", t);
                return false;
            }
            var value = p.ValueAsObject;
            if (value == null)
            {
                Log.DebugFormat("The value of [{0}] in [{1}] is null!", targetProperty, filePath);
                return false;
            }

            // dose the value matcth any of the given matchObjects ?
            var matchStrings = ((string) matchObjects).Split(Spliter);
            Log.DebugFormat("Matching {0} {1} [{2}]", value, operationCache.Name, String.Join(",", matchStrings));
            var result = matchStrings.Any(_ => operationCache.IsMatch(value, _)); //operationCache.IsMatch(value, matchObjects);

            Log.DebugFormat("Done matching : {0}", result);
            return result;

        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new MetadataConditionAddinView { Configuration = config };
        }
        #endregion

        /// <summary>
        /// An extension for Symbol
        /// </summary>
        internal class ConditionOpeartion
        {
            public Type TargetType { get; private set; }
            public string Name { get; private set; }
            public Func<object, object, bool> IsMatch { get; private set; }

            public ConditionOpeartion(Type type, string name, Func<object, object, bool> matchFunc)
            {
                TargetType = type;
                Name = name;
                IsMatch = matchFunc;
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
