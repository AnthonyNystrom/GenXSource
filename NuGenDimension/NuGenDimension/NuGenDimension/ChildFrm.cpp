// ChildFrm.cpp : implementation of the CChildFrame class
//
#include "stdafx.h"
#include "NuGenDimension.h"

#include "ChildFrm.h"
#include "Drawer.h" 
#include "Dialogs//DlgRender.h" 
#include ".\childfrm.h"
#include "RayTracing//RTMaterials.h"
//#include "RayTracing//rtDialog.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

ICommander*    global_commander = NULL;
// CChildFrame

IMPLEMENT_DYNCREATE(CChildFrame, CMDIChildWnd)

BEGIN_MESSAGE_MAP(CChildFrame, CMDIChildWnd)
	ON_WM_CREATE()
	ON_COMMAND(ID_WORK_PLANES_SETUPS, OnWorkPlanesSetups)
	ON_COMMAND(ID_X_WP_SHOW, OnXWpShow)
	ON_UPDATE_COMMAND_UI(ID_X_WP_SHOW, OnUpdateXWpShow)
	ON_COMMAND(ID_Y_WP_SHOW, OnYWpShow)
	ON_UPDATE_COMMAND_UI(ID_Y_WP_SHOW, OnUpdateYWpShow)
	ON_COMMAND(ID_Z_WP_SHOW, OnZWpShow)
	ON_UPDATE_COMMAND_UI(ID_Z_WP_SHOW, OnUpdateZWpShow)
	ON_WM_CHAR()
	ON_COMMAND(ID_EDIT_UNDO, OnEditUndo)
	ON_UPDATE_COMMAND_UI(ID_EDIT_UNDO, OnUpdateEditUndo)
	ON_COMMAND(ID_EDIT_REDO, OnEditRedo)
	ON_UPDATE_COMMAND_UI(ID_EDIT_REDO, OnUpdateEditRedo)
	ON_COMMAND(ID_SNAP_NO, OnSnapNo)
	ON_UPDATE_COMMAND_UI(ID_SNAP_NO, OnUpdateSnapNo)
	ON_COMMAND(ID_SNAP_POINTS, OnSnapPoints)
	ON_UPDATE_COMMAND_UI(ID_SNAP_POINTS, OnUpdateSnapPoints)
	ON_COMMAND(ID_SNAP_ENDS, OnSnapEnds)
	ON_UPDATE_COMMAND_UI(ID_SNAP_ENDS, OnUpdateSnapEnds)
	ON_COMMAND(ID_SNAP_MIDS, OnSnapMids)
	ON_UPDATE_COMMAND_UI(ID_SNAP_MIDS, OnUpdateSnapMids)
	ON_COMMAND(ID_DELETE_HOT_OBJECT, OnDeleteHotObject)
	ON_COMMAND(ID_FIT_ONE_OBJ, OnFitHotObject)
	ON_COMMAND(ID_THIS_PROJECTION_ON_2D, OnThisProjectionOn2D)
	ON_COMMAND(ID_SET_MATERIAL_TO_OBJ, OnSetMaterialToObj)
	ON_UPDATE_COMMAND_UI(ID_SET_MATERIAL_TO_OBJ, OnUpdateSetMaterialToObj)
	ON_COMMAND(ID_UNSET_MATERIAL, OnUnSetMaterialToObj)
	ON_UPDATE_COMMAND_UI(ID_UNSET_MATERIAL, OnUpdateUnSetMaterialToObj)
	ON_COMMAND(ID_END_COMMANDER, OnEndCommander)
	ON_COMMAND(ID_OBJECT_PROPERTIES, OnObjectProperties)
	ON_COMMAND(ID_SNAP_CENTERS, OnSnapCenters)
	ON_UPDATE_COMMAND_UI(ID_SNAP_CENTERS, OnUpdateSnapCenters)
	ON_COMMAND(ID_LAYERS, OnLayers)
	ON_COMMAND(ID_ATTACH_MAT_LIB, OnAttachMatLib)
	ON_COMMAND(ID_MAT_EDITOR, OnMatEditor)
	ON_COMMAND(ID_DETACH_MAT_LIB, OnDetachMatLib)
	ON_UPDATE_COMMAND_UI(ID_DETACH_MAT_LIB, OnUpdateDetachMatLib)
	ON_COMMAND(ID_FONTS, OnFonts)
	ON_COMMAND(ID_FONTS_PREVIEW, OnFontsPreview)
	ON_UPDATE_COMMAND_UI(ID_FONTS_PREVIEW, OnUpdateFontsPreview)
	ON_COMMAND(ID_BREAK_HOT_GROUP, OnBreakHotGroup)
	ON_COMMAND(ID_BREAK_HOT_CONTOUR, OnBreakHotContour)
	ON_WM_MDIACTIVATE()
	ON_COMMAND(ID_RAY_TRACE_START, OnRayTraceStart)
