using System;
using ImLazy.ControlPanel.ViewModel;
using ImLazy.Runtime;
using log4net;
using LogManager = ImLazy.Runtime.LogManager;

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
            AppEnvironment.InitRegistry(AppDomain.CurrentDomain.BaseDirectory);
        }

        ~App()
        {
            Log.Info("Controal panel finished.");
        }
	}
}
