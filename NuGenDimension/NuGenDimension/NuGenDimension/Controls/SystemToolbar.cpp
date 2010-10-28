// SystemToolbar.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "SystemToolbar.h"
#include ".\systemtoolbar.h"

// CSystemToolbar

IMPLEMENT_DYNAMIC(CSystemToolbar, CEGToolBar)
CSystemToolbar::CSystemToolbar()
{
}

CSystemToolbar::~CSystemToolbar()
{
}


BEGIN_MESSAGE_MAP(CSystemToolbar, CEGToolBar)
	ON_WM_CTLCOLOR()
END_MESSAGE_MAP()

void   CSystemToolbar::LoadControls()
{
	//SetButtonInfo(10, ID_SLOI, TBBS_SEPARATOR, 100);

	CRect rect;
	/*GetItemRect(10, &rect);
	rect.top = 1;
	rect.bottom = rect.top + 200;
	rect.right = rect.left+100;
	if (!m_LayersComboBox.Create(
		CBS_DROPDOWNLIST|WS_VISIBLE|WS_TABSTOP|WS_VSCROLL,
		rect, this, ID_SLOI))
	{
		TRACE("Failed to create combo-box\n");
		return;
	}*/

	SetButtonInfo(4, ID_TOCHN1, TBBS_SEPARATOR, 50);
	GetItemRect(4, &rect);
	rect.right = rect.left+50;
	rect.top = 1;
	if (!m_empty_static.Create("       ",WS_VISIBLE,rect, this))
	{
		TRACE("Failed to create static\n");
		return;
	}
	rect.top = 5;
	CString prSt;
	prSt.LoadString(IDS_PRECISION_STRING);
	if (!m_Precision_label.Create(prSt,WS_VISIBLE|WS_TABSTOP,rect, this,ID_TOCHN1))
	{
		TRACE("Failed to create static\n");
		return;
	}

	SetButtonInfo(5, ID_TOCHN2, TBBS_SEPARATOR, 100);
	GetItemRect(5, &rect);
	rect.top = 1;
	rect.bottom = rect.top + 200;
	rect.right = rect.left+100;
	if (!m_PrecisionComboBox.Create(
		CBS_DROPDOWNLIST|WS_VISIBLE|WS_TABSTOP|WS_VSCROLL,
		rect, this, ID_TOCHN2))
	{
		TRACE("Failed to create combo-box\n");
		return;
	}

	CString precStr;
	for (int i=0;i<PREC_COUNT;i++)
	{
		precStr.Format("%.3f",precisions[i]);
		m_PrecisionComboBox.AddString(precStr);
	}

	SetButtonInfo(7, ID_CVET, TBBS_SEPARATOR, 100);
	GetItemRect(7, &rect);
	unsigned int ComboH = rect.Height()-6;
	rect.top = 1;
	rect.bottom = rect.top + 200;
	rect.right = rect.left+100;
	if (!m_ColorComboBox.Create(CBS_DROPDOWNLIST|CBS_OWNERDRAWFIXED|WS_VISIBLE|WS_TABSTOP|WS_VSCROLL,
		rect, this, ID_CVET))
	{
		TRACE("Failed to create combo-box\n");
		return;
	}
	m_ColorComboBox.SetItemHeight(-1,ComboH);
	m_ColorComboBox.InitializeDefaultColors();

	SetButtonInfo(9, ID_TOLZHINA, TBBS_SEPARATOR, 100);
	GetItemRect(9, &rect);
	rect.top = 1;
	rect.bottom = rect.top + 200;
	rect.right = rect.left+100;
	if (!m_line_thicknessComboBox.Create(CBS_DROPDOWNLIST|CBS_OWNERDRAWFIXED|WS_VISIBLE|WS_TABSTOP|WS_VSCROLL,
		rect, this, ID_TOLZHINA))
	{
		TRACE("Failed to create combo-box\n");
		return;
	}
	
	m_line_thicknessComboBox.SetItemHeight(-1,ComboH);

	SetButtonInfo(11, ID_TIPLINII, TBBS_SEPARATOR, 100);
	GetItemRect(11, &rect);
	rect.top = 1;
	rect.bottom = rect.top + 200;
	rect.right = rect.left+100;
	if (!m_line_typeComboBox.Create(CBS_DROPDOWNLIST|CBS_OWNERDRAWFIXED|WS_VISIBLE|WS_TABSTOP|WS_VSCROLL,
		rect, this, ID_TIPLINII))
	{
		TRACE("Failed to create combo-box\n");
		return;
	}

	m_line_typeComboBox.SetItemHeight(-1,ComboH);

	SetButtonInfo(14, ID_ATT_FONTS, TBBS_SEPARATOR, 100);
	GetItemRect(14, &rect);
	rect.top = 1;
	rect.bottom = rect.top + 200;
	rect.right = rect.left+100;
	if (!m_Fonts_ComboBox.Create(CBS_DROPDOWNLIST|WS_VISIBLE|WS_TABSTOP|WS_VSCROLL,
		rect, this, ID_ATT_FONTS))
	{
		TRACE("Failed to create combo-box\n");
		return;
	}
	m_Fonts_ComboBox.SetItemHeight(-1,ComboH);
}