END_MESSAGE_MAP()


// CChildFrame construction/destruction

CChildFrame::CChildFrame():
				m_view(NULL)
				,m_command_panel_page_index(-1)
				,m_snap_type(SNAP_NO)
				,m_commander(NULL)
				,m_active_plugin(-1)
{
}

CChildFrame::~CChildFrame()
{
}

BOOL CChildFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying the CREATESTRUCT cs
	if( !CMDIChildWnd::PreCreateWindow(cs) )
		return FALSE;

	
	return TRUE;
}


int CChildFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CMDIChildWnd::OnCreate(lpCreateStruct) == -1)
		return -1;

	return 0;
}

void CChildFrame::PutMessage(IApplicationInterface::MESSAGE_TYPE mes_type,
						const char* mes_str)
{
	CMainFrame*  mnFr = static_cast<CMainFrame*>(GetParentFrame());
	mnFr->PutMessageFromChildFrame(mes_type,mes_str);
}

IViewPort*  CChildFrame::GetViewPort()
{
	CNuGenDimensionView* v = static_cast<CNuGenDimensionView*>(GetActiveView());
	ASSERT(v);
	return v;
}

ICommandPanel*	CChildFrame::GetCommandPanel()
{
	CMainFrame*  mnFr = static_cast<CMainFrame*>(GetParentFrame());
	return mnFr->GetCommandPanel();
}

double  CChildFrame::ApplyPrecision(double val)
{
	SCENE_SETUPS  tmpSS;
	reinterpret_cast<CNuGenDimensionDoc*>(GetActiveDocument())->GetSceneSetups(tmpSS);
	double prec = precisions[tmpSS.CurrentPrecision];
	return (  prec*((int)((val/prec + ((val>0)?0.5:-0.5))))   );
}


void    CChildFrame::CopyAttributes(sgCObject& where_obj, const sgCObject& from_obj)
{
	where_obj.SetAttribute(SG_OA_COLOR,from_obj.GetAttribute(SG_OA_COLOR));
	where_obj.SetAttribute(SG_OA_LAYER,from_obj.GetAttribute(SG_OA_LAYER));
	where_obj.SetAttribute(SG_OA_LINE_THICKNESS,from_obj.GetAttribute(SG_OA_LINE_THICKNESS));
	where_obj.SetAttribute(SG_OA_LINE_TYPE,from_obj.GetAttribute(SG_OA_LINE_TYPE));
	where_obj.SetAttribute(SG_OA_DRAW_STATE,from_obj.GetAttribute(SG_OA_DRAW_STATE));

	where_obj.SetName(from_obj.GetName());
}

void    CChildFrame::ApplyAttributes(sgCObject* ooo)
{
	if (ooo && ooo->GetType()!=SG_OT_GROUP && global_3D_document)
	{ 
		SCENE_SETUPS tmpSS;
		global_3D_document->GetSceneSetups(tmpSS);
		ooo->SetAttribute(SG_OA_LAYER, tmpSS.CurrentLayer);
		ooo->SetAttribute(SG_OA_COLOR, tmpSS.CurrentColor);
		ooo->SetAttribute(SG_OA_LINE_THICKNESS, tmpSS.CurrentLineThickness);
		ooo->SetAttribute(SG_OA_LINE_TYPE, tmpSS.CurrentLineType);
	}
}

const int CChildFrame::GetSnapSize() const
{
	return (static_cast<CMainFrame*>(GetParentFrame())->GetCursorer())->GetCursorStructure()->size;
}

void   CChildFrame::StartCommander(const char* str)
{
}

BOOL CChildFrame::DestroyWindow()
{	
    FreeCommander();
	return __super::DestroyWindow();
}

void CChildFrame::FreeCommander()
{
	if (m_commander && 
		theApp.m_main_pluginer && 
		m_active_plugin<theApp.m_main_pluginer->m_toolbar_plugins.size())
	{
		theApp.m_main_pluginer->m_toolbar_plugins[m_active_plugin]->FreeCommander(m_commander);
		m_commander = NULL;
		m_active_plugin = -1;
		Drawer::CurrentEditableObject = NULL;
	}
	if (m_commander)  
	{
		delete m_commander;
		m_commander = NULL;
		m_active_plugin = -1;
	}
	
	PutMessage(IApplicationInterface::MT_MESSAGE,"    ");
	global_commander = NULL;
}

