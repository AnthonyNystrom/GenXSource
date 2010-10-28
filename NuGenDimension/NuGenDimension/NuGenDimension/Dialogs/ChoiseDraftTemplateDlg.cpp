// ChoiseDraftTemplateDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "ChoiseDraftTemplateDlg.h"



IMPLEMENT_DYNAMIC(CDraftTemplatesListBox, CListBox)
CDraftTemplatesListBox::CDraftTemplatesListBox()
{
}

CDraftTemplatesListBox::~CDraftTemplatesListBox()
{
}


BEGIN_MESSAGE_MAP(CDraftTemplatesListBox, CListBox)
END_MESSAGE_MAP()

void CDraftTemplatesListBox::MeasureItem(LPMEASUREITEMSTRUCT lpMeasureItemStruct)
{
	/*CDC* hDC = GetDC();
	// Размер элемента списка
	lpMeasureItemStruct->itemHeight = HIWORD(GetTextExtent(hDC->m_hDC,"9",1))+ 
	LOWORD(GetTextExtent(hDC->m_hDC, "9",1))*4 + 3;
	lpMeasureItemStruct->itemWidth  = LOWORD(GetTextExtent(hDC->m_hDC, "9",1))*4 + 3;
	//SetColumnWidth(LOWORD(GetTextExtent(hDC->m_hDC, "9",1))*4 + 3);
	ReleaseDC(hDC);*/
}

//#pragma argsused
static void FPDrawItem(HWND hDlg, LPDRAWITEMSTRUCT lpdis, CxImage* drIm)
{
	RECT		rc;
//	char        buf[10];
//	WORD        i;
	SG_POINT    p = {0, 0, 0};
//	double      dx;
	HDC         hDC;

	RECT      draw_rect;


	hDC = lpdis->hDC;
	CopyRect ((LPRECT)&rc, (LPRECT)&lpdis->rcItem);
	//hOldPen = SelectObject(hDC,
	//(lpdis->itemState & ODS_SELECTED) ? hBLUEPen:hBTNSHADOWPen);
	MoveToEx(hDC, rc.left, rc.top, NULL);
	LineTo(hDC, rc.left, rc.bottom);
	LineTo(hDC, rc.right, rc.bottom);
	LineTo(hDC, rc.right, rc.top);
	LineTo(hDC, rc.left, rc.top);
	//SelectObject(hDC,hOldPen);

	rc.left +=  1;
	rc.right -= 1;
	rc.top += 1;
	rc.bottom -= 1;

	::SetBkColor(hDC, (lpdis->itemState & ODS_SELECTED) ? RGB(50,50,50):RGB(90,90,90));
	::ExtTextOut(hDC, 0, 0, ETO_OPAQUE, (LPRECT)&rc, NULL, 0, NULL);

	draw_rect	= rc;
	draw_rect.top	 = rc.bottom - 20;

	FillRect (hDC, (LPRECT)&draw_rect,
		(lpdis->itemState & ODS_SELECTED) ? (HBRUSH)GetStockObject(DKGRAY_BRUSH):
	(HBRUSH)GetStockObject(GRAY_BRUSH));
	SetBkMode(hDC, TRANSPARENT);
	SetTextColor(hDC, (lpdis->itemState & ODS_SELECTED) ? RGB(255,255,255) : RGB(0,0,255));
	DrawText(hDC, (LPCTSTR)lpdis->itemData, strlen((LPCTSTR)lpdis->itemData),
		&draw_rect, DT_SINGLELINE|DT_CENTER);
	
	draw_rect = rc;
	draw_rect.left+=4;
	draw_rect.top+=4;
	draw_rect.right-=4;
	draw_rect.bottom-=4;
	draw_rect.bottom = rc.bottom - 20;

	CRect rrr(draw_rect);
	draw_rect = FitFirstRectToSecond(CSize(drIm->GetWidth(),drIm->GetHeight()),rrr);
	drIm->Draw(hDC,draw_rect);
}

