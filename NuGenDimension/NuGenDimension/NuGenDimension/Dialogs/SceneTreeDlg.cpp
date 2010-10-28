// SceneTreeDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "SceneTreeDlg.h"
#include "..//Drawer.h"

#include "..//NuGenDimensionView.h"

typedef enum
{
	NDT_NONE,
	NDT_OBJECT,
	NDT_WORK_PLANE,
	NDT_LAYER
} NODE_DATA_TYPE;

class CNodeData
{
public:
	NODE_DATA_TYPE  m_type;
	CNodeData() {m_type = NDT_NONE;};
	virtual ~CNodeData() {};
};

class CObjectNodeData : public CNodeData
{
public:
	sgCObject**  m_object;
	CObjectNodeData(sgCObject** ooo) {m_type = NDT_OBJECT;m_object=ooo;};
	virtual ~CObjectNodeData() {};
};

class CWorkPlaneNodeData : public CNodeData
{
public:
	unsigned int  m_wp_number;
	CWorkPlaneNodeData(unsigned int nn) {m_type = NDT_WORK_PLANE;m_wp_number=nn;};
	virtual ~CWorkPlaneNodeData() {};
};

class CLayerNodeData : public CNodeData
{
public:
	unsigned int  m_l_number;
	CLayerNodeData(unsigned int nn) {m_type = NDT_LAYER;m_l_number=nn;};
	virtual ~CLayerNodeData() {};
};

CGlobalTree::CGlobalTree():CBirch()
{
	CString resStr;
	resStr.LoadString(IDS_WORK_PLANES);
	m_work_planes_node	= InsertChild( HTOPNODE, resStr );
	resStr.LoadString(IDS_LAYERS);
	m_layers_node	= InsertChild( HTOPNODE, resStr );
	resStr.LoadString(IDS_OBJCTS);
	m_objects_node	= InsertChild( HTOPNODE, resStr );
	m_objects_node->bOpen = true;
	m_editing_regime = false;
	m_editing_object_node = NULL;	
	m_wnd   =  NULL;

/*	m_vis_obj_bitmap = new CBitmap32();
	m_vis_obj_bitmap->LoadBitmap(IDB_VISIBLE_OBJ_TC);
	m_invis_obj_bitmap= new CBitmap32();
	m_invis_obj_bitmap->LoadBitmap(IDB_INVISIBLE_OBJ_TC);*/

	m_vis_obj_bitmap = new CBitmap32();
	m_vis_obj_bitmap->LoadBitmap(IDB_VISIBLE_OBJ_TC);

	m_invis_obj_bitmap= new CBitmap32();
	m_invis_obj_bitmap->LoadBitmap(IDB_INVISIBLE_OBJ_TC);

	m_radio_check_bmp= new CBitmap32();
	m_radio_check_bmp->LoadBitmap(IDB_RADIO_CHECK_BMP);

	m_radio_uncheck_bmp= new CBitmap32();
	m_radio_uncheck_bmp->LoadBitmap(IDB_RADIO_UNCHECK_BMP);

	m_unck_obj_bmp = new CBitmap32();
	m_unck_obj_bmp->LoadBitmap(IDB_UNCK_OBJ_BMP);


	m_objects_bmp = new CBitmap32();
	m_objects_bmp->LoadBitmap(IDB_OBJECTS_BMP);

	m_objects_node->item_bitmaps.push_back(m_objects_bmp);

	m_layers_bmp = new CBitmap32();
	m_layers_bmp->LoadBitmap(IDB_LAYERS_BMP);

	m_layers_node->item_bitmaps.push_back(m_layers_bmp);

	m_wp_bmp = new CBitmap32();
	m_wp_bmp->LoadBitmap(IDB_WP_BMP);

	m_work_planes_node->item_bitmaps.push_back(m_wp_bmp);


	resStr.LoadString(IDS_XY_WP);
	HTREENODE tmpNode  = InsertChild( m_work_planes_node, resStr );
	m_wp_nodes.push_back(tmpNode);
	tmpNode->userData = new CWorkPlaneNodeData(0);
	tmpNode->bAutoDeleteUserData = true;
	m_wp_bmps[0] = new CBitmap32();
	m_wp_bmps[0]->LoadBitmap(IDB_XY_WP_BMP);
	tmpNode->item_bitmaps.push_back(m_radio_check_bmp);
	tmpNode->item_bitmaps.push_back(m_vis_obj_bitmap);
	tmpNode->item_bitmaps.push_back(m_wp_bmps[0]);

	CString NormStr;
	NormStr.LoadString(IDS_NORMAL);
	HTREENODE tmpNode2 = InsertChild(tmpNode,NormStr);
	InsertChild(tmpNode2,"X = 0.0000");
	InsertChild(tmpNode2,"Y = 0.0000");
	InsertChild(tmpNode2,"Z = 1.0000");
	tmpNode = InsertChild(tmpNode,"D = 0.0000");
	//tmpNode->bEditable = true;
	

	resStr.LoadString(IDS_XZ_WP);
	tmpNode = InsertChild( m_work_planes_node, resStr );
	m_wp_nodes.push_back(tmpNode);
	tmpNode->userData = new CWorkPlaneNodeData(1);
	tmpNode->bAutoDeleteUserData = true;
	m_wp_bmps[1] = new CBitmap32();
	m_wp_bmps[1]->LoadBitmap(IDB_XZ_WP_BMP);
	tmpNode->item_bitmaps.push_back(m_radio_uncheck_bmp);
	tmpNode->item_bitmaps.push_back(m_invis_obj_bitmap);
	tmpNode->item_bitmaps.push_back(m_wp_bmps[1]);
	tmpNode2 = InsertChild(tmpNode,NormStr);
	InsertChild(tmpNode2,"X = 0.0000");
	InsertChild(tmpNode2,"Y = 1.0000");
	InsertChild(tmpNode2,"Z = 0.0000");
	tmpNode = InsertChild(tmpNode,"D = 0.0000");
	//tmpNode->bEditable = true;

	resStr.LoadString(IDS_YZ_WP);
	tmpNode = InsertChild( m_work_planes_node, resStr );
	m_wp_nodes.push_back(tmpNode);
	tmpNode->userData = new CWorkPlaneNodeData(2);
	tmpNode->bAutoDeleteUserData = true;
	m_wp_bmps[2] = new CBitmap32();
	m_wp_bmps[2]->LoadBitmap(IDB_YZ_WP_BMP);
	tmpNode->item_bitmaps.push_back(m_radio_uncheck_bmp);
	tmpNode->item_bitmaps.push_back(m_invis_obj_bitmap);
	tmpNode->item_bitmaps.push_back(m_wp_bmps[2]);
	tmpNode2 = InsertChild(tmpNode,NormStr);
	InsertChild(tmpNode2,"X = 1.0000");
	InsertChild(tmpNode2,"Y = 0.0000");
	InsertChild(tmpNode2,"Z = 0.0000");
	tmpNode = InsertChild(tmpNode,"D = 0.0000");
	//tmpNode->bEditable = true;
	
	m_work_planes_node->bOpen = true;
}

