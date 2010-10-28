#include "stdafx.h"

#include "..//NuGenDimension.h"
#include "..//Drawer.h"
#include "RotateCommand.h"

//#include "..//resource.h"

#define ROTATION_X 101
#define ROTATION_Y 102
#define ROTATION_Z 103

RotateCommand::RotateCommand(IApplicationInterface* appI):
m_app(appI)
, m_panel(NULL)
,m_get_objects_panel(NULL)
,m_step(0)
, m_active_axe(-1)
{
  ASSERT(m_app);
}

RotateCommand::~RotateCommand()
{
  if (sgGetScene()->GetSelectedObjectsList()->GetCount()>0)
  {
    sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
    while (curObj)
    {
      curObj->Select(false);
      curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
    }
  }
  m_app->GetCommandPanel()->RemoveAllDialogs();
  if (m_panel)
  {
    m_panel->DestroyWindow();
    delete m_panel;
    m_panel = NULL;
  }
  m_app->GetViewPort()->InvalidateViewPort();
}


bool    RotateCommand::PreTranslateMessage(MSG* pMsg)
{
	try { //#try
	  /*if (pMsg->message==WM_KEYUP||
	  pMsg->message==WM_CHAR)
	  return false;*/

	  if (pMsg->message==WM_KEYUP||pMsg->message==WM_KEYDOWN ||
		pMsg->message==WM_CHAR)
	  {
		if (pMsg->wParam==VK_RETURN)
		{
		  OnEnter();
		  return true;
		}
		if (pMsg->wParam==VK_ESCAPE)
		{
		  m_app->StopCommander();
		  return true;
		}
		if (m_step==1 && m_panel)
		  m_panel->SendMessage(pMsg->message,
		  pMsg->wParam,
		  pMsg->lParam);
		if (pMsg->message==WM_KEYDOWN)
		  return false;
		else
		  return true;
	  }
	  else
	  {
		if (pMsg->hwnd == m_app->GetViewPort()->GetWindow()->m_hWnd)
		{
		  switch(pMsg->message)
		  {
		  case WM_MOUSEMOVE:
			MouseMove(pMsg->wParam,GET_X_LPARAM(pMsg->lParam),GET_Y_LPARAM(pMsg->lParam));
			return true;
		  case WM_LBUTTONDOWN:
			LeftClick(pMsg->wParam,GET_X_LPARAM(pMsg->lParam),GET_Y_LPARAM(pMsg->lParam));
			return true;
		  case WM_LBUTTONUP:
			LeftUp(GET_X_LPARAM(pMsg->lParam),GET_Y_LPARAM(pMsg->lParam));
			return true;
		  default:
			return false;
		  }
		}
	  }
	}
	catch(...){
	}
  return false;
}


void   RotateCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, void* params)
{
  if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
  {
    ASSERT(params!=NULL);
    int   newActiveDlg = *(reinterpret_cast<int*>(params));

    ASSERT(newActiveDlg<=1);
    m_step = (unsigned int)newActiveDlg;
    if (newActiveDlg==0)
    {
      m_app->GetCommandPanel()->EnableRadio(1,false);
      m_message.LoadString(IDS_CHOISE_ROT_OBJ);
      m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
    }
    m_app->GetViewPort()->InvalidateViewPort();
    return;
  }
  if (mes==ICommander::CM_SELECT_OBJECT)
  {
    ASSERT(params!=NULL);
    sgCObject* so = (sgCObject*)params;
    if (so->IsSelect())
    {
      so->Select(false);
    }
    else
    {
      so->Select(true);
    }
    m_app->GetViewPort()->InvalidateViewPort();
  }
}

static   bool isObjAddToList(sgCObject*)
{
  return true;
}

