#pragma once

#include <configurationstorage.h>
#include <imagehelper.h>
#include <graphics_definitions.h>

class ConfigurationGraphics:
			public ConfigutationStorageBase
{
public:

	N2FCORE_API ConfigurationGraphics();
	N2FCORE_API virtual ~ConfigurationGraphics();

	N2FCORE_API bool GetImageKey(TGraphicsID id, IMAGE_KEY_TYPE& key);

protected:

	N2FCORE_API virtual void ClearStorage();
	N2FCORE_API virtual bool CreateConfigParser(XmlParserSimpleConfig **pParser);
	N2FCORE_API virtual void AddToStorage(SimpleConfigItem& item);

	CSimpleMap<TGraphicsID, IMAGE_KEY_TYPE>	iStoredGraphics;
};