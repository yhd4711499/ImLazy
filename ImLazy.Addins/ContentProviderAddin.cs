using ImLazy.Contracts;
using System;
using System.Collections.Generic;
using ImLazy.Util;
using ImLazy.Addins.ContentViews;

namespace ImLazy.Addins
{
    class ContentProviderAddin : IAddin
    {
        static readonly Dictionary<string, Func<IEditView>> Creators = new Dictionary<string, Func<IEditView>>
        {
            {typeof(String).FullName, ()=> new TextContent()},
            {typeof(Boolean).FullName, ()=> new BoolContent()}
        };

        public IEditView CreateMainView(SerializableDictionary<string, object> config)
        {
            var type = config.TryGetValue<string>("TargetType");
            Func<IEditView> c;
            if (!Creators.TryGetValue(type, out c)) return null;
            var v = c();
            v.Configuration = config;
            return v;
        }
    }
}