CGlobalTree::~CGlobalTree() 
{
	delete m_vis_obj_bitmap ;
	delete m_invis_obj_bitmap;
	delete m_objects_bmp;
	delete m_layers_bmp;
	delete m_wp_bmp;
	delete m_radio_check_bmp;
	delete m_radio_uncheck_bmp;
	delete m_wp_bmps[0];
	delete m_wp_bmps[1];
	delete m_wp_bmps[2];
	delete m_unck_obj_bmp;
}


void   CGlobalTree::SetIconsToNode(HTREENODE nd)
{
	if (nd->userData==NULL)
		return;
	CNodeData* nD = static_cast<CNodeData*>(nd->userData);

	if (nD->m_type!=NDT_OBJECT)
		return;
	
	CObjectNodeData* onD = reinterpret_cast<CObjectNodeData*>(nD);
	sgCObject* obb = *(onD->m_object);

	if (m_vis_obj_bitmap==NULL || m_invis_obj_bitmap==NULL)
	{
		APP_SWITCH_RESOURCE
		if (m_vis_obj_bitmap==NULL)
		{
			m_vis_obj_bitmap = new CBitmap32();
			m_vis_obj_bitmap->LoadBitmap(IDB_VISIBLE_OBJ_TC);
		}
		if (m_invis_obj_bitmap==NULL)
		{
			m_invis_obj_bitmap= new CBitmap32();
			m_invis_obj_bitmap->LoadBitmap(IDB_INVISIBLE_OBJ_TC);
		}
	}

	if (theApp.m_main_pluginer && obb)
	{
		std::string obID(obb->GetUserGeometryID());
		std::map<std::string, OBJ_SUPPORTER>::const_iterator itr = 
			theApp.m_main_pluginer->m_objects_supporter.find(obID);
		if (itr!=theApp.m_main_pluginer->m_objects_supporter.end())
		{
			if ((*itr).second.object_icon)
			{
				((CTreeNode*)nd)->item_bitmaps.push_back((*itr).second.object_icon);
			}
		}
		else
		{
			((CTreeNode*)nd)->item_bitmaps.push_back(m_unck_obj_bmp);
		}
	
		CBitmap32* bbb = NULL;
		if (obb->GetAttribute(SG_OA_DRAW_STATE)&SG_DS_HIDE)
			bbb = m_invis_obj_bitmap;
		else
			bbb = m_vis_obj_bitmap;
		((CTreeNode*)nd)->item_bitmaps.push_back(bbb);
	}
}

