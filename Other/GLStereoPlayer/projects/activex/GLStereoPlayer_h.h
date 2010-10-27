
#pragma warning( disable: 4049 )  /* more than 64k source lines */

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 6.00.0347 */
/* at Sun May 01 01:22:41 2005
 */
/* Compiler settings for GLStereoPlayer.odl:
    Os, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 440
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __GLStereoPlayer_h_h__
#define __GLStereoPlayer_h_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DGLStereoPlayer_FWD_DEFINED__
#define ___DGLStereoPlayer_FWD_DEFINED__
typedef interface _DGLStereoPlayer _DGLStereoPlayer;
#endif 	/* ___DGLStereoPlayer_FWD_DEFINED__ */


#ifndef ___DGLStereoPlayerEvents_FWD_DEFINED__
#define ___DGLStereoPlayerEvents_FWD_DEFINED__
typedef interface _DGLStereoPlayerEvents _DGLStereoPlayerEvents;
#endif 	/* ___DGLStereoPlayerEvents_FWD_DEFINED__ */


#ifndef __GLStereoPlayer_FWD_DEFINED__
#define __GLStereoPlayer_FWD_DEFINED__

#ifdef __cplusplus
typedef class GLStereoPlayer GLStereoPlayer;
#else
typedef struct GLStereoPlayer GLStereoPlayer;
#endif /* __cplusplus */

#endif 	/* __GLStereoPlayer_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 

void * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void * ); 


#ifndef __GLSTEREOPLAYERLib_LIBRARY_DEFINED__
#define __GLSTEREOPLAYERLib_LIBRARY_DEFINED__

/* library GLSTEREOPLAYERLib */
/* [control][helpstring][helpfile][version][uuid] */ 

typedef 
enum __MIDL___MIDL_itf_GLStereoPlayer_0000_0001
    {	Separated	= 0,
	HorizontallyCombined	= 1,
	HorizontallyCombinedAndCompressed	= 2,
	VerticallyCombined	= 3,
	VerticallyCombinedAndCompressed	= 4
    } 	EnumFormat;

typedef 
enum __MIDL___MIDL_itf_GLStereoPlayer_0000_0002
    {	LeftOnly	= 0,
	RightOnly	= 1,
	Anagryph	= 2,
	HorizontallySplitted	= 3,
	VerticallySplitted	= 4,
	HorizontallyInterLeaved	= 5,
	VerticallyInterLeaved	= 6,
	Sharp3D	= 7,
	QuadBuffer	= 8
    } 	EnumStereoType;

typedef 
enum __MIDL___MIDL_itf_GLStereoPlayer_0000_0003
    {	Hide	= 0,
	Auto	= 1,
	Show	= 2
    } 	EnumPlayControl;


DEFINE_GUID(LIBID_GLSTEREOPLAYERLib,0x06E4D5A3,0x70DF,0x4462,0xBF,0xEB,0x63,0x93,0x2E,0x2B,0xD7,0x9A);

#ifndef ___DGLStereoPlayer_DISPINTERFACE_DEFINED__
#define ___DGLStereoPlayer_DISPINTERFACE_DEFINED__

/* dispinterface _DGLStereoPlayer */
/* [hidden][helpstring][uuid] */ 


DEFINE_GUID(DIID__DGLStereoPlayer,0x3266D81F,0x3CD4,0x420E,0x9B,0x05,0x81,0xFA,0x7D,0xE5,0x58,0x74);

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("3266D81F-3CD4-420E-9B05-81FA7DE55874")
    _DGLStereoPlayer : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DGLStereoPlayerVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DGLStereoPlayer * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DGLStereoPlayer * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DGLStereoPlayer * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DGLStereoPlayer * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DGLStereoPlayer * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DGLStereoPlayer * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DGLStereoPlayer * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _DGLStereoPlayerVtbl;

    interface _DGLStereoPlayer
    {
        CONST_VTBL struct _DGLStereoPlayerVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DGLStereoPlayer_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define _DGLStereoPlayer_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define _DGLStereoPlayer_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define _DGLStereoPlayer_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define _DGLStereoPlayer_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define _DGLStereoPlayer_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define _DGLStereoPlayer_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DGLStereoPlayer_DISPINTERFACE_DEFINED__ */


#ifndef ___DGLStereoPlayerEvents_DISPINTERFACE_DEFINED__
#define ___DGLStereoPlayerEvents_DISPINTERFACE_DEFINED__

/* dispinterface _DGLStereoPlayerEvents */
/* [helpstring][uuid] */ 


DEFINE_GUID(DIID__DGLStereoPlayerEvents,0x29A74F8F,0x40E5,0x4930,0x9F,0xF0,0x32,0x86,0xF5,0x74,0x99,0xBB);

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("29A74F8F-40E5-4930-9FF0-3286F57499BB")
    _DGLStereoPlayerEvents : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DGLStereoPlayerEventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DGLStereoPlayerEvents * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DGLStereoPlayerEvents * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DGLStereoPlayerEvents * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DGLStereoPlayerEvents * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DGLStereoPlayerEvents * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DGLStereoPlayerEvents * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DGLStereoPlayerEvents * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _DGLStereoPlayerEventsVtbl;

    interface _DGLStereoPlayerEvents
    {
        CONST_VTBL struct _DGLStereoPlayerEventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DGLStereoPlayerEvents_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define _DGLStereoPlayerEvents_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define _DGLStereoPlayerEvents_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define _DGLStereoPlayerEvents_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define _DGLStereoPlayerEvents_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define _DGLStereoPlayerEvents_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define _DGLStereoPlayerEvents_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DGLStereoPlayerEvents_DISPINTERFACE_DEFINED__ */


DEFINE_GUID(CLSID_GLStereoPlayer,0x64F33E49,0xC865,0x434B,0x84,0xE1,0xAF,0x4B,0xA5,0x73,0x3D,0xD7);

#ifdef __cplusplus

class DECLSPEC_UUID("64F33E49-C865-434B-84E1-AF4BA5733DD7")
GLStereoPlayer;
#endif
#endif /* __GLSTEREOPLAYERLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


