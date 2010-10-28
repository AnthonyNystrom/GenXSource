
#if !defined(AFX_CLineThiknessCombo_H__8AAE34F7_7B02_11D3_A615_0060085FE616__INCLUDED_)
#define AFX_CLineThiknessCombo_H__8AAE34F7_7B02_11D3_A615_0060085FE616__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000 


class CLineThiknessCombo : public CComboBox
{
public:
  CLineThiknessCombo();
  virtual ~CLineThiknessCombo() {}
protected:
  virtual void DrawItem(LPDRAWITEMSTRUCT lpDIS);
  virtual void MeasureItem(LPMEASUREITEMSTRUCT lpMIS);
public:
  //virtual int AddString(LPCTSTR lpszString) { return -1; }
  virtual int InsertString(int nIndex, LPCTSTR lpszString) { return -1; }
  virtual int DeleteString(int nIndex) { return -1; }

#ifdef _DEBUG
  // @cmember,mfunc
  // for assertion only
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

#endif // !defined(AFX_CLineThiknessCombo_H__8AAE34F7_7B02_11D3_A615_0060085FE616__INCLUDED_)
