#include "StdAfx.h"
#include "RevelOutput.h"
#include <memory.h>

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace VidEnc::Revel;

int encoderHandle;
Revel_Error revError;
Revel_Params revParams;
Revel_VideoFrame frame;

RevelOutput::RevelOutput()
{
	// check header version
	if (REVEL_API_VERSION != Revel_GetApiVersion())
		throw gcnew Exception("Revel header version mismatch!");

	// create encoder
    revError = Revel_CreateEncoder(&encoderHandle);
	if (revError != REVEL_ERR_NONE)
		throw gcnew Exception("Revel Error while creating encoder: " + Convert::ToString(revError));
}

void RevelOutput::Init(int width, int height, int fps, String^ filename)
{
	// setup enc params
    Revel_InitializeParams(&revParams);
    revParams.width = width;
    revParams.height = height;
    revParams.frameRate = (float)fps;
    revParams.quality = 1.0f;
    revParams.codec = REVEL_CD_XVID;

    revParams.hasAudio = 0;
    revParams.audioChannels = 1;
    revParams.audioRate = 22050;
    revParams.audioBits = 8;
    revParams.audioSampleFormat = REVEL_ASF_PCM;

	// init encoding
	IntPtr ptr = Marshal::StringToHGlobalAnsi(filename);
	char* fn = (char*)(void*)ptr;
	revError = Revel_EncodeStart(encoderHandle, fn, &revParams);
    if (revError != REVEL_ERR_NONE)
		throw gcnew Exception("Revel Error while starting encoding: " + Convert::ToString(revError));

	Marshal::FreeHGlobal(ptr);

	// setup rendering frame
    frame.width = width;
    frame.height = height;
    frame.bytesPerPixel = 4;
    frame.pixelFormat = REVEL_PF_RGBA;
    frame.pixels = new int[width * height];
    memset(frame.pixels, 0, width * height * 4);
	this->frameSizeInInts = width * height;
	this->frameSizeInBytes = width * height * 4;
}

void RevelOutput::DrawFrame(array<int>^ buffer)
{
	// Check length
	if (buffer->Length != this->frameSizeInInts)
		throw gcnew Exception("Frame buffer wrong size");

	interior_ptr<int> data = &buffer[0];

	// copy data over
	int* pixels = (int*)frame.pixels;
	for (long pixel=0; pixel < frameSizeInInts; pixel++)
	{
		pixels[pixel] = data[pixel];
	}

	int frameSize;
	revError = Revel_EncodeFrame(encoderHandle, &frame, &frameSize);
	if (revError != REVEL_ERR_NONE)
		throw gcnew Exception("Revel Error while writing frame: " + Convert::ToString(revError));
}

void RevelOutput::DrawFrame(array<Byte>^ buffer)
{
	// Check length
	if (buffer->Length != this->frameSizeInInts)
		throw gcnew Exception("Frame buffer wrong size");

	interior_ptr<Byte> data = &buffer[0];

	// copy data over
	Byte* pixels = (Byte*)frame.pixels;
	for (long pixel=0; pixel < frameSizeInBytes; pixel++)
	{
		pixels[pixel] = data[pixel];
	}

	int frameSize;
	revError = Revel_EncodeFrame(encoderHandle, &frame, &frameSize);
	if (revError != REVEL_ERR_NONE)
		throw gcnew Exception("Revel Error while writing frame: " + Convert::ToString(revError));
}

//array<int>^ RevelOutput::GetFrameBuffer()
//{
//	return nullptr;
//}

RevelOutputStats^ RevelOutput::Close()
{
	// write audio track (empty)
	char *audioBuffer = 0;
	int audioBufferSize;
	int totalAudioBytes = 0;
    revError = Revel_EncodeAudio(encoderHandle, audioBuffer, audioBufferSize,
								 &totalAudioBytes);

	int totalSize;
    revError = Revel_EncodeEnd(encoderHandle, &totalSize);
    if (revError != REVEL_ERR_NONE)
	    throw gcnew Exception("Revel Error while ending encoding: " + Convert::ToString(revError));

    // Final cleanup
    Revel_DestroyEncoder(encoderHandle);
    delete [] (int*)frame.pixels;

	return nullptr;
}