using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.StartsWithVerb")]
    public class StartsWithVerb : StringVerbBase
    {
        public override string Name
        {
            get { return "StartsWithVerb"; }
        }

        protected override bool GetResult(string a, string b)
        {
            return a.StartsWith(b);
        }
    }
}
