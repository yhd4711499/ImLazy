using ImLazy.Contracts;
using System;
using System.Collections.Generic;
using ImLazy.Util;
using ImLazy.Addins.ContentViews;

namespace ImLazy.Addins
{
    class ContentProviderAddin : IAddin
    {
        public const string TargetType = "TargetType";

        static readonly Dictionary<string, Func<IEditView>> Creators = new Dictionary<string, Func<IEditView>>
        {
            {typeof(string).FullName, ()=> new TextContent()},
            {typeof(long).FullName, ()=> new LongContent()},
            {typeof(bool).FullName, ()=> new BoolContent()}
        };

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            var type = config.TryGetValue<string>(TargetType);
            Func<IEditView> c;
            if (!Creators.TryGetValue(type, out c)) return null;
            var v = c();
            v.Configuration = config;
            return v;
        }
    }
}
