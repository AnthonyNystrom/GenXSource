#pragma once
#include <list>
#include "afxcmn.h"

// CCustomColorPage ��ȭ �����Դϴ�.
typedef std::vector <COLORREF> CColorsList;

/////////////////////////////////////////////////////////////////////////////
// Custom Listbox - containing colors

class CColorListBox : public CListBox
{
public:
// Operations
	void AddColorItem(COLORREF color);
	void InsertColorItem(int index, COLORREF color);

// Implementation
	virtual void MeasureItem(LPMEASUREITEMSTRUCT lpMIS);
	virtual void DrawItem(LPDRAWITEMSTRUCT lpDIS);
	virtual int CompareItem(LPCOMPAREITEMSTRUCT lpCIS);
};

class CCustomColorPage : public CPropertyPage
{
	DECLARE_DYNAMIC(CCustomColorPage)

public:
	CColorsList m_pColorList;
	CStatic		m_colorPreview;

	CCustomColorPage();
	virtual ~CCustomColorPage();

// ��ȭ ���� �������Դϴ�.
	enum { IDD = IDD_DIALOG_CUSTOM_COLOR };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �����Դϴ�.

	BOOL OnInitDialog();
	void DrawPreviewColor();

	COLORREF GetColor(float value);


	DECLARE_MESSAGE_MAP()
public:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
public:
	afx_msg void OnCbnSelchangeComboColorSize();
public:
	afx_msg void OnCbnDropdownComboColorSize();
public:

public:
//	CXTListCtrl m_listCtrlColors;
	CColorListBox m_listCtrlColors;
public:
	afx_msg void OnPaint();
	afx_msg void OnLbnDblclkListColors();
};
