// MaterialsEditorView.h : interface of the CMaterialsEditorView class
//


#pragma once

#include "MaterialsEditorDoc.h"
#include "afxwin.h"
#include "afxcmn.h"

#include "Controls//ListCtrlHiddenSB.h"
#include "armaxctrl.h"

typedef enum
{
	AR_AMBIENT,
	AR_DIFFUSE,
	AR_EMISSION,
	AR_SPECULAR,
	AR_TRANSPARENT,
	AR_SHININESS,
	SG_TEXTURE
} ACTIVE_RADIO;


typedef struct
{
public:
	int nID;
	BOOL bRead;
	BOOL bWrite;
	const char* description;
	const char* ext;
} DocType;

#include "MyEdit.h"

class CMaterialsEditorView : public CFormView
{
protected: // create from serialization only
	CMaterialsEditorView();
	DECLARE_DYNCREATE(CMaterialsEditorView)

public:
	enum{ IDD = IDD_MATERIALSEDITOR_FORM };

// Attributes
public:
	CMaterialsEditorDoc* GetDocument() const;

// Operations
private:
	bool  m_image_list_was_created;
// Overrides
public:
virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual void OnInitialUpdate(); // called first time after construct

// Implementation
public:
	virtual ~CMaterialsEditorView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	DECLARE_MESSAGE_MAP()

public:
	afx_msg void OnEnChangeMatComment();
	afx_msg void OnLbnSelchangeMatNamesList();
	afx_msg void OnEnChangeMatName();
private:
	int				m_current_material_index;
	ACTIVE_RADIO	m_active_radio;

	int			m_mats_count;
	CString		m_comment;	
	CListBox	m_all_materials_list;
	CString		m_current_name;
	int 		m_ambient_radio;
	//CPlamatexctrl m_preview_material;
	CSliderCtrl m_red_slider;
	CSliderCtrl m_green_slider;
	CSliderCtrl m_blue_slider;
	CSliderCtrl m_transp_slider;
	CSliderCtrl m_shininess_slider;

	CImageList			m_ImageList;		// image list holding the thumbnails
	CListCtrlHiddenSB	m_textures_list;

	CStatic m_texture_preview;

	bool	PromptForFileName(CString& fileName, UINT nIDSTitle, DWORD dwFlags, BOOL bOpenFileDialog, int* pType);
	int		GetIndexFromType(int nDocType, BOOL bOpenFileDialog);
	int		GetTypeFromIndex(int nIndex, BOOL bOpenFileDialog);
	CString GetExtFromType(int nDocType);
	CString GetFileTypes(BOOL bOpenFileDialog);
	CString FindExtension(const CString& name);
	int		FindType(const CString& ext);

	void        SwichSlidersVisible();

	void        SwitchMaterial();
	void        SwitchActiveRadio();
public:
	afx_msg void OnAddNewMaterial();
	afx_msg void OnDeleteCurMaterial();

	afx_msg void OnBnClickedAmbientRadio();
	afx_msg void OnBnClickedDiffusionRadio();
	afx_msg void OnBnClickedEmissionRadio();
	afx_msg void OnBnClickedSpecularRadio();
	afx_msg void OnBnClickedTransparentRadio();
	afx_msg void OnBnClickedShininessRadio();
	afx_msg void OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);

	afx_msg void OnNMClickTexturesList(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnAddNewTexture();

	afx_msg void OnDeleteCurTexture();
	afx_msg void OnPaint();
	afx_msg void OnUpdateDeleteCurTexture(CCmdUI *pCmdUI);
	afx_msg void OnUpdateDeleteCurMaterial(CCmdUI *pCmdUI);
private:
	CArmaxctrl m_preview_material;
public:
//	virtual BOOL DestroyWindow();
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
private:
	CMyEdit m_red_edit;
	CMyEdit m_green_edit;
	CMyEdit m_blue_edit;
	CMyEdit m_transp_edit;
	CMyEdit m_sh_edit;
	CMyEdit m_count_edit;
	CComboBox m_cmbMaterial;
	afx_msg void OnCbnSelchangeCmbMaterial();
	int m_iSolidMaterial;
};

#ifndef _DEBUG  // debug version in MaterialsEditorView.cpp
inline CMaterialsEditorDoc* CMaterialsEditorView::GetDocument() const
   { return reinterpret_cast<CMaterialsEditorDoc*>(m_pDocument); }
#endif

