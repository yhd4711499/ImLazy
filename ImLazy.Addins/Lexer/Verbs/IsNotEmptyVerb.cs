using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.IsNotEmptyVerb")]
    class IsNotEmptyVerb : IsEmptyVerb
    {
        public override string Name
        {
            get { return "IsNotEmptyVerb"; }
        }

        public override bool IsMatch(object subject, object value)
        {
            return !base.IsMatch(subject, value);
        }
    }
}
