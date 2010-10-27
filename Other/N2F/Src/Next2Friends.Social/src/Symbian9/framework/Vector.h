/*! =================================================================
	\file Vector.h

	Revision History:

	[27.4.2007] 14:09 by Lobanov Anton
	\add Content added

	[21.3.2007] 16:40 by Ivan Petrochenko
	\add Content added

	[20.3.2007] 9:20 by Borodovsky Vitaliy
	\add file created
    ================================================================= */

#ifndef __NDSFRAMEWORK_VECTOR_H__
#define __NDSFRAMEWORK_VECTOR_H__

#include "BaseTypes.h"

//! \brief Class Vector
template <class ITEM>
class Vector
{
public:

	//! \brief Class Iterator Vector
	class Iterator
	{
		friend class Vector;

	public:

		// ***************************************************
		//! \brief    	operator+= - increments the iterator by the stated value
		//! 
		//! \param      val - states the value
		//! \return   	Iterator &
		// ***************************************************
		Iterator & operator+=(int32 val)
		{
			curr+=val;
			return *this;
		}
		
		// ***************************************************
		//! \brief    	operator-= - decrements the iterator by the stated value
		//! 
		//! \param      val - states the value
		//! \return   	Iterator &
		// ***************************************************
		Iterator & operator-=(int32 val)
		{
			curr-=val;
			return *this;
		}

		// ***************************************************
		//! \brief    	operator- - returns the difference between two iterators in the amount of elements 
		//! 
		//! \param      iterator - iterator being deducted
		//! \return   	int32
		// ***************************************************
		int32 operator-(const Iterator & iterator) const
		{
			return (curr - iterator.curr);
		}

		// ***************************************************
		//! \brief    	operator++ - postincrement the iterator
		//! 
		//! \return   	Iterator &
		// ***************************************************
		Iterator & operator++()
		{
			curr++;
			return *this;
		}

		// ***************************************************
		//! \brief    	operator-- - postdecrement the iterator
		//! 
		//! \return   	Iterator &
		// ***************************************************
		Iterator & operator--()
		{
			curr--;
			return *this;
		}

#ifdef TEST_TYPES
#	include "UndefWrongTypes.h"
#endif // TEST_TYPES

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

#	ifdef TEST_TYPES
#		include "DefWrongTypes.h"
#	endif // TEST_TYPES

		// ***************************************************
		//! \brief    	operator* - get item pointed by the iterator
		//! 
		//! \return   	ITEM &
		// ***************************************************
		ITEM & operator*()
		{
			return *curr;
		}

		// ***************************************************
		//! \brief    	operator-> - get the item pointer pointed by the iterator 
		//! 
		//! \return   	ITEM *
		// ***************************************************
		ITEM * operator->()
		{
			return curr;
		}

		// ***************************************************
		//! \brief    	operator= - assigns iterator value
		//! 
		//! \param      i
		//! \return   	Iterator &
		// ***************************************************
		Iterator & operator=(const Iterator & i)
		{
			curr = i.curr;
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
			return (curr != i.curr);
		}

		// ***************************************************
		//! \brief    	operator== - test for iterator equality 
		//! 
		//! \param      i
		//! \return   	bool
		// ***************************************************
		bool operator == (const Iterator & i) const
		{
			return (curr == i.curr);
		}

	private:

		ITEM * curr;
	};

	// ***************************************************
	//! \brief    	List - Default constructor
	// ***************************************************
	Vector()
	{
		reserve = 5;
		first = new ITEM[reserve + 1];
		last = first;
		end = last + reserve;
	}

	// ***************************************************
	//! \brief    	Vector
	//! 
	//! \param      size - size of the vector in the elements
	//! \param      _reserve - amount of items reserved in the vector allocator
	// ***************************************************
	Vector(int32 size, int32 _reserve)
	{
		reserve = _reserve;
		first = new ITEM[size + reserve + 1];
		last = first + size;
		end = last + reserve;
	}

	Vector(const Vector & vector)
	{
		// Not using copying constructor
		FASSERT(false);
	}

	// ***************************************************
	//! \brief    	~Vector - destructor
	// ***************************************************
	~Vector()
	{
		delete first;
	}

	// ***************************************************
	//! \brief    	operator= - Copies all the elements of the source vector to itself
	//! 
	//! \param      l - source list
	//! \return   	List &
	// ***************************************************
	Vector & operator=(Vector & l);

	// ***************************************************
	//! \brief    	operator[] - get the vector element by index
	//! 
	//! \param      index - element index
	//! \return   	ITEM &
	// ***************************************************
	ITEM & operator[](int32 index);

	// ***************************************************
	//! \brief    	Resize - Resizes the vector 
	//! 
	//! \param      size - New size of the vector
	//! \return   	void
	// ***************************************************
	void Resize(int32 size);

