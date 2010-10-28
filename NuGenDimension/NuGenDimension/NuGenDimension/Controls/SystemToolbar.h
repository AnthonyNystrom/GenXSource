#pragma once

#include "ColorPickerCB.h"
//#include "BitmapPickerCombo.h"
#include "LineThiknessCombo.h"
#include "LineStyleCombo.h"

//#include "My32Toolbar.h"

#include "..//NuGenDimensionView.h"

class CMyComboBox : public CComboBox
{
private:
	CFont			m_font;
	BOOL			CreateFont(LONG lfHeight, LONG lfWeight, LPCTSTR lpszFaceName);

public:

	// Generated message map functions
	DECLARE_MESSAGE_MAP()
protected:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);

};

class CLayersComboBox : public CMyComboBox
{
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnCbnSelchange();
};

class CPrecisionComboBox : public CMyComboBox
{
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnCbnSelchange();
};

class CPrecisionStatic : public CStatic
{
private:
	CFont			m_font;
	BOOL			CreateFont(LONG lfHeight, LONG lfWeight, LPCTSTR lpszFaceName);
	// Generated message map functions
protected:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	DECLARE_MESSAGE_MAP()
public:
//	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnPaint();
};

class CColorComboBox : public CColorPickerCB
{
	
public:
	
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnCbnSelchange();
};

class CLineThicknessComboBox : public CLineThiknessCombo//CBitmapPickerCombo
{

public:
	
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnCbnSelchange();
};

class CLineTypeComboBox : public CLineStyleCombo
{
	
public:
	
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnCbnSelchange();
};


class CFontsComboBox : public CMyComboBox
{
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnCbnSelchange();
};


// CSystemToolbar

class CSystemToolbar : public CEGToolBar
{
	DECLARE_DYNAMIC(CSystemToolbar)
private:
	CLayersComboBox    m_LayersComboBox;
	CStatic            m_empty_static;
	CPrecisionStatic   m_Precision_label;
	CPrecisionComboBox m_PrecisionComboBox;
	CColorComboBox     m_ColorComboBox;

	CFontsComboBox     m_Fonts_ComboBox;

	CLineThicknessComboBox   m_line_thicknessComboBox;
	CLineTypeComboBox		 m_line_typeComboBox;

	CNuGenDimensionView*           m_view;
public:
	CSystemToolbar();
	virtual ~CSystemToolbar();

	void     SetView(CNuGenDimensionView* v);
	void    UpdateSystemToolbar();
protected:
	DECLARE_MESSAGE_MAP()
public:
	void   LoadControls();
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
};


