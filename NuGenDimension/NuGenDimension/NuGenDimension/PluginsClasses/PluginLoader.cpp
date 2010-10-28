#include "stdafx.h"
#include <shlwapi.h>

#include "..//NuGenDimension.h"

#include "PluginLoader.h"

CPluginLoader::CPluginLoader()
{
	HMODULE h = ::GetModuleHandle( NULL );
	TCHAR szPath[MAX_PATH]; 
	GetModuleFileName( h, szPath, MAX_PATH );
	PathRemoveFileSpec(szPath);

	CString tmpStr(szPath);
	tmpStr += "\\";

	m_application_Path = tmpStr;
}

CPluginLoader::~CPluginLoader()
{
	for (size_t i=0, sz=m_toolbar_plugins.size();i<sz;i++)
		delete m_toolbar_plugins[i];
}

void  CPluginLoader::LoadAllPlugins()
{
	GetPlugInFiles(""); 
	size_t i,sz;
	for (i=0,sz=m_PluginsPathsArray.size();i<sz;i++)
	{
		PluginLoader(m_PluginsPathsArray[i]);
	}

	for (i=0,sz=m_toolbar_plugins.size();i<sz;i++)
	{
		m_toolbar_plugins[i]->LoadSupportedIDs();
		m_toolbar_plugins[i]->LoadObjectsBitmaps();
		size_t sz_1=m_toolbar_plugins[i]->m_supported_objectsIDs.size();
		for (size_t j=0;j<sz_1;j++)
		{
			OBJ_SUPPORTER tmpOS;
			tmpOS.plugin_index = i;
			tmpOS.object_icon = m_toolbar_plugins[i]->m_object_bitmaps[j];
			std::string tmpSt(m_toolbar_plugins[i]->m_supported_objectsIDs[j]);
			m_objects_supporter[tmpSt] = tmpOS;
		}
	}
}

void CPluginLoader::GetPlugInFiles(CString sPath)
{
	if (m_PluginsPathsArray.size()>=512) 
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
				GetPlugInFiles(sStr);
			}
			else 
			{
				char *ptr=strrchr(FindData.cFileName,'.');
				if (ptr) 
				{ 
					if (strlen(ptr)==4) 
					{
						if ((ptr[1]=='p' && ptr[2]=='p' && ptr[3]=='l') ||
							(ptr[1]=='P' && ptr[2]=='P' && ptr[3]=='L'))   //проверка расширения
						{
							CString sPath1=sPath;
							sPath1+=FindData.cFileName; 
							m_PluginsPathsArray.push_back(sPath1); 
						}
					} 
				}
			}
		}
		if (!FindNextFile(hFindFiles,&FindData)) break; 
	}
	FindClose(hFindFiles);
}

void CPluginLoader::PluginLoader(CString pluginFile)
{
	typedef void(*LPDLLFUNC_GETINFO)(PLUGIN_INFO*);
	LPDLLFUNC_GETINFO lpfnDllFuncGetInfo = NULL;
	HINSTANCE hDLL = NULL;     	  
	hDLL = AfxLoadLibrary(pluginFile);
	if(hDLL)
	{
		lpfnDllFuncGetInfo = (LPDLLFUNC_GETINFO)::GetProcAddress(hDLL,"GetPluginInfo");
		if (!lpfnDllFuncGetInfo)
		{
			AfxFreeLibrary(hDLL);
			return;
		}
		PLUGIN_INFO plInf;
		lpfnDllFuncGetInfo(&plInf);
		if (plInf.plugin_type==PLUGIN_TOOLBAR)
		{
			CToolbarPlugin* plT = new CToolbarPlugin(hDLL,plInf);
			m_toolbar_plugins.push_back(plT);
			//AddToolbarPlugin(plT);
		}
		else
		{
			AfxFreeLibrary(hDLL);
			return;
		}
	}
	else
	{
		//AfxMessageBox("Dll not found!");
	}

}