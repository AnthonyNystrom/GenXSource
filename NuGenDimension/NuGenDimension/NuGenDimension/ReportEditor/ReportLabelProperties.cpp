/* ==========================================================================
	Class :			CReportLabelProperties

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-25

	Purpose :		"CReportBoxProperties" is a "CDiagramPropertyDlg"-derived 
					class for setting properties to a "CReportEntityLabel" 
					object

	Description :	This is a Wizard-created dialogbox class.

	Usage :			See "CDiagramPropertyDlg".

   ========================================================================*/

#include "stdafx.h"
#include "ReportLabelProperties.h"
#include "ReportEntityLabel.h"
#include "ReportEntitySettings.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CReportLabelProperties dialog


CReportLabelProperties::CReportLabelProperties(IThumbnailerStorage* pParent /*=NULL*/)
	: CDiagramPropertyDlg(CReportLabelProperties::IDD, pParent)
/* ============================================================
	Function :		CReportLabelProperties::CReportLabelProperties
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	CWnd* pParent	-	Window parent
					
	Usage :			

   ============================================================*/
{
	//{{AFX_DATA_INIT(CReportLabelProperties)
	m_justification = 0;
	m_angle = 0;
	//}}AFX_DATA_INIT

	m_otiFont = NULL;
	m_text_edit = NULL;
	m_otiAlign_radio = NULL;
	m_otiAngle_radio = NULL;

	m_otiExistLeft = NULL;
	m_otiExistRight = NULL;
	m_otiExistTop = NULL;
	m_otiExistBottom= NULL;
	m_otiFrameColorCombo =NULL;
	m_otiFrameThicknessCombo = NULL;

	m_borderThickness = 0;
	m_borderStyle = 0;

}

void CReportLabelProperties::DoDataExchange(CDataExchange* pDX)
/* ============================================================
	Function :		CReportLabelProperties::DoDataExchange
	Description :	MFC data exchange handler.
	Access :		Protected

	Return :		void
	Parameters :	CDataExchange* pDX	-	Exchange object
					
	Usage :			Called from MFC.

   ============================================================*/
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CReportLabelProperties)
	//}}AFX_DATA_MAP
}

BOOL CReportLabelProperties::OnInitDialog() 

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

	// -- Edit Items
	otiRoot = m_otTree.InsertItem(new COptionTreeItem());
	resStr.LoadString(IDS_REP_LAB);
	otiRoot->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_FULL_LAB);
	otiRoot->SetInfoText(resStr);

	CClientDC	dc( this );
	int inch = dc.GetDeviceCaps( LOGPIXELSX );
	double pt = static_cast< double >( inch ) / 72;

	m_otiFont = (COptionTreeItemFont*)m_otTree.InsertItem(new COptionTreeItemFont(), otiRoot);
	resStr.LoadString(IDS_REP_LAB_FONT);
	m_otiFont->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_LAB_FULL_FONT);
	m_otiFont->SetInfoText(resStr);
	if (m_otiFont->CreateFontItem(&m_lf,pt))
	{
		m_otiFont->SetColor(m_color);
	}
	m_text_edit = (COptionTreeItemEdit*) m_otTree.InsertItem(new COptionTreeItemEdit(), otiRoot);
	resStr.LoadString(IDS_REP_LAB);
	m_text_edit->SetLabelText(resStr);
	m_text_edit->SetInfoText(resStr);
	if (m_text_edit->CreateEditItem(/*OT_EDIT_MULTILINE*/NULL, ES_WANTRETURN | ES_AUTOVSCROLL | ES_AUTOHSCROLL) == TRUE)
	{
		m_text_edit->SetWindowText(_T("Label"));
	}

	m_otiAngle_radio = (COptionTreeItemRadio*)m_otTree.InsertItem(new COptionTreeItemRadio(), otiRoot);
	resStr.LoadString(IDS_REPORT_TEXT_ANG);
	m_otiAngle_radio->SetLabelText(resStr);
	resStr.LoadString(IDS_REPORT_FULL_TEXT_ANG);
	m_otiAngle_radio->SetInfoText(resStr);
	if (m_otiAngle_radio->CreateRadioItem() == TRUE)
	{
		resStr.LoadString(IDS_REPORT_0_ANG);
		m_otiAngle_radio->InsertNewRadio(resStr, FALSE);
		resStr.LoadString(IDS_REPORT_90_ANG);
		m_otiAngle_radio->InsertNewRadio(resStr, FALSE);
		resStr.LoadString(IDS_REPORT_M90_ANG);
		m_otiAngle_radio->InsertNewRadio(resStr, FALSE);
		resStr.LoadString(IDS_REPORT_180_ANG);
		m_otiAngle_radio->InsertNewRadio(resStr, FALSE);
	}

	m_otiAlign_radio = (COptionTreeItemRadio*)m_otTree.InsertItem(new COptionTreeItemRadio(), otiRoot);
	resStr.LoadString(IDS_REP_LAB_ALIGN);
	m_otiAlign_radio->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_LAB_FULL_ALIGN);
	m_otiAlign_radio->SetInfoText(resStr);
	if (m_otiAlign_radio->CreateRadioItem() == TRUE)
	{
		resStr.LoadString(IDS_REP_LAB_ALIGN_L);
		m_otiAlign_radio->InsertNewRadio(resStr, FALSE);
		resStr.LoadString(IDS_REP_LAB_ALIGN_C);
		m_otiAlign_radio->InsertNewRadio(resStr, FALSE);
		resStr.LoadString(IDS_REP_LAB_ALIGN_R);
		m_otiAlign_radio->InsertNewRadio(resStr, FALSE);
	}
	

	otiRoot->Expand();

	otiRoot = m_otTree.InsertItem(new COptionTreeItem());
	resStr.LoadString(IDS_REP_PR_DL_FRAME);
	otiRoot->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_PR_DL_FULL_FRAME);
	otiRoot->SetInfoText(resStr);

	CString yesS;
	yesS.LoadString(IDS_YES);
	CString noS;
	noS.LoadString(IDS_NO);

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

