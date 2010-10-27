//-----------------------------------------------------------------------------
// XMLUtils.cpp : Implementation of the XML handling functions
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#include "stdafx.h"

#include "XMLUtils.h"

namespace glsp
{

// Load a text value from the node
CString loadElement(IXMLDOMNodePtr parent)
{
    if (parent == 0) return CString();
    if (parent->firstChild == 0) return CString();

    return CString((LPCSTR)(_bstr_t(parent->firstChild->nodeValue)));
}

// Load a color value from the node
void loadColorElement(IXMLDOMNodePtr parent, float&r, float& g, float& b)
{
    r = 0.0f;
    g = 0.0f;
    b = 0.0f;

    if (parent == 0) return;

    IXMLDOMElementPtr element = parent;
    if (element->getAttribute("r"))
        r = (float)atof((LPCSTR)(_bstr_t(element->getAttribute("r"))));
    if (element->getAttribute("g"))
        g = (float)atof((LPCSTR)(_bstr_t(element->getAttribute("g"))));
    if (element->getAttribute("b"))
        b = (float)atof((LPCSTR)(_bstr_t(element->getAttribute("b"))));
}

// Load a font value from the node
void loadFontElement(IXMLDOMNodePtr parent, CString& fontName, int& fontSize)
{
    fontName = "";
    fontSize = 12;

    if (parent == 0) return;

    IXMLDOMElementPtr element = parent;
    fontName = (LPCSTR)(_bstr_t(element->getAttribute("name")));
    fontSize = atoi((LPCSTR)(_bstr_t(element->getAttribute("size"))));
}


// Save the text value in the named node
void saveElement(IXMLDOMNodePtr parent, const CString& name, const CString& value)
{
    if (parent == 0) return;

    BSTR bstrName = name.AllocSysString();
    BSTR bstrValue = value.AllocSysString();

    IXMLDOMDocumentPtr doc = parent->ownerDocument;
    IXMLDOMNodePtr elem = doc->createElement(bstrName);
    IXMLDOMTextPtr text = doc->createTextNode(bstrValue);

    elem->appendChild(text);
    parent->appendChild(elem);

    SysFreeString(bstrName);
    SysFreeString(bstrValue);
}

// Save the numeric value in the named node
void saveElement(IXMLDOMNodePtr parent, const CString& name, double value)
{
    CString cstrValue;
    cstrValue.Format("%g", value);

    saveElement(parent, name, cstrValue);
}

// Save the color value in the named node
void saveColorElement(IXMLDOMNodePtr parent, const CString& name, float r, float g, float b)
{
    if (parent == 0) return;

    BSTR bstrName = name.AllocSysString();

    CString cstrR, cstrG, cstrB;
    cstrR.Format("%g", r);
    BSTR bstrRValue = cstrR.AllocSysString();
    cstrG.Format("%g", g);
    BSTR bstrGValue = cstrG.AllocSysString();
    cstrB.Format("%g", b);
    BSTR bstrBValue = cstrB.AllocSysString();

    IXMLDOMDocumentPtr doc = parent->ownerDocument;
    IXMLDOMElementPtr elem = doc->createElement(bstrName);

    elem->setAttribute("r", bstrRValue);
    elem->setAttribute("g", bstrGValue);
    elem->setAttribute("b", bstrBValue);

    parent->appendChild(elem);

    SysFreeString(bstrName);
    SysFreeString(bstrRValue);
    SysFreeString(bstrGValue);
    SysFreeString(bstrBValue);
}

// Save the font value in the named node
void saveFontElement(IXMLDOMNodePtr parent, const CString& name, const CString& fontName, int fontSize)
{
    if (parent == 0) return;

    BSTR bstrName = name.AllocSysString();
    BSTR bstrFontName = fontName.AllocSysString();
    CString cstrFontSize;
    cstrFontSize.Format("%i", fontSize);
    BSTR bstrFontSize = cstrFontSize.AllocSysString();

    IXMLDOMDocumentPtr doc = parent->ownerDocument;
    IXMLDOMElementPtr elem = doc->createElement(bstrName);

    elem->setAttribute("name", bstrFontName);
    elem->setAttribute("size", bstrFontSize);

    parent->appendChild(elem);

    SysFreeString(bstrName);
    SysFreeString(bstrFontName);
    SysFreeString(bstrFontSize);
}

// Save the named text attribute in the node
void saveAttribute(IXMLDOMNodePtr parent, const CString& name, const CString& value)
{
    if (parent == 0) return;

    BSTR bstrName = name.AllocSysString();
    BSTR bstrValue = value.AllocSysString();

    IXMLDOMElementPtr elem = parent;
    elem->setAttribute(bstrName, bstrValue);

    SysFreeString(bstrName);
    SysFreeString(bstrValue);
}

// Save the named numeric attribute in the node
void saveAttribute(IXMLDOMNodePtr parent, const CString& name, double value)
{
    CString cstrValue;
    cstrValue.Format("%g", value);

    saveAttribute(parent, name, cstrValue);
}

};  // glsp
