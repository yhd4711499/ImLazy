﻿using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.NotContainsVerb")]
    public class NotContainsVerb : ContainsVerb
    {
        public override string Name
        {
            get { return "NotContainsVerb"; }
        }

        protected override bool GetResult(string a, string b)
        {
            return !base.GetResult(a, b);
        }
    }
}