#include <float.h>
void  RotateCommand::Start()
{
  m_panel = new CRotatePanelDlg;
  m_panel->Create(IDD_ROTATE_PANEL,m_app->GetCommandPanel()->GetDialogsContainerWindow());
  m_panel->SetCommander(this);
  m_app->GetCommandPanel()->RemoveAllDialogs();

  APP_SWITCH_RESOURCE

  m_message.LoadString(IDS_ROT_OBJ);
  m_app->StartCommander(m_message);

  m_message.LoadString(IDS_CHOISE_ROT_OBJ);
  m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
    m_message);

  CString lab;
  lab.LoadString(IDS_ROT_OBJECTS);
  m_get_objects_panel = reinterpret_cast<IGetObjectsPanel*>(m_app->
    GetCommandPanel()->
    AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,lab.GetBuffer(),true));
  lab.ReleaseBuffer();

  lab.LoadString(IDS_ROT_PARAMS);
  m_app->GetCommandPanel()->AddDialog(m_panel,lab.GetBuffer(),true);
  lab.ReleaseBuffer();

  m_get_objects_panel->FillList(isObjAddToList);

  m_step=0;
  m_app->GetCommandPanel()->SetActiveRadio(0);
}

unsigned int  RotateCommand::GetRotateHandleInRect(CRect& hitsRect)
{
  int     name_background = -1;
  GLint   vp[4], nhits;
  double    CurProj[16];
#define  HITS_BUF_SIZE      10
  GLuint      hits_buf[HITS_BUF_SIZE];

  int  yPos = ((hitsRect.top+hitsRect.bottom)/2);
  int  xPos = ((hitsRect.left+hitsRect.right)/2);

  memset(hits_buf,0,HITS_BUF_SIZE*sizeof(GLuint));
  ::glSelectBuffer(HITS_BUF_SIZE, hits_buf);

  ::glGetIntegerv(GL_VIEWPORT, vp);
  ::glMatrixMode (GL_PROJECTION);
  ::glGetDoublev(GL_PROJECTION_MATRIX,CurProj);
  ::glPushMatrix ();
  ::glRenderMode (GL_SELECT);
  ::glInitNames();
  ::glPushName(name_background);
  yPos = vp[3]-yPos;
  ::glLoadIdentity ();
  ::gluPickMatrix(xPos, yPos, hitsRect.Width(), hitsRect.Height(),vp);

  ::glMultMatrixd(CurProj);

  ::glMatrixMode(GL_MODELVIEW);

  ::glScissor(xPos-hitsRect.Width()/2, yPos-hitsRect.Height()/2, hitsRect.Width(), hitsRect.Height());
  ::glEnable(GL_SCISSOR_TEST);
  drawRotateCircles(true);
  ::glDisable(GL_SCISSOR_TEST);

  nhits = ::glRenderMode(GL_RENDER);

  ::glMatrixMode (GL_PROJECTION);
  ::glPopMatrix ();
  ::glMatrixMode(GL_MODELVIEW);

  {
    const int pick_maxz = 0xffffffff;
    GLint     hit = name_background;
    GLuint    minz;
    GLint     i,j;
    GLint   nnames;

    minz = pick_maxz;
    for (i = j = 0; j < nhits; j++)
    {
      nnames = hits_buf[i];
      i++;
      if (hits_buf[i] < minz)
      {
        minz = hits_buf[i];
        hit = (GLint)hits_buf[i + 1 + nnames];
      }
      i++;
      i += nnames + 1;
    }

    return hit;
  }
}

