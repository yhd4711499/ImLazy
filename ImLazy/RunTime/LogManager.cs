using System;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace ImLazy.RunTime
{
    public static class LogManager
    {
        static LogManager()
        {
            InitLogger();
        }
        static void InitLogger()
        {
/*            var fa = new FileAppender
            {
                File = Path.Combine(AppEnvironment.LocalStorageFolder, "log.log"),
                Layout = new PatternLayout("%d[%t]%-5p %c [%x] - %m%n")
            };
            fa.ActivateOptions();
            BasicConfigurator.Configure(fa
                , (new ConsoleAppender { Layout = new PatternLayout("%d[%t]%-5p %c [%x] - %m%n") }));*/
        }

        public static ILog GetLogger(Type type)
        {
            return log4net.LogManager.GetLogger(type);
        }

        public static ILog GetLogger(String name)
        {
            return log4net.LogManager.GetLogger(name);
        }
    }
}
