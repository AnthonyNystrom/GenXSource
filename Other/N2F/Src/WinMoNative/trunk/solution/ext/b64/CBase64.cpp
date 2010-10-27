/************************************************
 *												*
 * CBase64.cpp									*
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

#include "CBase64.h"


CBase64::CBase64 ()
	{
	}

unsigned int CBase64::CalculateRecquiredEncodeOutputBufferSize (unsigned int p_InputByteCount)
	{
	div_t result = div (p_InputByteCount, 3);

	unsigned int RecquiredBytes = 0;
	if (result.rem == 0)
		{
		// Number of encoded characters
		RecquiredBytes = result.quot * 4;

		// CRLF -> "\r\n" each 76 characters
		result = div (RecquiredBytes, 76);
		RecquiredBytes += result.quot * 2;

		// Terminating null for the Encoded String
		RecquiredBytes += 1;

		return RecquiredBytes;
		}
	else
		{
		// Number of encoded characters
		RecquiredBytes = result.quot * 4 + 4;

		// CRLF -> "\r\n" each 76 characters
		result = div (RecquiredBytes, 76);
		RecquiredBytes += result.quot * 2;

		// Terminating null for the Encoded String
		RecquiredBytes += 1;

		return RecquiredBytes;
		}
	}

unsigned int CBase64::CalculateRecquiredDecodeOutputBufferSize (char* p_pInputBufferString)
	{
	unsigned int BufferLength = strlen (p_pInputBufferString);

	div_t result = div (BufferLength, 4);

	if (p_pInputBufferString [BufferLength - 1] != '=')
		{
		return result.quot * 3;
		}
	else
		{
		if (p_pInputBufferString [BufferLength - 2] == '=')
			{
			return result.quot * 3 - 2;
			}
		else
			{
			return result.quot * 3 - 1;
			}
		}
	}

void CBase64::EncodeByteTriple  (char* p_pInputBuffer, unsigned int InputCharacters, char* p_pOutputBuffer)
	{
	unsigned int mask = 0xfc000000;
	unsigned int buffer = 0;


	char* temp = (char*) &buffer;
	temp [3] = p_pInputBuffer [0];
	if (InputCharacters > 1)
		temp [2] = p_pInputBuffer [1];
	if (InputCharacters > 2)
		temp [1] = p_pInputBuffer [2];

	switch (InputCharacters)
		{
		case 3:
			{
			p_pOutputBuffer [0] = BASE64_ALPHABET [(buffer & mask) >> 26];
			buffer = buffer << 6;
			p_pOutputBuffer [1] = BASE64_ALPHABET [(buffer & mask) >> 26];
			buffer = buffer << 6;
			p_pOutputBuffer [2] = BASE64_ALPHABET [(buffer & mask) >> 26];
			buffer = buffer << 6;
			p_pOutputBuffer [3] = BASE64_ALPHABET [(buffer & mask) >> 26];
			break;
			}
		case 2:
			{
			p_pOutputBuffer [0] = BASE64_ALPHABET [(buffer & mask) >> 26];
			buffer = buffer << 6;
			p_pOutputBuffer [1] = BASE64_ALPHABET [(buffer & mask) >> 26];
			buffer = buffer << 6;
			p_pOutputBuffer [2] = BASE64_ALPHABET [(buffer & mask) >> 26];
			p_pOutputBuffer [3] = '=';
			break;
			}
		case 1:
			{
			p_pOutputBuffer [0] = BASE64_ALPHABET [(buffer & mask) >> 26];
			buffer = buffer << 6;
			p_pOutputBuffer [1] = BASE64_ALPHABET [(buffer & mask) >> 26];
			p_pOutputBuffer [2] = '=';
			p_pOutputBuffer [3] = '=';
			break;
			}
		}
	}

unsigned int CBase64::DecodeByteQuartet (char* p_pInputBuffer, char* p_pOutputBuffer)
	{
	unsigned int buffer = 0;

	if (p_pInputBuffer[3] == '=')
		{
		if (p_pInputBuffer[2] == '=')
			{
			buffer = (buffer | BASE64_DEALPHABET [p_pInputBuffer[0]]) << 6;
			buffer = (buffer | BASE64_DEALPHABET [p_pInputBuffer[1]]) << 6;
			buffer = buffer << 14;

			char* temp = (char*) &buffer;
			p_pOutputBuffer [0] = temp [3];
			
			return 1;
			}
		else
			{
			buffer = (buffer | BASE64_DEALPHABET [p_pInputBuffer[0]]) << 6;
			buffer = (buffer | BASE64_DEALPHABET [p_pInputBuffer[1]]) << 6;
			buffer = (buffer | BASE64_DEALPHABET [p_pInputBuffer[2]]) << 6;
			buffer = buffer << 8;

			char* temp = (char*) &buffer;
			p_pOutputBuffer [0] = temp [3];
			p_pOutputBuffer [1] = temp [2];
			
			return 2;
			}
		}
	else
		{
		buffer = (buffer | BASE64_DEALPHABET [p_pInputBuffer[0]]) << 6;
		buffer = (buffer | BASE64_DEALPHABET [p_pInputBuffer[1]]) << 6;
		buffer = (buffer | BASE64_DEALPHABET [p_pInputBuffer[2]]) << 6;
		buffer = (buffer | BASE64_DEALPHABET [p_pInputBuffer[3]]) << 6; 
		buffer = buffer << 2;

		char* temp = (char*) &buffer;
		p_pOutputBuffer [0] = temp [3];
		p_pOutputBuffer [1] = temp [2];
		p_pOutputBuffer [2] = temp [1];

		return 3;
		}

	return -1;
	}

void CBase64::EncodeBuffer (char* p_pInputBuffer, unsigned int p_InputBufferLength, char* p_pOutputBufferString)
	{
	unsigned int FinishedByteQuartetsPerLine = 0;
	unsigned int InputBufferIndex  = 0;
	unsigned int OutputBufferIndex = 0;

	memset (p_pOutputBufferString, 0, CalculateRecquiredEncodeOutputBufferSize (p_InputBufferLength));

	while (InputBufferIndex < p_InputBufferLength)
		{
		if (p_InputBufferLength - InputBufferIndex <= 2)
			{
			FinishedByteQuartetsPerLine ++;
			EncodeByteTriple (p_pInputBuffer + InputBufferIndex, p_InputBufferLength - InputBufferIndex, p_pOutputBufferString + OutputBufferIndex);
			break;
			}
		else
			{
			FinishedByteQuartetsPerLine++;
			EncodeByteTriple (p_pInputBuffer + InputBufferIndex, 3, p_pOutputBufferString + OutputBufferIndex);
			InputBufferIndex  += 3;
			OutputBufferIndex += 4;
			}

		if (FinishedByteQuartetsPerLine == 19)
			{
			p_pOutputBufferString [OutputBufferIndex  ] = '\r';
			p_pOutputBufferString [OutputBufferIndex+1] = '\n';
			p_pOutputBufferString += 2;
			FinishedByteQuartetsPerLine = 0;
			}
		}
	}

unsigned int CBase64::DecodeBuffer (char* p_pInputBufferString, char* p_pOutputBuffer)
	{
	unsigned int InputBufferIndex  = 0;
	unsigned int OutputBufferIndex = 0;
	unsigned int InputBufferLength = strlen (p_pInputBufferString);

	char ByteQuartet [4];

	while (InputBufferIndex < InputBufferLength)
		{
		for (int i = 0; i < 4; i++)
			{
			ByteQuartet [i] = p_pInputBufferString [InputBufferIndex];

			// Ignore all characters except the ones in BASE64_ALPHABET
			if ((ByteQuartet [i] >= 48 && ByteQuartet [i] <=  57) ||
				(ByteQuartet [i] >= 65 && ByteQuartet [i] <=  90) ||
				(ByteQuartet [i] >= 97 && ByteQuartet [i] <= 122) ||
				 ByteQuartet [i] == '+' || ByteQuartet [i] == '/' || ByteQuartet [i] == '=')
				{
				}
			else
				{
				// Invalid character
				i--;
				}

			InputBufferIndex++;
			}

		OutputBufferIndex += DecodeByteQuartet (ByteQuartet, p_pOutputBuffer + OutputBufferIndex);
		}

	// OutputBufferIndex gives us the next position of the next decoded character
	// inside our output buffer and thus represents the number of decoded characters
	// in our buffer.
	return OutputBufferIndex;
	}

unsigned int CBase64::CreateMatchingEncodingBuffer (unsigned int p_InputByteCount, char** p_ppEncodingBuffer)
	{
	unsigned int Size = CalculateRecquiredEncodeOutputBufferSize (p_InputByteCount);
	(*p_ppEncodingBuffer) = (char*) malloc (Size);
	memset (*p_ppEncodingBuffer, 0, Size);
	return Size;
	}

unsigned int CBase64::CreateMatchingDecodingBuffer (char* p_pInputBufferString, char** p_ppDecodingBuffer)
	{
	unsigned int Size = CalculateRecquiredDecodeOutputBufferSize (p_pInputBufferString);
	(*p_ppDecodingBuffer) = (char*) malloc (Size);
	memset (*p_ppDecodingBuffer, 0, Size);
	return Size;
	}