void  CChildFrame::SetView(CNuGenDimensionView* v)
{
	ASSERT(v);
	m_view = v;
	static_cast<CMainFrame*>(GetParentFrame())->SetView(v);
}

BOOL CChildFrame::OnCmdMsg(UINT nID, int nCode, void* pExtra, AFX_CMDHANDLERINFO* pHandlerInfo)
{
	if (pHandlerInfo == NULL)
	{
		if ((nID>=START_ID_FOR_COMMANDER_CONTEXT_MENU)&&(nID<START_ID_FOR_PLUGINS_MENU))
		{
			ASSERT(m_commander);
			if (nCode == CN_UPDATE_COMMAND_UI)
			{
				bool enbl, ch;
				m_commander->GetContextMenuInterface()->GetItemState(nID-START_ID_FOR_COMMANDER_CONTEXT_MENU,
							enbl,ch);
				((CCmdUI*)pExtra)->Enable(enbl);
				((CCmdUI*)pExtra)->SetCheck(ch);
				return TRUE;
			}
			else
				if (nCode == CN_COMMAND)
				{
					m_commander->GetContextMenuInterface()->Run(nID-START_ID_FOR_COMMANDER_CONTEXT_MENU);
					return TRUE;
				}
		}
		if (nID>=START_ID_FOR_PLUGINS_TOOLBARS)
		{
				size_t sz = (theApp.m_main_pluginer)?(theApp.m_main_pluginer->m_toolbar_plugins.size()):0;
				for (size_t i = 0; i < sz; i++)
				{
					if ((nCode == CN_UPDATE_COMMAND_UI) && 
						((nID>=theApp.m_main_pluginer->m_toolbar_plugins[i]->m_start_ID)&&
						(nID<=theApp.m_main_pluginer->m_toolbar_plugins[i]->m_end_ID)))
					{
						bool ch=false;
						bool enbl = false;
						theApp.m_main_pluginer->m_toolbar_plugins[i]->GetToolbarButtonState(nID,ch,enbl);
						if (m_view && m_view->IsPrintRegime())
							((CCmdUI*)pExtra)->Enable(FALSE);
						else
							((CCmdUI*)pExtra)->Enable(enbl);
						((CCmdUI*)pExtra)->SetCheck(ch);
						return TRUE;
					}
					if ((nID>=theApp.m_main_pluginer->m_toolbar_plugins[i]->m_start_ID)&&
						(nID<=theApp.m_main_pluginer->m_toolbar_plugins[i]->m_end_ID))
					{
						if (theApp.m_main_pluginer->m_toolbar_plugins[i]->m_was_load && (nCode == CN_COMMAND))
						{
							FreeCommander();
							m_commander = theApp.m_main_pluginer->m_toolbar_plugins[i]->GetNewCommander(nID,
																		this);
							global_commander = m_commander;
							if (m_commander)
							{
								m_active_plugin = i;
								m_commander->Start();
							}
							else
							{
								ASSERT(0);
								m_active_plugin = 0;
							}
							SetFocus();

						}
						return TRUE;
					}
				}
		}
	}

	return CMDIChildWnd::OnCmdMsg(nID, nCode, pExtra, pHandlerInfo);
}

void CChildFrame::OnEndCommander()
{
	PostMessage(WM_CHAR,VK_ESCAPE,0);
}

void CChildFrame::OnChar(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	switch(nChar) 
	{
	case VK_ESCAPE:
		FreeCommander();
		break;
	case VK_RETURN:
		//if (m_commander)
	//		m_commander->OnEnter();
		break;
	default:
		break;
		//m_Command_Bar.OnChar(nChar,nRepCnt,nFlags);
	}

	__super::OnChar(nChar, nRepCnt, nFlags);
}


// CChildFrame diagnostics

#ifdef _DEBUG
void CChildFrame::AssertValid() const
{
	CMDIChildWnd::AssertValid();
}

void CChildFrame::Dump(CDumpContext& dc) const
{
	CMDIChildWnd::Dump(dc);
}

#endif //_DEBUG

void CChildFrame::OnWorkPlanesSetups()
{
	ASSERT(m_view);
	 translate_messages_through_app = true;	
	m_view->GetWorkPlanes()->SetupDialog();
	 translate_messages_through_app = false;	
	m_view->Invalidate();
}

void CChildFrame::OnXWpShow()
{
	ASSERT(m_view);
	m_view->GetWorkPlanes()->EnableXWorkPlane(TRUE);
	m_view->GetWorkPlanes()->EnableYWorkPlane(FALSE);
	m_view->GetWorkPlanes()->EnableZWorkPlane(FALSE);
	m_view->Invalidate();
}

