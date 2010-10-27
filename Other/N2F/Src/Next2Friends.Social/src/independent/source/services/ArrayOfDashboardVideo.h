#ifndef __ARRAYOFDASHBOARDVIDEO_H__
#define __ARRAYOFDASHBOARDVIDEO_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "DashboardVideo.h"

struct ArrayOfDashboardVideo 
{
    int32 length;
    DashboardVideo *pBuffer;
    
    ArrayOfDashboardVideo ()
    {
        length = 0;
    }
    ArrayOfDashboardVideo (int32 len)
    {
        length = len;
        pBuffer = new DashboardVideo[len];
    }
    ArrayOfDashboardVideo (DashboardVideo *buffer, int32 len)
    {
        length = len;
        pBuffer = buffer;
    }
    ~ArrayOfDashboardVideo ()
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
         DashboardVideo *p = pBuffer;
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
            pBuffer = new DashboardVideo[length];
        }
         DashboardVideo *p = pBuffer;
        for(int i = 0; i < length; i++)
        {
            p->ReadFromStream(pStream);
            p++;
        }
        return true;
    }


  
};

#endif // __ARRAYOFDASHBOARDVIDEO_H__

