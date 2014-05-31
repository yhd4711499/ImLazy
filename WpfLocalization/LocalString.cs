using System;

namespace WpfLocalization
{
    public class LocalString:IEquatable<LocalString>,IEquatable<string>
    {
        public LocalString(string value, string localizedValue)
        {
            LocalizedValue = localizedValue;
            Value = value;
        }

        public string Value { get; private set; }
        public string LocalizedValue { get; private set; }

        public static implicit operator string(LocalString ls)
        {
            return ls.Value;
        }

        public static implicit operator LocalString(string s)
        {
            return new LocalString(s, null);
        }

        public bool Equals(LocalString other)
        {
            return Value.Equals(other.Value);
        }

        public bool Equals(string other)
        {
            return Value.Equals(other);
        }

        public override string ToString()
        {
            return LocalizedValue;
        }
    }
}
