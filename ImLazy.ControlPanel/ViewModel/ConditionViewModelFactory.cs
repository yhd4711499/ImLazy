using ImLazy.Data;

namespace ImLazy.ControlPanel.ViewModel
{
    public static class ConditionViewModelFactory
    {
        public static ConditionCorpViewModel Create(ConditionCorp corp, ConditionBranchViewModel parent)
        {
            if(corp is ConditionBranch)
                return new ConditionBranchViewModel(corp as ConditionBranch, parent);
            else if(corp is ConditionLeaf)
                return new ConditionLeafViewModel(corp as ConditionLeaf, parent);
            else
                return new ConditionCorpViewModel(corp, parent);
        }
    }
}