void CChildFrame::OnUpdateXWpShow(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_view->GetWorkPlanes()->IsXWorkPlaneEnable());
}

void CChildFrame::OnYWpShow()
{
	ASSERT(m_view);
	m_view->GetWorkPlanes()->EnableXWorkPlane(FALSE);
	m_view->GetWorkPlanes()->EnableYWorkPlane(TRUE);
	m_view->GetWorkPlanes()->EnableZWorkPlane(FALSE);
	m_view->Invalidate();
}

void CChildFrame::OnUpdateYWpShow(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_view->GetWorkPlanes()->IsYWorkPlaneEnable());
}

void CChildFrame::OnZWpShow()
{
	ASSERT(m_view);
	m_view->GetWorkPlanes()->EnableXWorkPlane(FALSE);
	m_view->GetWorkPlanes()->EnableYWorkPlane(FALSE);
	m_view->GetWorkPlanes()->EnableZWorkPlane(TRUE);
	m_view->Invalidate();
}

void CChildFrame::OnUpdateZWpShow(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_view->GetWorkPlanes()->IsZWorkPlaneEnable());
}


void CChildFrame::OnEditUndo()
{
	ASSERT(m_view);
	sgGetScene()->Undo();
	m_view->Invalidate();
}

void CChildFrame::OnUpdateEditUndo(CCmdUI *pCmdUI)
{
	pCmdUI->Enable(!sgGetScene()->IsUndoStackEmpty());
}

void CChildFrame::OnEditRedo()
{
	ASSERT(m_view);
	sgGetScene()->Redo();
	m_view->Invalidate();
}

void CChildFrame::OnUpdateEditRedo(CCmdUI *pCmdUI)
{
	pCmdUI->Enable(!sgGetScene()->IsRedoStackEmpty());
}

void CChildFrame::OnSnapNo()
{
	m_snap_type = SNAP_NO;
}

void CChildFrame::OnUpdateSnapNo(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_snap_type==SNAP_NO);
}

void CChildFrame::OnSnapPoints()
{
	m_snap_type = SNAP_POINTS;
}

void CChildFrame::OnUpdateSnapPoints(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_snap_type==SNAP_POINTS);
}

void CChildFrame::OnSnapEnds()
{
	m_snap_type = SNAP_ENDS;
}

void CChildFrame::OnUpdateSnapEnds(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_snap_type==SNAP_ENDS);
}

void CChildFrame::OnSnapMids()
{
	m_snap_type = SNAP_MIDS;
}

void CChildFrame::OnUpdateSnapMids(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_snap_type==SNAP_MIDS);
}

void  CChildFrame::CommanderContextMenu(int x,int y)
{
	CEGMenu PopupMenu;
	PopupMenu.CreatePopupMenu();

	int nItem = 0;
	
	unsigned int cmitms = m_commander->GetContextMenuInterface()->GetItemsCount();
	CString csMenu;
	if (cmitms)
		csMenu = GetLeftHalfOfString(ID_CANCEL_COMMANDER);
	else
		csMenu = GetLeftHalfOfString(ID_END_COMMANDER);
	
	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION , ID_END_COMMANDER, csMenu);

	
	if (cmitms>0)
		PopupMenu.InsertMenu(nItem++, MF_BYPOSITION|MF_SEPARATOR);
	CString tmpItS;
	
	for (unsigned int i=0;i<cmitms;i++)
	{
		m_commander->GetContextMenuInterface()->GetItem(i,tmpItS);
		PopupMenu.InsertMenu(nItem++, MF_BYPOSITION , 
			START_ID_FOR_COMMANDER_CONTEXT_MENU+i, tmpItS); 
		HBITMAP hhh= m_commander->GetContextMenuInterface()->GetItemBitmap(i);
		if (hhh)
			PopupMenu.SetMenuItemBitmap(START_ID_FOR_COMMANDER_CONTEXT_MENU+i,
											hhh);
	}

	PopupMenu.TrackPopupMenu(0,x,y,this);
}

