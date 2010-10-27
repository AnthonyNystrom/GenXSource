#ifndef __NDSFRAMEWORK_ALLOCATOR_H__
#define __NDSFRAMEWORK_ALLOCATOR_H__

#include "BaseTypes.h"

template <class ITEM>
class Allocator
{
	struct AllocBlock
	{
		ITEM * startAddr;
		AllocBlock * nextBlock;
	};

public:
	Allocator(uint32 _reserve = 1)
	{
		reserve = _reserve;
		freeCount = 0;
		allocBlockList = NULL;
		freeNode = NULL;
	}

	~Allocator()
	{
		while (allocBlockList)
		{
			AllocBlock * temp = allocBlockList;
			allocBlockList = allocBlockList->nextBlock;
			delete[] temp->startAddr;
			delete temp;
		}
	}

	ITEM * Alloc()
	{
		if (!freeCount--)
		{
			freeCount += reserve;

			ITEM * newfreeNode = new ITEM[reserve];
			for (uint32 i = 0; i < (reserve - 1); ++i)
			{
				newfreeNode[i].nextI = &newfreeNode[i + 1];
			}

			newfreeNode[reserve - 1].nextI = freeNode;
			freeNode = newfreeNode;

			AllocBlock * newAllocBlock = new AllocBlock;
			newAllocBlock->startAddr = newfreeNode;
			newAllocBlock->nextBlock = allocBlockList;
			allocBlockList = newAllocBlock;
		}

		ITEM * node = freeNode;
		freeNode = freeNode->nextI;
		return node;
	}

	void DeAlloc(ITEM * node)
	{
		node->~ITEM();
		freeCount++;
		node->nextI = freeNode;
		freeNode = node;
	}

private:

	ITEM * freeNode;
	
	uint32 freeCount;
	uint32 reserve;

	AllocBlock * allocBlockList;
};

#endif // __NDSFRAMEWORK_ALLOCATOR_H__