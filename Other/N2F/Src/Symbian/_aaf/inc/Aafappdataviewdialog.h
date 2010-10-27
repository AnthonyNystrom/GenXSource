/*
============================================================================
Name        : Aafappdataviewdialog.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : General view dialog to display various data
============================================================================
*/
#ifndef __AAFAPPDATAVIEWDIALOG_H__
#define __AAFAPPDATAVIEWDIALOG_H__

#include <eikdialg.h>
#include <eikedwin.h>

// For internal usage,
// define data to be displayed
enum TViewDialogType
{
	EPrivateQuestion,
	EUserQuestion,
	EQuestionComment
};

class CAafAppDataViewDialog: protected CEikDialog
{
public:
	/**
	* Static method
	*/
	static CAafAppDataViewDialog* ConstructL(TViewDialogType aDataType);

protected:
	/**
	* Default constructor
	*/
	CAafAppDataViewDialog();

	/**
	* Constructor with argument
	*/
	CAafAppDataViewDialog(TViewDialogType aDataType);

public:
	/**
	* Destructor
	*/
	virtual ~CAafAppDataViewDialog();

	/**
	* Shows dialog
	*/
	TInt ShowLD();

protected:
	/**
	* From CEikDialog
	*/
	void PreLayoutDynInitL();

	/**
	* From CEikDialog
	*/
	void PostLayoutDynInitL();

private:	

	TViewDialogType iCurrentData;

	CEikEdwin* iNickName;

	CEikEdwin* iDateTime;

	CEikEdwin* iText;
};

#endif // __AAFAPPDATAVIEWDIALOG_H__