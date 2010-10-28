#pragma once
#include "afxwin.h"
// CObjSysParamsDlg dialog
#include "..//Controls//ColorPickerCB.h"
//#include "..//Controls//BitmapPickerCombo.h"
#include "..//Controls//LineThiknessCombo.h"
#include "..//Controls//LineStyleCombo.h"


#include "..//Controls//OptionTree//OptionTree.h"

class CObjSysParamsDlg : public CDialog
{
	DECLARE_DYNCREATE(CObjSysParamsDlg)
private:
	COptionTree m_otTree;

	COptionTreeItemEdit             *m_otiEdit;  
	COptionTreeItemComboBox         *m_otiLayerCombo; 
	COptionTreeItemColorComboBox    *m_otiColorCombo;
	COptionTreeItemLineThikComboBox *m_otiThicknessCombo;
	COptionTreeItemLineTypeComboBox *m_otiTypeCombo;
	
	COptionTreeItemCheckBox         *m_otiVis_check;
	COptionTreeItemCheckBox         *m_otiGabVis_check;
	
	COptionTreeItemRadio         *m_otiFrameOrSolidVis_radio;
	
	sgCObject*        m_editing_object;
public:
	CObjSysParamsDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CObjSysParamsDlg();

	void    SetEditingObject(sgCObject* ob)
	{
		ASSERT(ob);
		ASSERT(!m_editing_object);
		m_editing_object = ob;
	}
// Overrides
// Dialog Data
	enum { IDD = IDD_SYSTEMS_PARAMS_OBJ_DLG };
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	DECLARE_MESSAGE_MAP()
private:
	CComboBox m_layer_combo;
private:
	
	CLineThiknessCombo m_line_thickness_combo;
	CLineThiknessCombo      m_line_type_combo;
public:
	afx_msg void OnSize(UINT nType, int cx, int cy);
protected:
	virtual void OnOK();
};
