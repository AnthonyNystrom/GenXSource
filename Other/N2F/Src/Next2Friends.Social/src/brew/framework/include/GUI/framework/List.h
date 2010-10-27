/*! =================================================================
	\file List.h

	Revision History:

	[27.4.2007] 14:08 by Lobanov Anton
	\fix исправил баг в Clear

	[30.3.2007] 13:56 by Sergey Zdanevich
	\change		Параметры функций теперь ссылки.

	[21.3.2007] 16:40 by Ivan Petrochenko
	\add		Content added

	[20.3.2007] 9:20 by Borodovsky Vitaliy
	\add		file created
    ================================================================= */
#ifndef __NDSFRAMEWORK_LIST_H__
#define __NDSFRAMEWORK_LIST_H__

#include "BaseTypes.h"
#include "Allocator.h"

//! \brief Class List
template <class ITEM>
class List
{
public:
	struct Node
	{
		ITEM i;
		Node * prevI;
		Node * nextI;
	};

public:

	//! \brief Class List Iterator
	class Iterator
	{
		friend class List;

	public:

		// ***************************************************
		//! \brief    	operator++ - postincrement the iterator
		//! 
		//! \return   	Iterator &
		// ***************************************************
		Iterator & operator++()
		{
			cNode = cNode->nextI;
			return *this;
		}

		// ***************************************************
		//! \brief    	operator++ - preincrement the iterator
		//! 
		//! \return   	List::Iterator
		// ***************************************************
		Iterator operator++(int)
		{
			Iterator i = *this;
			++(*this);
			return i;
		}

		// ***************************************************
		//! \brief    	operator-- - postdecrement the iterator
		//! 
		//! \return   	Iterator &
		// ***************************************************
		Iterator & operator--()
		{
			cNode = cNode->prevI;
			return *this;
		}
		
		// ***************************************************
		//! \brief    	operator-- - predecrement the iterator
		//! 
		//! \return   	List::Iterator
		// ***************************************************
		Iterator operator--(int)
		{
			Iterator i = *this;
			--(*this);
			return i;
		}
		
		// ***************************************************
		//! \brief    	operator* - get item pointed by the iterator
		//! 
		//! \return   	ITEM &
		// ***************************************************
		ITEM & operator*()
		{
			return this->cNode->i;
		}
		
		// ***************************************************
		//! \brief    	operator-> - get the item pointer pointed by the iterator 
		//! 
		//! \return   	ITEM *
		// ***************************************************
		ITEM * operator->()
		{
			return &this->cNode->i;
		}
		
		// ***************************************************
		//! \brief    	operator= - assigns iterator value
		//! 
		//! \param      i
		//! \return   	Iterator &
		// ***************************************************
		Iterator & operator=(const Iterator & i)
		{
			this->cNode = i.cNode;
			return *this;
		}
		
		// ***************************************************
		//! \brief    	operator!= - test for iterator inequality 
		//! 
		//! \param      i
		//! \return   	bool
		// ***************************************************
		bool operator != (const Iterator & i) const
		{
			return (this->cNode != i.cNode);
		}
		
		// ***************************************************
		//! \brief    	operator== - test for iterator equality 
		//! 
		//! \param      i
		//! \return   	bool
		// ***************************************************
		bool operator == (const Iterator & i) const
		{
			return (this->cNode == i.cNode);
		}

	private:

		typename List<ITEM>::Node * cNode;
	};
	
	// ***************************************************
	//! \brief    	List - Default constructor
	// ***************************************************
	List()
	{
		allocator = new Allocator<Node>(5);
		size = 0;
		head = allocator->Alloc();
		head->prevI = head;
		head->nextI = head;
	}

	// ***************************************************
	//! \brief    	List
	//! 
	//! \param      _reserve - amount of items reserved in the list allocator 	
	// ***************************************************
	List(int _reserve)
	{
		allocator = new Allocator<Node>(_reserve);
		size = 0;
		head = allocator->Alloc();
		head->prevI = head;
		head->nextI = head;
	}

	List(const List & _list)
	{
		// Not using copying constructor
		FASSERT(false);
	}

	// ***************************************************
	//! \brief    	~List - destructor
	// ***************************************************
	~List()
	{
		Clear();
		allocator->DeAlloc(head);
		delete allocator;
	}

	// ***************************************************
	//! \brief    	operator= - Copies all the elements of the source list to itself
	//! 
	//! \param      l - source list
	//! \return   	List &
	// ***************************************************
	List & operator=(List & l);

	// ***************************************************
	//! \brief    	Empty - test for list emptiness
	//! 
	//! \return   	bool
	// ***************************************************
	bool Empty();

	// ***************************************************
	//! \brief    	Resize - Resizes the list 
	//! 
	//! \param      size - New size of the list
	//! \return   	void
	// ***************************************************
	void Resize(int32 size);

	// ***************************************************
	//! \brief    	Size - returns the amount of list elements
	//! 
	//! \return   	int32
	// ***************************************************
	int32 Size();

	// ***************************************************
	//! \brief    	Begin - Returns the iterator to the first element of the list
	//! 
	//! \return   	Iterator
	// ***************************************************
	Iterator Begin();

