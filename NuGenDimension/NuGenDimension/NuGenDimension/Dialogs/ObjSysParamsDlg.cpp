// ObjSysParamsDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "ObjSysParamsDlg.h"

// CObjSysParamsDlg dialog

IMPLEMENT_DYNCREATE(CObjSysParamsDlg, CDialog)

CObjSysParamsDlg::CObjSysParamsDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CObjSysParamsDlg::IDD, pParent)
	, m_editing_object(NULL)
{
	m_otiEdit = NULL;
	m_otiLayerCombo = NULL;
	m_otiColorCombo = NULL;
	m_otiThicknessCombo = NULL;
	m_otiTypeCombo = NULL;
	m_otiFrameOrSolidVis_radio = NULL;
	m_otiVis_check = NULL;
	m_otiGabVis_check = NULL;
}

CObjSysParamsDlg::~CObjSysParamsDlg()
{
}

void CObjSysParamsDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BOOL CObjSysParamsDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	if (!m_editing_object)
	{
		ASSERT(0);
		return FALSE;
	}
	
	/*m_layer_combo.ResetContent();
	CString  labl;
	labl.LoadString(IDS_LAYER_LABEL);
	CString  curLbl;
	for (unsigned char i=0;i<5;i++)
	{
		curLbl.Format(" %i",i);
		m_layer_combo.AddString(labl+curLbl);
	}

	m_layer_combo.SetCurSel(0);*/

	//m_color_combo.SetItemHeight(-1,5);
	
	//m_line_thickness_combo.SetCurSel(0);

	//m_line_type_combo.SetCurSel(0);

	/*m_visible = !(m_draw_state & DRAW_STYLE_HIDE);
	m_frame = (m_draw_state & DRAW_STYLE_FRAME);
	m_gab = (m_draw_state & DRAW_STYLE_BOX);
	m_solid = (m_draw_state & DRAW_STYLE_FULL);*/

	/************************************************************************/
	/*                                                                      */
	/************************************************************************/
	COptionTreeItem *otiRoot = NULL;
	COptionTreeItem *otiItem = NULL;
	COptionTreeItemStatic *otiStatic = NULL;
	/*COptionTreeItemCheckBox *otiCheck = NULL;
	COptionTreeItemRadio *otiRadio = NULL;
	COptionTreeItemSpinner *otiSpinner = NULL;
	COptionTreeItemColor *otiColor = NULL;
	COptionTreeItemHyperLink *oti_Hyperlink = NULL;*/
	CRect rcClient;
	DWORD dwStyle, dwOptions;
	LOGFONT lfFont, lfDefaultFont;

	// Get log fonts
	GetFont()->GetLogFont(&lfFont);
	GetFont()->GetLogFont(&lfDefaultFont);
	//strcpy(lfDefaultFont.lfFaceName, _T("Arial"));#OBSOLETE
	strcpy_s(lfDefaultFont.lfFaceName, sizeof (lfDefaultFont.lfFaceName) / sizeof (lfDefaultFont.lfFaceName[0]), "Arial");


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

	// Setup options tree
	// -- Style Test
