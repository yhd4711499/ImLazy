using System;
using System.ServiceProcess;

namespace ImLazy.Service
{
    public class ServiceStatus
    {
        public ServiceStatus(ServiceStatusEnum status)
        {
            Construct(status);
        }

        public ServiceStatus(ServiceControllerStatus status)
        {
            ServiceStatusEnum s;
            if (Enum.TryParse(status.ToString(), out s))
                Construct(s);
            else
            {
                throw new ArgumentException("status");
            }
        }

        private void Construct(ServiceStatusEnum status)
        {
            Status = status;
            IsPending = status.ToString().Contains("Pending");
        }
        public ServiceStatusEnum Status { get; private set; }
        public bool IsPending { get; private set; } 

        public override string ToString()
        {
            return Status.ToString();
        }
    }

    public enum ServiceStatusEnum
    {

        // 摘要: 
        //     服务未运行。 这对应于 Win32 SERVICE_STOPPED 常数，该常数定义为 0x00000001。
        Stopped = 1,
        //
        // 摘要: 
        //     服务正在启动。 这对应于 Win32 SERVICE_START_PENDING 常数，该常数定义为 0x00000002。
        StartPending = 2,
        //
        // 摘要: 
        //     服务正在停止。 这对应于 Win32 SERVICE_STOP_PENDING 常数，该常数定义为 0x00000003。
        StopPending = 3,
        //
        // 摘要: 
        //     服务正在运行。 这对应于 Win32 SERVICE_RUNNING 常数，该常数定义为 0x00000004。
        Running = 4,
        //
        // 摘要: 
        //     服务即将继续。 这对应于 Win32 SERVICE_CONTINUE_PENDING 常数，该常数定义为 0x00000005。
        ContinuePending = 5,
        //
        // 摘要: 
        //     服务即将暂停。 这对应于 Win32 SERVICE_PAUSE_PENDING 常数，该常数定义为 0x00000006。
        PausePending = 6,
        //
        // 摘要: 
        //     服务已暂停。 这对应于 Win32 SERVICE_PAUSED 常数，该常数定义为 0x00000007。
        Paused = 7,
        InstallPending,
        Installed,
        UninstallPending,
        Uninstalled,
    }
}
