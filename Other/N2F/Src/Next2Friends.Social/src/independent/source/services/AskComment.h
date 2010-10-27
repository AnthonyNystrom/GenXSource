#ifndef __ASKCOMMENT_H__
#define __ASKCOMMENT_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "AskComment.h"

struct AskComment 
{
    int32 ID;
    int32 AskQuestionID;
    char16* Nickname;
    char16* Text;
    char16* DTCreated;
    bool WriteToStream(PacketStream *pStream)
    {
          pStream->WriteInt32(ID);
          pStream->WriteInt32(AskQuestionID);
          pStream->WriteWString(Nickname);
          pStream->WriteWString(Text);
          pStream->WriteWString(DTCreated);
        return true;
    }

    bool ReadFromStream(PacketStream *pStream)
    {
        pStream->ReadInt32(ID);
        pStream->ReadInt32(AskQuestionID);
        Nickname = pStream->GetWString();
        Text = pStream->GetWString();
        DTCreated = pStream->GetWString();
        return true;
    }
    ~AskComment()
    {
        SAFE_DELETE(Nickname);
        SAFE_DELETE(Text);
        SAFE_DELETE(DTCreated);
    }
};

#endif // __ASKCOMMENT_H__

