#ifndef __REG_MANAGER__
#define __REG_MANAGER__

typedef struct
{
	int   interface_theme;
	int   iCursorSize;
	bool  bIsCircle;
	int   iInColor;
	int   iOutColor;
} REGISTER_SETTINGS;

#define      COMPANY_NAME_FOR_REGISTER      _T("Genetibase")

class CNuGenDimensionApp;

class CRegisterManager
{
	CNuGenDimensionApp*   m_application;
public:
	REGISTER_SETTINGS m_register_settings;

  CRegisterManager(CNuGenDimensionApp* app);
  ~CRegisterManager();

  bool LoadSettings();
  bool SaveSettings();
 
};

#endif