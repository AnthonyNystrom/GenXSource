#pragma once

namespace Genetibase
{
	namespace Debug
	{
		public __gc __interface IHookInstall
		{
		public:
			void OnInstallHook(System::Byte data[]) = 0;
		};

		public __gc class NuGenHookInstaller
		{
		private:

		public:
			static void InjectAssembly(int processID, int threadID, System::String* assemblyLocation, System::String* typeName, System::Byte additionalData[]);
		};
	}
}
