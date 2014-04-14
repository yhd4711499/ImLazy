using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using ImLazy.Data;
using ImLazy.ControlPanel.Util;
using ImLazy.RunTime;

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
        public ObservableCollection<RuleViewModel> Rules { get; private set; }
        public ObservableCollection<ConditionCorpViewModel> Conditions { get; private set; }
        public ObservableCollection<ActionViewModel> Actions { get; private set; }
        public ObservableCollection<FolderViewModel> Folders { get; private set; }

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
                                              if (d.ShowDialog() == true)
                                              {
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
                                          }));
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