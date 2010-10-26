// CustomColorPage.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include <list>
//#include "XTListBase.h"

//	#include "surfacecurvature.h"
#include "CustomColorPage.h"
#include "colorscheme.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

void CColorListBox::InsertColorItem(int index, COLORREF color)
{
	// add a listbox item
	InsertString(index, (LPCTSTR) color);
		// Listbox does not have the LBS_HASSTRINGS style, so the
		//  normal listbox string is used to store an RGB color
}

void CColorListBox::AddColorItem(COLORREF color)
{
	// add a listbox item
	AddString((LPCTSTR) color);
		// Listbox does not have the LBS_HASSTRINGS style, so the
		//  normal listbox string is used to store an RGB color
}

/////////////////////////////////////////////////////////////////////////////

#define COLOR_ITEM_HEIGHT   20

void CColorListBox::MeasureItem(LPMEASUREITEMSTRUCT lpMIS)
{
	// all items are of fixed size
	// must use LBS_OWNERDRAWVARIABLE for this to work
	lpMIS->itemHeight = COLOR_ITEM_HEIGHT;
}

void CColorListBox::DrawItem(LPDRAWITEMSTRUCT lpDIS)
{
	CDC* pDC = CDC::FromHandle(lpDIS->hDC);
	COLORREF cr = (COLORREF)lpDIS->itemData; // RGB in item data

	if (lpDIS->itemAction & ODA_DRAWENTIRE)
	{
		// Paint the color item in the color requested
		CBrush br(cr);
		pDC->FillRect(&lpDIS->rcItem, &br);
	}

	if ((lpDIS->itemState & ODS_SELECTED) &&
		(lpDIS->itemAction & (ODA_SELECT | ODA_DRAWENTIRE)))
	{
		// item has been selected - hilite frame
		COLORREF crHilite = RGB(255-GetRValue(cr),
						255-GetGValue(cr), 255-GetBValue(cr));
		CBrush br(crHilite);
		pDC->FrameRect(&lpDIS->rcItem, &br);
	}

	if (!(lpDIS->itemState & ODS_SELECTED) &&
		(lpDIS->itemAction & ODA_SELECT))
	{
		// Item has been de-selected -- remove frame
		CBrush br(cr);
		pDC->FrameRect(&lpDIS->rcItem, &br);
	}
}

int CColorListBox::CompareItem(LPCOMPAREITEMSTRUCT lpCIS)
{
	COLORREF cr1 = (COLORREF)lpCIS->itemData1;
	COLORREF cr2 = (COLORREF)lpCIS->itemData2;
	if (cr1 == cr2)
		return 0;       // exact match

	// first do an intensity sort, lower intensities go first
	int intensity1 = GetRValue(cr1) + GetGValue(cr1) + GetBValue(cr1);
	int intensity2 = GetRValue(cr2) + GetGValue(cr2) + GetBValue(cr2);
	if (intensity1 < intensity2)
		return -1;      // lower intensity goes first
	else if (intensity1 > intensity2)
		return 1;       // higher intensity goes second

	// if same intensity, sort by color (blues first, reds last)
	if (GetBValue(cr1) > GetBValue(cr2))
		return -1;
	else if (GetGValue(cr1) > GetGValue(cr2))
		return -1;
	else if (GetRValue(cr1) > GetRValue(cr2))
		return -1;
	else
		return 1;
}


// CCustomColorPage 대화 상자입니다.

IMPLEMENT_DYNAMIC(CCustomColorPage, CPropertyPage)

CCustomColorPage::CCustomColorPage()
	: CPropertyPage(CCustomColorPage::IDD)
{

}

CCustomColorPage::~CCustomColorPage()
{
}

void CCustomColorPage::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_COLORS, m_listCtrlColors);
	DDX_Control(pDX, IDC_STATIC_COLORPREVIEW, m_colorPreview);
}

BEGIN_MESSAGE_MAP(CCustomColorPage, CPropertyPage)
	ON_WM_CREATE()
	ON_CBN_SELCHANGE(IDC_COMBO_COLOR_SIZE, &CCustomColorPage::OnCbnSelchangeComboColorSize)
	ON_CBN_DROPDOWN(IDC_COMBO_COLOR_SIZE, &CCustomColorPage::OnCbnDropdownComboColorSize)
	ON_WM_PAINT()
	ON_LBN_DBLCLK(IDC_LIST_COLORS, &CCustomColorPage::OnLbnDblclkListColors)
