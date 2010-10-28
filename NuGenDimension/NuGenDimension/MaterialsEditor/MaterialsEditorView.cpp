// MaterialsEditorView.cpp : implementation of the CMaterialsEditorView class
//

#include "stdafx.h"
#include "MaterialsEditor.h"

#include "MaterialsEditorView.h"
#include ".\materialseditorview.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


#define  THUMBNAIL_WIDTH  104
#define  THUMBNAIL_SPACE  10

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

// CMaterialsEditorView

IMPLEMENT_DYNCREATE(CMaterialsEditorView, CFormView)

BEGIN_MESSAGE_MAP(CMaterialsEditorView, CFormView)
	ON_EN_CHANGE(IDC_MAT_COMMENT, OnEnChangeMatComment)
	ON_LBN_SELCHANGE(IDC_MAT_NAMES_LIST, OnLbnSelchangeMatNamesList)
	ON_EN_CHANGE(IDC_MAT_NAME, OnEnChangeMatName)
	ON_COMMAND(ID_ADD_NEW_MATERIAL, OnAddNewMaterial)
	ON_COMMAND(ID_DELETE_CUR_MATERIAL, OnDeleteCurMaterial)
	ON_BN_CLICKED(IDC_AMBIENT_RADIO, OnBnClickedAmbientRadio)
	ON_BN_CLICKED(IDC_DIFFUSION_RADIO, OnBnClickedDiffusionRadio)
	ON_BN_CLICKED(IDC_EMISSION_RADIO, OnBnClickedEmissionRadio)
	ON_BN_CLICKED(IDC_SPECULAR_RADIO, OnBnClickedSpecularRadio)
	ON_BN_CLICKED(IDC_TRANSPARENT_RADIO, OnBnClickedTransparentRadio)
	ON_BN_CLICKED(IDC_SHININESS_RADIO, OnBnClickedShininessRadio)
	ON_WM_HSCROLL()
	ON_NOTIFY(NM_CLICK, IDC_TEXTURES_LIST, OnNMClickTexturesList)
	ON_COMMAND(ID_ADD_NEW_TEXTURE, OnAddNewTexture)
	ON_COMMAND(ID_DELETE_CUR_TEXTURE, OnDeleteCurTexture)
	ON_WM_PAINT()
	ON_UPDATE_COMMAND_UI(ID_DELETE_CUR_TEXTURE, OnUpdateDeleteCurTexture)
	ON_UPDATE_COMMAND_UI(ID_DELETE_CUR_MATERIAL, OnUpdateDeleteCurMaterial)
	ON_WM_ERASEBKGND()
	ON_WM_CTLCOLOR()
	ON_CBN_SELCHANGE(IDC_CMB_MATERIAL, &CMaterialsEditorView::OnCbnSelchangeCmbMaterial)
END_MESSAGE_MAP()

// CMaterialsEditorView construction/destruction

CMaterialsEditorView::CMaterialsEditorView()
	: CFormView(CMaterialsEditorView::IDD)
	, m_image_list_was_created(false)
	, m_current_material_index(-1)
	, m_active_radio(AR_AMBIENT)
	, m_mats_count(0)
	, m_comment(_T(""))
	, m_current_name(_T(""))
	, m_ambient_radio(0)
	, m_iSolidMaterial(0)
{
	// TODO: add construction code here

}

CMaterialsEditorView::~CMaterialsEditorView()
{
}

void CMaterialsEditorView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_MAT_COUNT, m_mats_count);
	DDX_Text(pDX, IDC_MAT_COMMENT, m_comment);
	DDV_MaxChars(pDX, m_comment, 63);
	DDX_Control(pDX, IDC_MAT_NAMES_LIST, m_all_materials_list);
	DDX_Text(pDX, IDC_MAT_NAME, m_current_name);
	DDV_MaxChars(pDX, m_current_name, 31);
	DDX_Radio(pDX, IDC_AMBIENT_RADIO, m_ambient_radio);
	DDX_Control(pDX, IDC_RED_SLIDER, m_red_slider);
	DDX_Control(pDX, IDC_GREEN_SLIDER, m_green_slider);
	DDX_Control(pDX, IDC_BLUE_SLIDER, m_blue_slider);
	DDX_Control(pDX, IDC_TRANSP_SLIDER, m_transp_slider);
	DDX_Control(pDX, IDC_SHININESS_SLIDER, m_shininess_slider);
	DDX_Control(pDX, IDC_TEXTURES_LIST, m_textures_list);
	DDX_Control(pDX, IDC_TEX_PREVIEW, m_texture_preview);
	DDX_Control(pDX, IDC_ARMAXCTRL, m_preview_material);
	DDX_Control(pDX, IDC_RED_EDIT, m_red_edit);
	DDX_Control(pDX, IDC_GREEN_EDIT, m_green_edit);
	DDX_Control(pDX, IDC_BLUE_EDIT, m_blue_edit);
	DDX_Control(pDX, IDC_TRANSP_EDIT, m_transp_edit);
	DDX_Control(pDX, IDC_SHININESS_EDIT, m_sh_edit);
	DDX_Control(pDX, IDC_MAT_COUNT, m_count_edit);
	DDX_Control(pDX, IDC_CMB_MATERIAL, m_cmbMaterial);
	DDX_CBIndex(pDX, IDC_CMB_MATERIAL, m_iSolidMaterial);
}

BOOL CMaterialsEditorView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CFormView::PreCreateWindow(cs);
}

// CMaterialsEditorView diagnostics

#ifdef _DEBUG
void CMaterialsEditorView::AssertValid() const
{
	CFormView::AssertValid();
}

void CMaterialsEditorView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CMaterialsEditorDoc* CMaterialsEditorView::GetDocument() const // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CMaterialsEditorDoc)));
	return (CMaterialsEditorDoc*)m_pDocument;
}
#endif //_DEBUG

