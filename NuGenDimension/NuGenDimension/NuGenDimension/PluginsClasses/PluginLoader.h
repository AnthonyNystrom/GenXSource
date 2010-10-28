#ifndef __PLUGIN_LOADER__
#define __PLUGIN_LOADER__

#include "ToolbarPlugin.h"

#include <string>
#include <map>

struct  OBJ_SUPPORTER
{
	int				plugin_index;
	CBitmap32*      object_icon;
	OBJ_SUPPORTER()
	{
		plugin_index = -1;
		object_icon  = NULL;
	};
	OBJ_SUPPORTER(const OBJ_SUPPORTER &src)
	{
		plugin_index = src.plugin_index;
		object_icon  = src.object_icon;
	};
	OBJ_SUPPORTER &operator = (OBJ_SUPPORTER &src)
	{
		plugin_index = src.plugin_index;
		object_icon  = src.object_icon;
		return *this;
	}
} ;

class CPluginLoader
{
public:
	CPluginLoader();
	virtual ~CPluginLoader();

	void  LoadAllPlugins();
	
	std::vector<CToolbarPlugin*>   m_toolbar_plugins;

	std::map< std::string, OBJ_SUPPORTER>   m_objects_supporter;

protected:
	std::vector<CString>		   m_PluginsPathsArray; 
	CString						   m_application_Path; 

	void						   GetPlugInFiles(CString sPath);

	

	void  PluginLoader(CString pluginFile);

};
#endif