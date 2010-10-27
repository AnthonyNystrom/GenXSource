//-----------------------------------------------------------------------------
// GLStereoPlayerDoc.h : Interface of the CGLStereoPlayerDoc class
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#if !defined(AFX_GLSTEREOPLAYERDOC_H__83AE0C85_D855_488B_955F_46C89D64D483__INCLUDED_)
#define AFX_GLSTEREOPLAYERDOC_H__83AE0C85_D855_488B_955F_46C89D64D483__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CGLStereoPlayerDoc : public CDocument
{
protected: // create from serialization only
    CGLStereoPlayerDoc();
    DECLARE_DYNCREATE(CGLStereoPlayerDoc)

// Attributes
public:

// Operations
public:

// Overrides
    // ClassWizard generated virtual function overrides
    //{{AFX_VIRTUAL(CGLStereoPlayerDoc)
	public:
    virtual void Serialize(CArchive& ar);
    virtual BOOL OnOpenDocument(LPCTSTR lpszPathName);
    virtual BOOL OnSaveDocument(LPCTSTR lpszPathName);
    virtual void SetPathName(LPCTSTR lpszPathName, BOOL bAddToMRU = TRUE);
	protected:
	virtual BOOL SaveModified();
	//}}AFX_VIRTUAL

// Implementation
public:
    virtual ~CGLStereoPlayerDoc();
#ifdef _DEBUG
    virtual void AssertValid() const;
    virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
    //{{AFX_MSG(CGLStereoPlayerDoc)
	//}}AFX_MSG
    DECLARE_MESSAGE_MAP()

public:
	BOOL m_bSaveModified;
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_GLSTEREOPLAYERDOC_H__83AE0C85_D855_488B_955F_46C89D64D483__INCLUDED_)
