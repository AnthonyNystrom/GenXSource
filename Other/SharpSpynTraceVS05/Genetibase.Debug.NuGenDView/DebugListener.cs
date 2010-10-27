using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Genetibase.Debug
{
    public sealed class DebugListener
    {
        private const int SHARED_MEMORY_BUFFER_SIZE = 4096; 

        private IntPtr _pSecurityDescriptor;
        private int _hAckEvent;
        private int _hReadyEvent;
        private int _hSharedFile;
        private IntPtr _nativePointer;
        private bool _running;
        private Thread _thread;

        public event DebugMessageAvailable DebugMessageAvailable;

        public DebugListener()
        {
            _pSecurityDescriptor = IntPtr.Zero;		
            _hAckEvent = 0;
            _hReadyEvent = 0;
            _hSharedFile = 0;
            _nativePointer = IntPtr.Zero;
            _running = false;
        }

        public void Start()
        {
            if(_running)
            {
                return;
            }

            Win32API.SECURITY_DESCRIPTOR sd = new Win32API.SECURITY_DESCRIPTOR();
            Win32API.SECURITY_ATTRIBUTES sa = new Win32API.SECURITY_ATTRIBUTES();

            try
            {
                _pSecurityDescriptor = Marshal.AllocHGlobal(Marshal.SizeOf(sd));
            }
            catch(System.Exception)
            {
                throw new Exception("Unable to allocate global buffer for SECURITY_DESCRIPTOR");
            }

            Marshal.StructureToPtr(sd, _pSecurityDescriptor, false);
            sa.bInheritHandle = true;
            sa.nLength = Marshal.SizeOf(sa);
            sa.lpSecurityDescriptor = _pSecurityDescriptor;

            if(!Win32API.InitializeSecurityDescriptor(_pSecurityDescriptor, Win32API.SECURITY_DESCRIPTOR_REVISION))
            {
                throw new Exception("Unable to initialise security descriptor, error code: " + Marshal.GetLastWin32Error());
            }
	
            if(!Win32API.SetSecurityDescriptorDacl(_pSecurityDescriptor, true, IntPtr.Zero, false))
            {
                throw new Exception("Unable to set security descriptor dacl, error code: " + Marshal.GetLastWin32Error());
            }

            _hAckEvent = Win32API.CreateEvent(ref sa, false, false, "DBWIN_BUFFER_READY");
            if(_hAckEvent == 0)
            {
                throw new Exception("Unable to create synchronisation object DBWIN_BUFFER_READY");
            }

            if(Marshal.GetLastWin32Error() == Win32API.ERROR_ALREADY_EXISTS)
            {
                throw new Exception("Another instance of a debug listener is already running");
            }

            _hReadyEvent = Win32API.CreateEvent(ref sa, false, false, "DBWIN_DATA_READY");
            if(_hReadyEvent == 0)
            {
                throw new Exception("Unable to create synchronisation object DBWIN_DATA_READY");
            }

            _hSharedFile = Win32API.CreateFileMapping(-1, ref sa, Win32API.PAGE_READWRITE,  0, 4096, "DBWIN_BUFFER");
            if(_hSharedFile == 0)
            {
                throw new Exception("Unable to create file mapping object DBWIN_BUFFER");
            }

            _nativePointer = Win32API.MapViewOfFile(_hSharedFile, Win32API.FILE_MAP_READ,  0, 0, SHARED_MEMORY_BUFFER_SIZE);
            if(_nativePointer == IntPtr.Zero)
            {
                throw new Exception("Unable to map shared memory");
            }

            _thread = new Thread(new ThreadStart(this.ThreadProc));
            _thread.Priority = ThreadPriority.Highest;
            _thread.Start();

            _running = true;
        }

        public void Stop()
        {
            if(_running)
            {
                _running = false;
                _thread.Join(10000);
            }
		
            if(_pSecurityDescriptor != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_pSecurityDescriptor);
                _pSecurityDescriptor = IntPtr.Zero;
            }

            if(_hAckEvent != 0)
            {
                Win32API.CloseHandle(_hAckEvent);
                _hAckEvent = 0;
            }

            if(_hReadyEvent != 0)
            {
                Win32API.CloseHandle(_hReadyEvent);
                _hReadyEvent = 0;
            }

            if(_nativePointer != IntPtr.Zero)
            {
                Win32API.UnmapViewOfFile(_nativePointer);
            }

            if(_hSharedFile != 0)
            {
                Win32API.CloseHandle(_hSharedFile);
                _hSharedFile = 0;
            }
        }

        private void ThreadProc()
        {
            // I'm not sure why, but unless we sleep for a bit we don't get
            // any messages ... investigate and tell me why ...
            Thread.Sleep(100);
            Win32API.SetEvent(_hAckEvent);
            while(_running)
            {
                int ret = Win32API.WaitForSingleObject(_hReadyEvent, 1000);
                if(ret == Win32API.WAIT_OBJECT_0)
                {
                    string message = GetStringFromNativeBuffer();
                    OnDebugMessageAvailable(message);
                    Win32API.SetEvent(_hAckEvent);
                }
            }
        }

        private unsafe string GetStringFromNativeBuffer()
        {
            IntPtr destinationBuffer = Marshal.AllocHGlobal(SHARED_MEMORY_BUFFER_SIZE - 4);
            byte * pNativeBuffer = (byte *)_nativePointer.ToPointer();
            int * pPid = (int *)pNativeBuffer;
            byte * pMessage = pNativeBuffer + 4;

            void * pDest = destinationBuffer.ToPointer();
            Win32API.CopyMemory((int)pDest, (int)pMessage, SHARED_MEMORY_BUFFER_SIZE - 4);
		
            string message = Marshal.PtrToStringAnsi(destinationBuffer);
            if(message.EndsWith("\n"))
            {
                message = message.Substring(0, message.Length - 1);
            }

            Marshal.FreeHGlobal(destinationBuffer);
            return message;
        }

        private void OnDebugMessageAvailable(string message)
        {
            if(DebugMessageAvailable != null)
            {
                DebugMessageAvailable(this, new DebugMessageArgs(message));
            }
        }
    }
}