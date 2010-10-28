/* ==========================================================================
	Class :			CReportBoxProperties

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-25


	Purpose :		"CReportBoxProperties" is a "CDiagramPropertyDlg"-derived 
					class for setting properties to a "CReportEntityBox" 
					object

	Description :	This is a Wizard-created dialogbox class.

	Usage :			See "CDiagramPropertyDlg".

   ========================================================================*/

#include "stdafx.h"
#include "ReportBoxProperties.h"
#include "ReportEntityBox.h"
#include "ReportEntitySettings.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CReportBoxProperties dialog

CReportBoxProperties::CReportBoxProperties(IThumbnailerStorage* pParent /*=NULL*/)
	: CDiagramPropertyDlg(CReportBoxProperties::IDD, pParent)
/* ============================================================
	Function :		CReportBoxProperties::CReportBoxProperties
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	CWnd* pParent	-	Parent window
					
	Usage :			

   ============================================================*/
{
	//{{AFX_DATA_INIT(CReportBoxProperties)
	m_borderThickness = 0;
	m_fill = FALSE;
	//}}AFX_DATA_INIT

	m_borderStyle = 0;

	m_otiExistFill = NULL;
	m_otiFillColorCombo = NULL;

	m_otiExistLeft = NULL;
	m_otiExistRight = NULL;
	m_otiExistTop = NULL;
	m_otiExistBottom= NULL;
	m_otiFrameColorCombo =NULL;
	m_otiFrameThicknessCombo = NULL;
	
}

void CReportBoxProperties::DoDataExchange(CDataExchange* pDX)
/* ============================================================
	Function :		CReportBoxProperties::DoDataExchange
	Description :	MFC data exchange handler.
	Access :		Protected

	Return :		void
	Parameters :	CDataExchange* pDX	-	Exchange object
					
	Usage :			Called from MFC.

   ============================================================*/
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CReportBoxProperties)
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CReportBoxProperties, CDialog)
	//{{AFX_MSG_MAP(CReportBoxProperties)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CReportBoxProperties message handlers

