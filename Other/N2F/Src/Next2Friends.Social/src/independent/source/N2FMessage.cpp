#include "N2FMessage.h"
#include "N2FMessageOwner.h"
#include "LibPhotoItem.h"
#include "FileSystem.h"
#include "ApplicationManager.h"


N2FMessage::N2FMessage()
{
	type = EMT_EMPTY;
	text[0] = new char16[ENP_MAX_QUESTION_SIZE];
	text[1] = new char16[ENP_MAX_ADDTEXT_SIZE];
	text[2] = new char16[ENP_MAX_ADDTEXT_SIZE];
}

N2FMessage::~N2FMessage()
{
	for (int i = 0; i < MAX_ATTACHED_PHOTOS; i++)
	{
		SAFE_DELETE(photos[i]);
	}
	for (int i = 0; i < 3; i++)
	{
		SAFE_DELETE_ARRAY(text[i]);
	}
}

void N2FMessage::InitAsQuestion( eInnerType innType, eQuestionDuration qDuration )
{
	Clear();
	type = EMT_QUESTION;
	innerType = innType;
	questionDuration = qDuration;
}

void N2FMessage::InitAsPhoto()
{
	Clear();
	type = EMT_PHOTO;
}

void N2FMessage::InitAsNews( eInnerType innType )
{
	Clear();
	innerType = innType;
	type = EMT_DASHBOARD;
}

void N2FMessage::InitAsQuestionName()
{
	Clear();
	type = EMT_QUESTION_NAME;
}

void N2FMessage::InitAsComment()
{
	Clear();
	type = EMT_COMMENT;
}


void N2FMessage::SetText( const char16 *newText, eTextType textType )
{
	FASSERT(Utils::WStrLen(newText) < ENP_MAX_QUESTION_SIZE - 1 && TEXT_LINKS[textType] == 0 || Utils::WStrLen(newText) < ENP_MAX_ADDTEXT_SIZE);
	Utils::WStrCpy(text[TEXT_LINKS[textType]], newText);
}

const char16* N2FMessage::GetText( eTextType textType ) const
{
	return text[TEXT_LINKS[textType]];
}

const char16* N2FMessage::GetUnlinkedText( int32 index ) const
{
	FASSERT(index >= 0 && index < 3);
	return text[index];
}

void N2FMessage::SetUnlinkedText( const char16 *newText, int32 index )
{
	FASSERT(index >= 0 && index < 3);
	Utils::WStrCpy(text[index], newText);
}


void N2FMessage::SetOwner( N2FMessageOwner *newOwner )
{
	if (owner == newOwner)
	{
		owner->MessgeChanged(this);
		return;
	}
	if (owner)
	{
		owner->MessgeWillSwitchOwner(this);
	}
	owner = newOwner;
	if (owner)
	{
		owner->AddMessage(this);
	}
}

void N2FMessage::SetInnerType( eInnerType innType )
{
		innerType = innType;
}

void N2FMessage::SetQuestionDuration( eQuestionDuration qDuration )
{
	questionDuration = qDuration;
}

void N2FMessage::SetPrivate( bool privateState )
{
	isPrivate = privateState;
}

bool N2FMessage::AttachPhoto( const LibPhotoItem *photoItem )
{
	if (numberOfAttachedPhotos >= MAX_ATTACHED_PHOTOS)
	{
		return false;
	}

	for (int i = 0; i < numberOfAttachedPhotos; i++)
	{
		if (*photos[i] == *photoItem)
		{
			return false;
		}
	}


	if (!photos[numberOfAttachedPhotos])
	{
		photos[numberOfAttachedPhotos] = new LibPhotoItem();
	}

	*photos[numberOfAttachedPhotos] = *photoItem;
	numberOfAttachedPhotos++;
	return true;
}

const LibPhotoItem * N2FMessage::GetPhoto( int32 index ) const
{
	FASSERT(index < numberOfAttachedPhotos);
	return photos[index];
}

bool N2FMessage::RemovePhoto( const LibPhotoItem *photoItem )
{
	if (!numberOfAttachedPhotos)
	{
		return false;
	}

	for (int i = 0; i < numberOfAttachedPhotos; i++)
	{
		if (*photos[i] == *photoItem)
		{
			for (int n = i; n < numberOfAttachedPhotos - 1; n++)
			{
				*photos[n] = *photos[n + 1];
			}
			numberOfAttachedPhotos--;
			photos[numberOfAttachedPhotos]->SetSize(0);
			return true;
		}
	}

	return false;
}