static CRect  FitFirstRectToSecond(CSize& fitSz, CRect& windRect)
{
	CRect resRect;
	double frame_width = min(windRect.Width(), fitSz.cx);
	double frame_height = min(windRect.Height(), fitSz.cy);
	double scale_w = frame_width / fitSz.cx;
	double scale_h = frame_height / fitSz.cy;
	double scale = min(scale_w, scale_h);
	frame_width = fitSz.cx * scale;
	frame_height = fitSz.cy * scale;
	/*resRect.left   = windRect.CenterPoint().x - frame_width  / 2;
	resRect.right  = windRect.CenterPoint().x + frame_width  / 2;
	resRect.top    = windRect.CenterPoint().y - frame_height / 2;
	resRect.bottom = windRect.CenterPoint().y + frame_height / 2; #WARNING*/
	
	resRect.left   = windRect.CenterPoint().x - (int)frame_width  / 2;
	resRect.right  = windRect.CenterPoint().x + (int)frame_width  / 2;
	resRect.top    = windRect.CenterPoint().y - (int)frame_height / 2;
	resRect.bottom = windRect.CenterPoint().y + (int)frame_height / 2;

	return resRect;
}

void CMaterialsEditorView::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	CDC* pDC = m_texture_preview.GetDC();
	CRect		rc;
	m_texture_preview.GetClientRect(&rc);

	POSITION pos = m_textures_list.GetFirstSelectedItemPosition();

	pDC->FillSolidRect(rc,GetSysColor(COLOR_BTNFACE));
	if (pos == NULL)
		goto ext;
	
	int selTexture = m_textures_list.GetNextSelectedItem(pos);

	if (selTexture>0)
	{
		CRect ImR;
		ImR = FitFirstRectToSecond(CSize(GetDocument()->GetTextureByIndex(selTexture-1)->image->GetWidth(),
			GetDocument()->GetTextureByIndex(selTexture-1)->image->GetHeight()),rc);
		GetDocument()->GetTextureByIndex(selTexture-1)->image->Draw(pDC->m_hDC,ImR);
	}
ext:
	m_texture_preview.ReleaseDC(pDC);

}


void CMaterialsEditorView::OnAddNewMaterial()
{
	MAT_ITEM* newItem = GetDocument()->AddNewMaterial();
	m_all_materials_list.AddString(newItem->szName);
	m_mats_count++;
	UpdateData(FALSE);

}

void CMaterialsEditorView::OnDeleteCurMaterial()
{
	m_current_material_index = m_all_materials_list.GetCurSel();
	if (!GetDocument()->DeleteMaterial(m_current_material_index))
	{
		ASSERT(0);
		return;
	}
	m_all_materials_list.DeleteString(m_current_material_index);
	if (m_current_material_index>0)
		m_current_material_index--;
	m_all_materials_list.SetCurSel(m_current_material_index);
	OnLbnSelchangeMatNamesList();
	m_mats_count--;
	UpdateData(FALSE);
}

void CMaterialsEditorView::OnAddNewTexture()
{
	// prompt the user (with all document templates)
	CString newName;
	int nDocType = -1;
	if (!PromptForFileName(newName, AFX_IDS_OPENFILE,
		OFN_HIDEREADONLY | OFN_FILEMUSTEXIST, TRUE, &nDocType))
		return; // open cancelled




	CString ext(FindExtension(newName));
	ext.MakeLower();
	if (ext == "") return;

	int type = FindType(ext);

	TEXTURE_ITEM* texture = GetDocument()->AddNewTexture(newName,type);

	if (!texture)
	{
		ASSERT(0);
		return;
	}

	int imCnt = m_ImageList.GetImageCount();
	m_ImageList.SetImageCount(imCnt+1);

	RGBQUAD pRgb;
	COLORREF winCol = ::GetSysColor(COLOR_WINDOW);
	pRgb.rgbRed = GetRValue(winCol);
	pRgb.rgbGreen = GetGValue(winCol);
	pRgb.rgbBlue = GetBValue(winCol);
	pRgb.rgbReserved = 0;

	CxImage thumbnail(*texture->image);
	thumbnail.Thumbnail(THUMBNAIL_WIDTH,THUMBNAIL_WIDTH,pRgb);

	// add bitmap to our image list
	m_ImageList.Replace(imCnt, CBitmap::FromHandle(thumbnail.MakeBitmap()), NULL);

	m_textures_list.InsertItem(imCnt,NULL,imCnt);
	m_textures_list.SetItemPosition(imCnt,CPoint(imCnt*(THUMBNAIL_WIDTH+THUMBNAIL_SPACE),0));
}

bool CMaterialsEditorView::PromptForFileName(CString& fileName, UINT nIDSTitle, 
									  DWORD dwFlags, BOOL bOpenFileDialog, int* pType)
{
	CFileDialog dlgFile(bOpenFileDialog);

	dlgFile.m_ofn.Flags |= dwFlags;

	int nDocType = (pType != NULL) ? *pType : CXIMAGE_FORMAT_BMP;
	if (nDocType==0) nDocType=1;

	int nIndex = GetIndexFromType(nDocType, bOpenFileDialog);
	if (nIndex == -1) nIndex = 0;

	dlgFile.m_ofn.nFilterIndex = nIndex +1;
	// strDefExt is necessary to hold onto the memory from GetExtFromType
	CString strDefExt = GetExtFromType(nDocType).Mid(2,3);
	dlgFile.m_ofn.lpstrDefExt = strDefExt;

	CString strFilter = GetFileTypes(bOpenFileDialog);
	dlgFile.m_ofn.lpstrFilter = strFilter;
	dlgFile.m_ofn.lpstrFile = fileName.GetBuffer(_MAX_PATH);

	bool bRet = (dlgFile.DoModal() == IDOK) ? true : false;
	fileName.ReleaseBuffer();
	if (bRet){
		if (pType != NULL){
			int nIndex = (int)dlgFile.m_ofn.nFilterIndex - 1;
			ASSERT(nIndex >= 0);
			*pType = GetTypeFromIndex(nIndex, bOpenFileDialog);
		}
	}
	return bRet;
}

int CMaterialsEditorView::GetIndexFromType(int nDocType, BOOL bOpenFileDialog)
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
//////////////////////////////////////////////////////////////////////////////
int CMaterialsEditorView::GetTypeFromIndex(int nIndex, BOOL bOpenFileDialog)
{
	int nCnt = 0;
	for (int i=0;i<CMAX_IMAGE_FORMATS;i++){
		if (bOpenFileDialog ? doctypes[i].bRead : doctypes[i].bWrite){
			if (nCnt == nIndex)
				//              return i; // PJO - Buglet ?
				return doctypes[i].nID;
			nCnt++;
		}
	}
	ASSERT(FALSE);
	return -1;
}

