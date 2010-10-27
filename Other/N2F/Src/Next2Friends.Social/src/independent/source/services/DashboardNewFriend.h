#ifndef __DASHBOARDNEWFRIEND_H__
#define __DASHBOARDNEWFRIEND_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "DashboardNewFriend.h"

struct DashboardNewFriend 
{
    char16* DateTime;
    char16* Nickname1;
    char16* Nickname2;
    bool WriteToStream(PacketStream *pStream)
    {
          pStream->WriteWString(DateTime);
          pStream->WriteWString(Nickname1);
          pStream->WriteWString(Nickname2);
        return true;
    }

    bool ReadFromStream(PacketStream *pStream)
    {
        DateTime = pStream->GetWString();
        Nickname1 = pStream->GetWString();
        Nickname2 = pStream->GetWString();
        return true;
    }
    ~DashboardNewFriend()
    {
        SAFE_DELETE(DateTime);
        SAFE_DELETE(Nickname1);
        SAFE_DELETE(Nickname2);
    }
};

#endif // __DASHBOARDNEWFRIEND_H__