void  RotateCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
  if (m_step==0)
  {
    if (!(nFlags & MK_LBUTTON))
    {
      int snapSz = m_app->GetViewPort()->GetSnapSize();

      Drawer::CurrentHotObject = m_app->GetViewPort()->GetTopObject(
        m_app->GetViewPort()->GetHitsInRect(CRect(pX-snapSz, pY-snapSz,
        pX+snapSz, pY+snapSz)));
      m_app->GetViewPort()->InvalidateViewPort();
    }
  }
  else
  {
    double scM = 0.0;
        if (!(nFlags & MK_LBUTTON))
        {
              int snSz = m_app->GetViewPort()->GetSnapSize();
              switch(GetRotateHandleInRect(CRect(pX-snSz, pY-snSz,  pX+snSz, pY+snSz)))
              {
              case ROTATION_X:
                m_active_axe = ROTATION_X;
                break;
              case ROTATION_Y:
                m_active_axe = ROTATION_Y;
                break;
              case ROTATION_Z:
                m_active_axe = ROTATION_Z;
                break;
              default:
                m_active_axe = -1;
                break;
              }
        }
        else
        {
          if (m_active_axe!=ROTATION_X && m_active_axe!=ROTATION_Y && m_active_axe!=ROTATION_Z)
            return;

          SG_VECTOR rot_plane_nrml;
          rot_plane_nrml.x= rot_plane_nrml.y = rot_plane_nrml.z =0.0;
          double   plD;
          SG_POINT cur_pnt;
          SG_VECTOR cur_vect;
          bool  mov = true;
          switch(m_active_axe)
          {
          case ROTATION_X:
            rot_plane_nrml.x = 1.0;
            sgSpaceMath::PlaneFromNormalAndPoint(m_rot_plane_pnt,rot_plane_nrml,plD);
            if (!m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,rot_plane_nrml,plD,cur_pnt))
            {
              mov = false;
              break;
            }
            cur_vect.x = cur_pnt.x - m_rot_plane_pnt.x;
            cur_vect.y = cur_pnt.y - m_rot_plane_pnt.y;
            cur_vect.z = cur_pnt.z - m_rot_plane_pnt.z;
            sgSpaceMath::NormalVector(cur_vect);
            scM = sgSpaceMath::VectorsScalarMult(m_start_drag_vector,cur_vect);
            if (fabs(scM)>1.000)
            {
              mov = false;
              break;
            }
            if (sgSpaceMath::VectorsScalarMult(rot_plane_nrml,
                sgSpaceMath::VectorsVectorMult(m_start_drag_vector,cur_vect))>0)
              m_rot_angles.x += acos(scM);
            else
              m_rot_angles.x -= acos(scM);
            break;
          case ROTATION_Y:
            rot_plane_nrml.y = 1.0;
            sgSpaceMath::PlaneFromNormalAndPoint(m_rot_plane_pnt,rot_plane_nrml,plD);
            if (!m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,rot_plane_nrml,plD,cur_pnt))
            {
              mov = false;
              break;
            }
            cur_vect.x = cur_pnt.x - m_rot_plane_pnt.x;
            cur_vect.y = cur_pnt.y - m_rot_plane_pnt.y;
            cur_vect.z = cur_pnt.z - m_rot_plane_pnt.z;
            sgSpaceMath::NormalVector(cur_vect);
            scM = sgSpaceMath::VectorsScalarMult(m_start_drag_vector,cur_vect);
            if (fabs(scM)>1.000)
            {
              mov = false;
              break;
            }
            if (sgSpaceMath::VectorsScalarMult(rot_plane_nrml,
              sgSpaceMath::VectorsVectorMult(m_start_drag_vector,cur_vect))>0)
            m_rot_angles.y += acos(scM);
            else
              m_rot_angles.y -= acos(scM);
            break;
          case ROTATION_Z:
            rot_plane_nrml.z = 1.0;
            sgSpaceMath::PlaneFromNormalAndPoint(m_rot_plane_pnt,rot_plane_nrml,plD);
            if (!m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,rot_plane_nrml,plD,cur_pnt))
            {
              mov = false;
              break;
            }
            cur_vect.x = cur_pnt.x - m_rot_plane_pnt.x;
            cur_vect.y = cur_pnt.y - m_rot_plane_pnt.y;
            cur_vect.z = cur_pnt.z - m_rot_plane_pnt.z;
            sgSpaceMath::NormalVector(cur_vect);
            scM = sgSpaceMath::VectorsScalarMult(m_start_drag_vector,cur_vect);
            if (fabs(scM)>1.000)
            {
              mov = false;
              break;
            }
            if (sgSpaceMath::VectorsScalarMult(rot_plane_nrml,
              sgSpaceMath::VectorsVectorMult(m_start_drag_vector,cur_vect))>0)
            m_rot_angles.z += acos(scM);
            else
              m_rot_angles.z -= acos(scM);
            break;
          default:
            ASSERT(0);
            break;
          }
          if (mov)
          {
            m_start_drag_vector=cur_vect;
            if (!m_panel->IsDouble())
            {
              sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
              while (curObj)
              {
                rot_plane_nrml.x= rot_plane_nrml.y = rot_plane_nrml.z =0.0;
                rot_plane_nrml.x=1.0;
                curObj->InitTempMatrix()->
                  Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.x);
                rot_plane_nrml.x=0.0;
                rot_plane_nrml.y=1.0;
                curObj->GetTempMatrix()->
                  Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.y);
                rot_plane_nrml.y=0.0;
                rot_plane_nrml.z=1.0;
                curObj->GetTempMatrix()->
                  Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.z);
                curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
              }
            }
            m_panel->SetAngles(m_rot_angles);
          }
          m_app->GetViewPort()->InvalidateViewPort();
        }
  }
}