	// ***************************************************
	//! \brief    	End - Returns the iterator to the last element of the list
	//! 
	//! \return   	Iterator
	// ***************************************************
	Iterator End();

	// ***************************************************
	//! \brief    	Pos - returns the iterator to the list element by index
	//! 
	//! \param      _index - element index
	//! \return   	Iterator
	// ***************************************************
	Iterator Pos(int32 _index);

	// ***************************************************
	//! \brief    	PosItem - the function returns the iterator to the set element
	//! 
	//! \param      item - sets the element
	//! \return   	Iterator
	// ***************************************************
	Iterator PosItem(const ITEM & item);
	
	// ***************************************************
	//! \brief    	PushFront - Adds a new element to the beginning of the list
	//! 
	//! \param      i - the element
	//! \return   	void
	// ***************************************************
	void PushFront(const ITEM & i);

	// ***************************************************
	//! \brief    	PopFront - Removes the element from the beginning of the list
	//! 
	//! \return   	void
	// ***************************************************
	void PopFront();
	
	// ***************************************************
	//! \brief    	PushBack - Adds a new element to the end of the list
	//! 
	//! \param      i - the element
	//! \return   	void
	// ***************************************************
	void PushBack(const ITEM & i);
	
	// ***************************************************
	//! \brief    	PopBack - Removes the element from the end of the list
	//! 
	//! \return   	void
	// ***************************************************
	void PopBack();
	
	// ***************************************************
	//! \brief    	Insert - Inserts the element before the element pointed by the iterator
	//! 
	//! \param      i - the iterator
	//! \param      it - the element
	//! \return   	Iterator
	// ***************************************************
	Iterator Insert(Iterator & i, const ITEM & it);
	
	// ***************************************************
	//! \brief    	Erase - Removes the element pointed by the iterator
	//! 
	//! \param      i - the iterator
	//! \return   	Iterator
	// ***************************************************
	Iterator Erase(Iterator & i);
	
	// ***************************************************
	//! \brief    	Clear - Clears all the list
	//! 
	//! \return   	void
	// ***************************************************
	void Clear();

private:

	Node * head;

	Allocator<Node> * allocator;

	int32 size;
};

// Class implementation

template <class T>
List<T> & List<T>::operator=(List<T> & l)
{
	this->Clear();
	Iterator firstSrc = l.Begin();
	Iterator lastSrc = l.End();
	Iterator lastDst = End();

	for (; firstSrc != lastSrc; ++firstSrc)
	{
		Insert(lastDst, *firstSrc);
	}
	return *this;
}

template <class T>
bool List<T>::Empty()
{
	return (head == head->nextI);
}

template <class T>
void List<T>::Resize(int32 size)
{
	if (this->size < size)
		while (this->size < size)
			PushBack(T());
	else
		while (this->size > size)
			PopBack();
}

template <class T>
int32 List<T>::Size()
{
	return size;
}

template <class T>
typename List<T>::Iterator List<T>::Begin()
{
	Iterator i;
	i.cNode = head->nextI;
	return i;
}

template <class T>
typename List<T>::Iterator List<T>::End()
{
	Iterator i;
	i.cNode = head;
	return i;
}

template <class T>
typename List<T>::Iterator List<T>::Pos(int32 _index)
{
	Iterator it = Begin();

	for (int32 i = 0; i < _index; ++i)
		it++;

	return it;
}

template <class T>
typename List<T>::Iterator List<T>::PosItem(const T & _item)
{
	Iterator it = Begin();
	for (; it != End(); ++it)
	{
		if (*it == _item)
			return it;
	}

	return it;
}

template <class T>
void List<T>::PushFront(const T & i)
{
	List<T>::Iterator begin = Begin();
	Insert(begin, i);
}

template <class T>
void List<T>::PopFront()
{
	List<T>::Iterator begin = Begin();
	Erase(begin);
}

template <class T>
void List<T>::PushBack(const T & i)
{
	List<T>::Iterator end = End();
	Insert(end, i);
}

template <class T>
void List<T>::PopBack()
{
	List<T>::Iterator end = End();
	end--;
	Erase(end);
}

template <class T>
typename List<T>::Iterator List<T>::Insert(Iterator & i, const T & it)
{
	List<T>::Iterator newIt;
	Node * pos = i.cNode;
	Node * newpos = allocator->Alloc();
	newpos->i = it;
	newpos->nextI = pos;
	newpos->prevI = pos->prevI;
	pos->prevI->nextI = newpos;
	pos->prevI = newpos;
	size++;
	newIt.cNode = newpos;
	return newIt;
}

template <class T>
typename List<T>::Iterator List<T>::Erase(Iterator & i)
{
	List<T>::Iterator newIt;
	Node * pos = i.cNode;
	if (pos == head)
		return i;

	pos->prevI->nextI = pos->nextI;
	pos->nextI->prevI = pos->prevI;

	newIt.cNode = pos->nextI;

	allocator->DeAlloc(pos);
	size--;

	return newIt;
}

template <class T>
void List<T>::Clear()
{
	while (Size())
	{
		List<T>::Iterator begin = Begin();
		Erase(begin);
	}
}

#endif // __NDSFRAMEWORK_LIST_H__