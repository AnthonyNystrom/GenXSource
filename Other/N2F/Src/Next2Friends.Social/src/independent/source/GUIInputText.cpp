#include "GUIInputText.h"
#include "GUISystem.h"
#include "ResourceSystem.h"
#include "GUISkinLocal.h"
#include "Font.h"
#include "Application.h"
#include "GUIIndicator.h"
#include "GUICapsIndicator.h"


GUIInputText::GUIInputText(eEditTextType eeType/* = EETT_ALL*/)
	:	textBuffer		(NULL)
	,	textSize		(0)
	,	currentPos		(0)
	,	showCursor		(true)
	,	cursorCounter	(0)
	,	cursorWidth		(CURSOR_WIDTH)
	,	textChanged		(0)
{
	SetEditType(eeType);
	textEntered = false;
}

void GUIInputText::Update()
{
	UpdateCursorCounter();

	UpdateCharCounters();

	Application	 *pApp = GUISystem::Instance()->GetApp();
	if(pApp->IsKeyDown(Application::EKC_LEFT) || pApp->IsKeyRepeat(Application::EKC_LEFT))
	{
		if(currentPos && !unfocusedEnter)
		{
			MoveCursorOnPrev();
			isKeyDown			=	false;
		}
		else
		{
			GUISystem::Instance()->ProcessKey(Application::EKC_LEFT);
		}
	}
	if(pApp->IsKeyDown(Application::EKC_RIGHT) || pApp->IsKeyRepeat(Application::EKC_RIGHT))
	{
		if(currentPos < textSize && !unfocusedEnter)
		{
			MoveCursorOnNext();
			isKeyDown			=	false;
		}
		else
		{
			GUISystem::Instance()->ProcessKey(Application::EKC_RIGHT);
		}
	}
	if(pApp->IsKeyDown(Application::EKC_UP) || pApp->IsKeyRepeat(Application::EKC_UP))
	{
		bool ret = MoveCursorOnUp();
		if(ret && !unfocusedEnter)
		{
			isKeyDown			=	false;
		}
		else
		{
			GUISystem::Instance()->ProcessKey(Application::EKC_UP);
		}
	}
	if(pApp->IsKeyDown(Application::EKC_DOWN) || pApp->IsKeyRepeat(Application::EKC_DOWN))
	{
		bool ret = MoveCursorOnDown();
		if(ret && !unfocusedEnter)
		{
			isKeyDown			=	false;
		}
		else
		{
			GUISystem::Instance()->ProcessKey(Application::EKC_DOWN);
		}
	}
	if(pApp->IsKeyDown(Application::EKC_SELECT) || pApp->IsKeyRepeat(Application::EKC_SELECT))
	{
		GUISystem::Instance()->ProcessKey(Application::EKC_SELECT);
	}
	if(pApp->IsKeyDown(Application::EKC_CLR) || pApp->IsKeyRepeat(Application::EKC_CLR))
	{
		if(textSize && currentPos)
		{
			DeleteSymbol();
			isKeyDown			=	false;
		}
		else
		{
			UTILS_LOG(EDMP_DEBUG, "GUIInputText::Update: process CLR");
			GUISystem::Instance()->ProcessKey(Application::EKC_CLR);
		}
	}
	else
	{
		ProcessNumKeys();
	}
}

void GUIInputText::SetText(char16 *text, int32 textSz, int32 buffSz)
{
	textBuffer			=	text;
	textSize			=	textSz;
	bufferSize			=	buffSz;

	currentPos			=	textSz;

	textChanged			=	true;
	textEntered			=	true;
}

void GUIInputText::OnSetFocus()
{
	StopEditSymbol();
	isKeyDown	=	false;
	GUICapsIndicator *ind = (GUICapsIndicator*)GUISystem::Instance()->GetIndicator(GUIIndicator::EIT_CAPS);
	if (ind)
	{
		ind->Show();
		if (editType == EETT_DIGITS)
		{
			ind->SetMode(ECM_DIGITS);
		}
		else
		{
			ind->SetMode(ECM_FIRST_BIG);
		}
	}
}

void GUIInputText::OnLostFocus()
{
	if(isKeyDown)
	{
		MoveCursorOnNext();
		StopEditSymbol();
		isKeyDown	=	false;
	}
	GUIIndicator *ind = GUISystem::Instance()->GetIndicator(GUIIndicator::EIT_CAPS);
	if (ind)
	{
		ind->Hide();
	}
}