CString CMaterialsEditorView::GetExtFromType(int nDocType)
{
	for (int i=0;i<CMAX_IMAGE_FORMATS;i++){
		if (doctypes[i].nID == nDocType)
			return doctypes[i].ext;
	}
	return CString("");
}

CString CMaterialsEditorView::GetFileTypes(BOOL bOpenFileDialog)
{
	CString str;
	for (int i=0;i<CMAX_IMAGE_FORMATS;i++){
		if (bOpenFileDialog && doctypes[i].bRead){
			str += doctypes[i].description;
			str += (TCHAR)NULL;
			str += doctypes[i].ext;
			str += (TCHAR)NULL;
		} else if (!bOpenFileDialog && doctypes[i].bWrite) {
			str += doctypes[i].description;
			str += (TCHAR)NULL;
			str += doctypes[i].ext;
			str += (TCHAR)NULL;
		}
	}
	return str;
}

CString CMaterialsEditorView::FindExtension(const CString& name)
{
	int len = name.GetLength();
	int i;
	for (i = len-1; i >= 0; i--){
		if (name[i] == '.'){
			return name.Mid(i+1);
		}
	}
	return CString("");
}
//////////////////////////////////////////////////////////////////////////////
int CMaterialsEditorView::FindType(const CString& ext)
{
	int type = 0;
	if (ext == "bmp")					type = CXIMAGE_FORMAT_BMP;
#if CXIMAGE_SUPPORT_JPG
	else if (ext=="jpg"||ext=="jpeg")	type = CXIMAGE_FORMAT_JPG;
#endif
#if CXIMAGE_SUPPORT_GIF
	else if (ext == "gif")				type = CXIMAGE_FORMAT_GIF;
#endif
#if CXIMAGE_SUPPORT_PNG
	else if (ext == "png")				type = CXIMAGE_FORMAT_PNG;
#endif
#if CXIMAGE_SUPPORT_MNG
	else if (ext=="mng"||ext=="jng")	type = CXIMAGE_FORMAT_MNG;
#endif
#if CXIMAGE_SUPPORT_ICO
	else if (ext == "ico")				type = CXIMAGE_FORMAT_ICO;
#endif
#if CXIMAGE_SUPPORT_TIF
	else if (ext=="tiff"||ext=="tif")	type = CXIMAGE_FORMAT_TIF;
#endif
#if CXIMAGE_SUPPORT_TGA
	else if (ext=="tga")				type = CXIMAGE_FORMAT_TGA;
#endif
#if CXIMAGE_SUPPORT_PCX
	else if (ext=="pcx")				type = CXIMAGE_FORMAT_PCX;
#endif
#if CXIMAGE_SUPPORT_WBMP
	else if (ext=="wbmp")				type = CXIMAGE_FORMAT_WBMP;
#endif
#if CXIMAGE_SUPPORT_WMF
	else if (ext=="wmf"||ext=="emf")	type = CXIMAGE_FORMAT_WMF;
#endif
#if CXIMAGE_SUPPORT_J2K
	else if (ext=="j2k"||ext=="jp2")	type = CXIMAGE_FORMAT_J2K;
#endif
#if CXIMAGE_SUPPORT_JBG
	else if (ext=="jbg")				type = CXIMAGE_FORMAT_JBG;
#endif
#if CXIMAGE_SUPPORT_JP2
	else if (ext=="jp2"||ext=="j2k")	type = CXIMAGE_FORMAT_JP2;
#endif
#if CXIMAGE_SUPPORT_JPC
	else if (ext=="jpc"||ext=="j2c")	type = CXIMAGE_FORMAT_JPC;
#endif
#if CXIMAGE_SUPPORT_PGX
	else if (ext=="pgx")				type = CXIMAGE_FORMAT_PGX;
#endif
#if CXIMAGE_SUPPORT_RAS
	else if (ext=="ras")				type = CXIMAGE_FORMAT_RAS;
#endif
#if CXIMAGE_SUPPORT_PNM
	else if (ext=="pnm"||ext=="pgm"||ext=="ppm") type = CXIMAGE_FORMAT_PNM;
#endif
	else type = CXIMAGE_FORMAT_UNKNOWN;

	return type;
}

void CMaterialsEditorView::OnDeleteCurTexture()
{
	POSITION pos = m_textures_list.GetFirstSelectedItemPosition();
	int selTexture = m_textures_list.GetNextSelectedItem(pos);

	if (!GetDocument()->DeleteTexture(selTexture-1))
	{
		ASSERT(0);
		return;
	}

	m_textures_list.DeleteItem(selTexture);

	int lSz = m_textures_list.GetItemCount();
	for (int i=selTexture;i<lSz;i++)
		m_textures_list.SetItemPosition(i,CPoint(i*(THUMBNAIL_WIDTH+THUMBNAIL_SPACE),0));
	m_textures_list.SetFocus();
	m_textures_list.SetItemState(0, LVIS_SELECTED|LVIS_FOCUSED, LVIS_SELECTED|LVIS_FOCUSED);
	m_textures_list.EnsureVisible(0,TRUE);

	Invalidate(FALSE);

}

void CMaterialsEditorView::OnUpdateDeleteCurTexture(CCmdUI *pCmdUI)
{
	POSITION pos = m_textures_list.GetFirstSelectedItemPosition();
	int selTexture = m_textures_list.GetNextSelectedItem(pos);

	if (m_textures_list.GetItemCount()==1 || selTexture==0)
		pCmdUI->Enable(FALSE);
	else
		pCmdUI->Enable(TRUE);
}

void CMaterialsEditorView::OnUpdateDeleteCurMaterial(CCmdUI *pCmdUI)
{
	if (m_mats_count==1)
		pCmdUI->Enable(FALSE);
	else
		pCmdUI->Enable(TRUE);
}

static CBrush brHollow;
static bool   existBrush=false;

