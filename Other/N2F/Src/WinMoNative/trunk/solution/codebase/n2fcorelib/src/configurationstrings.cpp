#include "stdafx.h"

#include <configurationstrings.h>

N2FCORE_API ConfigurationStrings::ConfigurationStrings()
	:ConfigutationStorageBase(ECSTStrings)
{

}

N2FCORE_API ConfigurationStrings::~ConfigurationStrings()
{

}

N2FCORE_API void ConfigurationStrings::ClearStorage()
{
	iStoredStrings.RemoveAll();
}

N2FCORE_API bool ConfigurationStrings::CreateConfigParser( XmlParserSimpleConfig **pParser )
{
	if ( NULL == pParser )
		return false;

	*pParser = XmlParserSimpleConfig::CreateStringConfigParser();

	return ( NULL == *pParser );

}

N2FCORE_API void ConfigurationStrings::AddToStorage( SimpleConfigItem& item )
{
	TStringID id = (TStringID)(item.id);
	int idx = iStoredStrings.FindKey(id);
	if ( -1 != idx )
	{
		ASSERT(FALSE);
		CString error;
		error.Format(_T("value with id %d is duplicated"), id);
	
		iStoredStrings.GetValueAt(idx) = error;
	}
	else
	{
		iStoredStrings.Add(id, item.value);
	}
}

N2FCORE_API bool ConfigurationStrings::GetString( TStringID id, CString& string )
{
	string.Empty();

	int idx = iStoredStrings.FindKey(id);
	if ( -1 == idx )
		return false;

	string = iStoredStrings.GetValueAt(idx);
	return true;
}
