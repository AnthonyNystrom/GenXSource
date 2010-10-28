// LayersDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "LayersDlg.h"
#include ".\layersdlg.h"


// CLayersDlg dialog

IMPLEMENT_DYNAMIC(CLayersDlg, CDialog)
CLayersDlg::CLayersDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CLayersDlg::IDD, pParent)
{
}

CLayersDlg::~CLayersDlg()
{
}

void CLayersDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LAYERS_LIST, m_layers_list);
}


BEGIN_MESSAGE_MAP(CLayersDlg, CDialog)
	ON_NOTIFY(LVN_ITEMCHANGED, IDC_LAYERS_LIST, OnLvnItemchangedLayersList)
	ON_BN_CLICKED(IDC_ADD_LAYER, OnBnClickedAddLayer)
END_MESSAGE_MAP()


// CLayersDlg message handlers

BOOL CLayersDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	m_layers_list.SetExtendedStyle(m_layers_list.GetExtendedStyle()
		|LVS_EX_CHECKBOXES|LVS_EX_GRIDLINES);


	CString  tttRes;

	tttRes.LoadString(IDS_LAYER_VIS);
	m_layers_list.InsertColumn(0,tttRes,LVCFMT_LEFT, 70);
	tttRes.LoadString(IDS_LAYER_LABEL);
	m_layers_list.InsertColumn(1,tttRes,LVCFMT_LEFT, 200);

	size_t sz = 1;//sgGetScene()->GetLayersCount();
	
	CString  ttt;
	
	m_layers.clear();
	for (size_t i=0;i<sz;i++)
	{
		m_layers_list.InsertItem(i,"");
		ttt.Format(" %i",i);
		m_layers_list.SetItem(i, 1, LVIF_TEXT, tttRes+ttt, 0, 0,0,0);
		bool  lv = true;//sgGetScene()->GetLayerVisible(i);
		m_layers_list.SetCheck(i, lv);
		m_layers.push_back((lv)?1:0);
	}
	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CLayersDlg::OnLvnItemchangedLayersList(NMHDR *pNMHDR, LRESULT *pResult)
{
	if (!IsWindowVisible())
	{
		*pResult = 0;
		return;
	}
	LPNMLISTVIEW pNMLV = reinterpret_cast<LPNMLISTVIEW>(pNMHDR);
	CString ttt;
	if((pNMLV->uChanged & LVIF_STATE) != 0) // изменилось состояние
	{
		if((pNMLV->uNewState & 0x2000) != 0) // поставили check
		{
			m_layers[pNMLV->iItem]=1;
		}
		else if((pNMLV->uNewState & 0x1000) != 0) // убрали check
		{
			m_layers[pNMLV->iItem]=0;
		}
	}

	*pResult = 0;
}

void CLayersDlg::OnBnClickedAddLayer()
{
	m_layers.push_back(1);
	CString  tttRes,ttt;
	tttRes.LoadString(IDS_LAYER_LABEL);
	int sz = m_layers.size()-1;
	m_layers_list.InsertItem(sz,"");
	ttt.Format(" %i",sz);
	m_layers_list.SetItem(sz, 1, LVIF_TEXT, tttRes+ttt, 0, 0,0,0);
	m_layers_list.SetCheck(sz, TRUE);
}

void CLayersDlg::OnOK()
{
	/*size_t thisLayersCount = m_layers.size();
	size_t sceneLayersCount = sgGetScene()->GetLayersCount();
	ASSERT(thisLayersCount>=sceneLayersCount);
	for (size_t i=sceneLayersCount;i<thisLayersCount;i++)
		sgGetScene()->AddLayer(true);
	sceneLayersCount = sgGetScene()->GetLayersCount();
	ASSERT(thisLayersCount==sceneLayersCount);
	for (size_t i=0;i<sceneLayersCount;i++)
		sgGetScene()->SetLayerVisible(i, m_layers[i]!=0);*/
	CDialog::OnOK();
}
