using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using ImLazy.Runtime;
using ImLazy.SDK.Exceptions;
using ImLazy.SDK.Util;

namespace ImLazy.Service
{
    public static class Util
    {
        public static Task DoAsync(Action action)
        {
            return Task.Factory.StartNew(action);
        }

        public static void Install()
        {
            ShellUtil.Run(new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = "Install.bat",
                CreateNoWindow = true
            });
        }

        public static void Uninstall()
        {
            ShellUtil.Run(new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = "Uninstall.bat",
                CreateNoWindow = true
            });
        }

        public static ServiceStatus CheckStatus()
        {
            var ctl = ServiceController.GetServices()
               .FirstOrDefault(s => s.ServiceName == Constaints.ServiceName);
            if (ctl == null)
                return new ServiceStatus(ServiceStatusEnum.Uninstalled);
            return new ServiceStatus(ctl.Status);
        }

        public static void Start()
        {
            var serviceController = new ServiceController(Constaints.ServiceName);
            if (serviceController.Status == ServiceControllerStatus.Stopped)
            {
                try
                {
                    serviceController.Start();
                }
                catch (Exception ex)
                {
                    var win32Ex = ex.InnerException as Win32Exception;
                    if (win32Ex != null && win32Ex.NativeErrorCode == 5)
                    {
                        // not privilliged
                        throw new NotPrivilligedException();
                    }
                    throw;
                }
                
                while (serviceController.Status != ServiceControllerStatus.Running)
                {
                    serviceController.Refresh();
                    Thread.Sleep(100);
                }
                //LblStatus.Text = "服务已启动";
            }
        }

        public static void Stop()
        {
            var serviceController = new ServiceController(Constaints.ServiceName);
            if (serviceController.CanStop)
            {
                serviceController.Stop();
                while (serviceController.Status != ServiceControllerStatus.Stopped)
                {
                    serviceController.Refresh();
                    Thread.Sleep(100);
                }
            }
        }

        public static void Pause()
        {
            var serviceController = new ServiceController(Constaints.ServiceName);
            if (serviceController.CanPauseAndContinue)
            {
                if (serviceController.Status == ServiceControllerStatus.Running)
                {
                    serviceController.Pause();
                    //LblStatus.Text = "服务已暂停";
                }
                else if (serviceController.Status == ServiceControllerStatus.Paused)
                {
                    serviceController.Continue();
                    //LblStatus.Text = "服务已继续";
                }
            }
        }

        readonly static Executor Executor = Executor.Instance;
        public static void Execute()
        {
            Executor.Execute(DataStorage.Instance.Folders);
        }
    }
}
