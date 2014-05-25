using System;

namespace ImLazy.SDK.Exceptions
{
    [Serializable]
    public class UnknownException : BaseException
    {
        public override int GetErrorCode()
        {
            return ErrorCodeDefinitions.ERR_Unknown;
        }
    }
}
