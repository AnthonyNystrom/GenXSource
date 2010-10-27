#include "N2FOutboxManager.h"
#include "AskService.h"
#include "ApplicationManager.h"
#include "N2FMessage.h"
#include "AskQuestionConfirm.h"
#include "SnapUpService.h"
#include "AskComment.h"

#include "stringres.h"
#include "GUIAlert.h"
#include "GUIMultiString.h"
#include "LibPhotoItem.h"
#include "StringWrapper.h"


N2FOutboxManager::N2FOutboxManager( ApplicationManager *appManager )
: N2FMessageOwner(EOT_DRAFT_MANAGER, appManager)
{
	pImageBuffer = new int8[1024 * 100];
}

N2FOutboxManager::~N2FOutboxManager()
{
	SAFE_DELETE(pImageBuffer);
	if (currentSending)
	{
		SAFE_DELETE(currentSending);
	}
}

void N2FOutboxManager::OnMessageChanged( N2FMessage *pMsg )
{

}

void N2FOutboxManager::OnMessageWillSwitchOwner( N2FMessage *pMsg )
{

}

void N2FOutboxManager::OnMessageAdded( N2FMessage *pMsg )
{

}

void N2FOutboxManager::OwnerUpdate()
{
	if (isMessageCountChanged || isMessagesChanged)
	{
		Write("outbox.dat", pManager->GetStringWrapper()->GetStringText(IDS_OUTBOX_INVALID_SAVE));
	}

	N2FMessageOwner::OwnerUpdate();
	if (!pManager->IsUserLoggedIn())
	{
		return;
	}
	
	if (!messages->Empty() && !currentSending)
	{
		if (pManager->GetCore()->GetApplicationData()->isValid && pManager->IsUserLoggedIn())
		{
			for (VList::Iterator it = messages->Begin(); it != messages->End(); it++ )
			{
				N2FMessage *msg = (N2FMessage*)(*it);
				if (msg->NeedToSend())
				{
					sendingState = N2FOutboxManager::ESS_START;
					currentSending = msg;
					msg->SetSend(false);
					msg->SetOwner(NULL);
					photoIndex = 0;
					break;
				}
			}
		}
	}

	if (currentSending)
	{
		switch(sendingState)
		{
		case N2FOutboxManager::ESS_START:
			{
				switch(currentSending->GetType())
				{
				case EMT_QUESTION:
					{
						pManager->ShowOutbox(true);
						UTILS_LOG(EDMP_DEBUG, "N2FOutboxManager::Start sending QUESTION");
						ArrayOfString sarr(2);
						sarr.ppStrings[0] = (char16*)currentSending->GetText(ETT_QUESTION_VARIANT_A);
						sarr.ppStrings[1] = (char16*)currentSending->GetText(ETT_QUESTION_VARIANT_B);
						if (currentSending->GetInnerType() != EIT_QUESTION_A_OR_B)
						{
							sarr.length = 0;
						}
						pManager->GetAskService()->SubmitQuestion(this
							, pManager->GetCore()->GetApplicationData()->login
							, pManager->GetCore()->GetApplicationData()->password
							, (char16*)currentSending->GetText(ETT_QUESTION_TEXT)
							, currentSending->GetNumberOfPhotos()
							, (int32)currentSending->GetInnerType()
							, &sarr
							, (int32)currentSending->GetQuestionDuration()
							, currentSending->IsPrivate());
						sarr.length = 2;
						sarr.ppStrings[0] = NULL;
						sarr.ppStrings[1] = NULL;

						sendingState = N2FOutboxManager::ESS_WAIT_START_RESPONSE;
					}
					break;
				case EMT_PHOTO:
					{
						pManager->ShowOutbox(true);
						UTILS_LOG(EDMP_DEBUG, "N2FOutboxManager::Start sending PHOTO");
						if (PreparePhoto(N2FOutboxManager::ESS_WAIT_START_RESPONSE))
						{
							ArrayOfInt8	buf(pImageBuffer, pImageFile->GetSize());
							//test save to file
							//////////////////////////////////////////////////////////////////////////
							//File *fl = GetApplication()->GetFileSystem()->Open("test.jpg", File::EFM_CREATE);
							//fl->Write(buf.pBuffer, buf.length);
							//fl->Release();
							//////////////////////////////////////////////////////////////////////////
							pManager->GetSnapUpService()->DeviceUploadPhoto(this
								, pManager->GetCore()->GetApplicationData()->login
								, pManager->GetCore()->GetApplicationData()->password
								, &buf
								, (char16*)L"");
							buf.pBuffer = NULL;
							pImageFile->Release();
							pImageFile = NULL;
						}

					}
					break;
				case EMT_COMMENT:
					{
						pManager->ShowOutbox(true);
						UTILS_LOG(EDMP_DEBUG, "N2FOutboxManager::Start sending COMMENT");
						AskComment *comment = new AskComment();
						comment->DTCreated = (char16*)L"";
						comment->ID = currentSending->GetCommentID();
						comment->AskQuestionID = currentSending->GetID();
						comment->Nickname = pManager->GetCore()->GetApplicationData()->login;
						comment->Text = (char16*)currentSending->GetText(ETT_COMMENT_TEXT);

						pManager->GetAskService()->AddComment(this
							, pManager->GetCore()->GetApplicationData()->login
							, pManager->GetCore()->GetApplicationData()->password
							, comment);

						comment->DTCreated = NULL;
						comment->Nickname = NULL;
						comment->Text = NULL;

						SAFE_DELETE(comment);
						sendingState = N2FOutboxManager::ESS_WAIT_START_RESPONSE;

					}
					break;
				}
			}
			break;
		case N2FOutboxManager::ESS_QUESTION_PHOTO:
			{
				UTILS_LOG(EDMP_DEBUG, "N2FOutboxManager::Sending PHOTO index %d", photoIndex);
				if (PreparePhoto(N2FOutboxManager::ESS_WAIT_QUESTION_PHOTO_RESPONSE))
				{
					ArrayOfInt8	buf(pImageBuffer, pImageFile->GetSize());
					//test save to file
					//////////////////////////////////////////////////////////////////////////
					//File *fl = GetApplication()->GetFileSystem()->Open("test.jpg", File::EFM_CREATE);
					//fl->Write(buf.pBuffer, buf.length);
					//fl->Release();
					//////////////////////////////////////////////////////////////////////////
					pManager->GetAskService()->AttachPhoto(this
						, pManager->GetCore()->GetApplicationData()->login
						, pManager->GetCore()->GetApplicationData()->password
						, questionID
						, photoIndex + 1
						, &buf);
					buf.pBuffer = NULL;
					pImageFile->Release();
					pImageFile = NULL;
				}
			}
			break;
		case N2FOutboxManager::ESS_QUESTION_FINISH:
			{
				UTILS_LOG(EDMP_DEBUG, "N2FOutboxManager::Sending QUESTION COMPLETE");
				pManager->GetAskService()->CompleteQuestion(this
					, pManager->GetCore()->GetApplicationData()->login
					, pManager->GetCore()->GetApplicationData()->password
					, questionID);
				sendingState = N2FOutboxManager::ESS_WAIT_QUESTION_FINISH_RESPONSE;
			}
		}
	}

}

