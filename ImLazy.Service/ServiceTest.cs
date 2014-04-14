using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace WindowsServiceTest
{
	public partial class ServiceTest : ServiceBase
	{
        Timer timer;

		public ServiceTest()
		{
			InitializeComponent();
		}

        private bool TryInitTimer()
        {
            bool result = false;
            try
            {
                timer = new Timer(2000.0);
                timer.Elapsed += timer_Tick;
                timer.Enabled = true;
                Log("Timer initiated.");
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Log("Error in TryInitTimer() : {0}", ex.Message);
            }
            return result;
        }


        void timer_Tick(object sender, ElapsedEventArgs e)
        {
            Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "tick.");
        }

        private void Log(string format, params object[] args)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\log.txt", true))
            {
                sw.WriteLine(format, args);
            }
        }

		protected override void OnStart(string[] args)
		{
            Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "Start.");
            if (TryInitTimer())
                timer.Start();
            else
            {
                this.Stop();
            }
		}

		protected override void OnStop()
		{
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
            Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "Stop.");
		}

        protected override void OnPause()
        {
            base.OnPause();
            if (timer != null)
            {
                timer.Stop();
            }
        }

        protected override void OnContinue()
        {
            base.OnContinue();
            if (timer != null)
            {
                timer.Start();
            }
        }
	}
}
