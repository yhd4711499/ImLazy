using System;
using System.Windows;
using System.Windows.Interactivity;
using ImLazy.SDK.Util;

namespace ImLazy.ControlPanel.Interactions
{
    public class ShellExecuteAction : TriggerAction<DependencyObject>
    {
        public string Command { get; set; }

        protected override void Invoke(object parameter)
        {
            var cmd = parameter as string;
            if(cmd == null)return;
            ShellUtil.ShellExecute(IntPtr.Zero, "open", cmd, null, null, (int)ShellUtil.ShowWindowCommands.SW_NORMAL);
        }
    }
}
