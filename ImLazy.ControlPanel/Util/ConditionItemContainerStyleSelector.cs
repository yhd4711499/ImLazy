using ImLazy.ControlPanel.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace ImLazy.ControlPanel.Util
{
    public class ConditionItemContainerStyleSelector : StyleSelector 
    {
        public Style NestedStyle { private get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ConditionBranchViewModel)
                return NestedStyle;
            return base.SelectStyle(item, container);
        }
    }
}
