#ifndef __PLUGINBASE__
#define __PLUGINBASE__

#include "..//resource.h"

#include "..//CommonStructures.h"

static   UINT pluginMenuIDStart = START_ID_FOR_PLUGINS_MENU;

class CPluginBase
{
public:
      CPluginBase(HINSTANCE hInst, PLUGIN_INFO plInfo) 
	  {
		  m_hInst = hInst;
		  m_info = plInfo;
		  m_nID_Menu = pluginMenuIDStart++;
	  };
      ~CPluginBase() 
	  {
		  if (m_hInst)
			  AfxFreeLibrary(m_hInst);
	  };

protected:
      HINSTANCE   m_hInst;
public:
	  PLUGIN_INFO m_info;
	  UINT        m_nID_Menu;
};

#endif
