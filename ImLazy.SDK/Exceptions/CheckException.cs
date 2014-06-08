using System;

namespace ImLazy.SDK.Exceptions
{
    [Serializable]
    public sealed class CheckedException : BaseException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        private readonly int _errorCode;
        public CheckedException(int errorCode)
        {
            _errorCode = errorCode;
        }

        public CheckedException(int errorCode, string message) : base(message)
        {
            _errorCode = errorCode;
        }

        public CheckedException(int errorCode, string message, Exception inner) : base(message, inner)
        {
            _errorCode = errorCode;
        }

        public override int GetErrorCode()
        {
            return _errorCode;
        }
    }
}
