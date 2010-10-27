#include <e32base.h>
#include <eikon.hrh>
#include <eikmenup.h>
#include "common.h"

#include "Aafappaskquestionform.h"
#include "aafuserquestionsserviceprovider.h"
#include "Aafappui.h"
#include <aknpopupfieldtext.h>
#include <eikcapc.h>
#include <eikedwin.h>
#include <eikon.hrh>

_ns1__QuestionData* CAafQuestionForm::iQuestionData = NULL;

// ----------------------------------------------------------------------------
// CAafQuestionForm::NewL()
// Two-phased constructor.
// ----------------------------------------------------------------------------
//
CAafQuestionForm* CAafQuestionForm::NewL()
{
	__LOGSTR_TOFILE("CAafQuestionForm::NewL() begins");

	CAafQuestionForm* self = new ( ELeave ) CAafQuestionForm();
	CleanupStack::PushL( self );
	self->ConstructL();
	CleanupStack::Pop();

	__LOGSTR_TOFILE("CAafQuestionForm::NewL() ends");

	return self;
}

// ----------------------------------------------------------------------------
// CAafQuestionForm::~CAafQuestionForm()
// Destructor.
// ----------------------------------------------------------------------------
//
CAafQuestionForm::~CAafQuestionForm()
{
	__LOGSTR_TOFILE("CAafQuestionForm::~CAafQuestionForm() begins");
}


// ----------------------------------------------------------------------------
// CAafQuestionForm::CAafQuestionForm()
// Default constructor.
// ----------------------------------------------------------------------------
//
CAafQuestionForm::CAafQuestionForm()
{
	iCustomResponses = EFalse;

	iVerifyContent = EFalse;
}

// ----------------------------------------------------------------------------
// CAafQuestionForm::ConstructL()
// Second-phase constructor.
// ----------------------------------------------------------------------------
//
void CAafQuestionForm::ConstructL()
{
	__LOGSTR_TOFILE("CAafQuestionForm::ConstructL() begins");

	CAknForm::ConstructL(R_QUESTION_FORM_MENUBAR);

	__LOGSTR_TOFILE("CAafQuestionForm::ConstructL() ends");
}

// ----------------------------------------------------------------------------
// CAafQuestionForm::ExecuteLD()
// 
// ----------------------------------------------------------------------------
//
TInt CAafQuestionForm::ExecuteLD( TInt aResourceId )
{
	__LOGSTR_TOFILE("CAafQuestionForm::ExecuteLD() begins");

	return CAknForm::ExecuteLD( aResourceId );
}

// ----------------------------------------------------------------------------
// CAafQuestionForm::PrepareLC( TInt aResourceId )
// 
// ----------------------------------------------------------------------------
//
void CAafQuestionForm::PrepareLC( TInt aResourceId )
{
	__LOGSTR_TOFILE("CAafQuestionForm::PrepareLC() begins");

	CAknForm::PrepareLC( aResourceId );

	__LOGSTR_TOFILE("CAafQuestionForm::PreapareLC() ends");
}

void CAafQuestionForm::SetFormDataL(_ns1__QuestionData* aQuestionData)
{
	// Question text
	CEikEdwin* questionText = (CEikEdwin*)Control( EQuestionText );
	if (questionText)
		questionText->SetTextL(&aQuestionData->iQuestionText);

	// Question type
	CAknPopupFieldText* questionType = (CAknPopupFieldText*)Control( EQuestionType );
	if (questionType)
		questionType->SetCurrentValueIndex( aQuestionData->iQuestionType);

	if (aQuestionData->iQuestionType == 1)
	{
		AddItemL();
		
		// Custom response A
		CEikEdwin* responseA = static_cast <CEikEdwin*> (ControlOrNull(ECustomResponseA));

		if (responseA)
		{
			responseA->SetTextL(&aQuestionData->iCustomResponseA);
		}

		// Custom response B
		CEikEdwin* responseB = static_cast <CEikEdwin*> (ControlOrNull(ECustomResponseB));

		if (responseB)
		{
			responseB->SetTextL(&aQuestionData->iCustomResponseB);
		}
	}

	// Question duration
	CAknPopupFieldText* questionDuration = (CAknPopupFieldText*)Control( EQuestionDuration );
	if (questionDuration)
		questionDuration->SetCurrentValueIndex( aQuestionData->iQuestionDuration);

	// Private mark
	CAknPopupFieldText* privateMark = (CAknPopupFieldText*)Control( EQuestionPrivate );
	if (aQuestionData->iPrivateMark)
		privateMark->SetCurrentValueIndex(0);
	else
		privateMark->SetCurrentValueIndex(1);
}

