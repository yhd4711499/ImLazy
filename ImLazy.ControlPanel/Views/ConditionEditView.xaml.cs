using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ImLazy.ControlPanel.ViewModel;

namespace ImLazy.ControlPanel.Views
{
    /// <summary>
    /// ConditionEditView.xaml 的交互逻辑
    /// </summary>
    public partial class ConditionEditView
    {
        Point _lastMouseDown;
        FrameworkElement _draggedItem, _target;
        private ICollection<ConditionCorpViewModel> _itemsSource; 
        public ConditionEditView()
        {
            InitializeComponent();
            DataContextChanged += ConditionEditView_DataContextChanged;
        }

        void ConditionEditView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _itemsSource = e.NewValue as ICollection<ConditionCorpViewModel>;
        }
        
        private void treeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _lastMouseDown = e.GetPosition(this);
            }
        }

        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton != MouseButtonState.Pressed) return;
                var currentPosition = e.GetPosition(this);


                _draggedItem = (FrameworkElement)sender;
                if (_draggedItem == null) return;
                var source = FindVisualParent<ItemsControl>(_draggedItem);
                if (source == null) return;
                var originalControl = e.OriginalSource as FrameworkElement;
                if (originalControl == null) return;
                var data = new DataObject(originalControl.DataContext);
                var finalDropEffect = DragDrop.DoDragDrop(source, data,
                    DragDropEffects.Move);
                //Checking target is not null and item is dragging(moving)
                if ((finalDropEffect != DragDropEffects.Move) || (_target == null)) return;
                // A Move drop was accepted
                if (
                    !CheckDropTarget(originalControl.DataContext as ConditionCorpViewModel,
                        _target.DataContext as ConditionCorpViewModel)) return;
                CopyItem(originalControl.DataContext as ConditionCorpViewModel, _target.DataContext as ConditionBranchViewModel);
                _target = null;
                _draggedItem = null;
            }
            catch (Exception)
            {
            }
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            try
            {

                var currentPosition = e.GetPosition(this);


                // Verify that this is a valid drop and then store the drop target
                var item = GetNearestContainer(e.OriginalSource as UIElement);
                e.Effects = CheckDropTarget(_draggedItem.DataContext as ConditionCorpViewModel, item.DataContext as ConditionCorpViewModel) ? DragDropEffects.Move : DragDropEffects.None;
                e.Handled = true;
            }
            catch (Exception)
            {
            }
        }

        private void treeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                // Verify that this is a valid drop and then store the drop target
                var targetItem = GetNearestContainer(e.OriginalSource as UIElement);
                if (targetItem != null && _draggedItem != null)
                {
                    _target = targetItem;
                    e.Effects = DragDropEffects.Move;

                }
            }
            catch (Exception)
            {
            }
        }
        private bool CheckDropTarget(ConditionCorpViewModel vmSource, ConditionCorpViewModel vmTarget)
        {
            //Check whether the target item is meeting your condition
            var isEqual = vmSource.AddinInfo != vmTarget.AddinInfo;
            return isEqual;

        }
        private void CopyItem(ConditionCorpViewModel sourceData, ConditionBranchViewModel targetData)
        {

            //Asking user wether he want to drop the dragged TreeViewItem here or not
            if (MessageBox.Show("Would you like to drop " + sourceData + " into " + targetData + "", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    
                    //adding dragged TreeViewItem in target TreeViewItem
                    if (targetData != null)
                    {
                        targetData.InsertCondition(0, sourceData);
                    }

                    if (sourceData.Parent == null)
                    {
                        
                    }
                    else
                    {
                        sourceData.Parent.DeleteConditionCommand.Execute(sourceData);
                    }
                }
                catch
                {

                }
            }

        }

        static TObject FindVisualParent<TObject>(UIElement child) where TObject : UIElement
        {
            if (child == null)
            {
                return null;
            }

            var parent = VisualTreeHelper.GetParent(child) as UIElement;

            while (parent != null)
            {
                var found = parent as TObject;
                if (found != null)
                {
                    return found;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent) as UIElement;
                }
            }

            return null;
        }
        private ContentControl GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            var container = element as ContentControl;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as ContentControl;
            }
            return container;
        }
    }
}
