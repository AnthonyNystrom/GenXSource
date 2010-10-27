/*!
@file Decompress.h
@brief Class Decompress
*/

#ifndef __FRAMEWORK_DECOMPRESS_H__
#define __FRAMEWORK_DECOMPRESS_H__

#include "Utils.h"

//! Decompress class
class Decompress
{
private:

	uint8 *data;

	uint8 *output_data;
	uint32 original_length;

	int32 position;
	uint8 bit_data;
	int32 bit_count;

public:
	//! Reads bit from compressed data stream.
	uint32 ReadBit();

	//! Reads byte from compressed data stream.
	inline uint8 ReadByte();

	//! Gets a gamma encoded value from the bit-stream.
	uint32 ReadGammaValue();

	//! @brief    	Decompress data.
	//! @param[in]	pInputBuffer	- compressed data buffer pointer.
	//! @param[in]	ppOutputBuffer	- pointer to output uncompressed data buffer pointer.
	//! @param[in]	recreateBuffer	- if true, function will automatically allocate memory,
	//!	for output buffer, else the user will have to allocate memory himself.
	//! @return		uint32			- uncompressed data size.
	uint32 Start(uint8 *pInputBuffer, uint8 **ppOutputBuffer, bool recreateBuffer);
};

#endif // __FRAMEWORK_DECOMPRESS_H__
