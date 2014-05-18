using System;
using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.EndsWithVerb")]
    public class EndsWithVerb : StringVerbBase
    {
        public override string Name
        {
            get { return "EndsWithVerb"; }
        }

        protected override bool GetResult(string a, string b)
        {
            return a.EndsWith(b);
        }
    }
}
