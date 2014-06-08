using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;
using ImLazy.SDK.Lexer;
using WpfLocalization;

namespace ImLazy.Addins.Utils
{
    public static class ComboBoxUtil
    {
        /// <summary>
        /// 设置SelctedItem，用于在使用了LocalString或ILexer做为ItemsSource的控件。
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="item">原始名称，对于LocalString为资源名；对于ILexer为类名全称</param>
        public static void SelectItem(this Selector selector, string item)
        {
            if(item == null)
                return;
            var index = -1;
            while (true)
            {
                var localStringSource = selector.ItemsSource as IEnumerable<LocalString>;
                if (localStringSource != null)
                {
                    index = localStringSource.ToList().FindLastIndex(_ => _.Value.Equals(item));
                    break;
                }
                var lazyAddinSource = selector.ItemsSource as IEnumerable<ILexer>;
                if (lazyAddinSource != null)
                {
                    index = lazyAddinSource.ToList().FindLastIndex(_ => _.GetType().FullName.Equals(item));
                }
                
                break;
            }
            selector.SelectedIndex = index == -1 ? 0 : index;
            
        }
    }
}
