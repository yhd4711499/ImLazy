using System;
using ImLazy.Addins;
using System.Collections.Generic;
using System.Xml.Serialization;
using ImLazy.Util;
using ImLazy.RunTime;

namespace ImLazy.Data
{
    [XmlInclude(typeof(ConditionLeaf))]
    [Serializable]
    public class ConditionBranch : ConditionCorp
    {
        [XmlIgnore]
        public ConditionMode Mode { get; private set; }

        public ConditionBranch()
        {
            SubConditions = new List<ConditionCorp>();
            AddinType = typeof(ConditionsConfigAddin).FullName;
        }

        public List<ConditionCorp> SubConditions { get; set; }

        public override void Save(SerializableDictionary<string, object> config)
        {
            base.Save(config);
            Mode = GetMode(config);
        }

        private static ConditionMode GetMode(Dictionary<string, object> config)
        {
            var modeStr = config.TryGetValue<string>(ConfigNames.Symbol);
            return modeStr != null ? EnumHelper.Parse<ConditionMode>(modeStr) : ConditionMode.All;
        }

        public void Add(ConditionCorp corp)
        {
            SubConditions.Add(corp);
        }
    }
}