void CAafQuestionForm::DynInitMenuPaneL(TInt aResourceId, CEikMenuPane *aMenuPane)
{
	__LOGSTR_TOFILE("CAafQuestionForm::DynInitMenuPaneL() begins");

	CAknForm::DynInitMenuPaneL(aResourceId, aMenuPane);

	if (aResourceId == R_AVKON_FORM_MENUPANE)
	{
		// Default behavior handled in AknForm is suitable for this
		// form
		// If not, comment out and modify the following lines
		TBool editOptionDimmed = IsEditable();
		aMenuPane->SetItemDimmed(EAknFormCmdEdit, editOptionDimmed);

		// We do not want to allow the user to modify our Form
		// therefore
		// Disable the Label, Add, Delete, and Save Field
		aMenuPane->SetItemDimmed(EAknFormCmdLabel, ETrue);
		aMenuPane->SetItemDimmed(EAknFormCmdAdd, ETrue);
		aMenuPane->SetItemDimmed(EAknFormCmdDelete, ETrue);
		aMenuPane->SetItemDimmed(EAknFormCmdSave, ETrue);
	}

	__LOGSTR_TOFILE("CAafQuestionForm::DynInitMenuPaneL() ends");
}

void CAafQuestionForm::ProcessCommandL(TInt aCommandId)
{
	__LOGSTR_TOFILE("CAafQuestionForm::ProcessCommandL() begins");

	// Need to do this in all cases or menu does not disappear after a
	// selection has been made

	switch(aCommandId)
	{
	case EAskQuestionSubmit:
	case EResponses:
	case EPrivateQuestions:
	case EHelp:
	case EAbout:
	case EAknSoftkeyExit:
		{
			// Set question data to service provider
			if (aCommandId == EAskQuestionSubmit)
			{
				iVerifyContent = ETrue;

				if (SaveFormDataL())
				{
					__LOGSTR_TOFILE("CAafQuestionForm::ProcessCommandL(EAskQuestionSubmit) before questionProvider creating");

					CAafUserQuestionsProvider* questionProvider = CAafUserQuestionsProvider::GetInstanceL();

					__LOGSTR_TOFILE("CAafQuestionForm::ProcessCommandL(EAskQuestionSubmit) after questionProvider creating");

					if (questionProvider)
					{
						questionProvider->SetQuestionData(*iQuestionData);
					}

					delete iQuestionData;
					iQuestionData = NULL;

					iVerifyContent = EFalse;
				}
				else
				{
					iVerifyContent = EFalse;

					break;
				}
			}

			// Pass command handling to app ui instance
			CEikonEnv* eikonEnv = CEikonEnv::Static();
			CAafAppUi* appUi = (CAafAppUi*)eikonEnv->AppUi();

			if (appUi)
				appUi->HandleCommandL(aCommandId);

	
			// If necessary,
			// close the form
			switch(aCommandId)
			{
				case EAskQuestionSubmit:
					{
						CAknForm::TryExitL(EAknSoftkeyDone);
					}
					break;
				case EResponses:
				case EPrivateQuestions:
					{

						CAknForm::TryExitL(EAknSoftkeyBack);
					}
					break;
				default:
					break;
			}
		}
		break;
	default:
		CAknForm::ProcessCommandL(aCommandId);
		break;
	}

	__LOGSTR_TOFILE("CAafQuestionForm::ProcessCommandL() ends");
}

void CAafQuestionForm::HandleControlStateChangeL(TInt aControlId)
{
	CAknForm::HandleControlStateChangeL( aControlId );

	if( aControlId == EQuestionType )
	{
		CAknPopupFieldText* questionType = static_cast <CAknPopupFieldText*> (ControlOrNull(EQuestionType));

		// Add custom responses fields
		if (questionType->CurrentValueIndex() == 1)
		{
			AddItemL();
		}
		else
		{
			// If custom responses fields are present,
			// delete them
			if (iCustomResponses)
			{
				DeleteItem(ECustomResponseA);

				DeleteItem(ECustomResponseB);

				iCustomResponses = EFalse;

				DrawNow();
			}
		}
	}
}

TKeyResponse CAafQuestionForm::OfferKeyEventL(const TKeyEvent& aKeyEvent,TEventCode aType)
{
	if (aKeyEvent.iCode == EKeyOK)
	{
		TKeyEvent key;
		key.iCode = EKeyEnter;
		key.iScanCode = EStdKeyEnter;
		key.iRepeats = 0;
		key.iModifiers = 0;
		return CAknForm::OfferKeyEventL(key, EEventKey);
	}
	
	return CAknForm::OfferKeyEventL(aKeyEvent, aType);
}