void N2FOutboxManager::OnSuccess( int32 packetId )
{
	switch(packetId)
	{
	case AskService::EPS_SUBMITQUESTION:
		{
			UTILS_LOG(EDMP_DEBUG, "N2FOutboxManager::!!! Success QUESTION SUBMIT !!!");
			AskQuestionConfirm *res = pManager->GetAskService()->OnSubmitQuestion();
			Utils::WStrCpy(questionID, res->AskQuestionID);
			sendingState = N2FOutboxManager::ESS_QUESTION_PHOTO;
			SAFE_DELETE(res);
		}
		break;
	case AskService::EPS_ATTACHPHOTO:
		{
			UTILS_LOG(EDMP_DEBUG, "N2FOutboxManager::!!! Success ATTACH PHOTO !!!");
			pManager->GetAskService()->OnAttachPhoto();
			photoIndex++;
			if (currentSending->GetNumberOfPhotos() > photoIndex)
			{
				sendingState = N2FOutboxManager::ESS_QUESTION_PHOTO;
			}
			else
			{
				sendingState = N2FOutboxManager::ESS_QUESTION_FINISH;
			}
		}
		break;
	case AskService::EPS_COMPLETEQUESTION:
	//case SnapUpService::EPS_DEVICEUPLOADPHOTO// TODO: fix enum numbers in generated code
		{
			switch(currentSending->GetType())
			{
			case EMT_QUESTION:
				{
					UTILS_LOG(EDMP_DEBUG, "N2FOutboxManager::!!! Success QUESTION COMPLETE !!!");
					pManager->GetAskService()->OnCompleteQuestion();
					ReturnCurrent();
					pManager->ShowOutbox(false);
				}
				break;
			case EMT_PHOTO:
				{
					UTILS_LOG(EDMP_DEBUG, "N2FOutboxManager::!!! Success PHOTO UPLOADED !!!");
					pManager->GetSnapUpService()->OnDeviceUploadPhoto();
					ReturnCurrent();
					pManager->ShowOutbox(false);
				}
				break;
			}
		}
		break;
	case AskService::EPS_ADDCOMMENT:
		{
			UTILS_LOG(EDMP_DEBUG, "N2FOutboxManager::!!! Success PHOTO UPLOADED !!!");
			pManager->GetAskService()->OnAddComment();
			ReturnCurrent();
			pManager->ShowOutbox(false);
		}
		break;
	}
}

