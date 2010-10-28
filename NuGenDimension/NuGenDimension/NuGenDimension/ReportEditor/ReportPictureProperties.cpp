/* ==========================================================================
	Class :			CReportPictureProperties

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-23

	Purpose :		"CReportBoxProperties" is a "CDiagramPropertyDlg"-derived 
					class for setting properties to a "CReportEntityPicture" 
					object

	Description :	This is a Wizard-created dialogbox class.

	Usage :			See "CDiagramPropertyDlg".

   ========================================================================*/

#include "stdafx.h"
#include "ReportPictureProperties.h"
#include "ReportEntityPicture.h"
#include "ReportEntitySettings.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


DocType doctypes[CMAX_IMAGE_FORMATS] =
{
	{ -1, TRUE, TRUE, "Supported files", "*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.ico;*.tif;*.tiff;*.tga;*.pcx;*.wbmp;*.wmf;*.emf;*.j2k;*.jp2;*.jbg;*.j2c;*.jpc;*.pgx;*.pnm;*.pgm;*.ppm;*.ras" },
#if CXIMAGE_SUPPORT_BMP
	{ CXIMAGE_FORMAT_BMP, TRUE, TRUE, "BMP files", "*.bmp" },
#endif
#if CXIMAGE_SUPPORT_GIF
	{ CXIMAGE_FORMAT_GIF, TRUE, TRUE, "GIF files", "*.gif" },
#endif
#if CXIMAGE_SUPPORT_JPG
	{ CXIMAGE_FORMAT_JPG, TRUE, TRUE, "JPG files", "*.jpg;*.jpeg" },
#endif
#if CXIMAGE_SUPPORT_PNG
	{ CXIMAGE_FORMAT_PNG, TRUE, TRUE, "PNG files", "*.png" },
#endif
#if CXIMAGE_SUPPORT_MNG
	{ CXIMAGE_FORMAT_MNG, TRUE, TRUE, "MNG files", "*.mng;*.jng;*.png" },
#endif
#if CXIMAGE_SUPPORT_ICO
	{ CXIMAGE_FORMAT_ICO, TRUE, TRUE, "ICO CUR files", "*.ico;*.cur" },
#endif
#if CXIMAGE_SUPPORT_TIF
	{ CXIMAGE_FORMAT_TIF, TRUE, TRUE, "TIF files", "*.tif;*.tiff" },
#endif
#if CXIMAGE_SUPPORT_TGA
	{ CXIMAGE_FORMAT_TGA, TRUE, TRUE, "TGA files", "*.tga" },
#endif
#if CXIMAGE_SUPPORT_PCX
	{ CXIMAGE_FORMAT_PCX, TRUE, TRUE, "PCX files", "*.pcx" },
#endif
#if CXIMAGE_SUPPORT_WBMP
	{ CXIMAGE_FORMAT_WBMP, TRUE, TRUE, "WBMP files", "*.wbmp" },
#endif
#if CXIMAGE_SUPPORT_WMF
	{ CXIMAGE_FORMAT_WMF, TRUE, FALSE, "WMF EMF files", "*.wmf;*.emf" },
#endif
#if CXIMAGE_SUPPORT_J2K
	{ CXIMAGE_FORMAT_J2K, TRUE, TRUE, "J2K files", "*.j2k;*.jp2" },
#endif
#if CXIMAGE_SUPPORT_JBG
	{ CXIMAGE_FORMAT_JBG, TRUE, TRUE, "JBG files", "*.jbg" },
#endif
#if CXIMAGE_SUPPORT_JP2
	{ CXIMAGE_FORMAT_JP2, TRUE, TRUE, "JP2 files", "*.j2k;*.jp2" },
#endif
#if CXIMAGE_SUPPORT_JPC
	{ CXIMAGE_FORMAT_JPC, TRUE, TRUE, "JPC files", "*.j2c;*.jpc" },
#endif
#if CXIMAGE_SUPPORT_PGX
	{ CXIMAGE_FORMAT_PGX, TRUE, TRUE, "PGX files", "*.pgx" },
#endif
#if CXIMAGE_SUPPORT_RAS
	{ CXIMAGE_FORMAT_RAS, TRUE, TRUE, "RAS files", "*.ras" },
#endif
#if CXIMAGE_SUPPORT_PNM
	{ CXIMAGE_FORMAT_PNM, TRUE, TRUE, "PNM files", "*.pnm;*.pgm;*.ppm" }
#endif
};

/////////////////////////////////////////////////////////////////////////////
// CReportPictureProperties dialog

CReportPictureProperties::CReportPictureProperties(IThumbnailerStorage* pParent /*=NULL*/)
	: CDiagramPropertyDlg(CReportPictureProperties::IDD, pParent)
