/*! =================================================================
	\file List.h

	Revision History:

	[20.3.2007] 9:20 by Borodovsky Vitaliy
	\add		file created
    ================================================================= */
#ifndef __LIST_VOID_H__
#define __LIST_VOID_H__

#include "BaseTypes.h"
#include "Allocator.h"

#ifdef TEST_TYPES
#	include "UndefWrongTypes.h"
#endif // TEST_TYPES

//! \brief Class List
class VList
{
public:
	struct Node
	{
		void *i;
		Node * prevI;
		Node * nextI;
	};

public:

	//! \brief Class List Iterator
	class Iterator
	{
		friend class VList;

	public:

		// ***************************************************
		//! \brief    	operator++ - postincrement the iterator
		//! 
		//! \return   	Iterator &
		// ***************************************************
		Iterator & operator++();

		// ***************************************************
		//! \brief    	operator-- - post decrement the iterator
		//! 
		//! \return   	Iterator &
		// ***************************************************
		Iterator & operator--();
		
		// ***************************************************
		//! \brief    	operator++ - preincrement the iterator
		//! 
		//! \return   	List::Iterator
		// ***************************************************
		Iterator operator++(int);

		// ***************************************************
		//! \brief    	operator-- - predecrement the iterator
		//! 
		//! \return   	List::Iterator
		// ***************************************************
		Iterator operator--(int);

		// ***************************************************
		//! \brief    	operator* - get item pointed by the iterator
		//! 
		//! \return   	void *
		// ***************************************************
		void * operator*();
		
		// ***************************************************
		//! \brief    	operator-> - get the item pointer pointed by the iterator 
		//! 
		//! \return   	ITEM *
		// ***************************************************
		//void * operator->();
		
		// ***************************************************
		//! \brief    	operator= - assigns iterator value
		//! 
		//! \param      i
		//! \return   	Iterator &
		// ***************************************************
		Iterator & operator=(const Iterator & i);
		
		// ***************************************************
		//! \brief    	operator!= - test for iterator inequality 
		//! 
		//! \param      i
		//! \return   	bool
		// ***************************************************
		bool operator != (const Iterator & i) const;
		
		// ***************************************************
		//! \brief    	operator== - test for iterator equality 
		//! 
		//! \param      i
		//! \return   	bool
		// ***************************************************
		bool operator == (const Iterator & i) const;

	private:

		VList::Node * cNode;
	};
	
	// ***************************************************
	//! \brief    	List - Default constructor
	// ***************************************************
	VList();

	// ***************************************************
	//! \brief    	List
	//! 
	//! \param      _reserve - amount of items reserved in the list allocator 	
	// ***************************************************
	VList(int32 _reserve);

	VList(const VList & _list);

	// ***************************************************
	//! \brief    	~List - destructor
	// ***************************************************
	~VList();

	// ***************************************************
	//! \brief    	operator= - Copies all the elements of the source list to itself
	//! 
	//! \param      l - source list
	//! \return   	List &
	// ***************************************************
	VList & operator=(VList & l);

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
	Iterator PosItem(void * item);
	
	// ***************************************************
	//! \brief    	PushFront - Adds a new element to the beginning of the list
	//! 
	//! \param      i - the element
	//! \return   	void
	// ***************************************************
	void PushFront(void * i);

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
	void PushBack(void * i);
	
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
	Iterator Insert(const Iterator & i, void * it);
	
	// ***************************************************
	//! \brief    	Erase - Removes the element pointed by the iterator
	//! 
	//! \param      i - the iterator
	//! \return   	Iterator
	// ***************************************************
	Iterator Erase(const Iterator & i);
	
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

#ifdef TEST_TYPES
#	include "DefWrongTypes.h"
#endif // TEST_TYPES

#endif // __NDSFRAMEWORK_LIST_H__