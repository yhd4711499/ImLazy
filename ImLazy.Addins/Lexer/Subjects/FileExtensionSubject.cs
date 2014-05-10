using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImLazy.Addins.Utils;
using ImLazy.SDK.Lexer;

namespace ImLazy.Addins.Lexer.Subjects
{
    [Export(typeof(ISubject))]
    [ExportMetadata("Name", "ImLazy.Addins.Lexer.Subjects.FileExtensionSubject")]
    public class FileExtensionSubject : ISubject
    {
        public string UniqueName { get { return "ImLazy.Addins.Lexer.Subjects.FileExtensionSubject"; } }

        public string ElementType { get; private set; }

        public string Name
        {
            get { return "FileExtensionSubject"; }
        }

        public string GetVerbType()
        {
            return "string";
        }

        public object GetProperty(string filePath)
        {
            return Path.GetExtension(filePath);
        }
    }
}
