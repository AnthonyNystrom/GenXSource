#pragma once

#include <xmlparsersimpleconfig.h>

enum TConfigStorageType
{
	ECSTStrings = 0,
	ECSTGraphics,
	ECSTColors
};

class ConfigutationStorageBase
{
public:

	N2FCORE_API ConfigutationStorageBase(TConfigStorageType type);
	N2FCORE_API virtual ~ConfigutationStorageBase();

	N2FCORE_API virtual bool LoadFromFile( CString& fileName );

	N2FCORE_API virtual void ReleaseStorage();

	N2FCORE_API TConfigStorageType Type();

	static N2FCORE_API ConfigutationStorageBase* CreateConfigurationStorage(TConfigStorageType type);

protected:

	N2FCORE_API virtual void ClearStorage() = 0;
	N2FCORE_API virtual bool CreateConfigParser(XmlParserSimpleConfig **pParser) = 0;
	N2FCORE_API virtual void AddToStorage(SimpleConfigItem& item) = 0;

private:

	TConfigStorageType		iType;

};

