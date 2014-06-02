namespace ImLazy.SDK.Exceptions
{
    public static class ErrorCodeDefinitions
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        public const int ErrUnknown = 0x00000000;
        /// <summary>
        /// 权限不够
        /// </summary>
        public const int ErrNotPrivilliged = 0x00000001;
        /// <summary>
        /// 正则表达式错误
        /// </summary>
        public const int ErrRegexpIllFormated = 0x00000002;
    }
}
