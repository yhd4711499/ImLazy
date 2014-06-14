using System;
using GalaSoft.MvvmLight.Command;
using ImLazy.SDK.Util;

namespace ImLazy.ControlPanel.Commands
{
    public class CommonCommands
    {
        private static RelayCommand<object> _shellExecuteCommand;

        /// <summary>
        /// Gets the ShellExecuteCommand.
        /// </summary>
        public static RelayCommand<object> ShellExecuteCommand
        {
            get
            {
                return _shellExecuteCommand
                       ?? (_shellExecuteCommand = new RelayCommand<object>(
                           p =>
                           {
                               var cmd = p as string;
                               if (cmd == null) return;
                               ShellUtil.ShellExecute(IntPtr.Zero, "open", cmd, null, null,
                                   (int) ShellUtil.ShowWindowCommands.SW_NORMAL);
                           }));
            }
        }
    }
}
