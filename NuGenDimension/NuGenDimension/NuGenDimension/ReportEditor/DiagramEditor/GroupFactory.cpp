/* ==========================================================================
	File :			GroupFactory.cpp
	
	Class :			CGroupFactory

	Date :			06/26/04

	Purpose :		"CGroupFactory" is a utility class generating unique ids 
					for group values.

	Description :	The class is a utility class with one static "int" 
					member and a static function returning a new value 
					each time it is called..

	Usage :			Call "GetNewGroup" as soon as a new group id is needed.

   ========================================================================*/

#include "stdafx.h"
#include "GroupFactory.h"


int CGroupFactory::GetNewGroup()
/* ============================================================
	Function :		CGroupFactory::GetNewGroup()
	Description :	Gets a new unique group-id.
	Access :		Public
					
	Return :		int		-	Unique group id.
	Parameters :	none

	Usage :			Call to get a new unique group id.

   ============================================================*/
{

	CGroupFactory::m_sCurrentGroup++;
	return CGroupFactory::m_sCurrentGroup;

}

int CGroupFactory::m_sCurrentGroup = 0;