bool N2FMessage::HasPhoto( const LibPhotoItem *photoItem ) const
{
	if (!numberOfAttachedPhotos)
	{
		return false;
	}

	for (int i = 0; i < numberOfAttachedPhotos; i++)
	{
		if (*photos[i] == *photoItem)
		{
			return true;
		}
	}

	return false;
}

N2FMessage & N2FMessage::operator=( const N2FMessage &msg )
{
	Clear();
	type = msg.type;

	innerType = msg.GetInnerType();
	questionDuration = msg.GetQuestionDuration();
	isPrivate = msg.IsPrivate();
	for (int n = 0; n < 3; n++)
	{
		SetUnlinkedText(msg.GetUnlinkedText(n), n);
	}
	for(int32 i = 0; i < msg.GetNumberOfPhotos(); i++)
	{
		AttachPhoto(msg.GetPhoto(i));
	}
	Utils::WStrCpy(dateTime, msg.dateTime);
	sortDate = msg.sortDate;
	sortTime = msg.sortTime;
	sendNow = msg.NeedToSend();
	id = msg.id;
	commentID = msg.commentID;
	return *this;
}

bool N2FMessage::operator==( const N2FMessage &msg ) const
{
	if (type != msg.type
		|| innerType != msg.innerType
		|| questionDuration != msg.questionDuration
		|| isPrivate != msg.isPrivate
		|| numberOfAttachedPhotos != msg.numberOfAttachedPhotos
		|| sendNow != msg.sendNow
		|| id != msg.id 
		|| sortDate != msg.sortDate
		|| sortTime != msg.sortTime)
	{
		return false;
	}
	for (int n = 0; n < 3; n++)
	{
		if (Utils::WStrCmp(text[n], msg.text[n]))
		{
			return false;
		}
	}
	for(int32 i = 0; i < numberOfAttachedPhotos; i++)
	{
		if (*msg.photos[i] != *photos[i])
		{
			return false;
		}
	}
	if (Utils::WStrCmp(dateTime, msg.dateTime))
	{
		return false;
	}

	return true;
}

void N2FMessage::SetSend( bool needSendImmediately )
{
	sendNow = needSendImmediately;
}

const char16* N2FMessage::GetDateTime( void ) const
{
	return dateTime;
}

int32 N2FMessage::GetDateForSort(void) const
{
	return sortDate;
}
int32 N2FMessage::GetTimeForSort(void) const
{
	return sortTime;
}

void N2FMessage::SetDateTimeForSort(int32 newDate, int32 newTime)
{
	sortDate = newDate;
	sortTime = newTime;
	int32 hr = (sortTime / 100);
	if (hr < 12)
	{
		if (hr == 0)
		{
			hr = 12;
		}

		Utils::WSPrintf(dateTime, ENP_MAX_ADDTEXT_SIZE
			, (char16*)L"%d/%d/%d %d:%.2d AM"
			, (sortDate / 100) % 100	//month
			, (sortDate) % 100			//day
			, (sortDate / 10000)		//year
			, hr						//hours
			, sortTime  % 100);			//minutes
	}
	else
	{
		hr = hr - 12;
		if (hr == 0)
		{
			hr = 12;
		}

		Utils::WSPrintf(dateTime, ENP_MAX_ADDTEXT_SIZE
			, (char16*)L"%d/%d/%d %d:%.2d PM"
			, (sortDate / 100) % 100	//month
			, (sortDate) % 100			//day
			, (sortDate / 10000)		//year
			, hr						//hours
			, sortTime  % 100);			//minutes
	}
}

