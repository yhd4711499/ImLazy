using ImLazy.ControlPanel.ViewModel;
using ImLazy.RunTime;
using System.Windows;
using log4net;
using LogManager = ImLazy.RunTime.LogManager;

namespace ImLazy.ControlPanel
{
	/// <summary>
	/// App.xaml 的交互逻辑
	/// </summary>
	public partial class App : Application
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
            AppEnvironment.InitRegistry(System.AppDomain.CurrentDomain.BaseDirectory);
        }

        ~App()
        {
            Log.Info("Controal panel finished.");
        }
	}
}
