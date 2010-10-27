//-----------------------------------------------------------------------------
// XMLUtils.h : Interface of the XML handling functions
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#ifndef _XMLUTILS_H_
#define _XMLUTILS_H_

#include <msxml.h>

#import "msxml.dll" rename_namespace("msxml")
using namespace msxml;

namespace glsp
{

// Load a text value from the node
CString loadElement(IXMLDOMNodePtr parent);

// Load a color value from the node
void loadColorElement(IXMLDOMNodePtr parent, float&r, float& g, float& b);

// Load a font value from the node
void loadFontElement(IXMLDOMNodePtr parent, CString& fontName, int& fontSize);

// Save the text value in the named node
void saveElement(IXMLDOMNodePtr parent, const CString& name, const CString& value);

// Save the numeric value in the named node
void saveElement(IXMLDOMNodePtr parent, const CString& name, double value);

// Save the color value in the named node
void saveColorElement(IXMLDOMNodePtr parent, const CString& name, float r, float g, float b);

// Save the font value in the named node
void saveFontElement(IXMLDOMNodePtr parent, const CString& name, const CString& fontName, int fontSize);

// Save the named text attribute in the node
void saveAttribute(IXMLDOMNodePtr parent, const CString& name, const CString& value);

// Save the named numeric attribute in the node
void saveAttribute(IXMLDOMNodePtr parent, const CString& name, double value);

};  // glsp

#endif // _XMLUTILS_H_
