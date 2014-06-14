using System;
using System.Collections;
using System.Windows;
using System.Windows.Input;
using ImLazy.ControlPanel.ViewModel;
using ImLazy.Data;

namespace ImLazy.ControlPanel
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow
	{
	    class DragInfo<T>
	    {
	        internal bool IsDragging;
	        internal T Data;
            internal int SourceIndex;
	        internal Point StartPoint;
	    }

	    private readonly DragInfo<RuleViewModel> ruleInfo = new DragInfo<RuleViewModel>();
	    private DragInfo<FolderViewModel> folderInfo = new DragInfo<FolderViewModel>();

		public MainWindow()
		{
			InitializeComponent();
		}

	    private void RuleViewModel_OnDrop(object sender, DragEventArgs e)
	    {
	        if (!ruleInfo.IsDragging)
	        {
	            e.Effects = DragDropEffects.None;
	            return;
	        }
	        var itemsSource = RuleListBox.ItemsSource as IList;
            if(itemsSource == null)return;

            var dropTarge = ((FrameworkElement)sender).DataContext as RuleViewModel;

            if (ruleInfo.Data == dropTarge) return;

	        var index = itemsSource.IndexOf(dropTarge);

            itemsSource.Remove(ruleInfo.Data);
            itemsSource.Insert(index, ruleInfo.Data);
            ruleInfo.Data = null;
	    }

	    private void RuleListBox_OnDragOver(object sender, DragEventArgs e)
	    {
            if(e.Data.GetData(typeof(RuleViewModel)) == null)
                e.Effects = DragDropEffects.None;
	    }

	    private void RuleItem_OnMouseMove(object sender, MouseEventArgs e)
	    {
            if(ruleInfo.IsDragging)return;
            if(e.LeftButton != MouseButtonState.Pressed)return;

            var currentPos = e.GetPosition(sender as IInputElement);

            if (((Math.Abs(currentPos.X - ruleInfo.StartPoint.X) < SystemParameters.MinimumHorizontalDragDistance) &&
                (Math.Abs(currentPos.Y - ruleInfo.StartPoint.Y) < SystemParameters.MinimumVerticalDragDistance)))
                return;

            var fe = (FrameworkElement)sender;
	        ruleInfo.IsDragging = true;
            DragDrop.DoDragDrop(fe, ruleInfo.Data, DragDropEffects.All);
            ruleInfo.IsDragging = false;
	    }
        private void RuleItem_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	    {
            var fe = (FrameworkElement)sender;
            ruleInfo.Data = fe.DataContext as RuleViewModel;
            if (ruleInfo.Data == null) return;
            ruleInfo.SourceIndex = RuleListBox.SelectedIndex;
            ruleInfo.StartPoint = e.GetPosition(sender as IInputElement);
	    }

	    private void FolderItem_OnDrop(object sender, DragEventArgs e)
	    {
	        var data = e.Data.GetData(typeof (RuleViewModel)) as RuleViewModel;
	        if (data == null)
	        {
	            return;
	        }
	        var folderVm = ((FrameworkElement) sender).DataContext as FolderViewModel;
            if(folderVm == null)return;
	        folderVm.CopyRule(data.Rule);
	    }

	    private void FolderItem_OnMouseMove(object sender, MouseEventArgs e)
	    {
	    }

	    private void FolderItem_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	    {
	    }
	}
}
