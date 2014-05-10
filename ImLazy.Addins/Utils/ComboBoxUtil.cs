using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ImLazy.SDK.Lexer;
using WpfLocalization;

namespace ImLazy.Addins.Utils
{
    public static class ComboBoxUtil
    {
        public static void SelectItem(this ComboBox cmb, string item)
        {
            if(item == null)
                return;
            var localStringSource = cmb.ItemsSource as IEnumerable<LocalString>;
            if (localStringSource != null)
            {
                var index = localStringSource.ToList().FindLastIndex(_ => _.Value.Equals(item));
                cmb.SelectedIndex = index;
            }
            else
            {
                var lazyAddinSource = cmb.ItemsSource as IEnumerable<ILexer>;
                if (lazyAddinSource != null)
                {
                    var index = lazyAddinSource.ToList().FindLastIndex(_ => _.GetType().FullName.Equals(item));
                    cmb.SelectedIndex = index;
                }
            }
            
        }
    }
}
