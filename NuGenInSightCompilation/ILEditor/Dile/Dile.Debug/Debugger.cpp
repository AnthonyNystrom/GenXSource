#include "stdafx.h"

#include "Debugger.h"
#include "ManagedEventHandler.h"
#include "mscoree.h"
#include "ProcessWrapper.h"

#pragma comment(lib, "corguids")

using namespace System;
using namespace System::Runtime::InteropServices;

namespace Dile
{
	namespace Debug
	{
		void Debugger::CheckHResult(HRESULT hResult)
		{
			if (FAILED(hResult))
			{
				Marshal::ThrowExceptionForHR(hResult);
			}
		}

		void Debugger::Initialize(IDebugEventHandler^ debugEventHandler, String^ frameworkVersion)
		{	
			CoInitializeEx(NULL, COINIT_MULTITHREADED);
			
			const int iDebuggerVersion = CorDebugVersion_2_0;
			IUnknown* pObject;
			BSTR frameworkVersionBstr = BSTR(::Marshal::StringToBSTR(frameworkVersion).ToPointer());
			HRESULT hResult = CreateDebuggingInterfaceFromVersion(CorDebugVersion_2_0, frameworkVersionBstr, &pObject);
			SysFreeString(frameworkVersionBstr);
			CheckHResult(hResult);
    
			ICorDebug* debugTemp;
			CheckHResult(pObject->QueryInterface(IID_ICorDebug, (void**)&debugTemp));
			debug = debugTemp;

			CheckHResult(debug->Initialize());

			managedEventHandler = new ManagedEventHandler();
			CheckHResult(debug->SetManagedHandler(managedEventHandler));

			/*unmanagedEventHandler = new UnmanagedEventHandler();
			CheckHResult(debug->SetUnmanagedHandler(unmanagedEventHandler));*/

			managedEventHandler->debugEventHandler = debugEventHandler;
		}

		void Debugger::CreateProcess(System::String^ applicationName, System::String^ arguments, System::String^ currentDirectory)
		{
			STARTUPINFOW startupInfo;
			PROCESS_INFORMATION processInfo;
			ZeroMemory(&startupInfo, sizeof(startupInfo));
			ZeroMemory(&processInfo, sizeof(processInfo));
			startupInfo.cb = sizeof(STARTUPINFOW);			

			//DWORD debugFlag = CREATE_NEW_CONSOLE;// | DEBUG_PROCESS;

			BSTR applicationNameBstr = BSTR(::Marshal::StringToBSTR(applicationName).ToPointer());
			BSTR argumentsBstr = BSTR(::Marshal::StringToBSTR(arguments).ToPointer());
			BSTR currentDirectoryBstr = BSTR(::Marshal::StringToBSTR(currentDirectory).ToPointer());			

			HRESULT hResult = debug->CreateProcess(applicationNameBstr, argumentsBstr, NULL, NULL, TRUE, 0, NULL, currentDirectoryBstr, &startupInfo, &processInfo, DEBUG_NO_SPECIAL_OPTIONS, &managedEventHandler->process);
			
			CloseHandle(processInfo.hProcess);
			CloseHandle(processInfo.hThread);

			SysFreeString(argumentsBstr);
			SysFreeString(currentDirectoryBstr);
			SysFreeString(applicationNameBstr);

			CheckHResult(hResult);
		}

		void Debugger::DebugActiveProcess(DWORD id, BOOL win32Attach)
		{
			HRESULT hResult = debug->DebugActiveProcess(id, win32Attach, &managedEventHandler->process);

			CheckHResult(hResult);
		}

		ProcessWrapper^ Debugger::GetProcess(UInt32 processID)
		{
			ICorDebugProcess* process;
			CheckHResult(debug->GetProcess(processID, &process));

			ProcessWrapper^ result = gcnew ProcessWrapper();
			result->Process = process;

			return result;
		}

		void Debugger::Release()
		{
			if (managedEventHandler != NULL)
			{
				delete managedEventHandler;
			}

			CheckHResult(debug->Terminate());
			delete this;

			/*if (unmanagedEventHandler != NULL)
			{
				delete unmanagedEventHandler;
			}*/
		}

		String^ Debugger::GetVersionFromProcess(IntPtr^ processHandle)
		{
			WCHAR *version = new WCHAR[DefaultCharArraySize];
			DWORD expectedSize;
			HANDLE hProcess = processHandle->ToPointer();

			HRESULT hResult = ::GetVersionFromProcess(hProcess, version, DefaultCharArraySize, &expectedSize);

			if (FAILED(hResult))
			{
				delete [] version;
			}

			CheckHResult(hResult);

			if (expectedSize > DefaultCharArraySize)
			{
				delete [] version;

				version = new WCHAR[expectedSize];

				hResult = ::GetVersionFromProcess(hProcess, version, DefaultCharArraySize, &expectedSize);

				if (FAILED(hResult))
				{
					delete [] version;
				}

				CheckHResult(hResult);
			}

			String^ result = gcnew String(version);

			delete [] version;

			return result;
		}

		String^ Debugger::GetRequestedRuntimeVersion(String^ executablePath)
		{
			WCHAR *version = new WCHAR[DefaultCharArraySize];
			DWORD expectedSize;
			IntPtr bstrPointer = Marshal::StringToBSTR(executablePath);
			BSTR executablePathBstr = static_cast<BSTR>(bstrPointer.ToPointer());
			pin_ptr<BSTR> pinPointer = &executablePathBstr;

			HRESULT hResult = ::GetRequestedRuntimeVersion(executablePathBstr, version, DefaultCharArraySize, &expectedSize);

			if (FAILED(hResult))
			{
				delete [] version;
				Marshal::FreeBSTR(bstrPointer);
			}

			CheckHResult(hResult);

			if (expectedSize > DefaultCharArraySize)
			{
				delete [] version;

				version = new WCHAR[expectedSize];

				hResult = ::GetRequestedRuntimeVersion(executablePathBstr, version, DefaultCharArraySize, &expectedSize);

				if (FAILED(hResult))
				{
					delete [] version;
					Marshal::FreeBSTR(bstrPointer);
				}

				CheckHResult(hResult);
			}

			String^ result = gcnew String(version);

			delete [] version;
			Marshal::FreeBSTR(bstrPointer);

			return result;
		}
	}
}