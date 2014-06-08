using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.FolderNotContainsVerb")]
    class FolderNotContainsVerb : FolderContainsVerb
    {
        public override string Name
        {
            get { return "FolderNotContainsVerb"; }
        }

        public override bool IsMatch(object subject, object value)
        {
            return !base.IsMatch(subject, value);
        }
    }
}
