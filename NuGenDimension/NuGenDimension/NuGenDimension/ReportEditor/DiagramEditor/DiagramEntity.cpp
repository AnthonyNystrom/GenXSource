/* ==========================================================================
	Class :			CDiagramEntity

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-03-29

	Purpose :		"CDiagramEntity" is the base class for all objects that can 
					be drawn and managed by "CDiagramEditor".

	Description :	"CDiagramEntity" is derived from "CObject", to allow 
					instances to be stored in "CObArrays".

	Usage :			Classes should be derived from "CDiagramEntity". "Clone" 
					must be overridden, returning a copy of the this 
					pointer.

					Normally, "Draw" should also be overridden. 

					The class supports basic saving to a text file. If this 
					is desired, "SetType" must be called from the derived 
					class ctor with a string uniquely identifying the class. 
					"FromString" and "GetString" must be overridden if other 
					properties than the default are to be saved. Loading can 
					be accomplished by creating a static "factory"-function 
					returning an instance of the class if "FromString" returns 
					"TRUE" for a given line from a data file. See "CreateFromString" 
					in this class for a model implementation.

					Minimum- and maximum sizes for an instance of the 
					derived object can be set in the class ctor by calling
					"SetConstraints". A 0-constraint means that the object 
					can't be turned "inside out", while -1 means no 
					constraints.

					Popup menus for the derived classes can be created by 
					overriding "ShowPopup". Command ids for the menu items 
					must be in the range "CMD_START" to "CMD_END" inclusively, 
					and if a menu alternative is selected, it will be 
					returned to the class instance through "DoMessage" - 
					which must of course also be overriddden.

					Each derived class can also have a property dialog. The 
					dialog class must be derived from "CDiagramPropertyDlg". 
					The derived "CDiagramEntity" class must have a class 
					member instance of the desired "CDiagramPropertyDlg"-
					derived class, and call "SetAttributeDialog" in the "ctor". 
					Transport of data to and from the object is made in the 
					"CDiagramPropertyDlg"-derived class (see 
					CDiagramPropertyDlg.cpp)

					The number, position and types of the selection rects 
					can be modified by overriding "GetHitCode" and 
					"DrawSelectionMarkers", see CDiagramLine.cpp for an 
					example.

					"BodyInRect" can be overridden to allow non-rect hit 
					testing, see CDiagramLine.cpp for an example.

					"GetCursor" can be overridden to display other cursors 
					than the default ones.

   ========================================================================
	Changes :		10/4 2004	Changed accessors for m_type to public.
					24/4 2004	Added colon as a replace-character for 
								saving
					30/4 2004	Added redraw parent to the property dialog
   ========================================================================
					23/6 2004	Added \\newline as a replace-character when 
								saving/loading (Unruled Boy).
					26/6 2004	Added group handling (Unruled Boy).
					27/6 2004	Added help functions for saving and loading
   ========================================================================
					19/8 2004	Setting m_parent to NULL in the ctor (Marc G)
   ========================================================================
					23/1 2005	Made SetParent/GetParent public.
   ========================================================================*/
#include "stdafx.h"
#include "DiagramEntity.h"
#include "DiagramEntityContainer.h"
#include "Tokenizer.h"

#include "..//UnitConversion.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CDiagramEntity

CDiagramEntity::CDiagramEntity()
/* ============================================================
	Function :		CDiagramEntity::CDiagramEntity
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
	SetParent( NULL );
	SetAttributeDialog( NULL, 0 );
	Clear();
	SetType( _T( "basic" ) );

	SetGroup( 0 );
}

CDiagramEntity::~CDiagramEntity()
/* ============================================================
	Function :		CDiagramEntity::~CDiagramEntity
	Description :	Destructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
}

void CDiagramEntity::Clear()
/* ============================================================
	Function :		CDiagramEntity::Clear
	Description :	Zero all properties of this object.
	Access :		Protected

	Return :		void
	Parameters :	none

	Usage :			Call to initialize the object.

   ============================================================*/
{

	SetRect( 0.0, 0.0, 0.0, 0.0 );
	SetMarkerSize( CSize( 8, 8 ) );
	SetConstraints( CSize( 1, 1 ), CSize( -1, -1 ) );
	Select( FALSE );
	SetParent( NULL );
	SetName( _T( "" ) );

}

CDiagramEntity* CDiagramEntity::Clone()
/* ============================================================
	Function :		CDiagramEntity::Clone
	Description :	Clone this object to a new object.
	Access :		Public

	Return :		CDiagramEntity*	-	The new object.
	Parameters :	none

	Usage :			Call to create a clone of the object. The 
					caller will have to delete the object.

   ============================================================*/
{
	CDiagramEntity* obj = new CDiagramEntity;
	obj->Copy( this );
	return obj;
}

void CDiagramEntity::Copy( CDiagramEntity* obj )
/* ============================================================
	Function :		CDiagramEntity::Copy
	Description :	Copy the information in "obj" to this object.
	Access :		Public

	Return :		void
	Parameters :	CDiagramEntity* obj	-	The object to copy 
											from.
					
	Usage :			Copies basic information. from "obj" to this.
					"GetType" can be used to check for the correct 
					object type in overridden versions.
   ============================================================*/
{

	Clear();

	SetMarkerSize( obj->GetMarkerSize() );
	SetConstraints( obj->GetMinimumSize(), obj->GetMaximumSize() );

	Select( obj->IsSelected() );
	SetParent( obj->GetParent() );
	SetType( obj->GetType() );
	SetTitle( obj->GetTitle() );
	SetName( obj->GetName() );
	SetRect( obj->GetLeft(), obj->GetTop(), obj->GetRight(), obj->GetBottom() );

}

BOOL CDiagramEntity::FromString( const CString& str )
/* ============================================================
	Function :		CDiagramEntity::FromString
	Description :	Sets the values for an object from "str". 
	Access :		Public

	Return :		BOOL				-	"TRUE" if "str" 
											represents an 
											object of this 
											type.
	Parameters :	const CString& str	-	Possible text 
											format 
											representation.
					
	Usage :			Can be called to fill an existing object 
					with information from a string created with 
					"GetString".

   ============================================================*/
{

	BOOL result = FALSE;
	CString data( str );
	CString header = GetHeaderFromString( data );
	if( header == GetType() )
		if( GetDefaultFromString( data ) )
			result = TRUE;

	return result;

}

CString CDiagramEntity::GetHeaderFromString( CString& str )
/* ============================================================
	Function :		CDiagramEntity::GetHeaderFromString
	Description :	Gets the header from "str".
	Access :		Protected

	Return :		CString			-	The type of "str".
	Parameters :	CString& str	-	"CString" to get type from.
					
	Usage :			Call as a part of loading the object. "str" 
					will have the type removed after the call.

   ============================================================*/
{

	CTokenizer main( str, _T( ":" ) );
	CString header;
	CString data;
	if( main.GetSize() == 2 )
	{

		main.GetAt( 0, header );
		main.GetAt( 1, data );
		header.TrimLeft();
		header.TrimRight();
		data.TrimLeft();
		data.TrimRight();

		str = data;
	}

	return header;

}

