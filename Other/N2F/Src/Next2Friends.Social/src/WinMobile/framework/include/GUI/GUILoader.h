//// =================================================================
/*!	\file GUILoader.h

	Revision History:

	\par [9.8.2007]	13:48 by Ivan Petrochenko
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUILOADER_H__
#define __FRAMEWORK_GUILOADER_H__

#include "framework/BaseTypes.h"
#include "framework/List.h"

class ResourceSystem;
class GUISystem;
class GUIControl;

/* \brief класс GUILoader.
	«агрузчик бинарного ресурса gui
*/
class GUILoader
{
public:
	GUILoader(ResourceSystem * _resSys, GUISystem * _guiSys);
	~GUILoader() {}


	// *************************************************************
	//! \brief    	—оздать контролы из ресурса
	//! 
	//! \param[in]  resourceID - id ресурса
	//! \return   	true если все control'ы созданы успешно, иначе false
	// *************************************************************
	bool	CreateControls(uint16 resourceID);

	// *************************************************************
	//! \brief    	”далить контролы, созданные из ресурса
	//! 
	//! \param[in]  resourceID - id ресурса
	//! \return   	true если все control'ы созданы успешно, иначе false
	// *************************************************************
	bool	RemoveControls(uint16 resourceID);

private:	
	enum eControls
	{
		EC_SCREEN = 0,
		EC_BUTTON,
		EC_BUTTONTEXT,
		EC_STATICTEXT,
		EC_PROGRESS,
		EC_SLIDER,
		EC_LAYOUT,
		EC_CONTROL,
		EC_SCROLLVIEW,
		EC_CONTAINER,
		EC_IMAGE,
		EC_ANIMATION,
		EC_COMBOBOX,
		EC_TEXTVIEW,
		EC_POPUP,
		EC_CHECKBOX,
		EC_RADIO
	};

	enum eLayouts
	{
		EL_BOX = 0,
		EL_GRID
	};

	struct BinControl
	{
		uint32 id;
		uint32 parentId;
		uint16 type;
		uint16 subtype;
		uint32 attribute;
		uint32 skinId;
		int32  posX;
		int32  posY;
		uint32 width;
		uint32 height;
		uint32 paramCount;
	};

	int32 GetParam(BinControl * ctrl, uint32 index);

	ResourceSystem	* resSys;
	GUISystem		* guiSys;

	bool	AddControl(GUIControl * control, BinControl * ctrl);

	bool	CreateScreen(BinControl * ctrl);
	bool	CreateButton(BinControl * ctrl);
	bool	CreateButtonText(BinControl * ctrl);
	bool	CreateLayout(BinControl * ctrl);
	bool	CreateStaticText(BinControl * ctrl);
	bool	CreateScrollbar(BinControl * ctrl);
	bool	CreateScrollview(BinControl * ctrl);
	bool	CreateTextview(BinControl * ctrl);
	bool	CreateProgressbar(BinControl * ctrl);
	bool	CreateSlider(BinControl * ctrl);
	bool	CreateControl(BinControl * ctrl);
	bool	CreateContainer(BinControl * ctrl);
	bool	CreateImage(BinControl * ctrl);
	bool	CreateAnimation(BinControl * ctrl);
	bool	CreateCombobox(BinControl * ctrl);
	bool	CreatePopup(BinControl * ctrl);
	bool	CreateCheckbox(BinControl * ctrl);
	bool	CreateRadioContainer(BinControl * ctrl);
};

#endif //__FRAMEWORK_GUILOADER_H__

