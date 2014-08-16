using System;

namespace ImLazy.SDK.Exceptions
{
    [Serializable]
    public class UnknownException : BaseException
    {
        public UnknownException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public override int GetErrorCode()
        {
            return ErrorCodeDefinitions.ErrUnknown;
        }
    }
}
