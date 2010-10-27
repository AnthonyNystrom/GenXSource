//// =================================================================
/*!	\file GUILayoutBox.h

	Revision History:

	\par [9.8.2007]	13:43 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __FRAMEWORKGUI_GUILAYOUTBOX__
#define __FRAMEWORKGUI_GUILAYOUTBOX__

#include "GUITypes.h"
#include "GUILayout.h"

//! \brief The class placing the child controls of the control to which it is set in a horizontal or vertical line.
class GUILayoutBox: public GUILayout
{
public:

	// ***************************************************
	//! \brief    	Insert a cell into the layout
	//! \param      _index - cell index before which a new one should be inserted
	// ***************************************************
	void InsertCell(int32 _index);

	// ***************************************************
	//! \brief    	Remove a cell
	//! \param      _index - cell index
	// ***************************************************
	void RemoveCell(int32 _index);

	// ***************************************************
	//! \brief    	Add a control to the layout
	//! \param		_control	- the control being added to the layout
	//! \param      _index		- layout cell index. If there is no such cell, it is added together with all the preceding ones.
	//! \param      _sizeInCell - the number of cells occupied by the control.
	// ***************************************************
	void Add(GUIControl * _control, int32 _index, int32 _sizeInCell);

	// ***************************************************
	//! \brief    	Remove the control from the layout
	//! \param		_control- control to be removed from the layout
	// ***************************************************
	int32 Remove(GUIControl * _control);

	// ***************************************************
	//! \brief    	Remove the control from the layout
	//! \param      _index - Index of cell to remove.
	// ***************************************************
	void  Remove(int32 _index);

	// ***************************************************
	//! \brief    	Set cell size
	//! \param      _index - Index of cell.
	//! \param      _size - Cell size (if it is -1 then it is counted, if more than it is constant).
	// ***************************************************
	void SetSize(int32 _index, int32 _size);

	// ***************************************************
	//! \brief    	Get size of the specified cell
	//! \param      _index - cell index
	//! \return   	Size of the specified cell.
	// ***************************************************
	int32 GetSize(int32 _index);

	// ***************************************************
	//! \brief    	Clear - remove all the controls and cells from the Layout
	// ***************************************************
	virtual void Clear();

	// ***************************************************
	//! \brief    	Constructor.
	//! \param      _orientation - orientation
	// ***************************************************
	GUILayoutBox(eGUIOrientation _orientation);
	virtual ~GUILayoutBox();


	// ***************************************************
	//! \brief    	RecalcLayout - recount child controls' coordinates
	// ***************************************************
	virtual void RecalcLayout(GUIControl * _control);

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool IsClass(uint32 classType) const;

protected:

	eGUIOrientation orientation;

	List<Point> posList;
	Vector<CellProp> cellProp;
};

#endif // __FRAMEWORKGUI_GUILAYOUTBOX__