ISceneTreeControl::TREENODEHANDLE  CGlobalTree::AddNode(sgCObject** obj, 
														  ISceneTreeControl::TREENODEHANDLE par)
{
	HTREENODE  res = NULL;
	if (m_editing_regime && m_editing_object_node)
	{
		res = InsertSibling(m_editing_object_node,(*obj)?((*obj)->GetName()):("Object") );
		m_editing_regime = false;
		m_editing_object_node = NULL;
	}
	else
		res = InsertChild( (par)?((CTreeNode*)par):m_objects_node, 
		(*obj)?((*obj)->GetName()):("Object") );
	if (res)
	{
		res->userData = new CObjectNodeData(obj);
		res->bAutoDeleteUserData = true;
		res->bEditable = true;
		SetIconsToNode(res);
	}
	if (m_wnd && m_wnd->IsWindowVisible())
		m_wnd->Invalidate();
	
	return res;
}

bool  CGlobalTree::ShowNode(sgCObject* obj)
{
	if (!obj)
		return false;
	if (sgGetFromObjectTreeNodeHandle(obj)==NULL)
		return false;

	const sgCObject* obPar = obj->GetParent();


	HTREENODE curCh = NULL;
	HTREENODE parHn = NULL;
	if (obPar)
	{
		parHn = (HTREENODE)sgGetFromObjectTreeNodeHandle(const_cast<sgCObject*>(obPar));
		if (!parHn)
			return false;
		curCh = parHn->pFirstChild;
	}
	else
	{
		parHn = m_objects_node;
		curCh = m_objects_node->pFirstChild;
	}
	while (curCh)
	{
		if (curCh->userData==NULL)
		{
			ASSERT(0);
			return false;
		}
		CNodeData* nD = static_cast<CNodeData*>(curCh->userData);

		if (nD->m_type!=NDT_OBJECT)
		{
			ASSERT(0);
			return false;
		}

		CObjectNodeData* onD = reinterpret_cast<CObjectNodeData*>(nD);
		sgCObject* obb = *(onD->m_object);
		if (obb==obj)
		{
			curCh->bVisible = true;
			sgSetToObjectTreeNodeHandle(obj,curCh);
			if (m_wnd && m_wnd->IsWindowVisible())
				m_wnd->Invalidate();
			return true;
		}
		curCh = curCh->pNextSibling;
	}
	return false;
}

bool  CGlobalTree::HideNode(TREENODEHANDLE nd)
{
	((CTreeNode*)nd)->bVisible =false;
	if (Drawer::CurrentEditableObject!=NULL)
	{
		if (((CTreeNode*)nd)->userData==NULL)
		{
			ASSERT(0);
			return false;
		}
		CNodeData* nD = static_cast<CNodeData*>(((CTreeNode*)nd)->userData);

		if (nD->m_type!=NDT_OBJECT)
		{
			ASSERT(0);
			return false;
		}

		CObjectNodeData* onD = reinterpret_cast<CObjectNodeData*>(nD);
		sgCObject* obb = *(onD->m_object);
		if (obb==Drawer::CurrentEditableObject)
		{
			m_editing_regime = true;
			m_editing_object_node = (CTreeNode*)nd;
		}
	}
	if (m_wnd && m_wnd->IsWindowVisible())
		m_wnd->Invalidate();
	return true;
}