void CMaterialsEditorView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();

	m_cmbMaterial.Clear();
	m_cmbMaterial.AddString("Nothing");
	m_cmbMaterial.AddString("Glass 1");
	m_cmbMaterial.AddString("Glass 2");
	m_cmbMaterial.AddString("Glass 3");
	m_cmbMaterial.AddString("Glass 4");
	m_cmbMaterial.AddString("Glass 5");
	m_cmbMaterial.AddString("Glass 6");
	m_cmbMaterial.AddString("Glass 7");
	m_cmbMaterial.AddString("Glass 8");
	m_cmbMaterial.AddString("Glass 9");
	m_cmbMaterial.AddString("Glass 10");
	m_cmbMaterial.AddString("Glass 11");
	m_cmbMaterial.AddString("Glass 12");
	m_cmbMaterial.AddString("Bronze 1");
	m_cmbMaterial.AddString("Bronze 2");
	m_cmbMaterial.AddString("Bronze 3");
	m_cmbMaterial.AddString("Bronze 4");
	m_cmbMaterial.AddString("Bronze 5");
	m_cmbMaterial.AddString("Bronze 6");
	m_cmbMaterial.AddString("Bronze 7");
	m_cmbMaterial.AddString("Bronze 8");
	m_cmbMaterial.AddString("Bronze 9");
	m_cmbMaterial.AddString("Bronze 10");
	m_cmbMaterial.AddString("Bronze 11");
	m_cmbMaterial.AddString("Bronze 12");
	m_cmbMaterial.AddString("Bronze 13");
	m_cmbMaterial.AddString("Bronze 14");
	m_cmbMaterial.AddString("Bronze 15");
	m_cmbMaterial.AddString("Bronze 16");
	m_cmbMaterial.AddString("Bronze 17");
	m_cmbMaterial.AddString("Bronze 18");
	m_cmbMaterial.AddString("Bronze 19");
	m_cmbMaterial.AddString("Bronze 20");
	m_cmbMaterial.AddString("Bronze 21");
	m_cmbMaterial.AddString("Bronze 22");
	m_cmbMaterial.AddString("Bronze 23");
	m_cmbMaterial.AddString("Bronze 24");
	m_cmbMaterial.AddString("Bronze 25");
	m_cmbMaterial.AddString("Chrome 1");
	m_cmbMaterial.AddString("Chrome 2");
	m_cmbMaterial.AddString("Chrome 3");
	m_cmbMaterial.AddString("Chrome 4");
	m_cmbMaterial.AddString("Chrome 5");
	m_cmbMaterial.AddString("Chrome 6");
	m_cmbMaterial.AddString("Chrome 7");
	m_cmbMaterial.AddString("Chrome 8");
	m_cmbMaterial.AddString("Chrome 9");
	m_cmbMaterial.AddString("Chrome 10");
	m_cmbMaterial.AddString("Chrome 11");
	m_cmbMaterial.AddString("Chrome 12");
	m_cmbMaterial.AddString("Chrome 13");
	m_cmbMaterial.AddString("Chrome 14");
	m_cmbMaterial.AddString("Chrome 15");
	m_cmbMaterial.AddString("Chrome 16");
	m_cmbMaterial.AddString("Chrome 17");
	m_cmbMaterial.AddString("Chrome 18");
	m_cmbMaterial.AddString("Chrome 19");
	m_cmbMaterial.AddString("Chrome 20");
	m_cmbMaterial.AddString("Chrome 21");
	m_cmbMaterial.AddString("Chrome 22");
	m_cmbMaterial.AddString("Chrome 23");
	m_cmbMaterial.AddString("Chrome 24");
	m_cmbMaterial.AddString("Chrome 25");
	m_cmbMaterial.AddString("Silver 1");
	m_cmbMaterial.AddString("Silver 2");
	m_cmbMaterial.AddString("Silver 3");
	m_cmbMaterial.AddString("Silver 4");
	m_cmbMaterial.AddString("Silver 5");
	m_cmbMaterial.AddString("Silver 6");
	m_cmbMaterial.AddString("Silver 7");
	m_cmbMaterial.AddString("Silver 8");
	m_cmbMaterial.AddString("Silver 9");
	m_cmbMaterial.AddString("Silver 10");
	m_cmbMaterial.AddString("Silver 11");
	m_cmbMaterial.AddString("Silver 12");
	m_cmbMaterial.AddString("Silver 13");
	m_cmbMaterial.AddString("Silver 14");
	m_cmbMaterial.AddString("Silver 15");
	m_cmbMaterial.AddString("Silver 16");
	m_cmbMaterial.AddString("Silver 17");
	m_cmbMaterial.AddString("Silver 18");
	m_cmbMaterial.AddString("Silver 19");
	m_cmbMaterial.AddString("Silver 20");
	m_cmbMaterial.AddString("Silver 21");
	m_cmbMaterial.AddString("Silver 22");
	m_cmbMaterial.AddString("Silver 23");
	m_cmbMaterial.AddString("Silver 24");
	m_cmbMaterial.AddString("Silver 25");
	m_cmbMaterial.AddString("Gold 1");
	m_cmbMaterial.AddString("Gold 2");
	m_cmbMaterial.AddString("Gold 3");
	m_cmbMaterial.AddString("Gold 4");
	m_cmbMaterial.AddString("Gold 5");
	m_cmbMaterial.AddString("Gold 6");
	m_cmbMaterial.AddString("Gold 7");
	m_cmbMaterial.AddString("Gold 8");
	m_cmbMaterial.AddString("Gold 9");
	m_cmbMaterial.AddString("Gold 10");
	m_cmbMaterial.AddString("Gold 11");
	m_cmbMaterial.AddString("Gold 12");
	m_cmbMaterial.AddString("Gold 13");
	m_cmbMaterial.AddString("Gold 14");
	m_cmbMaterial.AddString("Gold 15");
	m_cmbMaterial.AddString("Gold 16");
	m_cmbMaterial.AddString("Gold 17");
	m_cmbMaterial.AddString("Gold 18");
	m_cmbMaterial.AddString("Gold 19");
	m_cmbMaterial.AddString("Gold 20");
	m_cmbMaterial.AddString("Gold 21");
	m_cmbMaterial.AddString("Gold 22");
	m_cmbMaterial.AddString("Gold 23");
	m_cmbMaterial.AddString("Gold 24");
	m_cmbMaterial.AddString("Gold 25");
	m_cmbMaterial.AddString("Granite 1");
	m_cmbMaterial.AddString("Granite 2");
	m_cmbMaterial.AddString("Granite 3");
	m_cmbMaterial.AddString("Granite 4");
	m_cmbMaterial.AddString("Granite 5");
	m_cmbMaterial.AddString("Granite 6");
	m_cmbMaterial.AddString("Granite 7");
	m_cmbMaterial.AddString("Granite 8");
	m_cmbMaterial.AddString("Granite 9");
	m_cmbMaterial.AddString("Granite 10");
	m_cmbMaterial.AddString("Granite 11");
	m_cmbMaterial.AddString("Granite 12");
	m_cmbMaterial.AddString("Granite 13");
	m_cmbMaterial.AddString("Granite 14");
	m_cmbMaterial.AddString("Granite 15");
	m_cmbMaterial.AddString("Granite 16");
	m_cmbMaterial.AddString("Wood 1");
	m_cmbMaterial.AddString("Wood 2");
	m_cmbMaterial.AddString("Wood 3");
	m_cmbMaterial.AddString("Wood 4");
	m_cmbMaterial.AddString("Wood 5");
	m_cmbMaterial.AddString("Wood 6");
	m_cmbMaterial.AddString("Wood 7");
	m_cmbMaterial.AddString("Wood 8");
	m_cmbMaterial.AddString("Wood 9");
	m_cmbMaterial.AddString("Wood 10");

	GetParentFrame()->RecalcLayout();
	ResizeParentToFit(FALSE);

	m_red_slider.SetRange(0,255);
	m_green_slider.SetRange(0,255);
	m_blue_slider.SetRange(0,255);
	m_transp_slider.SetRange(0,100);
	m_shininess_slider.SetRange(0,255);

	//m_ImageList.Create(MAKEINTRESOURCE(IDB_NO_TEXTURE), THUMBNAIL_WIDTH,  1, RGB(0,0,0));
	if (m_image_list_was_created)
	{
		//m_ImageList.Detach();
		//m_ImageList.DeleteImageList();
		while (m_ImageList.GetImageCount())
			m_ImageList.Remove(0);
		m_textures_list.DeleteAllItems();
	}
	else
	{
		m_ImageList.Create(THUMBNAIL_WIDTH, THUMBNAIL_WIDTH, ILC_COLOR24, 0, 0);
		m_image_list_was_created = true;
		m_textures_list.SetImageList(&m_ImageList, LVSIL_NORMAL );
		m_textures_list.HideScrollBars(LCSB_CLIENTDATA, SB_VERT);
	}

	m_ImageList.SetImageCount(1);

	CBitmap* Image = new CBitmap();		 
	Image->LoadBitmap(MAKEINTRESOURCE(IDB_NO_TEXTURE));

	// add bitmap to our image list
	m_ImageList.Replace(0, Image, NULL);

	m_textures_list.InsertItem(0,NULL,0);
	m_textures_list.SetItemPosition(0,CPoint(0,0));

	delete Image;

	CMaterialsEditorDoc*  pDoc = GetDocument();
	m_mats_count = pDoc->GetMainHeader()->nTotalMat;
	m_comment = pDoc->GetMainHeader()->szComment;

	m_all_materials_list.ResetContent();

	for (int i=0;i<m_mats_count;i++)
		m_all_materials_list.AddString(pDoc->GetMaterialByIndex(i)->szName);

	size_t tSz = pDoc->GetMainHeader()->nTotalTex;

	m_ImageList.SetImageCount((UINT)(tSz+1));

	for (size_t j=0;j<tSz;j++)
	{
		TEXTURE_ITEM* texture = pDoc->GetTextureByIndex((int)j);

		if (!texture)
		{
			ASSERT(0);
			return;
		}

		RGBQUAD pRgb;
		COLORREF winCol = ::GetSysColor(COLOR_WINDOW);
		pRgb.rgbRed = GetRValue(winCol);
		pRgb.rgbGreen = GetGValue(winCol);
		pRgb.rgbBlue = GetBValue(winCol);
		pRgb.rgbReserved = 0;

		CxImage thumbnail(*texture->image);
		thumbnail.Thumbnail(THUMBNAIL_WIDTH,THUMBNAIL_WIDTH,pRgb);

		// add bitmap to our image list
		m_ImageList.Replace((int)j+1, CBitmap::FromHandle(thumbnail.MakeBitmap()), NULL);

		m_textures_list.InsertItem((int)j+1,NULL,(int)j+1);
		m_textures_list.SetItemPosition((int)j+1,CPoint(((int)j+1)*(THUMBNAIL_WIDTH+THUMBNAIL_SPACE),0));
	}
	m_iSolidMaterial=pDoc->GetMaterialByIndex(0)->m_iSolidMaterial;

	UpdateData(FALSE);

	if (m_mats_count)
	{
		m_all_materials_list.SetCurSel(0);
		OnLbnSelchangeMatNamesList();
	}
	if (!existBrush)
	{
		brHollow.CreateSolidBrush(RGB(230,230,230));
		existBrush = true;
	}

	/*m_textures_list.SetFocus();
	m_textures_list.SetItemState(0, LVIS_SELECTED|LVIS_FOCUSED, LVIS_SELECTED|LVIS_FOCUSED);	*/

	/*	CRect rct;
	GetWindowRect(rct);
	GetParentFrame()->MoveWindow(rct.left,rct.top,450,300);*/
}

