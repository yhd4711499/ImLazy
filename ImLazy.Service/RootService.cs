using ImLazy.RunTime;
using log4net;
using System;
using System.ServiceProcess;
using System.Timers;
using LogManager = ImLazy.RunTime.LogManager;

namespace ImLazy.Service
{
	public partial class RootService : ServiceBase
	{
        Timer _timer;
        private static readonly ILog Log = LogManager.GetLogger(typeof(RootService));
	    private static readonly Executor Executor;

	    static RootService()
	    {
	        Executor = new Executor(
                CacheMap<object>.ConditionCacheMap,
                CacheMap<object>.ActionCacheMap,
                CacheMap<object>.RuleCacheMap
                );
	    }
		public RootService()
		{
			InitializeComponent();
		}

        private bool TryInitTimer()
        {
            bool result;
            try
            {
                _timer = new Timer(2000.0);
                _timer.Elapsed += timer_Tick;
                _timer.Enabled = true;
                Log.Debug("Timer initiated.");
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Log.Error("Error in TryInitTimer()", ex);
            }
            return result;
        }


        void timer_Tick(object sender, ElapsedEventArgs e)
        {
            Log.Debug("Executing...");
            Executor.Execute(DataStorage.Instance.Folders);
        }

		protected override void OnStart(string[] args)
		{
            Log.Debug("Timer Starting...");
		    if (TryInitTimer())
		    {
		        _timer.Start();
                Log.Debug("Timer started.");
		    }
		    else
		    {
		        Stop();
		    }
		}

		protected override void OnStop()
		{
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
            Log.Debug("Timer stopped.");
		}

        protected override void OnPause()
        {
            base.OnPause();
            if (_timer != null)
            {
                _timer.Stop();
                Log.Debug("Timer stopped.");
            }
        }

        protected override void OnContinue()
        {
            base.OnContinue();
            if (_timer != null)
            {
                _timer.Start();
                Log.Debug("Timer started.");
            }
        }
	}
}
