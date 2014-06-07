using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ImLazy.Addins.Lexer.Verbs;
using ImLazy.Addins.Utils;
using ImLazy.Runtime;
using ImLazy.SDK.Base.Contracts;
using ImLazy.Util;
using System.Linq;
using WpfLocalization;

namespace ImLazy.Addins.ContentViews
{
    class TimeSpanContent:StackPanel,IEditView
    {
        private readonly TextBox _textBox;
        private readonly ComboBox _unitComboBox;

        private static readonly LocalString[] Units = Enum.GetNames(typeof(OlderThanVerb.Units)).Select(_=>_.LocalString()).ToArray();

        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                return new SerializableDictionary<string, object>
                {
                    {ConfigNames.ObjectValue, OlderThanVerb.ToConfigString(_textBox.Text, (string)_unitComboBox.SelectedItem)}
                };
            }
            set
            {
                var args = OlderThanVerb.GetConfigArguments(value.TryGetValue<string>(ConfigNames.ObjectValue));
                _textBox.Text = args[0];
                _unitComboBox.SelectItem(args[1]);
            }
        }

        public TimeSpanContent()
        {
            _textBox = new TextBox
            {
                Margin = new Thickness(0, 0, 5, 0),
                MinWidth = 25
            };
            _unitComboBox = new ComboBox
            {
                VerticalAlignment = VerticalAlignment.Center,
                ItemsSource = Units
            };
            Orientation = Orientation.Horizontal;
            Children.Add(_textBox);
            Children.Add(_unitComboBox);
        }
    }
}
