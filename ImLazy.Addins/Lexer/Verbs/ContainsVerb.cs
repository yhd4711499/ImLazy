using System;
using System.ComponentModel.Composition;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.ContainsVerb")]
    public class ContainsVerb:IVerb
    {
        public string ElementType
        {
            get { return "string"; }
        }

        public string Name
        {
            get { return "ContainsVerb"; }
        }

        public string GetObjectType(string verbType)
        {
            return "string";
        }

        public bool IsMatch(object property, object value)
        {
            throw new NotImplementedException();
        }
    }
}
