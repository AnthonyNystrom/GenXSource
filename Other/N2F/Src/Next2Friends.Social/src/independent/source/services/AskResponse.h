#ifndef __ASKRESPONSE_H__
#define __ASKRESPONSE_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "AskResponse.h"

struct AskResponse 
{
    int32 AskQuestionID;
    char16* Question;
    ArrayOfInt8 PhotoBase64Binary;
    ArrayOfInt32 ResponseValues;
    int32 Average;
    int32 ResponseType;
    ArrayOfString CustomResponses;
    bool WriteToStream(PacketStream *pStream)
    {
          pStream->WriteInt32(AskQuestionID);
          pStream->WriteWString(Question);
          PhotoBase64Binary.WriteToStream(pStream);
          ResponseValues.WriteToStream(pStream);
          pStream->WriteInt32(Average);
          pStream->WriteInt32(ResponseType);
          CustomResponses.WriteToStream(pStream);
        return true;
    }

    bool ReadFromStream(PacketStream *pStream)
    {
        pStream->ReadInt32(AskQuestionID);
        Question = pStream->GetWString();
        PhotoBase64Binary.ReadFromStream(pStream);
        ResponseValues.ReadFromStream(pStream);
        pStream->ReadInt32(Average);
        pStream->ReadInt32(ResponseType);
        CustomResponses.ReadFromStream(pStream);
        return true;
    }
    ~AskResponse()
    {
        SAFE_DELETE(Question);
    }
};

#endif // __ASKRESPONSE_H__

