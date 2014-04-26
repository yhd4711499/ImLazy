using System.ServiceProcess;

namespace ImLazy.Service
{
	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		static void Main()
		{
		    var servicesToRun = new ServiceBase[] 
		    { 
		        new RootService() 
		    };
		    ServiceBase.Run(servicesToRun);
		}
	}
}
