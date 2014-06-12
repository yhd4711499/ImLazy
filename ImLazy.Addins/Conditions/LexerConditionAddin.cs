using System.Collections.Generic;
using System.ComponentModel.Composition;
using ImLazy.Addins.Lexer.Objects;
using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using ImLazy.Runtime;
using ImLazy.SDK.Util;
using ImLazy.Util;
using log4net;
using LogManager = ImLazy.Runtime.LogManager;

namespace ImLazy.Addins.Conditions
{
    [ExportMetadata("Type", typeof(LexerConditionAddin))]
    [Export(typeof(IConditionAddin))]
    class LexerConditionAddin : IConditionAddin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (LexerConditionAddin));
        private static readonly string LocalNameStatic = "LexerConditionAddin".Local();

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new LexerConditionAddinView{ Configuration = config };
        }

        public string LocalName { get { return LocalNameStatic; } }
        public bool IsMatch(string filePath, SerializableDictionary<string, object> dic)
        {
            // get all configs
            var subject = dic.TryGetValue<string>(ConfigNames.Subject);
            var verb = dic.TryGetValue<string>(ConfigNames.Verb);
            var obj = dic.TryGetValue<string>(ConfigNames.Object);
            var value = dic.TryGetValue<string>(ConfigNames.ObjectValue);
            if (!Log.CheckParams("One or more config(s) is null : ", subject, verb))
                return false;

            if (obj == null)
                obj = typeof (NullObject).FullName;

            // get function from caches
            var fs = LexerAddinHost.Instance.SubjectsCacheMap.Get(subject);
            var fv = LexerAddinHost.Instance.VerbsCacheMap.Get(verb);
            var fo = LexerAddinHost.Instance.ObjectsCacheMap.Get(obj);
            if (!Log.CheckParams("One or more function(s) is null : ", fs, fv, fo))
                return false;
            // check
            var property = fs(filePath);
            var objValue = fo(value);
            var isMatch = fv(property, objValue);

            return isMatch;
        }
    }
}
