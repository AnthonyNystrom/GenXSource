#ifndef __PHOTOITEM_H__
#define __PHOTOITEM_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "PhotoItem.h"

struct PhotoItem 
{
    char16* WebPhotoID;
    char16* MainPhotoURL;
    char16* ThumbnailURL;
    bool WriteToStream(PacketStream *pStream)
    {
          pStream->WriteWString(WebPhotoID);
          pStream->WriteWString(MainPhotoURL);
          pStream->WriteWString(ThumbnailURL);
        return true;
    }

    bool ReadFromStream(PacketStream *pStream)
    {
        WebPhotoID = pStream->GetWString();
        MainPhotoURL = pStream->GetWString();
        ThumbnailURL = pStream->GetWString();
        return true;
    }
    ~PhotoItem()
    {
        SAFE_DELETE(WebPhotoID);
        SAFE_DELETE(MainPhotoURL);
        SAFE_DELETE(ThumbnailURL);
    }
};

#endif // __PHOTOITEM_H__

