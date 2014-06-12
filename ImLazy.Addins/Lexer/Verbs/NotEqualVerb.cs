using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.NotEqualVerb")]
    public class NotEqualVerb : EqualVerb
    {
        public override string Name
        {
            get { return "NotEqualVerb"; }
        }

        public override bool IsMatch(object subject, object value)
        {
            return !base.IsMatch(subject, value);
        }
    }
}
