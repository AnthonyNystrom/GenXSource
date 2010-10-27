#ifndef __FRAMEWORK_LIB_PHOTO_ITEM__
#define __FRAMEWORK_LIB_PHOTO_ITEM__

#include "BaseTypes.h"

#define MAX_PHOTO_NAME	50
#define MAX_PHOTO_PATH	(200 + MAX_PHOTO_NAME)

class PhotoLibrary;

struct PhotoDate
{
	int8 month;
	int8 day;
	int16 year;

	int8 hour;
	int8 minute;
};

enum ePhotoItemState
{
	EPIS_EMPTY = 0,
	EPIS_PHOTO,
	EPIS_UNCONFIRMED
};

class LibPhotoItem
{
public:


	LibPhotoItem();
	virtual ~LibPhotoItem();

	const PhotoDate *GetDate() const;
	const char16 *GetName() const;
	const char8 *GetID() const;
	int32 GetIDSize() const;
	int32 GetSize() const;
	ePhotoItemState GetState() const;



	void SetDate(const PhotoDate &newDate);
	void SetName(const char16 *newName);
	void SetName(const char8 *newName);
	void SetID(const char8 *newID, int32 sizeInBytes);
	void SetSize(int32 newSize);
	void SetState(ePhotoItemState newState);
	LibPhotoItem&	operator = (const LibPhotoItem &anItem);
	bool		operator == ( const LibPhotoItem &anItem ) const;
	bool		operator != ( const LibPhotoItem &anItem ) const;
protected:


private:

	char8 id[MAX_PHOTO_PATH * 2];
	int32 idSize;
	char16 name[MAX_PHOTO_NAME];
	int32 size;
	PhotoDate date;
	ePhotoItemState state;

};
#endif