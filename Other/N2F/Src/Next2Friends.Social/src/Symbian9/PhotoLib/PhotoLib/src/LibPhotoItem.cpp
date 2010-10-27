#include "PhotoLibrary.h"
#include "LibPhotoItem.h"

LibPhotoItem::LibPhotoItem()
{
	SetState(EPIS_EMPTY);
}

LibPhotoItem::~LibPhotoItem()
{

}

const PhotoDate * LibPhotoItem::GetDate() const
{
	return &date;
}

const char16 * LibPhotoItem::GetName() const
{
	return name;
}

const char8 * LibPhotoItem::GetID() const
{
	return id;
}
int32 LibPhotoItem::GetIDSize() const
{
	return idSize;
}

int32 LibPhotoItem::GetSize() const
{
	return size;
}


void LibPhotoItem::SetDate( const PhotoDate &newDate )
{
	date = newDate;
}

void LibPhotoItem::SetName( const char16 *newName )
{
	Utils::WStrCpy(name, newName);
}

void LibPhotoItem::SetName( const char8 *newName )
{
	Utils::StrToWstr(newName, name, MAX_PHOTO_NAME * 2);
}

void LibPhotoItem::SetID( const char8 *newID, int32 sizeInBytes )
{
	FASSERT(sizeInBytes < MAX_PHOTO_PATH * 2)
	idSize = sizeInBytes;
	for (int i = 0; i < sizeInBytes; i++)
	{
		id[i] = newID[i];
	}
}

void LibPhotoItem::SetSize( int32 newSize )
{
	size = newSize;
}

ePhotoItemState LibPhotoItem::GetState() const
{
	return state;
}

void LibPhotoItem::SetState( ePhotoItemState newState )
{
	state = newState;
}

LibPhotoItem& LibPhotoItem::operator=( const LibPhotoItem &anItem )
{
	SetDate(*anItem.GetDate());
	SetID(anItem.GetID(), anItem.GetIDSize());
	SetName(anItem.GetName());
	SetSize(anItem.GetSize());
	return *this;
}

bool LibPhotoItem::operator==( const LibPhotoItem &anItem ) const
{
	if (size != anItem.GetSize())
	{
		return false;
	}
	if (Utils::WStrCmp(name, anItem.GetName()))
	{
		return false;
	}
	return true;
}

bool LibPhotoItem::operator!=( const LibPhotoItem &anItem ) const
{
	return !(*this == anItem);
}