BOOL CDiagramEntity::GetDefaultFromString( CString& str )
/* ============================================================
	Function :		CDiagramEntity::GetDefaultFromString
	Description :	Gets the default properties from "str"
	Access :		Protected

	Return :		BOOL			-	"TRUE" if the default 
										properties could be loaded ok.
	Parameters :	CString& str	-	"CString" to get the 
										default properties from.
					
	Usage :			Call as a part of loading the object from 
					disk. The default object properties will 
					be stripped from "str" and the object 
					properties set from the data.

   ============================================================*/
{
	BOOL result = FALSE;
	CString data( str );
	if( data[ data.GetLength() -1 ] == _TCHAR( ';' ) )
		data = data.Left( data.GetLength() - 1 ); // Strip the ';'

	CTokenizer tok( data ); 
	int size = tok.GetSize();
	if( size >= 7 )
	{
		double left;
		double top;
		double right;
		double bottom;
		CString title;
		CString name;
		int group;
		int count = 0;

		tok.GetAt( count++, left );
		tok.GetAt( count++, top );
		tok.GetAt( count++, right );
		tok.GetAt( count++, bottom );
		tok.GetAt( count++, title );
		tok.GetAt( count++, name );
		tok.GetAt( count++, group );

		SetRect( left, top, right, bottom );

		title.Replace( _T( "\\colon" ), _T( ":" ) );
		title.Replace( _T( "\\semicolon" ), _T( ";" ) );
		title.Replace( _T( "\\comma" ), _T( "," ) );
		title.Replace( _T( "\\newline" ), _T( "\r\n" ) );

		name.Replace( _T( "\\colon" ), _T( ":" ) );
		name.Replace( _T( "\\semicolon" ), _T( ";" ) );
		name.Replace( _T( "\\comma" ), _T( "," ) );
		name.Replace( _T( "\\newline" ), _T( "\r\n" ) );

		SetTitle( title );
		SetName( name );
		SetGroup( group );

		// Rebuild rest of string
		str = _T( "" );
		for( int t = count ; t < size ; t++ )
		{
			tok.GetAt( t, data );

			str += data;
			if( t < size - 1 )
				str += _T( "," );
		}

		result = TRUE;
	}

	return result;

}

BOOL CDiagramEntity::LoadFromString( CString& data )
/* ============================================================
	Function :		CDiagramEntity::LoadFromString
	Description :	Loads the object from "data".
	Access :		Public

	Return :		BOOL			-	"TRUE" if "str" is a 
										well-formed object prefix.
	Parameters :	CString& data	-	String to load from
					
	Usage :			Call to load the first part of an object 
					from string.

   ============================================================*/
{

	BOOL result = FALSE;
	CString header = GetHeaderFromString( data );
	if( header == GetType() )
		if( GetDefaultFromString( data ) )
			result = TRUE;

	return result;

}

CDiagramEntity* CDiagramEntity::CreateFromString( const CString& str )
/* ============================================================
	Function :		CDiagramEntity::CreateFromString
	Description :	Static factory function that creates and 
					returns an instance of this class if "str" 
					is a valid representation.
	Access :		Public

	Return :		CDiagramEntity*		-	The object, or "NULL" 
											if "str" is not a 
											representation of 
											this type.
	Parameters :	const CString& str	-	The string to create 
											from.
					
	Usage :			Can be used as a factory for text file loads. 
					Each object type should have its own 
					version - the default one is a model 
					implementation.

   ============================================================*/
{

	CDiagramEntity* obj = new CDiagramEntity;
	if(!obj->FromString( str ) )
	{
		delete obj;
		obj = NULL;
	}

	return obj;

}

void  CDiagramEntity::ReadStringFromArchive(CArchive& ar, CString& str)
{
	int str_sz = 0;
	ar.Read(&str_sz,sizeof(int));
	if (str_sz<=0)
		return;
	ar.Read(str.GetBufferSetLength(str_sz), str_sz);
}

void   CDiagramEntity::WriteStringToArchive(CArchive& ar, CString& str)
{
	int str_sz = str.GetLength();
	ar.Write(&str_sz, sizeof(int));
	if (str_sz<=0)
		return;
	ar.Write(str, str_sz);
}

void  CDiagramEntity::Serialize(CArchive& ar)
{

	typedef struct
	{
		int a_f;
		int b_f;
		int c_f;
		int d_f;
	} OBJ_RESERVE_FIELDS;

	OBJ_RESERVE_FIELDS obj_reserve;
	memset(&obj_reserve,0,sizeof(OBJ_RESERVE_FIELDS));

	CRect rect = GetRect();
	// Saving and loading to/from a text file
	if (ar.IsStoring())
	{
		// Сохраняем
		double   tmpDbl = 0.0;
		tmpDbl = CUnitConversion::PixelsToInches( rect.left );
		ar.Write(&tmpDbl,sizeof(double));
		tmpDbl = CUnitConversion::PixelsToInches( rect.top );
		ar.Write(&tmpDbl,sizeof(double));
		tmpDbl = CUnitConversion::PixelsToInches( rect.right );
		ar.Write(&tmpDbl,sizeof(double));
		tmpDbl = CUnitConversion::PixelsToInches( rect.bottom );
		ar.Write(&tmpDbl,sizeof(double));
		CString title = GetTitle();
		title.Replace( _T( ":" ), _T( "\\colon" ) );
		title.Replace( _T( ";" ), _T( "\\semicolon" ) );
		title.Replace( _T( "," ), _T( "\\comma" ) );
		title.Replace( _T( "\r\n" ), _T( "\\newline" ) );
		WriteStringToArchive(ar,title);
		CString name = GetName();
		name.Replace( _T( ":" ), _T( "\\colon" ) );
		name.Replace( _T( ";" ), _T( "\\semicolon" ) );
		name.Replace( _T( "," ), _T( "\\comma" ) );
		name.Replace( _T( "\r\n" ), _T( "\\newline" ) );
		WriteStringToArchive(ar,name);
		int tmpI = GetGroup();
		ar.Write(&tmpI,sizeof(int));

		ar.Write(&obj_reserve,sizeof(OBJ_RESERVE_FIELDS));
	}
	else
	{
		// Читаем

		double left;
		ar.Read(&left,sizeof(double));
		double top;
		ar.Read(&top,sizeof(double));
		double right;
		ar.Read(&right,sizeof(double));
		double bottom;
		ar.Read(&bottom,sizeof(double));
		CString title;
		ReadStringFromArchive(ar,title);
		CString name;
		ReadStringFromArchive(ar,name);
		int group;
		ar.Read(&group,sizeof(int));

		ar.Read(&obj_reserve,sizeof(OBJ_RESERVE_FIELDS));
		
		SetRect( left, top, right, bottom );

		title.Replace( _T( "\\colon" ), _T( ":" ) );
		title.Replace( _T( "\\semicolon" ), _T( ";" ) );
		title.Replace( _T( "\\comma" ), _T( "," ) );
		title.Replace( _T( "\\newline" ), _T( "\r\n" ) );

		name.Replace( _T( "\\colon" ), _T( ":" ) );
		name.Replace( _T( "\\semicolon" ), _T( ";" ) );
		name.Replace( _T( "\\comma" ), _T( "," ) );
		name.Replace( _T( "\\newline" ), _T( "\r\n" ) );

		SetTitle( title );
		SetName( name );
		SetGroup( group );

		int left_i = CUnitConversion::InchesToPixels( GetLeft() );
		int right_i = CUnitConversion::InchesToPixels( GetRight() );
		int top_i = CUnitConversion::InchesToPixels( GetTop() );
		int bottom_i = CUnitConversion::InchesToPixels( GetBottom() );

		CRect rect_in( left_i, top_i, right_i, bottom_i );
		SetRect( rect_in );


	}
}

