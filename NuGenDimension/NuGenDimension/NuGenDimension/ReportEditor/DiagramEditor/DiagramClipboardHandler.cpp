/* ==========================================================================
	Class :			CDiagramClipboardHandler

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-04-30

	Purpose :		"CDiagramClipboardHandler" handles the copy/paste 
					functionality for a "CDiagramEntityContainer". It's a 
					separate class to allow several containers to share 
					the same clipboard in an MDI-application.

	Description :	"CDiagramClipboardHandler" copy/paste is implemented as 
					a "CObArray" with "CDiagramEntity"-derived objects. As 
					soon as objects are put to the 'clipboard', they are 
					cloned into the paste array. As soon as objects are 
					pasted, they are cloned from the paste array. 

	Usage :			"CDiagramEntityContainer" takes a pointer to an instance 
					of "CDiagramClipboardHandler". The clipboard handler must 
					live as long as the "CDiagramEntityContainer". Several 
					"CDiagramEntityContainer"'s can share the same clipboard 
					handler.
   ========================================================================
					26/6 2004	Added group handling (Unruled Boy).
   ========================================================================*/
#include "stdafx.h"
#include "DiagramClipboardHandler.h"
#include "DiagramEntityContainer.h"
#include "GroupFactory.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// 

CDiagramClipboardHandler::CDiagramClipboardHandler()
/* ============================================================
	Function :		CDiagramClipboardHandler::CDiagramClipboardHandler
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
}

CDiagramClipboardHandler::~CDiagramClipboardHandler()
/* ============================================================
	Function :		CDiagramClipboardHandler::~CDiagramClipboardHandler
	Description :	Destructor
	Access :		Public
					
	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{

	ClearPaste();

}

void CDiagramClipboardHandler::Copy( CDiagramEntity* obj )
/* ============================================================
	Function :		CDiagramClipboardHandler::Copy
	Description :	Copies the object "obj" to the 'clipboard'.
	Access :		Public
					
	Return :		void
	Parameters :	CDiagramEntity* obj	-	The object to copy.	
					
	Usage :			Call in response to a Copy-command. Note 
					that "obj" will only be copied to the 
					clipboard, not the screen. See also the 
					functions for copy/paste below.

   ============================================================*/
{

	ClearPaste();
	CDiagramEntity* newobj = obj->Clone();
	newobj->Select( TRUE );
	newobj->MoveRect( 10, 10 );
	m_paste.Add( newobj );

}

void CDiagramClipboardHandler::CopyAllSelected( CDiagramEntityContainer* container )
/* ============================================================
	Function :		CDiagramClipboardHandler::CopyAllSelected
	Description :	Clones all selected object to the paste 
					array.
	Access :		Public
					
	Return :		void
	Parameters :	none

	Usage :			Call to copy all selected objects to the 
					clipboard. "Paste" will put them on screen.

   ============================================================*/
{

	ClearPaste();
	CObArray* arr = container->GetData();

	int	max = arr->GetSize();
	for( int t = 0 ; t < max ; t++ )
	{
		CDiagramEntity* obj = static_cast< CDiagramEntity* >( arr->GetAt( t ) );
		if( obj->IsSelected() )
		{
			CDiagramEntity* newobj = obj->Clone();
			newobj->Select( TRUE );
			newobj->MoveRect( 10, 10 );
			newobj->SetGroup( obj->GetGroup() );
			m_paste.Add( newobj );
		}
	}

}

int CDiagramClipboardHandler::ObjectsInPaste()
/* ============================================================
	Function :		CDiagramClipboardHandler::ObjectsInPaste
	Description :	Returns the number of objects in the paste 
					array.
	Access :		Public
					
	Return :		int		-	The number of objects.
	Parameters :	none

	Usage :			Call to get the number of objects in the 
					clipboard.

   ============================================================*/
{

	return m_paste.GetSize();

}

void CDiagramClipboardHandler::ClearPaste()
/* ============================================================
	Function :		CDiagramEntityContainer::ClearPaste
	Description :	Clears the paste-array.
	Access :		Public
					
	Return :		void
	Parameters :	none

	Usage :			Call to clear the clipboard. All memory is 
					released.

   ============================================================*/
{

	int count = m_paste.GetSize() - 1;
	for( int t = count ; t >= 0 ; t-- )
		delete static_cast< CDiagramEntity* >( m_paste.GetAt( t ) );
	m_paste.RemoveAll();

}

void CDiagramClipboardHandler::Paste( CDiagramEntityContainer* container )
/* ============================================================
	Function :		CDiagramClipboardHandler::Paste
	Description :	Clones the contents of the paste array 
					into the container data array.
	Access :		Public
					
	Return :		void
	Parameters :	none

	Usage :			Call to paste the contents of the clipboard 
					to screen.

   ============================================================*/
{

	CDWordArray	oldgroup;
	CDWordArray	newgroup;

	int max = m_paste.GetSize();
	for( int t = 0 ; t < max ; t++ )
	{
		CDiagramEntity* obj = static_cast< CDiagramEntity* >( m_paste.GetAt( t ) );
		if( obj->GetGroup() )
		{
			int size = oldgroup.GetSize();
			BOOL found = FALSE;
			for( int i = 0 ; i < size ; i++ )
				if( obj->GetGroup() == static_cast< int > ( oldgroup[ i ] ) )
					found = TRUE;

			if( !found )
			{
				oldgroup.Add( obj->GetGroup() );
				newgroup.Add( CGroupFactory::GetNewGroup() );
			}
		}
	}

	for( int t = 0 ; t < max ; t++ )
	{
		CDiagramEntity* obj = static_cast< CDiagramEntity* >( m_paste.GetAt( t ) );
		CDiagramEntity* clone = obj->Clone();

		int group = 0;
		if( obj->GetGroup() )
		{
			int size = oldgroup.GetSize();
			for( int i = 0 ; i < size ; i++ )
				if( obj->GetGroup() == static_cast< int >( oldgroup[ i ] ) )
					group = newgroup[ i ];
		}

		clone->SetGroup( group );
		container->Add( clone );
	}

}

CObArray* CDiagramClipboardHandler::GetData() 
/* ============================================================
	Function :		CDiagramClipboardHandler::GetData
	Description :	Get a pointer to the clipboard data
	Access :		Public
					
	Return :		CObArray*	-	The clipboard data
	Parameters :	none

	Usage :			Call to get the clipboard data.

   ============================================================*/
{ 
	
	return &m_paste; 

}
