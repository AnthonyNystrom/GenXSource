/*!
@file FileSystem.h
@brief FileSystem and File classes
*/

#ifndef __FRAMEWORK_FILESYSTEM_H__
#define __FRAMEWORK_FILESYSTEM_H__

#include "Utils.h"

//! Maximum file name lengths
#define MAX_FILENAME_SIZE 255 


//! File class. It is created by FileSystem.
class File
{
	friend class FileSystem;

public:

	//! Initial position for point movement functions in files.
	enum eSeekOrigin
	{
		ESO_START,		//!< Beginning of file.
		ESO_END,		//!< End of file.
		ESO_CURRENT		//!< Current position of file pointer.
	};

	//! File opening mode.
	enum eFileMode
	{
		EFM_READ,		//!< Read.
		EFM_READWRITE,	//!< Read/write.
		EFM_CREATE,		//!< Create.
		EFM_APPEND		//!< Append.
	};

	//! Reads data from the file.
	//! @param[in] srcBuffer	- Data storage location.
	//! @param[in] size			- Size in bytes to be read.
	//! @return The number of bytes actually read.
	uint32	Read(void *dstBuffer, uint32 size);

	//! Writes data to the file.
	//! @param[in] dstBuffer	- Pointer to data to be written.
	//! @param[in] size			- Size in bytes to be written.
	//! @return The number of bytes actually written.
	uint32	Write(void *srcBuffer, uint32 size);

	//! Moves the file pointer to a specified location.
	//! @param[in] offset		- Number of bytes from origin.
	//! @param[in] origin		- Initial position.
	//! @return	true if successful, or false if failed.
	bool	Seek(uint32 offset, eSeekOrigin origin);

	//! Closes the file.
	void	Release();

	//! Gets the file size.
	uint32	GetSize();

	//! Gets the file name.
	const char8 *GetName();

private:

	uint32			fileSize;						//!< File size
	char8 *			name;
	void			*handler;						//!< File data handler

	File(void *handler);
};

//! File system class.
class FileSystem
{
public:
	FileSystem();
	~FileSystem();
/*
	//! Finds the first instance of a filename that matches the file specified in the filename argument
	//! @param[in] filename	- Target file specification (may include wildcards).
	//! @return	If successful, returns true, else false.
	bool	FindFirst(const char *filename);

	//! Find the next name, if any, that matches the filename argument in a previous call to FindFirst.
	//! @return If successful, returns true, else false.
	bool	FindNext();
*/
	//! Removes specified file.
	//! @param[in] filename	- The name of the file to be removed.
	//! @return If successful, returns true, else false.
	bool	Remove(const char8 *filename);

	//! Open specified file.
	//! @param[in] filename	The name of the file to open.
	//! @param[in] format	File opening mode @ref eFileMode
	//! @return If successful, returns pointer on @ref File class that represents opened file, else returns NULL.
	File*	Open(const char8 *filename, File::eFileMode fileMode);

private:

	//uint16		nextFileHandler;				
	//uint32		fileSize;						
	//char8			fileName[MAX_FILENAME_SIZE];	
	void			*fileMgr;						
};

#endif // __FRAMEWORK_FILESYSTEM_H__