void CMaterialsEditorView::OnEnChangeMatComment()
{
	GetDlgItem(IDC_MAT_COMMENT)->GetWindowText(m_comment);
	GetDocument()->SetComment(m_comment);
}

void CMaterialsEditorView::OnLbnSelchangeMatNamesList()
{
	m_current_material_index = m_all_materials_list.GetCurSel();
	MAT_ITEM* cur_mat=GetDocument()->GetMaterialByIndex(m_current_material_index);

	if (cur_mat)
		SwitchMaterial();
}

void CMaterialsEditorView::OnEnChangeMatName()
{
	CString newName;
	GetDlgItem(IDC_MAT_NAME)->GetWindowText(newName);

	int cur_ind =  m_all_materials_list.GetCurSel();
	m_all_materials_list.InsertString(cur_ind+1,newName);
	m_all_materials_list.DeleteString(cur_ind);
	m_all_materials_list.SetCurSel(cur_ind);

	//strcpy(GetDocument()->GetMaterialByIndex(cur_ind)->szName,newName.GetBuffer());#OBSOLETE
	strcpy_s(GetDocument()->GetMaterialByIndex(cur_ind)->szName,
		sizeof(GetDocument()->GetMaterialByIndex(cur_ind)->szName),
		newName.GetBuffer());

	m_current_name = newName;
	UpdateData(FALSE);
	GetDocument()->SetModifiedFlag();
}

