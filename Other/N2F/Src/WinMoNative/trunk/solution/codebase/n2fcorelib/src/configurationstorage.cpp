#include "stdafx.h"

#include <configurationstorage.h>
#include <xmlparserwrapper.h>

#include <configurationstrings.h>
#include <configurationgraphics.h>
#include <configurationcolors.h>

N2FCORE_API ConfigutationStorageBase::ConfigutationStorageBase(TConfigStorageType type)
	:iType(type)
{

}

N2FCORE_API ConfigutationStorageBase::~ConfigutationStorageBase()
{
	
}

N2FCORE_API bool ConfigutationStorageBase::LoadFromFile( CString& fileName )
{
	this->ClearStorage();

	if ( 0 == fileName.GetLength() )
		return false;

	XmlParserSimpleConfig *configParser = NULL;
	XmlParserWrapper *xmlParser = new XmlParserWrapper;
	if ( NULL == xmlParser )
		return false;

	bool result = false;

	if ( true == xmlParser->Initialize() )
	{
		
		if ( true == this->CreateConfigParser(&configParser) 
			|| NULL != configParser )
		{
			result = xmlParser->ParseXmlFile(fileName, configParser);
		}
	}

	if ( true == result )
	{
		SimpleConfigItemsList list = configParser->GetResultsList();

		for ( int i = 0; i < list.GetSize(); ++i )
		{
			SimpleConfigItem *pItem = list[i];
			this->AddToStorage(*pItem);

			delete pItem;
			list[i] = NULL;
		}
	}

	delete xmlParser;

	if ( NULL != configParser )
		delete configParser;

	return result;

}

N2FCORE_API void ConfigutationStorageBase::ReleaseStorage()
{
	this->ClearStorage();
}

N2FCORE_API TConfigStorageType ConfigutationStorageBase::Type()
{
	return iType;
}

N2FCORE_API  ConfigutationStorageBase* ConfigutationStorageBase::CreateConfigurationStorage( TConfigStorageType type )
{
	ConfigutationStorageBase *result = NULL;
	if ( ECSTStrings == type )
	{
		result = new ConfigurationStrings;
	}
	else if ( ECSTGraphics == type )
	{
		result = new ConfigurationGraphics;
	}
	else if ( ECSTColors == type )
	{
		result = new ConfigurationColors;
	}
	else
	{
		// should be added implementation for all supported configuration storage types
		ASSERT(FALSE);
	}

	ASSERT( NULL != result );

	return result;
}
