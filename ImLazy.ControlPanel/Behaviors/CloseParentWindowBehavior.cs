using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using ImLazy.ControlPanel.Util;

namespace ImLazy.ControlPanel.Behaviors
{
    public class CloseParentWindowBehavior:Behavior<Button>
    {
        public bool? Result { private get; set; }
        Button _btn;
        protected override void OnAttached()
        {
            base.OnAttached();
            _btn = AssociatedObject;
            _btn.Click += btn_Click;
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            var w = (Window)MyVisualTreeHelper.GetParent<Window>(_btn);
            w.DialogResult = Result;
            w.Close();
        }

        protected override void OnDetaching()
        {
            _btn.Click -= btn_Click;
            base.OnDetaching();
        }
    }
}
