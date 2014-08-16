using System;
using System.Collections;
using System.Windows;
using System.Windows.Input;
using ImLazy.ControlPanel.ViewModel;
using ImLazy.Runtime;

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
	        internal Point StartPoint;
	    }

	    private readonly DragInfo<RuleViewModel> _ruleInfo = new DragInfo<RuleViewModel>();

	    public MainWindow()
		{
			InitializeComponent();
		}

	    private void RuleViewModel_OnDrop(object sender, DragEventArgs e)
	    {
	        if (!_ruleInfo.IsDragging)
	        {
	            e.Effects = DragDropEffects.None;
	            return;
	        }
	        var itemsSource = RuleListBox.ItemsSource as IList;
            if(itemsSource == null)return;

            var dropTarge = ((FrameworkElement)sender).DataContext as RuleViewModel;

            // source and target are the same.
            if (_ruleInfo.Data == dropTarge) return;

            // insert.
	        var index = itemsSource.IndexOf(dropTarge);
            itemsSource.Remove(_ruleInfo.Data);
            itemsSource.Insert(index, _ruleInfo.Data);
            _ruleInfo.Data = null;

            // maintain orders.
            var i = 0;
            foreach (RuleViewModel ruleViewModel in itemsSource)
            {
                ruleViewModel.Property.Priority = i++;
            }
            DataStorage.Instance.Save();
	    }

	    private void RuleListBox_OnDragOver(object sender, DragEventArgs e)
	    {
            if(e.Data.GetData(typeof(RuleViewModel)) == null)
                e.Effects = DragDropEffects.None;
	    }

	    private void RuleItem_OnMouseMove(object sender, MouseEventArgs e)
	    {
            if(_ruleInfo.IsDragging)return;
            if(e.LeftButton != MouseButtonState.Pressed)return;

            var currentPos = e.GetPosition(sender as IInputElement);

            if (((Math.Abs(currentPos.X - _ruleInfo.StartPoint.X) < SystemParameters.MinimumHorizontalDragDistance) &&
                (Math.Abs(currentPos.Y - _ruleInfo.StartPoint.Y) < SystemParameters.MinimumVerticalDragDistance)))
                return;

            var fe = (FrameworkElement)sender;
	        _ruleInfo.IsDragging = true;
            DragDrop.DoDragDrop(fe, _ruleInfo.Data, DragDropEffects.All);
            _ruleInfo.IsDragging = false;
	    }
        private void RuleItem_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	    {
            var fe = (FrameworkElement)sender;
            _ruleInfo.Data = fe.DataContext as RuleViewModel;
            if (_ruleInfo.Data == null) return;
            //_ruleInfo.SourceIndex = RuleListBox.SelectedIndex;
            _ruleInfo.StartPoint = e.GetPosition(sender as IInputElement);
	    }

	    private void FolderItem_OnDrop(object sender, DragEventArgs e)
	    {
	        var data = e.Data.GetData(typeof (RuleViewModel)) as RuleViewModel;
	        if (data == null)
	        {
	            return;
	        }
            e.Effects = DragDropEffects.Copy;
	        var folderVm = ((FrameworkElement) sender).DataContext as FolderViewModel;
            if(folderVm == null)return;
	        folderVm.CopyRule(data.Rule);
	    }
	}
}
