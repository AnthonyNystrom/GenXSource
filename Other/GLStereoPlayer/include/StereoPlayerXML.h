//-----------------------------------------------------------------------------
// StereoPlayerXML.cpp : Interface of the StereoPlayer persistence functions
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#ifndef _STEREOPLAYERXML_H_
#define _STEREOPLAYERXML_H_

#include <strsafe.h>

namespace glsp
{

// Utility functions for loading/saving the settings of StereoPlayer instance as an XML file.
BOOL LoadSettingFile(class StereoPlayer* stereoPlayer, const CString& homeDir, const CString& fileName);
BOOL SaveSettingFile(class StereoPlayer* stereoPlayer, const CString& homeDir, const CString& fileName);

}; // glsp

#endif // _STEREOPLAYERXML_H_