END_MESSAGE_MAP()

BOOL CCustomColorPage::OnInitDialog() 
{
	CDialog::OnInitDialog();
   
   
	// 기존 property를 세팅한다
	CComboBox * colorSizeCombo = static_cast <CComboBox*> (GetDlgItem(IDC_COMBO_COLOR_SIZE));

	if(!(m_pColorList.empty()))
	{
		// 콤보박스를 세팅한다.
		colorSizeCombo->AddString("2");
		colorSizeCombo->AddString("3");
		colorSizeCombo->AddString("4");
		colorSizeCombo->AddString("5");

		colorSizeCombo->SetCurSel(m_pColorList.size() - 2);
		// setting a listbox

		int j;
		int iSize = colorSizeCombo->GetCurSel();

		//for( j 0= m_pColorList.begin( ), j = 1; j < iSize + 3;  colorIter++,j++)
		//{
		//	m_listCtrlColors.InsertColorItem(j-1,*colorIter);
		//}

	}

	DrawPreviewColor();

   return TRUE;   // return TRUE unless you set the focus to a control
                  // EXCEPTION: OCX Property Pages should return FALSE
}


int CCustomColorPage::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CPropertyPage::OnCreate(lpCreateStruct) == -1)
		return -1;

	// 여기에 특수화된 작성 코드를 추가합니다.

	return 0;
}

void CCustomColorPage::OnCbnSelchangeComboColorSize()
{
	CComboBox * colorSizeCombo = static_cast<CComboBox*> (GetDlgItem(IDC_COMBO_COLOR_SIZE));

	long iSize = colorSizeCombo->GetCurSel();

	int colorlistSize = m_pColorList.size();

	if (iSize > colorlistSize - 3 ) 
	{
		for(long i = colorlistSize + 1; i < iSize + 3 ; i++)
		{
			CString str;
			str.Format("Color %d", i );
			
			COLORREF color = RGB(255,255,0);
			m_listCtrlColors.InsertColorItem(i-1, color);
			m_pColorList.push_back(color);
		}
	}
	else 
	{
		for(long i = colorlistSize-1; i > iSize + 1 ; i--)
		{
			m_listCtrlColors.DeleteString(i);
			m_pColorList.pop_back();
		}
	}
	DrawPreviewColor();
}

void CCustomColorPage::OnCbnDropdownComboColorSize()
{
	CComboBox * colorSizeCombo = static_cast <CComboBox*> (GetDlgItem(IDC_COMBO_COLOR_SIZE));

	if (colorSizeCombo->GetCount() != 0)
		colorSizeCombo->ResetContent();

	colorSizeCombo->AddString("2");
	colorSizeCombo->AddString("3");
	colorSizeCombo->AddString("4");
	colorSizeCombo->AddString("5");
}

void CCustomColorPage::DrawPreviewColor()
{
	CRect rc;

	m_colorPreview.GetWindowRect(&rc);
	ScreenToClient( &rc );

	this->InvalidateRect(&rc);
}


void CCustomColorPage::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	
	// 그리기 메시지에 대해서는 CPropertyPage::OnPaint()을(를) 호출하지 마십시오.
	CRect rc;
	m_colorPreview.GetWindowRect(&rc);
	ScreenToClient( &rc );

	// Center icon in client rectangle
	CRect rect;

	COLORREF ref;
	std::list <COLORREF>::iterator colorIter;

	int nIndex = m_pColorList.size();
	int i, j;

	int height = rc.Height();
	int a = height / (nIndex - 1);


	for( i = 1; i < height; i++)
	{
		rect.top = rc.top+ i-1 ;
		rect.left = rc.left;
		rect.right = rc.right;
		rect.bottom = rc.top + i;

		float value = (float)(height - i)/ (float) height;

		ref =  GetColor(value);
		CBrush brush(ref);
		dc.FillRect(rect,&brush);
	}

}

COLORREF CCustomColorPage::GetColor(float value)
{
	return 0;

}
void CCustomColorPage::OnLbnDblclkListColors()
{
 
}
