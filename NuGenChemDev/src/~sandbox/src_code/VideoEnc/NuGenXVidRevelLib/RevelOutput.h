#pragma once
#include <Revel.h>

using namespace System;

namespace VidEnc
{
	namespace Revel
	{

		public ref struct RevelOutputStats
		{
			int totalAudioBytes;
			int totalBytes;
		};

		public ref class RevelOutput
		{
		private:
			int frameSizeInInts;
			long frameSizeInBytes;

		public:
			RevelOutput();

			void Init(int width, int height, int fps, String^ filename);
			void DrawFrame(array<int>^ buffer);
			void DrawFrame(array<Byte>^ buffer);
			RevelOutputStats^ Close();
			//array<int>^ GetFrameBuffer();
		};

	}
}