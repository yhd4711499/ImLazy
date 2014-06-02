using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace ImLazy.ControlPanel.Behaviors
{
    class ShortkeyCommand : Behavior<Window>
    {
        private Dictionary<Key, Action<Window>> _dictionary;

        public void AddAction(Key key, Action<Window> action)
        {
            _dictionary[key] = action;
        }

        public ShortkeyCommand()
        {
            _dictionary = new Dictionary<Key, Action<Window>>();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += AssociatedObject_KeyDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
            base.OnDetaching();
        }

        void AssociatedObject_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Action<Window> action;
            if (!_dictionary.TryGetValue(e.Key, out action)) return;
            var window = sender as Window;
            if(window == null)return;
            action(window);
        }
    }
}