void  RotateCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
  if (m_step==0)
  {
    if (Drawer::CurrentHotObject)
    {
      if (Drawer::CurrentHotObject->IsSelect())
      {
        Drawer::CurrentHotObject->Select(false);
      }
      else
      {
        Drawer::CurrentHotObject->Select(true);
      }
      m_get_objects_panel->SelectObject(Drawer::CurrentHotObject,
        Drawer::CurrentHotObject->IsSelect());
      m_app->GetViewPort()->InvalidateViewPort();
    }
  }
  else
  {
      if (m_active_axe!=ROTATION_X && m_active_axe!=ROTATION_Y && m_active_axe!=ROTATION_Z)
        return;

      SG_VECTOR rot_plane_nrml;
      rot_plane_nrml.x= rot_plane_nrml.y = rot_plane_nrml.z =0.0;
      double   plD;
      SG_POINT cur_pnt;

      switch(m_active_axe)
      {
      case ROTATION_X:
        rot_plane_nrml.x = 1.0;
        sgSpaceMath::PlaneFromNormalAndPoint(m_rot_plane_pnt,rot_plane_nrml,plD);
        if (!m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,rot_plane_nrml,plD,cur_pnt))
          ASSERT(0);
        break;
      case ROTATION_Y:
        rot_plane_nrml.y = 1.0;
        sgSpaceMath::PlaneFromNormalAndPoint(m_rot_plane_pnt,rot_plane_nrml,plD);
        if (!m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,rot_plane_nrml,plD,cur_pnt))
          ASSERT(0);
        break;
      case ROTATION_Z:
        rot_plane_nrml.z = 1.0;
        sgSpaceMath::PlaneFromNormalAndPoint(m_rot_plane_pnt,rot_plane_nrml,plD);
        if (!m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,rot_plane_nrml,plD,cur_pnt))
          ASSERT(0);
        break;
      default:
        ASSERT(0);
        break;
      }
      m_start_drag_vector.x = cur_pnt.x - m_rot_plane_pnt.x;
      m_start_drag_vector.y = cur_pnt.y - m_rot_plane_pnt.y;
      m_start_drag_vector.z = cur_pnt.z - m_rot_plane_pnt.z;
      sgSpaceMath::NormalVector(m_start_drag_vector);
      m_rot_angles.x = m_rot_angles.y = m_rot_angles.z=0.0;
      m_panel->SetAngles(m_rot_angles);
  }
}

