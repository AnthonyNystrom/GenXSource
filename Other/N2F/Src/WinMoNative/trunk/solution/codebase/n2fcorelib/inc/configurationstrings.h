#pragma once

#include <configurationstorage.h>
#include <strings_definitions.h>

class ConfigurationStrings:
			public ConfigutationStorageBase
{
public:

	N2FCORE_API ConfigurationStrings();
	N2FCORE_API virtual ~ConfigurationStrings();

	N2FCORE_API bool GetString( TStringID id, CString& string );

private:

	N2FCORE_API virtual void ClearStorage();
	N2FCORE_API virtual bool CreateConfigParser(XmlParserSimpleConfig **pParser);
	N2FCORE_API virtual void AddToStorage(SimpleConfigItem& item);

	CSimpleMap<TStringID, CString>	iStoredStrings;

};