CString CDiagramEntity::GetString() const
/* ============================================================
	Function :		CDiagramEntity::GetString
	Description :	Creates a string representing the object.
	Access :		Public

	Return :		CString	-	The resulting string
	Parameters :	none

	Usage :			Used to save this object to a text file.

   ============================================================*/
{

	CString str = GetDefaultGetString();

	str += _T( ";" );

	return str;

}

CString CDiagramEntity::GetDefaultGetString() const
/* ============================================================
	Function :		CDiagramEntity::GetDefaultString
	Description :	Gets the default properties of the object 
					as a string.
	Access :		Protected

	Return :		CString	-	Resulting string
	Parameters :	none

	Usage :			Call as a part of the saving of objects 
					to disk.

   ============================================================*/
{
	CString str;

	CString title = GetTitle();
	title.Replace( _T( ":" ), _T( "\\colon" ) );
	title.Replace( _T( ";" ), _T( "\\semicolon" ) );
	title.Replace( _T( "," ), _T( "\\comma" ) );
	title.Replace( _T( "\r\n" ), _T( "\\newline" ) );

	CString name = GetName();
	name.Replace( _T( ":" ), _T( "\\colon" ) );
	name.Replace( _T( ";" ), _T( "\\semicolon" ) );
	name.Replace( _T( "," ), _T( "\\comma" ) );
	name.Replace( _T( "\r\n" ), _T( "\\newline" ) );

	str.Format( _T( "%s:%f,%f,%f,%f,%s,%s,%i" ), GetType(), GetLeft(), GetTop(), GetRight(), GetBottom(), title, name, GetGroup() );

	return str;
}

CRect CDiagramEntity::GetRect() const
/* ============================================================
	Function :		CDiagramEntity::GetRect
	Description :	Returns the object rectangle.
	Access :		Public

	Return :		CRect	-	The object rectangle.
	Parameters :	none

	Usage :			Call to get the object position and size. 
					Will round of fractions.

   ============================================================*/
{

	CRect rect( static_cast< int >( GetLeft() ), 
				static_cast< int >( GetTop() ), 
				static_cast< int >( GetRight() ), 
				static_cast< int >( GetBottom() ) );
	return rect;

}

void CDiagramEntity::SetRect( CRect rect )
/* ============================================================
	Function :		CDiagramEntity::SetRect
	Description :	Sets the object rectangle, normalized.
	Access :		Public

	Return :		void
	Parameters :	CRect rect	-	The rectangle to set.
					
	Usage :			Call to place the object.

   ============================================================*/
{

	rect.NormalizeRect();
	SetRect( static_cast< double >( rect.left ), 
				static_cast< double >( rect.top ), 
				static_cast< double >( rect.right ), 
				static_cast< double >( rect.bottom ) );

}

void CDiagramEntity::SetRect( double left, double top, double right, double bottom )
/* ============================================================
	Function :		CDiagramEntity::SetRect
	Description :	Sets the object rectangle.
	Access :		Public

	Return :		void
	Parameters :	double left		-	Left edge
					double top		-	Top edge
					double right	-	Right edge
					double bottom	-	Bottom edge
					
	Usage :			Call to place the object.

   ============================================================*/
{

	SetLeft( left );
	SetTop( top );
	SetRight( right );
	SetBottom( bottom );

	if( GetMinimumSize().cx != -1 )
		if( GetRect().Width() < GetMinimumSize().cx )
			SetRight( GetLeft() + GetMinimumSize().cx );

	if( GetMinimumSize().cy != -1 )
		if( GetRect().Height() < GetMinimumSize().cy )
			SetBottom( GetTop() + GetMinimumSize().cy );

	if( GetMaximumSize().cx != -1 )
		if( GetRect().Width() > GetMaximumSize().cx )
			SetRight( GetLeft() + GetMaximumSize().cx );

	if( GetMaximumSize().cy != -1 )
		if( GetRect().Height() > GetMaximumSize().cy )
			SetBottom( GetTop() + GetMaximumSize().cy );

 // if( GetAttributeDialog() )
//		GetAttributeDialog()->SetValues();

}

void CDiagramEntity::MoveRect( double x, double y )
/* ============================================================
	Function :		CDiagramEntity::MoveRect
	Description :	Moves the object rectangle.
	Access :		Public

	Return :		void
	Parameters :	double x	-	Move x steps horizontally.
					double y	-	Move y steps vertically.
					
	Usage :			Call to move the object on screen.

   ============================================================*/
{

	SetRect( GetLeft() + x, GetTop() + y, GetRight() + x, GetBottom() + y );

}

void CDiagramEntity::Select( BOOL selected )
/* ============================================================
	Function :		CDiagramEntity::Select
	Description :	Sets the object select state.
	Access :		Public

	Return :		void
	Parameters :	BOOL selected	-	"TRUE" to select, "FALSE" 
										to unselect.
					
	Usage :			Call to select/deselect the object.

   ============================================================*/
{

	m_selected = selected;

	if( selected && GetGroup() )
	{
		CDiagramEntityContainer* parent = GetParent();
		if( parent )
			parent->SendMessageToObjects( CMD_SELECT_GROUP, FALSE, this );
	}

}

BOOL CDiagramEntity::IsSelected() const
/* ============================================================
	Function :		CDiagramEntity::IsSelected
	Description :	Checks if the object is selected.
	Access :		Public

	Return :		BOOL	-	"TRUE" if the object is selected.
	Parameters :	none

	Usage :			Call to see if the object is selected.

   ============================================================*/
{

	return m_selected;

}

