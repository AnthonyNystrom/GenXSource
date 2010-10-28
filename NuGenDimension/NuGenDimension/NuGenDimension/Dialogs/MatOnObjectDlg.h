#pragma once
#include "afxwin.h"
#include "..//..//MaterialsEditor//ArmaxCtrl.h"
#include "..//Controls//NumEdit.h"
#include "..//Controls//NumSpinCtrl.h"


// CMatOnObjectDlg dialog

class CMatOnObjectDlg : public CDialog
{
	DECLARE_DYNAMIC(CMatOnObjectDlg)

public:
	CMatOnObjectDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CMatOnObjectDlg();

// Dialog Data
	enum { IDD = IDD_MAT_ON_OBJ_DLG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnInitDialog();
private:
	sgC3DObject*  m_object;
	CListBox m_all_mats_names;
	int m_mat_count;
	//CPlamatexctrl m_armatex;
public:
	void    SetObject(sgC3DObject* Ob);
	afx_msg void OnLbnSelchangeMatNamesList();
private:
	CArmaxctrl m_preview_material;
	CStatic m_texture_preview;

	CNumSpinCtrl	c_sprotx;
	CNumEdit	c_rotx;
	CNumSpinCtrl	c_spshiftV;
	CNumEdit	c_shiftV;
	CNumSpinCtrl	c_spshiftU;
	CNumEdit	c_shiftU;
	CNumSpinCtrl	c_spscaleV;
	CNumEdit	c_scaleV;
	CNumSpinCtrl	c_spscaleU;
	CNumEdit	c_scaleU;

	void        ShowHideControls();

public:
	afx_msg void OnPaint();
protected:
	virtual void OnOK();
private:
	CComboBox m_mix_color_combo;
	BOOL m_mult_texture;
	BOOL m_smooth_texture;
public:
	afx_msg void OnBnClickedTexOnObjBtn();
private:
	int   m_map_type;
public:
	afx_msg void OnBnClickedRadio1();
	afx_msg void OnBnClickedRadio2();
	afx_msg void OnBnClickedRadio3();
};
