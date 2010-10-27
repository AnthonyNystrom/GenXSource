#ifndef __I_NET_CLIENT_H__
#define __I_NET_CLIENT_H__

// Интерфейс для платформозависимого кода работы с сетью

#include "BaseTypes.h"

class INetClient
{
public:

	virtual bool Request(const uint8 *data, uint32 sz/*, bool isUpdateRequest*/)	=	NULL;
	//virtual bool Init(const char8 *addr)				=	NULL;

	virtual ~INetClient() {};
};

#endif // __I_NET_CLIENT_H__