using ImLazy.Data;

namespace ImLazy.ControlPanel.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ConditionCorpViewModel : AddinInfoViewModelBase
    {
        public new ConditionBranchViewModel Parent
        {
            get { return base.Parent as ConditionBranchViewModel; }
            set { base.Parent = value; }
        }
        /// <summary>
        /// Initialize a new instance of the ConditionCorpViewModel class.
        /// </summary>
        public ConditionCorpViewModel(ConditionCorp conditionCorp, ConditionCorpViewModel parent)
            : base(conditionCorp, parent)
        {
        }
    }
}