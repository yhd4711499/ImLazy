using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using ImLazy.ControlPanel.Views;
using ImLazy.Data;
using ImLazy.ControlPanel.Util;
using ImLazy.Runtime;

namespace ImLazy.ControlPanel.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region View models

// ReSharper disable MemberCanBePrivate.Global
        public ObservableCollection<RuleViewModel> Rules { get; private set; }
        public ObservableCollection<ConditionCorpViewModel> Conditions { get; private set; }
        public ObservableCollection<ActionViewModel> Actions { get; private set; }
        public ObservableCollection<FolderViewModel> Folders { get; private set; }
// ReSharper restore MemberCanBePrivate.Global
        #endregion

        #region Commands

        private RelayCommand _addFolderCommand;

        /// <summary>
        /// Gets the AddFolderCommand.
        /// </summary>
        public RelayCommand AddFolderCommand
        {
            get
            {
                return _addFolderCommand
                    ?? (_addFolderCommand = new RelayCommand(
                                          () =>
                                          {
                                              var d = new VistaFolderBrowserDialog();
                                              if (d.ShowDialog() != true) return;
                                              if (Folders.Select(_ => _.Folder.FolderPath).Contains(d.SelectedPath))
                                              {
                                                  MessageBox.Show("已存在相同的目录。");
                                              }
                                              else
                                              {
                                                  var f = Folder.Create();
                                                  f.FolderPath = d.SelectedPath;
                                                  Folders.Add(new FolderViewModel(f, null));
                                                  DataStorage.Instance.Folders.Add(f);
                                                  DataStorage.Instance.Save();
                                              }
                                          }));
            }
        }

        private RelayCommand<FolderViewModel> _deleteFolderCommnad;

        /// <summary>
        /// Gets the DeleteFolderCommand.
        /// </summary>
        public RelayCommand<FolderViewModel> DeleteFolderCommand
        {
            get
            {
                return _deleteFolderCommnad
                       ?? (_deleteFolderCommnad = new RelayCommand<FolderViewModel>(
                           p =>
                           {
                               Folders.Remove(p);
                               DataStorage.Instance.Folders.RemoveAll(_ => _.FolderPath.Equals(p.Folder.FolderPath));
                               DataStorage.Instance.Save();
                           },
                           p => p != null));
            }
        }

        private RelayCommand<FolderViewModel> _walkthroughCommnad;

        /// <summary>
        /// Gets the WalkthroughCommnad.
        /// </summary>
        public RelayCommand<FolderViewModel> WalkthroughCommnad
        {
            get
            {
                return _walkthroughCommnad
                       ?? (_walkthroughCommnad = new RelayCommand<FolderViewModel>(
                           async p =>
                           {
                               var results = await Executor.Instance.Walkthrough(p.Folder);
                               var view = new WalkthroughResultsView();
                               view.SetResults(results);
                               WindowUtil.CreateWindow(view, "WalkthroughResults".Local(), 550).ShowDialog();
                           },
                           p => p != null));
            }
        }

        private RelayCommand _walkthroughAllCommnad;

        /// <summary>
        /// Gets the WalkthroughCommnad.
        /// </summary>
        public RelayCommand WalkthroughAllCommnad
        {
            get
            {
                return _walkthroughAllCommnad
                       ?? (_walkthroughAllCommnad = new RelayCommand(() =>
                           WindowUtil.CreateWindow(new WalkthroughResultsView(), "WalkthroughResults".Local(), 550).ShowDialog(),
                           () => true));
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
                           ruleVm =>
                           {
                               Rules.Remove(ruleVm);
                               DataStorage.Instance.Rules.Remove(ruleVm.Property.RuleGuid);
                           },
                           ruleVm => ruleVm != null));
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Conditions = new ObservableCollection<ConditionCorpViewModel>(
                DataStorage.Instance.Conditions.Select(
                    c => new ConditionCorpViewModel(c, null)
                )
            );

            Rules = new ObservableCollection<RuleViewModel>(
                DataStorage.Instance.Rules.Select(
                    r => new RuleViewModel(r.Value)
                )
            );

            Actions = new ObservableCollection<ActionViewModel>(
                DataStorage.Instance.Actions.Select(
                    r => new ActionViewModel(r, null)
                )
            );

            Folders = new ObservableCollection<FolderViewModel>(
                DataStorage.Instance.Folders.Select(
                    f => new FolderViewModel(f, Rules)
                )
            );
        }
    }
}