BOOL CDiagramEntity::BodyInRect( CRect rect ) const
/* ============================================================
	Function :		CDiagramEntity::BodyInRect
	Description :	Used to see if any part of the object lies 
					in "rect".
	Access :		Public

	Return :		BOOL		-	"TRUE" if any part of the 
									object lies inside rect.
	Parameters :	CRect rect	-	The rectangle to check.
					
	Usage :			Call to see if the object overlaps - for 
					example - a selection rubberband.

   ============================================================*/
{

	BOOL result = FALSE;
	CRect rectEntity = GetRect();
	CRect rectIntersect;

	rect.NormalizeRect();
	rectEntity.NormalizeRect();

	rectIntersect.IntersectRect( rect, rectEntity );
	if( !rectIntersect.IsRectEmpty() )
		result = TRUE;

	return result;

}

int CDiagramEntity::GetHitCode( CPoint point ) const
/* ============================================================
	Function :		CDiagramEntity::GetHitCode
	Description :	Returns the hit point constant for "point".
	Access :		Public

	Return :		int				-	The hit point, 
										"DEHT_NONE" if none.
	Parameters :	CPoint point	-	The point to check
					
	Usage :			Call to see in what part of the object point 
					lies. The hit point can be one of the following:
						"DEHT_NONE" No hit-point
						"DEHT_BODY" Inside object body
						"DEHT_TOPLEFT" Top-left corner
						"DEHT_TOPMIDDLE" Middle top-side
						"DEHT_TOPRIGHT" Top-right corner
						"DEHT_BOTTOMLEFT" Bottom-left corner
						"DEHT_BOTTOMMIDDLE" Middle bottom-side
						"DEHT_BOTTOMRIGHT" Bottom-right corner
						"DEHT_LEFTMIDDLE" Middle left-side
						"DEHT_RIGHTMIDDLE" Middle right-side

   ============================================================*/
{

	CRect rect = GetRect();
	return GetHitCode( point, rect );

}

BOOL CDiagramEntity::DoMessage( UINT msg, CDiagramEntity* sender, IThumbnailerStorage* from )
/* ============================================================
	Function :		CDiagramEntity::DoMessage
	Description :	Message handler for the object.
	Access :		Public

	Return :		BOOL					-	"TRUE" to stop 
												further processing.
	Parameters :	UINT msg				-	The message.
					CDiagramEntity* sender	-	Original sender of 
												this message, or 
												"NULL" if not an object.

	Usage :			The container can send messages to all 
					objects. The messages should lie in the 
					range "CMD_START" to "CMD_STOP" inclusively - 
					a few are already predefined in 
					DiagramEntity.h. This function will be 
					called as response to those messages. This 
					mechanism is already used for sending back 
					messages from "CDiagramEditor" to the 
					relevant object when a object popup menu 
					alternative is selected.

   ============================================================*/
{

	BOOL stop = FALSE;
	switch( msg )
	{
		case CMD_CUT:
			if( m_parent && IsSelected() )
			{
				stop = TRUE;
				m_parent->Cut( this );
			}
		break;

		case CMD_COPY:
			if( m_parent && IsSelected() )
			{
				stop = TRUE;
				m_parent->Copy( this );
			}
		break;

		case CMD_UP:
			if( m_parent && IsSelected() )
			{
				stop = TRUE;
				m_parent->Up( this );
			}
		break;

		case CMD_DOWN:
			if( m_parent && IsSelected() )
			{
				stop = TRUE;
				m_parent->Down( this );
			}
		break;

		case CMD_FRONT:
			if( m_parent && IsSelected() )
			{
				stop = TRUE;
				m_parent->Front( this );
			}
		break;

		case CMD_BOTTOM:
			if( m_parent && IsSelected() )
			{
				stop = TRUE;
				m_parent->Bottom( this );
			}
		break;

		case CMD_DUPLICATE:
			if( m_parent && IsSelected() )
			{
				stop = TRUE;
				m_parent->Duplicate( this );
				Select( FALSE );
			}
		break;

		case CMD_PROPERTIES:
			if( IsSelected() )
			{
				ShowProperties( from );
				stop = TRUE;
			}
		break;

		case CMD_SELECT_GROUP:
			if( sender != this )
				if( sender->GetGroup() == GetGroup() )
					m_selected = TRUE;
		break;

	}

	return stop;

}

#include "..//..//resource.h"
CString      GetLeftHalfOfString(UINT nID);
void CDiagramEntity::ShowPopup( CPoint point, CWnd* parent )
/* ============================================================
	Function :		CDiagramEntity::ShowPopup
	Description :	Shows the popup menu for the object.
	Access :		Public

	Return :		void
	Parameters :	CPoint point	-	The point to track.
					CWnd* parent	-	The parent "CWnd" of the 
										menu (should be the 
										"CDiagramEditor")

	Usage :			The function uses hardcoded strings to 
					avoid having to include resource file 
					fragments. Derived classes needing a non-
					standard or localized menu should load 
					menues from resources instead.

   ============================================================*/
{

	CEGMenu PopupMenu;

	PopupMenu.CreatePopupMenu();

	int nItem=0;
	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION, CMD_CUT, GetLeftHalfOfString(IDS_REP_CUT));
	CBitmap* mib1 = new CBitmap;
	mib1->LoadBitmap(IDB_REP_CUT);
	PopupMenu.SetMenuItemBitmap(CMD_CUT,*mib1);
	delete mib1;

	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION, CMD_COPY, GetLeftHalfOfString(IDS_REP_COPY));
	mib1 = new CBitmap;
	mib1->LoadBitmap(IDB_REP_COPY);
	PopupMenu.SetMenuItemBitmap(CMD_COPY,*mib1);
	delete mib1;

	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION, CMD_DUPLICATE, GetLeftHalfOfString(IDS_REP_DUPLIC));
	
	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION|MF_SEPARATOR);

	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION, CMD_UP, GetLeftHalfOfString(IDS_REP_UP));
	mib1 = new CBitmap;
	mib1->LoadBitmap(IDB_REP_UP);
	PopupMenu.SetMenuItemBitmap(CMD_UP,*mib1);
	delete mib1;

	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION, CMD_DOWN, GetLeftHalfOfString(IDS_REP_DOWN));
	mib1 = new CBitmap;
	mib1->LoadBitmap(IDB_REP_DOWN);
	PopupMenu.SetMenuItemBitmap(CMD_DOWN,*mib1);
	delete mib1;

	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION, CMD_FRONT, GetLeftHalfOfString(IDS_REP_FRONT));
	mib1 = new CBitmap;
	mib1->LoadBitmap(IDB_REP_FR);
	PopupMenu.SetMenuItemBitmap(CMD_FRONT,*mib1);
	delete mib1;

	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION, CMD_BOTTOM, GetLeftHalfOfString(IDS_REP_BACK));
	mib1 = new CBitmap;
	mib1->LoadBitmap(IDB_REP_BACK);
	PopupMenu.SetMenuItemBitmap(CMD_BOTTOM,*mib1);
	delete mib1;

	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION|MF_SEPARATOR);

	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION, CMD_PROPERTIES, GetLeftHalfOfString(IDS_REP_COMMON_PROPS));
	mib1 = new CBitmap;
	mib1->LoadBitmap(IDB_REP_PROP);
	PopupMenu.SetMenuItemBitmap(CMD_PROPERTIES,*mib1);
	delete mib1;


	PopupMenu.TrackPopupMenu(TPM_LEFTALIGN | TPM_RIGHTBUTTON,point.x, point.y,parent);




}

