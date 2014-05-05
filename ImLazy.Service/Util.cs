using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using ImLazy.RunTime;

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
            var process = new Process
            {
                StartInfo = { UseShellExecute = false, FileName = "Install.bat", CreateNoWindow = true }
            };
            process.Start();
            process.WaitForExit();
        }

        public static void Uninstall()
        {
            var process = new Process
            {
                StartInfo = { UseShellExecute = false, FileName = "Uninstall.bat", CreateNoWindow = true }
            };
            if(!process.Start())
                throw new Exception();
        }

        public static ServiceStatus CheckStatus()
        {
            var ctl = ServiceController.GetServices()
               .FirstOrDefault(s => s.ServiceName == Constaints.ServiceName);
            if (ctl == null)
                return new ServiceStatus(ServiceStatusEnum.Uninstalled);
            else
                return new ServiceStatus(ctl.Status);
        }

        public static void Start()
        {
            var serviceController = new ServiceController(Constaints.ServiceName);
            if (serviceController.Status == ServiceControllerStatus.Stopped)
            {
                serviceController.Start();
                while (serviceController.Status != ServiceControllerStatus.Running)
                {
                    serviceController.Refresh();
                    Thread.Sleep(100);
                }
                //LblStatus.Text = "服务已启动";
            }
            else
            {
                //LblStatus.Text = "此时无法启动服务，请稍后重试";
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
            else
            {
                
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
                else
                {
                    //LblStatus.Text = "服务未处于暂停和启动状态";
                }
            }
            else
            {
                //LblStatus.Text = "服务不能暂停";
            }

        }

        readonly static Executor Executor = new Executor(
                        CacheMap<object>.ConditionCacheMap,
                        CacheMap<object>.ActionCacheMap,
                        CacheMap<object>.RuleCacheMap
                        );
        public static void Execute()
        {
            Executor.Execute(DataStorage.Instance.Folders);
        }
    }
}