/*	otiRoot = m_otTree.InsertItem(new COptionTreeItem());
	otiRoot->SetLabelText(_T("Style Test"));
	otiRoot->SetInfoText(_T("This is an example of numerous styles."));
	otiRoot->SetRootBackgroundColor(RGB(0, 150, 200));
	otiRoot->SetLabelTextColor(RGB(0, 0, 0));
	// -- Style Test-> Label Text Color
	otiStatic = (COptionTreeItemStatic*) m_otTree.InsertItem(new COptionTreeItemStatic(), otiRoot);
	otiStatic->SetLabelText(_T("Label Text Color"));
	otiStatic->SetInfoText(_T("This is an example of setting the label text color."));
	otiStatic->SetStaticText(_T("I am a static item."));
	if (otiStatic->CreateStaticItem(0) == TRUE)
	{
		otiStatic->SetLabelTextColor(RGB(0, 0, 255));
	}
	// -- Style Test-> Label Background Color
	otiStatic = (COptionTreeItemStatic*) m_otTree.InsertItem(new COptionTreeItemStatic(), otiRoot);
	otiStatic->SetLabelText(_T("Label Background Color"));
	otiStatic->SetInfoText(_T("This is an example of setting the label background color."));
	otiStatic->SetStaticText(_T("I am a static item."));
	if (otiStatic->CreateStaticItem(0) == TRUE)
	{
		otiStatic->SetLabelBackgroundColor(RGB(0, 150, 200));
	}
	// -- Style Test-> Text Color
	otiStatic = (COptionTreeItemStatic*) m_otTree.InsertItem(new COptionTreeItemStatic(), otiRoot);
	otiStatic->SetLabelText(_T("Text Color"));
	otiStatic->SetInfoText(_T("This is an example of setting the text color of an item."));
	otiStatic->SetStaticText(_T("I am a static item."));
	if (otiStatic->CreateStaticItem(0) == TRUE)
	{
		otiStatic->SetTextColor(RGB(0, 0, 255));
	}
	// -- Style Test-> Background Color
	otiStatic = (COptionTreeItemStatic*) m_otTree.InsertItem(new COptionTreeItemStatic(), otiRoot);
	otiStatic->SetLabelText(_T("Background Color"));
	otiStatic->SetInfoText(_T("This is an example of setting the background color of an item."));
	otiStatic->SetStaticText(_T("I am a static item."));
	if (otiStatic->CreateStaticItem(0) == TRUE)
	{
		otiStatic->SetBackgroundColor(RGB(0, 150, 200));
	}
*/
	// -- Edit Items

	CString resStr;
	otiRoot = m_otTree.InsertItem(new COptionTreeItem());
	resStr.LoadString(IDS_OB_PR_DL_ATTRIBUTES);
	otiRoot->SetLabelText(resStr);
	resStr.LoadString(IDS_OB_PR_DL_FULL_ATTRIBUTES);
	otiRoot->SetInfoText(resStr);
	// -- Edit Items -> Edit Item
	m_otiEdit = (COptionTreeItemEdit*)m_otTree.InsertItem(new COptionTreeItemEdit(), otiRoot);
	resStr.LoadString(IDS_OB_PR_DL_NAME);
	m_otiEdit->SetLabelText(resStr);
	resStr.LoadString(IDS_OB_PR_DL_FULL_NAME);
	m_otiEdit->SetInfoText(resStr);
	if (m_otiEdit->CreateEditItem(NULL, NULL) == TRUE)
	{
		m_otiEdit->SetWindowText(m_editing_object->GetName());	
	}

	m_otiLayerCombo = (COptionTreeItemComboBox*)m_otTree.InsertItem(new COptionTreeItemComboBox(), otiRoot);
	resStr.LoadString(IDS_OB_PR_DL_LAYER);
	m_otiLayerCombo->SetLabelText(resStr);
	resStr.LoadString(IDS_OB_PR_DL_FULL_LAYER);
	m_otiLayerCombo->SetInfoText(resStr);
	resStr.LoadString(IDS_OB_PR_DL_LAYER);
	if (m_otiLayerCombo->CreateComboItem(NULL) == TRUE)
	{
		CString  labl;
		labl.LoadString(IDS_LAYER_LABEL);
		CString  curLbl;
		unsigned char lCnt = 1;//sgGetScene()->GetLayersCount();
		for (unsigned char i=0;i<lCnt;i++)
		{
			curLbl.Format(" %i",i);
			m_otiLayerCombo->AddString(labl+curLbl);
		}
		m_otiLayerCombo->SelectString(m_editing_object->GetAttribute(SG_OA_LAYER), resStr);
	}
	// -- Edit Items -> Multiline
	/*otiEdit = (COptionTreeItemEdit*) m_otTree.InsertItem(new COptionTreeItemEdit(), otiRoot);
	otiEdit->SetLabelText(_T("Multiline"));
	otiEdit->SetInfoText(_T("This is an example of a multiline edit item."));
	if (otiEdit->CreateEditItem(OT_EDIT_MULTILINE, ES_WANTRETURN | ES_AUTOVSCROLL) == TRUE)
	{
		otiEdit->SetWindowText(_T("I am a multiline edit item.\r\nHere is my second line."));
	}
	// -- Edit Items -> Password
	otiEdit = (COptionTreeItemEdit*) m_otTree.InsertItem(new COptionTreeItemEdit(), otiRoot);
	otiEdit->SetLabelText(_T("Password"));
	otiEdit->SetInfoText(_T("This is an example of a password edit item."));
	if (otiEdit->CreateEditItem(OT_EDIT_PASSWORD, NULL) == TRUE)
	{
		otiEdit->SetWindowText(_T("Password"));
	}
	// -- Edit Items -> Numerical
	otiEdit = (COptionTreeItemEdit*) m_otTree.InsertItem(new COptionTreeItemEdit(), otiRoot);
	otiEdit->SetLabelText(_T("Numerical"));
	otiEdit->SetInfoText(_T("This is an example of a numerical edit item."));
	if (otiEdit->CreateEditItem(OT_EDIT_NUMERICAL, NULL) == TRUE)
	{
		otiEdit->SetWindowText(_T("100"));
	}
	// -- Edit Items -> Read Only
	otiEdit = (COptionTreeItemEdit*) m_otTree.InsertItem(new COptionTreeItemEdit(), otiRoot);
	otiEdit->SetLabelText(_T("Read Only"));
	otiEdit->SetInfoText(_T("This is an example of a read only edit item."));
	if (otiEdit->CreateEditItem(NULL, NULL) == TRUE)
	{
		otiEdit->SetWindowText(_T("I am a read only edit."));
		otiEdit->ReadOnly(TRUE);
	}*/
	// -- Combo Box Items
	m_otiColorCombo = (COptionTreeItemColorComboBox*)m_otTree.InsertItem(new COptionTreeItemColorComboBox(), otiRoot);
	resStr.LoadString(IDS_OB_PR_DL_COLOR);
	m_otiColorCombo->SetLabelText(resStr);
	resStr.LoadString(IDS_OB_PR_DL_FULL_COLOR);
	m_otiColorCombo->SetInfoText(resStr);
	if (m_otiColorCombo->CreateComboItem(NULL) == TRUE)
	{
		m_otiColorCombo->SetCurColor(m_editing_object->GetAttribute(SG_OA_COLOR));	
	}
	m_otiThicknessCombo = (COptionTreeItemLineThikComboBox*)m_otTree.InsertItem(new COptionTreeItemLineThikComboBox(), otiRoot);
	resStr.LoadString(IDS_OB_PR_DL_LIN_TH);
	m_otiThicknessCombo->SetLabelText(resStr);
	resStr.LoadString(IDS_OB_PR_DL_FULL_LIN_TH);
	m_otiThicknessCombo->SetInfoText(resStr);
	if (m_otiThicknessCombo->CreateComboItem(NULL) == TRUE)
	{
		m_otiThicknessCombo->SetLineThickness(m_editing_object->GetAttribute(SG_OA_LINE_THICKNESS));
	}
	m_otiTypeCombo = (COptionTreeItemLineTypeComboBox*)m_otTree.InsertItem(new COptionTreeItemLineTypeComboBox(), otiRoot);
	resStr.LoadString(IDS_OB_PR_DL_LN_TY);
	m_otiTypeCombo->SetLabelText(resStr);
	resStr.LoadString(IDS_OB_PR_DL_FULL_LN_TY);
	m_otiTypeCombo->SetInfoText(resStr);
	if (m_otiTypeCombo->CreateComboItem(NULL) == TRUE)
	{
		m_otiTypeCombo->SetLineType(m_editing_object->GetAttribute(SG_OA_LINE_TYPE));
	}

	otiRoot->Expand();


	otiRoot = m_otTree.InsertItem(new COptionTreeItem());
	resStr.LoadString(IDS_OB_PR_DL_VIS);
	otiRoot->SetLabelText(resStr);
	resStr.LoadString(IDS_OB_PR_DL_FULL_VIS);
	otiRoot->SetInfoText(resStr);

	m_otiVis_check = (COptionTreeItemCheckBox*)m_otTree.InsertItem(new COptionTreeItemCheckBox(), otiRoot);
	resStr.LoadString(IDS_OB_PR_DL_SHOW);
	m_otiVis_check->SetLabelText(resStr);
	resStr.LoadString(IDS_OB_PR_DL_FULL_SHOW);
	m_otiVis_check->SetInfoText(resStr);
	short ds = m_editing_object->GetAttribute(SG_OA_DRAW_STATE);
	if (m_otiVis_check->CreateCheckBoxItem((ds & SG_DS_HIDE)==0, 
		OT_CHECKBOX_SHOWCHECK) == TRUE)
	{
		CString yesS;
		yesS.LoadString(IDS_OB_PR_DL_SHOW_YES);
		CString noS;
		noS.LoadString(IDS_OB_PR_DL_SHOW_NO);
		m_otiVis_check->SetCheckText(yesS, noS);
	}

	m_otiGabVis_check = (COptionTreeItemCheckBox*)m_otTree.InsertItem(new COptionTreeItemCheckBox(), otiRoot);
	resStr.LoadString(IDS_OB_PR_DL_GAB);
	m_otiGabVis_check->SetLabelText(resStr);
	resStr.LoadString(IDS_OB_PR_DL_FULL_GAB);
	m_otiGabVis_check->SetInfoText(resStr);
	if (m_otiGabVis_check->CreateCheckBoxItem((ds & SG_DS_GABARITE)!=0, 
		OT_CHECKBOX_SHOWCHECK) == TRUE)
	{
		CString yesS;
		yesS.LoadString(IDS_OB_PR_DL_GAB_YES);
		CString noS;
		noS.LoadString(IDS_OB_PR_DL_GAB_NO);
		m_otiGabVis_check->SetCheckText(yesS, noS);
	}

	if (m_editing_object->GetType()==SG_OT_3D)
	{

		m_otiFrameOrSolidVis_radio = (COptionTreeItemRadio*)m_otTree.InsertItem(new COptionTreeItemRadio(), otiRoot);
		resStr.LoadString(IDS_OB_PR_DL_DRAW);
		m_otiFrameOrSolidVis_radio->SetLabelText(resStr);
		resStr.LoadString(IDS_OB_PR_DL_FULL_DRAW);
		m_otiFrameOrSolidVis_radio->SetInfoText(resStr);
		if (m_otiFrameOrSolidVis_radio->CreateRadioItem() == TRUE)
		{
			resStr.LoadString(IDS_OB_PR_DL_DRAW_FR);
			m_otiFrameOrSolidVis_radio->InsertNewRadio(resStr, (ds & SG_DS_FRAME)!=0);
			resStr.LoadString(IDS_OB_PR_DL_DRAW_SOL);
			m_otiFrameOrSolidVis_radio->InsertNewRadio(resStr, (ds & SG_DS_FULL)!=0);
		}

	}

	otiRoot->Expand();

