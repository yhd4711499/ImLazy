using System;

namespace ImLazy.Data
{
    public class WalkthroughResult
    {
        public WalkthroughResult(string entryName, string ruleName, string folder)
        {
            EntryName = entryName;
            RuleName = ruleName;
            Folder = folder;
        }
        public string Folder { get; private set; }
        public string EntryName { get; private set; }
        public string RuleName { get; private set; }

        public override string ToString()
        {
            return String.Format("{0}\t\t{1}", EntryName, RuleName);
        }
    }
}