bool  CGlobalTree::UpdateNode(ISceneTreeControl::TREENODEHANDLE nd)
{
	if (((CTreeNode*)nd)->userData==NULL)
	{
		ASSERT(0);
		return false;
	}
	CNodeData* nD = static_cast<CNodeData*>(((CTreeNode*)nd)->userData);

	if (nD->m_type!=NDT_OBJECT)
	{
		ASSERT(0);
		return false;
	}

	CObjectNodeData* onD = reinterpret_cast<CObjectNodeData*>(nD);
	sgCObject* obb = *(onD->m_object);

	((CTreeNode*)nd)->csLabel = obb->GetName();
	if (((CTreeNode*)nd)->item_bitmaps.size()==0)
	{
		SetIconsToNode((CTreeNode*)nd);
	}
	if (m_wnd && m_wnd->IsWindowVisible())
		m_wnd->Invalidate();
	return true;
}

bool  CGlobalTree::RemoveNode(ISceneTreeControl::TREENODEHANDLE nd)
{
	if (((CTreeNode*)nd)->userData==NULL)
	{
		ASSERT(0);
		return false;
	}
	CNodeData* nD = static_cast<CNodeData*>(((CTreeNode*)nd)->userData);

	if (nD->m_type!=NDT_OBJECT)
	{
		ASSERT(0);
		return false;
	}

	CObjectNodeData* onD = reinterpret_cast<CObjectNodeData*>(nD);
	sgCObject* obb = *(onD->m_object);

	DeleteNode((CTreeNode*)nd);
	sgSetToObjectTreeNodeHandle(obb,NULL);
	if (m_wnd && m_wnd->IsWindowVisible())
		m_wnd->Invalidate();
	return true;
}

bool CGlobalTree::ClearTree()
{
	DeleteSubTree(m_objects_node);
	if (m_wnd && m_wnd->IsWindowVisible())
		m_wnd->Invalidate();
	return true;
}

void  CGlobalTree::SetCurrentWorkPlane(size_t nmbr)
{
	if (nmbr>2)
		return;
	for (size_t i=0;i<m_wp_nodes.size();i++)
		if (i==nmbr)
			m_wp_nodes[i]->item_bitmaps[0] = m_radio_check_bmp;
		else	
			m_wp_nodes[i]->item_bitmaps[0] = m_radio_uncheck_bmp;
	switch(nmbr) 
	{
	case 0:
		global_opengl_view->GetWorkPlanes()->EnableXWorkPlane(FALSE);
		global_opengl_view->GetWorkPlanes()->EnableYWorkPlane(FALSE);
		global_opengl_view->GetWorkPlanes()->EnableZWorkPlane(TRUE);
		m_wp_nodes[0]->item_bitmaps[1] = m_vis_obj_bitmap;
		global_opengl_view->GetWorkPlanes()->SetXWorkPlaneVisibles(FALSE);
		global_opengl_view->GetWorkPlanes()->SetYWorkPlaneVisibles(FALSE);
		global_opengl_view->GetWorkPlanes()->SetZWorkPlaneVisibles(TRUE);
		m_wp_nodes[1]->item_bitmaps[1] = m_invis_obj_bitmap;
		m_wp_nodes[2]->item_bitmaps[1] = m_invis_obj_bitmap;
		break;
	case 1:
		global_opengl_view->GetWorkPlanes()->EnableXWorkPlane(FALSE);
		global_opengl_view->GetWorkPlanes()->EnableYWorkPlane(TRUE);
		global_opengl_view->GetWorkPlanes()->EnableZWorkPlane(FALSE);
		m_wp_nodes[1]->item_bitmaps[1] = m_vis_obj_bitmap;
		global_opengl_view->GetWorkPlanes()->SetXWorkPlaneVisibles(FALSE);
		global_opengl_view->GetWorkPlanes()->SetYWorkPlaneVisibles(TRUE);
		global_opengl_view->GetWorkPlanes()->SetZWorkPlaneVisibles(FALSE);
		m_wp_nodes[0]->item_bitmaps[1] = m_invis_obj_bitmap;
		m_wp_nodes[2]->item_bitmaps[1] = m_invis_obj_bitmap;
		break;
	case 2:
		global_opengl_view->GetWorkPlanes()->EnableXWorkPlane(TRUE);
		global_opengl_view->GetWorkPlanes()->EnableYWorkPlane(FALSE);
		global_opengl_view->GetWorkPlanes()->EnableZWorkPlane(FALSE);
		m_wp_nodes[2]->item_bitmaps[1] = m_vis_obj_bitmap;
		global_opengl_view->GetWorkPlanes()->SetXWorkPlaneVisibles(TRUE);
		global_opengl_view->GetWorkPlanes()->SetYWorkPlaneVisibles(FALSE);
		global_opengl_view->GetWorkPlanes()->SetZWorkPlaneVisibles(FALSE);
		m_wp_nodes[0]->item_bitmaps[1] = m_invis_obj_bitmap;
		m_wp_nodes[1]->item_bitmaps[1] = m_invis_obj_bitmap;
		break;
	default:
		ASSERT(0);
	}
	global_opengl_view->Invalidate();
	m_wnd->Invalidate(FALSE);
}

