// MMediaObj.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <Winbase.h>
#include <stdlib.h>
#include <sstream>
#include <iostream>
#include "MMediaObj.h"


extern "C" {
	#include "amr-nb\interf_dec.h"
	
}

int iCntFile;
//#define TRACE_DBG

using namespace std;
void DbgOutInt(string label, int value ) {
#ifdef TRACE_DBG	
	FILE *flog;
	stringstream strs;
	strs << value;
	label.append(strs.str()) ;
	const char *c_str =label.c_str() ;
	//OutputDebugString( c_str ) ;
	flog=fopen("MMedia.log","a");
	fwrite(c_str,1,strlen(c_str),flog);
	fwrite("\r\n",1,2,flog);
	fclose(flog);
#endif
}
void DbgOut(string label) {
#ifdef TRACE_DBG
	
	FILE *flog;
	const char *c_str =label.c_str() ;
	//OutputDebugString( c_str ) ;
	flog=fopen("MMedia.log","a");
	fwrite(c_str,1,strlen(c_str),flog);
	fwrite("\r\n",1,2,flog);
	fclose(flog);
#endif
}

// This is an example of an exported variable
MMEDIAOBJ_API int nMMediaObj=0;

// This is an example of an exported function.
MMEDIAOBJ_API int fnMMediaObj(void)
{
	return 42;
}

// This is the constructor of a class that has been exported.
// see MMediaObj.h for the class definition
CMMediaObj::CMMediaObj()
{
	destate       = NULL;
	m_iDecFps     = 0;
	m_bDecFirstFps= true;
	m_iEncFps     = 0;
	m_bEncFirstFps= true;
	bKeyFrame     = true;
#ifdef TRACE_DBG
	FILE*flog=fopen("MMedia.log","ab");
	fclose(flog);
#endif
	iCntFile=0;
	avcodec_init();
	avcodec_register_all();
	//InitH263Decoder();

	return;
}
CMMediaObj::~CMMediaObj()
{
	AmrFreeMemory();

	//ExitH263Decoder();
}

bool CMMediaObj::FFInit(int iCodecIn, int iCodecOut, int iWidth, int iHeight, int iBitRate, char *pFileNameThumbBmp)
{
	if (iCodecOut==iCodecIn)
		return false;

	DbgOutInt("iWidth:",iWidth);
	DbgOutInt("iHeight:",iHeight);
	DbgOutInt("iBitRate:",iBitRate);
	switch(iCodecIn)
	{
	case N2F_CODEC_FLV:
		DbgOut("CODEC IN:CODEC_ID_FLV1");
		iCodecIn=CODEC_ID_FLV1;
		break;
	case N2F_CODEC_H263:
		DbgOut("CODEC IN:CODEC_ID_H263");
		iCodecIn=CODEC_ID_H263;
		break;
	case N2F_CODEC_MPEG1:
		DbgOut("CODEC IN:CODEC_ID_MPEG1VIDEO");
		iCodecIn=CODEC_ID_MPEG1VIDEO;
		break;
	case N2F_CODEC_MPEG4:
		DbgOut("CODEC OUT:CODEC_ID_MPEG4");
		iCodecIn=CODEC_ID_MPEG4;
		break;
	case N2F_CODEC_JPG:
		DbgOut("CODEC OUT:CODEC_ID_JPEGLS");
		iCodecIn=CODEC_ID_JPEGLS;
		break;

	default:
		DbgOut("BAD CODEC IN");
		return false;
	}
	switch(iCodecOut)
	{
	case N2F_CODEC_FLV:
		DbgOut("CODEC OUT:CODEC_ID_FLV1");
		iCodecOut=CODEC_ID_FLV1;
		break;
	case N2F_CODEC_H263:
		DbgOut("CODEC OUT:CODEC_ID_H263");
		iCodecOut=CODEC_ID_H263;
		break;
	case N2F_CODEC_MPEG1:
		DbgOut("CODEC OUT:CODEC_ID_MPEG1VIDEO");
		iCodecOut=CODEC_ID_MPEG1VIDEO;
		break;
	case N2F_CODEC_MPEG4:
		DbgOut("CODEC OUT:CODEC_ID_MPEG4");
		iCodecOut=CODEC_ID_MPEG4;
		break;
	case N2F_CODEC_JPG:
		DbgOut("CODEC OUT:CODEC_ID_JPEGLS");
		iCodecOut=CODEC_ID_JPEGLS;
		break;
		
	default:
		DbgOut("BAD CODEC OUT");
		return false;
	}
	if(m_ffDecode.Init(iCodecIn, iWidth, iHeight, 1000, pFileNameThumbBmp)==S_OK)
	{	if(m_ffEncode.Init(iCodecOut, iWidth, iHeight, 1000, iBitRate)==S_OK)
			DbgOut("FFINIT OK");
		else
			DbgOut("Encode Init error");
	}	
	else
		DbgOut("Decode Init error");
		
	m_iSizeYUV = (iWidth*iHeight*3)/2;

#ifdef TRACE_DBG

	FILE *flog=fopen("MMedia.flv","wb");
	unsigned char byHeader[]={'F','L','V',0x1,0x5,0,0,0,0x9,0,0,0,0};
	fwrite(byHeader,1,sizeof(byHeader),flog);
	fclose(flog);

	flog=fopen("MMedia.src","wb");
	fclose(flog);
#endif
	DbgOut("FFINIT COMPLETED");
	return true;
}