void   CChildFrame::EditCommanderContextMenu(int x,int y)
{
	ASSERT(m_commander==NULL);

	CEGMenu PopupMenu;

	//PopupMenu.LoadMenu(IDR_CONTEXT_MENUS);
	PopupMenu.CreatePopupMenu();
	
	ASSERT(Drawer::CurrentHotObject);
	if (Drawer::CurrentHotObject)
	{
		std::string obID(Drawer::CurrentHotObject->GetUserGeometryID());

		if (theApp.m_main_pluginer)
		{
			std::map<std::string, OBJ_SUPPORTER>::const_iterator itr = 
				theApp.m_main_pluginer->m_objects_supporter.find(obID);
			if (itr!=theApp.m_main_pluginer->m_objects_supporter.end())
			{
					if (theApp.m_main_pluginer->m_toolbar_plugins[(*itr).second.plugin_index]->m_was_load)
					{
						m_commander = theApp.m_main_pluginer->m_toolbar_plugins[(*itr).second.plugin_index]->GetEditCommander(Drawer::CurrentHotObject,
											this);
						global_commander = m_commander;
						m_active_plugin = (*itr).second.plugin_index;
						SetFocus();
						goto brLab;
					}
			}
		}
	}
brLab:
	int nItem=0;
	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION , ID_OBJECT_PROPERTIES, GetLeftHalfOfString(ID_OBJECT_PROPERTIES));
	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION , ID_FIT_ONE_OBJ, GetLeftHalfOfString(ID_FIT_ONE_OBJ));
	CBitmap* mib1 = new CBitmap;
	mib1->LoadBitmap(IDB_FIT_OBJECT);
	PopupMenu.SetMenuItemBitmap(ID_FIT_ONE_OBJ,*mib1);
	delete mib1;

	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION , ID_DELETE_HOT_OBJECT, GetLeftHalfOfString(ID_DELETE_HOT_OBJECT));
	CBitmap* mib3 = new CBitmap;
	mib3->LoadBitmap(IDB_DEL_OBJ_TC);
	PopupMenu.SetMenuItemBitmap(ID_DELETE_HOT_OBJECT,*mib3);
	delete mib3;

	PopupMenu.InsertMenu(nItem++, MF_BYPOSITION , ID_THIS_PROJECTION_ON_2D, GetLeftHalfOfString(ID_THIS_PROJECTION_ON_2D));
	SG_OBJECT_TYPE objT = Drawer::CurrentHotObject->GetType();
	if (objT==SG_OT_GROUP)
	{
		PopupMenu.InsertMenu(nItem++, MF_BYPOSITION|MF_SEPARATOR);
		PopupMenu.InsertMenu(nItem++, MF_BYPOSITION , ID_BREAK_HOT_GROUP, GetLeftHalfOfString(ID_BREAK_HOT_GROUP));
	}
	if (objT==SG_OT_CONTOUR)
	{
		PopupMenu.InsertMenu(nItem++, MF_BYPOSITION|MF_SEPARATOR);
		PopupMenu.InsertMenu(nItem++, MF_BYPOSITION , ID_BREAK_HOT_CONTOUR, GetLeftHalfOfString(ID_BREAK_HOT_CONTOUR));
	}
	if (objT==SG_OT_3D)
	{
		PopupMenu.InsertMenu(nItem++, MF_BYPOSITION , ID_SET_MATERIAL_TO_OBJ, GetLeftHalfOfString(ID_SET_MATERIAL_TO_OBJ));
		PopupMenu.InsertMenu(nItem++, MF_BYPOSITION , ID_UNSET_MATERIAL, GetLeftHalfOfString(ID_UNSET_MATERIAL));
	}

	if (m_commander)
	{
		unsigned int cmitms = m_commander->GetContextMenuInterface()->GetItemsCount();
		if (cmitms>0)
			PopupMenu.InsertMenu(nItem++, MF_BYPOSITION|MF_SEPARATOR);
		CString tmpItS;
		for (unsigned int i=0;i<cmitms;i++)
		{
			m_commander->GetContextMenuInterface()->GetItem(i,tmpItS);
			/*PopupMenu.GetSubMenu(subMenu)->AppendMenu(MF_STRING, 
				START_ID_FOR_COMMANDER_CONTEXT_MENU+i, tmpItS); */
			PopupMenu.InsertMenu(nItem++, MF_BYPOSITION , 
				START_ID_FOR_COMMANDER_CONTEXT_MENU+i, tmpItS);

			/*PopupMenu.GetSubMenu(subMenu)->SetMenuItemBitmaps(START_ID_FOR_COMMANDER_CONTEXT_MENU+i,
				MF_BYCOMMAND,m_commander->GetContextMenuInterface()->GetItemBitmap(i),NULL);*/
			HBITMAP mBmp = m_commander->GetContextMenuInterface()->GetItemBitmap(i);
			if (mBmp)
				PopupMenu.SetMenuItemBitmap(START_ID_FOR_COMMANDER_CONTEXT_MENU+i,
					mBmp);
		}
	}

	UINT cmd=PopupMenu.TrackPopupMenu(TPM_RETURNCMD,x,y,this);
	if (cmd!=0)
	{
		SendMessage(WM_COMMAND,cmd,0);
		if ((cmd<START_ID_FOR_COMMANDER_CONTEXT_MENU)||(cmd>START_ID_FOR_PLUGINS_MENU))
			if (m_commander)
				FreeCommander();
	}	
	else
		if (m_commander)
			FreeCommander();
}

