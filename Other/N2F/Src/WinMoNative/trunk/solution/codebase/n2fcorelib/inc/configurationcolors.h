#pragma once

#include <configurationstorage.h>
#include <colors_definitions.h>

class ConfigurationColors:
			public ConfigutationStorageBase
{
public:

	N2FCORE_API ConfigurationColors();
	N2FCORE_API virtual ~ConfigurationColors();

	N2FCORE_API COLORREF GetColor( TColorID id );

private:

	N2FCORE_API virtual void ClearStorage();
	N2FCORE_API virtual bool CreateConfigParser(XmlParserSimpleConfig **pParser);
	N2FCORE_API virtual void AddToStorage(SimpleConfigItem& item);

	N2FCORE_API COLORREF StringToColor( CString& value );
	N2FCORE_API COLORREF GetNotFoundValue();

	CSimpleMap<TColorID, COLORREF>	iStoredColors;
};
