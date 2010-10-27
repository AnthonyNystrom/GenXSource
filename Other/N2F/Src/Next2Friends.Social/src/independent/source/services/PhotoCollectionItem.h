#ifndef __PHOTOCOLLECTIONITEM_H__
#define __PHOTOCOLLECTIONITEM_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "PhotoCollectionItem.h"

struct PhotoCollectionItem 
{
    char16* WebPhotoCollectionID;
    char16* Name;
    bool WriteToStream(PacketStream *pStream)
    {
          pStream->WriteWString(WebPhotoCollectionID);
          pStream->WriteWString(Name);
        return true;
    }

    bool ReadFromStream(PacketStream *pStream)
    {
        WebPhotoCollectionID = pStream->GetWString();
        Name = pStream->GetWString();
        return true;
    }
    ~PhotoCollectionItem()
    {
        SAFE_DELETE(WebPhotoCollectionID);
        SAFE_DELETE(Name);
    }
};

#endif // __PHOTOCOLLECTIONITEM_H__

