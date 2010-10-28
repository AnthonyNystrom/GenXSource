#include "stdafx.h"
#include "RegManager.h"

#include "..//NuGenDimension.h"

CRegisterManager::CRegisterManager(CNuGenDimensionApp* app):
						m_application(app)
{
	ASSERT(app);
	m_register_settings.interface_theme = CEGMenu::STYLE_XP_2003;
}

CRegisterManager::~CRegisterManager()
{
}

#define   SETTINGS_KEY                 _T("Settings")
#define   INTERFACE_THEME_VALUE        _T("InterfaceTheme")
#define   INTERFACE_CURSOR_TYPE        _T("CursorType")
#define   INTERFACE_CURSOR_SIZE        _T("CursorSize")
#define   INTERFACE_CURSOR_INCOLOR     _T("CursorInColor")
#define   INTERFACE_CURSOR_OUTCOLOR    _T("CursorOutColor")

bool CRegisterManager::LoadSettings()
{
	m_register_settings.interface_theme = m_application->GetProfileInt(SETTINGS_KEY, INTERFACE_THEME_VALUE, CEGMenu::STYLE_XP);
	if ((m_application->GetProfileInt(SETTINGS_KEY, INTERFACE_CURSOR_TYPE, 1))==0)
		m_register_settings.bIsCircle=false;
	else
		m_register_settings.bIsCircle=true;
	m_register_settings.iCursorSize  = m_application->GetProfileInt(SETTINGS_KEY, INTERFACE_CURSOR_SIZE, 4);
	m_register_settings.iInColor  = m_application->GetProfileInt(SETTINGS_KEY, INTERFACE_CURSOR_INCOLOR, 0);
	m_register_settings.iOutColor = m_application->GetProfileInt(SETTINGS_KEY, INTERFACE_CURSOR_OUTCOLOR, 10);
	return true;
}

bool CRegisterManager::SaveSettings()
{
	int iVal;
	if(m_register_settings.bIsCircle)
		iVal=1;
	else
		iVal=0;
	m_application->WriteProfileInt(SETTINGS_KEY, INTERFACE_THEME_VALUE, m_register_settings.interface_theme);
	m_application->WriteProfileInt(SETTINGS_KEY, INTERFACE_CURSOR_TYPE, iVal);
	m_application->WriteProfileInt(SETTINGS_KEY, INTERFACE_CURSOR_SIZE, m_register_settings.iCursorSize  );
	m_application->WriteProfileInt(SETTINGS_KEY, INTERFACE_CURSOR_INCOLOR, m_register_settings.iInColor  );
	m_application->WriteProfileInt(SETTINGS_KEY, INTERFACE_CURSOR_OUTCOLOR, m_register_settings.iOutColor  );
	return true;
}
