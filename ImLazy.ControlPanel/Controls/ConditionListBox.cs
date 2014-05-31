using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ImLazy.ControlPanel.Annotations;
using ImLazy.ControlPanel.ViewModel;

namespace ImLazy.ControlPanel.Controls
{
    class ConditionListBox : ItemsControl
    {
        private ObservableCollection<ConditionCorpViewModel> _conditoins;
        private Dictionary<ConditionCorpViewModel, ListBoxItem> map = new Dictionary<ConditionCorpViewModel, ListBoxItem>();
        public ListBoxItem SelectedItem { [UsedImplicitly] get; private set; }

        public ConditionListBox()
        {
            DataContextChanged += ConditionListBox_DataContextChanged;
        }

        void ConditionListBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var oldConditions = e.OldValue as ObservableCollection<ConditionCorpViewModel>;
            if (oldConditions != null)
            {
                UnRegister(oldConditions);
                oldConditions.ForEach(Remove);
            }
            _conditoins = e.NewValue as ObservableCollection<ConditionCorpViewModel>;
            if (_conditoins != null)
            {
                Register(_conditoins);
                var index = 0;
                foreach (ConditionCorpViewModel newItem in _conditoins)
                {
                    Insert(index, newItem);
                    index++;
                }
            }
        }

        private void UnRegister(ObservableCollection<ConditionCorpViewModel> items)
        {
            items.CollectionChanged -= Conditoins_CollectionChanged;
            items.ForEach(_ =>
            {
                var branch = _ as ConditionBranchViewModel;
                if (branch != null)
                    UnRegister(branch.SubConditions);
            });
        }

        private void Register(ObservableCollection<ConditionCorpViewModel> items)
        {
            if (items == null)
                return;

            items.CollectionChanged += Conditoins_CollectionChanged;
            items.ForEach(_ =>
            {
                var branch = _ as ConditionBranchViewModel;
                if (branch != null)
                    Register(branch.SubConditions);
            });
        }

        private void Insert(int startIndex, ConditionCorpViewModel vm)
        {
            if (vm.Parent != null && vm.Parent != vm)
            {
                ListBoxItem lbi;
                if (!map.TryGetValue(vm.Parent, out lbi))
                {
                    return;
                    //map[vm.Parent] = new ListBoxItem();
                }
                var itemsControl = (ItemsControl)((GroupBox)((ConditionBranchView)lbi.Content).Content).Content;
                itemsControl.Items.Insert(startIndex, NewListBoxItem(vm));
            }
            else
            {
                Items.Add(NewListBoxItem( vm.Parent));
            }
        }

        private ListBoxItem NewListBoxItem(ConditionCorpViewModel vm)
        {
            UserControl view;
            if (vm is ConditionBranchViewModel)
            {
                view = new ConditionBranchView();
                ((GroupBox) view.Content).Header = vm.MainView;
            }
            else
                view = new ConditionLeafView();
            view.DataContext = vm;
            view.HorizontalAlignment = HorizontalAlignment.Stretch;
            var lbi = new ListBoxItem
            {
                Content = view, 
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch
            };
            lbi.MouseLeftButtonDown += lbi_MouseLeftButtonDown;
            map[vm] = lbi;

            var branch = vm as ConditionBranchViewModel;
            if (branch != null)
            {
                for (int i = 0; i < branch.SubConditions.Count; i++)
                {
                    Insert(i, branch.SubConditions[i]);
                }
            }
            

            return lbi;
        }

        void lbi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            map.ForEach(_ => _.Value.IsSelected = false);

            var lbi = sender as ListBoxItem;
            Debug.Assert(lbi != null, "lbi != null");
            lbi.IsSelected = true;

            SelectedItem = lbi;
        }

        private void Remove(ConditionCorpViewModel vm)
        {
            if (vm.Parent != null)
            {
                var lbi = map[vm.Parent];
                var itemsControl = (ItemsControl)((GroupBox)((ConditionBranchView)lbi.Content).Content).Content;
                
                itemsControl.Items.Remove(map[vm]);
                map[vm].MouseLeftButtonDown -= lbi_MouseLeftButtonDown;

                var branch = vm as ConditionBranchViewModel;
                if (branch != null)
                {
                    
                    branch.SubConditions.ForEach(Remove);
                }
            }
            else
            {
                map[vm].MouseLeftButtonDown -= lbi_MouseLeftButtonDown;
                Items.Remove(map[vm]);
            }
        }

        void Conditoins_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                var index = e.NewStartingIndex;
                foreach (ConditionCorpViewModel newItem in e.NewItems)
                {
                    var item = newItem as ConditionBranchViewModel;
                    if (item != null)
                        Register(item.SubConditions);

                    Insert(index, newItem);
                    index++;
                }
            }
            if (e.OldItems != null)
            {
                foreach (ConditionCorpViewModel oldItem in e.OldItems)
                {
                    var item = oldItem as ConditionBranchViewModel;
                    if (item != null)
                        UnRegister(item.SubConditions);

                    Remove(oldItem);
                }
            }
        }
    }
}