void CReportBoxProperties::OnOK() 
/* ============================================================
	Function :		CReportBoxProperties::OnOK
	Description :	Handler for the dialog OK-button.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{
	CReportEntityBox* obj = static_cast< CReportEntityBox* >( GetEntity() );

	if (m_otiExistFill->IsSelected())
	{
		m_otiExistFill->CommitChanges();
		m_otiExistFill->LostFocus();
	}
	if (m_otiFillColorCombo->IsSelected())
	{
		m_otiFillColorCombo->CommitChanges();
		m_otiFillColorCombo->LostFocus();
	}
	if (m_otiExistLeft->IsSelected())
	{
		m_otiExistLeft->CommitChanges();
		m_otiExistLeft->LostFocus();
	}
	if (m_otiExistRight->IsSelected())
	{
		m_otiExistRight->CommitChanges();
		m_otiExistRight->LostFocus();
	}
	if (m_otiExistTop->IsSelected())
	{
		m_otiExistTop->CommitChanges();
		m_otiExistTop->LostFocus();
	}
	if (m_otiExistBottom->IsSelected())
	{
		m_otiExistBottom->CommitChanges();
		m_otiExistBottom->LostFocus();
	}
	if (m_otiFrameColorCombo->IsSelected())
	{
		m_otiFrameColorCombo->CommitChanges();
		m_otiFrameColorCombo->LostFocus();
	}
	if (m_otiFrameThicknessCombo->IsSelected())
	{
		m_otiFrameThicknessCombo->CommitChanges();
		m_otiFrameThicknessCombo->LostFocus();
	}

	m_fill = m_otiExistFill->GetCheck();
	m_fillColor = m_otiFillColorCombo->GetCurColor();

	m_borderStyle = 0;
	if (m_otiExistLeft->GetCheck())
		m_borderStyle |= DIAGRAM_FRAME_STYLE_LEFT;
	if (m_otiExistRight->GetCheck())
		m_borderStyle |= DIAGRAM_FRAME_STYLE_RIGHT;
	if (m_otiExistTop->GetCheck())
		m_borderStyle |= DIAGRAM_FRAME_STYLE_TOP;
	if (m_otiExistBottom->GetCheck())
		m_borderStyle |= DIAGRAM_FRAME_STYLE_BOTTOM;

	m_borderColor = m_otiFrameColorCombo->GetCurColor();
	m_borderThickness = m_otiFrameThicknessCombo->GetLineThickness();

	obj->SetBorderColor( m_borderColor );
	obj->SetBorderThickness( m_borderThickness );
	obj->SetFill( m_fill );
	obj->SetFillColor( m_fillColor );
    obj->SetBorderStyle( m_borderStyle );

	CReportEntitySettings::GetRESInstance()->SetFill( m_fill );
	CReportEntitySettings::GetRESInstance()->SetFillColor( m_fillColor );
	CReportEntitySettings::GetRESInstance()->SetBorderColor( m_borderColor );
	CReportEntitySettings::GetRESInstance()->SetBorderThickness( m_borderThickness );
	CReportEntitySettings::GetRESInstance()->SetBorderStyle( m_borderStyle );

	ShowWindow( SW_HIDE );
	Redraw();

}

void CReportBoxProperties::OnCancel() 
/* ============================================================
	Function :		CReportBoxProperties::OnCancel
	Description :	Handler for the dialog Cancel-button.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_otiExistFill->IsSelected())
	{
		m_otiExistFill->Select(FALSE);
		m_otiExistFill->LostFocus();
	}
	if (m_otiFillColorCombo->IsSelected())
	{
		m_otiFillColorCombo->Select(FALSE);
		m_otiFillColorCombo->LostFocus();
	}
	if (m_otiExistLeft->IsSelected())
	{
		m_otiExistLeft->Select(FALSE);
		m_otiExistLeft->LostFocus();
	}
	if (m_otiExistRight->IsSelected())
	{
		m_otiExistRight->Select(FALSE);
		m_otiExistRight->LostFocus();
	}
	if (m_otiExistTop->IsSelected())
	{
		m_otiExistTop->Select(FALSE);
		m_otiExistTop->LostFocus();
	}
	if (m_otiExistBottom->IsSelected())
	{
		m_otiExistBottom->Select(FALSE);
		m_otiExistBottom->LostFocus();
	}
	if (m_otiFrameColorCombo->IsSelected())
	{
		m_otiFrameColorCombo->Select(FALSE);
		m_otiFrameColorCombo->LostFocus();
	}
	if (m_otiFrameThicknessCombo->IsSelected())
	{
		m_otiFrameThicknessCombo->Select(FALSE);
		m_otiFrameThicknessCombo->LostFocus();
	}

	ShowWindow( SW_HIDE );

}

BOOL CReportBoxProperties::OnInitDialog() 
/* ============================================================
	Function :		CReportBoxProperties::OnInitDialog
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

	CString resStr;

	CString yesS;
	yesS.LoadString(IDS_YES);
	CString noS;
	noS.LoadString(IDS_NO);

	// -- Edit Items
	otiRoot = m_otTree.InsertItem(new COptionTreeItem());
	resStr.LoadString(IDS_REP_BOX_FILL);
	otiRoot->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_BOX_FULL_FILL);
	otiRoot->SetInfoText(resStr);

	m_otiExistFill = (COptionTreeItemCheckBox*)m_otTree.InsertItem(new COptionTreeItemCheckBox(), otiRoot);
	resStr.LoadString(IDS_REP_BOX_IS_FILL);
	m_otiExistFill->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_BOX_FULL_IS_FILL);
	m_otiExistFill->SetInfoText(resStr);
	if (m_otiExistFill->CreateCheckBoxItem(m_fill, OT_CHECKBOX_SHOWCHECK) == TRUE)
	{
		m_otiExistFill->SetCheckText(yesS, noS);
	}

	m_otiFillColorCombo = (COptionTreeItemColorComboBox*)m_otTree.InsertItem(new COptionTreeItemColorComboBox(), otiRoot);
	resStr.LoadString(IDS_OB_PR_DL_COLOR);
	m_otiFillColorCombo->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_BOX_FULL_COLOR);
	m_otiFillColorCombo->SetInfoText(resStr);
	if (m_otiFillColorCombo->CreateComboItem(NULL) == TRUE)
	{
		m_otiFillColorCombo->SetCurColor(m_fillColor);	
	}

	otiRoot->Expand();

	otiRoot = m_otTree.InsertItem(new COptionTreeItem());
	resStr.LoadString(IDS_REP_PR_DL_FRAME);
	otiRoot->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_PR_DL_FULL_FRAME);
	otiRoot->SetInfoText(resStr);

	m_otiExistLeft = (COptionTreeItemCheckBox*)m_otTree.InsertItem(new COptionTreeItemCheckBox(), otiRoot);
	resStr.LoadString(IDS_REP_PR_DL_LEFT);
	m_otiExistLeft->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_PR_DL_FULL_LEFT);
	m_otiExistLeft->SetInfoText(resStr);
	if (m_otiExistLeft->CreateCheckBoxItem(m_borderStyle&DIAGRAM_FRAME_STYLE_LEFT, OT_CHECKBOX_SHOWCHECK) == TRUE)
	{
		m_otiExistLeft->SetCheckText(yesS, noS);
	}
	m_otiExistRight = (COptionTreeItemCheckBox*)m_otTree.InsertItem(new COptionTreeItemCheckBox(), otiRoot);
	resStr.LoadString(IDS_REP_PR_DL_RIGHT);
	m_otiExistRight->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_PR_DL_FULL_RIGHT);
	m_otiExistRight->SetInfoText(resStr);
	if (m_otiExistRight->CreateCheckBoxItem(m_borderStyle&DIAGRAM_FRAME_STYLE_RIGHT, OT_CHECKBOX_SHOWCHECK) == TRUE)
	{
		m_otiExistRight->SetCheckText(yesS, noS);
	}
	m_otiExistTop = (COptionTreeItemCheckBox*)m_otTree.InsertItem(new COptionTreeItemCheckBox(), otiRoot);
	resStr.LoadString(IDS_REP_PR_DL_TOP);
	m_otiExistTop->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_PR_DL_FULL_TOP);
	m_otiExistTop->SetInfoText(resStr);
	if (m_otiExistTop->CreateCheckBoxItem(m_borderStyle&DIAGRAM_FRAME_STYLE_TOP, OT_CHECKBOX_SHOWCHECK) == TRUE)
	{
		m_otiExistTop->SetCheckText(yesS, noS);
	}
	m_otiExistBottom = (COptionTreeItemCheckBox*)m_otTree.InsertItem(new COptionTreeItemCheckBox(), otiRoot);
	resStr.LoadString(IDS_REP_PR_DL_BOTTOM);
	m_otiExistBottom->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_PR_DL_FULL_BOTTOM);
	m_otiExistBottom->SetInfoText(resStr);
	if (m_otiExistBottom->CreateCheckBoxItem(m_borderStyle&DIAGRAM_FRAME_STYLE_BOTTOM, OT_CHECKBOX_SHOWCHECK) == TRUE)
	{
		m_otiExistBottom->SetCheckText(yesS, noS);
	}
	m_otiFrameColorCombo = (COptionTreeItemColorComboBox*)m_otTree.InsertItem(new COptionTreeItemColorComboBox(), otiRoot);
	resStr.LoadString(IDS_PER_PR_DL_COL_FR);
	m_otiFrameColorCombo->SetLabelText(resStr);
	resStr.LoadString(IDS_PER_PR_DL_FULL_COL_FR);
	m_otiFrameColorCombo->SetInfoText(resStr);
	if (m_otiFrameColorCombo->CreateComboItem(NULL) == TRUE)
	{
		m_otiFrameColorCombo->SetCurColor(m_borderColor);	
	}
	m_otiFrameThicknessCombo = (COptionTreeItemLineThikComboBox*)m_otTree.InsertItem(new COptionTreeItemLineThikComboBox(), otiRoot);
	resStr.LoadString(IDS_PER_PR_DL_TH_FR);
	m_otiFrameThicknessCombo->SetLabelText(resStr);
	resStr.LoadString(IDS_PER_PR_DL_FULL_TH_FR);
	m_otiFrameThicknessCombo->SetInfoText(resStr);
	if (m_otiFrameThicknessCombo->CreateComboItem(NULL) == TRUE)
	{
		m_otiFrameThicknessCombo->SetLineThickness(m_borderThickness);
	}

	otiRoot->Expand();
		


	return TRUE;

}

void CReportBoxProperties::SetValues()
/* ============================================================
	Function :		CReportBoxProperties::SetValues
	Description :	Sets the data in the box from the attached 
					object.
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			Called from the object

   ============================================================*/
{

	CReportEntityBox* obj = static_cast< CReportEntityBox* >( GetEntity() );

	m_borderColor = obj->GetBorderColor();
	m_fillColor = obj->GetFillColor();
	m_borderThickness = obj->GetBorderThickness();

	if( m_otiFillColorCombo )
		m_otiFillColorCombo->SetCurColor( m_fillColor );

	if( m_otiFrameColorCombo )
		m_otiFrameColorCombo->SetCurColor( m_borderColor );

	if (m_otiFrameThicknessCombo)
		m_otiFrameThicknessCombo->SetLineThickness(m_borderThickness);

	m_fill = obj->GetFill();

	if (m_otiExistFill)
		m_otiExistFill->SetCheck(m_fill);

	m_borderStyle = obj->GetBorderStyle();

	if (m_otiExistLeft)
		m_otiExistLeft->SetCheck(m_borderStyle&DIAGRAM_FRAME_STYLE_LEFT);
	if (m_otiExistRight)
		m_otiExistRight->SetCheck((m_borderStyle&DIAGRAM_FRAME_STYLE_RIGHT)?TRUE:FALSE);
	if (m_otiExistTop)
		m_otiExistTop->SetCheck((m_borderStyle&DIAGRAM_FRAME_STYLE_TOP)?TRUE:FALSE);
	if (m_otiExistBottom)
		m_otiExistBottom->SetCheck((m_borderStyle&DIAGRAM_FRAME_STYLE_BOTTOM)?TRUE:FALSE);
}
