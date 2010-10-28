#include "stdafx.h"
#include "EGMenu.h"
#include "EGToolBar.h"
#include "EGProperty.h"

// CEGPropertyPage dialog

IMPLEMENT_DYNAMIC(CEGPropertyPage,  CPropertyPage)
CEGPropertyPage::CEGPropertyPage()
{
}

CEGPropertyPage::~CEGPropertyPage()
{
}

#if _MFC_VER < 0x0700 
CEGPropertyPage::CEGPropertyPage(UINT nIDTemplate, UINT nIDCaption)
{
  ASSERT(nIDTemplate != 0);
  CommonConstruct(MAKEINTRESOURCE(nIDTemplate), nIDCaption);
}

CEGPropertyPage::CEGPropertyPage(LPCTSTR lpszTemplateName, UINT nIDCaption)
{
  ASSERT(AfxIsValidString(lpszTemplateName));
  CommonConstruct(lpszTemplateName, nIDCaption);
}

#else

CEGPropertyPage::CEGPropertyPage(UINT nIDTemplate, UINT nIDCaption, DWORD dwSize)
{
  free(m_pPSP);
  m_pPSP=NULL;
  
  ASSERT(nIDTemplate != 0);
  AllocPSP(dwSize);
  CommonConstruct(MAKEINTRESOURCE(nIDTemplate), nIDCaption);
}

CEGPropertyPage::CEGPropertyPage(LPCTSTR lpszTemplateName, UINT nIDCaption , DWORD dwSize)
{
  free(m_pPSP);
  m_pPSP=NULL;
  
  ASSERT(AfxIsValidString(lpszTemplateName));
  AllocPSP(dwSize);
  CommonConstruct(lpszTemplateName, nIDCaption);
}

// extended construction
CEGPropertyPage::CEGPropertyPage(UINT nIDTemplate, UINT nIDCaption, UINT nIDHeaderTitle, UINT nIDHeaderSubTitle, DWORD dwSize)
{
  free(m_pPSP);
  m_pPSP=NULL;
  
  ASSERT(nIDTemplate != 0);
  AllocPSP(dwSize);
  CommonConstruct(MAKEINTRESOURCE(nIDTemplate), nIDCaption, nIDHeaderTitle, nIDHeaderSubTitle);
}

CEGPropertyPage::CEGPropertyPage(LPCTSTR lpszTemplateName, UINT nIDCaption, 	UINT nIDHeaderTitle, UINT nIDHeaderSubTitle, DWORD dwSize)
{
  free(m_pPSP);
  m_pPSP=NULL;
  
  ASSERT(AfxIsValidString(lpszTemplateName));
  AllocPSP(dwSize);
  CommonConstruct(lpszTemplateName, nIDCaption, nIDHeaderTitle, nIDHeaderSubTitle);
}

CEGPropertyPage::CEGPropertyPage(UINT nIDTemplate, TCHAR* lpszHeaderTitle, TCHAR* lpszHeaderSubTitle, DWORD dwSize ){
	free(m_pPSP);
	m_pPSP=NULL;
  
	ASSERT(nIDTemplate != 0);
	AllocPSP(dwSize);
	CommonConstruct(MAKEINTRESOURCE(nIDTemplate), 0);
	
	m_strHeaderTitle = lpszHeaderTitle;
	m_strHeaderSubTitle = lpszHeaderSubTitle;
	
	m_psp.dwFlags &= ~PSP_HASHELP;
}

void CEGPropertyPage::SetWizardButtons( DWORD dwFlags ) {
	CEGPropertySheet* pSheet = (CEGPropertySheet*) GetParent();   
	if (pSheet)
		pSheet->SetWizardButtons( dwFlags );
}

#endif

BEGIN_MESSAGE_MAP(CEGPropertyPage,  CEGFrame<CPropertyPage>)
END_MESSAGE_MAP()

