#ifndef __STRINGARRAY_H__
#define __STRINGARRAY_H__

#include "PacketStream.h"
#include "BaseTypes.h"
#include "StringArray.h"

struct StringArray 
{
    bool WriteToStream(PacketStream *pStream)
    {
        return true;
    }

    bool ReadFromStream(PacketStream *pStream)
    {
        return true;
    }
    ~StringArray()
    {
    }
};

#endif // __STRINGARRAY_H__