void CDraftTemplatesListBox::DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct)
{
	//hText = HIWORD(GetTextExtent(lpDrawItemStruct->hDC, "9",1));
	if (GetCount()<1)
		return;
	FPDrawItem(this->m_hWnd, lpDrawItemStruct, (*m_images)[lpDrawItemStruct->itemID]);
}







// CChoiseDraftTemplateDlg dialog

IMPLEMENT_DYNAMIC(CChoiseDraftTemplateDlg, CDialog)
CChoiseDraftTemplateDlg::CChoiseDraftTemplateDlg(const CString& appPath, CWnd* pParent /*=NULL*/)
	: CDialog(CChoiseDraftTemplateDlg::IDD, pParent)
	, m_regime(FALSE)
{
	m_application_Path = appPath;
	m_select = -1;
	m_selected_type = 0;
}

CChoiseDraftTemplateDlg::~CChoiseDraftTemplateDlg()
{
	m_TemplatesPathsArray.clear();
	for (size_t i=0;i<m_thumbnails.size();i++)
		delete m_thumbnails[i];
	m_thumbnails.clear();
}

void CChoiseDraftTemplateDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_DRAFT_TEMPLATES_LIST, m_templates_list);
	DDX_Radio(pDX, IDC_REP_TEMPL_TYPE, m_regime);
}


BEGIN_MESSAGE_MAP(CChoiseDraftTemplateDlg, CDialog)
	ON_WM_CTLCOLOR()
	ON_WM_PAINT()
	ON_BN_CLICKED(IDC_REP_TEMPL_TYPE, OnBnClickedRepTemplType)
	ON_BN_CLICKED(IDC_REP_TEMPL_TYPE2, OnBnClickedRepTemplType2)
	ON_BN_CLICKED(IDC_REP_TEMPL_TYPE3, OnBnClickedRepTemplType3)
	ON_LBN_SELCHANGE(IDC_DRAFT_TEMPLATES_LIST, OnLbnSelchangeDraftTemplatesList)
END_MESSAGE_MAP()


// CChoiseDraftTemplateDlg message handlers

HBRUSH CChoiseDraftTemplateDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);

	int cID = pWnd->GetDlgCtrlID();

	if (cID == IDC_DRAFT_TEMPLATES_LIST)
		return (HBRUSH)GetStockObject(GRAY_BRUSH);


	// TODO:  Return a different brush if the default is not desired
	return hbr;
}

BOOL CChoiseDraftTemplateDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	m_TemplatesPathsArray.clear();
	m_names.clear();

	GetTemplatesFiles("");

	
	LoadAllTemplatesThumbnails();

	m_templates_list.SetImagesVectorPointer(&m_thumbnails);

	for (size_t i=0;i<m_thumbnails.size();i++)
	{
		m_templates_list.AddString(m_names[i]);
		m_templates_list.SetItemHeight( i, 180 );
	}

	m_templates_list.SetColumnWidth(180);

	if (m_thumbnails.size()==0)
	{
		GetDlgItem(IDC_REP_TEMPL_TYPE3)->EnableWindow(FALSE);
	}

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void	CChoiseDraftTemplateDlg::GetTemplatesFiles(CString sPath)
{
	if (m_TemplatesPathsArray.size()>=512) 
		return;

	CString sStr;
	CString sCurFullPath=m_application_Path;

	sCurFullPath+=sPath;
	sCurFullPath+="*";

	WIN32_FIND_DATA FindData;
	HANDLE hFindFiles=FindFirstFile(sCurFullPath,&FindData); 

	if (hFindFiles==INVALID_HANDLE_VALUE) return;

	for(;;) 
	{
		if ((strcmp(FindData.cFileName,".")!=0) && (strcmp(FindData.cFileName,"..")!=0)) 
		{
			if (FindData.dwFileAttributes&FILE_ATTRIBUTE_DIRECTORY) 
			{
				sStr=sPath; 
				sStr+=FindData.cFileName;
				sStr+="\\"; 
				GetTemplatesFiles(sStr);
			}
			else 
			{
				char *ptr=strrchr(FindData.cFileName,'.');
				if (ptr) 
				{ 
					if (strlen(ptr)==4) 
					{
						if ((ptr[1]=='s' && ptr[2]=='d' && ptr[3]=='t') ||
							(ptr[1]=='S' && ptr[2]=='D' && ptr[3]=='T'))   //проверка расширения
						{
							CString sPath1=sPath;
							sPath1+=FindData.cFileName; 
							CString nm(FindData.cFileName);
							m_names.push_back(nm.Left(nm.GetLength()-4));
							m_TemplatesPathsArray.push_back(sPath1); 
						}
					} 
				}
			}
		}
		if (!FindNextFile(hFindFiles,&FindData)) break; 
	}
	FindClose(hFindFiles);
}