void GUIInputText::SetEditType(int32 eeType)
{
	editType = (eEditTextType)eeType;
	switch(editType)
	{
		case EETT_ALL:
			{
				for(int32 iKey = 0; iKey < KEYS_COUNT; ++iKey)
				{
					pKeys[iKey]	=	(char16 *)ALL_SYMBOLS[iKey];
				}
				break;
			}

		case EETT_LOGIN:	
			{
				for(int32 iKey = 0; iKey < KEYS_COUNT; ++iKey)
				{
					pKeys[iKey]	=	(char16 *)LOGIN_SYMBOLS[iKey];
				}
				break;
			}

		case EETT_DIGITS:	
			{
				for(int32 iKey = 0; iKey < KEYS_COUNT; ++iKey)
				{
					pKeys[iKey]	=	(char16 *)DIGIT_SYMBOLS[iKey];
				}
				break;
			}
	}
}

void GUIInputText::MoveCursorOnNext()
{
	if(currentPos >= textSize) return;

	++currentPos;

	StopEditSymbol();

	CalcCursorXY();
}

void GUIInputText::ProcessKey(int32 iKey)
{
	// change symbol for key
	if(iKey == curKey)
	{
		if(changeCharCounter++ >= CHANGE_CHAR_AGE)
		{
			// find next char for key
			if(0 == pKeys[iKey][++curSymbolInTable])
				curSymbolInTable = 0;

			changeCharCounter	=	0;
			applyCharTime	=	Utils::GetTime();
			textEntered			=	true;

		}
	}
	else
	{
		if(curKey != -1)
		{
			// move cursor on correct position
			MoveCursorOnNext();
		}

		// init new key/char
		StopEditSymbol();
		curKey					=	iKey;

		GUICapsIndicator *ind = (GUICapsIndicator*)GUISystem::Instance()->GetIndicator(GUIIndicator::EIT_CAPS);
		eCapsMode cm = ECM_FIRST_BIG;
		if (ind)
		{
			cm = ind->GetMode();
		}
		if(EETT_DIGITS	==	editType || cm == ECM_DIGITS)
		{
			applyCharTime = Utils::GetTime() - APPLY_CHAR_AGE;
		}
	}
}

void GUIInputText::UpdateEnteredText()
{
	if(		(textSize	>=	bufferSize-addChar) 
		||	(-1			==	curKey))	
	{
		isKeyDown = false;
		return;
	}

	// insert char or modify it
	GUICapsIndicator *ind = (GUICapsIndicator*)GUISystem::Instance()->GetIndicator(GUIIndicator::EIT_CAPS);
	eCapsMode cm = ECM_FIRST_BIG;
	if (ind)
	{
		cm = ind->GetMode();
	}
	if(addChar)
	{
		if (cm == ECM_DIGITS)
		{
			if(! AddChar(DIGIT_SYMBOLS[curKey][0]))
			{
				isKeyDown = false;
			}
		}
		else
		{
			if(! AddChar(pKeys[curKey][curSymbolInTable]))
			{
				isKeyDown = false;
			}
		}

		addChar = false;
	}
	else
	{
		if (cm == ECM_DIGITS)
		{
			ChangeChar(DIGIT_SYMBOLS[curKey][0]);
		}
		else
		{
			ChangeChar(pKeys[curKey][curSymbolInTable]);
		}
	}
}


bool GUIInputText::AddChar(char16 ch)
{
	if(textSize >= bufferSize - 1) 
	{
		UTILS_LOG(EDMP_ERROR, "GUIEditText::AddChar:  too long text");	
		return false;
	}

	// move part of text
	for(int32 iChar = textSize; iChar >= currentPos; --iChar)
	{
		textBuffer[iChar] = textBuffer[iChar-1];
	}
	// insert char
	ChangeChar(ch);

	// end of text
	++textSize;
	textBuffer[textSize] = 0;
	textEntered			=	true;

	return true;
}
void GUIInputText::ChangeChar(char16 ch)
{
	char16 newChar = GetChar(ch);

	// change char
	textBuffer[currentPos] = newChar;

	textChanged			=	true;
}

void GUIInputText::MoveCursorOnPrev()
{
	StopEditSymbol();

	if(currentPos)
	{
		--currentPos;
	}

	CalcCursorXY();
}

void GUIInputText::StopEditSymbol()
{
	applyCharTime	=	Utils::GetTime();
	curSymbolInTable	=	0;
	changeCharCounter	=	0;
	curKey				=	-1;
}

