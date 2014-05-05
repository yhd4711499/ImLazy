using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfLocalization;

namespace ImLazy.Addins.Utils
{
    public static class ComboBoxUtil
    {
        public static void SelectItem(this ComboBox cmb, string item)
        {
            var modeSource = (IEnumerable<LocalString>) cmb.ItemsSource;
            cmb.ItemsSource = modeSource;
            var index = modeSource.ToList().FindLastIndex(_ => _.Value.Equals(item));
            cmb.SelectedIndex = index;
        }
    }
}
