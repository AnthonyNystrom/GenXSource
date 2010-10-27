#ifndef __ARRAYOFPHOTOITEM_H__
#define __ARRAYOFPHOTOITEM_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "PhotoItem.h"

struct ArrayOfPhotoItem 
{
    int32 length;
    PhotoItem *pBuffer;
    
    ArrayOfPhotoItem ()
    {
        length = 0;
    }
    ArrayOfPhotoItem (int32 len)
    {
        length = len;
        pBuffer = new PhotoItem[len];
    }
    ArrayOfPhotoItem (PhotoItem *buffer, int32 len)
    {
        length = len;
        pBuffer = buffer;
    }
    ~ArrayOfPhotoItem ()
    {
        ClearArray();
    }

    void ClearArray()
    {
        SAFE_DELETE_ARRAY(pBuffer);
        length = 0;
    }

    bool WriteToStream(PacketStream *pStream)
    {
        pStream->WriteInt32(length);
         PhotoItem *p = pBuffer;
        for(int i = 0; i < length; i++)
        {
            p->WriteToStream(pStream);
            p++;
        }
        return true;
    }
    bool ReadFromStream(PacketStream *pStream)
    {
        ClearArray();
        pStream->ReadInt32(length);
        if (length > 0)
        {
            pBuffer = new PhotoItem[length];
        }
         PhotoItem *p = pBuffer;
        for(int i = 0; i < length; i++)
        {
            p->ReadFromStream(pStream);
            p++;
        }
        return true;
    }


  
};

#endif // __ARRAYOFPHOTOITEM_H__

