#ifndef __DASHBOARDVIDEO_H__
#define __DASHBOARDVIDEO_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "DashboardVideo.h"

struct DashboardVideo 
{
    char16* DateTime;
    char16* Nickname;
    char16* Text;
    char16* Title;
    bool WriteToStream(PacketStream *pStream)
    {
          pStream->WriteWString(DateTime);
          pStream->WriteWString(Nickname);
          pStream->WriteWString(Text);
          pStream->WriteWString(Title);
        return true;
    }

    bool ReadFromStream(PacketStream *pStream)
    {
        DateTime = pStream->GetWString();
        Nickname = pStream->GetWString();
        Text = pStream->GetWString();
        Title = pStream->GetWString();
        return true;
    }
    ~DashboardVideo()
    {
        SAFE_DELETE(DateTime);
        SAFE_DELETE(Nickname);
        SAFE_DELETE(Text);
        SAFE_DELETE(Title);
    }
};

#endif // __DASHBOARDVIDEO_H__

