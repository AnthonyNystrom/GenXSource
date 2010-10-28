// stdafx.cpp : source file that includes just the standard includes
// MaterialsEditor.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"


#ifndef _DEBUG
	#pragma comment( lib, "../vendors/ExGuiLib/Release/exguilib.lib" )
	#pragma comment( lib, "../vendors/png/Release/png.lib" )
	#pragma comment( lib, "../vendors/jpeg/Release/jpeg.lib" )
	#pragma comment( lib, "../vendors/zlib/Release/zlib.lib" )
	#pragma comment( lib, "../vendors/tiff/Release/tiff.lib" )
	#pragma comment( lib, "../vendors/cximage/Release/cximage.lib" )    
#else
    #pragma comment( lib, "../vendors/ExGuiLib/Debug/exguilibd.lib" )
	#pragma comment( lib, "../vendors/png/Debug/png.lib" )
	#pragma comment( lib, "../vendors/jpeg/Debug/jpeg.lib" )
	#pragma comment( lib, "../vendors/zlib/Debug/zlib.lib" )
	#pragma comment( lib, "../vendors/tiff/Debug/tiff.lib" )
	#pragma comment( lib, "../vendors/cximage/Debug/cximage.lib" )  
#endif




