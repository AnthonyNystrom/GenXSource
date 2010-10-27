#include "PacketStream.h"
// #include "Storage.h"

#define SYMBOL(ch) L##ch


PacketStream::PacketStream(int32 sz)
:	buffer	(NULL)
,	size	(sz)
{
	buffer = new uint8 [sz];
}

PacketStream::~PacketStream()
{
	SAFE_DELETE_ARRAY(buffer);
}

bool PacketStream::WriteInt8(int8 val)
{
	if (size <= tail)
		return false;

	buffer[tail++] = val;

	return true;
}

bool PacketStream::WriteInt16(int16 val)
{
	if (size <= tail + 1) 
		return false;

	buffer[tail++]	= uint8(val & 0xFF);
	buffer[tail++]	= uint8((val >> 8) & 0xFF);

	return true;
}

bool PacketStream::WriteInt32(int32 val)
{
	if (size <= tail + 3) 
		return false;

	buffer[tail++]	= uint8(val & 0xFF);
	buffer[tail++]	= uint8((val >> 8) & 0xFF);
	buffer[tail++]	= uint8((val >> 16) & 0xFF);
	buffer[tail++]	= uint8((val >> 24) & 0xFF);

	return true;
}

bool PacketStream::WriteInt64(int64 val)
{
	if (size <= tail + 7) 
		return false;

	buffer[tail++]	= uint8(val & 0xFF);
	buffer[tail++]	= uint8((val >> 8) & 0xFF);
	buffer[tail++]	= uint8((val >> 16) & 0xFF);
	buffer[tail++]	= uint8((val >> 24) & 0xFF);
	buffer[tail++]	= uint8((val >> 32) & 0xFF);
	buffer[tail++]	= uint8((val >> 40) & 0xFF);
	buffer[tail++]	= uint8((val >> 48) & 0xFF);
	buffer[tail++]	= uint8((val >> 56) & 0xFF);

	return true;
}

bool PacketStream::WriteBuffer(const uint8 *buff, int32 sz)
{
	int32 end = tail + sz;

	if (size <= end) 
		return false;

	while (tail < end)
	{
		buffer[tail++] = *buff++;
	}

	return true;
}

bool PacketStream::WriteString(const char8 *str)
{
	int32 end = tail + Utils::StrLen(str) + 1;

	if (size <= end) 
		return false;

	while (tail < end)
	{
		buffer[tail++] = *str++;
	}

	return true;
}

bool PacketStream::WriteWString(const char16 *str)
{
	int32 end = tail + (Utils::WStrLen(str) + 1) * 2;

	if (size <= end) 
		return false;

	char16 c;
	while (tail < end)
	{
		c = *str;
		//if(SYMBOL('\'') == c)
		//{
		//	c = SYMBOL(']');
		//}
		buffer[tail++]	= uint8(c & 0xFF);
		buffer[tail++]	= uint8((c >> 8) & 0xFF);

		str++;
	}

	tail = end;

	return true;
}

bool PacketStream::WritePacket(const PacketStream &packet)
{
	return WriteBuffer(packet.GetDataBuffer(), packet.GetDataSize());
}

bool PacketStream::WriteDate(int16 year, int8 month, int8 day)
{
	return (WriteInt16(year) 
		&&	WriteInt8(month) 
		&&	WriteInt8(day));
}

bool PacketStream::WriteTime(int8 hour, int8 min, int8 sec)
{
	return (WriteInt8(hour) 
		&&	WriteInt8(min)
		&&	WriteInt8(sec));
}

bool PacketStream::WriteDateTime(int16 year, int8 month, int8 day, int8 hour, int8 min, int8 sec)
{
	return (WriteInt16(year)
		&&	WriteInt8(month)
		&&	WriteInt8(day)
		&&	WriteInt8(hour)
		&&	WriteInt8(min)
		&&	WriteInt8(sec));
}

bool PacketStream::ReadInt8(int8 &var)
{
	if (tail <= head) 
		return false;

	var = buffer[head++];

	return true;
}