CEGStepsPropertyPage::CEGStepsPropertyPage( UINT nIDTemplate, TCHAR* lpszTitle, TCHAR* lpszSubTitle ):
	CEGPropertyPage( nIDTemplate, lpszTitle, lpszSubTitle )
{
	m_nStep = 0;
}

void CEGStepsPropertyPage::InitStepControls(UINT nIDListView, UINT nIDProgress, UINT nIDBImages ) {

	m_hList = ::GetDlgItem( m_hWnd, nIDListView );
	m_hProgress = ::GetDlgItem( m_hWnd, nIDProgress );

	::SendMessage( m_hProgress, PBM_SETSTEP, 1, 0 );

	ListView_SetExtendedListViewStyle( m_hList, WS_EX_STATICEDGE | LVS_EX_FULLROWSELECT  );

	CRect rc; 
	::GetClientRect( m_hList, &rc );

	LV_COLUMN lvc;

	lvc.iSubItem = 0;
	lvc.mask = LVCF_FMT | LVCF_WIDTH | LVCF_TEXT | LVCF_SUBITEM;
	lvc.fmt = LVCFMT_LEFT;

	lvc.iSubItem++;
	lvc.cx = rc.right - rc.left-2; 
	lvc.pszText = _T("ируш");
	ListView_InsertColumn(m_hList, lvc.iSubItem, &lvc);

	CImageList iml;
	iml.Create(16, 16, ILC_COLOR24 | ILC_MASK, 0, 4 );
	CBitmap bmp;
	bmp.LoadBitmap( nIDBImages );
	iml.Add(&bmp, RGB(255,0,255) );
	ListView_SetImageList( m_hList, iml.Detach(), LVSIL_SMALL );
}

void CEGStepsPropertyPage::AddStep( TCHAR* lpszName ) {
	ASSERT( m_hList != NULL );
	
	LV_ITEM lvi;
	lvi.iItem = ListView_GetItemCount( m_hList );
	lvi.iSubItem = 0;
	lvi.mask = LVIF_IMAGE | LVIF_TEXT ;
	lvi.iImage = 0;

	lvi.pszText = lpszName;
	ListView_InsertItem( m_hList, &lvi);	
}

void CEGStepsPropertyPage::SetStepImage(int nIndex, int nImage){
	ASSERT( m_hList != NULL );
	ASSERT( m_hProgress != NULL );

	LV_ITEM lvi;
	lvi.iItem = nIndex;
	lvi.iSubItem = 0;
	lvi.mask = LVIF_IMAGE;
	lvi.iImage = nImage;
	ListView_SetItem(m_hList, &lvi );
}
	
void CEGStepsPropertyPage::BeginSubStep(DWORD dwMin, DWORD dwMax){
	ASSERT( m_hList != NULL );
	ASSERT( m_hProgress != NULL );

	::SendMessage( m_hProgress, PBM_SETPOS, 0, 0 );
	::SendMessage( m_hProgress, PBM_SETRANGE, dwMin, dwMax );

	::UpdateWindow( m_hWnd );
	::UpdateWindow( m_hProgress );
}

void CEGStepsPropertyPage::NextSubStep(){
	ASSERT( m_hList != NULL );
	ASSERT( m_hProgress != NULL );

	::SendMessage( m_hProgress, PBM_STEPIT, 0, 0 );

	::UpdateWindow( m_hWnd );
	::UpdateWindow( m_hProgress );
}

void CEGStepsPropertyPage::BeginWork(){
	ASSERT( m_hList != NULL );
	ASSERT( m_hProgress != NULL );

	m_nStep = 0;

	// clear progress
	::SendMessage( m_hProgress, PBM_SETPOS, 0, 0 );
	::SendMessage( m_hProgress, PBM_SETRANGE, 1, 100 );
	::UpdateWindow( m_hProgress );

	// clear steps
	int nSteps = ListView_GetItemCount( m_hList );
	while (nSteps--)
		SetStepImage( nSteps, 0 );
	::UpdateWindow( m_hList );

	// update entire window
	::UpdateWindow( m_hWnd );
}

