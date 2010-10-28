#pragma once
#include "afxwin.h"

#include "..//Controls//FloatEdit.h"
#include "afxcmn.h"

// CWorkPlanesSetupsDlg dialog

class CWorkPlanesSetupsDlg : public CDialog
{
	DECLARE_DYNAMIC(CWorkPlanesSetupsDlg)

public:
	CWorkPlanesSetupsDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CWorkPlanesSetupsDlg();

public:
// Dialog Data
	enum { IDD = IDD_WORK_PLANES_SETUPS_DLG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support


	DECLARE_MESSAGE_MAP()
public:
	
	virtual BOOL OnInitDialog();
protected:
	virtual void OnOK();
public:

	float       m_x_pos;
	float       m_y_pos;
	float       m_z_pos;
	
private:
	CFloatEdit  m_x_pos_edit;
	CFloatEdit  m_y_pos_edit;
	CFloatEdit  m_z_pos_edit;

	void        SwitchActivePlane();
public:
	int m_cur_work_plane;
	afx_msg void OnBnClickedCurWorkPlaneRadio();
	afx_msg void OnBnClickedCurWorkPlaneRadio2();
	afx_msg void OnBnClickedCurWorkPlaneRadio3();
	BOOL m_xy_vis;
	BOOL m_yz_vis;
	BOOL m_xz_vis;
};
