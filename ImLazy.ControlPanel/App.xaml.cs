using ImLazy.ControlPanel.ViewModel;
using log4net;
using System.Windows;
using LogManager = ImLazy.RunTime.LogManager;

namespace ImLazy.ControlPanel
{
	/// <summary>
	/// App.xaml 的交互逻辑
	/// </summary>
	public partial class App : Application
	{
        static ILog Log = LogManager.GetLogger(typeof(App));
        private static ViewModelLocator _locator;
        public static ViewModelLocator Locator
        {
            get
            {
                if (_locator == null)
                {
                    _locator = new ViewModelLocator();
                }
                return _locator;
            }
        }
        static App()
        {
            Log.Info("App started.");
        }

        ~App()
        {
            Log.Info("App finished.");
        }
	}
}
