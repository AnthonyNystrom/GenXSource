#include "VList.h"

#ifdef TEST_TYPES
#	include "UndefWrongTypes.h"
#endif // TEST_TYPES

VList::VList()
{
	allocator = new Allocator<Node>(5);
	size = 0;
	head = allocator->Alloc();
	head->prevI = head;
	head->nextI = head;
}

VList::VList(int32 _reserve)
{
	allocator = new Allocator<Node>(_reserve);
	size = 0;
	head = allocator->Alloc();
	head->prevI = head;
	head->nextI = head;
}

VList::VList(const VList & _list)
{
	// Not using copying constructor
	FASSERT(false);
}

VList::~VList()
{
	Clear();
	allocator->DeAlloc(head);
	delete allocator;
}

VList & VList::operator=(VList & l)
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

bool VList::Empty()
{
	return (head == head->nextI);
}

void VList::Resize(int32 size)
{
	if (this->size < size)
		while (this->size < size)
			PushBack(NULL);
	else
		while (this->size > size)
			PopBack();
}

int32 VList::Size()
{
	return size;
}

VList::Iterator VList::Begin()
{
	Iterator i;
	i.cNode = head->nextI;
	return i;
}

VList::Iterator VList::End()
{
	Iterator i;
	i.cNode = head;
	return i;
}

VList::Iterator VList::Pos(int32 _index)
{
	Iterator it = Begin();

	for (int32 i = 0; i < _index; ++i)
		it++;

	return it;
}

VList::Iterator VList::PosItem(void * _item)
{
	Iterator it = Begin();
	for (; it != End(); ++it)
	{
		if (*it == _item)
			return it;
	}

	return it;
}

void VList::PushFront(void * i)
{
	VList::Iterator begin = Begin();
	Insert(begin, i);
}

void VList::PopFront()
{
	VList::Iterator begin = Begin();
	Erase(begin);
}

void VList::PushBack(void * i)
{
	VList::Iterator end = End();
	Insert(end, i);
}

void VList::PopBack()
{
	VList::Iterator end = End();
	end--;
	Erase(end);
}

VList::Iterator VList::Insert(const Iterator & i, void * it)
{
	VList::Iterator newIt;
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

VList::Iterator VList::Erase(const Iterator & i)
{
	VList::Iterator newIt;
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

void VList::Clear()
{
	while (Size())
	{
		VList::Iterator begin = Begin();
		Erase(begin);
	}
}

VList::Iterator & VList::Iterator::operator++()
{
	cNode = cNode->nextI;
	return *this;
}

VList::Iterator VList::Iterator::operator++(int)
{
	Iterator i = *this;
	++(*this);
	return i;
}

VList::Iterator & VList::Iterator::operator--()
{
	cNode = cNode->prevI;
	return *this;
}

VList::Iterator VList::Iterator::operator--(int)
{
	Iterator i = *this;
	--(*this);
	return i;
}

void * VList::Iterator::operator*()
{
	return this->cNode->i;
}

//void * VList::Iterator::operator->()
//{
//	return this->cNode->i;
//}

VList::Iterator & VList::Iterator::operator=(const Iterator & i)
{
	this->cNode = i.cNode;
	return *this;
}

bool VList::Iterator::operator != (const Iterator & i) const
{
	return (this->cNode != i.cNode);
}

bool VList::Iterator::operator == (const Iterator & i) const
{
	return (this->cNode == i.cNode);
}

#ifdef TEST_TYPES
#	include "DefWrongTypes.h"
#endif // TEST_TYPES