void N2FOutboxManager::OnFailed( int32 packetId, char16 *errorString )
{
	UTILS_LOG(EDMP_DEBUG, "N2FOutboxManager:: !!! Sending FAILED !!! ");
	char8 tempStr[5000];
	Utils::WstrToStr(errorString, tempStr, 5000);
	UTILS_LOG(EDMP_DEBUG, "N2FOutboxManager:: %s ", tempStr);
	currentSending->SetOwner(this);
	currentSending = NULL;
	pManager->ShowOutbox(false);
	switch(packetId)
	{
	case AskService::EPS_SUBMITQUESTION:
		{

		}
		break;
	case AskService::EPS_ATTACHPHOTO:
		{

		}
		break;
	case AskService::EPS_COMPLETEQUESTION:
		{

		}
		break;
	}
}


void N2FOutboxManager::OnEncodingSuccess(int32 size)
{
	UTILS_LOG(EDMP_DEBUG, "N2FOutboxManager::Resizing DODE !!!");

	//test save to file
	//////////////////////////////////////////////////////////////////////////
	//File *fl = GetApplication()->GetFileSystem()->Open("test.jpg", File::EFM_CREATE);
	//fl->Write(pImageBuffer,size);
	//fl->Release();
	//////////////////////////////////////////////////////////////////////////
	ArrayOfInt8	buf(pImageBuffer, size);
	switch(currentSending->GetType())
	{
	case EMT_QUESTION:
		{
			pManager->GetAskService()->AttachPhoto(this
				, pManager->GetCore()->GetApplicationData()->login
				, pManager->GetCore()->GetApplicationData()->password
				, questionID
				, photoIndex + 1
				, &buf);
		}
		break;
	case EMT_PHOTO:
		{
			pManager->GetSnapUpService()->DeviceUploadPhoto(this
				, pManager->GetCore()->GetApplicationData()->login
				, pManager->GetCore()->GetApplicationData()->password
				, &buf
				, (char16*)L"");
		}
		break;
	}
	buf.pBuffer = NULL;
	pImageFile->Release();
	pImageFile = NULL;
}

void N2FOutboxManager::OnEncodingCanceled()
{
	pImageFile->Release();
	pImageFile = NULL;
	MoveCurrentToUnsend();
}

void N2FOutboxManager::OnEncodingFailed()
{
	pImageFile->Release();
	pImageFile = NULL;
	char16 temp[1000];
	Utils::WSPrintf(temp, 1000 * 2, pManager->GetStringWrapper()->GetStringText(IDS_UNABLE_TO_SEND_PHOTO),  currentSending->GetPhoto(photoIndex)->GetName());
	pManager->GetAlert()->GetText()->SetText(temp);
	pManager->GetAlert()->Show();
	MoveCurrentToUnsend();
}

