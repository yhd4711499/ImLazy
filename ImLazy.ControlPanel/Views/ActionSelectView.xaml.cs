using ImLazy.ControlPanel.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ImLazy.ControlPanel.Views
{
    /// <summary>
    /// AddinSelectView.xaml 的交互逻辑
    /// </summary>
    public partial class ActionSelectView : UserControl
    {
        ActionViewModel _dataContext;

        public ActionSelectView()
        {
            InitializeComponent();
            DataContextChanged += RuleDetailView_DataContextChanged;
            comboBox.SelectionChanged += comboBox_SelectionChanged;
        }

        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            var neutralCondition = ((ActionViewModel)e.AddedItems[0]).AddinInfo;
            _dataContext.AddinInfo.AddinType = neutralCondition.AddinType;
            content.Content = _dataContext.MainView;
        }

        void RuleDetailView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _dataContext = e.NewValue as ActionViewModel;
            if (_dataContext == null)
                return;
            if (_dataContext.AddinInfo == null)
                return;
            var selectedVm = App.Locator.Main.Actions.FirstOrDefault(_ => _.AddinInfo.AddinType == _dataContext.AddinInfo.AddinType);
            comboBox.SelectedItem = selectedVm;
        }
    }
}