#include "Dialogs//ObjSysParamsDlg.h"
void CChildFrame::OnObjectProperties()
{
	ASSERT(Drawer::CurrentHotObject);
	CObjSysParamsDlg dlg;
	translate_messages_through_app = true;
	CNuGenDimensionDoc* dc = reinterpret_cast<CNuGenDimensionDoc*>(GetActiveDocument());
	dlg.SetEditingObject(Drawer::CurrentHotObject);
	if (dlg.DoModal()==IDOK)
	{	
	}
	translate_messages_through_app = false;
	GetActiveView()->Invalidate();
}

void CChildFrame::OnDeleteHotObject()
{
	ASSERT(Drawer::CurrentHotObject);
	sgGetScene()->StartUndoGroup();
	sgGetScene()->DetachObject(Drawer::CurrentHotObject);
	sgGetScene()->EndUndoGroup();
	Drawer::CurrentHotObject=NULL;
}

void CChildFrame::OnFitHotObject()
{
	ASSERT(Drawer::CurrentHotObject);
	SG_POINT a1,a2;
	Drawer::CurrentHotObject->GetGabarits(a1,a2);
	CNuGenDimensionView* v = static_cast<CNuGenDimensionView*>(GetActiveView());
	v->GetCamera()->FitBounds(a1.x,a1.y,a1.z,a2.x,a2.y,a2.z);
	v->Invalidate();
}

#include "ReportCreatorView.h"
void CChildFrame::OnThisProjectionOn2D()
{
	POSITION posit = theApp.m_pDocManager->GetFirstDocTemplatePosition();
	CDocTemplate* pTemplate = theApp.m_pDocManager->GetNextDocTemplate(posit);
	while (pTemplate)
	{
		POSITION docPos = pTemplate->GetFirstDocPosition();
		CDocument* curDoc =  pTemplate->GetNextDoc(docPos);
		POSITION viewPos = curDoc->GetFirstViewPosition();
		CView*  curView = curDoc->GetNextView(viewPos);	
		if (curView->GetRuntimeClass()==RUNTIME_CLASS(CReportCreatorView))
		{
			CReportCreatorView* rcv = reinterpret_cast<CReportCreatorView*>(curView);
			rcv->OnReportAddPage();
			static_cast<CMainFrame*>(GetParentFrame())->ActivateTab(pTemplate);

			CMetaFileDC * cDC = new CMetaFileDC();
			cDC->CreateEnhanced(GetDC(),NULL,NULL,"meta");
			//call draw routine here that makes GDI calls int cDC

			CNuGenDimensionView* v = static_cast<CNuGenDimensionView*>(GetActiveView());
			
			CRect rect;
			v->GetClientRect(&rect);

			C3dCamera   tempCamera = *(v->GetCamera());

			SG_POINT    hotObjMin,hotObjMax;
			Drawer::CurrentHotObject->GetGabarits(hotObjMin,hotObjMax);

			GLdouble lookAtOldCamX = 0.5*(hotObjMin.x+hotObjMax.x);
			GLdouble lookAtOldCamY = 0.5*(hotObjMin.y+hotObjMax.y);
			GLdouble lookAtOldCamZ = 0.5*(hotObjMin.z+hotObjMax.z);

			v->GetCamera()->m_bPerspective = false;
			v->GetCamera()->SetLookAtPos(lookAtOldCamX,lookAtOldCamY,lookAtOldCamZ);
			v->GetCamera()->SetEyePos(lookAtOldCamX,lookAtOldCamY,hotObjMax.z);
			v->GetCamera()->FitBounds(hotObjMin.x, hotObjMin.y, hotObjMin.z, 
											hotObjMax.x, hotObjMax.y, hotObjMax.z);
			// Set our camera position and save its transformation matrix
			v->GetCamera()->PositionCamera();
			
			Drawer::ProjectObjectOnMetaDC(Drawer::CurrentHotObject,
								cDC,
								v->GetCamera()->m_dModelViewMatrix,
								v->GetCamera()->m_dProjectionMatrix,
								v->GetCamera()->m_iViewport,
								rect.Height(),Drawer::PROJECT_LINES,
								30.0,
								1.0);
			
			tempCamera.ResetView();
			v->SetCamera(tempCamera);
			//close meta CMetafileDC and get its handle
			HENHMETAFILE hMF = cDC->CloseEnhanced();
			
			//CxImage* projWMF = new CxImage();
			//projWMF->Load("tmpMeta.wmf",CXIMAGE_FORMAT_WMF);

			rcv->InsertPictureToPage(rcv->GetCurrentPage(),hMF,5.0,5.0,40.0,40.0);

			delete cDC;

			return;
		}
		pTemplate = theApp.m_pDocManager->GetNextDocTemplate(posit);
	}
}

