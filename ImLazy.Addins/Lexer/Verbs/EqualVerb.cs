using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImLazy.Addins.Utils;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Verbs
{
    [Export(typeof(IVerb))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Verbs.EqualVerb")]
    public class EqualVerb : IVerb
    {
        public string ElementType
        {
            get { return "object"; }
        }

        public string Name
        {
            get { return "EqualVerb"; }
        }

        public string GetObjectType(string verbType)
        {
            return verbType;
        }

        public bool IsMatch(object property, object value)
        {
            return property.Equals(value);
        }
    }
}
