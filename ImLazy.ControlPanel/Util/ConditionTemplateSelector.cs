using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