bool CMMediaObj::AmrInit()
{
	AmrFreeMemory();
	destate = Decoder_Interface_init();

	
	/*speex_bits_init(&bits);
	destate = speex_decoder_init(&speex_nb_mode);
	int tmp=1;
	speex_decoder_ctl(destate, SPEEX_SET_ENH, &tmp);
	int iErr;
	resampler = speex_resampler_init(1, 8000, 22050, 10, &iErr);
	//DbgOutInt("speex_resampler_init:",iErr);
	*/
	return true;
}

void CMMediaObj::AmrFreeMemory()
{
   if (destate)
      Decoder_Interface_exit(destate);
   /*
	if (destate){
		speex_bits_destroy(&bits);
		speex_decoder_destroy(destate);
	}*/
}
#define FRAME_SIZE 320
int CMMediaObj::AmrDecodeOneFrame(char *src,char *dst) 
{

	Decoder_Interface_Decode(destate, (unsigned char*)src, (short*)dst, 0);
	return FRAME_SIZE;
/*
	speex_bits_read_from(&bits, src, 14);
	speex_decode_int(destate, &bits, (short*)dst);
	return FRAME_SIZE;
	
	unsigned int iLenIn=FRAME_SIZE;
	unsigned int iLenOut=5000;
	int err = speex_resampler_process_int(resampler, 0, iDest, &iLenIn, (short*)dst, &iLenOut);
	//DbgOutInt("Err speex_resampler_process_int:",err);
	return iLenOut;*/
}
// Decode and Encode a single frame.
// pData : frame buffer source
// lsize : size frame buffer source
// pEncodedData : buffer data return.
// Return : size data of pEncodedData. 0 if frame is not encoded
// The func. alloc mem into pEncodedData. Doesn't free it. it will be free to the next call or on destructor
int  CMMediaObj::FFDecodeEncode(unsigned char* pData, long lsize, unsigned char **pEncodedData, bool *bKeyFrame)
{
	int iRetSizeEncoded=0;
	*pEncodedData=NULL;


	// calc fps for decode
	int iTick=GetTickCount();
	if (m_bDecFirstFps)
		m_bDecFirstFps=false;
	else
		//m_iDecFps+=(iTick-m_iDecTick);
		m_iDecFps+=66;
	m_iDecTick = iTick;

#ifdef TRACE_DBG
	DbgOutInt("Fps Decode ",m_iDecFps);
	FILE *flog=fopen("MMedia.src","ab");
	fwrite(pData,1,lsize,flog);
	fclose(flog);
#endif

	AVFrame *pFrame = NULL;
	bool bKeyF;
	if ((m_ffDecode.DecodePic(pData, lsize, m_iDecFps, &pFrame, NULL))==S_OK)
	//if(DecompressFrame(pData,lsize,pBufDecoded,m_iSizeYUV)) // other h263 decompress. it work
	{
		DbgOut("Decoded ");

		// calc fps for decode
		iTick=GetTickCount();
		if (m_bEncFirstFps)
			m_bEncFirstFps=false;
		else
			m_iEncFps+=(iTick-m_iEncTick);
			//m_iEncFps+=66;
		m_iEncTick = iTick;

		DbgOutInt("Fps Encode ",m_iEncFps);
		m_ffEncode.EncodePic(pFrame, m_iEncFps);
		if (m_ffEncode.IsPicEncoded())
		{

			DbgOut("ENCODED ");
			unsigned char *p;
			m_ffEncode.GetPicEncoded(&p, (unsigned int*)&iRetSizeEncoded,&bKeyF);
			*bKeyFrame=bKeyF;
			DbgOutInt("GetPicEncoded Size:",iRetSizeEncoded);
			*pEncodedData=p;
#ifdef TRACE_DBG
			flog=fopen("MMedia.flv","ab");
			unsigned char byHeader[20];
			int iSizeData=iRetSizeEncoded+1;
			byHeader[0]=0x09;
			byHeader[1]=(unsigned char)((iSizeData >> 16) & 0xff);
			byHeader[2]=(unsigned char)((iSizeData >> 8) & 0xff);
			byHeader[3]=(unsigned char)(iSizeData & 0xFF);
			byHeader[4]=(unsigned char)((m_iEncFps >> 16) & 0xff);
			byHeader[5]=(unsigned char)((m_iEncFps >> 8) & 0xff);
			byHeader[6]=(unsigned char)(m_iEncFps & 0xFF);
			byHeader[7]=byHeader[8]=byHeader[9]=byHeader[10]=0;
			if (bKeyFrame)
			{	byHeader[11]=0x12;
				bKeyFrame=false;
			}
			else
				byHeader[11]=0x22;
			fwrite(byHeader,1,12,flog);

			fwrite(p,1,iRetSizeEncoded,flog);
			int iFoot=iRetSizeEncoded+11;
			byHeader[0]=(unsigned char)((iFoot >> 24) & 0xff);
			byHeader[1]=(unsigned char)((iFoot >> 16) & 0xff);
			byHeader[2]=(unsigned char)((iFoot >> 8) & 0xff);
			byHeader[3]=(unsigned char)(iFoot & 0xFF);
			fwrite(byHeader,1,4,flog);
			DbgOutInt("Footer size:",iFoot);

			fclose(flog);
#endif
		}
		else
			DbgOut("Not Encode ");

	}
	else
		DbgOut("No Decode ");

	
	return iRetSizeEncoded;
}
