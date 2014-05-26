﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ImLazy.RunTime;
using ImLazy.SDK.Base.Contracts;
using ImLazy.Util;

namespace ImLazy.Addins.ContentViews
{
    class TextContent : TextBox, IEditView
    {
        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                return new SerializableDictionary<string, object>
                {
                    {ConfigNames.ObjectValue,Text}
                };
            }
            set
            {
                Text = value.TryGetValue<string>(ConfigNames.ObjectValue);
            }
        }
    }
}
