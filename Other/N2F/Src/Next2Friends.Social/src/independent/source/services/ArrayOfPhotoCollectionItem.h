#ifndef __ARRAYOFPHOTOCOLLECTIONITEM_H__
#define __ARRAYOFPHOTOCOLLECTIONITEM_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "PhotoCollectionItem.h"

struct ArrayOfPhotoCollectionItem 
{
    int32 length;
    PhotoCollectionItem *pBuffer;
    
    ArrayOfPhotoCollectionItem ()
    {
        length = 0;
    }
    ArrayOfPhotoCollectionItem (int32 len)
    {
        length = len;
        pBuffer = new PhotoCollectionItem[len];
    }
    ArrayOfPhotoCollectionItem (PhotoCollectionItem *buffer, int32 len)
    {
        length = len;
        pBuffer = buffer;
    }
    ~ArrayOfPhotoCollectionItem ()
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
         PhotoCollectionItem *p = pBuffer;
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
            pBuffer = new PhotoCollectionItem[length];
        }
         PhotoCollectionItem *p = pBuffer;
        for(int i = 0; i < length; i++)
        {
            p->ReadFromStream(pStream);
            p++;
        }
        return true;
    }


  
};

#endif // __ARRAYOFPHOTOCOLLECTIONITEM_H__

