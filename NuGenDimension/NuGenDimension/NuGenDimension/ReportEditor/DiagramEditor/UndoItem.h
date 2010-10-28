#ifndef _CUNDOITEM_H_D0245507_FE15_4E26_9C3FCD56AE91
#define _CUNDOITEM_H_D0245507_FE15_4E26_9C3FCD56AE91

///////////////////////////////////////////////////////////
// File :		UndoItem.h
// Created :	06/04/04
//

class CUndoItem : public CObject 
{

public:
	// Construction/destruction
	CUndoItem();
	virtual ~CUndoItem();

	// Public data
	CObArray	arr;	// Object array
	CPoint		pt;		// Virtual editor size

};

#endif //_CUNDOITEM_H_D0245507_FE15_4E26_9C3FCD56AE91
