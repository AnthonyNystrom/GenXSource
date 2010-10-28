#pragma once

// ArMaXPropPage.h : Declaration of the CArMaXPropPage property page class.


// CArMaXPropPage : See ArMaXPropPage.cpp for implementation.

class CArMaXPropPage : public COlePropertyPage
{
	DECLARE_DYNCREATE(CArMaXPropPage)
	DECLARE_OLECREATE_EX(CArMaXPropPage)

// Constructor
public:
	CArMaXPropPage();

// Dialog Data
	enum { IDD = IDD_PROPPAGE_ArMaX };

// Implementation
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

// Message maps
protected:
	DECLARE_MESSAGE_MAP()
};

