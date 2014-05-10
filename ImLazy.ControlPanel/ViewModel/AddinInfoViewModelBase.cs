using GalaSoft.MvvmLight;
using ImLazy.Data;
using ImLazy.RunTime;
using ImLazy.SDK.Base.Contracts;

namespace ImLazy.ControlPanel.ViewModel
{
    /*public abstract class AddinInfoViewModelBase<TAddinInfo, TParent> : 
        ViewModelBase
        where TAddinInfo:IAddinInfo
    {
        public TParent Parent { get; protected set; }

        private readonly TAddinInfo _addinInfo;

        private IEditView _mainView;

        protected AddinInfoViewModelBase(TAddinInfo addinInfo, TParent parent)
        {
            _addinInfo = addinInfo;
            Parent = parent;
        }

        public IEditView MainView
        {
            get
            {
                if (_mainView != null) return _mainView;
                var creator = CacheMap<object>.ViewCreatorCacheMap.Get(AddinInfo.AddinType);
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
        }

        public TAddinInfo AddinInfo
        {
            get { return _addinInfo; }
        }
        public virtual void Save()
        {
            AddinInfo.Save(MainView.Configuration);
        }
    }*/

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
                var creator = CacheMap<object>.ViewCreatorCacheMap.Get(AddinInfo.AddinType);
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