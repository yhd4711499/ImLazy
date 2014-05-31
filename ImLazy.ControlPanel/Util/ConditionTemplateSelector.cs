using System.Windows;
using System.Windows.Controls;
using ImLazy.ControlPanel.ViewModel;

namespace ImLazy.ControlPanel.Util
{
    class ConditionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NormalTemplate { get; set; }
        public DataTemplate NestedTempldate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ConditionBranchViewModel)
                return NestedTempldate;
            return NormalTemplate;
        }
    }
}
