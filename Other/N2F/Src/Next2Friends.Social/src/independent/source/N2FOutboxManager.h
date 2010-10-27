#ifndef _N2F_OUTBOX_MANAGER_H_
#define _N2F_OUTBOX_MANAGER_H_

#include "N2FMessageOwner.h"
#include "IAnswer.h"
#include "EncoderObject.h"

class ApplicationManager;
class File;

class N2FOutboxManager : public N2FMessageOwner, public IAnswer, public EncoderListener
{
	enum eSandingState
	{
			ESS_START = 0
		,	ESS_WAIT_START_RESPONSE
		,	ESS_QUESTION_PHOTO
		,	ESS_WAIT_QUESTION_PHOTO_RESPONSE
		,	ESS_QUESTION_FINISH
		,	ESS_WAIT_QUESTION_FINISH_RESPONSE
	};
public:
	N2FOutboxManager(ApplicationManager *appManager);
	virtual ~N2FOutboxManager();

	virtual void OwnerUpdate();


	virtual void OnSuccess(int32 packetId);
	virtual void OnFailed(int32 packetId, char16 *errorString);

	virtual void OnEncodingSuccess(int32 size);
	virtual void OnEncodingCanceled();
	virtual void OnEncodingFailed();

	void SendAll();

	void Init();

protected:	
	virtual void OnMessageChanged(N2FMessage *pMsg);
	virtual void OnMessageWillSwitchOwner(N2FMessage *pMsg);
	virtual void OnMessageAdded(N2FMessage *pMsg);

	void ReturnCurrent();
	void MoveCurrentToUnsend();

	bool PreparePhoto(eSandingState stateIfDone);


private:


	N2FMessage			*currentSending;
	eSandingState		sendingState;
	char16				questionID[ENP_MAX_QUESTION_ID_SIZE];
	int8				*pImageBuffer;
	File				*pImageFile;
	int32				photoIndex;
};


#endif//_SCREEN_BASE_H_
