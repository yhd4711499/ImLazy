using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImLazy.Addins.Utils;
using ImLazy.Contracts;
using ImLazy.RunTime;
using ImLazy.SDK.Lexer;
using ImLazy.Util;
using WpfLocalization;

namespace ImLazy.Addins.Conditions
{
    [ExportMetadata("Type", typeof(LexerConditionAddin))]
    [Export(typeof(IConditionAddin))]
    class LexerConditionAddin : IConditionAddin
    {
        private static readonly string LocalNameStatic = "LexerConditionAddin".Local();

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            return new LexerConditionAddinView{ Configuration = config };
        }

        public string LocalName { get { return LocalNameStatic; } }
        public bool IsMatch(string filePath, SerializableDictionary<string, object> dic)
        {
            var subject = dic.TryGetValue<string>(ConfigNames.Subject);
            var verb = dic.TryGetValue<string>(ConfigNames.Verb);
            var obj = dic.TryGetValue<string>(ConfigNames.Object);
            var value = dic.TryGetValue<string>(ConfigNames.ObjectValue);

            var fs = CacheMap<object>.SubjectsCacheMap.Get(subject);
            var fv = CacheMap<object>.VerbsCacheMap.Get(verb);
            var fo = CacheMap<object>.ObjectsCacheMap.Get(obj);

            var property = fs(filePath);
            var objValue = fo(value);
            var isMatch = fv(property, objValue);

            return isMatch;
        }
    }
}
