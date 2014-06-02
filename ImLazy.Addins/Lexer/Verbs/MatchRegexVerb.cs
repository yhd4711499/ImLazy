using System;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using ImLazy.SDK.Exceptions;
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
            bool match;
            try
            {
                match = Regex.IsMatch(a, b);
            }
            catch
            {
                throw new CheckedException(ErrorCodeDefinitions.ErrRegexpIllFormated);
            }
            return match;
        }
    }
}