/* ============================================================
	Function :		CReportPictureProperties::CReportPictureProperties
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	CWnd* pParent	-	Parent window
					
	Usage :			

   ============================================================*/
{
	//{{AFX_DATA_INIT(CReportPictureProperties)
	m_filename = _T("");
	m_borderThickness = 0;
	//}}AFX_DATA_INIT

	m_borderStyle = 0;

	m_otiFile = NULL;

	m_otiExistLeft = NULL;
	m_otiExistRight = NULL;
	m_otiExistTop = NULL;
	m_otiExistBottom= NULL;
	m_otiFrameColorCombo =NULL;
	m_otiFrameThicknessCombo = NULL;


}

void CReportPictureProperties::DoDataExchange(CDataExchange* pDX)
/* ============================================================
	Function :		CReportPictureProperties::DoDataExchange
	Description :	MFC data exchange handler.
	Access :		Protected

	Return :		void
	Parameters :	CDataExchange* pDX	-	Exchange object
					
	Usage :			Called from MFC.

   ============================================================*/
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CReportPictureProperties)
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CReportPictureProperties, CDialog)
	//{{AFX_MSG_MAP(CReportPictureProperties)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CReportPictureProperties message handlers

void CReportPictureProperties::OnOK() 
/* ============================================================
	Function :		CReportPictureProperties::OnOK
	Description :	Handler for the dialog OK-button.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{
	CReportEntityPicture* pic = static_cast< CReportEntityPicture* >( GetEntity() );
	
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

	m_filename = m_otiFile->GetFileDir()+m_otiFile->GetFileName();

	pic->SetFilename( m_filename );
	//pic->AdjustSize();

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

	pic->SetBorderColor( m_borderColor );
	pic->SetBorderThickness( m_borderThickness );
	pic->SetBorderStyle( m_borderStyle );

	CReportEntitySettings::GetRESInstance()->SetBorderColor( m_borderColor );
	CReportEntitySettings::GetRESInstance()->SetBorderThickness( m_borderThickness );
	CReportEntitySettings::GetRESInstance()->SetBorderStyle( m_borderStyle );

	ShowWindow( SW_HIDE );
	Redraw();

}

void CReportPictureProperties::OnCancel() 
/* ============================================================
	Function :		CReportPictureProperties::OnCancel
	Description :	Handler for the dialog Cancel-button.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Called from MFC.

   ============================================================*/
{


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


static int GetIndexFromType(int nDocType, BOOL bOpenFileDialog)
{
	int nCnt = 0;
	for (int i=0;i<CMAX_IMAGE_FORMATS;i++){
		if (bOpenFileDialog ? doctypes[i].bRead : doctypes[i].bWrite){
			if (doctypes[i].nID == nDocType) return nCnt;
			nCnt++;
		}
	}
	return -1;
}


static CString GetExtFromType(int nDocType)
{
	for (int i=0;i<CMAX_IMAGE_FORMATS;i++){
		if (doctypes[i].nID == nDocType)
			return doctypes[i].ext;
	}
	return CString("");
}

static CString GetFileTypes(BOOL bOpenFileDialog)
{
	CString str;
	for (int i=0;i<CMAX_IMAGE_FORMATS;i++){
		if (bOpenFileDialog && doctypes[i].bRead){
			str += doctypes[i].description;
			str += (TCHAR)'|';
			str += doctypes[i].ext;
			str += (TCHAR)'|';
		} else if (!bOpenFileDialog && doctypes[i].bWrite) {
			str += doctypes[i].description;
			str += (TCHAR)NULL;
			str += doctypes[i].ext;
			str += (TCHAR)NULL;
		}
	}
	return str;
}

void CReportPictureProperties::SetValues()
/* ============================================================
	Function :		CReportPictureProperties::SetValues
	Description :	Sets the values of the property dialog
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			Call to set the property dialog values.

   ============================================================*/
{

	CReportEntityPicture* pic = static_cast< CReportEntityPicture* >( GetEntity() );
	
	m_filename = pic->GetFilename();
	m_borderColor = pic->GetBorderColor();
	m_borderStyle = pic->GetBorderStyle();
	m_borderThickness = pic->GetBorderThickness();

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

	if (m_otiFile)
		m_otiFile->AddFileName(m_filename);

}

BOOL CReportPictureProperties::OnInitDialog() 
/* ============================================================
	Function :		CReportPictureProperties::OnInitDialog
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

	m_otiFile = (COptionTreeItemFile*)m_otTree.InsertItem(new COptionTreeItemFile(), otiRoot);
	resStr.LoadString(IDS_REP_PR_FILE);
	m_otiFile->SetLabelText(resStr);
	resStr.LoadString(IDS_REP_PR_FULL_FILE);
	m_otiFile->SetInfoText(resStr);
	if (m_otiFile->CreateFileItem("",GetExtFromType(0).Mid(2,3),GetFileTypes(TRUE),
							OT_FILE_OPENDIALOG | OT_FILE_SHOWFULLPATH, 
							OFN_OVERWRITEPROMPT|OFN_HIDEREADONLY | OFN_FILEMUSTEXIST) == TRUE)
	{
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
