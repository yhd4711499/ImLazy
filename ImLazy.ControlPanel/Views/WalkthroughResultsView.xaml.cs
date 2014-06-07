using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace ImLazy.ControlPanel.Views
{
    /// <summary>
    /// WalkthroughResultsView.xaml 的交互逻辑
    /// </summary>
    public partial class WalkthroughResultsView : UserControl
    {
        public WalkthroughResultsView()
        {
            InitializeComponent();
        }

        public void SetResults(object results)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(results);
            view.GroupDescriptions.Add(new PropertyGroupDescription("Folder"));
            view.SortDescriptions.Add(new SortDescription("EntryName", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("RuleName", ListSortDirection.Ascending));
            ListBox.ItemsSource = view;
        }
    }
}