void CDiagramEntity::ShowProperties( IThumbnailerStorage* parent, BOOL show )
/* ============================================================
	Function :		CDiagramEntity::ShowProperties
	Description :	Shows the property dialog for the object.
	Access :		Public

	Return :		void
	Parameters :	CWnd* parent	-	Parent of the dialog
					BOOL show		-	"TRUE" to show, "FALSE" 
										to hide.

	Usage :			Call to show the property dialog for this 
					object.

   ============================================================*/
{

	if( m_propertydlg )
	{
		if( show )
		{
			if( !m_propertydlg->m_hWnd )
				m_propertydlg->Create( ( UINT ) m_propertydlgresid, parent );

			m_propertydlg->ShowWindow( SW_SHOW );
			m_propertydlg->SetValues();
			m_propertydlg->SetFocus();
		}
		else
			if( m_propertydlg->m_hWnd )
				m_propertydlg->ShowWindow( SW_HIDE );
	}

}

void CDiagramEntity::DrawObject( CDC* dc, double zoom , bool draw_markers /*= true*/)
/* ============================================================
	Function :		CDiagramEntity::DrawObject
	Description :	Top-level drawing function for the object.
	Access :		Public

	Return :		void
	Parameters :	CDC* dc		-	"CDC" to draw to.
					double zoom	-	Zoom level to use
					
	Usage :			Even though virtual, this function should 
					normally not be overridden (use "Draw" 
					instead). The function stores the zoom and 
					calculates the true drawing rectangle.

   ============================================================*/
{

	SetZoom( zoom );
	CRect rect( round( GetLeft() * zoom ), 
				round( GetTop() * zoom ), 
				round( GetRight() * zoom ), 
				round( GetBottom() * zoom ) );

	Draw( dc, rect );

	if( draw_markers && IsSelected() )
		DrawSelectionMarkers( dc, rect );

}

void CDiagramEntity::Draw( CDC* dc, CRect rect )
/* ============================================================
	Function :		CDiagramEntity::Draw
	Description :	Draws the object.
	Access :		Public

	Return :		void
	Parameters :	CDC* dc		-	The "CDC" to draw to. 
					CRect rect	-	The real rectangle of the 
									object.

	Usage :			The function should clean up all selected 
					objects. Note that the "CDC" is a memory "CDC", 
					so creating a memory "CDC" in this function 
					will probably not speed up the function.

   ============================================================*/
{

	dc->SelectStockObject( BLACK_PEN );
	dc->SelectStockObject( WHITE_BRUSH );

	dc->Rectangle( rect );


}

HCURSOR CDiagramEntity::GetCursor( int hit ) const
/* ============================================================
	Function :		CDiagramEntity::GetCursor
	Description :	Returns the cursor for the given hit point.
	Access :		Public

	Return :		HCURSOR	-	The cursor to show
	Parameters :	int hit	-	The hit point constant ("DEHT_") 
								to get the cursor for.

	Usage :			Call to get the cursor for a specific hit 
					point constant.
					"hit" can be one of the following:
						"DEHT_NONE" No hit-point
						"DEHT_BODY" Inside object body
						"DEHT_TOPLEFT" Top-left corner
						"DEHT_TOPMIDDLE" Middle top-side
						"DEHT_TOPRIGHT" Top-right corner
						"DEHT_BOTTOMLEFT" Bottom-left corner
						"DEHT_BOTTOMMIDDLE" Middle bottom-side
						"DEHT_BOTTOMRIGHT" Bottom-right corner
						"DEHT_LEFTMIDDLE" Middle left-side
						"DEHT_RIGHTMIDDLE" Middle right-side

   ============================================================*/
{
	HCURSOR cursor = NULL;
	switch( hit )
	{
		case DEHT_BODY:
			cursor = LoadCursor( NULL, IDC_SIZEALL );
		break;
		case DEHT_TOPLEFT:
			cursor = LoadCursor( NULL, IDC_SIZENWSE );
		break;
		case DEHT_TOPMIDDLE:
			cursor = LoadCursor( NULL, IDC_SIZENS );
		break;
		case DEHT_TOPRIGHT:
			cursor = LoadCursor( NULL, IDC_SIZENESW );
		break;
		case DEHT_BOTTOMLEFT:
			cursor = LoadCursor( NULL, IDC_SIZENESW );
		break;
		case DEHT_BOTTOMMIDDLE:
			cursor = LoadCursor( NULL, IDC_SIZENS );
		break;
		case DEHT_BOTTOMRIGHT:
			cursor = LoadCursor( NULL, IDC_SIZENWSE );
		break;
		case DEHT_LEFTMIDDLE:
			cursor = LoadCursor( NULL, IDC_SIZEWE );
		break;
		case DEHT_RIGHTMIDDLE:
			cursor = LoadCursor( NULL, IDC_SIZEWE );
		break;
	}
	return cursor;
}

void CDiagramEntity::DrawSelectionMarkers( CDC* dc, CRect rect ) const
/* ============================================================
	Function :		CDiagramEntity::DrawSelectionMarkers
	Description :	Draws the selection markers for the 
					object.
	Access :		Protected

					
	Return :		void
	Parameters :	CDC* dc		-	The "CDC" to draw to
					CRect rect	-	The real object rectangle.
					
	Usage :			"rect" is the true rectangle (zoomed) of the 
					object.

   ============================================================*/
{

	// Draw selection markers
	CRect rectSelect;

	dc->SelectStockObject( BLACK_BRUSH );
	rectSelect = GetSelectionMarkerRect( DEHT_TOPLEFT, rect );
	dc->Rectangle( rectSelect );

	rectSelect = GetSelectionMarkerRect( DEHT_TOPMIDDLE, rect );
	dc->Rectangle( rectSelect );

	rectSelect = GetSelectionMarkerRect( DEHT_TOPRIGHT, rect );
	dc->Rectangle( rectSelect );

	rectSelect = GetSelectionMarkerRect( DEHT_BOTTOMLEFT, rect );
	dc->Rectangle( rectSelect );

	rectSelect = GetSelectionMarkerRect( DEHT_BOTTOMMIDDLE, rect );
	dc->Rectangle( rectSelect );

	rectSelect = GetSelectionMarkerRect( DEHT_BOTTOMRIGHT, rect );
	dc->Rectangle( rectSelect );

	rectSelect = GetSelectionMarkerRect( DEHT_RIGHTMIDDLE, rect );
	dc->Rectangle( rectSelect );

	rectSelect = GetSelectionMarkerRect( DEHT_LEFTMIDDLE, rect );
	dc->Rectangle( rectSelect );

}

