/*!
@file	ResourceSystem.h
@brief	Class ResourceSystem
*/
#ifndef __FRAMEWORK_RESOURCESYSTEM_H__
#define __FRAMEWORK_RESOURCESYSTEM_H__

#include "FileSystem.h"
#include "SoundSystem.h"
#include "Resource.h"
#include "Sprite.h"
#include "Font.h"
#include "Graphics.h"
#include "String.h"
#include "ArrayList.h"
#include "Binary.h"


class ResourceSystem;

//! Resource handling class
class ResourceSystem
{

public:
	//! @brief Constructor
	//!
	//! If some of subsystems are not used, for example sound, give 0 as corresponding parameter
	//! @param[in] fileSys - pointer to created FileSystem
	//! @param[in] soundSys - pointer to created SoundSystem
	//! @param[in] graphicsSys - pointer to created GraphicsSystem
	ResourceSystem(FileSystem * fileSys, SoundSystem * soundSys, GraphicsSystem * graphicsSys);

	//! @brief Destructor
	~ResourceSystem();

	//! @brief Create sprite from archive
	//! @param[in] resourceID - resource's ID from ResourcePacker utility
	Sprite	* CreateSprite(int16 resourceID);

	//! @brief Create sprite from file
	//! @param[in] fileName - resource file name
	Sprite	* CreateSprite(char8 *fileName);

	//! @brief Create string from archive
	//! @param[in] resourceID - resource's ID from ResourcePacker utility
	String  * CreateString(int16 resourceID);

	//! @brief Create writable string
	//! @param[in] size - string character count (for string with length len you need size = len+1)
	String  * CreateWritableString(uint32 size);

	//! @brief Create font from archive
	//! @param[in] spriteID - resource's ID from ResourcePacker utility
	//! @param[in] binaryID - resource's ID from ResourcePacker utility
	Font	* CreateFont(int16 spriteID, int16 binaryID);
	//Font	* CreateFont(char8 *fileName);

	//! @brief Create sound from archive
	//! @param[in] resourceID - resource's ID from ResourcePacker utility
	Sound	* CreateSound(int16 resourceID);

	//! @brief Create sound from file
	//! @param[in] fileName - resource file name
	Sound	* CreateSound(char8 *fileName);

	//! @brief Create binary resource from archive
	//! @param[in] resourceID - resource's ID from ResourcePacker utility
	Binary	* CreateBinary(int16 resourceID);

	//! @brief Create binary resource from file
	//! @param[in] fileName - resource file name
	Binary	* CreateBinary(char8 *fileName);

	//! @brief Create 2D array from archive
	//! @param[in] resourceID - resource's ID from ResourcePacker utility
	ArrayList * CreateArrayList(int16 resourceID);

	//! @brief Open archive file
	//! @param[in] resourceFileName - archive file name
	//! @return true if file opened successfully, false otherwise
	bool Open(const char8 * resourceFileName);


	//! @brief Close archive file
	void Close();

	//! @brief Destroy resource system
	//!
	//! Function needed for compatibility
	void Release();

private:

	struct ResourceNode
	{
		Resource		* data;
		uint32			resourceID;
		char8			* resourceName;
		Resource::eType type;
		ResourceNode	* next;
	};

	enum eResourceFlags
	{
		ERF_STRING	= 0x01,
		ERF_BINARY	= 0x02,
		ERF_ASCII	= 0x04,
		ERF_UNICODE	= 0x08,
		ERF_PACKED	= 0x10
	};

	//! @brief Функция предназначенная для добавления ресурса в ресурс менеджер
	//! Эта функция вызывается из класса ресурс в его конструкторе, для добавления
	//! класса в список ресурсов которые уже загружены. 
	//! @param[in] resource ресурс 
	//! @param[in] resourceId ID ресурса 
	void	RegisterResource(Resource * resource, int16 resourceId, Resource::eType type);

	//! @brief Функция предназначенная для добавления ресурса в ресурс менеджер
	//! Эта функция вызывается из класса ресурс в его конструкторе, для добавления
	//! класса в список ресурсов которые уже загружены. 
	//! @param[in] resource ресурс 
	//! @param[in] resourceName имя ресурса 
	void	RegisterResource(Resource * resource, char8 * resourceName, Resource::eType type);

	//! @brief Функция предназначенная для добавления ресурса в ресурс менеджер
	//! Эта функция вызывается из класса ресурс в его деструкторе, для удаления
	//! класса из списока ресурсов.
	//! @param[in] resource ресурс 
	//! @param[in] resourceId ID ресурса 
	void	UnregisterResource(int16 resourceId, Resource::eType type);

	//! @brief Функция предназначенная для добавления ресурса в ресурс менеджер
	//! Эта функция вызывается из класса ресурс в его деструкторе, для удаления
	//! класса из списока ресурсов.
	//! @param[in] resource ресурс 
	//! @param[in] resourceName имя ресурса 
	void	UnregisterResource(char8 * resourceName, Resource::eType type);

	//! @brief Ищет запрашиваемых ресурс в списке уже созданных. Если нашел, 
	//! возвращает указатель на него, имначе NULL
	//! @param[in] resourceID - ID искомого ресурса
	Resource	* FindResource(	int16 resourceID,	
								Resource::eType type, 
								ResourceNode ** findedNode = 0);
	
	Resource	* FindResource(	char8 * resourceName,	
								Resource::eType type, 
								ResourceNode ** findedNode = 0);


	//! Function to create & load resource of any type
	//! Load resource [ResourceID] to ResourceBuf pointer
	//! ResourceBuf pointer created inside this function
	//! return [size of resource]
	uint32 CreateResource(int16 ResourceID, void ** ResourceBuf);

	//! Function to load resource of any type
	//! Load resource [ResourceID] to ResourceBuf pointer
	//! if (ResourceBuf == 0) function return size of resource
	uint32 LoadResource(int16 ResourceID, void * ResourceBuf);

	FileSystem	* fileSystem;
	SoundSystem * soundSystem;
	GraphicsSystem * graphicsSystem;

	File		* resFile;
	uint32      fileBeginShift;
	uint16		fileVersion;
	uint16		resourceCount;
	uint8		* dictionary;
	uint8		* hashPos;
	int16		userResourceID;		

	int32 LoadResourceFromDict(uint8 *dictPos, void * resourceBuf, int16 resourceID);

    ResourceNode * resourceList; 

	friend class Resource;
	friend class Font;
	friend class Sprite;
	friend class Sound;
};

#endif // __FRAMEWORK_RESOURCESYSTEM_H__

