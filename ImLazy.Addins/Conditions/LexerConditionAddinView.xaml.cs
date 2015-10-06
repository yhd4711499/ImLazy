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
                ObjectContent.Visibility = Visibility.Collapsed;
                ObjectContent.Content = null;
                _objectTypeString = null;
            }
            else
            {
                var dic = _configuration == null ? 
                    new SerializableDictionary<string, object>() : new SerializableDictionary<string, object>(_configuration);
                var obj = objects.FirstOrDefault().Value;
                ObjectContent.Visibility = Visibility.Visible;
                ObjectContent.Content = obj.CreateMainView(dic, verb.GetObjectType(verbType));
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
            if (verbs == null || !verbs.Any())
            {
                CmbVerbs.Visibility = Visibility.Collapsed;
                ObjectContent.Visibility = Visibility.Collapsed;
                ObjectContent.Content = null;
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
                if (ObjectContent.Content == null) return _configuration;
                _configuration.Add(ConfigNames.Object,
                    _objectTypeString);
                var config = ((IEditView) ObjectContent.Content).Configuration;
                config.ForEach(_=>_configuration[_.Key] = _.Value);
                return _configuration;
            }
            set
            {
                if (_configuration == value)
                    return;
                if(_configuration == null)
                    CmbSubjects.SelectedIndex = 0;
                _isDirty = true;
                _configuration = value;
                UpdateUi();
            }
        }
    }
}
