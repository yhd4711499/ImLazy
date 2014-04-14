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
        /// <summary>
        /// Initialize a new instance of the ConditionCorpViewModel class.
        /// </summary>
        public ConditionCorpViewModel(ConditionCorp conditionCorp, ConditionCorpViewModel parent)
            : base(conditionCorp, parent)
        {
        }
    }
}