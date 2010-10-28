
#if !defined(AFX_CLineStyleCombo_H__8AAE34F7_7B02_11D3_A615_0060085FE616__INCLUDED_)
#define AFX_CLineStyleCombo_H__8AAE34F7_7B02_11D3_A615_0060085FE616__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000 


class CLineStyleCombo : public CComboBox
{
  // @access Public Member Functions and Variables
public:

  // @cmember
  // constructor
  CLineStyleCombo();

  // @cmember,mfunc
  // destructor
  virtual ~CLineStyleCombo() {}

protected:

  // @cmember
  // Called by MFC when visual aspect of combo box changes 
  virtual void DrawItem(LPDRAWITEMSTRUCT lpDIS);
  virtual void MeasureItem(LPMEASUREITEMSTRUCT lpMIS);

  //virtual int AddString(LPCTSTR lpszString) { return -1; }
  virtual int InsertString(int nIndex, LPCTSTR lpszString) { return -1; }
  virtual int DeleteString(int nIndex) { return -1; }

#ifdef _DEBUG

  virtual void PreSubclassWindow();
#endif

// @access Private Member Functions and Variables
private:

public:
	DECLARE_MESSAGE_MAP()
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_CLineStyleCombo_H__8AAE34F7_7B02_11D3_A615_0060085FE616__INCLUDED_)
