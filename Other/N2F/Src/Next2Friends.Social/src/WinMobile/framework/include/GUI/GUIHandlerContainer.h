//// =================================================================
/*!	\file GUIHandlerContainer.h
	
	Revision History:

	\par [11.5.2007] 14:30 by Sergey Zdanevich
	Remove function added.

	\par [10.5.2007] 12:45 by Sergey Zdanevich
	Comments.
	
	\par [8.5.2007]	12:40 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUIHANDLERCONTAINER_H__
#define __FRAMEWORK_GUIHANDLERCONTAINER_H__

#include "BaseTypes.h"
#include "List.h"
#include "GUIEventData.h"
#include "GUIConsts.h"

class GUIControl;

//! Use this macro for declaring your event handlers.
#define HANDLER_PROTOTYPE(f) uint32 f(GUIControl * const pControl, uint32 eventID, GUIEventData *pData)

//! Use this macro for declaring your event handler implementation.
#define HANDLER_IMPLEMENTATION(c, f) uint32 c::f(GUIControl * const pControl, uint32 eventID, GUIEventData *pData)

//! One of those values should be returned by any event handler.
enum eHandlerReturn
{
	EHR_NONE	= 0x0,	//!< Event wasn't processed.
	EHR_END		= 0x1	//!< Event was processed.
};

//  ***************************************************
//! \brief Event handler container.
//!
//! Class is an objective representation of the event handler.
//  ***************************************************
class GUIHandlerContainer
{
protected:

	class GUIHandler
	{
	private:

		class GUIHandlerBase
		{
		public:
			virtual ~GUIHandlerBase()
			{ }
			virtual GUIHandlerBase* Clone() = 0;
			virtual uint32 operator ()(GUIControl * const pControl, uint32 eventID, GUIEventData *pData) = 0;
		};

		template<class T>
		class GUIHandlerImpl : public GUIHandlerBase
		{
		public:
			typedef uint32 (T::*F)(GUIControl * const pControl, uint32 eventID, GUIEventData *pData);

			T*	pThis;	
			F	pFunc;	

			GUIHandlerImpl(T* _pThis, F _pFunc)
			{
				pThis = _pThis;
				pFunc = _pFunc;
			}

			GUIHandlerImpl & operator =(const GUIHandlerImpl &handler)
			{
				pThis = handler.pThis;
				pFunc = handler.pFunc;
			}

			virtual GUIHandlerImpl<T> * Clone()
			{
				return new GUIHandlerImpl(pThis, pFunc);
			}

			virtual ~GUIHandlerImpl()
			{ }

			virtual uint32 operator ()(GUIControl * const pControl, uint32 eventID, GUIEventData *pData)
			{
				return (pThis->*pFunc)(pControl, eventID, pData);
			}
		};

	public:
		GUIHandler()
		{
			base = NULL;
			eventID = 0;
			needDelete = false;
		}

		virtual ~GUIHandler()
		{
			if(NULL != base)
			{
				delete base;
				base = NULL;
			}
		}

		GUIHandlerBase *base;
		uint32			eventID;
		bool			needDelete;

		virtual uint32 operator()(GUIControl * const pControl, uint32 eventID, GUIEventData *pData)
		{
			return (*base)(pControl, eventID, pData);
		}

		GUIHandler(const GUIHandler &guiHandler)
		{
			base		= guiHandler.base->Clone();
			eventID		= guiHandler.eventID;
			needDelete	= guiHandler.needDelete;
		}

		GUIHandler & operator =(const GUIHandler &guiHandler)
		{
			base		= guiHandler.base->Clone();
			eventID		= guiHandler.eventID;
			needDelete	= guiHandler.needDelete;
			return *this;
		}

		template<class T>
			void Create(uint32 _eventID, T* pClass, uint32 (T::*pFunction)(GUIControl * const pControl, uint32 eventID, GUIEventData *pData))
		{
			if(NULL != base)
			{
				delete base;
				base = NULL;
			}

			eventID	= _eventID;
			base	= new GUIHandlerImpl<T>(pClass, pFunction);
		}

		template<class T>
			bool IsEqualTo(uint32 _eventID, T* pClass, uint32 (T::*pFunction)(GUIControl * const pControl, uint32 eventID, GUIEventData *pData))
		{
			if(NULL != base)
			{
				if(_eventID == eventID)
				{
					GUIHandlerImpl<T> *baseImpl = (GUIHandlerImpl<T>*) base;
					if(	(baseImpl->pThis == pClass) &&
						(baseImpl->pFunc == pFunction))
					{
						return true;
					}
				}
			}

			return false;
		}
	};

protected:

	bool						isProcessingEvent;	//!< Indicates, that some event are processing now
	List<GUIHandler>			handlerList;		//!< List of the even handlers.

public:
	GUIHandlerContainer()
	:	isProcessingEvent(false)
	,	handlerList(EGC_HANDLER_RESERVE)
	{ }

	virtual ~GUIHandlerContainer()
	{
		RemoveHandlerAll();
	}

	//  ***************************************************
	//! \brief    	Perform event processing.
	//! \param[in]	pControl - Control on which event has happened.
	//! \param[in]	eventID - Event ID.
	//! \param[in]	pData - Additional event data, or NULL.
	//! \return		Return one of the \ref eHandlerReturn value.
	//  ***************************************************
	virtual uint32 ProcessEvent(GUIControl * const pControl, uint32 eventID, GUIEventData *pData)
	{
		uint32 ret = EHR_NONE;

		//if(handlerList.Size() > 0)
		{
			bool tempProcessing = isProcessingEvent;
			isProcessingEvent = true;

			List<GUIHandler>::Iterator iterator = handlerList.Begin();
			for (; iterator != handlerList.End(); ++iterator)
			{
				if(iterator->eventID == eventID)
				{
					ret |= (*iterator)(pControl, eventID, pData);
				}
			}
			isProcessingEvent = tempProcessing;

			List<GUIHandler>::Iterator begin = handlerList.Begin();
			for (; begin != handlerList.End();)
			{
				List<GUIHandler>::Iterator next = begin;
				next++;
				if (begin->needDelete)
				{
					handlerList.Erase(begin);
				}
				begin = next;
			}
		}

		return ret;
	}

	//  ***************************************************
	//! \brief    	Adds the specified function to the container.
	//! 
	//! \param[in]	eventID	- event id.
	//! \param[in]	pClass	- "this" Ò class pointer.
	//! \param[in]	pFunction - Ò class public member.
	//  ***************************************************
	template<class T>
	void AddHandler(uint32 eventID, T* pClass, uint32 (T::*pFunction)(GUIControl * const pControl, uint32 eventID, GUIEventData *pData))
	{
		GUIHandler h;
		h.Create(eventID, pClass, pFunction);
		handlerList.PushBack(h);
	}

	//  ***************************************************
	//! \brief    	Removes the specified function from the container.
	//! 
	//! \param[in]	eventID	- event id.
	//! \param[in]	pClass	- "this" Ò class pointer.
	//! \param[in]	pFunction - Ò class public member.
	//  ***************************************************
	template<class T>
	void RemoveHandler(uint32 eventID, T* pClass, uint32 (T::*pFunction)(GUIControl * const pControl, uint32 eventID, GUIEventData *pData))
	{
		List<GUIHandler>::Iterator i = handlerList.Begin();
		List<GUIHandler>::Iterator end = handlerList.End();
		for (; i != end; ++i)
		{
			if(i->IsEqualTo(eventID, pClass, pFunction))
			{
				if (isProcessingEvent)
				{
					i->needDelete = true;
				}
				else
				{
					handlerList.Erase(i);
				}
				return;
			}
		}
	}

	// ***************************************************
	//! \brief    	Removes all handlers from container
	// ***************************************************
	void RemoveHandlerAll()
	{
		handlerList.Clear();
	}
};

#endif // __FRAMEWORK_GUIHANDLERCONTAINER_H__