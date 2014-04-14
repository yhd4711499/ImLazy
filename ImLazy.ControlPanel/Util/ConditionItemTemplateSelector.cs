using ImLazy.ControlPanel.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace ImLazy.ControlPanel.Util
{
    public class ConditionItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NestedTemplate { get; set; }
        public DataTemplate NormalTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ConditionBranchViewModel)
                return NestedTemplate;
            if (item is ConditionCorpViewModel)
                return NormalTemplate;
            return base.SelectTemplate(item, container);
        }
    }
}
