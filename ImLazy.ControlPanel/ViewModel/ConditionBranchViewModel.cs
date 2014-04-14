using GalaSoft.MvvmLight.Command;
using ImLazy.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ImLazy.ControlPanel.ViewModel
{
    /// <summary>
    /// An ConditionCorpViewModel derived from <see cref="ConditionCorpViewModel"/> to present and interact with <see cref="ConditionBranch"/>
    /// </summary>
    public class ConditionBranchViewModel : ConditionCorpViewModel
    {
        //private ConditionCorp _conditionBranchAs;
        public ConditionBranch ConditionCorp
        {
            get { return base.AddinInfo as ConditionBranch; }
        }

        public new ConditionBranchViewModel Parent
        {
            get { return base.Parent as ConditionBranchViewModel; }
            set { base.Parent = value; }
        }

        public ObservableCollection<ConditionCorpViewModel> SubConditions { get; protected set; }

        /// <summary>
        /// Create an instance of <see cref="ConditionBranchViewModel"/><para/>A new instance of <see cref="ConditionCorpViewModel"/> will be added to <see cref="SubConditions"/> as default.
        /// </summary>
        /// <param name="conditionCorp">an existed <see cref="ConditionCorp"/> instance or a new() one </param>
        /// <param name="parent">parent</param>
        public ConditionBranchViewModel(ConditionBranch conditionCorp, ConditionBranchViewModel parent)
            : base(conditionCorp, parent)
        {
            // Set this ViewModel as Parent if no parent.
            if (Parent == null)
                Parent = this;

            if (!ConditionCorp.SubConditions.Any())
            {
                // No SubConditions in ConditionCorp. So a default list will be created.
                // And then a new instance of ConditionCorpViewModel will be added as default.
                SubConditions = new ObservableCollection<ConditionCorpViewModel>();
                NewCondition(false);
            }
            else
            {
                // ConditionCorp is fully configurated. So all of it's SubConditions will be added
                // to the SubConditions of this ViewModel as ConditionCorpViewModel.
                SubConditions = new ObservableCollection<ConditionCorpViewModel>(conditionCorp.SubConditions.Select(CreateViewModel));
            }
        }

        ConditionCorpViewModel CreateViewModel(ConditionCorp c)
        {
            var nc = c as ConditionBranch;
            
            if (nc == null || nc.SubConditions == null || !nc.SubConditions.Any())
                return new ConditionLeafViewModel(c as ConditionLeaf, this);
            return new ConditionBranchViewModel(nc, this);
        }

        private List<ConditionCorp> _addPendingConditions;

        private RelayCommand _newConditionLeafCommand;

        /// <summary>
        /// Gets the NewConditionLeafCommand.
        /// </summary>
        public RelayCommand NewConditionLeafCommand
        {
            get
            {
                return _newConditionLeafCommand
                    ?? (_newConditionLeafCommand = new RelayCommand(() => NewCondition(false)));
            }
        }

        /// <summary>
        /// Create an <see cref="ConditionCorpViewModel"/> or <see cref="ConditionBranchViewModel"/> with default <see cref="ConditionCorp"/>
        /// </summary>
        /// <param name="isBranch">true if you want to create a <see cref="ConditionBranchViewModel"/>, otherwise false</param>
        /// <returns>An instance of <see cref="ConditionCorpViewModel"/> or <see cref="ConditionBranchViewModel"/></returns>
        private ConditionCorpViewModel NewCondition(bool isBranch)
        {
            if (_addPendingConditions == null)
                _addPendingConditions = new List<ConditionCorp>();
            ConditionCorpViewModel vm;
            ConditionCorp c;
            if (isBranch)
            {
                c = new ConditionBranch()
                {
                    //Parent = ConditionCorp
                };
                vm = new ConditionBranchViewModel(c as ConditionBranch, this);
            }
            else
            {
                c = new ConditionLeaf();
                vm = new ConditionLeafViewModel(c as ConditionLeaf, this);
            }
            _addPendingConditions.Add(c);
            SubConditions.Add(vm);
            return vm;
        }

        /// <summary>
        /// Delete an sub ConditionCorp VM from SubConditions and related ConditionCorp data from<para/>
        /// the SubConditions of the ConditionCorp data contained in this VM.<para/>
        /// And also delete from the pending list if neccesary.
        /// </summary>
        /// <param name="p"></param>
        protected void DeleteCondition(ConditionCorpViewModel p)
        {
            SubConditions.Remove(p);
            if (SubConditions.Count == 0)
            {
                if (Parent != null && Parent != this)
                    Parent.DeleteCondition(this);
            }
            if (_addPendingConditions != null)
            {
                _addPendingConditions.Remove(p.AddinInfo as ConditionCorp);
 
            }
            ConditionCorp.SubConditions.RemoveAll(_ => _.Equals(p.AddinInfo));
        }

        private RelayCommand _newConditionBranchCommand;

        /// <summary>
        /// Gets the NewConditionLeafCommand.
        /// </summary>
        public RelayCommand NewConditionBranchCommand
        {
            get
            {
                return _newConditionBranchCommand
                    ?? (_newConditionBranchCommand = new RelayCommand(() =>
                    {
                        NewCondition(true); ;
                    }));
            }
        }

        private RelayCommand<ConditionCorpViewModel> _deleteConditionCommand;

        /// <summary>
        /// Gets the DeleteConditionCommand.
        /// </summary>
        public RelayCommand<ConditionCorpViewModel> DeleteConditionCommand
        {
            get
            {
                return _deleteConditionCommand
                    ?? (_deleteConditionCommand = new RelayCommand<ConditionCorpViewModel>(DeleteCondition));
            }
        }

        /// <summary>
        /// Load configs for every ConditionCorp and it's sub conditions if possible from it's MainView.
        /// </summary>
        public override void Save()
        {
            if (_addPendingConditions != null)
            {
                if (ConditionCorp.SubConditions == null)
                {
                    throw new Exception("Unknown state!");
                }
                else
                {
                    _addPendingConditions.ForEach(_ => ConditionCorp.Add(_));
                }

                _addPendingConditions = null;
            }

            if (SubConditions != null)
                SubConditions.ForEach(_ => _.Save());
            base.Save();
        }
    }
}
