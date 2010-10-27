#pragma once

enum TSimpleConfigItemType
{
	ESCTUnknown = 0,
	ESCTString,
	ESCTGraphics,
	ESCTColor
};


struct SimpleConfigItem
{
	SimpleConfigItem()
	{
		type = ESCTUnknown;
		id = 0;
	}

	TSimpleConfigItemType	type;
	long					id;
	CString					name;
	CString					value;
};

typedef	CSimpleArray<SimpleConfigItem*>	SimpleConfigItemsList;
