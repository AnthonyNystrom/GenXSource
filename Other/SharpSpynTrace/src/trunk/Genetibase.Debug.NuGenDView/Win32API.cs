using System;
using System.Runtime.InteropServices;

namespace Genetibase.Debug
{
	internal sealed class Win32API
	{
		public struct SECURITY_ATTRIBUTES 
		{
			public int nLength;
			public IntPtr lpSecurityDescriptor;
			public bool bInheritHandle; 
		}

		public struct SECURITY_DESCRIPTOR
		{
			public byte Revision;
			public byte Sbz1;
			public int Control;
			public int Owner;
			public int Group;
			public ACL Sacl;
			public ACL Dacl;
		}

		public struct ACL
		{
			public byte AclRevision;
			public byte Sbz1;
			public short AclSize;
			public short AceCount;
			public short Sbz2;
		}

		public const int SECURITY_DESCRIPTOR_REVISION = 1;
		public const int ERROR_ALREADY_EXISTS = 183;
		public const int PAGE_READWRITE = 4;
		public const int FILE_MAP_READ = 4;
		public const int WAIT_OBJECT_0 = 0;

		[DllImport("advapi32", SetLastError=true)] 
		public static extern bool InitializeSecurityDescriptor(IntPtr pSecurityDescriptor, int dwRevision);

		[DllImport("advapi32", SetLastError=true)] 
		public static extern bool SetSecurityDescriptorDacl(IntPtr pSecurityDescriptor, bool bDaclPresent, IntPtr pDacl, bool bDaclDefaulted);

		[DllImport("kernel32", SetLastError = true)] 
		public static extern int CreateEvent(ref SECURITY_ATTRIBUTES lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

		[DllImport("kernel32")] 
		public static extern int CloseHandle(int hObject);

		[DllImport("kernel32", SetLastError=true)] 
		public static extern int CreateFileMapping(int hFile, ref SECURITY_ATTRIBUTES lpFileMappigAttributes, int flProtect, int dwMaximumSizeHigh, int dwMaximumSizeLow, string lpName);

		[DllImport("kernel32", SetLastError=true)] 
		public static extern IntPtr MapViewOfFile(int hFileMappingObject, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, int dwNumberOfBytesToMap);

		[DllImport("kernel32")] 
		public static extern int UnmapViewOfFile(IntPtr lpBaseAddress);

		[DllImport("kernel32")] 
		public static extern int SetEvent(int hEvent);

		[DllImport("kernel32")] 
		public static extern int WaitForSingleObject(int hHandle, int dwMilliseconds);

		[DllImport("kernel32")] 
		public static extern void CopyMemory(int Destination, int Source, int Length);

		private Win32API()
		{
		}
	}
}