void N2FMessage::SetDateTime( const char16 *newText )
{
	//FASSERT(Utils::WStrLen(newText) < ENP_MAX_ADDTEXT_SIZE);
	//Utils::WStrCpy(dateTime, newText);
	sortDate = 0;
	for (int i = 0; i < 8; i++)
	{
		sortDate *= 10;
		sortDate += newText[i] - L'0';
	}
	sortTime = 0;
	for (int i = 8; i < 12; i++)
	{
		sortTime *= 10;
		sortTime += newText[i] - L'0';
	}
	SetDateTimeForSort(sortDate, sortTime);

	//char16 *dt = dateTime;
	//for (int i = 0; i < 2; i++)
	//{
	//	*dt = newText[i + 4];
	//	dt++;
	//}
	//*dt = (char16)L'/';
	//dt++;
	//for (int i = 0; i < 2; i++)
	//{
	//	*dt = newText[i + 6];
	//	dt++;
	//}
	//*dt = (char16)L'/';
	//dt++;
	//for (int i = 0; i < 4; i++)
	//{
	//	*dt = newText[i];
	//	dt++;
	//}

	//for (int i = 0; i < 2; i++)
	//{
	//	*dt = (char16)L' ';
	//	dt++;
	//}

	//for (int i = 0; i < 2; i++)
	//{
	//	*dt = newText[i + 8];
	//	dt++;
	//}
	//*dt = (char16)L':';
	//dt++;
	//for (int i = 0; i < 2; i++)
	//{
	//	*dt = newText[i + 10];
	//	dt++;
	//}
	//*dt = (char16)0;

}


bool N2FMessage::WriteToFile( File *fl )
{
	uint8 tp = type;
	fl->Write(&tp, 1);
	switch(type)
	{
	case EMT_EMPTY:
		{

		}
		break;
	case EMT_QUESTION:
		{
			uint8 it = (uint8)innerType;
			fl->Write(&it, 1);
			uint8 dt = (uint8)questionDuration;
			fl->Write(&dt, 1);
			uint8 isp = (uint8)isPrivate;
			fl->Write(&isp, 1);
			uint8 sn = (uint8)sendNow;
			fl->Write(&sn, 1);
			for (int i = 0; i < 3; i++)
			{
				fl->Write(text[i], Utils::WStrLen(text[i]) * 2 + 2);
			}
			uint8 nph = (uint8)numberOfAttachedPhotos;
			fl->Write(&nph, 1);
			for (int i = 0; i < numberOfAttachedPhotos; i++)
			{
				int32 isz = photos[i]->GetIDSize();
				fl->Write(&isz, 4);
				fl->Write((void*)photos[i]->GetID(), photos[i]->GetIDSize());
			}

		}
		break;
	case EMT_COMMENT:
		{
			fl->Write(&id, 1);
			fl->Write(&commentID, 1);
			uint8 sn = (uint8)sendNow;
			fl->Write(&sn, 1);
			for (int i = 0; i < 3; i++)
			{
				fl->Write(text[i], Utils::WStrLen(text[i]) * 2 + 2);
			}
		}
		break;
	case EMT_PHOTO:
		{
			fl->Write(text[0], Utils::WStrLen(text[0]) * 2 + 2);
			uint8 nph = (uint8)numberOfAttachedPhotos;
			fl->Write(&nph, 1);
			for (int i = 0; i < numberOfAttachedPhotos; i++)
			{
				int32 isz = photos[i]->GetIDSize();
				fl->Write(&isz, 4);
				fl->Write((void*)photos[i]->GetID(), photos[i]->GetIDSize());
			}

		}
		break;
	case EMT_DASHBOARD:
		{
			uint8 it = (uint8)innerType;
			fl->Write(&it, 1);
			for (int i = 0; i < 3; i++)
			{
				fl->Write(text[i], Utils::WStrLen(text[i]) * 2 + 2);
			}
			fl->Write(&sortDate, 4);
			fl->Write(&sortTime, 4);

		}
		break;
	case EMT_QUESTION_NAME:
		{
			fl->Write(text[0], Utils::WStrLen(text[0]) * 2 + 2);
			fl->Write(&sortDate, 4);
			fl->Write(&sortTime, 4);
			fl->Write(&id, 4);
			fl->Write(&commentID, 4);

		}
		break;
	}

	return true;
}

