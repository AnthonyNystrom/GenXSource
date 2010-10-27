#ifndef _N2F_MESSAGE_H_
#define _N2F_MESSAGE_H_

#include "BaseTypes.h"
#include "N2FData.h"

enum eMessageType
{
		EMT_EMPTY = 0
	,	EMT_QUESTION
	,	EMT_PHOTO
	,	EMT_DASHBOARD
	,	EMT_COMMENT
	,	EMT_QUESTION_NAME


	,	EMT_COUNT
};

enum eInnerType
{
		EIT_QUESTION_YES_NO = 0
	,	EIT_QUESTION_A_OR_B
	,	EIT_QUESTION_RATE
	,	EIT_DASHBOARD_FIRST
	,	EIT_DASHBOARD_FIREND = EIT_DASHBOARD_FIRST
	,	EIT_DASHBOARD_WALL
	,	EIT_DASHBOARD_PHOTO
	,	EIT_DASHBOARD_VIDEO


	,	EIT_COUNT
};

enum eQuestionDuration
{
		EQD_5MIN = 0
	,	EQD_10MIN
	,	EQD_60MIN
	,	EQD_24HRS


	,	EQD_COUNT
};

enum eTextType
{
		ETT_QUESTION_TEXT = 0	//0
	,	ETT_QUESTION_VARIANT_A	//1
	,	ETT_QUESTION_VARIANT_B	//2
	,	ETT_DASHBOARD_TEXT		//0
	,	ETT_DASHBOARD_NICKNAME1	//1
	,	ETT_DASHBOARD_NICKNAME2	//2
	,	ETT_DASHBOARD_TITLE		//2
	,	ETT_PHOTO_TITLE			//0
	,	ETT_COMMENT_TEXT		//0
	,	ETT_COMMENT_NICKNAME	//1


	,	ETT_COUNT
};


static const int32 TEXT_LINKS[ETT_COUNT] = 
{
	0,	//ETT_QUESTION_TEXT
	1,	//ETT_QUESTION_VARIANT_A
	2,	//ETT_QUESTION_VARIANT_B
	0,	//ETT_DASHBOARD_TEXT
	1,	//ETT_DASHBOARD_NICKNAME1
	2,	//ETT_DASHBOARD_NICKNAME2
	2,	//ETT_DASHBOARD_TITLE
	0,	//ETT_PHOTO_TITLE
	0,	//ETT_COMMENT_TEXT
	1,	//ETT_COMMENT_NICKNAME

};

//enum eResponseType
//{
//		ERT_VAR_A = 0
//	,	ERT_VAR_B
//	,	ERT_AVERAGE
//
//
//	,	ERT_COUNT
//};

#define MAX_ATTACHED_PHOTOS		3

class N2FMessageOwner;
class LibPhotoItem;
class File;
class ApplicationManager;

class N2FMessage 
{
public:
	N2FMessage();
	virtual ~N2FMessage();

	bool IsFree()
	{
		return type == EMT_EMPTY;
	};

	void Clear()
	{
		type = EMT_EMPTY;
		numberOfAttachedPhotos = 0;
		for (int i = 0; i < 3; i++)
		{
			text[i][0] = 0;
		}
		dateTime[0] = 0;

		innerType = (eInnerType)0;
		questionDuration = (eQuestionDuration)0;
		isPrivate = false;
		sendNow = false;
		id = 0;
		commentID = 0;
	};

	eMessageType GetType()
	{
		return type;
	};
	
	void InitAsQuestion(eInnerType innType, eQuestionDuration qDuration);
	void InitAsPhoto();
	void InitAsNews(eInnerType innType);
	void InitAsQuestionName();
	void InitAsComment();

	const char16* GetText(eTextType textType) const;
	void SetText(const char16 *newText, eTextType textType);

	const char16* GetDateTime(void) const;
	void SetDateTime(const char16 *newText);

	int32 GetDateForSort(void) const;
	int32 GetTimeForSort(void) const;
	void SetDateTimeForSort(int32 newDate, int32 newTime);

	const char16* GetUnlinkedText(int32 index) const;
	void SetUnlinkedText(const char16 *newText, int32 index);

	void SetPrivate(bool privateState);


	eInnerType GetInnerType() const
	{
		return innerType;
	};

	void SetInnerType(eInnerType inType);

	eQuestionDuration GetQuestionDuration() const
	{
		return questionDuration;
	};

	void SetQuestionDuration(eQuestionDuration qDuration);

	N2FMessageOwner *GetOwner() const
	{
		return owner;
	};

	bool IsPrivate() const
	{
		return isPrivate;
	};


	void SetOwner(N2FMessageOwner *newOwner);


	bool IsPossibleToAttachPhoto() const
	{
		return numberOfAttachedPhotos < MAX_ATTACHED_PHOTOS;
	};

	bool AttachPhoto(const LibPhotoItem *photoItem);
	
	int32 GetNumberOfPhotos() const
	{
		return numberOfAttachedPhotos;
	};

	const LibPhotoItem *GetPhoto(int32 index) const;
	
	// ***************************************************
	//! \brief    	RemovePhoto
	//! 
	//! \param      photoItem
	//! \return   	bool true if photot removed
	// ***************************************************
	bool RemovePhoto(const LibPhotoItem *photoItem);

	bool HasPhoto(const LibPhotoItem *photoItem) const;

	bool NeedToSend() const
	{
		return sendNow;
	}
	void SetSend(bool needSendImmediately);

	int32 GetID() const
	{
		return id;
	};

	void SetID(int32 newID)
	{
		id = newID;
	};

	int32 GetCommentID() const
	{
		return commentID;
	};

	void SetCommentID(int32 newID)
	{
		commentID = newID;
	};

	
	N2FMessage& operator = (const N2FMessage &msg);
	bool		operator == ( const N2FMessage &msg ) const;


	bool WriteToFile(File *fl);
	bool ReadFromFile(File *fl, ApplicationManager *pManager);


private:

	eMessageType		type;
	N2FMessageOwner		*owner;

	eInnerType			innerType;
	eQuestionDuration	questionDuration;
	bool				isPrivate;

	char16 dateTime[ENP_MAX_ADDTEXT_SIZE];
	int32 sortTime;
	int32 sortDate;

	char16 *text[3];


	LibPhotoItem *photos[MAX_ATTACHED_PHOTOS];
	int32 numberOfAttachedPhotos;

	bool				sendNow;
	int32				id;
	int32				commentID;

};


#endif//_SCREEN_BASE_H_
