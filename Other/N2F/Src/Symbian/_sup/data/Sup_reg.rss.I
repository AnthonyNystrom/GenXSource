# 1 "c:\\Projects\\_next2friends\\_sup\\data\\Sup_reg.rss"
/*
============================================================================
 Name        : Sup_reg.rss
 Author      : Vitaly Vinogradov
 Version     : 1.0.0
 Copyright   : (c) Next2Friends, 2008
 Description : Application registration resource file
============================================================================
*/

# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\appinfo.rh" 1
// APPINFO.RH
//
// Copyright (c) Symbian Software Ltd 2004-2005. All rights reserved.
//



//
// contains definitions for the Application Information resource files

/**
@publishedAll
@released
*/


//////////////////////////////////////////////////////////////////
// DEFINITIONS FOR THE NON-LOCALISABLE REGISTRATION RESOURCE FILE
//////////////////////////////////////////////////////////////////

/**
@publishedAll
@released
*/


/**
@publishedAll
@released
*/
// constants for capability.hidden



/**
@publishedAll
@released
*/
// constants for capability.newfile



/**
@publishedAll
@released
*/
// constants for capability.embeddability






/**
@publishedAll
@released
*/
// constants for capability.launch



/**
@publishedAll
@released
*/
// bit flags for capability.attributes




/**
@publishedAll
@released
*/
// application group name



/**
@publishedAll
@released
*/
STRUCT APP_REGISTRATION_INFO
	{
	LONG reserved_long = 0; // reserved for future use, do not use
	LLINK reserved_llink = 0; // reserved for future use, do not use
	//
	LTEXT app_file(256 ) = ""; // name of application binary file (not including extension) - mandatory
	LONG attributes = 0;
	//
	LTEXT localisable_resource_file(256 ) = ""; // path (not including drive) and name of localisable resource file
	LONG localisable_resource_id = 1;
	//
	BYTE hidden = 0 ;
	BYTE embeddability = 0 ;
	BYTE newfile = 0 ;
	BYTE launch = 0 ;
	LTEXT group_name(16 ) = "";
	//
	BYTE default_screen_number = 0;
	//
	LEN WORD STRUCT datatype_list[]; // DATATYPE
	//
	LEN WORD STRUCT file_ownership_list[]; // FILE_OWNERSHIP_INFO
	//
	LEN WORD STRUCT service_list[]; // SERVICE_INFO
	//
	LLINK opaque_data = 0;	// use for opaque data to send to non-native application launchers i.e. MIDlet id
	}


/**
@publishedAll
@released
*/


/**
@publishedAll
@released
*/
enum
	{
	EDataTypePrioritySystem = 0xFFF9,
	EDataTypePriorityHigh = 10000,
	EDataTypePriorityNormal = 0,
	EDataTypePriorityLow = -10000,
	EDataTypePriorityLastResort = -20000
	};

/**
@publishedAll
@released
*/
STRUCT DATATYPE
	{
	LONG priority;
	LTEXT8 type(256 );
	}

/**
@publishedAll
@released
*/
STRUCT FILE_OWNERSHIP_INFO
	{
	LTEXT file_name(256 );
	}

/**
@publishedAll
@released
*/
STRUCT SERVICE_INFO
	{
	LONG uid = 0;
	LEN WORD STRUCT datatype_list[];
	LLINK opaque_data = 0;
	}


/////////////////////////////////////////////////
// DEFINITIONS FOR THE LOCALISABLE RESOURCE FILE
/////////////////////////////////////////////////

/**
@publishedAll
@released
*/


/**
@publishedAll
@released
*/
STRUCT LOCALISABLE_APP_INFO
	{
	LONG reserved_long = 0; // reserved for future use, do not use
	LLINK reserved_llink = 0; // reserved for future use, do not use
	//
	LTEXT short_caption(256 ) = "";
	STRUCT caption_and_icon; // CAPTION_AND_ICON_INFO
	//
	LEN WORD STRUCT view_list[]; // VIEW_DATA
	//
	LTEXT group_name(16 ) = "";
	}

/**
@publishedAll
@released
*/
STRUCT CAPTION_AND_ICON_INFO
	{
	LONG reserved_long = 0; // reserved for future use, do not use
	LLINK reserved_llink = 0; // reserved for future use, do not use
	//
	LTEXT caption(256 ) = "";
	//
	WORD number_of_icons = 0; // each icon must be a bitmap/mask pair
	LTEXT icon_file(256 ) = "";
	}

/**
@publishedAll
@released
*/
STRUCT VIEW_DATA
	{
	LONG reserved_long = 0; // reserved for future use, do not use
	LLINK reserved_llink = 0; // reserved for future use, do not use
	//
	LONG uid = 0;
	//
	LONG screen_mode = 0;
	//
	STRUCT caption_and_icon; // CAPTION_AND_ICON_INFO
	}



# 11 "c:\\Projects\\_next2friends\\_sup\\data\\Sup_reg.rss" 2

# 1 "C:\\Projects\\_next2friends\\_sup\\inc\\Sup.rsg" 1















































# 12 "c:\\Projects\\_next2friends\\_sup\\data\\Sup_reg.rss" 2


UID2 0x101f8021 
UID3 0xeb83db8a

RESOURCE APP_REGISTRATION_INFO
	{
	app_file="Sup";
	localisable_resource_file = "\\resource\\apps\\Sup";
	localisable_resource_id = 0x277c4004 ;
	embeddability= 0 ;
	newfile= 0 ;
	}
