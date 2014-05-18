using System.Collections.Generic;
using System.Linq;
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
            var index = -1;
            while (true)
            {
                var localStringSource = cmb.ItemsSource as IEnumerable<LocalString>;
                if (localStringSource != null)
                {
                    index = localStringSource.ToList().FindLastIndex(_ => _.Value.Equals(item));
                    break;
                }
                var lazyAddinSource = cmb.ItemsSource as IEnumerable<ILexer>;
                if (lazyAddinSource != null)
                {
                    index = lazyAddinSource.ToList().FindLastIndex(_ => _.GetType().FullName.Equals(item));
                    break;
                }
                
                break;
            }
            cmb.SelectedIndex = index == -1 ? 0 : index;
            
        }
    }
}
