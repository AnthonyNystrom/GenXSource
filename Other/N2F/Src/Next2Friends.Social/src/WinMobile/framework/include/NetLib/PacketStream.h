/*! =================================================================
	\file PacketStream.h
	
	Revision History:
	
	[12.11.2007] 16:53 by Victor Kleschenko
	\changed	Renamed ReadStr and ReadWStr. 
				Added work with Date, Time and DateTime types.

    ================================================================= */

#ifndef __PACKET_STREAM_H__
#define __PACKET_STREAM_H__

// Класс предоставляет возможность записывать/читать данные в/из пакета

#include "BaseTypes.h"
// #include "Storage.h"

// struct DataTime;

class PacketStream
{
	enum eConst
	{	
		DEFAULT_PACKET_SIZE		=	100*1024
	};

private:

	uint8	*buffer;	// byte buffer
	int32	size;		// buffer size
	int32	head;		// first element position
	int32	tail;		// next behind last element position

public:

	PacketStream	(int32 sz = DEFAULT_PACKET_SIZE);
	~PacketStream	();

	bool	WriteInt8		(int8 val);
	bool	WriteInt16		(int16 val);
	bool	WriteInt32		(int32 val);
	bool	WriteInt64		(int64 val);
	bool	WriteBuffer		(const uint8 *buff, int32 sz);
	bool	WriteString		(const char8 *str);
	bool	WriteWString	(const char16 *str);
	bool	WritePacket		(const PacketStream &packet);
	bool	WriteDate		(int16 year, int8 month, int8 day);
	bool	WriteTime		(int8 hour, int8 min, int8 sec);
	bool	WriteDateTime	(int16 year, int8 month, int8 day, int8 hour, int8 min, int8 sec);



	bool	ReadInt8		(int8 &var);
	bool	ReadInt16		(int16 &var);
	bool	ReadInt32		(int32 &var);
	bool	ReadInt64		(int64 &var);
	bool	ReadBuffer		(uint8 *buff, int32 sz);
	bool	ReadString		(char8 *str, int32 maxSz);
	bool	ReadWString		(char16 *str, int32 maxSz);
	char16	*GetWString		();
	bool	ReadDate		(int16 &year, int8 &month, int8 &day);
	bool	ReadTime		(int8 &hour, int8 &min, int8 &sec);
	bool	ReadDateTime	(int16 &year, int8 &month, int8 &day, int8 &hour, int8 &min, int8 &sec);
// 	bool	ReadDateTime	(ARDateTime &dt);

	// TODO: проверки
	void	SeekHead		(int32 pos)		{	head = pos;	}
	void	SeekTail		(int32 pos)		{	tail = pos;	}

	void	Reset();

	int32	GetFreeSize() const		{	return size - tail;		};
	int32	GetDataSize() const		{	return tail - head;		};
	int32	GetMaxSize() const		{	return size;			};

	uint8*	GetDataBuffer() const	{	return &buffer[head];	};

	int32	GetHead() const			{	return head;			}
	int32	GetTail() const			{	return tail;			}

private:

	// close copy constructor
	PacketStream	(const PacketStream &ps);
};

#endif // __PACKET_STREAM_H__