	// ***************************************************
	//! \brief    	Empty - test for vector emptiness
	//! 
	//! \return   	bool
	// ***************************************************
	bool Empty();

	// ***************************************************
	//! \brief    	Capacity - return capacity of the vector (item reserved in the vector allocator)
	//! 
	//! \return   	int32
	// ***************************************************
	int32 Capacity();

	// ***************************************************
	//! \brief    	Size - returns the amount of vector elements
	//! 
	//! \return   	int32
	// ***************************************************
	int32 Size();

	// ***************************************************
	//! \brief    	Begin - Returns the iterator to the first element of the vector
	//! 
	//! \return   	Iterator
	// ***************************************************
	Iterator Begin();

	// ***************************************************
	//! \brief    	End - Returns the iterator to the last element of the vector
	//! 
	//! \return   	Iterator
	// ***************************************************
	Iterator End();

	// ***************************************************
	//! \brief    	PosItem - the function returns the iterator to the set element
	//! 
	//! \param      item - sets the element
	//! \return   	Iterator
	// ***************************************************
	Iterator PosItem(const ITEM & item);

	// ***************************************************
	//! \brief    	PushBack - Adds a new element to the end of the vector
	//! 
	//! \param      i - the element
	//! \return   	void
	// ***************************************************
	void PushBack(const ITEM & i);

	// ***************************************************
	//! \brief    	PopBack - Removes the element from the end of the vector
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
	//! \brief    	Clear - Clears all the vector
	//! 
	//! \return   	void
	// ***************************************************
	void Clear();

private:

	ITEM * first;

	ITEM * last;

	ITEM * end;

	int32 reserve;
};

// Class implementation
template <class T>
Vector<T> & Vector<T>::operator=(Vector & l)
{
	Resize(l.Size());
	for (int32 i = 0; i < Size(); ++i)
	{
		first[i] = l[i];
	}
	return *this;
}

template <class T>
T & Vector<T>::operator[](int32 index)
{
	return (*(first + index));
}

template <class T>
void Vector<T>::Resize(int32 size)
{
	if (Capacity() < size)
	{
		T * newVec = new T[size + reserve + 1];
		T * peekPos = first;
		T * pokePos = newVec;

		for (; peekPos < last;)
		{
			*pokePos++ = *peekPos++;
		}

		delete first;

		first = newVec;
		last = first + size;
		end = last + reserve;
	}
	else
	{
		last = first + size;
	}
}

template <class T>
bool Vector<T>::Empty()
{
	return (last == first);
}

template <class T>
int32 Vector<T>::Capacity()
{
	return end - first;
}

template <class T>
int32 Vector<T>::Size()
{
	return last - first;
}

template <class T>
typename Vector<T>::Iterator Vector<T>::Begin()
{
	Iterator i;
	i.curr = first;
	return i;
}

template <class T>
typename Vector<T>::Iterator Vector<T>::End()
{
	Iterator i;
	i.curr = last;
	return i;
}

template <class T>
typename Vector<T>::Iterator Vector<T>::PosItem(const T & _item)
{
	for (Iterator it = Begin(); it != End(); ++it)
	{
		if (*it == _item)
			return it;
	}

	return NULL;
}

template <class T>
void Vector<T>::PushBack(const T & i)
{
	Vector<T>::Iterator end = End();
	Insert(end, i);
}

template <class T>
void Vector<T>::PopBack()
{
	if (!Empty())
	{
		Vector<T>::Iterator end = End();
		end--;
		Erase(end);
	}
}

template <class T>
typename Vector<T>::Iterator Vector<T>::Insert(Iterator & i, const T & it)
{
	Vector<T>::Iterator newIt;
	if (Size() == Capacity())
	{
		T * newVec = new T[Size() + reserve + 1];
		T * peekPos = first;
		T * pokePos = newVec;

		for (; peekPos < i.curr;)
		{
			*pokePos++ = *peekPos++;
		}

		newIt.curr = pokePos;
		*pokePos++ = it;

		for (; peekPos < last;)
		{
			*pokePos++ = *peekPos++;
		}

		delete first;

		first = newVec;
		last = pokePos;
		end = last + reserve;
	}
	else
	{
		T * peekPos = last - 1;
		T * pokePos = last;

		for (; peekPos >= i.curr;)
		{
			*pokePos-- = *peekPos--;
		}

		newIt.curr = pokePos;
		*pokePos = it;

		last++;
	}

	return newIt;
}

template <class T>
typename Vector<T>::Iterator Vector<T>::Erase(Iterator & i)
{
	if (i.curr == last)
		return i;

	T * peekPos = i.curr + 1;
	T * pokePos = i.curr;

	for (; peekPos < last;)
	{
		*pokePos++ = *peekPos++;
	}

	last--;

	return i;
}

template <class T>
void Vector<T>::Clear()
{
	last = first;
}

#endif // __NDSFRAMEWORK_VECTOR_H__