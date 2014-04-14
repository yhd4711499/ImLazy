using System;
using System.Windows;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using ImLazy.RunTime;

namespace ImLazy.ControlPanel.Views
{
    /// <summary>
    /// ServiceControlView.xaml 的交互逻辑
    /// </summary>
    public partial class ServiceControlView
    {
        public ServiceControlView()
        {
            InitializeComponent();
        }


        private void btnInstall_Click(object sender, RoutedEventArgs e)
        {
            var currentDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = currentDirectory + "\\Service";
            var process = new Process
            {
                StartInfo = {UseShellExecute = false, FileName = "Install.bat", CreateNoWindow = true}
            };
            process.Start();
            lblLog.Text = "安装成功";
            Environment.CurrentDirectory = currentDirectory;
        }

        private void btnUninstall_Click(object sender, RoutedEventArgs e)
        {
            var currentDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = currentDirectory + "\\Service";
            var process = new Process
            {
                StartInfo = {UseShellExecute = false, FileName = "Uninstall.bat", CreateNoWindow = true}
            };
            process.Start();
            lblLog.Text = "卸载成功";
            Environment.CurrentDirectory = currentDirectory;
        }

        private void btnCheckStatus_Click(object sender, RoutedEventArgs e)
        {
            var serviceController = new ServiceController("ServiceTest");
            lblCheckStatus.Text = serviceController.Status.ToString();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            var serviceController = new ServiceController("ServiceTest");
            if (serviceController.Status == ServiceControllerStatus.Stopped)
            {
                serviceController.Start();
                while (serviceController.Status != ServiceControllerStatus.Running)
                {
                    serviceController.Refresh();
                    Thread.Sleep(100);
                }
                lblStatus.Text = "服务已启动";
            }
            else
            {
                lblStatus.Text = "此时无法启动服务，请稍后重试";
            }

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            var serviceController = new ServiceController("ServiceTest");
            if (serviceController.CanStop)
            {
                serviceController.Stop();
                while (serviceController.Status != ServiceControllerStatus.Stopped)
                {
                    serviceController.Refresh();
                    Thread.Sleep(100);
                }
                lblStatus.Text = "服务已停止";
            }
            else
                lblStatus.Text = "服务不能停止";
        }

        private void btnPauseContinue_Click(object sender, RoutedEventArgs e)
        {
            var serviceController = new ServiceController("ServiceTest");
            if (serviceController.CanPauseAndContinue)
            {
                if (serviceController.Status == ServiceControllerStatus.Running)
                {
                    serviceController.Pause();
                    lblStatus.Text = "服务已暂停";
                }
                else if (serviceController.Status == ServiceControllerStatus.Paused)
                {
                    serviceController.Continue();
                    lblStatus.Text = "服务已继续";
                }
                else
                {
                    lblStatus.Text = "服务未处于暂停和启动状态";
                }
            }
            else
                lblStatus.Text = "服务不能暂停";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var executor = new Executor(
                CacheMap<object>.ConditionCacheMap,
                CacheMap<object>.ActionCacheMap,
                CacheMap<object>.RuleCacheMap
                );
                executor.Execute(DataStorage.Instance.Folders);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            
        }
    }
}
