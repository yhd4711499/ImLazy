using System;
using System.Collections.Generic;
using log4net;
using log4net.Config;

namespace ImLazy.RunTime
{
    public static class LogManager
    {
        private static readonly Dictionary<Type, ILog> CachedLogs; 
        static LogManager()
        {
            CachedLogs = new Dictionary<Type, ILog>();
            InitLogger();
        }
        static void InitLogger()
        {
            XmlConfigurator.Configure();
        }

        public static ILog GetLogger(Type type)
        {
            ILog log;
            if (CachedLogs.TryGetValue(type, out log))
                return log;
            log = log4net.LogManager.GetLogger(type);
            CachedLogs[type] = log;
            return log;
        }

        public static ILog GetLogger(this object obj)
        {
            return GetLogger(obj.GetType());
        }
    }
}
