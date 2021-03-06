﻿using System.Collections.Generic;
using ImLazy.SDK.Base.Contracts;
using System.Windows.Controls;
using ImLazy.Runtime;
using ImLazy.Util;

namespace ImLazy.Addins.ContentViews
{
    class LongContent : TextBox , IEditView
    {
        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                return new SerializableDictionary<string, object>
                {
                    {ConfigNames.TargetObject, Text}
                };
            }
            set
            {
                Text = value.TryGetValue(ConfigNames.TargetObject) as string;
            }
        }
    }
}
