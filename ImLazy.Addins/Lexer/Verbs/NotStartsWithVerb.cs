using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.NotStartsWithVerb")]
    public class NotStartsWithVerb : StartsWithVerb
    {
        public override string Name
        {
            get { return "NotStartsWithVerb"; }
        }

        protected override bool GetResult(string a, string b)
        {
            return !base.GetResult(a, b);
        }
    }
}
