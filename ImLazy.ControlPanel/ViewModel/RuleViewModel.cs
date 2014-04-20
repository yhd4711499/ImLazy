using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ImLazy.ControlPanel.Views;
using ImLazy.Data;
using System.Windows;

namespace ImLazy.ControlPanel.ViewModel
{
    /// <summary>
    /// To hold the Rule data in Folder.
    /// </summary>
    public class RuleViewModel : ViewModelBase
    {
        public const string NamePropertyName = "Name";
        public string Name
        {
            get { return Rule.Name; }
            set
            {
                if (Rule.Name == value)
                    return;
                Rule.Name = value;
                RaisePropertyChanged(NamePropertyName);
                SaveCommand.RaiseCanExecuteChanged();
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
                return Property.Enabled;
            }

            set
            {
                if (Property.Enabled == value)
                {
                    return;
                }

                Property.Enabled = value;
                RaisePropertyChanged(EnabledPropertyName);
            }
        }

        public FolderViewModel FolderParent { get; private set; }
        public RuleProperty Property { get; private set; }
        public Rule Rule { get; private set; }

        /// <summary>
        /// Use a ViewModel instead of ConditionCorp data class to provide UI interaction for future use.
        /// </summary>
        public ObservableCollection<ConditionCorpViewModel> Conditions { get; set; }
        public ObservableCollection<ActionViewModel> Actions { get; private set; }

        #region Commands

        private RelayCommand _saveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                       ?? (_saveCommand = new RelayCommand(
                           () => FolderParent.SaveRuleCommand.Execute(this),
                           () =>
                           {
                               if (string.IsNullOrEmpty(Rule.Name))
                                   return false;
                               return true;
                           }));
            }
        }

        private RelayCommand _editCommand;

        /// <summary>
        /// Gets the AddRuleCommand.
        /// </summary>
        public RelayCommand EditCommand
        {
            get
            {
                return _editCommand
                       ?? (_editCommand = new RelayCommand(
                           () =>
                           {
                               var memento = GetMemento() as Memento;
                               var ruleVm = new RuleViewModel(FolderParent, memento.Rule, memento.Property);
                               var w = new Window
                               {
                                   ShowActivated = true,
                                   Title = "New Rule",
                                   Content = new RuleDetailView
                                   {
                                       DataContext = ruleVm
                                   }
                               };
                               if (w.ShowDialog() == true)
                               {
                                   FolderParent.SaveRuleCommand.Execute(ruleVm);
                               }
                           }));
            }
        }

        private RelayCommand _newActionCommand;

        /// <summary>
        /// Gets the AddActionCommand.
        /// </summary>
        public RelayCommand NewActionCommand
        {
            get
            {
                return _newActionCommand
                       ?? (_newActionCommand = new RelayCommand(() => NewAction()));
            }
        }

        private RelayCommand<ActionViewModel> _deleteActionCommand;

        /// <summary>
        /// Gets the DeleteActionModel.
        /// </summary>
        public RelayCommand<ActionViewModel> DeleteActionCommand
        {
            get
            {
                return _deleteActionCommand
                       ?? (_deleteActionCommand = new RelayCommand<ActionViewModel>(
                           p =>
                           {
                               Actions.Remove(p);
                               if (_addPendingActions != null)
                               {
                                   _addPendingActions.Remove(p.AddinInfo as ActionAddinInfo);
                                   //var d = _addPendingActions.Where(_ => _.Equals(p.ActionAddinInfo)).FirstOrDefault();
                                   //if (d != null)
                                   //{
                                   //    _addPendingActions.Remove(d);
                                   //    return;
                                   //}
                               }
                               Rule.Actions.RemoveAll(_ => _.Equals(p.AddinInfo));
                           }));
            }
        }

        #endregion

        private List<ActionAddinInfo> _addPendingActions;

        /// <summary>
        /// Initializes a new instance of the RuleViewModel class.
        /// </summary>
        public RuleViewModel(Rule rule)
        {
            Rule = rule;
            Actions = new ObservableCollection<ActionViewModel>();
            Conditions = new ObservableCollection<ConditionCorpViewModel>()
            {
                new ConditionBranchViewModel(rule.ConditionBranch, null)
            };
            rule.Actions.ForEach(_ => Actions.Add(new ActionViewModel(_, this)));
            if (Actions.Count == 0)
            {
                NewAction();
            }
        }

        /// <summary>
        /// Create an <see cref="ActionViewModel"/> with default <see cref="ActionAddinInfo"/>
        /// </summary>
        /// <returns>An instance of <see cref="ActionViewModel"/></returns>
        protected ActionViewModel NewAction()
        {
            if (_addPendingActions == null)
                _addPendingActions = new List<ActionAddinInfo>();
            var ac = new ActionAddinInfo();
            var vm = new ActionViewModel(ac, this);
            _addPendingActions.Add(ac);
            Actions.Add(vm);
            return vm;
        }

        public void Save()
        {
            Conditions.ForEach(_=>_.Save());
            Actions.ForEach(_ => _.Save());
            if (_addPendingActions == null) return;
            if (Actions == null)
            {
                throw new Exception("Unknown state!");
            }
            _addPendingActions.ForEach(_ => Rule.Actions.Add(_));
            _addPendingActions = null;
        }

        public RuleViewModel(FolderViewModel folderParent,Rule rule, RuleProperty p):this(rule)
        {
            FolderParent = folderParent;
            Property = p;

        }

        #region Memento

        private object GetMemento()
        {
            return new Memento(this);
        }

        private void SetMemento(object mementoObj)
        {
            var memento = mementoObj as Memento;
            if (memento == null) throw new ArgumentNullException("memento");
            Property = memento.Property;
        }

        private class Memento
        {
            public readonly RuleProperty Property;
            public readonly Rule Rule;
            public Memento(RuleViewModel rm)
            {

                Property = rm.Property.Clone() as RuleProperty;
                Rule = rm.Rule.Clone() as Rule;
            }
        }

        #endregion
    }
}