void CEGStepsPropertyPage::BeginStep(DWORD dwMin, DWORD dwMax){
	ASSERT( m_hList != NULL );
	ASSERT( m_hProgress != NULL );

	SetStepImage( m_nStep, 1);
	BeginSubStep( dwMin, dwMax );

	::UpdateWindow( m_hWnd );
	::UpdateWindow (m_hList );
}

void CEGStepsPropertyPage::FinishStep(BOOL bSucceed){
	ASSERT( m_hList != NULL );
	ASSERT( m_hProgress != NULL );

	SetStepImage( m_nStep++, bSucceed ? 2 : 3);

	::UpdateWindow( m_hWnd );
	::UpdateWindow (m_hList );
}

// CEGPropertyPage dialog

IMPLEMENT_DYNAMIC(CEGPropertySheet,  CPropertySheet)
CEGPropertySheet::CEGPropertySheet()
{
}

CEGPropertySheet::~CEGPropertySheet()
{
}

CEGPropertySheet::CEGPropertySheet(UINT nIDCaption, CWnd* pParentWnd,	UINT iSelectPage)
{
  ASSERT(nIDCaption != 0);
  
  VERIFY(m_strCaption.LoadString(nIDCaption));
  CommonConstruct(pParentWnd, iSelectPage);
}

CEGPropertySheet::CEGPropertySheet(LPCTSTR pszCaption, CWnd* pParentWnd, UINT iSelectPage)
{
  ASSERT(pszCaption != NULL);
  
  m_strCaption = pszCaption;
  CommonConstruct(pParentWnd, iSelectPage);
}

#if _MFC_VER < 0x0700
#else
// extended construction
CEGPropertySheet::CEGPropertySheet(UINT nIDCaption, CWnd* pParentWnd,	UINT iSelectPage, HBITMAP hbmWatermark,	HPALETTE hpalWatermark, HBITMAP hbmHeader)
{
  ASSERT(nIDCaption != 0);
  
  VERIFY(m_strCaption.LoadString(nIDCaption));
	CommonConstruct(pParentWnd, iSelectPage, hbmWatermark, hpalWatermark, hbmHeader);
}

CEGPropertySheet::CEGPropertySheet(LPCTSTR pszCaption, CWnd* pParentWnd,	UINT iSelectPage, HBITMAP hbmWatermark,	HPALETTE hpalWatermark , HBITMAP hbmHeader)
{
  ASSERT(pszCaption != NULL);
  
  m_strCaption = pszCaption;
	CommonConstruct(pParentWnd, iSelectPage, hbmWatermark, hpalWatermark, hbmHeader);
}

void CEGPropertySheet::SetWizard97Mode( UINT nDBImage ){
	SetWizardMode();

	m_psh.hInstance = AfxGetInstanceHandle();
	m_psh.dwFlags = PSH_PROPSHEETPAGE | PSH_WIZARD97 | PSH_HEADER;
	m_psh.pszbmHeader = MAKEINTRESOURCE( nDBImage );	
}

#endif

BEGIN_MESSAGE_MAP(CEGPropertySheet,  CEGFrame<CPropertySheet>)
END_MESSAGE_MAP()

BOOL CEGPropertySheet::OnInitDialog()
{
  BOOL bRetval = CPropertySheet::OnInitDialog();

  HMENU hMenu = m_SystemNewMenu.Detach();
  HMENU hSysMenu = ::GetSystemMenu(m_hWnd,FALSE);
  if(hMenu!=hSysMenu)
  {
    if(IsMenu(hMenu))
    {
      ::DestroyMenu(hMenu);
    }
  }
  m_SystemNewMenu.Attach(hSysMenu);
  m_DefaultNewMenu.LoadMenu(::GetMenu(m_hWnd));

  if(IsMenu(m_DefaultNewMenu.m_hMenu))
  {
    UpdateMenuBarColor(m_DefaultNewMenu);
  }

  return bRetval;
}
// CEGPropertyPage message handlers

