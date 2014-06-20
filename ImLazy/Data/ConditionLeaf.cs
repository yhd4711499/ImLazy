using System;
using ImLazy.Entities;

namespace ImLazy.Data
{
    [Serializable]
    public class ConditionLeaf : ConditionCorp
    {
        protected override AddinInfoEntity GetDerivedEntity()
        {
            return new ConditionLeafEntity();
        }
    }
}