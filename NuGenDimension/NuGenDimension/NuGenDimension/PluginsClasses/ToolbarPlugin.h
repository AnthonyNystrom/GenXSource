#ifndef __TOOLBARPLUGIN__
#define __TOOLBARPLUGIN__

#include "PluginBase.h"
//#include "..//Controls//My32Toolbar.h"

static   UINT pluginToolbarIDStart = START_ID_FOR_PLUGINS_TOOLBARS;

typedef void(*LPDLLFUNC_RESET_NAMES)();
typedef ICommander*(*LPDLLFUNC_GET_NEW_COMMANDER)(unsigned int, IApplicationInterface*);
typedef ICommander*(*LPDLLFUNC_GET_EDIT_COMMANDER)(sgCObject* , IApplicationInterface*);
typedef bool(*LPDLLFUNC_FREE_COMMANDER)(ICommander*);
typedef void(*LPDLLFUNC_GET_BUTTON_STATE)(unsigned int, bool&, bool&);
typedef LPCTSTR(*LPDLLFUNC_STATUSBAR_STRING)(unsigned int);
typedef LPCTSTR(*LPDLLFUNC_TOOLTIP_STRING)(unsigned int);

class CToolbarPlugin : public CPluginBase
{
	friend class CPluginLoader;
public:
      CToolbarPlugin(HINSTANCE hInst, PLUGIN_INFO plInfo);
	  ~CToolbarPlugin();
   
	  CEGToolBar    m_toolbar;

	  UINT           m_start_ID;
	  UINT           m_end_ID;
	  bool           m_was_load;
	  bool           LoadSupportedIDs();
	  bool           LoadObjectsBitmaps();
	  bool           LoadToolbar(CWnd* pParent);
	  
private:
	  std::vector<const char*>		m_supported_objectsIDs;
	  std::vector<CBitmap32*>		m_object_bitmaps;
	  LPDLLFUNC_RESET_NAMES         m_reset_names_func;
	  LPDLLFUNC_GET_BUTTON_STATE	m_get_button_state_func;
	  LPDLLFUNC_STATUSBAR_STRING	m_get_status_bar_message;
	  LPDLLFUNC_TOOLTIP_STRING		m_get_tooltip_message;

	  LPDLLFUNC_GET_NEW_COMMANDER   m_get_new_commander;
	  LPDLLFUNC_GET_EDIT_COMMANDER  m_get_edit_commander;
	  LPDLLFUNC_FREE_COMMANDER      m_free_commander;
public:
	void           ResetNames();
	void           GetToolbarButtonState(unsigned int nID, bool& ch, bool& enbl);
	CString        GetStatusBarMessage(unsigned int nID);
	CString        GetTooltipMessage(unsigned int nID);

    ICommander*    GetNewCommander(unsigned int nID, IApplicationInterface* appInt);
	ICommander*    GetEditCommander(sgCObject* editableObj, IApplicationInterface* appInt);
	void           FreeCommander(ICommander* cmndr);
};

#endif