TBool CAafQuestionForm::OkToExitL(TInt aButtonId)
{
	__LOGSTR_TOFILE("CAafQuestionForm::OkToExitL() begins");

	if (aButtonId == EAknSoftkeyBack)
	{
		__LOGSTR_TOFILE("CAafQuestionForm::OkToExitL() ends with aButtonId == EAknSoftkeyBack");

		SaveFormDataL();

		return ETrue;			
	}
	else if (aButtonId == EAknSoftkeyDone)
	{
		return ETrue;
	}
	
	__LOGSTR_TOFILE("CAafQuestionForm::OkToExitL() ends");

	return CAknForm::OkToExitL(aButtonId);
}

// ----------------------------------------------------------------------------
// CAafQuestionForm::SaveFormDataL()
// Save form data.
// ----------------------------------------------------------------------------
//
TBool CAafQuestionForm::SaveFormDataL()
{
	__LOGSTR_TOFILE("CAafQuestionForm::SaveFormDataL() begins");

	if (iQuestionData)
	{
		delete iQuestionData;
		iQuestionData = NULL;
	}

	iQuestionData = new (ELeave)_ns1__QuestionData;

	// Question text
	CEikEdwin* questionText = static_cast <CEikEdwin*> (ControlOrNull(EQuestionText));

	__LOGSTR_TOFILE("CAafQuestionForm::SaveFormDataL() step 01");

	if (questionText)
	{
		questionText->GetText(iQuestionData->iQuestionText);

		if (iQuestionData->iQuestionText.Length() == 0 && iVerifyContent)
		{
			CAknNoteDialog* dialog = new (ELeave)CAknNoteDialog(CAknNoteDialog::EErrorTone, CAknNoteDialog::ELongTimeout);

			// Get CCoeEnv instance
			CEikonEnv* eikonEnv = CEikonEnv::Static();

			HBufC* stringHolder = StringLoader::LoadL(R_EMPTY_QUESTION_NOTE, eikonEnv );

			dialog->SetTextL(*stringHolder);

			if (dialog->ExecuteDlgLD(R_DIALOG_FAILURE))
			{

			}

			delete stringHolder;
			stringHolder = NULL;

			return EFalse;
		}
	}
	else
	{
		return EFalse;
	}


	__LOGSTR_TOFILE("CAafQuestionForm::SaveFormDataL() step 02");

	// Response type
	CAknPopupFieldText* questionType = static_cast <CAknPopupFieldText*> (ControlOrNull(EQuestionType));
	
	__LOGSTR_TOFILE("CAafQuestionForm::SaveFormDataL() step 03");

	if (questionType)
	{
		__LOGSTR_TOFILE("CAafQuestionForm::SaveFormDataL() step 04");

		iQuestionData->iQuestionType = questionType->CurrentValueIndex();

		// Custom responses,
		// if required
		if (questionType->CurrentValueIndex() == 1)
		{
			__LOGSTR_TOFILE("CAafQuestionForm::SaveFormDataL() step 05");

			// Custom response A
			CEikEdwin* responseA = static_cast <CEikEdwin*> (ControlOrNull(ECustomResponseA));

			__LOGSTR_TOFILE("CAafQuestionForm::SaveFormDataL() step 06");

			if (responseA)
			{
				responseA->GetText(iQuestionData->iCustomResponseA);

				if (iQuestionData->iCustomResponseA.Length() == 0 && iVerifyContent)
				{
					CAknNoteDialog* dialog = new (ELeave)CAknNoteDialog(CAknNoteDialog::EErrorTone, CAknNoteDialog::ELongTimeout);

					// Get CCoeEnv instance
					CEikonEnv* eikonEnv = CEikonEnv::Static();

					HBufC* stringHolder = StringLoader::LoadL( R_EMPTY_CUSTOM_RESPONSEA_NOTE, eikonEnv );

					dialog->SetTextL(*stringHolder);

					if (dialog->ExecuteDlgLD(R_DIALOG_FAILURE))
					{

					}

					delete stringHolder;
					stringHolder = NULL;

					return EFalse;
				}
			}
			else
			{
				return EFalse;
			}

			__LOGSTR_TOFILE("CAafQuestionForm::SaveFormDataL() step 07");

			// Custom response B
			CEikEdwin* responseB = static_cast <CEikEdwin*> (ControlOrNull(ECustomResponseB));

			__LOGSTR_TOFILE("CAafQuestionForm::SaveFormDataL() step 08");

			if (responseB)
			{
				responseB->GetText(iQuestionData->iCustomResponseB);

				if (iQuestionData->iCustomResponseB.Length() == 0 && iVerifyContent)
				{
					CAknNoteDialog* dialog = new (ELeave)CAknNoteDialog(CAknNoteDialog::EErrorTone, CAknNoteDialog::ELongTimeout);

					// Get CCoeEnv instance
					CEikonEnv* eikonEnv = CEikonEnv::Static();

					HBufC* stringHolder = StringLoader::LoadL( R_EMPTY_CUSTOM_RESPONSEB_NOTE, eikonEnv );

					dialog->SetTextL(*stringHolder);

					if (dialog->ExecuteDlgLD(R_DIALOG_FAILURE))
					{

					}

					delete stringHolder;
					stringHolder = NULL;

					return EFalse;
				}
			}
			else
			{
				return EFalse;
			}
		}
	}
	else
	{
		return EFalse;
	}
	
	__LOGSTR_TOFILE("CAafQuestionForm::SaveFormDataL() step 09");

	// Question duration
	CAknPopupFieldText* questionDuration = static_cast <CAknPopupFieldText*> (ControlOrNull(EQuestionDuration));

	__LOGSTR_TOFILE("CAafQuestionForm::SaveFormDataL() step 10");

	if (questionDuration)
	{
		iQuestionData->iQuestionDuration = questionDuration->CurrentValueIndex();
	}
	else
	{
		return EFalse;
	}

	__LOGSTR_TOFILE("CAafQuestionForm::SaveFormDataL() step 11");

	// Private mark
	CAknPopupFieldText* privateMark = static_cast <CAknPopupFieldText*> (ControlOrNull(EQuestionPrivate));

	__LOGSTR_TOFILE("CAafQuestionForm::SaveFormDataL() step 12");

	if (privateMark)
	{
		iQuestionData->iPrivateMark = !(privateMark->CurrentValueIndex());
	}
	else
	{
		return EFalse;
	}

	__LOGSTR_TOFILE("CAafQuestionForm::SaveFormDataL() ends");

	return ETrue;
}