void CMaterialsEditorView::OnBnClickedAmbientRadio()
{
	m_active_radio = AR_AMBIENT;
	m_ambient_radio = 0;
	SwitchActiveRadio();
}

void CMaterialsEditorView::OnBnClickedDiffusionRadio()
{
	m_active_radio = AR_DIFFUSE;
	m_ambient_radio = 1;
	SwitchActiveRadio();
}

void CMaterialsEditorView::OnBnClickedEmissionRadio()
{
	m_active_radio = AR_EMISSION;
	m_ambient_radio = 2;
	SwitchActiveRadio();
}

void CMaterialsEditorView::OnBnClickedSpecularRadio()
{
	m_active_radio = AR_SPECULAR;
	m_ambient_radio = 3;
	SwitchActiveRadio();
}

void CMaterialsEditorView::OnBnClickedTransparentRadio()
{
	m_active_radio = AR_TRANSPARENT;
	m_ambient_radio = 4;
	SwitchActiveRadio();
}

void CMaterialsEditorView::OnBnClickedShininessRadio()
{
	m_active_radio = AR_SHININESS;
	m_ambient_radio = 5;
	SwitchActiveRadio();
}

void CMaterialsEditorView::SwitchMaterial()
{
	MAT_ITEM* cur_mat=GetDocument()->GetMaterialByIndex(m_current_material_index);

	if (!cur_mat)
		return;

	m_current_name = cur_mat->szName;
	m_preview_material.SetAmbient(cur_mat->AmbientR,cur_mat->AmbientG,cur_mat->AmbientB);
	m_preview_material.SetDiffuse(cur_mat->DiffuseR,cur_mat->DiffuseG,cur_mat->DiffuseB);
	m_preview_material.SetEmission(cur_mat->EmissionR,cur_mat->EmissionG,cur_mat->EmissionB);
	m_preview_material.SetSpecular(cur_mat->SpecularR,cur_mat->SpecularG,cur_mat->SpecularB);
	m_preview_material.SetTransparent(cur_mat->Transparent);
	m_preview_material.SetShininess(cur_mat->Shininess);

	SwitchActiveRadio();

	if (m_textures_list.GetItemCount()>cur_mat->nIdxTexture)
	{
		m_textures_list.SetFocus();
		m_textures_list.SetItemState(cur_mat->nIdxTexture, LVIS_SELECTED|LVIS_FOCUSED, LVIS_SELECTED|LVIS_FOCUSED);
		m_textures_list.EnsureVisible(cur_mat->nIdxTexture,TRUE);
	}
	else
	{
		m_textures_list.SetFocus();
		m_textures_list.SetItemState(0, LVIS_SELECTED|LVIS_FOCUSED, LVIS_SELECTED|LVIS_FOCUSED);
		m_textures_list.EnsureVisible(0,TRUE);
	}
	m_iSolidMaterial=cur_mat->m_iSolidMaterial;

	Invalidate(FALSE);

	UpdateData(FALSE);
}

void CMaterialsEditorView::SwichSlidersVisible()
{
	switch(m_active_radio) 
	{
	case AR_AMBIENT:
	case AR_DIFFUSE:
	case AR_EMISSION:
	case AR_SPECULAR:
		GetDlgItem(IDC_R_STATIC)->ShowWindow(SW_SHOW);
		GetDlgItem(IDC_G_STATIC)->ShowWindow(SW_SHOW);
		GetDlgItem(IDC_B_STATIC)->ShowWindow(SW_SHOW);
		GetDlgItem(IDC_RED_SLIDER)->ShowWindow(SW_SHOW);
		GetDlgItem(IDC_GREEN_SLIDER)->ShowWindow(SW_SHOW);
		GetDlgItem(IDC_BLUE_SLIDER)->ShowWindow(SW_SHOW);
		GetDlgItem(IDC_RED_EDIT)->ShowWindow(SW_SHOW);
		GetDlgItem(IDC_GREEN_EDIT)->ShowWindow(SW_SHOW);
		GetDlgItem(IDC_BLUE_EDIT)->ShowWindow(SW_SHOW);
		GetDlgItem(IDC_TRANSP_SLIDER)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_SHININESS_SLIDER)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_TRANSP_EDIT)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_SHININESS_EDIT)->ShowWindow(SW_HIDE);
		break;
	case AR_TRANSPARENT:
		GetDlgItem(IDC_R_STATIC)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_G_STATIC)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_B_STATIC)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_RED_SLIDER)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_GREEN_SLIDER)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_BLUE_SLIDER)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_RED_EDIT)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_GREEN_EDIT)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_BLUE_EDIT)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_TRANSP_SLIDER)->ShowWindow(SW_SHOW);
		GetDlgItem(IDC_SHININESS_SLIDER)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_TRANSP_EDIT)->ShowWindow(SW_SHOW);
		GetDlgItem(IDC_SHININESS_EDIT)->ShowWindow(SW_HIDE);
		break;
	case AR_SHININESS:
		GetDlgItem(IDC_R_STATIC)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_G_STATIC)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_B_STATIC)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_RED_SLIDER)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_GREEN_SLIDER)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_BLUE_SLIDER)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_RED_EDIT)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_GREEN_EDIT)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_BLUE_EDIT)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_TRANSP_SLIDER)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_SHININESS_SLIDER)->ShowWindow(SW_SHOW);
		GetDlgItem(IDC_TRANSP_EDIT)->ShowWindow(SW_HIDE);
		GetDlgItem(IDC_SHININESS_EDIT)->ShowWindow(SW_SHOW);
		break;
	default:
		break;
	}
}