void  CSystemToolbar::SetView(CNuGenDimensionView* v)
{
	ASSERT(v);
	m_view = v;

	/*m_LayersComboBox.ResetContent();
	CString  labl;
	labl.LoadString(IDS_LAYER_LABEL);
	CString  curLbl;
	unsigned char lCnt = sgGetScene()->GetLayersCount();
	for (unsigned char i=0;i<lCnt;i++)
	{
		curLbl.Format(" %i",i);
		m_LayersComboBox.AddString(labl+curLbl);
	}
	m_LayersComboBox.SetCurSel(0);*/

	if (global_3D_document)
	{
		SCENE_SETUPS  tmpCV;
		global_3D_document->GetSceneSetups(tmpCV);
		//m_LayersComboBox.SetCurSel(tmpCV.CurrentLayer);
		m_PrecisionComboBox.SetCurSel(tmpCV.CurrentPrecision);
		m_ColorComboBox.SetCurSel(tmpCV.CurrentColor);
		m_line_thicknessComboBox.SetCurSel(tmpCV.CurrentLineThickness);
		m_line_typeComboBox.SetCurSel(tmpCV.CurrentLineType);
	}
	else
	{
		ASSERT(0);
		//m_LayersComboBox.SetCurSel(0);
		m_PrecisionComboBox.SetCurSel(0);
		m_ColorComboBox.SetCurSel(0);
		m_line_thicknessComboBox.SetCurSel(0);
		m_line_typeComboBox.SetCurSel(0);
	}
}

void    CSystemToolbar::UpdateSystemToolbar()
{
	ASSERT(m_view);
	/*m_LayersComboBox.ResetContent();
	CString  labl;
	labl.LoadString(IDS_LAYER_LABEL);
	CString  curLbl;
	unsigned char lCnt = sgGetScene()->GetLayersCount();
	for (unsigned char i=0;i<lCnt;i++)
	{
		curLbl.Format(" %i",i);
		m_LayersComboBox.AddString(labl+curLbl);
	}*/
	SCENE_SETUPS  tmpCV;
	if (global_3D_document)
		global_3D_document->GetSceneSetups(tmpCV);

	//m_LayersComboBox.SetCurSel(tmpCV.CurrentLayer);
	//m_view->GetDocument()->SetCurLayer(tmpCV.CurrentLayer);

	m_Fonts_ComboBox.ResetContent();
	unsigned int fCnt = sgFontManager::GetFontsCount();
	for (unsigned int i=0;i<fCnt;i++)
	{
		const sgCFont* tmpFnt = sgFontManager::GetFont(i);
		CString aaaStr(tmpFnt->GetFontData()->name);
		m_Fonts_ComboBox.AddString(aaaStr.Left(aaaStr.GetLength()-4).MakeLower());
	}
	m_Fonts_ComboBox.SetCurSel(sgFontManager::GetCurrentFont());
}


HBRUSH CSystemToolbar::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = CEGToolBar::OnCtlColor(pDC, pWnd, nCtlColor);

	//if(pWnd->GetDlgCtrlID() == ID_TOCHN1)
	if (pWnd->GetRuntimeClass()==RUNTIME_CLASS(CStatic))
	{
		pDC->SetBkMode(TRANSPARENT);

		LOGBRUSH lb;
		lb.lbStyle = BS_NULL;
		lb.lbColor = RGB(255, 0, 255);
		lb.lbHatch = HS_HORIZONTAL | HS_VERTICAL;
		hbr = CreateBrushIndirect(&lb);

	}
	// TODO: Return a different brush if the default is not desired
	return hbr;
}







#define    CONTROLS_FONT_HEIGHT    12
#define    CONTROLS_FONT_WEIDTH     FW_NORMAL
#define    CONTROLS_FONT		   _T("MS Shell Dlg")

BEGIN_MESSAGE_MAP(CMyComboBox, CComboBox)
	ON_WM_CREATE()
END_MESSAGE_MAP()

int CMyComboBox::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CComboBox::OnCreate(lpCreateStruct) == -1)
		return -1;

	if( !CreateFont(CONTROLS_FONT_HEIGHT, CONTROLS_FONT_WEIDTH, CONTROLS_FONT) )
	{
		TRACE0("Failed to create font for combo box\n");
		return -1;      // fail to create
	}

	return 0;
}

