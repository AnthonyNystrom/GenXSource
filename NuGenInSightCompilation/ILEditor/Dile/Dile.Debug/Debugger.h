#pragma once

#include <vcclr.h>
#include "IDebugEventHandler.h"
#include "ManagedEventHandler.h"
#include "ProcessWrapper.h"
//#include "UnmanagedEventHandler.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		public ref class Debugger
		{
		private:
			static ULONG32 DefaultCharArraySize = 255;

			ICorDebug* debug;
			ManagedEventHandler* managedEventHandler;
			//UnmanagedEventHandler* unmanagedEventHandler;
			IDebugEventHandler^ debugEventHandler;

			static void CheckHResult(HRESULT hResult);

		public:
			void Initialize(IDebugEventHandler^ debugEventHandler, String^ frameworkVersion);
			
			void CreateProcess(System::String^ applicationName, System::String^ arguments, System::String^ currentDirectory);

			void DebugActiveProcess(DWORD id, BOOL win32Attach);

			ProcessWrapper^ GetProcess(UInt32 processID);

			void Release();

			static String^ GetVersionFromProcess(IntPtr^ processHandle);

			static String^ GetRequestedRuntimeVersion(String^ executablePath);
		};
	}
}