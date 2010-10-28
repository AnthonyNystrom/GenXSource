#pragma once

#include "..//Resource.h"
#include "afxcmn.h"

// CSphereParamsDlg dialog

class CMeridiansDlg : public CDialog
{
	DECLARE_DYNAMIC(CMeridiansDlg)

public:
	CMeridiansDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CMeridiansDlg();

// Dialog Data
	enum { IDD = IDD_MERIDIANS_DLG };

	typedef enum
	{
		CONE_PARAMS,
		CYL_PARAMS,
		SPH_BAND_PARAMS
	} DLG_TYPE;

	void SetDlgType(DLG_TYPE);

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
private:
	CSpinButtonCtrl m_merid_spin;
	int m_merid_cnt;
	
	DLG_TYPE        m_type;
public:
	void  SetParams(int meridCnt)
	{
		m_merid_cnt = meridCnt;
		if (m_merid_cnt>36)
			m_merid_cnt=36;
		if (m_merid_cnt<3)
			m_merid_cnt=3;
	};
	void GetParams(int& meridCnt)
	{
		if (m_merid_cnt>36)
			m_merid_cnt=36;
		if (m_merid_cnt<3)
			m_merid_cnt=3;
		meridCnt = m_merid_cnt;
	}
	afx_msg void OnDeltaposMeridSphSpin(NMHDR *pNMHDR, LRESULT *pResult);
	virtual BOOL OnInitDialog();
};
