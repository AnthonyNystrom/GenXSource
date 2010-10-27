/*!
@file	Application.h
@brief	Class Application
*/

#ifndef __FRAMEWORK_APPLICATION_H__
#define __FRAMEWORK_APPLICATION_H__

#include "Utils.h"
#include "IApplicationCore.h"
#include "Config.h"


// Replay settings
#ifdef AEE_SIMULATOR
//! Replay file directory (on win32)
#	define WRITE_PLAYBACK_DIR		"C:\\DBGOUT"
//! Replay file for recording
#	define WRITE_PLAYBACK_FILE		"C:\\DBGOUT\\demo.plb"
#else
//! Replay file for recording (on device)
#	define WRITE_PLAYBACK_FILE		"demo.plb"
#endif
//! Replay file for playback
#define READ_PLAYBACK_FILE			"demo.plb"

//! Max global pointers count (needed for Singleton implementation)
#define GLOBALPOINTER_COUNT				10


//uncomment this define for enable unit-test system
//#define UNIT_TESTING_SYSTEM 

#ifdef UNIT_TESTING_SYSTEM

	//file name for screen crc32 
	#define OUT_CRC_FILE				"screen.crc"

	//file name for unit testing state flag 
	#define FLAG_FILE				"flag.info"

#endif //UNIT_TESTING_SYSTEM


class FileSystem;
class File;
class ResourceSystem;
class GraphicsSystem;
class SoundSystem;

//! Class connecting user layer and OS layer
class Application
{
public:

	//! Keys codes
	enum eKeyCode
	{
		EKC_NONE	=	-1,

		EKC_0,
		EKC_1,
		EKC_2,
		EKC_3,
		EKC_4,
		EKC_5,
		EKC_6,
		EKC_7,
		EKC_8,
		EKC_9,
		EKC_STAR,
		EKC_POUND,

		EKC_POWER,
		EKC_END,
		EKC_SEND,
		EKC_CLR,

		EKC_UP,
		EKC_DOWN,
		EKC_LEFT,
		EKC_RIGHT,
		EKC_SELECT,

		EKC_SOFT1,
		EKC_SOFT2,
		EKC_SOFT3,
		EKC_SOFT4,

		EKC_UNKNOWN,

		EKC_COUNT
	};

	//! Color schemes
	enum eColorScheme
	{
		ECS_332,
		ECS_444,
		ECS_555,
		ECS_565,
		ECS_666,
		ECS_888,
		ECS_UNKNOWN
	};

	//! Exceptions
	enum eException
	{
		EE_NONE		=	0, //!< No exception
		EE_MEMORY_ALLOC, //!< Memory allocation error
		EE_FILE_WRITE, //!< File writing error
		EE_FILE_READ, //!< File reading error
		EE_UNKNOWN
	};

	//! Screen structure
	struct Display
	{
		void			*pBmp;
		uint16			width;
		uint16			height;
		int32			pitch;
		uint32			depth;
		eColorScheme	colorScheme;
	};

	//! Playback status
	enum ePlaybackState
	{
		EPS_NONE,	//!< off
		EPS_READ,	//!< playback
		EPS_WRITE	//!< record
	};

	Application();
	virtual ~Application();

	//! @brief Closes application
	virtual void		CloseApplication() = 0;
	
	//! @brief	Get physical display count
	//! @return display count
	virtual uint32 		GetDisplayCount() = 0;

	//! @brief	Get display array pointer @ref Display
	//! @return pointer to @ref Display pointers array
	virtual Display*	GetDisplayArray() = 0;
	
	//! @brief	Checks if key is pressed
	//! @param[in] keyCode - @ref eKeyCode
	bool				IsKeyPressed(eKeyCode keyCode);

	//! @brief	Checks if key was pressed during last frame
	//! @param[in] keyCode - @ref eKeyCode
	//! @return true if key was pressed during last frame
	bool				IsKeyDown(eKeyCode keyCode);

	//! @brief	Checks if key was released during last frame
	//! @param[in] keyCode - @ref eKeyCode
	//! @return true if key was released during last frame
	bool				IsKeyUp(eKeyCode keyCode);

	//! @brief	Checks if key is pressed
	//! @param[in] keyCode - @ref eKeyCode
	//! @return true once per "rate" frames if key is pressed
	//! "rate" is set by @ref SetRepeatRate(). 
	//! true is returned only for last pressed key
	bool				IsKeyRepeat(eKeyCode keyCode);

