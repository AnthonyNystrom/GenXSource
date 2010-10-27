#ifndef __AAFAPPASKQUESTIONFORM_H__
#define __AAFAPPASKQUESTIONFORM_H__

#include <aknform.h>
#include "aaf.rsg"


//FORWARD DECLARATION
class _ns1__QuestionData;

// Class declaration
class CAafQuestionForm : public CAknForm, public MEikCommandObserver
{
public:  // Constructor and destructor  
	/**
	* NewL
	* Two-phased constructor.
	*/
	static CAafQuestionForm* NewL();

	/**
	* ~CAafQuestionForm
	* Destructor.
	*/
	virtual ~CAafQuestionForm();

public:
	/**
	* From CAknForm, ExecuteLD
	* @return CAknForm::ExecuteLD return value
	* @param aResourceId resource ID
	*/
	TInt ExecuteLD( TInt aResourceId ); 

	/**
	* From CAknForm, PrepareLC
	* @param aResourceId resource ID
	*/
	void PrepareLC( TInt aResourceId ); 

	/**
	* Initiate question's data
	* @param aQuestionData question data structure
	*/
	void SetFormDataL(_ns1__QuestionData* aQuestionData);

protected:
	/**
	* From CAknForm, DynInitMenuPaneL
	* @param aResourceId resource ID
	* @param aMenuPane menu pane pointer
	*/
	void DynInitMenuPaneL(TInt aResourceId, CEikMenuPane *aMenuPane);

	/**
	* From MEikCommandObserver, ProcessCommandL
	* @param aCommandId command ID
	*/
	void ProcessCommandL(TInt aCommandId);

	/**
	* To observer form lines changes
	*/
	void HandleControlStateChangeL(TInt aControlId);

	/**
	* From , OfferKeyEventL
	*/
	TKeyResponse OfferKeyEventL(const TKeyEvent& aKeyEvent,TEventCode aType);

	/**
	* From , OkToExitL
	*/
	TBool OkToExitL(TInt aButtonId);
		
private:  // Constructor
	/**
	* CAafQuestionForm
	* Default constructor.
	*/
	CAafQuestionForm();

	/**
	* ConstructL
	* Second-phase constructor.
	*/
	void ConstructL();

private:  // Functions from base class
	/**
	* From CEikDialog, PostLayoutDynInitL 
	* Set default field value to member data.
	*/
	void PostLayoutDynInitL();

	/**
	* From CEikDialog, PreLayoutDynInitL
	*/
	void PreLayoutDynInitL();

	/**
	* From CAknForm , SaveFormDataL
	* Save the contents of the form.
	*/
	TBool SaveFormDataL(); 

	/**
	* From CAknForm, AddItemL
	* Add dynamically new item (in edit mode)
	*/
	void AddItemL();

	/**
	* Delete specified item
	*/
	void DeleteItem(TInt aItemId);

private:
	// Data members
	static _ns1__QuestionData* iQuestionData;

	TBool iCustomResponses; // Indicates, whether custom responses fields are present

	TBool iVerifyContent; // Indicates, whether we verify form content while saving
};

#endif