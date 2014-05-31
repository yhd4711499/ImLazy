using GalaSoft.MvvmLight;
using ImLazy.Data;
using ImLazy.Runtime;
using ImLazy.SDK.Base.Contracts;

namespace ImLazy.ControlPanel.ViewModel
{
    /// <summary>
    /// Wrapper for viewmodel 
    /// <para/>It contains a <see cref="IAddinInfo"/> and provides <see cref="IEditView"/>
    /// </summary>
    public abstract class AddinInfoViewModelBase : ViewModelBase
    {
// ReSharper disable once MemberCanBeProtected.Global
        public ViewModelBase Parent { get; protected set; }

        private IEditView _mainView;
        private IAddinInfo _addinInfo;

        protected AddinInfoViewModelBase(IAddinInfo addinInfo, ViewModelBase parent)
        {
            AddinInfo = addinInfo;
            Parent = parent;
        }

        public IEditView MainView
        {
            get
            {
                if (_mainView != null) return _mainView;
                var creator = LexerAddinHost.Instance.ViewCreatorCacheMap.Get(AddinInfo.AddinType);
                if (creator == null)
                {
                    // TODO:error
                }
                else
                {
                    _mainView = creator(AddinInfo.Config);
                }
                return _mainView;
            }
            private set
            {
                if(_mainView == value)
                    return;
                _mainView = value;
                RaisePropertyChanged(()=>MainView);
            }
        }

        public void ChangeAddinType(string newType)
        {
            AddinInfo.AddinType = newType;
            MainView = null;
        }

        public IAddinInfo AddinInfo
        {
            get { return _addinInfo; }
            private set
            {
                if(_addinInfo == value)return;
                _addinInfo = value;
                MainView = null;
            }
        }

        public virtual void Save()
        {
            if (MainView != null)
                AddinInfo.Save(MainView.Configuration);
        }
    }
}