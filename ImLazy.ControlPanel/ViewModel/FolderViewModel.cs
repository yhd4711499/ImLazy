using System;
using System.Collections.Specialized;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ImLazy.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ImLazy.Runtime;
using ImLazy.ControlPanel.Util;

// ReSharper disable MemberCanBePrivate.Global

namespace ImLazy.ControlPanel.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class FolderViewModel : ViewModelBase
    {
        public Folder Folder { get; private set; }

        public ObservableCollection<RuleViewModel> Rules { get; private set; }


        /// <summary>
        /// Exist属性的名称
        /// </summary>
        public const string ExistPropertyName = "Exist";
        private bool _exist;
        /// <summary>
        /// 文件夹是否存在
        /// </summary>
        public bool Exist
        {
            get
            {
                return _exist;
            }
            set
            {
                if (_exist == value) return;
                _exist = value;
                RaisePropertyChanged(ExistPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Enabled" /> property's name.
        /// </summary>
        public const string EnabledPropertyName = "Enabled";

        /// <summary>
        /// Sets and gets the Enabled property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool Enabled
        {
            get
            {
                return Folder.Enabled;
            }

            set
            {
                if (Folder.Enabled == value)
                {
                    return;
                }
                Folder.Enabled = value;
                RaisePropertyChanged(EnabledPropertyName);
                DataStorage.Instance.Save();
            }
        }

        /*
        private RelayCommand _addFromAllRulesCommand;

        /// <summary>
        /// Gets the AddFromAllRulesCommand.
        /// </summary>
        public RelayCommand AddFromAllRulesCommand
        {
            get
            {
                return _addFromAllRulesCommand
                       ?? (_addFromAllRulesCommand = new RelayCommand(
                           () =>
                           {
                               var view = new SelectRuleView {FolderParent = this};
                               var w = WindowUtil.CreateWindow(view, "SelectRule".Local());
                               if (w.ShowDialog() != true) return;
                               var rule = view.SelectedRuleViewModel.Rule;
                               var ruleVm = new RuleViewModel(this, rule, RuleProperty.Create(Rules.Count, rule.Guid));
                               Rules.Add(ruleVm);
                               ruleVm.EditCommand.Execute(null);
                           },
                           () => true));
            }
        }
        */

        private RelayCommand<bool> _setEnabledCommand;

        /// <summary>
        /// Gets the SetEnabledCommand.
        /// </summary>
        public RelayCommand<bool> SetEnabledCommand
        {
            get
            {
                return _setEnabledCommand
                       ?? (_setEnabledCommand = new RelayCommand<bool>(
                           p =>
                           {
                               Enabled = p;
                           }));
            }
        }

        private RelayCommand _addRuleCommand;

        /// <summary>
        /// Gets the AddRuleCommand.
        /// </summary>
        public RelayCommand AddRuleCommand
        {
            get
            {
                return _addRuleCommand
                       ?? (_addRuleCommand = new RelayCommand(
                           () =>
                           {
                               var rule = Rule.Create();
                               var ruleVm = new RuleViewModel(this, rule, RuleProperty.Create(Rules.Count, rule.Guid));
                               var w = WindowUtil.CreateRuleWindow(ruleVm, "NewRule".Local());
                               if (w.ShowDialog() == true)
                               {
                                   Rules.Add(ruleVm);
                               }
                           }));
            }
        }

        private RelayCommand<RuleViewModel> _saveRuleCommand;

        /// <summary>
        /// Gets the SaveRuleCommand.
        /// </summary>
        public RelayCommand<RuleViewModel> SaveRuleCommand
        {
            get
            {
                return _saveRuleCommand
                       ?? (_saveRuleCommand = new RelayCommand<RuleViewModel>(
                           p =>
                           {
                               p.Save();
                               DataStorage.Instance.Rules[p.Rule.Guid] = p.Rule;
                               if (!Folder.RuleProperties.Any(_ => _.RuleGuid.Equals(p.Rule.Guid)))
                               {
                                   Folder.RuleProperties.Add(p.Property);
                               }
                               DataStorage.Instance.Save();
                               Executor.ClearCache(p.Rule.Guid);
                           },
                           p => true));
            }
        }

        private RelayCommand<RuleViewModel> _deleteRuleCommand;

        /// <summary>
        /// Gets the DeleteRuleCommand.
        /// </summary>
        public RelayCommand<RuleViewModel> DeleteRuleCommand
        {
            get
            {
                return _deleteRuleCommand
                       ?? (_deleteRuleCommand = new RelayCommand<RuleViewModel>(
                           p =>
                           {
                               Rules.Remove(p);
                               Folder.RuleProperties.RemoveAll(_ => _.RuleGuid.Equals(p.Rule.Guid));
                               DataStorage.Instance.Rules.Remove(p.Rule.Guid);
                               DataStorage.Instance.Save();
                           },
                           p => p != null));
            }
        }

        private RelayCommand<RuleViewModel> _moveDownRuleCommand;

        /// <summary>
        /// Gets the MoveDownRuleCommand.
        /// </summary>
        public RelayCommand<RuleViewModel> MoveDownRuleCommand
        {
            get
            {
                return _moveDownRuleCommand
                       ?? (_moveDownRuleCommand = new RelayCommand<RuleViewModel>(
                           p =>
                           {
                               var index = Rules.IndexOf(p);
                               Rules.Move(index, index + 1);
                               var tmp = Folder.RuleProperties[index].Priority;
                               Folder.RuleProperties[index].Priority =
                                   Folder.RuleProperties[index + 1].Priority;
                               Folder.RuleProperties[index + 1].Priority = tmp;
                               DataStorage.Instance.Save();
                           },
                           p => p != null && Rules.IndexOf(p) < Rules.Count - 1));
            }
        }

        private RelayCommand<RuleViewModel> _moveUpRuleCommand;

        /// <summary>
        /// Gets the MoveDownRuleCommand.
        /// </summary>
        public RelayCommand<RuleViewModel> MoveUpRuleCommand
        {
            get
            {
                return _moveUpRuleCommand
                       ?? (_moveUpRuleCommand = new RelayCommand<RuleViewModel>(
                           p =>
                           {
                               var index = Rules.IndexOf(p);
                               Rules.Move(index, index - 1);
                               var tmp = Folder.RuleProperties[index].Priority;
                               Folder.RuleProperties[index].Priority =
                                   Folder.RuleProperties[index - 1].Priority;
                               Folder.RuleProperties[index - 1].Priority = tmp;
                               DataStorage.Instance.Save();
                           },
                           p => p != null && Rules.IndexOf(p) > 0));
            }
        }

        /// <summary>
        /// Initializes a new instance of the FolderViewModel class.
        /// </summary>
        public FolderViewModel(Folder f, IEnumerable<RuleViewModel> allRuls)
        {
            Folder = f;
            Exist = Directory.Exists(f.FolderPath);
            if (allRuls != null)
            {
                // Due to some unknown reasons, "orderby" in LINQ didn't work.
                // So I used OrderBy method here.
                // Notes: Item with lower Priority value is actual prior. 
                var items = (from rm in allRuls
                    let property = f.RuleProperties.FirstOrDefault(rp => rp.RuleGuid.Equals(rm.Rule.Guid))
                    where property != null
                    select new RuleViewModel(this, rm.Rule, property)).OrderBy(_ => _.Property.Priority);
                Rules = new ObservableCollection<RuleViewModel>(items);
            }
            else
            {
                Rules = new ObservableCollection<RuleViewModel>();
            }
            Rules.CollectionChanged += Rules_CollectionChanged;
        }

        public void CopyRule(Rule rule)
        {
            if (Rules.Any(_=>_.Name == rule.Name)) return;

            var ruleClone = (Rule)rule.Clone();
            ruleClone.Guid = Guid.NewGuid();
            var newRuleVm = new RuleViewModel(this, ruleClone,
                RuleProperty.Create(Rules.Count, ruleClone.Guid));

            Rules.Add(newRuleVm);
            SaveRuleCommand.Execute(newRuleVm);
        }

        private void Rules_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                e.NewItems.OfType<RuleViewModel>().ForEach(r => Executor.Instance.RuleCacheMap.Put(r.Rule.Guid, r.Rule));
            if (e.OldItems != null)
                e.OldItems.OfType<RuleViewModel>().ForEach(r => Executor.Instance.RuleCacheMap.Remove(r.Rule.Guid));
        }
    }
}