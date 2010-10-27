#ifndef __BASE_SERVICE_H__
#define __BASE_SERVICE_H__

#include "BaseTypes.h"
#include "PacketStream.h"
#include "IAnswer.h"
#include "NetSystem.h"

//enum TypeID : short
//{
//	Null = -1,
//		Void = 5,
//		Boolean = 10,
//		Byte = 20,
//		Float = 30,
//		Int16 = 40,
//		Int32 = 50,
//		Int32Array = 55,
//		String = 60,
//		StringArray = 65,
//		AskComment = 68,
//		GetCommentResult = 90,
//		GetQuestionResult = 110,
//		GetResponseResult = 130,
//		SubmitQuestionResult = 160
//}
struct ArrayOfString
{
	ArrayOfString(int32 count)
	{
		ppStrings = new char16*[count];
		length = count;
	};
	ArrayOfString(char16** strings, int32 count)
	{
		ppStrings = strings;
		length = count;
	};
	ArrayOfString()
	{
		length = 0;
	};
	~ArrayOfString()
	{
		ClearArray();
	}

	void ClearArray()
	{
		for (int32 i = 0; i < length; i++)
		{
			SAFE_DELETE(ppStrings[i]);
		}
		SAFE_DELETE(ppStrings);
		length = 0;
	}

	bool WriteToStream(PacketStream *pStream)
	{
		pStream->WriteInt32(length);
		for (int32 i = 0; i < length; i++)
		{
			pStream->WriteWString(ppStrings[i]);
		}
		return true;
	}
	bool ReadFromStream(PacketStream *pStream)
	{
		ClearArray();
		pStream->ReadInt32(length);
		if (length)
		{
			ppStrings = new char16*[length];
			for (int32 i = 0; i < length; i++)
			{
				ppStrings[i] = pStream->GetWString();
			}
		}
		return true;
	}

	char16 **ppStrings;
	int32 length;
};

struct ArrayOfInt32
{
	ArrayOfInt32(int32 len)
	{
		pBuffer = new int32[len];
		length = len;
	};
	ArrayOfInt32(int32 *buffer, int32 len)
	{
		pBuffer = buffer;
		length = len;
	};
	ArrayOfInt32()
	{
		length = 0;
	};
	~ArrayOfInt32()
	{
		ClearArray();
	}

	void ClearArray()
	{
		SAFE_DELETE(pBuffer);
		length = 0;
	}

	bool WriteToStream(PacketStream *pStream)
	{
		pStream->WriteInt32(length);
		for (int32 i = 0; i < length; i++)
		{
			pStream->WriteInt32(pBuffer[i]);
		}
		return true;
	}
	bool ReadFromStream(PacketStream *pStream)
	{
		ClearArray();
		pStream->ReadInt32(length);
		if (length)
		{
			pBuffer = new int32[length];
			for (int32 i = 0; i < length; i++)
			{
				pStream->ReadInt32(pBuffer[i]);
			}
		}
		return true;
	}

	int32 *pBuffer;
	int32 length;
};

struct ArrayOfInt8
{
	ArrayOfInt8(int32 len)
	{
		pBuffer = new int8[len];
		length = len;
	};
	ArrayOfInt8(int8 *buffer, int32 len)
	{
		pBuffer = buffer;
		length = len;
	};
	ArrayOfInt8()
	{
		length = 0;
	};
	~ArrayOfInt8()
	{
		ClearArray();
	}

	void ClearArray()
	{
		SAFE_DELETE(pBuffer);
		length = 0;
	}

	bool WriteToStream(PacketStream *pStream)
	{
		pStream->WriteInt32(length);
		pStream->WriteBuffer((uint8*)pBuffer, length);
		return true;
	}
	bool ReadFromStream(PacketStream *pStream)
	{
		ClearArray();
		pStream->ReadInt32(length);
		if (length)
		{
			pBuffer = new int8[length];
			pStream->ReadBuffer((uint8*)pBuffer, length);
		}
		return true;
	}

	int8 *pBuffer;
	int32 length;
};

class BaseService 
{
	friend class Server;

public:

	BaseService(Server *server);
	~BaseService();

	bool StartRequest(IAnswer *answerCB, int32 requestID);
	bool EndRequest();


protected:
	virtual void OnAnswer(PacketStream *answer, IAnswer *answerCB, int32 requestID);
	virtual void OnError(IAnswer *answerCB, int32 requestID, char16 *errorString);


protected:

	Server *pServer;

	PacketStream *answerStream;
};

#endif // __MEMBER_SERVICE_H__
