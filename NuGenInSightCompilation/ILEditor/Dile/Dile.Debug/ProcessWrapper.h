#pragma once

#include "stdafx.h"
#include "CorError.h"
#include "ControllerWrapper.h"

using namespace System;
using namespace System::Collections::Generic;

namespace Dile
{
	namespace Debug
	{
		ref class ModuleWrapper;
		ref class ThreadWrapper;
		ref class AppDomainWrapper;

		public ref class ProcessWrapper : ControllerWrapper
		{
		private:
			ICorDebugProcess* process;

		internal:
			property ICorDebugProcess* Process
			{
				ICorDebugProcess* get()
				{
					return process;
				}

				void set(ICorDebugProcess* value)
				{
					process = value;
					Controller = value;
				}
			}

			ProcessWrapper()
			{
			}

			ProcessWrapper(ICorDebugProcess* process)
			{
				ProcessWrapper();

				Process = process;
			}

			void EnumerateAppDomains(String^ moduleName, List<ModuleWrapper^>^ foundModules);

			void EnumerateAssemblies(String^ moduleName, ICorDebugAppDomain* appDomain, List<ModuleWrapper^>^ foundModules);

			void EnumerateModules(String^ moduleName, ICorDebugAssembly* assembly, List<ModuleWrapper^>^ foundModules);

		public:
			void EnableLogMessages(bool enable);

			UInt32 GetHelperThreadID();

			UInt32 GetID();

			void ClearCurrentException(DWORD threadID);

			List<ModuleWrapper^>^ FindModulesByName(String^ moduleName);

			List<ModuleWrapper^>^ GetModules();

			ThreadWrapper^ GetThread(UInt32 threadID);

			List<AppDomainWrapper^>^ GetAppDomains();

			void DeactivateSteppers();

			void ActivateBreakpoints(bool active);

			void virtual Detach() override;

			array<Byte>^ ReadMemory(CORDB_ADDRESS address, DWORD size);
		};
	}
}