BEGIN_MESSAGE_MAP(CReportLabelProperties, CDialog)
	//{{AFX_MSG_MAP(CReportLabelProperties)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CReportLabelProperties message handlers

void CReportLabelProperties::OnOK() 
/* ============================================================
	Function :		CReportLabelProperties::OnOK
	Description :	Handler for the dialog OK-button.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	CReportEntityLabel* label = static_cast< CReportEntityLabel* >( GetEntity() );

	if (m_text_edit->IsSelected())
	{
		m_text_edit->CommitChanges();
		m_text_edit->LostFocus();
	}
	if (m_otiAngle_radio->IsSelected())
	{
		m_otiAngle_radio->CommitChanges();
		m_otiAngle_radio->LostFocus();
	}
	if (m_otiAlign_radio->IsSelected())
	{
		m_otiAlign_radio->CommitChanges();
		m_otiAlign_radio->LostFocus();
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

	memcpy(&m_lf,&m_otiFont->m_lf,sizeof(LOGFONT));
	m_color = m_otiFont->GetColor();

	label->SetFontName( m_lf.lfFaceName );
	label->SetCharSet(m_lf.lfCharSet);
	label->SetFontSize( m_lf.lfHeight );
	label->SetFontColor( m_color );
	label->SetFontBold( ( m_lf.lfWeight == FW_BOLD ) );
	label->SetFontItalic( m_lf.lfItalic );
	label->SetFontUnderline( m_lf.lfUnderline );
	label->SetFontStrikeout( m_lf.lfStrikeOut );

	CString sss;
	m_text_edit->GetWindowText(sss);
	label->SetTitle(sss);

	switch(m_otiAngle_radio->GetCheckedRadio()) {
	case 0:
		m_angle = 0;
		break;
	case 1:
		m_angle = 90;
		break;
	case 2:
		m_angle = -90;
		break;
	case 3:
		m_angle = 180;
		break;
	default:
		ASSERT(0);
		m_angle = 0;
	}

	m_justification = m_otiAlign_radio->GetCheckedRadio();


	
	if( m_justification == 0 )
		label->SetJustification( DT_LEFT );
	if( m_justification == 1 )
		label->SetJustification( DT_CENTER );
	if( m_justification == 2 )
		label->SetJustification( DT_RIGHT );

	label->SetAngle(m_angle);

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

	label->SetBorderColor( m_borderColor );
	label->SetBorderThickness( m_borderThickness );
	label->SetBorderStyle( m_borderStyle );

	CReportEntitySettings::GetRESInstance()->SetLogFont( m_lf );
	CReportEntitySettings::GetRESInstance()->SetColor( m_color );
	CReportEntitySettings::GetRESInstance()->SetJustification( label->GetJustification() );
	CReportEntitySettings::GetRESInstance()->SetBorderColor( m_borderColor );
	CReportEntitySettings::GetRESInstance()->SetBorderThickness( m_borderThickness );
	CReportEntitySettings::GetRESInstance()->SetBorderStyle( m_borderStyle );

	ShowWindow( SW_HIDE );
	Redraw();

}

void CReportLabelProperties::OnCancel() 
/* ============================================================
	Function :		CReportLabelProperties::OnCancel
	Description :	Handler for the dialog Cancel-button.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{

	if (m_text_edit->IsSelected())
	{
		m_text_edit->Select(FALSE);
		m_text_edit->LostFocus();
	}
	if (m_otiAngle_radio->IsSelected())
	{
		m_otiAngle_radio->Select(FALSE);
		m_otiAngle_radio->LostFocus();
	}
	if (m_otiAlign_radio->IsSelected())
	{
		m_otiAlign_radio->Select(FALSE);
		m_otiAlign_radio->LostFocus();
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

//void CReportLabelProperties::OnButtonFont() 
/* ============================================================
	Function :		CReportLabelProperties::OnButtonFont
	Description :	Handler for the dialog button Font
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
/*{

	CClientDC	dc( this );
	int inch = dc.GetDeviceCaps( LOGPIXELSX );
	double pt = static_cast< double >( inch ) / 72;
	int height = m_lf.lfHeight / 10;
	height = round( static_cast< double >( height ) * pt );
	m_lf.lfHeight = -( height );

	CFontDialog	dlg( &m_lf );
	dlg.m_cf.rgbColors = m_color;
	dlg.m_cf.Flags |= CF_EFFECTS;
	if( dlg.DoModal() == IDOK )
	{

		dlg.GetCurrentFont( &m_lf );

		m_lf.lfHeight = dlg.GetSize();

		m_color = dlg.GetColor();

	}

}*/

