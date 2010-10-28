/* ==========================================================================
	Class :			CRulerMeasurementsDialog

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-25

	Purpose :		"CRulerMeasurementsDialog" is derived from "CDialog" and 
					allows the changing of the measurement units for the 
					application.

	Description :	Standard ClassWizard-created dialog class.

	Usage :			Created by "CCornerBox".

   ========================================================================*/

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "RulerMeasurementsDialog.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CRulerMeasurementsDialog dialog


CRulerMeasurementsDialog::CRulerMeasurementsDialog(CWnd* pParent /*=NULL*/)
	: CDialog(CRulerMeasurementsDialog::IDD, pParent)
/* ============================================================
	Function :		CRulerMeasurementsDialog::CRulerMeasurementsDialog
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	CWnd* pParent	-	Parent of dialog
					
	Usage :			

   ============================================================*/
{
	//{{AFX_DATA_INIT(CRulerMeasurementsDialog)
	m_measurements = 0;
	//}}AFX_DATA_INIT
}

void CRulerMeasurementsDialog::DoDataExchange(CDataExchange* pDX)
/* ============================================================
	Function :		CRulerMeasurementsDialog::DoDataExchange
	Description :	MFC data exchange handler.
	Access :		Protected

	Return :		void
	Parameters :	CDataExchange* pDX	-	Data exchange object
					
	Usage :			Called from MFC.

   ============================================================*/
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CRulerMeasurementsDialog)
	DDX_CBIndex(pDX, IDC_REPORT_COMBO_MEASUREMENTS, m_measurements);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CRulerMeasurementsDialog, CDialog)
	//{{AFX_MSG_MAP(CRulerMeasurementsDialog)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CRulerMeasurementsDialog message handlers
