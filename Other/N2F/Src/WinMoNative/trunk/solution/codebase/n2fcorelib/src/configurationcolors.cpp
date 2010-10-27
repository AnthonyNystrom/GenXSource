#include "stdafx.h"
#include <configurationcolors.h>


N2FCORE_API ConfigurationColors::ConfigurationColors()
	:ConfigutationStorageBase(ECSTColors)
{

}

N2FCORE_API ConfigurationColors::~ConfigurationColors()
{

}

N2FCORE_API  void ConfigurationColors::ClearStorage()
{
	iStoredColors.RemoveAll();
}

N2FCORE_API  bool ConfigurationColors::CreateConfigParser( XmlParserSimpleConfig **pParser )
{
	if ( NULL == pParser )
		return false;

	*pParser = XmlParserSimpleConfig::CreateColorConfigParser();

	return ( NULL != *pParser );
}

N2FCORE_API  void ConfigurationColors::AddToStorage( SimpleConfigItem& item )
{
	TColorID id = (TColorID)(item.id);
	int idx = iStoredColors.FindKey( id );
	if ( -1 == id )
	{
		ASSERT(FALSE);
		// color already exists - ignoring
	}
	else
	{
		LOGMSG("Loading color %s with value: %s", item.name, item.value);
		COLORREF color = this->StringToColor(item.value);
		iStoredColors.Add( id, color );
	}
}

N2FCORE_API COLORREF ConfigurationColors::GetNotFoundValue()
{
	COLORREF color = RGB(0, 0xff, 0);

	int idx = iStoredColors.FindKey( ECNotFound );
	if ( -1 != idx )
		color = iStoredColors.GetValueAt( idx );

	return color;
}

N2FCORE_API COLORREF ConfigurationColors::StringToColor( CString& value )
{
	COLORREF color = this->GetNotFoundValue();
	if ( value.GetLength() == 0 )
	{
		ASSERT(FALSE);
		return color;
	}

	int red = 0, green = 0, blue = 0;



	_stscanf( value, _T("[%x][%x][%x]"), &red, &green, &blue );

	LOGMSG("Color parsed to (RGB): %x %x %x", red, green, blue);

	color = RGB( red, green, blue );

	return color;
}

N2FCORE_API COLORREF ConfigurationColors::GetColor( TColorID id )
{
	COLORREF color = this->GetNotFoundValue();

	int idx = iStoredColors.FindKey( id );
	if ( -1 != idx )
	{
		color = iStoredColors.GetValueAt( idx );
	}

	return color;
}