void CMaterialsEditorView::SwitchActiveRadio()
{
	MAT_ITEM* curMat=GetDocument()->GetMaterialByIndex(m_current_material_index);

	if (!curMat)
		return;

	SwichSlidersVisible();

	switch(m_active_radio) 
	{
	case AR_AMBIENT:
		m_red_slider.SetPos((int)(curMat->AmbientR*255.0f));
		m_green_slider.SetPos((int)(curMat->AmbientG*255.0f));
		m_blue_slider.SetPos((int)(curMat->AmbientB*255.0f));
		break;
	case AR_DIFFUSE:
		m_red_slider.SetPos((int)(curMat->DiffuseR*255.0f));
		m_green_slider.SetPos((int)(curMat->DiffuseG*255.0f));
		m_blue_slider.SetPos((int)(curMat->DiffuseB*255.0f));
		break;
	case AR_EMISSION:
		m_red_slider.SetPos((int)(curMat->EmissionR*255.0f));
		m_green_slider.SetPos((int)(curMat->EmissionG*255.0f));
		m_blue_slider.SetPos((int)(curMat->EmissionB*255.0f));
		break;
	case AR_SPECULAR:
		m_red_slider.SetPos((int)(curMat->SpecularR*255.0f));
		m_green_slider.SetPos((int)(curMat->SpecularG*255.0f));
		m_blue_slider.SetPos((int)(curMat->SpecularB*255.0f));
		break;
	case AR_TRANSPARENT:
		m_transp_slider.SetPos((int)(curMat->Transparent*100.0f));
		break;
	case AR_SHININESS:
		m_shininess_slider.SetPos((int)(curMat->Shininess));
		break;
	default:
		break;
	}

	CString  labelStr;
	labelStr.Format("%i",m_red_slider.GetPos());
	GetDlgItem(IDC_RED_EDIT)->SetWindowText(labelStr);
	labelStr.Format("%i",m_green_slider.GetPos());
	GetDlgItem(IDC_GREEN_EDIT)->SetWindowText(labelStr);
	labelStr.Format("%i",m_blue_slider.GetPos());
	GetDlgItem(IDC_BLUE_EDIT)->SetWindowText(labelStr);
	labelStr.Format("%i",m_transp_slider.GetPos());
	GetDlgItem(IDC_TRANSP_EDIT)->SetWindowText(labelStr);
	labelStr.Format("%i",m_shininess_slider.GetPos());
	GetDlgItem(IDC_SHININESS_EDIT)->SetWindowText(labelStr);

	UpdateData(FALSE);

}

void CMaterialsEditorView::OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{
	if (!pScrollBar)
	{
		CFormView::OnHScroll(nSBCode, nPos, pScrollBar);
		return;
	}

	CSliderCtrl *pSlider = (CSliderCtrl *)pScrollBar;
	int nID = pSlider->GetDlgCtrlID();

	CString  labelStr;
	labelStr.Format("%i",pSlider->GetPos());

	MAT_ITEM* curMat = GetDocument()->GetMaterialByIndex(m_current_material_index);

	ASSERT(curMat);

	switch (nID)
	{
	case IDC_RED_SLIDER:
		switch(m_active_radio) 
		{
		case AR_AMBIENT:
			curMat->AmbientR = (float)pSlider->GetPos()/255.0f;
			m_preview_material.SetAmbient(curMat->AmbientR,curMat->AmbientG,curMat->AmbientB);
			break;
		case AR_DIFFUSE:
			curMat->DiffuseR = (float)pSlider->GetPos()/255.0f;
			m_preview_material.SetDiffuse(curMat->DiffuseR,curMat->DiffuseG,curMat->DiffuseB);
			break;
		case AR_EMISSION:
			curMat->EmissionR = (float)pSlider->GetPos()/255.0f;
			m_preview_material.SetEmission(curMat->EmissionR,curMat->EmissionG,curMat->EmissionB);
			break;
		case AR_SPECULAR:
			curMat->SpecularR = (float)pSlider->GetPos()/255.0f;
			m_preview_material.SetSpecular(curMat->SpecularR,curMat->SpecularG,curMat->SpecularB);
			break;
		default:
			break;
		}
		GetDlgItem(IDC_RED_EDIT)->SetWindowText(labelStr);
		break;
	case IDC_GREEN_SLIDER:
		switch(m_active_radio) 
		{
		case AR_AMBIENT:
			curMat->AmbientG = (float)pSlider->GetPos()/255.0f;
			m_preview_material.SetAmbient(curMat->AmbientR,curMat->AmbientG,curMat->AmbientB);
			break;
		case AR_DIFFUSE:
			curMat->DiffuseG = (float)pSlider->GetPos()/255.0f;
			m_preview_material.SetDiffuse(curMat->DiffuseR,curMat->DiffuseG,curMat->DiffuseB);
			break;
		case AR_EMISSION:
			curMat->EmissionG = (float)pSlider->GetPos()/255.0f;
			m_preview_material.SetEmission(curMat->EmissionR,curMat->EmissionG,curMat->EmissionB);
			break;
		case AR_SPECULAR:
			curMat->SpecularG = (float)pSlider->GetPos()/255.0f;
			m_preview_material.SetSpecular(curMat->SpecularR,curMat->SpecularG,curMat->SpecularB);
			break;
		default:
			break;
		}
		GetDlgItem(IDC_GREEN_EDIT)->SetWindowText(labelStr);
		break;
	case IDC_BLUE_SLIDER:
		switch(m_active_radio) 
		{
		case AR_AMBIENT:
			curMat->AmbientB = (float)pSlider->GetPos()/255.0f;
			m_preview_material.SetAmbient(curMat->AmbientR,curMat->AmbientG,curMat->AmbientB);
			break;
		case AR_DIFFUSE:
			curMat->DiffuseB = (float)pSlider->GetPos()/255.0f;
			m_preview_material.SetDiffuse(curMat->DiffuseR,curMat->DiffuseG,curMat->DiffuseB);
			break;
		case AR_EMISSION:
			curMat->EmissionB = (float)pSlider->GetPos()/255.0f;
			m_preview_material.SetEmission(curMat->EmissionR,curMat->EmissionG,curMat->EmissionB);
			break;
		case AR_SPECULAR:
			curMat->SpecularB = (float)pSlider->GetPos()/255.0f;
			m_preview_material.SetSpecular(curMat->SpecularR,curMat->SpecularG,curMat->SpecularB);
			break;
		default:
			break;
		}
		GetDlgItem(IDC_BLUE_EDIT)->SetWindowText(labelStr);
		break;
	case IDC_TRANSP_SLIDER:
		curMat->Transparent = (float)pSlider->GetPos()/100.0f;
		m_preview_material.SetTransparent(curMat->Transparent);
		GetDlgItem(IDC_TRANSP_EDIT)->SetWindowText(labelStr);
		break;
	case IDC_SHININESS_SLIDER:
		curMat->Shininess = (float)pSlider->GetPos();
		m_preview_material.SetShininess(curMat->Shininess);
		GetDlgItem(IDC_SHININESS_EDIT)->SetWindowText(labelStr);
		break;
	}
GetDocument()->SetModifiedFlag();
	CFormView::OnHScroll(nSBCode, nPos, pScrollBar);
}