	//! @brief	Checks if any key is pressed now
	//! Attention! Slow implementation using <for> statement
	bool				IsAnyKeyPressed();

	//! @brief	Checks if any key is pressed/released/held now
	//! Attention! Slow implementation using <for> statement
	bool				IsAnyKey();

	//! @brief	Checks if mouse is pressed
	bool				IsMousePressed(int32 * x, int32 * y);
	bool				IsMouseDown(int32 * x, int32 * y);
	bool				IsMouseUp(int32 * x, int32 * y);

	//! @brief	Disables render on screen
	void				LockRender();

	//! @brief	Enables render on screen
	void				UnlockRender();

	//! @brief	Clears keys buffer, removes all "pressed/released/held" flags
	void				ClearKeys();
	
	//! @brief	Get frame start time
	uint32				GetApproximateTime();

	//! @brief  Set application minimum sleep time (time for system processes)
	void				SetMinSleepTime(int32 minSleepTime);

	//! @brief	Set frame time (1/frame rate)
	void				SetElapsedTime(int32 elapsedTime);

	//! @brief	Random generator(xorshift-128) initialization
	//! @param[in]		seed - random seed
	void				SetSeed(uint32 seed);

	//! @brief Get random integer from random generator(xorshift-128)
	//! @param[in] maxValue - maximum generated value
	//! @return random number in [0...maxValue-1] interval
	uint32				Random(uint32 maxVal);

	//! @brief Set rate for KeyRepeat
	//! @param[in] rate - new rate value
	void				SetRepeatRate(uint16 rate);

	//! @brief Set delay for first KeyRepeat
	//!
	//!	First time IsKeyRepeat() will return true after "delay" frames, 
	//! 2nd and all other times - after "delay" frames
	void				SetRepeatFirstDelay(uint16 delay);

	//! @brief Sets exception
	//!
	//! Exception is handled by framework after current Update() iteration.
	//! For all exceptions error message is displayed.
	//! Then if EE_MEMORY_ALLOC occurred, application closes.
	//! @param[in] e - exception type. 
	void				SetException(eException e);

	//! Get application file system
	FileSystem*			GetFileSystem();

	//! Get application graphics
	GraphicsSystem*		GetGraphicsSystem();

	//! Get application sound
	SoundSystem*		GetSoundSystem();

	//! Get application core
	IApplicationCore*	GetApplicationCore() { return appCore; }

	//! Create application resource system
	ResourceSystem*		CreateResourceSystem();

	//! @brief Opens replay file
	//! 
	//! Restores storage.
	//! On every frame, before Update(), state of keyboard buffer is restored.
	//! @param[in] saveData - pointer to application data buffer
	//! @param[in] dataSz - application data buffer size
	virtual bool StartPlaybackRead(void *saveData, uint32 dataSz);

	//! @brief Record replay file
	//!
	//! Opens file for writing and saves current Preferences.
	//! On every frame, before Update(), state of keyboard buffer is saved.
	//! If running on simulator, file is created in WRITE_PLAYBACK_DIR directory.
	//! @param[in] saveData - pointer to application data buffer
	//! @param[in] dataSz - application data buffer size
	virtual bool StartPlaybackWrite(void *saveData, uint32 dataSz);

	//! @brief Get playback state
	ePlaybackState GetPlaybackState();

	//! @brief Closes replay file
	virtual void ClosePlayback();

	//! @brief Saves pointer in application static section
	//! @param[in] index - pointer index
	//! @param[in] ptr - pointer for saving
	//! @return true if pointer successfully saved, false otherwise
	bool SetGlobalPointer(uint32 index, void *ptr);

	//! @brief Get pointer from application static section
	//! @param[in] index - pointer index
	//! @return saved pointer
	void* GetGlobalPointer(uint32 index);

	// @brief Сохранить пользовательские данные в playback-файл.
	// Должна использоваться симметрично с функцией LoadUserDataToPlayback.
	// @param[in] data - указатель на буфер пользовательских данных.
	// @param[in] size - размер буфера пользовательских данных.
	virtual bool SaveUserDataToPlayback(void *data, uint32 size);

