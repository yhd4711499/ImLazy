using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.MatchRegexVerb")]
    class MatchRegexVerb : StringVerbBase
    {
        public override string Name
        {
            get { return "MatchRegexVerb"; }
        }

        protected override bool GetResult(string a, string b)
        {
            return Regex.IsMatch(a, b);
        }
    }
}
