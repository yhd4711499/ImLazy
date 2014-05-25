﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ImLazy.Addins.ContentViews;
using ImLazy.Addins.Lexer.Subjects;
using ImLazy.Addins.Utils;
using ImLazy.RunTime;
using ImLazy.SDK.Base.Contracts;
using ImLazy.SDK.Lexer;
using ImLazy.Util;

namespace ImLazy.Addins.Lexer.Objects
{
    [Export(typeof(IObject))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Objects.FileTypeObject")]
    class FileTypeObject : IObject
    {
        public string Name
        {
            get { return "FileTypeObject"; }
        }

        public LexerType ElementType
        {
            get { return FileTypeSubject.FileType; }
        }

        public object GetObject(string value)
        {
            return LexerType.FromFullName(value);
        }

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            config[ConfigNames.AvailableItems] = FileTypeSubject.SupportedTypes;//.Select(_ => _.Name);
            return new ComboxContent { Configuration = config, MinWidth = 50};
        }
    }
}