void   CGlobalTree::ChangeWorkPlaneVisible(size_t nmbr)
{
	if (nmbr>2)
		return;
	switch(nmbr) 
	{
	case 0:
		if (!global_opengl_view->GetWorkPlanes()->IsZWorkPlaneEnable())
			return;
		if (global_opengl_view->GetWorkPlanes()->IsZWorkPlaneVisible())
		{
			m_wp_nodes[nmbr]->item_bitmaps[1] = m_invis_obj_bitmap;
			global_opengl_view->GetWorkPlanes()->SetZWorkPlaneVisibles(FALSE);
		}
		else
		{
			m_wp_nodes[nmbr]->item_bitmaps[1] = m_vis_obj_bitmap;
			global_opengl_view->GetWorkPlanes()->SetXWorkPlaneVisibles(FALSE);
			global_opengl_view->GetWorkPlanes()->SetYWorkPlaneVisibles(FALSE);
			global_opengl_view->GetWorkPlanes()->SetZWorkPlaneVisibles(TRUE);
			m_wp_nodes[1]->item_bitmaps[1] = m_invis_obj_bitmap;
			m_wp_nodes[2]->item_bitmaps[1] = m_invis_obj_bitmap;
		}
		break;
	case 1:
		if (!global_opengl_view->GetWorkPlanes()->IsYWorkPlaneEnable())
			return;
		if (global_opengl_view->GetWorkPlanes()->IsYWorkPlaneVisible())
		{
			m_wp_nodes[nmbr]->item_bitmaps[1] = m_invis_obj_bitmap;
			global_opengl_view->GetWorkPlanes()->SetYWorkPlaneVisibles(FALSE);
		}
		else
		{
			m_wp_nodes[nmbr]->item_bitmaps[1] = m_vis_obj_bitmap;
			global_opengl_view->GetWorkPlanes()->SetXWorkPlaneVisibles(FALSE);
			global_opengl_view->GetWorkPlanes()->SetYWorkPlaneVisibles(TRUE);
			global_opengl_view->GetWorkPlanes()->SetZWorkPlaneVisibles(FALSE);
			m_wp_nodes[0]->item_bitmaps[1] = m_invis_obj_bitmap;
			m_wp_nodes[2]->item_bitmaps[1] = m_invis_obj_bitmap;
		}
		break;
	case 2:
		if (!global_opengl_view->GetWorkPlanes()->IsXWorkPlaneEnable())
			return;
		if (global_opengl_view->GetWorkPlanes()->IsXWorkPlaneVisible())
		{
			m_wp_nodes[nmbr]->item_bitmaps[1] = m_invis_obj_bitmap;
			global_opengl_view->GetWorkPlanes()->SetXWorkPlaneVisibles(FALSE);
		}
		else
		{
			m_wp_nodes[nmbr]->item_bitmaps[1] = m_vis_obj_bitmap;
			global_opengl_view->GetWorkPlanes()->SetXWorkPlaneVisibles(TRUE);
			global_opengl_view->GetWorkPlanes()->SetYWorkPlaneVisibles(FALSE);
			global_opengl_view->GetWorkPlanes()->SetZWorkPlaneVisibles(FALSE);
			m_wp_nodes[1]->item_bitmaps[1] = m_invis_obj_bitmap;
			m_wp_nodes[0]->item_bitmaps[1] = m_invis_obj_bitmap;
		}
		break;
	default:
		ASSERT(0);
	}
	global_opengl_view->Invalidate();
	m_wnd->Invalidate(FALSE);
}