bool N2FMessage::ReadFromFile( File *fl, ApplicationManager *pManager )
{
	Clear();
	uint8 tp;
	if (fl->Read(&tp, 1) != 1)
	{
		return false;
	}
	type = (eMessageType)tp;

	switch(type)
	{
	case EMT_EMPTY:
		{

		}
		break;
	case EMT_QUESTION:
		{
			uint8 it;
			if (fl->Read(&it, 1) != 1)
			{
				return false;
			}
			innerType = (eInnerType)it;
			uint8 dt;
			if (fl->Read(&dt, 1) != 1)
			{
				return false;
			}
			questionDuration = (eQuestionDuration)dt;
			uint8 isp;
			if (fl->Read(&isp, 1) != 1)
			{
				return false;
			}
			isPrivate = (bool)isp;
			uint8 sn;
			if (fl->Read(&sn, 1) != 1)
			{
				return false;
			}
			sendNow = (bool)sn;
			for (int i = 0; i < 3; i++)
			{
				char16 ch = 0;
				int32 n = 0;
				do 
				{
					if (fl->Read(&ch, 2) != 2)
					{
						return false;
					}
					text[i][n++] = ch;
				} while(ch);
			}
			uint8 nph;
			if (fl->Read(&nph, 1) != 1)
			{
				return false;
			}
			for (int i = 0; i < nph; i++)
			{
				int32 idSize;
				if (fl->Read(&idSize, 4) != 4)
				{
					return false;
				}
				char8 temp[1000];
				char8 ch = 0;
				int32 n = 0;
				for (int n = 0; n < idSize; n++)
				{
					if (fl->Read(&ch, 1) != 1)
					{
						return false;
					}
					temp[n] = ch;
				}

				AttachPhoto(pManager->GetPhotoLib()->GetByID(temp, idSize));
			}
			numberOfAttachedPhotos = nph;


		}
		break;
	case EMT_COMMENT:
		{
			if (fl->Read(&id, 4) != 4)
			{
				return false;
			}
			if (fl->Read(&commentID, 4) != 4)
			{
				return false;
			}

			uint8 sn;
			if (fl->Read(&sn, 1) != 1)
			{
				return false;
			}
			sendNow = (bool)sn;
			for (int i = 0; i < 3; i++)
			{
				char16 ch = 0;
				int32 n = 0;
				do 
				{
					if (fl->Read(&ch, 2) != 2)
					{
						return false;
					}
					text[i][n++] = ch;
				} while(ch);
			}


		}
		break;
	case EMT_PHOTO:
		{
			char16 ch = 0;
			int32 n = 0;
			do 
			{
				if (fl->Read(&ch, 2) != 2)
				{
					return false;
				}
				text[0][n++] = ch;
			} while(ch);

			uint8 nph;
			if (fl->Read(&nph, 1) != 1)
			{
				return false;
			}
			for (int i = 0; i < nph; i++)
			{
				int32 idSize;
				if (fl->Read(&idSize, 4) != 4)
				{
					return false;
				}
				char8 temp[1000];
				char8 ch = 0;
				int32 n = 0;
				for (int n = 0; n < idSize; n++)
				{
					if (fl->Read(&ch, 1) != 1)
					{
						return false;
					}
					temp[n] = ch;
				}

				AttachPhoto(pManager->GetPhotoLib()->GetByID(temp, idSize));
			}
			numberOfAttachedPhotos = nph;
		}
		break;
	case EMT_DASHBOARD:
		{
			uint8 it;
			if (fl->Read(&it, 1) != 1)
			{
				return false;
			}
			innerType = (eInnerType)it;
			for (int i = 0; i < 3; i++)
			{
				char16 ch = 0;
				int32 n = 0;
				do 
				{
					if (fl->Read(&ch, 2) != 2)
					{
						return false;
					}
					text[i][n++] = ch;
				} while(ch);
			}

			{
				if (fl->Read(&sortDate, 4) != 4)
				{
					return false;
				}
				if (fl->Read(&sortTime, 4) != 4)
				{
					return false;
				}
				SetDateTimeForSort(sortDate, sortTime);
			}
		}
		break;
	case EMT_QUESTION_NAME:
		{
			{
				char16 ch = 0;
				int32 n = 0;
				do 
				{
					if (fl->Read(&ch, 2) != 2)
					{
						return false;
					}
					text[0][n++] = ch;
				} while(ch);
			}

			{
				if (fl->Read(&sortDate, 4) != 4)
				{
					return false;
				}
				if (fl->Read(&sortTime, 4) != 4)
				{
					return false;
				}
				SetDateTimeForSort(sortDate, sortTime);
			}
			if (fl->Read(&id, 4) != 4)
			{
				return false;
			}
			if (fl->Read(&commentID, 4) != 4)
			{
				return false;
			}

		}
		break;
	}

	return true;
}