CRect CDiagramEntity::GetSelectionMarkerRect( UINT marker, CRect rect ) const
/* ============================================================
	Function :		CDiagramEntity::GetSelectionMarkerRect
	Description :	Gets the selection marker rectangle for 
					marker, given the true object rectangle 
					"rect".
	Access :		Protected

					
	Return :		CRect		-	The marker rectangle
	Parameters :	UINT marker	-	The marker type ("DEHT_"-
									constants defined in 
									DiargramEntity.h)
					CRect rect	-	The object rectangle
					
	Usage :			"marker" can be one of the following:
						"DEHT_NONE" No hit-point
						"DEHT_BODY" Inside object body
						"DEHT_TOPLEFT" Top-left corner
						"DEHT_TOPMIDDLE" Middle top-side
						"DEHT_TOPRIGHT" Top-right corner
						"DEHT_BOTTOMLEFT" Bottom-left corner
						"DEHT_BOTTOMMIDDLE" Middle bottom-side
						"DEHT_BOTTOMRIGHT" Bottom-right corner
						"DEHT_LEFTMIDDLE" Middle left-side
						"DEHT_RIGHTMIDDLE" Middle right-side

   ============================================================*/
{

	CRect rectMarker;
	int horz = m_markerSize.cx / 2;
	int vert = m_markerSize.cy / 2;

	switch( marker )
	{
		case DEHT_TOPLEFT:
			rectMarker.SetRect( rect.left - horz,
								rect.top - vert,
								rect.left + horz,
								rect.top + vert );
		break;

		case DEHT_TOPMIDDLE:
			rectMarker.SetRect( rect.left + ( rect.Width() / 2 ) - horz,
								rect.top - vert,
								rect.left + ( rect.Width() / 2 ) + horz,
								rect.top + vert );
		break;

		case DEHT_TOPRIGHT:
			rectMarker.SetRect( rect.right - horz,
								rect.top - vert,
								rect.right + horz,
								rect.top + vert );
		break;

		case DEHT_BOTTOMLEFT:
			rectMarker.SetRect( rect.left - horz,
								rect.bottom - vert,
								rect.left + horz,
								rect.bottom + vert );
		break;

		case DEHT_BOTTOMMIDDLE:
			rectMarker.SetRect( rect.left + ( rect.Width() / 2 ) - horz,
								rect.bottom - vert,
								rect.left + ( rect.Width() / 2 ) + horz,
								rect.bottom + vert );
		break;

		case DEHT_BOTTOMRIGHT:
			rectMarker.SetRect( rect.right - horz,
								rect.bottom - vert,
								rect.right + horz,
								rect.bottom + vert );
		break;

		case DEHT_LEFTMIDDLE:
			rectMarker.SetRect( rect.left - horz,
								rect.top + ( rect.Height() / 2 ) - vert,
								rect.left + horz,
								rect.top + ( rect.Height() / 2 ) + vert );
		break;

		case DEHT_RIGHTMIDDLE:
			rectMarker.SetRect( rect.right - horz,
								rect.top + ( rect.Height() / 2 ) - vert,
								rect.right + horz,
								rect.top + ( rect.Height() / 2 ) + vert );
		break;
	}

	return rectMarker;

}

void CDiagramEntity::SetParent( CDiagramEntityContainer * parent )
/* ============================================================
	Function :		CDiagramEntity::SetParent
	Description :	Set the container owning the object.
	Access :		Protected

	Return :		void
	Parameters :	CDiagramEntityContainer * parent	-	the 
															parent.
					
	Usage :			Call to set the parent of the object. 
					Objects must know their parent, to allow 
					copying etc. 

   ============================================================*/
{

	m_parent = parent;

}

void CDiagramEntity::GetFont( LOGFONT& lf ) const
/* ============================================================
	Function :		CDiagramEntity::GetFont
	Description :	Returns the system GUI font in a "LOGFONT" 
					scaled to the zoom level of the object.
	Access :		Protected

					
	Return :		void
	Parameters :	LOGFONT& lf	-	The "LOGFONT" for the system
									GUI font.
					
	Usage :			Call to get the system font. Note that MS 
					Sans Serif will not scale below 8 points.

   ============================================================*/
{

	HFONT hfont = ( HFONT ) ::GetStockObject( DEFAULT_GUI_FONT );
	CFont* font = CFont::FromHandle( hfont );
	font->GetLogFont( &lf );
	lf.lfHeight = round( static_cast< double >( lf.lfHeight ) * m_zoom );

}

CString CDiagramEntity::GetType() const
/* ============================================================
	Function :		CDiagramEntity::GetType
	Description :	Returns the object type.
	Access :		Public

	Return :		CString	-	The type of the object.
	Parameters :	none

	Usage :			Call to get the type of the object. The type 
					is used when saving and loading objects 
					to/from a text file.

   ============================================================*/
{
	return m_type;
}

void CDiagramEntity::SetType( CString type )
/* ============================================================
	Function :		CDiagramEntity::SetType
	Description :	Set the object type.
	Access :		Public

	Return :		void
	Parameters :	CString type	-	The type to set
					
	Usage :			Call to set the object type - normally in 
					the "ctor" of this object. The type is used 
					when saving and loading objects to/from a 
					text file.

   ============================================================*/
{
	m_type = type;
}

CString CDiagramEntity::GetTitle() const
/* ============================================================
	Function :		CDiagramEntity::GetTitle
	Description :	Gets the Title property
	Access :		Public

	Return :		CString	-	The current title
	Parameters :	none

	Usage :			Call to get the title of the object. Title 
					is a property that the object can use in
					whatever way it wants.

   ============================================================*/
{

	return m_title;

}

void CDiagramEntity::SetTitle( CString title )
/* ============================================================
	Function :		CDiagramEntity::SetTitle
	Description :	Sets the Title property
	Access :		Public

	Return :		void
	Parameters :	CString title	-	The new title
					
	Usage :			Call to set the title of the object. Title 
					is a property that the object can use in
					whatever way it wants.

   ============================================================*/
{

	m_title = title;

}

CString CDiagramEntity::GetName() const
/* ============================================================
	Function :		CDiagramEntity::GetName
	Description :	Gets the Name property
	Access :		Public

	Return :		CString	-	The current name
	Parameters :	none

	Usage :			Call to get the name of the object. Name is 
					a property that the object can use in
					whatever way it wants.


   ============================================================*/
{

	return m_name;

}

void CDiagramEntity::SetName( CString name )
/* ============================================================
	Function :		CDiagramEntity::SetName
	Description :	Sets the Name property
	Access :		Public

	Return :		void
	Parameters :	CString name	-	The new name
					
	Usage :			Call to set the name of the object. Name is 
					a property that the object can use in
					whatever way it wants.

   ============================================================*/
{

	m_name = name;

}

