#include "StdAfx.h"
#include "LibNoise.h"

using namespace NuGenNoiseLib::LibNoise;

PerlinModuleWrapper::PerlinModuleWrapper(void)
: perlinModule(NULL)
{
	perlinModule = new module::Perlin();
}

PerlinModuleWrapper::~PerlinModuleWrapper(void)
{
	if (perlinModule)
	{
		delete perlinModule;
		perlinModule = NULL;
	}
}

double PerlinModuleWrapper::GetPerlinNoiseValue(double x, double y, double z)
{
	return perlinModule->GetValue(x, y, z);
}

double PerlinModuleWrapper::Frequency::get()
{
	return perlinModule->GetFrequency();
}

void PerlinModuleWrapper::Frequency::set(double value)
{
	perlinModule->SetFrequency(value);
}

double PerlinModuleWrapper::Lacunarity::get()
{
	return perlinModule->GetFrequency();
}

void PerlinModuleWrapper::Lacunarity::set(double value)
{
	perlinModule->SetLacunarity(value);
}

PerlinModuleWrapper::NoiseQuality PerlinModuleWrapper::Quality::get()
{
	return (PerlinModuleWrapper::NoiseQuality)perlinModule->GetNoiseQuality();
}

void PerlinModuleWrapper::Quality::set(PerlinModuleWrapper::NoiseQuality value)
{
	perlinModule->SetNoiseQuality((noise::NoiseQuality)value);
}

int PerlinModuleWrapper::OctaveCount::get()
{
	return perlinModule->GetOctaveCount();
}

void PerlinModuleWrapper::OctaveCount::set(int value)
{
	perlinModule->SetOctaveCount(value);
}

double PerlinModuleWrapper::Persistence::get()
{
	return perlinModule->GetPersistence();
}

void PerlinModuleWrapper::Persistence::set(double value)
{
	perlinModule->SetPersistence(value);
}

int PerlinModuleWrapper::Seed::get()
{
	return perlinModule->GetSeed();
}

void PerlinModuleWrapper::Seed::set(int value)
{
	perlinModule->SetSeed(value);
}