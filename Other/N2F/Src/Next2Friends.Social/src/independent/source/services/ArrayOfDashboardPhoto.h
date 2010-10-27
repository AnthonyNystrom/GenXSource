#ifndef __ARRAYOFDASHBOARDPHOTO_H__
#define __ARRAYOFDASHBOARDPHOTO_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "DashboardPhoto.h"

struct ArrayOfDashboardPhoto 
{
    int32 length;
    DashboardPhoto *pBuffer;
    
    ArrayOfDashboardPhoto ()
    {
        length = 0;
    }
    ArrayOfDashboardPhoto (int32 len)
    {
        length = len;
        pBuffer = new DashboardPhoto[len];
    }
    ArrayOfDashboardPhoto (DashboardPhoto *buffer, int32 len)
    {
        length = len;
        pBuffer = buffer;
    }
    ~ArrayOfDashboardPhoto ()
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
         DashboardPhoto *p = pBuffer;
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
            pBuffer = new DashboardPhoto[length];
        }
         DashboardPhoto *p = pBuffer;
        for(int i = 0; i < length; i++)
        {
            p->ReadFromStream(pStream);
            p++;
        }
        return true;
    }


  
};

#endif // __ARRAYOFDASHBOARDPHOTO_H__