/*
	otiRoot = m_otTree.InsertItem(new COptionTreeItem());
	otiRoot->SetLabelText(_T("Sub Item Test"));
	otiRoot->SetInfoText(_T("This is an example of numerous sub items."));
	// -- Sub Item Test -> Sub Item 1
	otiStatic = (COptionTreeItemStatic*)m_otTree.InsertItem(new COptionTreeItemStatic(), otiRoot);
	otiStatic->SetLabelText(_T("Sub Item 1"));
	otiStatic->SetInfoText(_T("Sub item 1."));
	otiStatic->SetStaticText(_T("I am a sub item."));
	if (otiStatic->CreateStaticItem(0) == TRUE)
	{
	}
	// -- Sub Item Test -> Sub Item 2
	otiStatic = (COptionTreeItemStatic*)m_otTree.InsertItem(new COptionTreeItemStatic(), otiStatic);
	otiStatic->SetLabelText(_T("Sub Item 2"));
	otiStatic->SetInfoText(_T("Sub item 2."));
	otiStatic->SetStaticText(_T("I am a sub item."));
	if (otiStatic->CreateStaticItem(0) == TRUE)
	{
	}
	// -- Sub Item Test -> Sub Item 3
	otiStatic = (COptionTreeItemStatic*)m_otTree.InsertItem(new COptionTreeItemStatic(), otiStatic);
	otiStatic->SetLabelText(_T("Sub Item 3"));
	otiStatic->SetInfoText(_T("Sub item 3."));
	otiStatic->SetStaticText(_T("I am a sub item."));
	if (otiStatic->CreateStaticItem(0) == TRUE)
	{
	}
	// -- Sub Item Test -> Sub Item 4
	otiStatic = (COptionTreeItemStatic*)m_otTree.InsertItem(new COptionTreeItemStatic(), otiStatic);
	otiStatic->SetLabelText(_T("Sub Item 4"));
	otiStatic->SetInfoText(_T("Sub item 4."));
	otiStatic->SetStaticText(_T("I am a sub item."));
	if (otiStatic->CreateStaticItem(0) == TRUE)
	{
	}
	// -- Combo Box Items -> Combo Box
	otiCombo = (COptionTreeItemColorComboBox*)m_otTree.InsertItem(new COptionTreeItemColorComboBox(), otiRoot);
	otiCombo->SetLabelText(_T("Combo Box"));
	otiCombo->SetInfoText(_T("This is an example of a combo box item."));
	if (otiCombo->CreateComboItem(NULL) == TRUE)
	{
		otiCombo->AddString("Yes");
		otiCombo->AddString("No");

		otiCombo->SelectString(0, "No");
	}
	// -- Combo Box Items -> Read Only
	otiCombo = (COptionTreeItemColorComboBox*)m_otTree.InsertItem(new COptionTreeItemColorComboBox(), otiRoot);
	otiCombo->SetLabelText(_T("Read Only"));
	otiCombo->SetInfoText(_T("This is an example of a read only combo box item."));
	if (otiCombo->CreateComboItem(NULL) == TRUE)
	{
		otiCombo->AddString("I am a read only combo box.");

		otiCombo->SelectString(0, "I am a read only combo box.");

		otiCombo->ReadOnly(TRUE);
	}
	// -- Check Box Items
	otiRoot = m_otTree.InsertItem(new COptionTreeItem());
	otiRoot->SetLabelText(_T("Check Box Items"));
	otiRoot->SetInfoText(_T("These are examples of check box items."));
	// -- Check Box Items-> Without Text
	otiCheck = (COptionTreeItemCheckBox*)m_otTree.InsertItem(new COptionTreeItemCheckBox(), otiRoot);
	otiCheck->SetLabelText(_T("Without Text"));
	otiCheck->SetInfoText(_T("This is an example of a check box item without text."));
	if (otiCheck->CreateCheckBoxItem(FALSE, OT_CHECKBOX_SHOWCHECK) == TRUE)
	{
		otiCheck->SetCheckText(_T("Checked"), _T("UnChecked"));
	}
	// -- Check Box Items -> With Text
	otiCheck = (COptionTreeItemCheckBox*)m_otTree.InsertItem(new COptionTreeItemCheckBox(), otiRoot);
	otiCheck->SetLabelText(_T("With Text"));
	otiCheck->SetInfoText(_T("This is an example of a check box item with text."));
	if (otiCheck->CreateCheckBoxItem(FALSE, OT_CHECKBOX_SHOWCHECK | OT_CHECKBOX_SHOWTEXT) == TRUE)
	{
		otiCheck->SetCheckText(_T("Checked"), _T("UnChecked"));
	}
	// -- Check Box Items -> Without Check Box
	otiCheck = (COptionTreeItemCheckBox*)m_otTree.InsertItem(new COptionTreeItemCheckBox(), otiRoot);
	otiCheck->SetLabelText(_T("Without Check Box"));
	otiCheck->SetInfoText(_T("This is an example of a check box item without check box when not activated."));
	if (otiCheck->CreateCheckBoxItem(FALSE, OT_CHECKBOX_SHOWTEXT) == TRUE)
	{
		otiCheck->SetCheckText(_T("Checked"), _T("UnChecked"));
	}
	// -- Check Box Items -> Read Only
	otiCheck = (COptionTreeItemCheckBox*)m_otTree.InsertItem(new COptionTreeItemCheckBox(), otiRoot);
	otiCheck->SetLabelText(_T("Read Only"));
	otiCheck->SetInfoText(_T("This is an example of a check box item that is read only."));
	if (otiCheck->CreateCheckBoxItem(FALSE, OT_CHECKBOX_SHOWCHECK | OT_CHECKBOX_SHOWTEXT) == TRUE)
	{
		otiCheck->SetCheckText(_T("Checked"), _T("UnChecked"));

		otiCheck->ReadOnly(TRUE);
	}
	// -- Radio Items
	otiRoot = m_otTree.InsertItem(new COptionTreeItem());
	otiRoot->SetLabelText(_T("Radio Items"));
	otiRoot->SetInfoText(_T("These are examples of radio items."));
	// -- Radio Items -> Radio Items
	otiRadio = (COptionTreeItemRadio*)m_otTree.InsertItem(new COptionTreeItemRadio(), otiRoot);
	otiRadio->SetLabelText(_T("Radio Items"));
	otiRadio->SetInfoText(_T("This is an example of a radio item."));
	if (otiRadio->CreateRadioItem() == TRUE)
	{
		otiRadio->InsertNewRadio(_T("One"), TRUE);
		otiRadio->InsertNewRadio(_T("Two"), FALSE);
		otiRadio->InsertNewRadio(_T("Three"), FALSE);
	}
	// -- Radio Items -> Read Only
	otiRadio = (COptionTreeItemRadio*)m_otTree.InsertItem(new COptionTreeItemRadio(), otiRoot);
	otiRadio->SetLabelText(_T("Read Only"));
	otiRadio->SetInfoText(_T("This is an example of a radio item that is read only."));
	if (otiRadio->CreateRadioItem() == TRUE)
	{
		otiRadio->InsertNewRadio(_T("Cat"), FALSE);
		otiRadio->InsertNewRadio(_T("Dog"), TRUE);

		otiRadio->ReadOnly(TRUE);
	}
	// -- Spinner Items
	otiRoot = m_otTree.InsertItem(new COptionTreeItem());
	otiRoot->SetLabelText(_T("Spinner Items"));
	otiRoot->SetInfoText(_T("These are examples of spinner items."));
	// -- Spinner Items -> Wrap Around
	otiSpinner = (COptionTreeItemSpinner*)m_otTree.InsertItem(new COptionTreeItemSpinner(), otiRoot);
	otiSpinner->SetLabelText(_T("Wrap Around"));
	otiSpinner->SetInfoText(_T("This is an example of a spinner item that wraps around."));
	if (otiSpinner->CreateSpinnerItem(OT_EDIT_WRAPAROUND | OT_EDIT_USEREDIT, 80, 1, 100) == TRUE)
	{
	}
	// -- Spinner Items -> No User Edit
	otiSpinner = (COptionTreeItemSpinner*)m_otTree.InsertItem(new COptionTreeItemSpinner(), otiRoot);
	otiSpinner->SetLabelText(_T("No User Edit"));
	otiSpinner->SetInfoText(_T("This is an example of spinner item that disallows user edit."));
	if (otiSpinner->CreateSpinnerItem(OT_EDIT_WRAPAROUND, 80, 1, 100) == TRUE)
	{
	}
	// -- Spinner Items -> No Wrap Around
	otiSpinner = (COptionTreeItemSpinner*)m_otTree.InsertItem(new COptionTreeItemSpinner(), otiRoot);
	otiSpinner->SetLabelText(_T("No Wrap Around"));
	otiSpinner->SetInfoText(_T("This is an example of spinner item that does not wrap around."));
	if (otiSpinner->CreateSpinnerItem(OT_EDIT_USEREDIT, 80, 1, 100) == TRUE)
	{
	}
	// -- Color Items
	otiRoot = m_otTree.InsertItem(new COptionTreeItem());
	otiRoot->SetLabelText(_T("Color Items"));
	otiRoot->SetInfoText(_T("These are examples of color items."));
	// -- Color Items -> Hex Color
	otiColor = (COptionTreeItemColor*)m_otTree.InsertItem(new COptionTreeItemColor(), otiRoot);
	otiColor->SetLabelText(_T("Hex Color"));
	otiColor->SetInfoText(_T("This is an example of hexadecimal color item."));
	if (otiColor->CreateColorItem(OT_COLOR_SHOWHEX, RGB(0, 0, 255), RGB(0, 0, 0)) == TRUE)
	{
	}
	// -- Color Items -> RGB Color
	otiColor = (COptionTreeItemColor*)m_otTree.InsertItem(new COptionTreeItemColor(), otiRoot);
	otiColor->SetLabelText(_T("RGB Color"));
	otiColor->SetInfoText(_T("This is an example of RGB color item."));
	if (otiColor->CreateColorItem(NULL, RGB(0, 0, 255), RGB(0, 0, 0)) == TRUE)
	{
	}
	// -- Color Items -> Live Update
	otiColor = (COptionTreeItemColor*)m_otTree.InsertItem(new COptionTreeItemColor(), otiRoot);
	otiColor->SetLabelText(_T("Live Update"));
	otiColor->SetInfoText(_T("This is an example of live update color item."));
	if (otiColor->CreateColorItem(OT_COLOR_SHOWHEX | OT_COLOR_LIVEUPDATE, RGB(255, 100, 255), RGB(0, 0, 0)) == TRUE)
	{
	}
	// -- Color Items -> Read Only
	otiColor = (COptionTreeItemColor*)m_otTree.InsertItem(new COptionTreeItemColor(), otiRoot);
	otiColor->SetLabelText(_T("Read Only"));
	otiColor->SetInfoText(_T("This is an example of read only color item."));
	if (otiColor->CreateColorItem(OT_COLOR_SHOWHEX | OT_COLOR_LIVEUPDATE, RGB(130, 100, 255), RGB(0, 0, 0)) == TRUE)
	{
		otiColor->ReadOnly(TRUE);
	}

	// -- Hyperlink
	otiRoot = m_otTree.InsertItem(new COptionTreeItem());
	otiRoot->SetLabelText(_T("Hyperlink"));
	otiRoot->SetInfoText(_T("These are examples of hyperlink items."));
	// -- Hyperlink -> Hover
	oti_Hyperlink = (COptionTreeItemHyperLink*)m_otTree.InsertItem(new COptionTreeItemHyperLink(), otiRoot);
	oti_Hyperlink->SetLabelText(_T("Hover"));
	oti_Hyperlink->SetInfoText(_T("This is an example of a hyperlink with the hover option."));
	if (oti_Hyperlink->CreateHyperlinkItem(OT_HL_HOVER, _T("http://www.computersmarts.net"), RGB(0, 0, 255), RGB(150, 0, 150), RGB(255, 0, 255)) == TRUE)
	{
	}
	// -- Hyperlink -> Visited
	oti_Hyperlink = (COptionTreeItemHyperLink*)m_otTree.InsertItem(new COptionTreeItemHyperLink(), otiRoot);
	oti_Hyperlink->SetLabelText(_T("Visited"));
	oti_Hyperlink->SetInfoText(_T("This is an example of a hyperlink with the visited option."));
	if (oti_Hyperlink->CreateHyperlinkItem(OT_HL_HOVER | OT_HL_VISITED, _T("http://www.computersmarts.net"), RGB(0, 0, 255), RGB(150, 0, 150), RGB(255, 0, 255)) == TRUE)
	{
	}
	// -- Hyperlink -> Underlined Hover
	oti_Hyperlink = (COptionTreeItemHyperLink*)m_otTree.InsertItem(new COptionTreeItemHyperLink(), otiRoot);
	oti_Hyperlink->SetLabelText(_T("Underlined Hover"));
	oti_Hyperlink->SetInfoText(_T("This is an example of a hyperlink with underlined text when hovered."));
	if (oti_Hyperlink->CreateHyperlinkItem(OT_HL_HOVER | OT_HL_UNDERLINEHOVER, _T("http://www.computersmarts.net"), RGB(0, 0, 255), RGB(150, 0, 150), RGB(255, 0, 255)) == TRUE)
	{
	}		
	// -- Hyperlink -> Underlined
	oti_Hyperlink = (COptionTreeItemHyperLink*)m_otTree.InsertItem(new COptionTreeItemHyperLink(), otiRoot);
	oti_Hyperlink->SetLabelText(_T("Underlined"));
	oti_Hyperlink->SetInfoText(_T("This is an example of a hyperlink with underlined text."));
	if (oti_Hyperlink->CreateHyperlinkItem(OT_HL_HOVER | OT_HL_UNDERLINE, _T("http://www.computersmarts.net"), RGB(0, 0, 255), RGB(150, 0, 150), RGB(255, 0, 255)) == TRUE)
	{
	}	
	// -- Hyperlink -> Read Only
	oti_Hyperlink = (COptionTreeItemHyperLink*)m_otTree.InsertItem(new COptionTreeItemHyperLink(), otiRoot);
	oti_Hyperlink->SetLabelText(_T("Read Only"));
	oti_Hyperlink->SetInfoText(_T("This is an example of a hyperlink that is read only."));
	if (oti_Hyperlink->CreateHyperlinkItem(OT_HL_HOVER, _T("http://www.computersmarts.net"), RGB(0, 0, 255), RGB(150, 0, 150), RGB(255, 0, 255)) == TRUE)
	{
		oti_Hyperlink->ReadOnly(TRUE);
	}*/
	/************************************************************************/
	/*                                                                      */
	/************************************************************************/

	return TRUE;  // return TRUE  unless you set the focus to a control
}

