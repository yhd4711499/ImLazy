using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ImLazy.ControlPanel.ViewModel;

namespace ImLazy.ControlPanel.Views
{
    /// <summary>
    /// SelectRuleView.xaml 的交互逻辑
    /// </summary>
    public partial class SelectRuleView : UserControl
    {
        public RuleViewModel SelectedRuleViewModel { get; private set; }
        public FolderViewModel FolderParent { get; set; }
        public SelectRuleView()
        {
            InitializeComponent();
        }

        private void OnItemDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            SelectRuleViewModel();
        }

        private void OnSelectButtonClicked(object sender, RoutedEventArgs e)
        {
            SelectRuleViewModel();
        }

        private void SelectRuleViewModel()
        {
            SelectedRuleViewModel = LstRules.SelectedItem as RuleViewModel;
            if (SelectedRuleViewModel == null) return;
            var folderVm = FolderParent;
            if(folderVm == null) return;
            if (folderVm.Folder.RuleProperties.Any(_ => _.RuleGuid.Equals(SelectedRuleViewModel.Rule.Guid)))
            {
                // exist same rule
                MessageBox.Show("Exist same rule in thie folder. Please select an other one.", "Same rule exist");
                SelectedRuleViewModel = null;
            }
            else
            {
                var window = Parent as Window;
                if (window == null) return;
                window.DialogResult = true;
                window.Close();
            }
        }
    }
}
