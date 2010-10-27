//// =================================================================
/*!	\file GUILayoutGrid.h

	Revision History:

	\par [9.8.2007]	13:43 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUILAYOUTGRID_H__
#define __FRAMEWORK_GUILAYOUTGRID_H__

#include "GUILayout.h"

//! \brief It's a class that arranges the child controls in a rectangle table.
class GUILayoutGrid : public GUILayout
{
public:

	// ***************************************************
	//! \brief    	GUIGridLayout - constructor
	// ***************************************************
	GUILayoutGrid();

	virtual ~GUILayoutGrid();

	// ***************************************************
	//! \brief    	InsertCell - Insert a column into the layout.
	//! \param      column - The number of the column before which a new one should be inserted.
	// ***************************************************
	void InsertColumn(int32 column);

	// ***************************************************
	//! \brief    	InsertCell - insert a row into the layout
	//! \param      row - the number of the row before which a new one should be inserted
	// ***************************************************
	void InsertRow(int32 row);

	// ***************************************************
	//! \brief    	RemoveCell - remove column
	//! \param      column - column index
	// ***************************************************
	void RemoveColumn(int32 column);

	// ***************************************************
	//! \brief    	RemoveCell - remove row
	//! \param      row - row index
	// ***************************************************
	void RemoveRow(int32 row);

	// ***************************************************
	//! \brief    	Add - add a control to the layout
	//! \param		_control - the control being added to the layout
	//! \param      column - layout column index. If there is no such column, it is added together with all the preceding ones.
	//! \param      row - layout row index. If there is no such row, it is added together with all the preceding ones.
	//! \param      _sizeInColumn - the number of columns occupied by the control.
	//! \param      _sizeInRow - the number of rows occupied by the control.
	// ***************************************************
	void Add(GUIControl * _control, int32 column, int32 row, int32 _sizeInColumn, int32 _sizeInRow);

	// ***************************************************
	//! \brief    	Remove - remove the control from the layout
	//! \param		_control - control to be removed from the layout
	//! \param      column - column index
	//! \param      row - row index
	// ***************************************************
	void Remove(GUIControl * _control, int32 & column, int32 & row);

	// ***************************************************
	//! \brief    	Remove - remove the control from the layout
	//! \param      column - column index
	//! \param      row - row index
	// ***************************************************
	void Remove(int32 column, int32 row);

	// ***************************************************
	//! \brief    	Set column size
	//! \param      _column - Column.
	///! \param      _size - Column size (if it is -1 then it is counted, if it is more than it is constant).
	// ***************************************************
	void SetColumnSize(int32 _column, int32 _size);

	// ***************************************************
	//! \brief    	SetRowSize - Set row size
	//! \param      row - Row.
	//! \param      _size - Row size (if it is -1 then it is counted, if it is more than it is constant).
	// ***************************************************
	void SetRowSize(int32 row, int32 _size);

	// ***************************************************
	//! \brief    	Get column size
	//! \param      _column - Column.
	//! \return		Column size.
	// ***************************************************
	int32 GetColumnSize(int32 _column);

	// ***************************************************
	//! \brief    	Set row size
	//! \param      row - Row.
	//! \return     Row size.
	// ***************************************************
	int32 GetRowSize(int32 row);

	// ***************************************************
	//! \brief    	Clear - remove all the controls and cells from the Layout
	// ***************************************************
	virtual void Clear();

	// ***************************************************
	//! \brief    	RecalcLayout - recount child controls' coordinates
	// ***************************************************
	virtual void RecalcLayout(GUIControl * control);

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool IsClass(uint32 classType) const;
protected:

	List<Rect> posList;

	Vector<CellProp> rowProp;
	Vector<CellProp> columnProp;
};

#endif // __FRAMEWORK_GUILAYOUTGRID_H__