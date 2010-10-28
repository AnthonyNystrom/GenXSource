#pragma once

#include "..//Resource.h"
#include "afxcmn.h"

// CSphereParamsDlg dialog

class CSphereParamsDlg : public CDialog
{
	DECLARE_DYNAMIC(CSphereParamsDlg)

public:
	CSphereParamsDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CSphereParamsDlg();

// Dialog Data
	enum { IDD = IDD_SPHERE_EDIT_DLG };

	typedef enum
	{
		SPHERE_PARAMS,
		ELLIPSOID_PARAMS
	} DLG_TYPE;

	void SetDlgType(DLG_TYPE);

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
private:
	CSpinButtonCtrl m_merid_spin;
	CSpinButtonCtrl m_paral_spin;
	int m_merid_cnt;
	int m_paral_cnt;

	DLG_TYPE        m_type;
public:
	void  SetParams(int meridCnt, int ParalCnt)
	{
		m_merid_cnt = meridCnt;
		m_paral_cnt = ParalCnt;
		if (m_merid_cnt>36)
			m_merid_cnt=36;
		if (m_merid_cnt<3)
			m_merid_cnt=3;
		if (m_paral_cnt>36)
			m_paral_cnt=36;
		if (m_paral_cnt<3)
			m_paral_cnt=3;
	};
	void GetParams(int& meridCnt, int& ParalCnt)
	{
		if (m_merid_cnt>36)
			m_merid_cnt=36;
		if (m_merid_cnt<3)
			m_merid_cnt=3;
		if (m_paral_cnt>36)
			m_paral_cnt=36;
		if (m_paral_cnt<3)
			m_paral_cnt=3;
		meridCnt = m_merid_cnt;
		ParalCnt = m_paral_cnt;
	}
	afx_msg void OnDeltaposMeridSphSpin(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnDeltaposParalSphSpin(NMHDR *pNMHDR, LRESULT *pResult);
	virtual BOOL OnInitDialog();
};
