#pragma once

#include "..//Resource.h"
#include "afxcmn.h"

// CSphereParamsDlg dialog

class CTorusParamsDlg : public CDialog
{
	DECLARE_DYNAMIC(CTorusParamsDlg)

public:
	CTorusParamsDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CTorusParamsDlg();

// Dialog Data
	enum { IDD = IDD_TORUS_EDIT_DLG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
private:
	CSpinButtonCtrl m_merid1_spin;
	CSpinButtonCtrl m_merid2_spin;
	int m_merid1_cnt;
	int m_merid2_cnt;
public:
	void  SetParams(int meridCnt, int ParalCnt)
	{
		m_merid1_cnt = meridCnt;
		m_merid2_cnt = ParalCnt;
		if (m_merid1_cnt>36)
			m_merid1_cnt=36;
		if (m_merid1_cnt<3)
			m_merid1_cnt=3;
		if (m_merid2_cnt>36)
			m_merid2_cnt=36;
		if (m_merid2_cnt<3)
			m_merid2_cnt=3;
	};
	void GetParams(int& meridCnt, int& ParalCnt)
	{
		if (m_merid1_cnt>36)
			m_merid1_cnt=36;
		if (m_merid1_cnt<3)
			m_merid1_cnt=3;
		if (m_merid2_cnt>36)
			m_merid2_cnt=36;
		if (m_merid2_cnt<3)
			m_merid2_cnt=3;
		meridCnt = m_merid1_cnt;
		ParalCnt = m_merid2_cnt;
	}
	afx_msg void OnDeltaposMerid1SphSpin(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnDeltaposMerid2SphSpin(NMHDR *pNMHDR, LRESULT *pResult);
	virtual BOOL OnInitDialog();
};
