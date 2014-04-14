using System;

namespace ImLazy.Data
{
    [Serializable]
    public class ConditionCorp : AddinInfo
    {
        // TODO: This property would result in Xml serialzation loop.
        //public ConditionCorp Parent { get; set; }
    }
}