void CReportLabelProperties::SetValues() 
/* ============================================================
	Function :		CReportLabelProperties::SetValues
	Description :	Sets the values of the property dialog
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			Call to set the property dialog values.

   ============================================================*/
{

	CReportEntityLabel* label = static_cast< CReportEntityLabel* >( GetEntity() );

	ZeroMemory( &m_lf, sizeof( m_lf ) );

	m_lf.lfHeight = label->GetFontSize();
	lstrcpy( m_lf.lfFaceName, label->GetFontName() );
	m_lf.lfCharSet = label->GetCharSet();

	if( label->GetFontBold() )
		m_lf.lfWeight = FW_BOLD;
	else
		m_lf.lfWeight = FW_NORMAL;

	if( label->GetFontUnderline() )
		m_lf.lfUnderline = TRUE;

	if( label->GetFontItalic() )
		m_lf.lfItalic = TRUE;

	if( label->GetFontStrikeout() )
		m_lf.lfStrikeOut = TRUE;

	m_color = label->GetFontColor();
	if (m_otiFont)
	{
		memcpy(&m_otiFont->m_lf,&m_lf,sizeof(LOGFONT));
		m_otiFont->SetColor(m_color);
	}

	if (m_text_edit)
		m_text_edit->SetWindowText(label->GetTitle());
	

	if( label->GetJustification() == DT_LEFT )
		m_justification = 0;

	if( label->GetJustification() == DT_CENTER )
		m_justification = 1;

	if( label->GetJustification() == DT_RIGHT )
		m_justification = 2;

	m_angle = label->GetAngle();

	CString resStr;

	if (m_otiAngle_radio)
	{
		m_otiAngle_radio->Node_DeleteAll();
		resStr.LoadString(IDS_REPORT_0_ANG);
		m_otiAngle_radio->InsertNewRadio(resStr, m_angle==0);
		resStr.LoadString(IDS_REPORT_90_ANG);
		m_otiAngle_radio->InsertNewRadio(resStr, m_angle==90);
		resStr.LoadString(IDS_REPORT_M90_ANG);
		m_otiAngle_radio->InsertNewRadio(resStr, m_angle==-90);
		resStr.LoadString(IDS_REPORT_180_ANG);
		m_otiAngle_radio->InsertNewRadio(resStr, m_angle==180);
	}

	if (m_otiAlign_radio)
	{
		m_otiAlign_radio->Node_DeleteAll();
		resStr.LoadString(IDS_REP_LAB_ALIGN_L);
		m_otiAlign_radio->InsertNewRadio(resStr, m_justification==0);
		resStr.LoadString(IDS_REP_LAB_ALIGN_C);
		m_otiAlign_radio->InsertNewRadio(resStr, m_justification==1);
		resStr.LoadString(IDS_REP_LAB_ALIGN_R);
		m_otiAlign_radio->InsertNewRadio(resStr, m_justification==2);
	}

	m_borderColor = label->GetBorderColor();
	m_borderThickness = label->GetBorderThickness();
	m_borderStyle = label->GetBorderStyle();

	if( m_otiFrameColorCombo )
		m_otiFrameColorCombo->SetCurColor( m_borderColor );

	if (m_otiFrameThicknessCombo)
		m_otiFrameThicknessCombo->SetLineThickness(m_borderThickness);

	if (m_otiExistLeft)
		m_otiExistLeft->SetCheck(m_borderStyle&DIAGRAM_FRAME_STYLE_LEFT);
	if (m_otiExistRight)
		m_otiExistRight->SetCheck((m_borderStyle&DIAGRAM_FRAME_STYLE_RIGHT)?TRUE:FALSE);
	if (m_otiExistTop)
		m_otiExistTop->SetCheck((m_borderStyle&DIAGRAM_FRAME_STYLE_TOP)?TRUE:FALSE);
	if (m_otiExistBottom)
		m_otiExistBottom->SetCheck((m_borderStyle&DIAGRAM_FRAME_STYLE_BOTTOM)?TRUE:FALSE);

}

