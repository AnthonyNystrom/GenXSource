#if !defined(AFX_PROPTREEINFO_H__22BD9C18_A68C_4BB8_B7FC_C4A7DA0E1EBF__INCLUDED_)
#define AFX_PROPTREEINFO_H__22BD9C18_A68C_4BB8_B7FC_C4A7DA0E1EBF__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// EGPropertyGridInfo.h : header file
//
//
//  Copyright (C) 1998-2001 Scott Ramsay
//	sramsay@gonavi.com
//	http://www.gonavi.com
//
//  This material is provided "as is", with absolutely no warranty expressed
//  or implied. Any use is at your own risk.
// 
//  Permission to use or copy this software for any purpose is hereby granted 
//  without fee, provided the above notices are retained on all copies.
//  Permission to modify the code and to distribute modified code is granted,
//  provided the above notices are retained, and a notice that the code was
//  modified is included with the above copyright notice.
// 
//	If you use this code, drop me an email.  I'd like to know if you find the code
//	useful.

class CEGPropertyGrid;

/////////////////////////////////////////////////////////////////////////////
// CEGPropertyGridInfo window

class GUILIBDLLEXPORT CEGPropertyGridInfo : public CStatic
{
// Construction
public:
	CEGPropertyGridInfo();

// Attributes
public:
	// CEGPropertyGrid class that this class belongs
	void SetPropOwner(CEGPropertyGrid* pProp);

protected:
	CEGPropertyGrid*		m_pProp;

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CEGPropertyGridInfo)
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CEGPropertyGridInfo();

	// Generated message map functions
protected:
	//{{AFX_MSG(CEGPropertyGridInfo)
	afx_msg void OnPaint();
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_PROPTREEINFO_H__22BD9C18_A68C_4BB8_B7FC_C4A7DA0E1EBF__INCLUDED_)
