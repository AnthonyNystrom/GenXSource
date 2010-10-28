#pragma once
#include "afxcmn.h"

#include "..//NuGenDimension.h"
// CLayersDlg dialog

class CLayersDlg : public CDialog
{
	DECLARE_DYNAMIC(CLayersDlg)

public:
	CLayersDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CLayersDlg();

// Dialog Data
	enum { IDD = IDD_LAYERS_DLG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnInitDialog();
private:
	CListCtrl m_layers_list;
	std::vector<unsigned char>  m_layers;
public:
	
public:
	afx_msg void OnLvnItemchangedLayersList(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnBnClickedAddLayer();
protected:
	virtual void OnOK();
};
