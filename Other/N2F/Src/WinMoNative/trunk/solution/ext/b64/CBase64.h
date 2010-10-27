/************************************************
 *												*
 * CBase64.h									*
 * Base 64 de- and encoding class				*
 *												*
 * ============================================ *
 *												*
 * This class was written on 28.05.2003			*
 * by Jan Raddatz [jan-raddatz@web.de]			*
 *												*
 * ============================================ *
 *												*
 * Copyright (c) by Jan Raddatz					*
 * This class was published @ codeguru.com		*
 * 28.05.2003									*
 *												*
 ************************************************/

#pragma once

#include <stdlib.h>
#include <math.h>
#include <memory.h>
const static unsigned int MAX_LINE_LENGTH = 76;

const static char BASE64_ALPHABET [64] = 
	{
	'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', //   0 -   9
	'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', //  10 -  19
	'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', //  20 -  29
	'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', //  30 -  39
	'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', //  40 -  49
	'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', //  50 -  59
	'8', '9', '+', '/'								  //  60 -  63
	};

const static char BASE64_DEALPHABET [128] = 
	{
	 0,  0,  0,  0,  0,  0,  0,  0,  0,  0, //   0 -   9
	 0,  0,  0,  0,  0,  0,  0,  0,  0,  0, //  10 -  19
	 0,  0,  0,  0,  0,  0,  0,  0,  0,  0, //  20 -  29
	 0,  0,  0,  0,  0,  0,  0,  0,  0,  0, //  30 -  39
	 0,  0,  0, 62,  0,  0,  0, 63, 52, 53, //  40 -  49
	54, 55, 56, 57, 58, 59, 60, 61,  0,  0, //  50 -  59
	 0, 61,  0,  0,  0,  0,  1,  2,  3,  4, //  60 -  69
	 5,  6,  7,  8,  9, 10, 11, 12, 13, 14, //  70 -  79
	15, 16, 17, 18, 19, 20, 21, 22, 23, 24, //  80 -  89
	25,  0,  0,  0,  0,  0,  0, 26, 27, 28, //  90 -  99
	29, 30, 31, 32, 33, 34, 35, 36, 37, 38, // 100 - 109
	39, 40, 41, 42, 43, 44, 45, 46, 47, 48, // 110 - 119
	49, 50, 51,  0,  0,  0,  0,  0			// 120 - 127
	};

enum
	{
	UNABLE_TO_OPEN_INPUT_FILE,
	UNABLE_TO_OPEN_OUTPUT_FILE,
	UNABLE_TO_CREATE_OUTPUTBUFFER
	};

class CBase64
	{
	public:
		CBase64 ();

		unsigned int CalculateRecquiredEncodeOutputBufferSize (unsigned int p_InputByteCount);
		unsigned int CalculateRecquiredDecodeOutputBufferSize (char* p_pInputBufferString);

		void			EncodeByteTriple  (char* p_pInputBuffer, unsigned int InputCharacters, char* p_pOutputBuffer);
		unsigned int	DecodeByteQuartet (char* p_pInputBuffer, char* p_pOutputBuffer);

		void			EncodeBuffer (char* p_pInputBuffer, unsigned int p_InputBufferLength, char*p_pOutputBufferString);
		unsigned int	DecodeBuffer (char* p_pInputBufferString, char* p_pOutputBuffer);

		unsigned int CreateMatchingEncodingBuffer (unsigned int p_InputByteCount, char** p_ppEncodingBuffer);
		unsigned int CreateMatchingDecodingBuffer (char* p_pInputBufferString, char** p_ppDecodingBuffer);

	};