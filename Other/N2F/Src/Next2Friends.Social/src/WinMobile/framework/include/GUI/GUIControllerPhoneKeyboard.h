//// =================================================================
/*!	\file GUIControllerPhoneKeyboard.h

	Revision History:

	[8.11.2007] 15:18 by Alexey 'Arrow' Prosin
	\add Now controller sends key up/key down global events to 
	guiSystem (only if key event is not handled by focused control)
	
	\par [9.8.2007]	13:41 by Ivan Petrochenko
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK__GUIPHONEKEYBOARDCONTROLLER_H__
#define __FRAMEWORK__GUIPHONEKEYBOARDCONTROLLER_H__

#include "GUIController.h"
#include "List.h"
#include "GUIConsts.h"
#include "Utils.h"
#include "Application.h"
#include "GUIEventData.h"

class GUISystem;

/*! \brief Implementation of GUIController corresponding to the phone keyboard.
*/	


class GUIControllerPhoneKeyboard : public GUIController
{
#ifndef DEBUG
	public:
#endif

	struct ControllerCharData
	{
		Application::eKeyCode	keyCode;
		List<char16>			keyChar;
	};

	struct ControllerCharset
	{
		List<ControllerCharData*> chars;
	};

public:
	//  ***************************************************
	//! \brief		Constructor.    	
	//! \param[in]	guiSystem - pointer on GUISystem.
	//  ***************************************************
	GUIControllerPhoneKeyboard(GUISystem * guiSystem) 
	:	GUIController(guiSystem)
	,	focused(NULL)
	,	tempList(EGC_PHONE_KEYBOARD_CONTROLLER_TEMP_LIST_RESERVE)
	{
		keyLastTime = Utils::GetTime();
		keyLastCode = 0;
		charEnterTime = 1000;
		currentCharsetNumber = 0;
		InitKeyboard();
	}

	virtual ~GUIControllerPhoneKeyboard();

	// *************************************************************
	//! \brief    	Update cycle implementation.
	// *************************************************************
	virtual void	Update();

	// *************************************************************
	//! \brief    	Render cycle implementation.
	// *************************************************************
	virtual void	Render();

	// *************************************************************
	//! \brief    	��������� ����� ������
	//! \return   	������ ������ �������
	// *************************************************************
	int32 AddCharset();

	// *************************************************************
	//! \brief    	�������� �������� ������
	//! \param[in]  newCharset - ������ ������� ����� ���������� ������� ��������
	//! \return   	������ ������� �������
	// *************************************************************
	int32 SetCharset(int32 newCharset);

	// *************************************************************
	//! \brief    	��������� ����� ������ � ��������� ������ ��� ��������� �������
	//! \param[in]  charsetNumber - ������ ������� ��� �������� ��������������� ����� ������
	//! \param[in]  code - ��� ������� ��� ������� ��������������� ������
	//! \param[in]  charToAdd - ��������������� ������
	// *************************************************************
	void AddChar(int32 charsetNumber, Application::eKeyCode code, char16 charToAdd);


private:
	enum { PHONE_KEY_COUNT = 12 };

	typedef			bool (GUIControllerPhoneKeyboard::*SignFunc)(int32 op1, int32 op2);	

	GUIControl * FindActiveParent(GUIControl * _control);

	void			SetFocus(GUIControl * newFocus);
	void			GenerateTempList(GUIControl *pControl);

	void			InitKeyboard();
	void			ProcessKeyboard();
	GUIEventData	GetKeyEventData(uint32 code, bool isKeyUp);
	
	GUIControl			* focused;
	int32				x;
	int32				y;



	List<ControllerCharset*>	charsets;
	ControllerCharset*			currentCharset;
	int32						defCharsetNumber;
	int32						currentCharsetNumber;
	List<char16>::Iterator		keyCounter;
	uint32						keyLastTime;
	uint32						keyLastCode;
	uint32						charEnterTime;

	List<GUIControl*>	tempList;
};

#endif // __FRAMEWORK_GUIPHONEKEYBOARDCONTROLLER_H__