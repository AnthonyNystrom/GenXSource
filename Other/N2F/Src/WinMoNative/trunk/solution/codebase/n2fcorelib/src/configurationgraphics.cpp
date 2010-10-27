#include "stdafx.h"


#include <configurationgraphics.h>


N2FCORE_API ConfigurationGraphics::ConfigurationGraphics()
	:ConfigutationStorageBase(ECSTGraphics)
{

}

N2FCORE_API ConfigurationGraphics::~ConfigurationGraphics()
{

}

N2FCORE_API  void ConfigurationGraphics::ClearStorage()
{
	for ( int i = 0; i < iStoredGraphics.GetSize(); ++i )
	{
		IMAGE_KEY_TYPE key = iStoredGraphics.GetValueAt(i);
		ImageHelper::GetInstance()->FreeImage(key);
	}

	iStoredGraphics.RemoveAll();
}

N2FCORE_API  bool ConfigurationGraphics::CreateConfigParser( XmlParserSimpleConfig **pParser )
{
	if ( NULL == pParser )
		return false;

	*pParser = XmlParserSimpleConfig::CreateGraphicsConfigParser();

	return ( NULL == *pParser );
}

N2FCORE_API  void ConfigurationGraphics::AddToStorage( SimpleConfigItem& item )
{
	TGraphicsID id = (TGraphicsID)(item.id);
	int idx = iStoredGraphics.FindKey(id);
	if ( -1 != idx )
	{
		// TODO: set-up an invalid image here!
		iStoredGraphics.GetValueAt(idx) = INVALID_IMAGE_KEY_VALUE;
	}
	else
	{
		CString fileToLoad = item.value;
		CString appPath;
		ControllerUtil::GetModuleFolder( appPath );
		CString filePath = appPath + _T("\\skins\\default\\");
		filePath += fileToLoad;

		LOGMSG("Graphics loading from: %s", filePath);

		IMAGE_KEY_TYPE key = INVALID_IMAGE_KEY_VALUE;
		//ImageHelper::GetInstance()->GetImageFromFile(fileToLoad, key);
		ImageHelper::GetInstance()->GetImageFromFile(filePath, key);

		ASSERT( INVALID_IMAGE_KEY_VALUE != key );
		if ( INVALID_IMAGE_KEY_VALUE != key )
			iStoredGraphics.Add(id, key);
	}
}

N2FCORE_API bool ConfigurationGraphics::GetImageKey( TGraphicsID id, IMAGE_KEY_TYPE& key )
{
	key = INVALID_IMAGE_KEY_VALUE;

	int idx = iStoredGraphics.FindKey(id);
	if ( -1 == idx )
		return false;

	key = iStoredGraphics.GetValueAt(idx);
	return true;
}
