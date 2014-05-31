using System.Collections.Generic;
using System.Windows;
using ImLazy.Addins.Lexer.Objects;
using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using System.Windows.Controls;
using ImLazy.Runtime;
using ImLazy.Util;

namespace ImLazy.Addins.ContentViews
{
    class RegexTextContent:StackPanel ,IEditView
    {
        private readonly TextBox _textBox;
        private readonly CheckBox _isRegexCheckBox;
        public RegexTextContent()
        {
            _textBox = new TextBox
            {
                Margin = new Thickness(0,0,5,0)
            };
            _isRegexCheckBox = new CheckBox
            {
                Content = "Regexp".Local(),
                VerticalAlignment = VerticalAlignment.Center
            };
            Orientation = Orientation.Horizontal;
            Children.Add(_textBox);
            Children.Add(_isRegexCheckBox);

        }
        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                return new SerializableDictionary<string, object>
                {
                    {ConfigNames.ObjectValue,_textBox.Text},
                    {StringObject.ConfigNames.IsRegexp, _isRegexCheckBox.IsChecked}
                };
            }
            set
            {
                _textBox.Text = value.TryGetValue<string>(ConfigNames.ObjectValue);
                _isRegexCheckBox.IsChecked = value.TryGetValue<bool?>(StringObject.ConfigNames.IsRegexp);
            }
        }
    }
}