void   CGlobalTree::ChangeObjectVisible(sgCObject* ob,bool vis)
{
	if (vis)
		((HTREENODE)sgGetFromObjectTreeNodeHandle(ob))->item_bitmaps[1]=m_vis_obj_bitmap;
	else
		((HTREENODE)sgGetFromObjectTreeNodeHandle(ob))->item_bitmaps[1]=m_invis_obj_bitmap;
	m_wnd->Invalidate(FALSE);
}

void  CMyBirchCtrl::StartEditLabel(HTREENODE pNode, CString& newLabel)
{
	if (pNode->userData==NULL)
	{
		return;
	}
	CNodeData* nD = static_cast<CNodeData*>(pNode->userData);

	if (nD->m_type!=NDT_OBJECT)
	{
		return;
	}

	CObjectNodeData* onD = reinterpret_cast<CObjectNodeData*>(nD);
	sgCObject* obb = *(onD->m_object);

	newLabel = obb->GetName();
}

void  CMyBirchCtrl::FinishEditLabel(HTREENODE pNode, CString& newLabel)
{
	if (pNode->userData==NULL)
	{
		return;
	}
	CNodeData* nD = static_cast<CNodeData*>(pNode->userData);

	if (nD->m_type!=NDT_OBJECT)
	{
		return;
	}

	CObjectNodeData* onD = reinterpret_cast<CObjectNodeData*>(nD);
	sgCObject* obb = *(onD->m_object);

	obb->SetName(newLabel);
	pNode->csLabel = obb->GetName();
}

void  CMyBirchCtrl::ClickOnIcon(HTREENODE pNode, unsigned int iconNumb)
{
	if (pNode->userData==NULL)
		return;
	CNodeData* nD = static_cast<CNodeData*>(pNode->userData);
	if (nD->m_type==NDT_WORK_PLANE)
	{
		CWorkPlaneNodeData* wpnD = reinterpret_cast<CWorkPlaneNodeData*>(nD);
		if (iconNumb==0)
			theApp.m_main_tree_control->SetCurrentWorkPlane(wpnD->m_wp_number);
		if (iconNumb==1)
			theApp.m_main_tree_control->ChangeWorkPlaneVisible(wpnD->m_wp_number);
	}

	if (nD->m_type==NDT_OBJECT && iconNumb==1)
	{
		CObjectNodeData* onD = reinterpret_cast<CObjectNodeData*>(nD);
		sgCObject* obb = *(onD->m_object);
		if (obb->GetAttribute(SG_OA_DRAW_STATE)&SG_DS_HIDE)
		{
			obb->SetAttribute(SG_OA_DRAW_STATE,obb->GetAttribute(SG_OA_DRAW_STATE)& ~SG_DS_HIDE);
			theApp.m_main_tree_control->ChangeObjectVisible(obb,true);
		}
		else
		{
		    obb->SetAttribute(SG_OA_DRAW_STATE,obb->GetAttribute(SG_OA_DRAW_STATE)|SG_DS_HIDE);
			theApp.m_main_tree_control->ChangeObjectVisible(obb,false);
		}
		global_opengl_view->Invalidate();
	}
}


// CSceneTreeDlg dialog

IMPLEMENT_DYNAMIC(CSceneTreeDlg, CDialog)
CSceneTreeDlg::CSceneTreeDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSceneTreeDlg::IDD, pParent)
{
}

CSceneTreeDlg::~CSceneTreeDlg()
{
}

void CSceneTreeDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CSceneTreeDlg, CDialog)
	ON_WM_SIZE()
END_MESSAGE_MAP()


// CSceneTreeDlg message handlers

void CSceneTreeDlg::OnOK()
{
}

void CSceneTreeDlg::OnCancel()
{
}

BOOL CSceneTreeDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	CRect clR;
	GetWindowRect(clR);

	m_tree.CreateEx(WS_EX_CLIENTEDGE,NULL,NULL,WS_CHILD|WS_VISIBLE,clR,this,1001);

	m_tree.SetBirch(theApp.m_main_tree_control);

	if (theApp.m_main_tree_control)
		theApp.m_main_tree_control->SetWnd(&m_tree);

	m_tree
		.SetTextFont( 8, FALSE, FALSE, "MS Shell Dlg" )
		.SetDefaultTextColor( RGB(0,64,128) );

	// Tree data
	
	//m_ST_Tree.SetLineHeight(30);

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CSceneTreeDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialog::OnSize(nType, cx, cy);

	if (::IsWindow(m_tree.m_hWnd))
	{
		m_tree.MoveWindow(0,0,cx,cy);
	}
}
