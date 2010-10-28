// DraftTemplateNameDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "DraftTemplateNameDlg.h"
#include ".\drafttemplatenamedlg.h"


// CDraftTemplateNameDlg dialog

IMPLEMENT_DYNAMIC(CDraftTemplateNameDlg, CDialog)
CDraftTemplateNameDlg::CDraftTemplateNameDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CDraftTemplateNameDlg::IDD, pParent)
{
}

CDraftTemplateNameDlg::~CDraftTemplateNameDlg()
{
}

void CDraftTemplateNameDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_TEMPLATE_NAME_EDIT, m_edit);
}


BEGIN_MESSAGE_MAP(CDraftTemplateNameDlg, CDialog)
END_MESSAGE_MAP()


// CDraftTemplateNameDlg message handlers

void CDraftTemplateNameDlg::OnOK()
{
	m_edit.GetWindowText(m_saving_name);
	CDialog::OnOK();
}