BEGIN_MESSAGE_MAP(CObjSysParamsDlg, CDialog)
	ON_WM_SIZE()
END_MESSAGE_MAP()


// CObjSysParamsDlg message handlers
/*void CObjSysParamsDlg::OnCbnSelchangeObjLayerCombo()
{
	m_obj_layer = m_layer_combo.GetCurSel();
}

void CObjSysParamsDlg::OnCbnSelchangeObjColorCombo()
{
	m_obj_color = m_color_combo.GetCurSel();
}

void CObjSysParamsDlg::OnCbnSelchangeObjThicknessCombo()
{
	m_obj_l_thickness = m_line_thickness_combo.GetCurSel();
}

void CObjSysParamsDlg::OnCbnSelchangeObjLineTypeCombo()
{
	m_obj_l_type = m_line_type_combo.GetCurSel();
}
*/
void CObjSysParamsDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialog::OnSize(nType, cx, cy);

	if (GetDlgItem(IDOK))
	{
		CRect rrr;
		GetDlgItem(IDOK)->GetWindowRect(rrr);
		ScreenToClient(rrr);

		m_otTree.MoveWindow(0,0,cx,cy-rrr.top-4);
	}
}

void CObjSysParamsDlg::OnOK()
{
	if (m_otiEdit->IsSelected())
		m_otiEdit->CommitChanges();
	if (m_otiLayerCombo->IsSelected())
		m_otiLayerCombo->CommitChanges();
	if (m_otiColorCombo->IsSelected())
		m_otiColorCombo->CommitChanges();
	if (m_otiThicknessCombo->IsSelected())
		m_otiThicknessCombo->CommitChanges();
	if (m_otiTypeCombo->IsSelected())
		m_otiTypeCombo->CommitChanges();

	if (m_otiFrameOrSolidVis_radio && m_otiFrameOrSolidVis_radio->IsSelected())
		m_otiFrameOrSolidVis_radio->CommitChanges();
	if (m_otiVis_check->IsSelected())
		m_otiVis_check->CommitChanges();
	if (m_otiGabVis_check->IsSelected())
		m_otiGabVis_check->CommitChanges();
	
	CString      nm;
	m_otiEdit->GetWindowText(nm);
	unsigned int col = m_otiColorCombo->GetCurColor();
	unsigned int thik = m_otiThicknessCombo->GetLineThickness();
	unsigned int typ = m_otiTypeCombo->GetLineType();

	if (m_editing_object)
	{
		m_editing_object->SetName(nm);
		m_editing_object->SetAttribute(SG_OA_LAYER,m_otiLayerCombo->GetCurSel());
		m_editing_object->SetAttribute(SG_OA_COLOR,col);
		m_editing_object->SetAttribute(SG_OA_LINE_THICKNESS,thik);
		m_editing_object->SetAttribute(SG_OA_LINE_TYPE, typ);

		if (m_otiFrameOrSolidVis_radio)
		{
			int chNode = m_otiFrameOrSolidVis_radio->Node_GetChecked();
			if (chNode==-1)
			{
				ASSERT(0);
			}
			if (chNode==0)
			{
				m_editing_object->SetAttribute(SG_OA_DRAW_STATE,m_editing_object->GetAttribute(SG_OA_DRAW_STATE)|SG_DS_FRAME);
				m_editing_object->SetAttribute(SG_OA_DRAW_STATE,m_editing_object->GetAttribute(SG_OA_DRAW_STATE)& ~SG_DS_FULL);
			}
			else
			{
				m_editing_object->SetAttribute(SG_OA_DRAW_STATE,m_editing_object->GetAttribute(SG_OA_DRAW_STATE)|SG_DS_FULL);
				m_editing_object->SetAttribute(SG_OA_DRAW_STATE,m_editing_object->GetAttribute(SG_OA_DRAW_STATE)& ~SG_DS_FRAME);
			}
		}

		if (m_otiVis_check->GetCheck())
			m_editing_object->SetAttribute(SG_OA_DRAW_STATE,m_editing_object->GetAttribute(SG_OA_DRAW_STATE)& ~SG_DS_HIDE);
		else
			m_editing_object->SetAttribute(SG_OA_DRAW_STATE,m_editing_object->GetAttribute(SG_OA_DRAW_STATE)|SG_DS_HIDE);

		if (m_otiGabVis_check->GetCheck())
			m_editing_object->SetAttribute(SG_OA_DRAW_STATE,m_editing_object->GetAttribute(SG_OA_DRAW_STATE)|SG_DS_GABARITE);
		else
			m_editing_object->SetAttribute(SG_OA_DRAW_STATE,m_editing_object->GetAttribute(SG_OA_DRAW_STATE)& ~SG_DS_GABARITE);
	}

	CDialog::OnOK();
}