#include "Dialogs//MatOnObjectDlg.h"
void CChildFrame::OnSetMaterialToObj()
{
	if (Drawer::CurrentHotObject &&	Drawer::CurrentHotObject->GetType()==SG_OT_3D)
	{
		/*sgC3DObject* Ob=reinterpret_cast<sgC3DObject*>(Drawer::CurrentHotObject);
		SetMaterialToObject(Ob,MATERIAL_IMAGE_1);*/
		
		CMatOnObjectDlg dlg;
		dlg.SetObject(reinterpret_cast<sgC3DObject*>(Drawer::CurrentHotObject));
		if (dlg.DoModal()==IDOK)
			Invalidate();
	}
}

void CChildFrame::OnUpdateSetMaterialToObj(CCmdUI *pCmdUI)
{
}

void CChildFrame::OnUnSetMaterialToObj()
{
}

void CChildFrame::OnUpdateUnSetMaterialToObj(CCmdUI *pCmdUI)
{
}

void CChildFrame::OnSnapCenters()
{
	m_snap_type = SNAP_CENTERS;
}

void CChildFrame::OnUpdateSnapCenters(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_snap_type==SNAP_CENTERS);
}

#include "Dialogs//LayersDlg.h"
void CChildFrame::OnLayers()
{
	CLayersDlg dlg;
	if (dlg.DoModal()==IDOK)
	{
		static_cast<CMainFrame*>(GetParentFrame())->UpdateSystemToolbar();
		GetActiveView()->Invalidate();
	}
}

void CChildFrame::OnAttachMatLib()
{
	CString			Path;
Path.LoadString(IDS_OPEN_MAT_LIB_PATH);
	CFileDialog dlg(
		TRUE,
		NULL,								// Open File Dialog
		_T("*.ngm"),							// Default extension
		OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT,	// No default filename
		Path);// Filter string

	if (dlg.DoModal() != IDOK)
		return;
	Path = dlg.GetPathName();

	Drawer::MatLoader.AttachMatLibrary(Path);
}

void CChildFrame::OnMatEditor()
{
	UINT res = WinExec("MaterialsEditor.exe",SW_SHOW);
	if (res>31)
		return;
	CString mesS;
	switch(res) 
	{
	case ERROR_BAD_FORMAT:
		mesS.LoadString(IDS_BAD_MAT_EDITOR_FILE);
		AfxMessageBox(mesS);
		break;
	case ERROR_FILE_NOT_FOUND:
	case ERROR_PATH_NOT_FOUND:
	default:
		mesS.LoadString(IDS_MAT_EDITOR_NOT_FOUND);
		AfxMessageBox(mesS);
		break;
	}
}

void CChildFrame::OnDetachMatLib()
{
	Drawer::MatLoader.DetachMatLibrary();
	CNuGenDimensionView* v = static_cast<CNuGenDimensionView*>(GetActiveView());
	v->ResetHandAction();
}

void CChildFrame::OnUpdateDetachMatLib(CCmdUI *pCmdUI)
{
	pCmdUI->Enable(Drawer::MatLoader.IsAttached());
}

#include "Tools//TranslateCommand.h"
#include "Tools//RotateCommand.h"

bool   CChildFrame::CreateTransformCommander()
{
	CNuGenDimensionView* v = static_cast<CNuGenDimensionView*>(GetActiveView());
	ASSERT(v);
	FreeCommander();
	switch(v->GetHandAction()) 
	{
	case HA_OBJ_TRANSLATE:
		{
			m_commander = new TranslateCommand(this);
			if (m_commander)
				m_commander->Start();
			else
			{
				ASSERT(0);
				m_active_plugin = -1;
			}
			SetFocus();
		}
		break;
	case HA_OBJ_ROTATE:
		{
			m_commander = new RotateCommand(this);
			if (m_commander)
				m_commander->Start();
			else
			{
				ASSERT(0);
				m_active_plugin = -1;
			}
			SetFocus();
		}
		 break;
	default:
		break;
	}
	global_commander = m_commander;
	return true;
}
#include "Tools//GroupCommand.h"
bool   CChildFrame::CreateGroupCommander()
{
	FreeCommander();
	m_commander = new GroupCommand(this);
	if (m_commander)
		m_commander->Start();
	else
	{
		ASSERT(0);
		m_active_plugin = -1;
	}
	SetFocus();
	global_commander = m_commander;
	return true;
}

