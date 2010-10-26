#pragma once
#include <noise/noise.h>
#include "noiseutils.h"
#include "LibNoise.h"

using namespace System::IO;

namespace NuGenNoiseLib
{
	namespace LibNoise
	{
		public ref struct NoiseGradient
		{
		public:
			double Position;
			System::Drawing::Color Clr;

			NoiseGradient(double pPosition, System::Drawing::Color pClr)
			{
				Position = pPosition;
				Clr = pClr;
			}
		};

		public ref struct NoiseTextureParams
		{
		public:
			// generation
			PerlinModuleWrapper^ Module;
			bool GenSeperateHeightMap;
			bool EncodeHeightAsAlpha;
			
			// lighting
			bool Light;
			double LightContrast, LightBrightness;

			// output texture & heightmap
			int Width;
			int Height;

			// colours
			array<NoiseGradient^>^ Gradients;

			NoiseTextureParams(int width, int height, bool genSeperateHeightMap,
							   PerlinModuleWrapper^ module, bool light)
				: Width(width)
				, Height(height)
				, GenSeperateHeightMap(genSeperateHeightMap)
				, Module(module)
				, LightContrast(3)
				, LightBrightness(2)
				, Light(light)
				, Gradients(gcnew array<NoiseGradient^>(2))
			{
				Gradients[0] = gcnew NoiseGradient(-1, System::Drawing::Color::Black);
				Gradients[1] = gcnew NoiseGradient(1, System::Drawing::Color::White);
			}

			/// Defaults
			NoiseTextureParams()
				: Width(512)
				, Height(256)
				, Module(gcnew PerlinModuleWrapper())
				, LightContrast(3)
				, LightBrightness(2)
				, Gradients(gcnew array<NoiseGradient^>(2))
			{
				Gradients[0] = gcnew NoiseGradient(-1, System::Drawing::Color::Black);
				Gradients[1] = gcnew NoiseGradient(1, System::Drawing::Color::White);
			}

			~NoiseTextureParams()
			{
				if (Module != nullptr)
					delete Module;
				if (Gradients != nullptr)
					delete Gradients;
			}
		};

		public ref class NoiseTextureBuilder
		{
		public:
			NoiseTextureBuilder(void);

			static void BuildSphericalTexture(const NoiseTextureParams^ params, Stream^% textureOut,
											  Stream^% heightMapOut);
		};

	}
}