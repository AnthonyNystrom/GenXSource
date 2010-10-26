#include "StdAfx.h"
#include "NoiseTextureBuilder.h"

using namespace System;
using namespace NuGenNoiseLib::LibNoise;
using namespace noise;

#define CalcWidthByteCount(width) ((width * 3) + 3) & ~0x03

void WriteImgToStream(utils::Image *image, Stream^ stream);
void WriteHeightMapToStream(int width, int height, float* map, Stream^ stream);

NoiseTextureBuilder::NoiseTextureBuilder(void)
{
}

void NoiseTextureBuilder::BuildSphericalTexture(const NoiseTextureParams^ params, Stream^% textureOut,
												Stream^% heightMapOut)
{
	if (params == nullptr)
		params = gcnew NoiseTextureParams();

	module::Perlin module = *params->Module->perlinModule;

	utils::NoiseMap heightMap;
	utils::NoiseMapBuilderSphere heightMapBuilder;
	heightMapBuilder.SetSourceModule(module);
	heightMapBuilder.SetDestNoiseMap(heightMap);
	heightMapBuilder.SetDestSize(params->Width, params->Height);
	heightMapBuilder.SetBounds(-90.0, 90.0, -180.0, 180.0);
	heightMapBuilder.Build();

	utils::RendererImage renderer;
	utils::Image image;
	renderer.SetSourceNoiseMap(heightMap);
	renderer.SetDestImage(image);
	renderer.ClearGradient();
	if (params->Gradients != nullptr)
	{
		for (int i = 0; i < params->Gradients->Length; i++)
		{
			renderer.AddGradientPoint(params->Gradients[i]->Position,
									  utils::Color(
											params->Gradients[i]->Clr.R,
											params->Gradients[i]->Clr.G,
											params->Gradients[i]->Clr.B,
											(params->EncodeHeightAsAlpha ? params->Gradients[i]->Position
																		 : 255))
									  );
		}
	}
	//renderer.AddGradientPoint(-1.0000, utils::Color (  0,   0, 128, 255)); // deeps
	//renderer.AddGradientPoint(-0.2500, utils::Color (  0,   0, 255, 255)); // shallow
	//renderer.AddGradientPoint( 0.0000, utils::Color (  0, 128, 255, 255)); // shore
	//renderer.AddGradientPoint( 0.0625, utils::Color (240, 240,  64, 255)); // sand
	//renderer.AddGradientPoint( 0.1250, utils::Color ( 32, 160,   0, 255)); // grass
	//renderer.AddGradientPoint( 0.3750, utils::Color (224, 224,   0, 255)); // dirt
	//renderer.AddGradientPoint( 0.7500, utils::Color (128, 128, 128, 255)); // rock
	//renderer.AddGradientPoint( 1.0000, utils::Color (255, 255, 255, 255)); // snow
	//renderer.AddGradientPoint(-1.0000, utils::Color (  64,   0, 0, 255));
	//renderer.AddGradientPoint( 1.0000, utils::Color (255, 0, 0, 255));
	if (params->Light)
	{
		renderer.EnableLight();
	}
	renderer.SetLightContrast(params->LightContrast);
	renderer.SetLightBrightness(params->LightBrightness);
	renderer.Render();

	textureOut = gcnew MemoryStream();
	WriteImgToStream(&image, textureOut);

	if (params->GenSeperateHeightMap)
	{
		// encode heightmap to bitmap
		heightMapOut = gcnew MemoryStream();
		WriteHeightMapToStream(params->Width, params->Height, heightMap.GetSlabPtr(), heightMapOut);
	}
}

