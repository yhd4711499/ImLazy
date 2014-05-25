using System;

namespace ImLazy.SDK.Exceptions
{
    [Serializable]
    public class NotPrivilligedException : BaseException
    {
        public override int GetErrorCode()
        {
            return ErrorCodeDefinitions.ERR_NotPrivilliged;
        }
    }
}