void GUIInputText::ProcessNumKeys()
{
	Application	 *pApp = GUISystem::Instance()->GetApp();
#ifndef	FULL_KEYBOARD_ENTER
	for(int32 iKey = 0 ; iKey <KEYS_COUNT; ++iKey)
	{
		if(		pApp->IsKeyDown((Application::eKeyCode)(Application::EKC_0 + iKey))
			||	pApp->IsKeyRepeat((Application::eKeyCode)(Application::EKC_0 + iKey))) 
		{
			if(	KEY_STAR	==	iKey )
			{
				if (editType != EETT_DIGITS)
				{
					GUICapsIndicator *ind = (GUICapsIndicator*)GUISystem::Instance()->GetIndicator(GUIIndicator::EIT_CAPS);
					if (ind)
					{
						ind->SetNextMode();
					}
				}
				continue;
			}

			// new key was pressed
			if(curKey != iKey)
			{
				addChar = true;
			}

			ProcessKey(iKey);
			isKeyDown = true;
		}
		else
		{
			if(curKey == iKey)
				changeCharCounter	=	CHANGE_CHAR_AGE;
		}
	}
	if(isKeyDown)
		UpdateEnteredText();
#endif
}


void GUIInputText::DeleteSymbol()
{
	if(isKeyDown)
	{
		isKeyDown = false;
		MoveCursorOnNext();
	}
	
	if(currentPos)
	{
		// move part of text
		for(int32 iChar = currentPos; iChar < textSize; ++iChar)
		{
			textBuffer[iChar-1]	= textBuffer[iChar];
		}

		--currentPos;
		--textSize;
		textBuffer[textSize]		=	0;

		CalcCursorXY();

		textChanged	=	true;
		textEntered =	true;
	}
}

void GUIInputText::UpdateCharCounters()
{
	if(isKeyDown)
	{
		// if time is out - move cursor on the next position
		if(Utils::GetTime() - applyCharTime >= APPLY_CHAR_AGE)
		{
			StopEditSymbol();
			MoveCursorOnNext();
		}
	}
}

void GUIInputText::SetCursorWidth(int32 cWidth)
{
	cursorWidth		=	cWidth;
}

char16 GUIInputText::GetChar(char16 ch)
{
	char16 newChar	=	ch;
	GUICapsIndicator *ind = (GUICapsIndicator*)GUISystem::Instance()->GetIndicator(GUIIndicator::EIT_CAPS);
	eCapsMode cm = ECM_FIRST_BIG;
	if (ind)
	{
		cm = ind->GetMode();
	}
	if(	EETT_ALL	==	editType  && cm != ECM_DIGITS)
	{
		if(SYMBOL('a') <= ch && ch <= SYMBOL('z'))
		{

			bool upperCase	=	false;
			if (cm == ECM_FIRST_BIG)
			{
				if(!currentPos)
				{
					upperCase	=	true;
				}
				else
				{
					int32 size	=	currentPos	-	1;
					while(size >= 0)
					{
						char16	oldChar	= textBuffer[size];
						if(SYMBOL('.')	==	oldChar)
						{
							upperCase	=	true;
							break;
						}
						else if(SYMBOL(' ') == oldChar)
						{
							--size;
							continue;
						}

						break;
					}
				}
			}
			if (cm == ECM_BIG)
			{
				upperCase = true;
			}

			if(upperCase)
			{
				newChar	= ch - SYMBOL('a') + SYMBOL('A');
			}
		}
	}

    return newChar;
}

void GUIInputText::UpdateCursorCounter()
{
	++cursorCounter;
	if(CURSOR_SHOW_AGE	<=	cursorCounter)
	{
		cursorCounter	=	0;
		showCursor		=	!showCursor;
	}
}

bool GUIInputText::IsEmpty()
{
	return (0	==	textSize);
}

bool GUIInputText::InsertText( const char16 *textToInsert )
{
	int32 tl = Utils::WStrLen(textToInsert);

	if(textSize + tl >= bufferSize - 1) 
	{
		UTILS_LOG(EDMP_ERROR, "GUIEditText::AddChar:  too long text");	
		return false;
	}

	// move part of text
	for(int32 iChar = textSize + tl; iChar >= currentPos + tl; --iChar)
	{
		textBuffer[iChar] = textBuffer[iChar-tl];
	}

	for(int32 iChar = currentPos; iChar < currentPos + tl; iChar++)
	{
		textBuffer[iChar] = textToInsert[iChar - currentPos];
	}

	// end of text
	textSize += tl;
	currentPos += tl;

	textChanged			=	true;
	textEntered			=	true;

	return true;
}

void GUIInputText::OnChar( char16 ch )
{
#ifdef	FULL_KEYBOARD_ENTER

	if (ch < 0x20 || ch > 0x7D)
	{
		return;
	}
	if (editType == EETT_DIGITS && (ch < L'0' || ch > L'9') && (ch != L'+' && ch != L'-' && ch != L' '))
	{
		return;
	}

	UTILS_LOG(EDMP_WARNING, "ADD_CHAR: %d (0x%X) mapped as '%C'", ch, ch, ch);
	addChar = true;
	StopEditSymbol();

	AddChar(ch);

	MoveCursorOnNext();
	applyCharTime = Utils::GetTime() - APPLY_CHAR_AGE;
	return;
#endif
}



