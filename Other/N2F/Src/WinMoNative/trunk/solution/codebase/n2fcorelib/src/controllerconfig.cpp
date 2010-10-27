#include "stdafx.h"

#include <controllerconfig.h>


N2FCORE_API ControllerConfig::ControllerConfig()
{
	
}

N2FCORE_API ControllerConfig::~ControllerConfig()
{
	this->ClearStorages();
}

N2FCORE_API bool ControllerConfig::Initialize( ConfigurationStoragesInitList& listInit )
{
	this->ClearStorages();

	bool result = true;

	for ( int i = 0; i < listInit.GetSize(); ++i )
	{
		TConfigStorageType type = listInit.GetKeyAt(i);
		CString filePath = listInit.GetValueAt(i);

		int idx = iStoragesList.FindKey(type);
		if ( -1 != idx )
		{
			ASSERT(FALSE);
			result = false;
			continue;
		}

		ConfigutationStorageBase *pStorage = ConfigutationStorageBase::CreateConfigurationStorage(type);
		if ( NULL == pStorage )
		{
			ASSERT(FALSE);
			result = false;
			continue;
		}

		if ( false == pStorage->LoadFromFile( filePath ) )
		{
			ASSERT(FALSE);
			result = false;
			delete pStorage;

			continue;
		}

		iStoragesList.Add(type, pStorage);
	}

	return result;
}

N2FCORE_API void ControllerConfig::ClearStorages()
{
	for ( int i = 0; i < iStoragesList.GetSize(); ++i )
	{
		ConfigutationStorageBase *pStorage = iStoragesList.GetValueAt(i);
		if ( NULL == pStorage )
			continue;

		pStorage->ReleaseStorage();
		delete pStorage;
	}

	iStoragesList.RemoveAll();
}

N2FCORE_API CString ControllerConfig::String( TStringID id )
{
	CString result = _T("InvalidString");

	ConfigurationStrings *storage = this->GetStringsStorage();
	if ( NULL != storage )
	{
		storage->GetString( id, result );
	}

	return result;
}

N2FCORE_API IMAGE_KEY_TYPE ControllerConfig::ImageKey( TGraphicsID id )
{
	IMAGE_KEY_TYPE key = INVALID_IMAGE_KEY_VALUE;

	ConfigurationGraphics *storage = this->GetGraphicsStorage();
	if ( NULL != storage )
	{
		storage->GetImageKey( id, key );
	}

	return key;
}

N2FCORE_API COLORREF ControllerConfig::Color( TColorID id )
{
	COLORREF color;
	ConfigurationColors *storage = this->GetColorsStorage();
	if ( NULL != storage )
	{
		color = storage->GetColor( id );
	}

	return color;
}

N2FCORE_API ConfigurationStrings* ControllerConfig::GetStringsStorage()
{
	ConfigurationStrings *result = NULL;

	int idx = iStoragesList.FindKey( ECSTStrings );
	if ( -1 != idx )
		result = (ConfigurationStrings*)(iStoragesList.GetValueAt(idx));
	else
		ASSERT(FALSE);

	return result;
}

N2FCORE_API ConfigurationGraphics* ControllerConfig::GetGraphicsStorage()
{
	ConfigurationGraphics *result = NULL;

	int idx = iStoragesList.FindKey( ECSTGraphics );
	if ( -1 != idx )
		result = (ConfigurationGraphics*)(iStoragesList.GetValueAt(idx));
	else
		ASSERT(FALSE);

	return result;
}

N2FCORE_API ConfigurationColors* ControllerConfig::GetColorsStorage()
{
	ConfigurationColors *result = NULL;
	
	int idx = iStoragesList.FindKey( ECSTColors );
	if ( -1 != idx )
		result = (ConfigurationColors*)(iStoragesList.GetValueAt(idx));
	else
		ASSERT(FALSE);

	return result;
}


