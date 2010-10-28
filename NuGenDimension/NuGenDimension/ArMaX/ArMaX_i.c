

/* this ALWAYS GENERATED file contains the IIDs and CLSIDs */

/* link this file in with the server and any clients */


 /* File created by MIDL compiler version 6.00.0366 */
/* at Thu Mar 08 23:52:18 2007
 */
/* Compiler settings for .\ArMaX.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#pragma warning( disable: 4049 )  /* more than 64k source lines */


#ifdef __cplusplus
extern "C"{
#endif 


#include <rpc.h>
#include <rpcndr.h>

#ifdef _MIDL_USE_GUIDDEF_

#ifndef INITGUID
#define INITGUID
#include <guiddef.h>
#undef INITGUID
#else
#include <guiddef.h>
#endif

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        DEFINE_GUID(name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8)

#else // !_MIDL_USE_GUIDDEF_

#ifndef __IID_DEFINED__
#define __IID_DEFINED__

typedef struct _IID
{
    unsigned long x;
    unsigned short s1;
    unsigned short s2;
    unsigned char  c[8];
} IID;

#endif // __IID_DEFINED__

#ifndef CLSID_DEFINED
#define CLSID_DEFINED
typedef IID CLSID;
#endif // CLSID_DEFINED

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        const type name = {l,w1,w2,{b1,b2,b3,b4,b5,b6,b7,b8}}

#endif !_MIDL_USE_GUIDDEF_

MIDL_DEFINE_GUID(IID, LIBID_ArMaXLib,0xFBB21704,0xA554,0x42C7,0xAA,0xCB,0xFE,0xAE,0x92,0x0E,0x8D,0xAC);


MIDL_DEFINE_GUID(IID, DIID__DArMaX,0x8E636C37,0xEBE4,0x46D3,0x8C,0x9E,0x10,0xDA,0x24,0x64,0x8E,0xD4);


MIDL_DEFINE_GUID(IID, DIID__DArMaXEvents,0xC72A2FDA,0x8376,0x4868,0x99,0xFB,0x58,0x18,0x41,0x38,0xC1,0x80);


MIDL_DEFINE_GUID(CLSID, CLSID_ArMaX,0x4012F180,0xAEF8,0x4DFB,0xA4,0x5D,0x56,0xBF,0x5B,0xA8,0x64,0x58);

#undef MIDL_DEFINE_GUID

#ifdef __cplusplus
}
#endif



