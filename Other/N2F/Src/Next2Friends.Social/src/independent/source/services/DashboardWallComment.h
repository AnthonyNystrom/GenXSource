#ifndef __DASHBOARDWALLCOMMENT_H__
#define __DASHBOARDWALLCOMMENT_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "DashboardWallComment.h"

struct DashboardWallComment 
{
    char16* DateTime;
    char16* Nickname1;
    char16* Nickname2;
    char16* Text;
    bool WriteToStream(PacketStream *pStream)
    {
          pStream->WriteWString(DateTime);
          pStream->WriteWString(Nickname1);
          pStream->WriteWString(Nickname2);
          pStream->WriteWString(Text);
        return true;
    }

    bool ReadFromStream(PacketStream *pStream)
    {
        DateTime = pStream->GetWString();
        Nickname1 = pStream->GetWString();
        Nickname2 = pStream->GetWString();
        Text = pStream->GetWString();
        return true;
    }
    ~DashboardWallComment()
    {
        SAFE_DELETE(DateTime);
        SAFE_DELETE(Nickname1);
        SAFE_DELETE(Nickname2);
        SAFE_DELETE(Text);
    }
};

#endif // __DASHBOARDWALLCOMMENT_H__

