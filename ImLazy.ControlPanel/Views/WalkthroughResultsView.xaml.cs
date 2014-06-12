using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ImLazy.ControlPanel.ViewModel;
using ImLazy.Runtime;

namespace ImLazy.ControlPanel.Views
{
    /// <summary>
    /// WalkthroughResultsView.xaml 的交互逻辑
    /// </summary>
    public partial class WalkthroughResultsView
    {
        public WalkthroughResultsView()
        {
            InitializeComponent();
            Loaded += WalkthroughResultsView_Loaded;
        }

        void WalkthroughResultsView_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        public void SetResults(object results)
        {
            var view = CollectionViewSource.GetDefaultView(results);
            view.GroupDescriptions.Add(new PropertyGroupDescription("Folder"));
            view.SortDescriptions.Add(new SortDescription("EntryName", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("RuleName", ListSortDirection.Ascending));
            ListBox.ItemsSource = view;
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await Service.Util.DoAsync(Service.Util.Execute);
            Refresh();
        }

        private async void Refresh()
        {
            var results = await Executor.Instance.Walkthrough(ViewModelLocator.Instance.Main.Folders.Select(_ => _.Folder).ToArray());
            SetResults(results);
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