void  RotateCommand::OnEnter()
{
  if (m_step==0)
  {
    if (sgGetScene()->GetSelectedObjectsList()->GetCount()==0)
    {
      CString lab;
      lab.LoadString(IDS_NO_SELECTED_OBJECTS);
      m_app->PutMessage(IApplicationInterface::MT_ERROR,lab);
      return;
    }

    m_selMinP.x = m_selMinP.y = m_selMinP.z = FLT_MAX;
    m_selMaxP.x = m_selMaxP.y = m_selMaxP.z = -FLT_MAX+1;

    sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
    while (curObj)
    {
        SG_POINT pMin,pMax;
        curObj->GetGabarits(pMin,pMax);
        if (pMin.x<=m_selMinP.x)  m_selMinP.x = pMin.x;
        if (pMin.y<=m_selMinP.y)  m_selMinP.y = pMin.y;
        if (pMin.z<=m_selMinP.z)  m_selMinP.z = pMin.z;
        if (pMax.x>=m_selMaxP.x)  m_selMaxP.x = pMax.x;
        if (pMax.y>=m_selMaxP.y)  m_selMaxP.y = pMax.y;
        if (pMax.z>=m_selMaxP.z)  m_selMaxP.z = pMax.z;
      curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
    }

    if (m_panel->GetRotateType()==1)
    {
      m_rot_plane_pnt.x = 0.5*(m_selMinP.x+m_selMaxP.x);
      m_rot_plane_pnt.y = 0.5*(m_selMinP.y+m_selMaxP.y);
      m_rot_plane_pnt.z = 0.5*(m_selMinP.z+m_selMaxP.z);
    }
    else
    {
      m_rot_plane_pnt.x = m_rot_plane_pnt.y = m_rot_plane_pnt.z = 0.0;
    }

    m_step++;
    m_app->GetCommandPanel()->SetActiveRadio(1);
    m_panel->SetFocus();
    m_message.LoadString(IDS_ENTER_ROT_PARAMS);
    m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
      m_message);
    m_app->GetViewPort()->InvalidateViewPort();
  }
  else
  {
      m_panel->GetAngles(m_rot_angles);
      SG_VECTOR rot_plane_nrml;
      sgGetScene()->StartUndoGroup();
      if (!m_panel->IsDouble())
      {
        sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
        while (curObj)
        {
          rot_plane_nrml.x= rot_plane_nrml.y = rot_plane_nrml.z =0.0;
          rot_plane_nrml.x=1.0;
          curObj->InitTempMatrix()->
            Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.x);
          rot_plane_nrml.x=0.0;
          rot_plane_nrml.y=1.0;
          curObj->GetTempMatrix()->
            Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.y);
          rot_plane_nrml.y=0.0;
          rot_plane_nrml.z=1.0;
          curObj->GetTempMatrix()->
            Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.z);
          curObj->ApplyTempMatrix();
          curObj->DestroyTempMatrix();
          curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
        }
      }
      else
      {
        sgCMatrix*  mtrx = new sgCMatrix();
        std::vector<sgCObject*> newObjcts;

        CString copyStr,numbStr;
        copyStr.LoadString(IDS_OBJECT_COPY);
        sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
        newObjcts.reserve(sgGetScene()->GetSelectedObjectsList()->GetCount()*
          m_panel->GetCopiesCount());
        while (curObj)
        {
          mtrx->Identity();
          rot_plane_nrml.x= rot_plane_nrml.y = rot_plane_nrml.z =0.0;
          rot_plane_nrml.x=1.0;
          mtrx->Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.x);
          rot_plane_nrml.x=0.0;
          rot_plane_nrml.y=1.0;
          mtrx->Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.y);
          rot_plane_nrml.y=0.0;
          rot_plane_nrml.z=1.0;
          mtrx->Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.z);
          int cc =m_panel->GetCopiesCount();
          for(int i=0;i<cc;i++)
          {
            sgCObject* clone = curObj->Clone();
            clone->Select(false);
            clone->InitTempMatrix()->SetMatrix(mtrx);
            clone->ApplyTempMatrix();
            sgGetScene()->AttachObject(clone);
            m_app->CopyAttributes(*clone,*curObj);
            CString nameClone(curObj->GetName());
            numbStr.Format("%i",i);
            nameClone += "__"+copyStr+"__"+numbStr;
            clone->SetName(nameClone.GetBuffer());
            nameClone.ReleaseBuffer();
            newObjcts.push_back(clone);
            curObj->DestroyTempMatrix();
            clone->DestroyTempMatrix();
            rot_plane_nrml.x= rot_plane_nrml.y = rot_plane_nrml.z =0.0;
            rot_plane_nrml.x=1.0;
            mtrx->Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.x);
            rot_plane_nrml.x=0.0;
            rot_plane_nrml.y=1.0;
            mtrx->Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.y);
            rot_plane_nrml.y=0.0;
            rot_plane_nrml.z=1.0;
            mtrx->Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.z);
          }
          curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
        }
        delete mtrx;
        size_t sz = newObjcts.size();
        for (size_t jj=0;jj<sz;jj++)
        {
          if (m_panel->IsSelectNew())
          {
            newObjcts[jj]->Select(true);
            m_get_objects_panel->AddObject(newObjcts[jj],true);
          }
          else
          {
            m_get_objects_panel->AddObject(newObjcts[jj],false);
          }
        }
        newObjcts.clear();

      }
      sgGetScene()->EndUndoGroup();
      m_app->GetViewPort()->InvalidateViewPort();
  }
}

