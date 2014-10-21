using System;
using ImLazy.ControlPanel.ViewModel;
using ImLazy.Runtime;
using log4net;
using LogManager = ImLazy.Runtime.LogManager;
using System.Windows;

namespace ImLazy.ControlPanel
{
	/// <summary>
	/// App.xaml 的交互逻辑
	/// </summary>
	public partial class App
	{
        static readonly ILog Log = LogManager.GetLogger(typeof(App));
        private static ViewModelLocator _locator;
        public static ViewModelLocator Locator
        {
            get { return _locator ?? (_locator = new ViewModelLocator()); }
        }
        static App()
        {
            Log.Info("Controal panel started.");
            try
            {
                AppEnvironment.InitRegistry(AppDomain.CurrentDomain.BaseDirectory);
            }
            catch (Exception e)
            {
                Log.Error("InitRegistry failed : ", e);
                MessageBox.Show("初次使用本软件，请以管理员身份运行。\nPlease run as administrator at first run of this app.");
                Environment.Exit(-1);
            }
        }

        ~App()
        {
            Log.Info("Controal panel finished.");
        }
	}
}
