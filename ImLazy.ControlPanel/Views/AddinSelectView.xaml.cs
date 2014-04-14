using System.Collections.Generic;
using ImLazy.ControlPanel.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ImLazy.ControlPanel.Views
{
    /// <summary>
    /// AddinSelectView.xaml 的交互逻辑
    /// </summary>
    public partial class AddinSelectView : UserControl
    {
        AddinInfoViewModelBase _dataContext;

        public AddinSelectView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            ComboBoxAddin.SelectionChanged += ComboBoxAddinSelectionChanged;
        }

        void ComboBoxAddinSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            var addinInfoVm = ((AddinInfoViewModelBase)e.AddedItems[0]).AddinInfo;
            _dataContext.ChangeAddinType(addinInfoVm.AddinType);
            ContentAddin.Content = _dataContext.MainView;
        }

        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _dataContext = e.NewValue as AddinInfoViewModelBase;
            if (_dataContext == null)
                return;
            if (_dataContext.AddinInfo == null)
                return;

            // Get items according to the type of _dataContext.
            IEnumerable<AddinInfoViewModelBase> items;
            if (_dataContext is ConditionCorpViewModel)
                items = App.Locator.Main.Conditions;
            else
                items = App.Locator.Main.Actions;

            // Set ItemsSource and selection for ComboBoxAddin.
            ComboBoxAddin.ItemsSource = items;
            var selectedVm = items.FirstOrDefault(_ => _.AddinInfo.AddinType == _dataContext.AddinInfo.AddinType);
            if (selectedVm != null)
            {
                // Select the specified item
                ComboBoxAddin.SelectedItem = selectedVm;
            }
            else
            {
                // Select tht first item if no item is specified.
                ComboBoxAddin.SelectedIndex = 0;
            }
        }
    }
}
