#ifndef __ARRAYOFDASHBOARDNEWFRIEND_H__
#define __ARRAYOFDASHBOARDNEWFRIEND_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "DashboardNewFriend.h"

struct ArrayOfDashboardNewFriend 
{
    int32 length;
    DashboardNewFriend *pBuffer;
    
    ArrayOfDashboardNewFriend ()
    {
        length = 0;
    }
    ArrayOfDashboardNewFriend (int32 len)
    {
        length = len;
        pBuffer = new DashboardNewFriend[len];
    }
    ArrayOfDashboardNewFriend (DashboardNewFriend *buffer, int32 len)
    {
        length = len;
        pBuffer = buffer;
    }
    ~ArrayOfDashboardNewFriend ()
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
         DashboardNewFriend *p = pBuffer;
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
            pBuffer = new DashboardNewFriend[length];
        }
         DashboardNewFriend *p = pBuffer;
        for(int i = 0; i < length; i++)
        {
            p->ReadFromStream(pStream);
            p++;
        }
        return true;
    }


  
};

#endif // __ARRAYOFDASHBOARDNEWFRIEND_H__

