//-----------------------------------------------------------------------------
// StereoPlayerXML.cpp : Implementation of the StereoPlayer persistence functions
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#include "stdafx.h"

#include "StereoPlayer.h"
#include "XMLUtils.h"

#include "StereoPlayerXML.h"

#include <direct.h>

#import "msxml.dll" rename_namespace("msxml")
using namespace msxml;

namespace glsp
{

// Load the settings from the specified XML file and set them to the StereoPlayer instance
int LoadSettingFile(StereoPlayer* stereoPlayer, const CString& homeDir, const CString& fileName)
{
	char curDir[_MAX_PATH];
	_getcwd(curDir, _MAX_PATH);
	_chdir(homeDir);

	try
	{
		IXMLDOMDocumentPtr doc("MSXML.DOMDocument");
		doc->validateOnParse = VARIANT_FALSE;
		doc->load(LPCTSTR(fileName));

		IXMLDOMNodePtr root = doc->documentElement;
		IXMLDOMNodePtr node;

		// Root node's name must be 'gsp' or 'glsp'
		if (root->nodeName != _bstr_t("gsp") &&
			root->nodeName != _bstr_t("glsp")) {
			_chdir(curDir);
			return FALSE;
		}

		// Parse each setting element
		CString value;
		node = root->firstChild;
		while (node)
		{
			if (node->nodeName == _bstr_t("left"))
				stereoPlayer->loadLeftFile(loadElement(node));
			else if (node->nodeName == _bstr_t("right"))
				stereoPlayer->loadRightFile(loadElement(node));
			else if (node->nodeName == _bstr_t("format"))
			{
				value = loadElement(node);
				if (value == "separated")
					stereoPlayer->setFormat(STEREO_FORMAT_SEPARATED);
				else if (value == "horizontal")
					stereoPlayer->setFormat(STEREO_FORMAT_HORIZONTAL);
				else if (value == "horizontalComp")
					stereoPlayer->setFormat(STEREO_FORMAT_HORIZONTAL_COMP);
				else if (value == "vertical")
					stereoPlayer->setFormat(STEREO_FORMAT_VERTICAL);
				else if (value == "verticalComp")
					stereoPlayer->setFormat(STEREO_FORMAT_VERTICAL_COMP);
			}
			else if (node->nodeName == _bstr_t("stereoType"))
			{
				value = loadElement(node);
				if (value == "left")
					stereoPlayer->setType(STEREO_TYPE_LEFT);
				else if (value == "right")
					stereoPlayer->setType(STEREO_TYPE_RIGHT);
				else if (value == "anagryph")
					stereoPlayer->setType(STEREO_TYPE_ANAGRYPH);
				else if (value == "horizontal")
					stereoPlayer->setType(STEREO_TYPE_HORIZONTAL);
				else if (value == "vertical")
					stereoPlayer->setType(STEREO_TYPE_VERTICAL);
				else if (value == "horizontalInterleaved")
					stereoPlayer->setType(STEREO_TYPE_HORIZONTAL_INTERLEAVED);
				else if (value == "verticalInterleaved")
					stereoPlayer->setType(STEREO_TYPE_VERTICAL_INTERLEAVED);
				else if (value == "sharp3D")
					stereoPlayer->setType(STEREO_TYPE_SHARP3D);
				else if (value == "quadBuffer")
					stereoPlayer->setType(STEREO_TYPE_QUADBUFFER);
			}
			else if (node->nodeName == _bstr_t("swap"))
				stereoPlayer->setSwap((BOOL)atoi(loadElement(node)));
			else if (node->nodeName == _bstr_t("offset"))
				stereoPlayer->setOffset((float)atof(loadElement(node)));
			else if (node->nodeName == _bstr_t("panX"))
				stereoPlayer->setPanX((float)atof(loadElement(node)));
			else if (node->nodeName == _bstr_t("panY"))
				stereoPlayer->setPanY((float)atof(loadElement(node)));
			else if (node->nodeName == _bstr_t("zoom"))
				stereoPlayer->setZoom((float)atof(loadElement(node)));
			else if (node->nodeName == _bstr_t("speed"))
				stereoPlayer->setRate((float)atof(loadElement(node)));
			else if (node->nodeName == _bstr_t("volume"))
				stereoPlayer->setVolume(atoi(loadElement(node)));

			node = node->nextSibling;
		}

		_chdir(curDir);

		return TRUE;
	}
	catch(...) {}

	_chdir(curDir);

	return FALSE;
}

// Save the settings of the StereoPlayer instance to the specified XML file
BOOL SaveSettingFile(StereoPlayer* stereoPlayer, const CString& homeDir, const CString& fileName)
{
	char curDir[_MAX_PATH];
	_getcwd(curDir, _MAX_PATH);
	_chdir(homeDir);

	try
	{
		IXMLDOMDocumentPtr doc("MSXML.DOMDocument");
		doc->appendChild(doc->createProcessingInstruction("xml", "version='1.0'"));

		IXMLDOMNodePtr root = doc->createElement("glsp");
		doc->appendChild(root);

		saveAttribute(root, "version", stereoPlayer->getVersion());

		saveElement(root, "left", stereoPlayer->getLeftFileName());
		saveElement(root, "right", stereoPlayer->getRightFileName());
		switch (stereoPlayer->getFormat())
		{
			case STEREO_FORMAT_SEPARATED:
				saveElement(root, "format", "separated"); break;
			case STEREO_FORMAT_HORIZONTAL:
				saveElement(root, "format", "horizontal"); break;
			case STEREO_FORMAT_HORIZONTAL_COMP:
				saveElement(root, "format", "horizontalComp"); break;
			case STEREO_FORMAT_VERTICAL:
				saveElement(root, "format", "vertical"); break;
			case STEREO_FORMAT_VERTICAL_COMP:
				saveElement(root, "format", "verticalComp"); break;
		}
		switch (stereoPlayer->getType())
		{
			case STEREO_TYPE_LEFT:
				saveElement(root, "stereoType", "left"); break;
			case STEREO_TYPE_RIGHT:
				saveElement(root, "stereoType", "right"); break;
			case STEREO_TYPE_ANAGRYPH:
				saveElement(root, "stereoType", "anagryph"); break;
			case STEREO_TYPE_HORIZONTAL:
				saveElement(root, "stereoType", "horizontal"); break;
			case STEREO_TYPE_VERTICAL:
				saveElement(root, "stereoType", "vertical"); break;
			case STEREO_TYPE_HORIZONTAL_INTERLEAVED:
				saveElement(root, "stereoType", "horizontalInterleaved"); break;
			case STEREO_TYPE_VERTICAL_INTERLEAVED:
				saveElement(root, "stereoType", "verticalInterleaved"); break;
			case STEREO_TYPE_SHARP3D:
				saveElement(root, "stereoType", "sharp3D"); break;
			case STEREO_TYPE_QUADBUFFER:
				saveElement(root, "stereoType", "quadBuffer"); break;
		}
		saveElement(root, "swap", stereoPlayer->getSwap()?1:0);
		saveElement(root, "offset", stereoPlayer->getOffset());
		saveElement(root, "panX", stereoPlayer->getPanX());
		saveElement(root, "panY", stereoPlayer->getPanY());
		saveElement(root, "zoom", stereoPlayer->getZoom());
		saveElement(root, "speed", stereoPlayer->getRate());
		saveElement(root, "volume", stereoPlayer->getVolume());

		doc->save(LPCTSTR(fileName));

		_chdir(curDir);

		return TRUE;
	}
	catch(...) {}

	_chdir(curDir);
	
	return FALSE;
}

};  // glsp
