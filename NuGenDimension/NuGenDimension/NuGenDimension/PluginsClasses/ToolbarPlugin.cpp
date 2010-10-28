#include "stdafx.h"

#include "ToolbarPlugin.h"

CToolbarPlugin::CToolbarPlugin(HINSTANCE hInst, PLUGIN_INFO plInfo):
				CPluginBase(hInst,plInfo)
{
	m_start_ID = 0;
	m_end_ID = 0;
	m_was_load = false;
	m_reset_names_func = NULL;
	m_get_button_state_func=NULL;
	m_get_status_bar_message = NULL;
	m_get_tooltip_message=NULL;

	m_get_new_commander = NULL;
	m_get_edit_commander = NULL;
	m_free_commander = NULL;
}

CToolbarPlugin::~CToolbarPlugin()
{
	for (size_t i=0, sz=m_object_bitmaps.size();i<sz;i++)
		delete m_object_bitmaps[i];
}

 bool CToolbarPlugin::LoadSupportedIDs()
 {
	 typedef void(*LPDLLFUNC_FILL_SUPPORTED_IDS)(std::vector<const char*>&);
	 LPDLLFUNC_FILL_SUPPORTED_IDS  lpfnDllFuncFillObjectsIDs = NULL;

	 lpfnDllFuncFillObjectsIDs = (LPDLLFUNC_FILL_SUPPORTED_IDS)::GetProcAddress(m_hInst,"GetSupportedObjectIDs");
	 if (lpfnDllFuncFillObjectsIDs)
		 lpfnDllFuncFillObjectsIDs(m_supported_objectsIDs);
	 else
		 return false;

	 return true;
}

 bool  CToolbarPlugin::LoadObjectsBitmaps()
 {
	 typedef HBITMAP(*LPDLLFUNC_GET_OBJECT_BITMAP)(const char*, LPWORD);
	 LPDLLFUNC_GET_OBJECT_BITMAP  lpfnDllFuncGetObjectBitmap = NULL;

	 m_object_bitmaps.assign(m_supported_objectsIDs.size(), NULL);
	 lpfnDllFuncGetObjectBitmap = (LPDLLFUNC_GET_OBJECT_BITMAP)::GetProcAddress(m_hInst,"GetObjectBitmap");
	 if (lpfnDllFuncGetObjectBitmap)
	 {
		 size_t sz=m_supported_objectsIDs.size();
		 for (size_t i=0;i<sz;i++)
		 {
			 CBitmap32* tmpBmp = new CBitmap32;
			 WORD lc;
			 HBITMAP tmpHbmp = lpfnDllFuncGetObjectBitmap(m_supported_objectsIDs[i],&lc);
			 if (tmpHbmp)
			 {
				tmpBmp->Attach(tmpHbmp,&lc);
				m_object_bitmaps[i] = tmpBmp;
			 }
			 else
			 {
				 ASSERT(0);
				 delete tmpBmp;
			 }
		 }
	 }
	 else
		 return false;

	 return true;
 }

bool  CToolbarPlugin::LoadToolbar(CWnd* pParent)
{
	typedef void(*LPDLLFUNC_LOADTOOLBAR)(unsigned int, CToolBar* , CWnd* );
	typedef HBITMAP(*LPDLLFUNC_GETTOOLBARBITMAP)(LPWORD);
	typedef void(*LPDLLFUNC_GETIDS)(unsigned int&, unsigned int&);
	
	LPDLLFUNC_LOADTOOLBAR lpfnDllFuncLoadToolbar = NULL;
	LPDLLFUNC_GETTOOLBARBITMAP lpfnDllGetToolbarBitmap = NULL;
	LPDLLFUNC_GETIDS  lpfnDllFuncGetIDS = NULL;
		
	lpfnDllFuncLoadToolbar = (LPDLLFUNC_LOADTOOLBAR)::GetProcAddress(m_hInst,"GetToolbar");
	if (!lpfnDllFuncLoadToolbar)
	{
		AfxMessageBox("Function not found in DLL");
		return false;
	}
	lpfnDllGetToolbarBitmap = (LPDLLFUNC_GETTOOLBARBITMAP)::GetProcAddress(m_hInst,"GetToolbarBitmap");
	if (!lpfnDllGetToolbarBitmap)
	{
		AfxMessageBox("Function not found in DLL");
		return false;
	}

	lpfnDllFuncLoadToolbar(pluginToolbarIDStart, &m_toolbar,pParent);
	WORD wColor;
	HBITMAP  hBmp = lpfnDllGetToolbarBitmap(&wColor);
	m_toolbar.LoadHiColor(hBmp,&wColor);
	lpfnDllFuncGetIDS = (LPDLLFUNC_GETIDS)::GetProcAddress(m_hInst,"GetFirstAndLastIDs");
	lpfnDllFuncGetIDS(m_start_ID,m_end_ID);
	pluginToolbarIDStart = m_end_ID+1;

	m_reset_names_func = (LPDLLFUNC_RESET_NAMES)::GetProcAddress(m_hInst,"ResetNames");
	m_get_button_state_func = (LPDLLFUNC_GET_BUTTON_STATE)::GetProcAddress(m_hInst,"GetButtonState");
	m_get_status_bar_message = (LPDLLFUNC_STATUSBAR_STRING)::GetProcAddress(m_hInst,"GetStatusBarMessage");
	m_get_tooltip_message=(LPDLLFUNC_TOOLTIP_STRING)::GetProcAddress(m_hInst,"GetTooltipMessage");

	m_get_new_commander = (LPDLLFUNC_GET_NEW_COMMANDER)::GetProcAddress(m_hInst,"GetNewCommander");
	m_get_edit_commander = (LPDLLFUNC_GET_EDIT_COMMANDER)::GetProcAddress(m_hInst,"GetEditCommander");
	m_free_commander = (LPDLLFUNC_FREE_COMMANDER)::GetProcAddress(m_hInst,"FreeCommander");
	
	m_was_load = true;
	return true;
}

void  CToolbarPlugin::ResetNames()
{
	if (m_reset_names_func)
		m_reset_names_func();
}

void CToolbarPlugin::GetToolbarButtonState(unsigned int nID, bool& ch, bool& enbl)
{
	if (m_get_button_state_func)
		m_get_button_state_func(nID,ch,enbl);
}

CString  CToolbarPlugin::GetStatusBarMessage(unsigned int nID)
{
	if (m_get_status_bar_message)
		return CString(m_get_status_bar_message(nID));
	return CString("");
}

CString  CToolbarPlugin::GetTooltipMessage(unsigned int nID)
{
	if (m_get_tooltip_message)
		return CString(m_get_tooltip_message(nID));
	return CString("");
}

ICommander* CToolbarPlugin::GetNewCommander(unsigned int nID, 
												IApplicationInterface* appInt)
{
	if (m_get_new_commander)
		return m_get_new_commander(nID,appInt);
	return NULL;
}

ICommander*  CToolbarPlugin::GetEditCommander(sgCObject* editableObj, 
											  IApplicationInterface* appInt)
{
	if (m_get_edit_commander)
		return m_get_edit_commander(editableObj,appInt);
	return NULL;
}


void        CToolbarPlugin::FreeCommander(ICommander* cmndr)
{
	if (m_free_commander)
		if (!m_free_commander(cmndr))
			ASSERT(0);
}