using System;
using System.Diagnostics;
using ImLazy.SDK.Exceptions;

namespace ImLazy.SDK.Util
{
    public static class CmdUtil
    {
        private static Exception GetCmdException(int code)
        {
            switch (code)
            {
                case 1060:
                    return new NotPrivilligedException();
            }
            return new UnknownException();
        }

        public static void Run(ProcessStartInfo startInfo)
        {
            var process = new Process
            {
                StartInfo = startInfo
            };
            if (!process.Start())
                throw new UnknownException();
            process.WaitForExit();
            if (process.ExitCode != 0)
                throw GetCmdException(process.ExitCode);
        }
    }
}
