using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.ContainsVerb")]
    public class ContainsVerb : StringVerbBase
    {
        public override string Name
        {
            get { return "ContainsVerb"; }
        }

        protected override bool GetResult(string a, string b)
        {
            return a.Contains(b);
        }
    }
}
