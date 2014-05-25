using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ImLazy.ControlPanel.Converters;
using ImLazy.ControlPanel.Util;
using ImLazy.SDK.Exceptions;
using ImLazy.Service;

namespace ImLazy.ControlPanel.ViewModel
{
    public class ControlPanelViewModel:ViewModelBase
    {
        /// <summary>
        /// Status属性的名称
        /// </summary>
        public const string StatusPropertyName = "Status";
        private Status _status = new Status(StatusPropertyName);
        /// <summary>
        /// 状态
        /// </summary>
        public Status Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaisePropertyChanged(StatusPropertyName);
                }
            }
        }


        /// <summary>
        /// ExecutionStatus属性的名称
        /// </summary>
        public const string ExecutionStatusPropertyName = "ExecutionStatus";

        private Status _executionStatus = new Status(ExecutionStatusPropertyName) { Message = "" };
        /// <summary>
        /// 
        /// </summary>
        public Status ExecutionStatus
        {
            get
            {
                return _executionStatus;
            }
            set
            {
                if (_executionStatus != value)
                {
                    _executionStatus = value;
                    RaisePropertyChanged(ExecutionStatusPropertyName);
                }
            }
        }

        /// <summary>
        /// ServiceStatus属性的名称
        /// </summary>
        public const string ServiceStatusPropertyName = "ServiceStatus";
        private ServiceStatus _serviceStatus;
        /// <summary>
        /// 作者很懒，什么描述也没有
        /// </summary>
        public ServiceStatus ServiceStatus
        {
            get
            {
                return _serviceStatus;
            }
            set
            {
                if (_serviceStatus != value)
                {
                    _serviceStatus = value;
                    RaisePropertyChanged(ServiceStatusPropertyName);
                    StartCommand.RaiseCanExecuteChanged();
                    StopCommand.RaiseCanExecuteChanged();
                    InstallCommand.RaiseCanExecuteChanged();
                    UninstallCommand.RaiseCanExecuteChanged();
                }
            }
        }
        
         

        private RelayCommand _startCommand;

        /// <summary>
        /// Gets the StartCommand.
        /// </summary>
        public RelayCommand StartCommand
        {
            get
            {
                return _startCommand
                       ?? (_startCommand = new RelayCommand(
                           () => CommonAction("StartPending", "Running", "StartFailed", Service.Util.Start),
                           () => !IfAny(ServiceStatusEnum.Uninstalled, ServiceStatusEnum.Running)));
            }
        }


        private RelayCommand _stopCommand;

        /// <summary>
        /// Gets the StopCommand.
        /// </summary>
        public RelayCommand StopCommand
        {
            get
            {
                return _stopCommand
                       ?? (_stopCommand = new RelayCommand(
                       ()=> CommonAction("StopPending", "Stopped", "StopFailed",Service.Util.Stop),
                       ()=> !IfAny(ServiceStatusEnum.Uninstalled, ServiceStatusEnum.Stopped)));
            }
        }

        private RelayCommand _installCommand;

        /// <summary>
        /// Gets the InstallCommand.
        /// </summary>
        public RelayCommand InstallCommand
        {
            get
            {
                return _installCommand
                       ?? (_installCommand = new RelayCommand(
                       ()=>CommonAction("InstallPending", "Installed", "InstallFailed",Service.Util.Install),
                       ()=>!IfAny(ServiceStatusEnum.Installed)));
            }
        }

        private RelayCommand _uninstallCommand;

        /// <summary>
        /// Gets the UninstallCommand.
        /// </summary>
        public RelayCommand UninstallCommand
        {
            get
            {
                return _uninstallCommand
                       ?? (_uninstallCommand = new RelayCommand(
                           () => CommonAction("UninstallPending", "Uninstalled", "UninstallFailed", Service.Util.Uninstall),
                           () => !IfAny(ServiceStatusEnum.Uninstalled)));
            }
        }

        private RelayCommand _executeCommand;

        /// <summary>
        /// Gets the ExecuteCommand.
        /// </summary>
        public RelayCommand ExecuteCommand
        {
            get
            {
                return _executeCommand
                       ?? (_executeCommand = new RelayCommand(
                           async () =>
                           {
                               UpdateStatus("Executing", false, null, ExecutionStatus);
                               var s = Stopwatch.StartNew();
                               try
                               {
                                   await Service.Util.DoAsync(Service.Util.Execute);
                               }
                               catch (Exception ex)
                               {
                                   UpdateStatus("ExecuteFailed", true, ex.Message, ExecutionStatus);
                               }
                               finally
                               {
                                   s.Stop();
                               }
                               UpdateStatus(String.Format("Execution done，花费了{0}毫秒", s.ElapsedMilliseconds), false, null,
                                   ExecutionStatus);
                           },
                           () => true));
            }
        }

        public ControlPanelViewModel()
        {
            Init();
        }

        private async void Init()
        {
            UpdateStatus("Initiating");
            ServiceStatus = await Task.FromResult(Service.Util.CheckStatus());
            UpdateStatus(ServiceStatus.ToString());
        }

        private async void UpdateStatus(string msg, int errorCode, Status status = null)
        {
            var st = status ?? Status;
            if (st.Update(msg.Local(), true, errorCode.LocalError()))
                RaisePropertyChanged(st.PropertyName);
            ServiceStatus = await Task.FromResult(Service.Util.CheckStatus());
        }

        private async void UpdateStatus(string msg, bool isError = false, string detailInfo = null, Status status = null)
        {
            var st = status ?? Status;
            if (st.Update(msg.Local(), isError, detailInfo.Local()))
                RaisePropertyChanged(st.PropertyName);
            ServiceStatus = await Task.FromResult(Service.Util.CheckStatus());
        }

        private bool IfAny(params ServiceStatusEnum[] status)
        {
            return ServiceStatus.IsPending || status.Any(_ => _ == ServiceStatus.Status);
        }

        private async void CommonAction(string beforeMsg, string aftMsg, string errorMsg, Action a)
        {
            UpdateStatus(beforeMsg);
            try
            {
                await Service.Util.DoAsync(a);
                UpdateStatus(aftMsg);
            }
            catch (BaseException ex)
            {
                UpdateStatus(errorMsg, ex.GetErrorCode());
            }
        }
    }
}
