#pragma once
#include <iostream>
#include <noise/noise.h>

using namespace noise;

namespace NuGenNoiseLib
{
	namespace LibNoise
	{

		public ref class PerlinModuleWrapper
		{
		internal:
			module::Perlin* perlinModule;

		public:
			PerlinModuleWrapper(void);
			~PerlinModuleWrapper(void);

			double GetPerlinNoiseValue(double x, double y, double z);

			enum class NoiseQuality
			{
				Quality_Fast,
				Quality_STD,
				Quality_Best
			};
			
			property double Frequency
			{
				double get();
				void set(double);
			}

			property double Lacunarity
			{
				double get();
				void set(double);
			}

			property NoiseQuality Quality
			{
				NoiseQuality get();
				void set(NoiseQuality);
			}

			property int OctaveCount
			{
				int get();
				void set(int);
			}

			property double Persistence
			{
				double get();
				void set(double);
			}

			property int Seed
			{
				int get();
				void set(int);
			}
		};

	}
}