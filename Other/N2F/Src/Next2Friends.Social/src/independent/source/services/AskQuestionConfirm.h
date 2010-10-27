#ifndef __ASKQUESTIONCONFIRM_H__
#define __ASKQUESTIONCONFIRM_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "AskQuestionConfirm.h"

struct AskQuestionConfirm 
{
    char16* AdvertURL;
    char16* AdvertImage;
    char16* AskQuestionID;
    bool WriteToStream(PacketStream *pStream)
    {
          pStream->WriteWString(AdvertURL);
          pStream->WriteWString(AdvertImage);
          pStream->WriteWString(AskQuestionID);
        return true;
    }

    bool ReadFromStream(PacketStream *pStream)
    {
        AdvertURL = pStream->GetWString();
        AdvertImage = pStream->GetWString();
        AskQuestionID = pStream->GetWString();
        return true;
    }
    ~AskQuestionConfirm()
    {
        SAFE_DELETE(AdvertURL);
        SAFE_DELETE(AdvertImage);
        SAFE_DELETE(AskQuestionID);
    }
};

#endif // __ASKQUESTIONCONFIRM_H__

