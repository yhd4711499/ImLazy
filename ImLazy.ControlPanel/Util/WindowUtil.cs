﻿using System.Collections.Generic;
using System.Windows;
using ImLazy.ControlPanel.ViewModel;
using ImLazy.ControlPanel.Views;

namespace ImLazy.ControlPanel.Util
{
    public static class WindowUtil
    {
        public static Window CreateRuleWindow(RuleViewModel ruleVm, string title)
        {
            var w = new Window
            {
                MaxHeight = 768,
                MinHeight = 200,
                Width = 600,
                MinWidth = 500,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowActivated = true,
                Title = title,
                Content = new RuleDetailView
                {
                    DataContext = ruleVm
                }
            };
            w.MaxWidth = w.Width + 140;
            w.Height = (ruleVm.Rule.ConditionBranch.Sum() + 2 + ruleVm.Rule.Actions.Sum()) * 50 + 80;
            return w;
        }
    }
}