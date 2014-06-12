using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.NotEndsWithVerb")]
    public class NotEndsWithVerb : EndsWithVerb
    {
        public override string Name
        {
            get { return "NotEndsWithVerb"; }
        }

        protected override bool GetResult(string a, string b)
        {
            return !base.GetResult(a, b);
        }
    }
}
