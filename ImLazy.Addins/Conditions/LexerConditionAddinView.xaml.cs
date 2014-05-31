using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using ImLazy.Runtime;
using ImLazy.SDK.Lexer;
using ImLazy.Util;

namespace ImLazy.Addins.Conditions
{
    /// <summary>
    /// LexerConditionAddinView.xaml 的交互逻辑
    /// </summary>
    public partial class LexerConditionAddinView : IEditView
    {
        private string _objectTypeString;
        private bool _isDirty = true;
        public LexerConditionAddinView()
        {
            InitializeComponent();

            CmbSubjects.SelectionChanged += CmbSubjectsOnSelectionChanged;
            CmbVerbs.SelectionChanged += CmbVerbsOnSelectionChanged;

            CmbSubjects.ItemsSource = LexerAddinHost.Instance.Subjects.Select(_=>_.Value);
            CmbSubjects.SelectedIndex = 0;
        }

        void UpdateUi()
        {
            if (_configuration == null) return;
            //顺序选择主语、谓语
            CmbSubjects.SelectItem(_configuration.TryGetValue<string>(ConfigNames.Subject));
            var name = _configuration.TryGetValue<string>(ConfigNames.Verb);
            CmbVerbs.SelectItem(name);
            //更新宾语
            UpdateObject();
        }

        /// <summary>
        /// 更新宾语
        /// </summary>
        private void UpdateObject()
        {
            if (!_isDirty)
                return;
            _isDirty = false;
            // 从Combox中获取主语和谓语类型
            // TODO:若没有谓语后的逻辑
            var verb = CmbVerbs.SelectedItem as IVerb;
            if (verb == null)
                return;

            var verbType = ((ISubject) CmbSubjects.SelectedItem).GetVerbType();

            // 获取Object对象，用它生成宾语的视图
            // 若没有Object对象，将宾语容器置null并隐藏
            var objects = LexerAddinHost.Instance.GetSupportedObjectsByVerbType(verb.GetObjectType(verbType));
            if (objects == null || !objects.Any())
            {
                Content.Visibility = Visibility.Collapsed;
                Content.Content = null;
                _objectTypeString = null;
            }
            else
            {
                Content.Visibility = Visibility;
                var dic = new SerializableDictionary<string, object>();
                if (_configuration != null)
                {
                    _configuration.ForEach(_ => dic.Add(_.Key, _.Value));
                }
                var obj = objects.FirstOrDefault().Value;
                Content.Content = obj.CreateMainView(dic);
                _objectTypeString = obj.GetType().ToString();
            }
        }

        private void CmbVerbsOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            _isDirty = true;
            UpdateObject();
        }

        private void CmbSubjectsOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (selectionChangedEventArgs.AddedItems.Count == 0)
                return;
            var subject = selectionChangedEventArgs.AddedItems[0] as ISubject;
            if (subject == null)
            {
                Debugger.Break();
                return;
            }

            _isDirty = true;

            var name = subject.GetVerbType();
            var verbs = LexerAddinHost.Instance.GetSupportedVerbsByType(name);
            if (!verbs.Any())
            {
                CmbVerbs.Visibility = Visibility.Collapsed;
                Content.Visibility = Visibility.Collapsed;
                Content.Content = null;
            }
            else
            {
                CmbVerbs.Visibility = Visibility.Visible;
                CmbVerbs.ItemsSource = verbs.Select(_ => _.Value);
                if (CmbVerbs.SelectedIndex == -1)
                    CmbVerbs.SelectedIndex = 0;
                UpdateObject();
            }
            
        }
        SerializableDictionary<string, object> _configuration;
        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                _configuration = new SerializableDictionary<string, object>
                {
                    {ConfigNames.Subject,CmbSubjects.SelectedItem.GetType().FullName},
                    {ConfigNames.Verb, CmbVerbs.SelectedItem.GetType().FullName},
                };
                if (Content.Content != null)
                {
                    _configuration.Add(ConfigNames.Object,
                        _objectTypeString);
                    var config = ((IEditView) Content.Content).Configuration;
                    config.ForEach(_=>_configuration.Add(_.Key, _.Value));
                    /*_configuration.Add(ConfigNames.ObjectValue,
                        ((IEditView)Content.Content).Configuration.TryGetValue(ConfigNames.ObjectValue));*/
                }
                return _configuration;
            }
            set
            {
                if (_configuration == value)
                    return;
                _isDirty = true;
                _configuration = value;
                UpdateUi();
            }
        }
    }
}