void N2FOutboxManager::SendAll()
{
	for (VList::Iterator it = messages->Begin(); it != messages->End(); it++ )
	{
		N2FMessage *msg = (N2FMessage*)(*it);
		msg->SetSend(true);
	}

}

void N2FOutboxManager::ReturnCurrent()
{
	currentSending->SetOwner(pManager);
	currentSending = NULL;
}

void N2FOutboxManager::MoveCurrentToUnsend()
{
	pManager->ShowOutbox(false);
	currentSending->SetOwner(this);
	currentSending = NULL;
}

void N2FOutboxManager::Init()
{
	Read("outbox.dat", pManager->GetStringWrapper()->GetStringText(IDS_OUTBOX_INVALID_LOAD));
}


bool N2FOutboxManager::PreparePhoto(eSandingState stateIfDone)
{
	if (!pImageFile)
	{
		pImageFile = pManager->GetPhotoLib()->GetPhotoFile(currentSending->GetPhoto(photoIndex));
		if (!pImageFile)
		{
			char16 temp[1000];
			Utils::WSPrintf(temp, 1000 * 2, pManager->GetStringWrapper()->GetStringText(IDS_UNABLE_TO_SEND_PHOTO),  currentSending->GetPhoto(photoIndex)->GetName());
			pManager->GetAlert()->GetText()->SetText(temp);
			pManager->GetAlert()->Show();
			MoveCurrentToUnsend();

			return false;
		}
		else
		{
			ImageInfo inf = *pManager->GetEncoder()->GetInfo(pImageFile);
			if (!inf.width || !inf.height)
			{
				pImageFile->Release();
				pImageFile = NULL;
				char16 temp[1000];
				Utils::WSPrintf(temp, 1000 * 2, pManager->GetStringWrapper()->GetStringText(IDS_UNABLE_TO_SEND_PHOTO),  currentSending->GetPhoto(photoIndex)->GetName());
				pManager->GetAlert()->GetText()->SetText(temp);
				pManager->GetAlert()->Show();
				MoveCurrentToUnsend();
				return false;
			}
			if (inf.width <= ENP_MAX_PHOTO_WIDTH && inf.height <= ENP_MAX_PHOTO_HEIGHT && pImageFile->GetSize() < ENP_MAX_PHOTO_SIZE)
			{
				pImageFile->Seek(0, File::ESO_START);
				pImageFile->Read(pImageBuffer, pImageFile->GetSize());
				sendingState = stateIfDone;
				return true;
			}
		}
	}
	else
	{
		if (pManager->GetEncoder()->IsFree())
		{
			UTILS_LOG(EDMP_DEBUG, "N2FOutboxManager::Resize PHOTO");
			ImageInfo inf = *pManager->GetEncoder()->GetInfo(pImageFile);
			if (!inf.width || !inf.height)
			{
				pImageFile->Release();
				pImageFile = NULL;
				char16 temp[1000];
				Utils::WSPrintf(temp, 1000 * 2, pManager->GetStringWrapper()->GetStringText(IDS_UNABLE_TO_SEND_PHOTO),  currentSending->GetPhoto(photoIndex)->GetName());
				pManager->GetAlert()->GetText()->SetText(temp);
				pManager->GetAlert()->Show();
				MoveCurrentToUnsend();
				return false;
			}
			pManager->GetEncoder()->SetListener(this);
			int32 w = inf.width;
			int32 h = inf.height;
			w = ENP_MAX_PHOTO_WIDTH;
			h = inf.height * w / inf.width;
			if (h > ENP_MAX_PHOTO_HEIGHT)
			{
				h = ENP_MAX_PHOTO_HEIGHT;
				w = inf.width * h / inf.height;
			}
			inf.width = w;
			inf.height = h;

			pManager->GetEncoder()->Resize(pImageFile, (char8*)pImageBuffer, ENP_MAX_PHOTO_SIZE, &inf);
			sendingState = stateIfDone;
		}
	}
	return false;
}