#ifndef __ASKQUESTION_H__
#define __ASKQUESTION_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "AskQuestion.h"

struct AskQuestion 
{
    int32 ID;
    char16* Question;
    char16* DTCreated;
    bool WriteToStream(PacketStream *pStream)
    {
          pStream->WriteInt32(ID);
          pStream->WriteWString(Question);
          pStream->WriteWString(DTCreated);
        return true;
    }

    bool ReadFromStream(PacketStream *pStream)
    {
        pStream->ReadInt32(ID);
        Question = pStream->GetWString();
        DTCreated = pStream->GetWString();
        return true;
    }
    ~AskQuestion()
    {
        SAFE_DELETE(Question);
        SAFE_DELETE(DTCreated);
    }
};

#endif // __ASKQUESTION_H__