void CMaterialsEditorView::OnNMClickTexturesList(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMLISTVIEW pNMLV = reinterpret_cast<LPNMLISTVIEW>(pNMHDR);
	MAT_ITEM* curMat = GetDocument()->GetMaterialByIndex(m_current_material_index);
	GetDocument()->SetModifiedFlag();
	curMat->nIdxTexture = pNMLV->iItem;
	Invalidate(FALSE);
	*pResult = 0;
}
// CMaterialsEditorView message handlers

//BOOL CMaterialsEditorView::DestroyWindow()
//{
//	// TODO: Add your specialized code here and/or call the base class
//
//	return CFormView::DestroyWindow();
//}
#include "MemDC.h"
#define USE_MEM_DC // Comment this out, if you don't want to use CMemDC

static void   DrawGroupFrame(CDC* pDC, const CRect& rct, 
							 const int leftLab, const int rightLab)
{
	CPen pen1;
	pen1.CreatePen(PS_SOLID, 1, ::GetSysColor(COLOR_3DSHADOW));
	CPen* pOldPen = (CPen*) pDC->SelectObject(&pen1);

	pDC->MoveTo(leftLab,rct.top);
	pDC->LineTo(rct.left,rct.top);
	pDC->LineTo(rct.left,rct.bottom-1);
	pDC->LineTo(rct.right-1,rct.bottom-1);
	pDC->LineTo(rct.right-1,rct.top);
	pDC->LineTo(rightLab,rct.top);

	CPen pen2;
	pen2.CreatePen(PS_SOLID, 1, ::GetSysColor(COLOR_3DHILIGHT));
	pDC->SelectObject(&pen2);
	DeleteObject(pen1);

	pDC->MoveTo(leftLab,rct.top+1);
	pDC->LineTo(rct.left+1,rct.top+1);
	pDC->LineTo(rct.left+1,rct.bottom-1);
	pDC->MoveTo(rct.left,rct.bottom);
	pDC->LineTo(rct.right,rct.bottom);
	pDC->LineTo(rct.right,rct.top);
	pDC->MoveTo(rct.right-1,rct.top+1);
	pDC->LineTo(rightLab,rct.top+1);

	pDC->SelectObject(pOldPen);
	DeleteObject(pen2);
}

BOOL CMaterialsEditorView::OnEraseBkgnd(CDC* pDC)
{
#ifdef USE_MEM_DC
	CMemDC memDC(pDC);
#else
	CDC* memDC = pDC;
#endif

	CRect rrr;
	GetClientRect(rrr);
	themeData.DrawThemedRect(&memDC,&rrr,FALSE);

	GetDlgItem(IDC_GROUP_FRAME)->GetWindowRect(rrr);
	ScreenToClient(rrr);

	CRect parRR;
	GetDlgItem(IDC_PARAMS_LAB)->GetWindowRect(parRR);
	ScreenToClient(parRR);

	DrawGroupFrame(&memDC,rrr,parRR.left-4,parRR.right);

	return TRUE; // tell Windows we handled it

}

HBRUSH CMaterialsEditorView::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = CFormView::OnCtlColor(pDC, pWnd, nCtlColor);

	if (pWnd==&m_red_slider ||
		pWnd==&m_green_slider ||
		pWnd==&m_blue_slider ||
		pWnd==&m_transp_slider ||
		pWnd==&m_shininess_slider)
			return hbr;

	
	if (pWnd==&m_red_edit ||
		pWnd==&m_green_edit ||
		pWnd==&m_blue_edit ||
		pWnd==&m_transp_edit ||
		pWnd==&m_sh_edit ||
		pWnd==&m_count_edit)
	{
		pDC->SetBkMode(TRANSPARENT);
		return brHollow;
	}

/*
	switch ( nCtlColor ) {
		case CTLCOLOR_STATIC:
		case CTLCOLOR_SCROLLBAR:
			pDC->SetBkMode(TRANSPARENT);
			//break;

	} // switch
*/
	if (nCtlColor!=CTLCOLOR_EDIT &&
		nCtlColor!=CTLCOLOR_LISTBOX &&
		nCtlColor!=CTLCOLOR_SCROLLBAR)
	{
		pDC->SetTextColor(0);
		//pDC->SetBkColor(RGB(255,255,255));
		pDC->SetBkMode(TRANSPARENT);
		hbr = (HBRUSH)GetStockObject(HOLLOW_BRUSH);
	}
	return hbr;
}

void CMaterialsEditorView::OnCbnSelchangeCmbMaterial()
{
	UpdateData();
	GetDocument()->GetMaterialByIndex(m_all_materials_list.GetCurSel())->m_iSolidMaterial=m_iSolidMaterial;
}
