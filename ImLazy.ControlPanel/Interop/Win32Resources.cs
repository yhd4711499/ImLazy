using System.ComponentModel;
using System.Runtime.InteropServices;
using ImLazy.ControlPanel.Util;
// Copyright (c) Sven Groot (Ookii.org) 2006
// See license.txt for details
using System;
using System.Text;

namespace ImLazy.ControlPanel.Interop
{
    class Win32Resources : IDisposable
    {
        private SafeModuleHandle _moduleHandle;
        private const int _bufferSize = 500;

        public Win32Resources(string module)
        {
            _moduleHandle = NativeMethods.LoadLibraryEx(module, IntPtr.Zero, NativeMethods.LoadLibraryExFlags.LoadLibraryAsDatafile);
            if( _moduleHandle.IsInvalid )
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public string LoadString(uint id)
        {
            CheckDisposed();

            var buffer = new StringBuilder(_bufferSize);
            if( NativeMethods.LoadString(_moduleHandle, id, buffer, buffer.Capacity + 1) == 0 )
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return buffer.ToString();
        }

        public string FormatString(uint id, params string[] args)
        {
            CheckDisposed();

            var buffer = IntPtr.Zero;
            var source = LoadString(id);

            // For some reason FORMAT_MESSAGE_FROM_HMODULE doesn't work so we use this way.
            var flags = NativeMethods.FormatMessageFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER | NativeMethods.FormatMessageFlags.FORMAT_MESSAGE_ARGUMENT_ARRAY | NativeMethods.FormatMessageFlags.FORMAT_MESSAGE_FROM_STRING;

            var sourcePtr = Marshal.StringToHGlobalAuto(source);
            try
            {
                if( NativeMethods.FormatMessage(flags, sourcePtr, id, 0, ref buffer, 0, args) == 0 )
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            finally
            {
                Marshal.FreeHGlobal(sourcePtr);
            }

            var result = Marshal.PtrToStringAuto(buffer);
            // FreeHGlobal calls LocalFree
            Marshal.FreeHGlobal(buffer);

            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if( disposing )
                _moduleHandle.Dispose();
        }

        private void CheckDisposed()
        {
            if( _moduleHandle.IsClosed )
            {
                throw new ObjectDisposedException("Win32Resources");
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion    
    }
}
