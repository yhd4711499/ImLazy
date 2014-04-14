using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ImLazy.ControlPanel.Views;
using ImLazy.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ImLazy.RunTime;

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

// ReSharper disable once MemberCanBePrivate.Global
        public ObservableCollection<RuleViewModel> Rules { get; private set; }


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
                                              var rule = new RuleViewModel(this, Rule.Create(), new RuleProperty { Enabled = true });
                                              var w = new Window
                                              {
                                                  ShowActivated = true,
                                                  Title = "New Rule",
                                                  Content = new RuleDetailView {DataContext = rule}
                                              };
                                              if (w.ShowDialog() == true)
                                              {
                                                  Rules.Add(rule);
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
                                                  Folder.RuleProperties.Add(new RuleProperty { Enabled = true, RuleGuid = p.Rule.Guid });
                                              }
                                              DataStorage.Instance.Save();
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
// ReSharper disable once UnusedVariable
                                              //DataStorage.Instance.Rules.Remove()
                                              DataStorage.Instance.Rules.RemoveAll(_ => _.Equals(p.Rule.Guid));
                                              DataStorage.Instance.Save();
                                          }));
            }
        }
        /// <summary>
        /// Initializes a new instance of the FolderViewModel class.
        /// </summary>
        public FolderViewModel(Folder f, IEnumerable<RuleViewModel> allRuls)
        {
            Folder = f;
            Rules = new ObservableCollection<RuleViewModel>();
            if (allRuls != null)
            {
                foreach (var rm in allRuls)
                {
                    var property = f.RuleProperties.FirstOrDefault(rp => rp.RuleGuid.Equals(rm.Rule.Guid));
                    if (property != null)
                    {
                        Rules.Add(new RuleViewModel(this, rm.Rule, property));
                    }
                }
            }
        }

/*
        public void Save()
        {
            //Folder.sa
        }
*/

        public override string ToString()
        {
            return Folder.ToString();
        }
    }
}