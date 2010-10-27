#ifndef __GUI_INPUT_TEXT_
#define __GUI_INPUT_TEXT_

#include "GUITypes.h"
#include "BaseTypes.h"

#define SYMBOL(ch) L##ch

static const char16 ALL_SYMBOLS	[12][17]	=	
{
	//	0				1				2				3				4				5				6				7				8				9				10				11				12				13				14				15				16
	/*0*/
	{	SYMBOL('+'),	SYMBOL('0'),	0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0	},
		/*1*/
	{	SYMBOL('.'),	SYMBOL(','),	SYMBOL('-'),	SYMBOL('?'),	SYMBOL('!'),	SYMBOL('\''),	SYMBOL('@'),	SYMBOL(':'),	SYMBOL(';'),	SYMBOL('/'),	SYMBOL('('),	SYMBOL(')'),	SYMBOL('%'),	SYMBOL('$'),	SYMBOL('='),	SYMBOL('*'),	SYMBOL('1')	},
	/*2*/
	{	SYMBOL('a'),	SYMBOL('b'),	SYMBOL('c'),	SYMBOL('2'),	0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0	},
	/*3*/
	{	SYMBOL('d'),	SYMBOL('e'),	SYMBOL('f'),	SYMBOL('3'),	0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0	},
	/*4*/
	{	SYMBOL('g'),	SYMBOL('h'),	SYMBOL('i'),	SYMBOL('4'),	0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0	},
	/*5*/
	{	SYMBOL('j'),	SYMBOL('k'),	SYMBOL('l'),	SYMBOL('5'),	0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0	},
	/*6*/
	{	SYMBOL('m'),	SYMBOL('n'),	SYMBOL('o'),	SYMBOL('6'),	0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0	},
	/*7*/
	{	SYMBOL('p'),	SYMBOL('q'),	SYMBOL('r'),	SYMBOL('s'),	SYMBOL('7'),	0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0	},
	/*8*/	
	{	SYMBOL('t'),	SYMBOL('u'),	SYMBOL('v'),	SYMBOL('8'),	0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0	},
	/*9*/
	{	SYMBOL('w'),	SYMBOL('x'),	SYMBOL('y'),	SYMBOL('z'),	SYMBOL('9'),	0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0	},
	/***/
	{	0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0	},
	/*#*/	
	{	SYMBOL(' '),	SYMBOL('#'),	0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0,				0	}
};


static const char16 DIGIT_SYMBOLS [12][2]	=	
{
	//			0		1
	/*0*/
	{	SYMBOL('0'),	0},
		/*1*/
	{	SYMBOL('1'),	0},
	/*2*/
	{	SYMBOL('2'),	0},
	/*3*/
	{	SYMBOL('3'),	0},
	/*4*/
	{	SYMBOL('4'),	0},
	/*5*/
	{	SYMBOL('5'),	0},
	/*6*/
	{	SYMBOL('6'),	0},
	/*7*/
	{	SYMBOL('7'),	0},
	/*8*/	
	{	SYMBOL('8'),	0},
	/*9*/
	{	SYMBOL('9'),	0},
	/***/
	{	0,				0},
	/*#*/	
	{	SYMBOL(' '),	0}
};



static const char16 LOGIN_SYMBOLS	[12][6]	=	
{
	//	0				1				2				3				4				5
	/*0*/
	{	SYMBOL('0'),	0,				0,				0,				0,				0	},
		/*1*/
	{	SYMBOL('1'),	0,				0,				0,				0,				0	},
	/*2*/
	{	SYMBOL('a'),	SYMBOL('b'),	SYMBOL('c'),	SYMBOL('2'),	0,				0	},
	/*3*/
	{	SYMBOL('d'),	SYMBOL('e'),	SYMBOL('f'),	SYMBOL('3'),	0,				0	},
	/*4*/
	{	SYMBOL('g'),	SYMBOL('h'),	SYMBOL('i'),	SYMBOL('4'),	0,				0	},
	/*5*/
	{	SYMBOL('j'),	SYMBOL('k'),	SYMBOL('l'),	SYMBOL('5'),	0,				0	},
	/*6*/
	{	SYMBOL('m'),	SYMBOL('n'),	SYMBOL('o'),	SYMBOL('6'),	0,				0	},
	/*7*/
	{	SYMBOL('p'),	SYMBOL('q'),	SYMBOL('r'),	SYMBOL('s'),	SYMBOL('7'),	0	},
	/*8*/	
	{	SYMBOL('t'),	SYMBOL('u'),	SYMBOL('v'),	SYMBOL('8'),	0,				0	},
	/*9*/
	{	SYMBOL('w'),	SYMBOL('x'),	SYMBOL('y'),	SYMBOL('z'),	SYMBOL('9'),	0	},
	/***/
	{	0,				0,				0,				0,				0,				0	},
	/*#*/	
	{	0,				0,				0,				0,				0,				0	}
};


class GUIInputText 
{
protected:
	enum eConst
	{
		MAX_SIZE				=	256,

		KEY_STAR				=	10,
		KEY_POUND				=	11,

		// key process
		KEYS_COUNT				=	12,
		CHANGE_CHAR_AGE			=	5,
		APPLY_CHAR_AGE			=	1000,//time in MS
		CURSOR_SHOW_AGE			=	3,

		CURSOR_WIDTH			=	1
	};

	GUIInputText(eEditTextType eeType = EETT_ALL);

public:
	
	virtual ~GUIInputText()	{}
	
	virtual void Update();

	void SetEditType(int32 eeType);

	void SetText(char16 *text, int32 textSz, int32 buffSize);

	virtual void OnSetFocus();
	virtual void OnLostFocus();

	void OnChar(char16 ch);

	virtual bool InsertText(const char16 *textToInsert);

	void SetCursorWidth(int32 cWidth);
	int32 GetCursorWidth()				const	{ return cursorWidth; }

	bool IsEmpty();

protected:
	
	char16 GetChar(char16 ch);

	virtual void DeleteSymbol();
	void StopEditSymbol();
	void MoveCursorOnNext();
	virtual void MoveCursorOnPrev();

	virtual bool MoveCursorOnUp()		{	return	true;	}
	virtual bool MoveCursorOnDown()		{	return	true;	}
	virtual void SetStartCursorPos()	{	}
	virtual void CalcCursorXY()			{	}

	void ProcessKey(int32 iKey);
	void UpdateEnteredText();
	virtual bool AddChar(char16	ch);
	virtual void ChangeChar(char16	ch);
	void ProcessNumKeys();
	void UpdateCharCounters();

	void UpdateCursorCounter();


protected:
	eEditTextType		editType;

	char16			*	textBuffer;
	int32				textSize;
	int32				bufferSize;

	int32				currentPos;

	char16				*pKeys[KEYS_COUNT];

	// key processing
	bool				isKeyDown;
	int32				curKey;
	int32				changeCharCounter;
	int32				curSymbolInTable;
	uint32				applyCharTime;

	// if we add the char or edit it
	bool				addChar;
	
	bool				showCursor;
	int32				cursorCounter;

	//! cursor
	int32				cursorX;
	int32				cursorY;
	int32				cursorWidth;

	//draw optimization
	bool				textChanged;

	bool				textEntered;


	bool unfocusedEnter;
};

#endif//__GUI_INPUT_TEXT_