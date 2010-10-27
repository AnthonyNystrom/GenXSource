/*!
@file	Sound.h
@brief	Sound class
*/
#ifndef __FRAMEWORK_SOUND_H__
#define __FRAMEWORK_SOUND_H__

#include "Config.h"
#include "Resource.h"
#include "Binary.h"

//! Sound resource class
class Sound : public Resource
{
protected:
	//! @brief Constructor
	//! Creates sound from the Binary resource. Use 'resID' or 'resName' for resource identification
	//! @param[in] _binary		- Binary sound resource
	//! @param[in] resSys		- Resource system pointer
	//! @param[in] resID		- Resource ID
	//! @param[in] resName		- Resource name
	Sound(Binary * _binary, ResourceSystem *resSys, int16 resID, char8 * resName);

	Binary		*binary;	//!< Binary sound resource
	void		*data;		//!< Sound data
	uint32		size;		//!< Sound data size
	uint8		prior;		//!< Sound priority

public:
	virtual ~Sound();

	//! @brief Start playing
	//! Playing continues from the pause point if a @ref Pause() was called before
	//! @param[in] loopCount	- Number of playing repeats, if 0 - playing repeats forever
	virtual void Play(int32 loopCount = 1) = 0;

	//! @brief Pause
	//! To continue playing call @ref Play()
	//! @note !!! For current moment this function is not work
	virtual void Pause() = 0;

	//! @brief Stop
	//! Stopping sound playing
	virtual void Stop() = 0;

	//! @brief Sets sound priority
	//! @param[in] prior		- Sound priority
	void SetPrior(uint8 prior);

	//! @brief Get sound priority
	//! @return					- Priority
	uint8 GetPrior();
};

#endif //__FRAMEWORK_SOUND_H__