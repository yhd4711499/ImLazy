using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.NotMatchRegexVerb")]
    class NotMatchRegexVerb : MatchRegexVerb
    {
        public override string Name
        {
            get { return "NotMatchRegexVerb"; }
        }

        protected override bool GetResult(string a, string b)
        {
            return !base.GetResult(a, b);
        }
    }
}