BOOL CMyComboBox::CreateFont(LONG lfHeight, LONG lfWeight, LPCTSTR lpszFaceName)
{
	//  Create a font for the combobox
	LOGFONT logFont;
	memset(&logFont, 0, sizeof(logFont));

	if (!::GetSystemMetrics(SM_DBCSENABLED))
	{
		// Since design guide says toolbars are fixed height so is the font.
		logFont.lfHeight = lfHeight;
		logFont.lfWeight = lfWeight;
		CString strDefaultFont = lpszFaceName;
		lstrcpy(logFont.lfFaceName, strDefaultFont);
		if (!m_font.CreateFontIndirect(&logFont))
		{
			TRACE("Could Not create font for combo\n");
			return FALSE;
		}		
	}
	else
	{
		m_font.Attach(::GetStockObject(SYSTEM_FONT));
	}
	SetFont(&m_font);
	return TRUE;
}

BEGIN_MESSAGE_MAP(CLayersComboBox, CMyComboBox)
	ON_CONTROL_REFLECT(CBN_SELCHANGE, OnCbnSelchange)
END_MESSAGE_MAP()
void CLayersComboBox::OnCbnSelchange()
{
	SCENE_SETUPS tmpSS;
	global_3D_document->GetSceneSetups(tmpSS);
	tmpSS.CurrentLayer = GetCurSel();
	global_3D_document->SetSceneSetups(tmpSS);
}

BEGIN_MESSAGE_MAP(CPrecisionComboBox, CMyComboBox)
	ON_CONTROL_REFLECT(CBN_SELCHANGE, OnCbnSelchange)
END_MESSAGE_MAP()
void CPrecisionComboBox::OnCbnSelchange()
{
	SCENE_SETUPS tmpSS;
	global_3D_document->GetSceneSetups(tmpSS);
	tmpSS.CurrentPrecision = GetCurSel();
	global_3D_document->SetSceneSetups(tmpSS);
}




BEGIN_MESSAGE_MAP(CPrecisionStatic, CStatic)
	ON_WM_CREATE()
//	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

int CPrecisionStatic::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CStatic::OnCreate(lpCreateStruct) == -1)
		return -1;

	if( !CreateFont(CONTROLS_FONT_HEIGHT, CONTROLS_FONT_WEIDTH, CONTROLS_FONT) )
	{
		TRACE0("Failed to create font for combo box\n");
		return -1;      // fail to create
	}
	return 0;
}

BOOL CPrecisionStatic::CreateFont(LONG lfHeight, LONG lfWeight, LPCTSTR lpszFaceName)
{
	//  Create a font for the combobox
	LOGFONT logFont;
	memset(&logFont, 0, sizeof(logFont));

	if (!::GetSystemMetrics(SM_DBCSENABLED))
	{
		// Since design guide says toolbars are fixed height so is the font.
		logFont.lfHeight = lfHeight;
		logFont.lfWeight = lfWeight;
		CString strDefaultFont = lpszFaceName;
		lstrcpy(logFont.lfFaceName, strDefaultFont);
		if (!m_font.CreateFontIndirect(&logFont))
		{
			TRACE("Could Not create font for combo\n");
			return FALSE;
		}		
	}
	else
	{
		m_font.Attach(::GetStockObject(SYSTEM_FONT));
	}
	SetFont(&m_font);
	return TRUE;
}


BEGIN_MESSAGE_MAP(CColorComboBox, CColorPickerCB)
	ON_CONTROL_REFLECT(CBN_SELCHANGE, OnCbnSelchange)
END_MESSAGE_MAP()

void CColorComboBox::OnCbnSelchange()
{
	SCENE_SETUPS tmpSS;
	global_3D_document->GetSceneSetups(tmpSS);
	tmpSS.CurrentColor = GetCurSel();
	global_3D_document->SetSceneSetups(tmpSS);
}




BEGIN_MESSAGE_MAP(CLineThicknessComboBox, CLineThiknessCombo)
	ON_CONTROL_REFLECT(CBN_SELCHANGE, OnCbnSelchange)
END_MESSAGE_MAP()

void CLineThicknessComboBox::OnCbnSelchange()
{
	SCENE_SETUPS tmpSS;
	global_3D_document->GetSceneSetups(tmpSS);
	tmpSS.CurrentLineThickness = GetCurSel();
	global_3D_document->SetSceneSetups(tmpSS);
}

BEGIN_MESSAGE_MAP(CLineTypeComboBox, CLineStyleCombo)
	ON_CONTROL_REFLECT(CBN_SELCHANGE, OnCbnSelchange)
END_MESSAGE_MAP()

void CLineTypeComboBox::OnCbnSelchange()
{
	SCENE_SETUPS tmpSS;
	global_3D_document->GetSceneSetups(tmpSS);
	tmpSS.CurrentLineType = GetCurSel();
	global_3D_document->SetSceneSetups(tmpSS);
}


BEGIN_MESSAGE_MAP(CFontsComboBox, CMyComboBox)
	ON_CONTROL_REFLECT(CBN_SELCHANGE, OnCbnSelchange)
END_MESSAGE_MAP()
void CFontsComboBox::OnCbnSelchange()
{
	sgFontManager::SetCurrentFont(GetCurSel());
}

