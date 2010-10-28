/* ==========================================================================
	Class :			CReportLineProperties

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-25

	Purpose :		"CReportBoxProperties" is a "CDiagramPropertyDlg"-derived 
					class for setting properties to a "CReportEntityLine" 
					object

	Description :	This is a Wizard-created dialogbox class.

	Usage :			See "CDiagramPropertyDlg".

   ========================================================================*/

#include "stdafx.h"
#include "ReportLineProperties.h"
#include "ReportEntityLine.h"
#include "ReportEntitySettings.h"

#include "..//Drawer.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CReportLineProperties dialog


CReportLineProperties::CReportLineProperties(IThumbnailerStorage* pParent /*=NULL*/)
	: CDiagramPropertyDlg(CReportLineProperties::IDD, pParent)
/* ============================================================
	Function :		CReportLineProperties::CReportLineProperties
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	CWnd* pParent	-	Window parent
					
	Usage :			

   ============================================================*/
{
	//{{AFX_DATA_INIT(CReportLineProperties)
	m_thickness = 0;
	//}}AFX_DATA_INIT
	m_otiColorCombo = NULL;
	m_otiThicknessCombo = NULL;
}


void CReportLineProperties::DoDataExchange(CDataExchange* pDX)
/* ============================================================
	Function :		CReportLineProperties::DoDataExchange
	Description :	MFC data exchange handler.
	Access :		Protected

	Return :		void
	Parameters :	CDataExchange* pDX	-	Exchange object
					
	Usage :			Called from MFC.

   ============================================================*/
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CReportLineProperties)
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CReportLineProperties, CDialog)
	//{{AFX_MSG_MAP(CReportLineProperties)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CReportLineProperties message handlers

void CReportLineProperties::OnOK() 
/* ============================================================
	Function :		CReportLineProperties::OnOK
	Description :	Handler for the dialog OK-button.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{
	CReportEntityLine* obj = static_cast< CReportEntityLine* >( GetEntity() );

	if (m_otiColorCombo->IsSelected())
	{
		m_otiColorCombo->CommitChanges();
		m_otiColorCombo->LostFocus();
	}
	if (m_otiThicknessCombo->IsSelected())
	{
		m_otiThicknessCombo->CommitChanges();
		m_otiThicknessCombo->LostFocus();
	}
		
	m_color = m_otiColorCombo->GetCurColor();
	m_thickness = m_otiThicknessCombo->GetLineThickness();
	
	obj->SetBorderColor( m_color );
	obj->SetBorderThickness( m_thickness );

	CReportEntitySettings::GetRESInstance()->SetLineColor( m_color );
	CReportEntitySettings::GetRESInstance()->SetLineThickness( m_thickness );

	ShowWindow( SW_HIDE );
	Redraw();

}

void CReportLineProperties::OnCancel() 
/* ============================================================
	Function :		CReportLineProperties::OnCancel
	Description :	Handler for the dialog Cancel-button.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_otiColorCombo->IsSelected())
	{
		m_otiColorCombo->Select(FALSE);
		m_otiColorCombo->LostFocus();
	}
	if (m_otiThicknessCombo->IsSelected())
	{
		m_otiThicknessCombo->Select(FALSE);
		m_otiThicknessCombo->LostFocus();
	}
	ShowWindow( SW_HIDE );

}

BOOL CReportLineProperties::OnInitDialog() 
/* ============================================================
	Function :		CReportLineProperties::OnInitDialog
	Description :	Handler for the "WM_INITDIALOG" messag
	Access :		Protected

	Return :		BOOL	-	Always "TRUE"
	Parameters :	none

	Usage :			Called from MFC

   ============================================================*/
{
	CDialog::OnInitDialog();
	
	COptionTreeItem *otiRoot = NULL;
	COptionTreeItem *otiItem = NULL;

	CRect rcClient;
	DWORD dwStyle, dwOptions;
	LOGFONT lfFont, lfDefaultFont;

	// Get log fonts
	GetFont()->GetLogFont(&lfFont);
	GetFont()->GetLogFont(&lfDefaultFont);
	//strcpy(lfDefaultFont.lfFaceName, _T("Arial"));#OBSOLETE
	strcpy_s(lfDefaultFont.lfFaceName,sizeof(lfDefaultFont.lfFaceName), _T("Arial"));

	// Get the clients rectangle
	GetClientRect(rcClient);

	// Setup the window style
	dwStyle = WS_VISIBLE | WS_CLIPSIBLINGS | WS_CLIPCHILDREN;

	// Setup the tree options 
	// OT_OPTIONS_SHOWINFOWINDOW
	dwOptions = OT_OPTIONS_SHADEEXPANDCOLUMN | OT_OPTIONS_SHADEROOTITEMS | OT_OPTIONS_SHOWINFOWINDOW;

	int  trBot=rcClient.bottom;
	if (GetDlgItem(IDOK))
	{
		CRect rrrr;
		GetDlgItem(IDOK)->GetWindowRect(rrrr);
		ScreenToClient(rrrr);
		trBot = rrrr.top -4;
	}
	// Create tree options
	CRect trR = rcClient;
	trR.bottom = trBot;
	if (m_otTree.Create(dwStyle, trR, this, dwOptions, 1004) == FALSE)
	{
		TRACE0("Failed to create options control.\r\n");
		return FALSE;
	}

	// Want to be notified
	m_otTree.SetNotify(TRUE, this);

	// -- Edit Items
	CString resStr;
	otiRoot = m_otTree.InsertItem(new COptionTreeItem());
	resStr.LoadString(IDS_REP_COMMON_PROPS);
	otiRoot->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_FULL_COMMON_PROPS);
	otiRoot->SetInfoText(resStr);

	m_otiColorCombo = (COptionTreeItemColorComboBox*)m_otTree.InsertItem(new COptionTreeItemColorComboBox(), otiRoot);
	resStr.LoadString(IDS_OB_PR_DL_COLOR);
	m_otiColorCombo->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_LINE_FULL_COL);
	m_otiColorCombo->SetInfoText(resStr);
	if (m_otiColorCombo->CreateComboItem(NULL) == TRUE)
	{
		m_otiColorCombo->SetCurColor(m_color);	
	}
	m_otiThicknessCombo = (COptionTreeItemLineThikComboBox*)m_otTree.InsertItem(new COptionTreeItemLineThikComboBox(), otiRoot);
	resStr.LoadString(IDS_OB_PR_DL_LIN_TH);
	m_otiThicknessCombo->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_LINE_FULL_TH);
	m_otiThicknessCombo->SetInfoText(resStr);
	if (m_otiThicknessCombo->CreateComboItem(NULL) == TRUE)
	{
		m_otiThicknessCombo->SetLineThickness(m_thickness);
	}

	otiRoot->Expand();


	
	return TRUE;

}

void CReportLineProperties::SetValues()
/* ============================================================
	Function :		CReportLineProperties::SetValues
	Description :	Sets the values of the property dialog
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			Call to set the property dialog values.

   ============================================================*/
{

	CReportEntityLine* obj = static_cast< CReportEntityLine* >( GetEntity() );
	m_color = obj->GetBorderColor();
	m_thickness = obj->GetBorderThickness();

	if (m_otiColorCombo)
		m_otiColorCombo->SetCurColor(m_color);	
	if (m_otiThicknessCombo)
		m_otiThicknessCombo->SetLineThickness(m_thickness);

}
