using ImLazy.Data;

namespace ImLazy.ControlPanel.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ConditionLeafViewModel : ConditionCorpViewModel
    {
        public ConditionLeafViewModel(ConditionLeaf conditionCorp, ConditionBranchViewModel parent) : base(conditionCorp, parent)
        {
        }
    }
}