void CAafQuestionForm::AddItemL()
{
	__LOGSTR_TOFILE("CAafQuestionForm::AddItemL() begins");

	TInt pageId = ActivePageId();
	TAny* unused = 0;
	
	// Set edit field caption
	// Get CCoeEnv instance
	CEikonEnv* eikonEnv = CEikonEnv::Static();

	HBufC* stringHolder = StringLoader::LoadL(R_RESPONSEA_LABEL, eikonEnv );	

	TInt idA = ECustomResponseA; 
	TInt typeA = EEikCtEdwin;
	CEikEdwin* edwinA = (CEikEdwin*)CreateLineByTypeL( *stringHolder, pageId, idA, typeA, unused );
	edwinA->ConstructL(EEikEdwinNoHorizScrolling | EEikEdwinResizable, 10, 256, 1);
	Line(idA)->ActivateL();
	SetEditableL( IsEditable() ) ;
	
	// Free dynamically allocated memory
	delete stringHolder;
	stringHolder = NULL;

	// Set edit field caption
	stringHolder = StringLoader::LoadL(R_RESPONSEB_LABEL, eikonEnv );

	TInt idB = ECustomResponseB; 
	TInt typeB = EEikCtEdwin;
	CEikEdwin* edwinB = (CEikEdwin*)CreateLineByTypeL( *stringHolder, pageId, idB, typeB, unused );
	edwinB->ConstructL(EEikEdwinNoHorizScrolling | EEikEdwinResizable, 10, 256, 1);
	Line(idB)->ActivateL();
	SetEditableL( IsEditable() ) ;

	// Free dynamically allocated memory
	delete stringHolder;
	stringHolder = NULL;

	iCustomResponses = ETrue;

	DrawNow();

	// Ensure that focus will stay on question type field
	TryChangeFocusToL( EQuestionType );

	__LOGSTR_TOFILE("CAafQuestionForm::AddItemL() ends");
}

void CAafQuestionForm::DeleteItem(TInt aItemId)
{
	DeleteLine(aItemId, EFalse);
}

// ----------------------------------------------------------------------------
// CAafQuestionForm::PostLayoutDynInitL()
// Set default field value to member data.
// ----------------------------------------------------------------------------
//
void CAafQuestionForm::PostLayoutDynInitL()
{
	__LOGSTR_TOFILE("CAafQuestionForm::PostLayoutDynInitL() begins");

	CAknForm::PostLayoutDynInitL();

	SetEditableL( ETrue ); // Set form in editable stat at start

	__LOGSTR_TOFILE("CAafQuestionForm::PostLayoutDynInitL() ends");
}

void CAafQuestionForm::PreLayoutDynInitL()
{
	CAknForm::PreLayoutDynInitL();

	if (iQuestionData)
	{
		SetFormDataL(iQuestionData);
	}
}