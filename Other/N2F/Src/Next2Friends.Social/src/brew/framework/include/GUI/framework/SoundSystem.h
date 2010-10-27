#ifndef __FRAMEWORK_SOUNDSYS_H__
#define __FRAMEWORK_SOUNDSYS_H__

#include "Sound.h"

//! Class for work with sound
class SoundSystem
{
public:
	SoundSystem() : mute(false)
	{ };

	virtual ~SoundSystem()
	{ };

	//! @brief Initialization
	virtual void Init() = 0;

	//! @brief Sound creation
	//! Creates sound from the Binary resource. Use 'resID' or 'resName' for resource identification
	//! @param[in] _binary		- Binary sound resource
	//! @param[in] resSys		- Resource system pointer
	//! @param[in] resID		- Resource ID
	//! @param[in] resName		- Resource name
	//! @return					- sound resource pointer
	virtual Sound* CreateSound(Binary * binary, ResourceSystem *resSys, int16 resID, char8 * resName) = 0;

	//! @brief Set current channel
	//! @param[in] channel		- Channel number
	virtual void SetChannel(uint8 channel) = 0;

	virtual uint8 GetChannel()
	{
		return 0;
	}
	//! @brief Set current volume
	//! @param[in] volume		- Sound volume level (from SoundSystem.MIN_VOLUME to SoundSystem.MAX_VOLUME)
	virtual void SetVolume(uint8 volume) = 0;

	//! @brief For proper work this function must be called at the every frame
	virtual void Update() = 0;

	//! @brief Turns sound off/on
	//! @param[in] set_mute		- if true turns sound off. elsewhere turns sounds on
	virtual void SetMute(bool set_mute) { mute = set_mute; }

	//! @brief Get Mute state
	//! @return					- returns mute state
	virtual bool GetMute() { return mute; }

	//! @brief Get current playing sound
	//! @return					- returns pointer to the current playing sound
    virtual Sound* GetPlayingSound(uint8 channel) = 0;

	// TODO:
	// must be static and const
	uint16	MIN_VOLUME;	//!< Minimum volume value
	uint16	MAX_VOLUME;	//!< Maximum volume value

protected:
	bool	mute;		//!< Sounds muting
};

#endif // __FRAMEWORK_SOUNDSYS_H__