unsigned int  RotateCommand::GetItemsCount()
{
  return 0;
}

void         RotateCommand::GetItem(unsigned int, CString&)
{
}

void     RotateCommand::GetItemState(unsigned int, bool&, bool&)
{
}

HBITMAP   RotateCommand::GetItemBitmap(unsigned int)
{
  return NULL;
}

void         RotateCommand::Run(unsigned int)
{
}

static  void draw_line11(SG_POINT* p1,SG_POINT* p2)
{
  glBegin(GL_LINES);
    glVertex3d(p1->x,p1->y,p1->z);
    glVertex3d(p2->x,p2->y,p2->z);
  glEnd();
}

void RotateCommand::drawRotateCircles(bool sel)
{
  glPushMatrix();

  if (m_panel->GetRotateType()==1)
    glTranslated(0.5*(m_selMinP.x+m_selMaxP.x),
      0.5*(m_selMinP.y+m_selMaxP.y),
      0.5*(m_selMinP.z+m_selMaxP.z));

  glDisable(GL_LIGHTING);

  SG_CIRCLE  circ;
  circ.center.x = circ.center.y = circ.center.z = 0.0;
  circ.normal.x = circ.normal.y = circ.normal.z = 0.0;
  circ.radius = m_app->GetViewPort()->GetGridSize()*0.5f;
  glLoadName(ROTATION_X);
    if (!sel)
      glColor3f(1.0f, 0.0f, 0.0f);
    circ.normal.x = 1.0;
    circ.Draw(draw_line11);

  circ.normal.x = circ.normal.y = circ.normal.z = 0.0;
  glLoadName(ROTATION_Y);
    if (!sel)
      glColor3f(0.0f, 1.0f, 0.0f);
    circ.normal.y = 1.0;
    circ.Draw(draw_line11);

  circ.normal.x = circ.normal.y = circ.normal.z = 0.0;
  glLoadName(ROTATION_Z);
    if (!sel)
      glColor3f(0.0f, 0.0f, 1.0f);
    circ.normal.z = 1.0;
    circ.Draw(draw_line11);

  glEnable(GL_LIGHTING);

  glPopMatrix();

}

void  RotateCommand::RedrawFromPanel()
{
  m_app->GetViewPort()->InvalidateViewPort();
  if (m_panel->GetRotateType()==1)
  {
    m_rot_plane_pnt.x = 0.5*(m_selMinP.x+m_selMaxP.x);
    m_rot_plane_pnt.y = 0.5*(m_selMinP.y+m_selMaxP.y);
    m_rot_plane_pnt.z = 0.5*(m_selMinP.z+m_selMaxP.z);
  }
  else
  {
    m_rot_plane_pnt.x = m_rot_plane_pnt.y = m_rot_plane_pnt.z = 0.0;
  }

}

#include "..//Drawer.h"
void RotateCommand::Draw()
{
  if (m_step==0)
    return ;

  glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

  glDisable(GL_TEXTURE_2D);
  glDisable(GL_DEPTH_TEST);
  glLineWidth(3);
  glEnable(GL_LINE_SMOOTH);

  drawRotateCircles();

  if (m_panel->IsDouble())
  {

    SG_VECTOR rot_plane_nrml;
    rot_plane_nrml.x= rot_plane_nrml.y = rot_plane_nrml.z =0.0;

      glEnable(GL_DEPTH_TEST);
      sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
      while (curObj)
      {
        if (curObj->GetTempMatrix()==NULL)
          curObj->InitTempMatrix();
        int cc =m_panel->GetCopiesCount();
        for(int i=0;i<cc;i++)
        {
          rot_plane_nrml.x= rot_plane_nrml.y = rot_plane_nrml.z =0.0;
          rot_plane_nrml.x=1.0;
          curObj->GetTempMatrix()->
            Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.x);
          rot_plane_nrml.x=0.0;
          rot_plane_nrml.y=1.0;
          curObj->GetTempMatrix()->
            Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.y);
          rot_plane_nrml.y=0.0;
          rot_plane_nrml.z=1.0;
          curObj->GetTempMatrix()->
            Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.z);
          Drawer::DrawObject(GL_RENDER,curObj,false);
        }
        curObj->DestroyTempMatrix();
        curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
      }
  }
  glPopAttrib();
}