#include "Dialogs//FontPreviewDlg.h"
void CChildFrame::OnFonts()
{
	TCHAR  szFile[MAX_PATH]      = "\0";
	if (!ShowOpenFontDialogPreview(m_hWnd,szFile))
		return;
	
	char       drive [_MAX_DRIVE];
	char       dir   [_MAX_DIR];
	char       file  [_MAX_FNAME];
	char       ext   [_MAX_EXT];

	//_splitpath(szFile, drive, dir, file, ext);#OBSOLETE
	_splitpath_s(szFile, drive, dir, file, ext);

	if ((strlen(file)+strlen(ext))>15)
	{
		CString sss;
		sss.LoadString(IDS_ERROR_LONG_FONT_NAME);
		AfxMessageBox(sss);
		OnFonts();
		return;
	}
	else
	{
		sgCFont* fnt = sgCFont::LoadFont(szFile,NULL,0);
		if (!fnt)
		{
			CString eee;
			eee.LoadString(IDS_CANNT_LOAD_FONT);
			AfxMessageBox(eee);
			OnFonts();
			return;
		}
		if (!sgFontManager::AttachFont(fnt))
		{
			CString www;
			www.LoadString(IDS_THIS_FONT_EXIST);
			AfxMessageBox(www);
			OnFonts();
			return;
		}
		static_cast<CMainFrame*>(GetParentFrame())->UpdateSystemToolbar();
	}
}

#include "Dialogs//FontPreviewDlgNew.h"
void CChildFrame::OnFontsPreview()
{
	CFontPreviewDlg ddd;
	ddd.DoModal();
}

void CChildFrame::OnUpdateFontsPreview(CCmdUI *pCmdUI)
{
	if (sgFontManager::GetFontsCount()>0)
		pCmdUI->Enable();
	else
		pCmdUI->Enable(FALSE);
}

void CChildFrame::OnBreakHotGroup()
{
	ASSERT(Drawer::CurrentHotObject);
	sgCGroup* gr = reinterpret_cast<sgCGroup*>(Drawer::CurrentHotObject);
	
	const int ChCnt = gr->GetChildrenList()->GetCount();

	sgCObject**  allChilds = (sgCObject**)malloc(ChCnt*sizeof(sgCObject*));
	sgGetScene()->StartUndoGroup();
	if (!gr->BreakGroup(allChilds))
	{
		ASSERT(0);
	}
	const int sz = gr->GetChildrenList()->GetCount();
	ASSERT(sz==0);
	sgGetScene()->DetachObject(gr);
	for (int i=0;i<ChCnt;i++)
	{
		sgGetScene()->AttachObject(allChilds[i]);
	}
	sgGetScene()->EndUndoGroup();
	free(allChilds);
}

void CChildFrame::OnBreakHotContour()
{
	ASSERT(Drawer::CurrentHotObject);
	sgCContour* cntr = reinterpret_cast<sgCContour*>(Drawer::CurrentHotObject);

	const int ChCnt = cntr->GetChildrenList()->GetCount();

	sgCObject**  allChilds = (sgCObject**)malloc(ChCnt*sizeof(sgCObject*));
	sgGetScene()->StartUndoGroup();
	if (!cntr->BreakContour(allChilds))
	{
		ASSERT(0);
	}
	const int sz = cntr->GetChildrenList()->GetCount();
	ASSERT(sz==0);
	sgGetScene()->DetachObject(cntr);
	for (int i=0;i<ChCnt;i++)
	{
		sgGetScene()->AttachObject(allChilds[i]);
	}
	sgGetScene()->EndUndoGroup();
	free(allChilds);
}

void CChildFrame::OnMDIActivate(BOOL bActivate, CWnd* pActivateWnd, CWnd* pDeactivateWnd)
{
	__super::OnMDIActivate(bActivate, pActivateWnd, pDeactivateWnd);
}


void CChildFrame::OnRayTraceStart()
{
	CNuGenDimensionView* v = static_cast<CNuGenDimensionView*>(GetActiveView());
	//CRTDialog ibox(v->m_hWnd,v->GetCamera());
	//ibox.DoModal();
	
	CDlgRender dlgRender(v->GetCamera());
	dlgRender.DoModal();
}