	// @brief Загрузить пользовательские данные из playback-файла.
	// Должна использоваться симметрично с функцией SaveUserDataToPlayback.
	// @param[in] data - указатель на буфер пользовательских данных.
	// @param[in] size - размер буфера пользовательских данных.
	virtual bool LoadUserDataToPlayback(void *data, uint32 size);

	// @brief Рассчитать CRC32 по входному буферу.
	// @param[in] data - указатель на буфер данных по которому производиться расчёт.
	// @param[in] size - размер буфера данных.
	uint32 Calculate_CRC32(uint8* data, uint32 size);


	void InitGraphicSystem(uint32 depth)
	{
		if (16 == depth)
		{
#ifndef GRAPHICS16_REDUCE
			CreateGraphics16();
#else // GRAPHICS16_REDUCE
			UTILS_LOG(EDMP_ERROR, "Wrong definition in config.h: GRAPHICS16_REDUCE");			
#endif // GRAPHICS16_REDUCE
		}
		else if (32 == depth)
		{
#ifndef GRAPHICS32_REDUCE
			CreateGraphics32();
#else // GRAPHICS32_REDUCE
			UTILS_LOG(EDMP_ERROR, "Wrong definition in config.h: GRAPHICS32_REDUCE");			
#endif // GRAPHICS32_REDUCE
		}
	}



protected:

	static const int32	DEFAULT_DELAY			=	20;
	static const int32	DEFAULT_REPEAT_RATE		=	1;
	static const int32	ALPHA_KEYS_BUFFER_SZ	=	10;

	int32 minSleepTime;
	int32 elapsedTime;

	bool isLockedRender;

	virtual void		OnUpdate();
	virtual bool		OnInitApplication();
	virtual void		OnFreeApplication();

	virtual bool		OnKeyDown(uint16 key);
	virtual bool		OnKeyUp(uint16 key);
	virtual bool		OnKey(uint16 key);

	virtual bool		OnMouseDown(int32 x, int32 y);
	virtual bool		OnMouseUp(int32 x, int32 y);

	virtual void		OnSuspend();
	virtual void		OnResume();

	virtual void		ReadPlaybackKeys();
	virtual void		WritePlaybackKeys();

	virtual void		ExceptionHandling()		=	0;

	uint32				approximateTime;

	IApplicationCore*	appCore;
	FileSystem*			fileSystem;
	SoundSystem*		soundSystem;
	GraphicsSystem*		graphicsSystem;

	eException			exception;

	uint8				keyArray[EKC_COUNT];

	eKeyCode			repeatKey;
	uint16				repeatRate;
	int32				repeatCount;
	uint16				repeatDelay;

	uint16				alphaKeysBuffer[ALPHA_KEYS_BUFFER_SZ];
	int32				alphaKeysCount;

	File*				playbackReadFile;
	int32				playbackPos;
	ePlaybackState		playbackState;


	void*				globalPointerList[GLOBALPOINTER_COUNT];


	//for mouse & touchpad support
	int32 mouseXPos;
	int32 mouseYPos;
	uint8 mouseState;

	enum eKeyState
	{
		EKS_DOWN	= 0x01,
		EKS_UP		= 0x02,
		EKS_PRESSED	= 0x04
	};


private:



#ifndef GRAPHICS16_REDUCE
	void CreateGraphics16();
#else // GRAPHICS16_REDUCE
	void CreateGraphics16(){}
#endif // GRAPHICS16_REDUCE

#ifndef GRAPHICS32_REDUCE
	void CreateGraphics32();
#else // GRAPHICS32_REDUCE
	void CreateGraphics32(){}
#endif // GRAPHICS32_REDUCE


	void				KeyUpdateEndframe();

	uint32				nSeed;
	uint32				rndX;
	uint32				rndY;
	uint32				rndZ;
	uint32				rndW;
	uint32				*crcTable;

#ifdef UNIT_TESTING_SYSTEM

	void				WriteCRCExit();
	enum eUnitTestSystemState
	{
		EUTSS_UNKNOW = 0,
		EUTSS_READ_EXIT,
		EUTSS_READ,
		EUTSS_WRITE
	};
	eUnitTestSystemState	testState;
#endif //UNIT_TESTING_SYSTEM

};

Application* GetApplication();

#endif//__FRAMEWORK_APPLICATION_H__