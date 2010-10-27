#include "BasicServices.h"

#include "Framework/Utils.h"

BasicServices::BasicServices(void)
	:	sessionID(NULL_SESSION)
{
	headerSize = sizeof(int32) + sizeof(int32) + sizeof(int64);
}

BasicServices::~BasicServices(void)
{
}

bool BasicServices::InitSystem()
{
// 	bool inited = pos.InitStream() | pis.InitStream();
// 	return inited;
	return true;
}

void BasicServices::SetListener( IListener *listener )
{
	eventListener = listener;
}

void BasicServices::CreateHeader(int32 packetID, int32 packetSize)
{
	Utils::Log(EDMP_DEBUG, "BasicServices::CreateHeader: packetId = %d, packetSize = %d", packetID, packetSize + headerSize);

	pos.writeInt32(packetSize + headerSize);
	pos.writeInt32(packetID);
	pos.writeInt64(sessionID);
}

bool BasicServices::ParsePackets()
{
	if(!eventListener)
		return false;

	int32 datasize = pis.ReadInt32();
	while(datasize)
	{
		int32 packetID = pis.ReadInt32();
		int64 sessionID = pis.ReadInt64();

		switch(packetID)
		{
		case EPID_REGISTER:
			{
				EventRegister();
				break;
			}
		case EPID_LOGIN:
			{
				EventLogin();
				break;
			}
		case EPID_LOGOUT:
			{
				EventLogout();
				break;
			}

		default:
			Utils::Log(EDMP_ERROR, "BasicServices::ParsePacket: unknown packet = %d", packetID);
			return false;
		}

		datasize = pis.ReadInt32();
	}

	return true;
}


void BasicServices::RequestLogin( char16 *login, char16 *password )
{
	Utils::Log(EDMP_DEBUG, "BasicServices::requestLogin: login = %S, passw = %S", login, password);
	int32 dataSize = (Utils::WStrLen(login) + Utils::WStrLen(password) + 2) << 1;
 
	CreateHeader(EPID_LOGIN,dataSize);
	pos.writeWString(login);
	pos.writeWString(password);

	AddPacket2Query();
}

void BasicServices::RequestLogout()
{
	Utils::Log(EDMP_DEBUG, "BasicServices::requestLogout");

	PacketOutputStream pos;
// 	pos.InitStream();

	CreateHeader(EPID_LOGOUT, 0);

	AddPacket2Query();
}

void BasicServices::EventLogin()
{
// 	int64 sessId = pis.ReadInt64();
// 	int32 userId = pis.ReadInt32();

// 	if (sessId > NULL_SESSION)
// 	{
// 		sessionID = sessId;
// 	}

//	eventListener->EventLogin(sessId, userId);

	int8 res = pis.ReadInt8();
}

void BasicServices::EventLogout()
{
	eventListener->EventLogout();
}

bool BasicServices::ReadyToSend() const
{
	return readyToSend;
}

void BasicServices::ClearSendData()
{
	readyToSend = false;
	pos.Reset();
}

uint8 * BasicServices::GetPackets() const
{
	return pos.GetBuffer();
}

int32 BasicServices::GetPacketsSize() const
{
	return pos.GetSize();
}

void BasicServices::AddPacket2Query()
{
	readyToSend = true;
}

void BasicServices::BeginWrite()
{
	pis.Reset();
}

void BasicServices::EndWrite()
{
	int32 nullPacket = 0;
	pis.AppendBuffer((uint8 *)(&nullPacket), sizeof(int32));
	
	pis.Reset();
	ParsePackets();
}

bool BasicServices::WriteData( uint8 *buf, int32 size )
{
	return pis.AppendBuffer(buf, size);
}

void BasicServices::RequestRegister( char16 *login, char16 *password )
{
	Utils::Log(EDMP_DEBUG, "BasicServices::RequestRegister: login = %S, passw = %S", login, password);
	int32 dataSize = (Utils::WStrLen(login) + Utils::WStrLen(password) + 2) << 1;

	CreateHeader(EPID_REGISTER,dataSize);
	pos.writeWString(login);
	pos.writeWString(password);

	AddPacket2Query();
}

void BasicServices::EventRegister()
{
	eventListener->EventRegister(pis.ReadInt8());	
}