bool PacketStream::ReadInt16(int16 &var)
{
	if (tail <= head + 1) 
		return false;

	var = (buffer[head + 1] << 8) | (buffer[head]);
	head += 2;

	return true;
}

bool PacketStream::ReadInt32(int32 &var)
{
	if (tail <= head + 3) 
		return false;

	var =	(buffer[head + 3] << 24) 
		|	(buffer[head + 2] << 16) 
		|	(buffer[head + 1] << 8) 
		|	(buffer[head]);

	head += 4;

	return true;
}

bool PacketStream::ReadInt64(int64 &var)
{
	if (tail <= head + 7) 
		return false;

	var =	((int64)buffer[head + 7] << 56) 
		|	((int64)buffer[head + 6] << 48) 
		|	((int64)buffer[head + 5] << 40) 
		|	((int64)buffer[head + 4] << 32) 
		|	((int64)buffer[head + 3] << 24) 
		|	((int64)buffer[head + 2] << 16) 
		|	((int64)buffer[head + 1] << 8) 
		|	((int64)buffer[head]);

	head += 8;

	return true;
}

bool PacketStream::ReadBuffer(uint8 *buff, int32 sz)
{
	int32 end = head + sz/* - 1*/;
	if (tail <= end) 
		return false;

	while(head < end)
	{
		*buff++ = buffer[head++];
	}

	return true;
}

bool PacketStream::ReadString(char8 *str, int32 maxSz)
{
	int32 end = head + MIN(maxSz, GetFreeSize());
	for (int32 i = head; i < end; ++i)
	{
		if (0 == buffer[i]) // end if string found
		{
			while(head <= i)
			{
				*str++ = buffer[head++];
			}

			return true;
		}
	}

	return false;
}

bool PacketStream::ReadWString(char16 *str, int32 maxSz)
{
	int32 end = head + MIN(maxSz * 2, GetFreeSize() / 2 * 2);
	for (int32 i = head; i < end; i += 2)
	{
		char16 ch = (buffer[i + 1] << 8) | buffer[i];
		if (0 == ch) // end if string found
		{
			while(head <= i)
			{
				char16 c = (buffer[head + 1] << 8) | buffer[head];

				if(SYMBOL(']') == c)
				{
					c = SYMBOL('\''); 
				}
				*str++ = c;
				head += 2;
			}


			return true;
		}
	}

	return false;
}

char16	* PacketStream::GetWString()
{
	int32 end = tail / 2 * 2;
	int32 strSize = 0;
	for (int32 i = head; i < end; i += 2)
	{
		char16 ch = (buffer[i + 1] << 8) | buffer[i];
		strSize++;
		if (0 == ch) // end if string found
		{
			char16 *str = new char16[strSize];
			char16 *ptr = str;
			while(head <= i)
			{
				char16 c = (buffer[head + 1] << 8) | buffer[head];
				*ptr++ = c;
				head += 2;
			}


			return str;
		}
	}
	return NULL;
}

void PacketStream::Reset()
{
	tail = 0;
	head = 0;
}

bool PacketStream::ReadDate(int16 &year, int8 &month, int8 &day)
{
	return (ReadInt16(year)
		&&	ReadInt8(month)
		&&	ReadInt8(day));
}

bool PacketStream::ReadTime(int8 &hour, int8 &min, int8 &sec)
{
	return (ReadInt8(hour)
		&&	ReadInt8(min)
		&&	ReadInt8(sec));
}

bool PacketStream::ReadDateTime(int16 &year, int8 &month, int8 &day, int8 &hour, int8 &min, int8 &sec)
{
	return (ReadInt16(year)
		&&	ReadInt8(month)
		&&	ReadInt8(day)
		&&	ReadInt8(hour)
		&&	ReadInt8(min)
		&&	ReadInt8(sec));
}

// bool PacketStream::ReadDateTime(ARDateTime &dt)
// {
// 	return (ReadInt16(dt.year)
// 		&&	ReadInt8(dt.month)
// 		&&	ReadInt8(dt.day)
// 		&&	ReadInt8(dt.hour)
// 		&&	ReadInt8(dt.min)
// 		&&	ReadInt8(dt.sec));
//}