double CDiagramEntity::GetLeft() const
/* ============================================================
	Function :		CDiagramEntity::GetLeft
	Description :	Gets the left edge of the object rectangle
	Access :		Public

	Return :		double	-	Left position
	Parameters :	none

	Usage :			Call to get the left edge of the object. 
					Note that if minimum sizes are not set for 
					the object, the left edge might be bigger 
					than the right.
					The object coordinates are expressed as 
					double values to allow unlimited zoom.

   ============================================================*/
{

	return m_left;

}

double CDiagramEntity::GetRight() const
/* ============================================================
	Function :		CDiagramEntity::GetRight
	Description :	Gets the right edge of the object 
					rectangle
	Access :		Public

	Return :		double	-	Right position
	Parameters :	none

	Usage :			Call to get the right edge of the object.
					Note that if minimum sizes are not set for 
					the object, the left edge might be bigger 
					than the right.
					The object coordinates are expressed as 
					double values to allow unlimited zoom.

   ============================================================*/
{

	return m_right;

}

double CDiagramEntity::GetTop() const
/* ============================================================
	Function :		CDiagramEntity::GetTop
	Description :	Gets the top edge of the object rectangle
	Access :		Public

	Return :		double	-	Top position
	Parameters :	none

	Usage :			Call to get the top edge of the object.
					Note that if minimum sizes are not set for 
					the object, the top edge might be bigger 
					than the bottom.
					The object coordinates are expressed as 
					double values to allow unlimited zoom.

   ============================================================*/
{

	return m_top;

}

double CDiagramEntity::GetBottom() const
/* ============================================================
	Function :		CDiagramEntity::GetBottom
	Description :	Gets the bottom edge of the object 
					rectangle
	Access :		Public

	Return :		double	-	Bottom postion
	Parameters :	none

	Usage :			Call to get the bottom edge of the object.
					Note that if minimum sizes are not set for 
					the object, the top edge might be bigger 
					than the bottom.
					The object coordinates are expressed as 
					double values to allow unlimited zoom.

   ============================================================*/
{

	return m_bottom;

}

void CDiagramEntity::SetLeft( double left )
/* ============================================================
	Function :		CDiagramEntity::SetLeft
	Description :	Sets the left edge of the object rectangle
	Access :		Public

	Return :		void
	Parameters :	double left	-	New left position
					
	Usage :			Call to set the left edge of the object.
					The object coordinates are expressed as 
					double values to allow unlimited zoom.

   ============================================================*/
{

	m_left = left;

}

void CDiagramEntity::SetRight( double right )
/* ============================================================
	Function :		CDiagramEntity::SetRight
	Description :	Sets the right edge of the object 
					rectangle
	Access :		Public

	Return :		void
	Parameters :	double right	-	New right position
					
	Usage :			Call to set the right edge of the object.
					The object coordinates are expressed as 
					double values to allow unlimited zoom.

   ============================================================*/
{

	m_right = right;

}

void CDiagramEntity::SetTop( double top )
/* ============================================================
	Function :		CDiagramEntity::SetTop
	Description :	Sets the top edge of the object rectangle
	Access :		Public

	Return :		void
	Parameters :	double top	-	New top position
					
	Usage :			Call to set the top edge of the object.
					The object coordinates are expressed as 
					double values to allow unlimited zoom.

   ============================================================*/
{
if( top == m_bottom )
	top = top;
	m_top = top;

}

void CDiagramEntity::SetBottom( double bottom )
/* ============================================================
	Function :		CDiagramEntity::SetBottom
	Description :	Sets the bottom edge of the object 
					rectangle
	Access :		Public

	Return :		void
	Parameters :	double bottom	-	New bottom position
					
	Usage :			Call to set the bottom edge of the object.
					The object coordinates are expressed as 
					double values to allow unlimited zoom.

   ============================================================*/
{

	m_bottom = bottom;

}

void CDiagramEntity::SetMarkerSize( CSize markerSize )
/* ============================================================
	Function :		CDiagramEntity::SetMarkerSize
	Description :	Gets the size of selection markers
	Access :		Protected
	Access :		Public

	Return :		void
	Parameters :	CSize markerSize	-	The new size of a 
											selection marker
					
	Usage :			Call to set a new selection marker size for 
					the object.

   ============================================================*/
{

	m_markerSize = markerSize;

}

CSize CDiagramEntity::GetMarkerSize() const
/* ============================================================
	Function :		CDiagramEntity::GetMarkerSize
	Description :	Gets the size of selection marker
	Access :		Protected
	Access :		Public

	Return :		CSize	-	The current size of a 
								selection marker
	Parameters :	none

	Usage :			Call to get the selection marker size for
					the object.

   ============================================================*/
{

	return m_markerSize;

}

void CDiagramEntity::SetMinimumSize( CSize minimumSize )
/* ============================================================
	Function :		CDiagramEntity::SetMinimumSize
	Description :	Sets the minimum size for instances of 
					this object.
	Access :		Public

	Return :		void
	Parameters :	CSize minimumSize	-	The minimum allowed 
											size
					
	Usage :			Call to set the minimum size of the object.
					It is not possible to resize an object to a 
					size smaller than the minimum allowed size.

   ============================================================*/
{

	m_minimumSize = minimumSize;

}

CSize CDiagramEntity::GetMinimumSize() const
/* ============================================================
	Function :		CDiagramEntity::GetMinimumSize
	Description :	Gets the minimum size for instances of 
					this object.
	Access :		Public

	Return :		CSize	-	The minimum allowed size
	Parameters :	none

	Usage :			Call to get the minimum size of the object.
					It is not possible to resize an object to a 
					size smaller than the minimum allowed size.

   ============================================================*/
{

	return m_minimumSize;

}

void CDiagramEntity::SetMaximumSize( CSize maximumSize )
/* ============================================================
	Function :		CDiagramEntity::SetMaximumSize
	Description :	Sets the maximum size for instances of 
					this object.
	Access :		Public

	Return :		void
	Parameters :	CSize maximumSize	-	The maximum allowed 
											size.
					
	Usage :			Call to set the maximum size of the object.
					It is not possible to resize an object to a 
					size larger than the maximum allowed size.

   ============================================================*/
{

	m_maximumSize = maximumSize;

}

CSize CDiagramEntity::GetMaximumSize() const
/* ============================================================
	Function :		CDiagramEntity::GetMaximumSize
	Description :	Returns the maximum size for instances of 
					this object.
	Access :		Public

	Return :		CSize	-	The maximum allowed size.
	Parameters :	none

	Usage :			Call to get the maximum size of the object.
					It is not possible to resize an object to a 
					size larger than the maximum allowed size.

   ============================================================*/
{

	return m_maximumSize;

}