void WriteBitmapHeader(Stream^ stream, int width, int height)
{
	// The width of one line in the file must be aligned on a 4-byte boundary.
	int bufferSize = CalcWidthByteCount(width);
	int destSize = bufferSize * height;

	const int BMP_HEADER_SIZE = 54;

	// Build the header.
	array<unsigned char>^ d = gcnew array<unsigned char>(4);
	System::Text::ASCIIEncoding::ASCII->GetBytes("BM", 0, 2, d, 0);
	stream->Write(d, 0, 2);
	array<unsigned char>^ bytes = BitConverter::GetBytes((unsigned int)destSize + BMP_HEADER_SIZE);
	stream->Write(BitConverter::GetBytes((unsigned int)destSize + BMP_HEADER_SIZE), 0, 4);
	System::Text::ASCIIEncoding::ASCII->GetBytes("\0\0\0\0", 0, 2, d, 0);
	stream->Write(d, 0, 4);
	stream->Write(BitConverter::GetBytes((unsigned int)BMP_HEADER_SIZE), 0, 4);
	stream->Write(BitConverter::GetBytes((unsigned int)40), 0, 4);   // Palette offset
	stream->Write(BitConverter::GetBytes((unsigned int)width), 0, 4);
	stream->Write(BitConverter::GetBytes((unsigned int)height), 0, 4);
	stream->Write(BitConverter::GetBytes((unsigned short)1), 0, 2);   // Planes per pixel
	stream->Write(BitConverter::GetBytes((unsigned short)24), 0, 2);   // Bits per plane
	stream->Write(d, 0, 4); // Compression (0 = none)
	stream->Write(BitConverter::GetBytes((unsigned int)destSize), 0, 4);
	stream->Write(BitConverter::GetBytes((unsigned int)2834), 0, 4); // X pixels per meter
	stream->Write(BitConverter::GetBytes((unsigned int)2834), 0, 4); // Y pixels per meter
	stream->Write(d, 0, 4);
	stream->Write(d, 0, 4);
}

void WriteHeightMapToStream(int width, int height, float* map, Stream^ stream)
{
	WriteBitmapHeader(stream, width, height);

	int bufferSize = CalcWidthByteCount(width);

	array<unsigned char>^ pLineBuffer = nullptr;
  
	// Allocate a buffer to hold one horizontal line in the bitmap.
	try {
		pLineBuffer = gcnew array<unsigned char>(bufferSize);
	}
	catch (...) {
		throw noise::ExceptionOutOfMemory ();
	}

	unsigned char* byteMap = (unsigned char*)map;

	// Build and write each horizontal line to the file.
	for (int y = 0; y < height; y++)
	{
		int index = 0;
		for (int x = 0; x < width; x++)
		{
			pLineBuffer[index++] = *byteMap++;
			pLineBuffer[index++] = *byteMap++;
			pLineBuffer[index++] = *byteMap++;
			byteMap++; // skip least sig byte :(
		}
		stream->Write(pLineBuffer, 0, bufferSize);
	}
	
	stream->Flush();
	stream->Seek(0, SeekOrigin::Begin);
}

void WriteImgToStream(utils::Image *image, Stream^ stream)
{
	if (image == NULL) {
		throw noise::ExceptionInvalidParam();
	}

	int width = image->GetWidth();
	int height = image->GetHeight();

	int bufferSize = CalcWidthByteCount(width);

	// This buffer holds one horizontal line in the destination file.
	array<unsigned char>^ pLineBuffer = nullptr;
  
	// Allocate a buffer to hold one horizontal line in the bitmap.
	try {
		pLineBuffer = gcnew array<unsigned char>(bufferSize);
	}
	catch (...) {
		throw noise::ExceptionOutOfMemory ();
	}

	WriteBitmapHeader(stream, width, height);

	// Build and write each horizontal line to the file.
	for (int y = 0; y < height; y++)
	{
		//Array::Clear(pLineBuffer, 0, bufferSize);

		noise::utils::Color* pSource = image->GetSlabPtr(y);

		int index = 0;
		for (int x = 0; x < width; x++)
		{
			pLineBuffer[index++] = pSource->blue;
			pLineBuffer[index++] = pSource->green;
			pLineBuffer[index++] = pSource->red;
			++pSource;
		}
		stream->Write(pLineBuffer, 0, bufferSize);
	}
	stream->Flush();
	stream->Seek(0, SeekOrigin::Begin);
}