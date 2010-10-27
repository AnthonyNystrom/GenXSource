#ifndef __ARRAYOFDASHBOARDWALLCOMMENT_H__
#define __ARRAYOFDASHBOARDWALLCOMMENT_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "DashboardWallComment.h"

struct ArrayOfDashboardWallComment 
{
    int32 length;
    DashboardWallComment *pBuffer;
    
    ArrayOfDashboardWallComment ()
    {
        length = 0;
    }
    ArrayOfDashboardWallComment (int32 len)
    {
        length = len;
        pBuffer = new DashboardWallComment[len];
    }
    ArrayOfDashboardWallComment (DashboardWallComment *buffer, int32 len)
    {
        length = len;
        pBuffer = buffer;
    }
    ~ArrayOfDashboardWallComment ()
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
         DashboardWallComment *p = pBuffer;
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
            pBuffer = new DashboardWallComment[length];
        }
         DashboardWallComment *p = pBuffer;
        for(int i = 0; i < length; i++)
        {
            p->ReadFromStream(pStream);
            p++;
        }
        return true;
    }


  
};

#endif // __ARRAYOFDASHBOARDWALLCOMMENT_H__

