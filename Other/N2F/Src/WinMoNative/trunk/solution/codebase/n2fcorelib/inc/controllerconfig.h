#pragma once

#include <configurationstrings.h>
#include <configurationgraphics.h>
#include <configurationcolors.h>



typedef CSimpleMap<TConfigStorageType, ConfigutationStorageBase*> ConfigurationStoragesList;
typedef CSimpleMap<TConfigStorageType, CString> ConfigurationStoragesInitList;

class ControllerConfig
{
public:

	N2FCORE_API ControllerConfig();
	N2FCORE_API virtual ~ControllerConfig();

	N2FCORE_API bool Initialize( ConfigurationStoragesInitList& listInit );

	N2FCORE_API CString String( TStringID id );
	N2FCORE_API IMAGE_KEY_TYPE ImageKey( TGraphicsID id );
	N2FCORE_API COLORREF Color( TColorID id );

private:

	N2FCORE_API void ClearStorages();

	N2FCORE_API ConfigurationStrings* GetStringsStorage();
	N2FCORE_API ConfigurationGraphics* GetGraphicsStorage();
	N2FCORE_API ConfigurationColors* GetColorsStorage();

	ConfigurationStoragesList	iStoragesList;
};