CString   CChoiseDraftTemplateDlg::GetSelectedTemplatePath()
{
	CString aaa = m_application_Path+m_TemplatesPathsArray[m_select];
	return aaa;
}

#include "..//Tools//DraftTemplateLoader.h"
#include ".\choisedrafttemplatedlg.h"
void     CChoiseDraftTemplateDlg::LoadAllTemplatesThumbnails()
{
	size_t taSz  = m_TemplatesPathsArray.size();
	if (taSz<1)
		return;

	if (m_thumbnails.size()>0)
	{
		for (size_t i=0;i<m_thumbnails.size();i++)
			delete m_thumbnails[i];
		m_thumbnails.clear();
	}

	m_thumbnails.reserve(taSz);

	for (size_t i=0;i<taSz;i++)
	{
		CString aaa = m_application_Path+m_TemplatesPathsArray[i];
		m_thumbnails.push_back(CDraftTemplateLoader::GetThumbnailFromFile(aaa));
	}
}


void CChoiseDraftTemplateDlg::OnOK()
{
	m_select = m_templates_list.GetCurSel();
	CDialog::OnOK();
}

void CChoiseDraftTemplateDlg::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	
	CRect hR;
	GetDlgItem(IDC_HOR_REP_FRAME)->GetWindowRect(hR);
	ScreenToClient(hR);

	CRect vR;
	GetDlgItem(IDC_VER_REP_FRAME)->GetWindowRect(vR);
	ScreenToClient(vR);

	vR.InflateRect(CSize(-15,-15));
	hR.InflateRect(CSize(-15,-15));

#define MIN_SIZE   100

	/*CSize  horPageSizes(MIN_SIZE*1.4142135623,MIN_SIZE);
	CSize  verPageSizes(MIN_SIZE, MIN_SIZE*1.4142135623);#WARNING*/
	CSize  horPageSizes((int)(MIN_SIZE*1.4142135623),MIN_SIZE);
	CSize  verPageSizes(MIN_SIZE, (int)(MIN_SIZE*1.4142135623));

	hR = FitFirstRectToSecond(horPageSizes, hR);
	vR = FitFirstRectToSecond(verPageSizes, vR);

	dc.Rectangle(hR);
	dc.Rectangle(vR);
}

void CChoiseDraftTemplateDlg::OnBnClickedRepTemplType()
{
	m_selected_type = 0;
}

void CChoiseDraftTemplateDlg::OnBnClickedRepTemplType2()
{
	m_selected_type = 1;
}

void CChoiseDraftTemplateDlg::OnBnClickedRepTemplType3()
{
	m_selected_type = 2;
}

void CChoiseDraftTemplateDlg::OnLbnSelchangeDraftTemplatesList()
{
	m_regime = 2;
	UpdateData(FALSE);
	m_selected_type = 2;
	m_select = m_templates_list.GetCurSel();
}
