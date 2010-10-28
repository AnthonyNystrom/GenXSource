// ArMaXPropPage.cpp : Implementation of the CArMaXPropPage property page class.

#include "stdafx.h"
#include "ArMaX.h"
#include "ArMaXPropPage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


IMPLEMENT_DYNCREATE(CArMaXPropPage, COlePropertyPage)



// Message map

BEGIN_MESSAGE_MAP(CArMaXPropPage, COlePropertyPage)
END_MESSAGE_MAP()



// Initialize class factory and guid

IMPLEMENT_OLECREATE_EX(CArMaXPropPage, "ArMaX.ArMaXPropPage.1",
	0x55662852, 0x6e39, 0x437a, 0xbe, 0x60, 0x86, 0x6, 0x67, 0x9c, 0xbe, 0x5b)



// CArMaXPropPage::CArMaXPropPageFactory::UpdateRegistry -
// Adds or removes system registry entries for CArMaXPropPage

BOOL CArMaXPropPage::CArMaXPropPageFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_ArMaX_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}



// CArMaXPropPage::CArMaXPropPage - Constructor

CArMaXPropPage::CArMaXPropPage() :
	COlePropertyPage(IDD, IDS_ArMaX_PPG_CAPTION)
{
}



// CArMaXPropPage::DoDataExchange - Moves data between page and properties

void CArMaXPropPage::DoDataExchange(CDataExchange* pDX)
{
	DDP_PostProcessing(pDX);
}



// CArMaXPropPage message handlers
