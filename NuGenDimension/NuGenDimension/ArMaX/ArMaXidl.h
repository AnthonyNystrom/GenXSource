

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


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


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__


#ifndef __ArMaXidl_h__
#define __ArMaXidl_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DArMaX_FWD_DEFINED__
#define ___DArMaX_FWD_DEFINED__
typedef interface _DArMaX _DArMaX;
#endif 	/* ___DArMaX_FWD_DEFINED__ */


#ifndef ___DArMaXEvents_FWD_DEFINED__
#define ___DArMaXEvents_FWD_DEFINED__
typedef interface _DArMaXEvents _DArMaXEvents;
#endif 	/* ___DArMaXEvents_FWD_DEFINED__ */


#ifndef __ArMaX_FWD_DEFINED__
#define __ArMaX_FWD_DEFINED__

#ifdef __cplusplus
typedef class ArMaX ArMaX;
#else
typedef struct ArMaX ArMaX;
#endif /* __cplusplus */

#endif 	/* __ArMaX_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 

void * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void * ); 


#ifndef __ArMaXLib_LIBRARY_DEFINED__
#define __ArMaXLib_LIBRARY_DEFINED__

/* library ArMaXLib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_ArMaXLib;

#ifndef ___DArMaX_DISPINTERFACE_DEFINED__
#define ___DArMaX_DISPINTERFACE_DEFINED__

/* dispinterface _DArMaX */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DArMaX;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("8E636C37-EBE4-46D3-8C9E-10DA24648ED4")
    _DArMaX : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DArMaXVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DArMaX * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DArMaX * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DArMaX * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DArMaX * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DArMaX * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DArMaX * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DArMaX * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _DArMaXVtbl;

    interface _DArMaX
    {
        CONST_VTBL struct _DArMaXVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DArMaX_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define _DArMaX_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define _DArMaX_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define _DArMaX_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define _DArMaX_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define _DArMaX_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define _DArMaX_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DArMaX_DISPINTERFACE_DEFINED__ */


#ifndef ___DArMaXEvents_DISPINTERFACE_DEFINED__
#define ___DArMaXEvents_DISPINTERFACE_DEFINED__

/* dispinterface _DArMaXEvents */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DArMaXEvents;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("C72A2FDA-8376-4868-99FB-58184138C180")
    _DArMaXEvents : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DArMaXEventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DArMaXEvents * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DArMaXEvents * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DArMaXEvents * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DArMaXEvents * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DArMaXEvents * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DArMaXEvents * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DArMaXEvents * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _DArMaXEventsVtbl;

    interface _DArMaXEvents
    {
        CONST_VTBL struct _DArMaXEventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DArMaXEvents_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define _DArMaXEvents_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define _DArMaXEvents_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define _DArMaXEvents_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define _DArMaXEvents_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define _DArMaXEvents_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define _DArMaXEvents_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DArMaXEvents_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_ArMaX;

#ifdef __cplusplus

class DECLSPEC_UUID("4012F180-AEF8-4DFB-A45D-56BF5BA86458")
ArMaX;
#endif
#endif /* __ArMaXLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


