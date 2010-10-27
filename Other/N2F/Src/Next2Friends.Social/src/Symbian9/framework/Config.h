#ifndef __FRAMEWORK_CONFIG_H__
#define __FRAMEWORK_CONFIG_H__

//! Count of channels used in SoundSystem
#define SOUND_CHANNEL_COUNT 2

//! Testing of project types for conformity with code style rules
//#define TEST_TYPES

//#define GRAPHICS16_REDUCE
//#define GRAPHICS32_REDUCE


#define	USE_BLT_NATIVE
#define	USE_BLT_NATIVE_PAL8
#define	USE_BLT_NATIVE_PAL4A4
#define	USE_BLT_NATIVE_PAL4
#define	USE_BLT_NATIVE_PAL2
#define	USE_BLT_4444
#define	USE_BLT_4444_PAL8

#define	USE_ROT_BLT_NATIVE
#define	USE_ROT_BLT_NATIVE_PAL8
#define	USE_ROT_BLT_NATIVE_PAL4
#define	USE_ROT_BLT_NATIVE_PAL2
#define	USE_ROT_BLT_4444
#define	USE_ROT_BLT_4444_PAL8

#define ROT_BLT_USED

#ifdef __SYMBIAN32__
//unable to use FxMath in Symbian 8
#undef	USE_ROT_BLT_NATIVE
#undef	USE_ROT_BLT_NATIVE_PAL8
#undef	USE_ROT_BLT_NATIVE_PAL4
#undef	USE_ROT_BLT_NATIVE_PAL2
#undef	USE_ROT_BLT_4444
#undef	USE_ROT_BLT_4444_PAL8

#undef ROT_BLT_USED
#endif


#define UTILS_USE_LOG
#define UTILS_USE_TRACE

#ifdef _WIN32
#define FRAMES_PER_SECOND 20
#define MS_PER_FRAME   1000 / FRAMES_PER_SECOND
#endif

#ifdef BREW

// hack for LGE VX9800 QWERTY keys proceed
#define   QWERTY_VX9800

//! Use BREW MEDIAPLAYER instead of SOUNDPLAYER
//#define SOUND_USE_BREWMEDIA


#else

// WAVE-FILE
// DO NOT EDIT
#define OFFSET_FORMATTAG      20
#define OFFSET_CHANNELS         22
#define OFFSET_SAMPLESPERSEC   24
#define OFFSET_AVGBYTESPERSEC   28
#define OFFSET_BLOCKALIGN      32
#define OFFSET_BITSPERSAMPLE   34
#define OFFSET_CBSIZE          36
#define OFFSET_WAVEDATA         44
#define HEADER_SIZE            OFFSET_WAVEDATA
#define FORMATTAG_SIMPLE       1
#define FORMATTAG_ADPCM        2

#endif

#endif // __FRAMEWORK_CONFIG_H__