void CDiagramEntity::SetConstraints( CSize min, CSize max )
/* ============================================================
	Function :		CDiagramEntity::SetConstraints
	Description :	Sets the minimum and maximum sizes for 
					instances of this object. -1 means no 
					constraints.
	Access :		Public

	Return :		void
	Parameters :	CSize min	-	Minimum size
					CSize max	-	Maximum size
					
	Usage :			Call to set the minimum and maximum sizes 
					of the object.
					It is not possible to resize an object to 
					smaller or bigger than the min- and max 
					size.

   ============================================================*/
{

	m_minimumSize = min;
	m_maximumSize = max;

}

CDiagramEntityContainer* CDiagramEntity::GetParent() const
/* ============================================================
	Function :		CDiagramEntity::GetParent
	Description :	Returns a pointer to the parent container.
	Access :		Protected

	Return :		CDiagramEntityContainer*	-	Parent
													container.
	Parameters :	none

	Usage :			Call to get the parent of the object.

   ============================================================*/
{

	return m_parent;

}

void CDiagramEntity::SetAttributeDialog( CDiagramPropertyDlg* dlg, UINT dlgresid )
/* ============================================================
	Function :		CDiagramEntity::SetAttributeDialog
	Description :	Sets the property dialog pointer.
	Access :		Protected

	Return :		void
	Parameters :	CDiagramPropertyDlg* dlg	-	a pointer 
													to a dialog 
													instance. 
					UINT dlgresid				-	The resource 
													id of the 
													dialog template.
					
	Usage :			Call to set a property dialog for the object 
					(normally in the "ctor"). 

   ============================================================*/
{

	m_propertydlg = dlg;
	m_propertydlgresid = dlgresid;

	if( dlg )
		m_propertydlg->SetEntity( this );

}

CDiagramPropertyDlg* CDiagramEntity::GetAttributeDialog() const
/* ============================================================
	Function :		CDiagramEntity::GetAttributeDialog
	Description :	Returns a pointer to the class property 
					dialog.
	Access :		Protected

	Return :		CDiagramPropertyDlg*	-	The dialog 
												pointer. "NULL" 
												if none.
	Parameters :	none

	Usage :			Call to get a pointer to the object property 
					dialog.

   ============================================================*/
{

	return m_propertydlg;

}

double CDiagramEntity::GetZoom() const
/* ============================================================
	Function :		CDiagramEntity::GetZoom
	Description :	Returns the zoom level for the object.
	Access :		Public

	Return :		double	-	
	Parameters :	none

	Usage :			Internal function. Can be called by derived 
					classes to get the zoom level. The zoom 
					level is set by the owning editor when 
					drawing the object, is read-only and this 
					function should only be called from "Draw".

   ============================================================*/
{

	return m_zoom;

}

void CDiagramEntity::SetZoom( double zoom )
/* ============================================================
	Function :		CDiagramEntity::SetZoom
	Description :	Sets the zoom level
	Access :		Protected

	Return :		nothing
	Parameters :	double zoom	-	The new zoom level
					
	Usage :			Internal call.

   ============================================================*/
{
	m_zoom = zoom;
}

int CDiagramEntity::GetGroup() const
/* ============================================================
	Function :		CDiagramEntity::GetGroup
	Description :	Gets the object group.
	Access :		Public

	Return :		int	-	Group of object
	Parameters :	none

	Usage :			Call to get the group for the object. All 
					objects in a group are selected together.

   ============================================================*/
{

	return m_group;

}

void CDiagramEntity::SetGroup( int group )
/* ============================================================
	Function :		CDiagramEntity::SetGroup
	Description :	Sets the object group to "group".
	Access :		Public

	Return :		void
	Parameters :	int group	-	New group to set
					
	Usage :			Call to set a group for the object. All 
					objects in a group are selected together.

   ============================================================*/
{

	m_group = group;

}

CString CDiagramEntity::Export( UINT /*format*/ ) const
/* ============================================================
	Function :		CDiagramEntity::Export
	Description :	Exports the object to format
	Access :		Public

	Return :		CString		-	The object representation 
									in format.
	Parameters :	UINT format	-	The format to export to.
					
	Usage :			Virtual function to allow easy exporting of 
					the objects to different text based formats.

   ============================================================*/
{

	return _T( "" );

}

int CDiagramEntity::GetHitCode( const CPoint& point, const CRect& rect ) const
/* ============================================================
	Function :		CDiagramEntity::GetHitCode
	Description :	Returns the hit point constant for "point" 
					assuming the object rectangle "rect".
	Access :		Public

	Return :		int				-	The hit point, 
										"DEHT_NONE" if none.
	Parameters :	CPoint point	-	The point to check
					CRect rect		-	The rect to check
					
	Usage :			Call to see in what part of the object point 
					lies. The hit point can be one of the following:
						"DEHT_NONE" No hit-point
						"DEHT_BODY" Inside object body
						"DEHT_TOPLEFT" Top-left corner
						"DEHT_TOPMIDDLE" Middle top-side
						"DEHT_TOPRIGHT" Top-right corner
						"DEHT_BOTTOMLEFT" Bottom-left corner
						"DEHT_BOTTOMMIDDLE" Middle bottom-side
						"DEHT_BOTTOMRIGHT" Bottom-right corner
						"DEHT_LEFTMIDDLE" Middle left-side
						"DEHT_RIGHTMIDDLE" Middle right-side

   ============================================================*/
{

	int result = DEHT_NONE;

	if( rect.PtInRect( point ) )
		result = DEHT_BODY;

	CRect rectTest;

	rectTest = GetSelectionMarkerRect( DEHT_TOPLEFT, rect );
	if( rectTest.PtInRect( point ) )
		result = DEHT_TOPLEFT;

	rectTest = GetSelectionMarkerRect( DEHT_TOPMIDDLE, rect );
	if( rectTest.PtInRect( point ) )
		result = DEHT_TOPMIDDLE;

	rectTest = GetSelectionMarkerRect( DEHT_TOPRIGHT, rect );
	if( rectTest.PtInRect( point ) )
		result = DEHT_TOPRIGHT;

	rectTest = GetSelectionMarkerRect( DEHT_BOTTOMLEFT, rect );
	if( rectTest.PtInRect( point ) )
		result = DEHT_BOTTOMLEFT;

	rectTest = GetSelectionMarkerRect( DEHT_BOTTOMMIDDLE, rect );
	if( rectTest.PtInRect( point ) )
		result = DEHT_BOTTOMMIDDLE;

	rectTest = GetSelectionMarkerRect( DEHT_BOTTOMRIGHT, rect );
	if( rectTest.PtInRect( point ) )
		result = DEHT_BOTTOMRIGHT;

	rectTest = GetSelectionMarkerRect( DEHT_LEFTMIDDLE, rect );
	if( rectTest.PtInRect( point ) )
		result = DEHT_LEFTMIDDLE;

	rectTest = GetSelectionMarkerRect( DEHT_RIGHTMIDDLE, rect );
	if( rectTest.PtInRect( point ) )
		result = DEHT_RIGHTMIDDLE;

	return result;

}

