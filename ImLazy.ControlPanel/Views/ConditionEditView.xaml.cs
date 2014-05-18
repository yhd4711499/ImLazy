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
        FrameworkElement draggedItem, _target;
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
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point currentPosition = e.GetPosition(this);


                    draggedItem = (FrameworkElement)sender;
                    if (draggedItem != null)
                    {
                        var source = FindVisualParent<ItemsControl>(draggedItem);
                        if (source == null) return;
                        var originalControl = e.OriginalSource as FrameworkElement;
                        if (originalControl == null) return;
                        var data = new DataObject(originalControl.DataContext);
                        DragDropEffects finalDropEffect = DragDrop.DoDragDrop(source, data,
                            DragDropEffects.Move);
                        //Checking target is not null and item is dragging(moving)
                        if ((finalDropEffect == DragDropEffects.Move) && (_target != null))
                        {
                            // A Move drop was accepted
                            if (CheckDropTarget(originalControl.DataContext as ConditionCorpViewModel, _target.DataContext as ConditionCorpViewModel))
                            {
                                CopyItem(originalControl.DataContext as ConditionCorpViewModel, _target.DataContext as ConditionBranchViewModel);
                                _target = null;
                                draggedItem = null;
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            try
            {

                Point currentPosition = e.GetPosition(this);


                // Verify that this is a valid drop and then store the drop target
                ContentControl item = GetNearestContainer(e.OriginalSource as UIElement);
                if (CheckDropTarget(draggedItem.DataContext as ConditionCorpViewModel, item.DataContext as ConditionCorpViewModel))
                {
                    e.Effects = DragDropEffects.Move;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
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
                ContentControl TargetItem = GetNearestContainer(e.OriginalSource as UIElement);
                if (TargetItem != null && draggedItem != null)
                {
                    _target = TargetItem;
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
        private void CopyItem(ConditionCorpViewModel _sourceData, ConditionBranchViewModel _targetData)
        {

            //Asking user wether he want to drop the dragged TreeViewItem here or not
            if (MessageBox.Show("Would you like to drop " + _sourceData + " into " + _targetData + "", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    
                    //adding dragged TreeViewItem in target TreeViewItem
                    if (_targetData != null)
                    {
                        _targetData.InsertCondition(0, _sourceData);
                    }

                    if (_sourceData.Parent == null)
                    {
                        
                    }
                    else
                    {
                        _sourceData.Parent.DeleteConditionCommand.Execute(_sourceData);
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

            UIElement parent = VisualTreeHelper.GetParent(child) as UIElement;

            while (parent != null)
            {
                TObject found = parent as TObject;
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
            ContentControl container = element as ContentControl;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as ContentControl;
            }
            return container;
        }
    }
}
