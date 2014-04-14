using System;

namespace ImLazy.Util
{
    public static class EnumHelper
    {
        public static T Parse<T>(string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }
    }
}