void   RotateCommand::LeftUp(int scrX,int scrY)
{
  if (m_step==0)
  {
  }
  else
  {
      if (m_active_axe!=ROTATION_X && m_active_axe!=ROTATION_Y && m_active_axe!=ROTATION_Z)
        return;
      sgGetScene()->StartUndoGroup();
      if (!m_panel->IsDouble())
      {
        sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
        while (curObj)
        {
          if (curObj->GetTempMatrix()!=NULL)
          {
            curObj->ApplyTempMatrix();
            curObj->DestroyTempMatrix();
          }
          curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
        }
      }
      else
      {
        sgCMatrix*  mtrx = new sgCMatrix();
        std::vector<sgCObject*> newObjcts;

        CString copyStr,numbStr;
        copyStr.LoadString(IDS_OBJECT_COPY);
        SG_VECTOR rot_plane_nrml;
        sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
        newObjcts.reserve(sgGetScene()->GetSelectedObjectsList()->GetCount()*
          m_panel->GetCopiesCount());
        while (curObj)
        {
          mtrx->Identity();
          rot_plane_nrml.x= rot_plane_nrml.y = rot_plane_nrml.z =0.0;
          rot_plane_nrml.x=1.0;
          mtrx->Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.x);
          rot_plane_nrml.x=0.0;
          rot_plane_nrml.y=1.0;
          mtrx->Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.y);
          rot_plane_nrml.y=0.0;
          rot_plane_nrml.z=1.0;
          mtrx->Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.z);

          int cc =m_panel->GetCopiesCount();
          for(int i=0;i<cc;i++)
          {
            sgCObject* clone = curObj->Clone();
            clone->Select(false);
            clone->InitTempMatrix()->SetMatrix(mtrx);
            clone->ApplyTempMatrix();
            sgGetScene()->AttachObject(clone);
            m_app->CopyAttributes(*clone, *curObj);
            CString nameClone( curObj->GetName());
            numbStr.Format("%i",i);
            nameClone += "__"+copyStr+"__"+numbStr;
            clone->SetName(nameClone.GetBuffer());
            nameClone.ReleaseBuffer();
            newObjcts.push_back(clone);
            curObj->DestroyTempMatrix();
            clone->DestroyTempMatrix();

            rot_plane_nrml.x= rot_plane_nrml.y = rot_plane_nrml.z =0.0;
            rot_plane_nrml.x=1.0;
            mtrx->Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.x);
            rot_plane_nrml.x=0.0;
            rot_plane_nrml.y=1.0;
            mtrx->Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.y);
            rot_plane_nrml.y=0.0;
            rot_plane_nrml.z=1.0;
            mtrx->Rotate(m_rot_plane_pnt,rot_plane_nrml,m_rot_angles.z);
          }
          curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
        }
        delete mtrx;
        size_t sz = newObjcts.size();
        for (size_t jj=0;jj<sz;jj++)
        {
          if (m_panel->IsSelectNew())
          {
            newObjcts[jj]->Select(true);
            m_get_objects_panel->AddObject(newObjcts[jj],true);
          }
          else
          {
            m_get_objects_panel->AddObject(newObjcts[jj],false);
          }
        }
        newObjcts.clear();

      }
      sgGetScene()->EndUndoGroup();
      m_rot_angles.x = m_rot_angles.y = m_rot_angles.z=0.0;
      m_panel->SetAngles(m_rot_angles);
      m_app->GetViewPort()->InvalidateViewPort();
  }
}