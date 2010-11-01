#pragma once

#include "stdafx.h"
#include "ControllerWrapper.h"
#include "ThreadWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		ref class ProcessWrapper;

		public ref class AppDomainWrapper : ControllerWrapper
		{
		private:
			ICorDebugAppDomain* appDomain;

		internal:
			property ICorDebugAppDomain* AppDomain
			{
				ICorDebugAppDomain* get()
				{
					return appDomain;
				}

				void set (ICorDebugAppDomain* value)
				{
					appDomain = value;
					Controller = value;
				}
			}

			AppDomainWrapper()
			{
			}

			AppDomainWrapper(ICorDebugAppDomain* appDomain)
			{
				AppDomainWrapper();

				AppDomain = appDomain;
			}

		public:
			void Attach();

			UInt32 GetID();

			String^ GetName();

			ProcessWrapper^ GetProcess();

			void DeactivateSteppers();

			void ActivateBreakpoints(bool active);

			void virtual Detach() override;

			List<StepperWrapper^>^ EnumerateSteppers();
		};
	}
}