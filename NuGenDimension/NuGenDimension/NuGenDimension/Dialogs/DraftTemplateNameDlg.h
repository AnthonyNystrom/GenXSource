#pragma once

#include "..//resource.h"
#include "afxwin.h"
// CDraftTemplateNameDlg dialog

class CDraftTemplateNameDlg : public CDialog
{
	DECLARE_DYNAMIC(CDraftTemplateNameDlg)

public:
	CDraftTemplateNameDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CDraftTemplateNameDlg();

// Dialog Data
	enum { IDD = IDD_DRAFT_TEMPLATE_NAME_DLG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
private:
	CEdit m_edit;
	CString m_saving_name;
public:
	void  GetName(CString& nn) {nn = m_saving_name;};
protected:
	virtual void OnOK();
};
