#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "..//Drawer.h"
#include "TranslateCommand.h"

//#include "..//resource.h"

#define TRANSLATION_X 101
#define TRANSLATION_Y 102
#define TRANSLATION_Z 103

TranslateCommand::TranslateCommand(IApplicationInterface* appI):
m_app(appI)
, m_panel(NULL)
,m_get_objects_panel(NULL)
,m_step(0)
, m_active_axe(-1)
{
  ASSERT(m_app);
}

TranslateCommand::~TranslateCommand()
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

bool    TranslateCommand::PreTranslateMessage(MSG* pMsg)
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

void   TranslateCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, void* params)
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
      m_message.LoadString(IDS_CHOISE_TRANS_OBJ);
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
void  TranslateCommand::Start()
{
  m_panel = new CTranslatePanelDlg;
  m_panel->Create(IDD_TRANSLATE_PANEL,m_app->GetCommandPanel()->GetDialogsContainerWindow());
  m_panel->SetCommander(this);
  m_app->GetCommandPanel()->RemoveAllDialogs();

  APP_SWITCH_RESOURCE

  m_message.LoadString(IDS_TRANS_OBJ);
  m_app->StartCommander(m_message);

  m_message.LoadString(IDS_CHOISE_TRANS_OBJ);
  m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
    m_message);

  CString lab;
  lab.LoadString(IDS_TRANS_OBJECTS);
  m_get_objects_panel = reinterpret_cast<IGetObjectsPanel*>(m_app->
    GetCommandPanel()->
    AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,lab,true));

  lab.LoadString(IDS_TRANS_PARAMS);
  m_app->GetCommandPanel()->AddDialog(m_panel,lab,true);

  m_get_objects_panel->FillList(isObjAddToList);

  m_step=0;
  m_app->GetCommandPanel()->SetActiveRadio(0);

}

unsigned int  TranslateCommand::GetTransHandleInRect(CRect& hitsRect)
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
  drawTransArrows(true);
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

void  TranslateCommand::MouseMove(unsigned int nFlags,int pX,int pY)
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
        if (!(nFlags & MK_LBUTTON))
        {
              int snSz = m_app->GetViewPort()->GetSnapSize();
              switch(GetTransHandleInRect(CRect(pX-snSz, pY-snSz, pX+snSz, pY+snSz)))
              {
              case TRANSLATION_X:
                m_active_axe = TRANSLATION_X;
                break;
              case TRANSLATION_Y:
                m_active_axe = TRANSLATION_Y;
                break;
              case TRANSLATION_Z:
                m_active_axe = TRANSLATION_Z;
                break;
              default:
                m_active_axe = -1;
                break;
              }
        }
        else
        {
          if (m_active_axe!=TRANSLATION_X && m_active_axe!=TRANSLATION_Y && m_active_axe!=TRANSLATION_Z)
            return;

          SG_POINT beg_axis;
          beg_axis.x = 0.5*(m_selMinP.x+m_selMaxP.x);
          beg_axis.y = 0.5*(m_selMinP.y+m_selMaxP.y);
          beg_axis.z = 0.5*(m_selMinP.z+m_selMaxP.z);
          SG_VECTOR axe_dir;
          axe_dir.x= axe_dir.y = axe_dir.z =0.0;
          SG_POINT cur_pnt;
          bool  mov = true;
          switch(m_active_axe)
          {
          case TRANSLATION_X:
            axe_dir.x = 1.0;
            if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,beg_axis,axe_dir,cur_pnt))
            {
              mov = false;
              break;
            }
            m_selMinP.x -= m_start_drag_point.x - cur_pnt.x;
            m_selMaxP.x -= m_start_drag_point.x - cur_pnt.x;
            m_trans_vector.x += -m_start_drag_point.x + cur_pnt.x;
            break;
          case TRANSLATION_Y:
            axe_dir.y = 1.0;
            if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,beg_axis,axe_dir,cur_pnt))
            {
              mov = false;
              break;
            }
            m_selMinP.y -= m_start_drag_point.y - cur_pnt.y;
            m_selMaxP.y -= m_start_drag_point.y - cur_pnt.y;
            m_trans_vector.y += -m_start_drag_point.y + cur_pnt.y;
            break;
          case TRANSLATION_Z:
            axe_dir.z = 1.0;
            if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,beg_axis,axe_dir,cur_pnt))
            {
              mov = false;
              break;
            }
            m_selMinP.z -= m_start_drag_point.z - cur_pnt.z;
            m_selMaxP.z -= m_start_drag_point.z - cur_pnt.z;
            m_trans_vector.z += -m_start_drag_point.z + cur_pnt.z;
            break;
          default:
            ASSERT(0);
            break;
          }
          if (mov)
          {
            m_start_drag_point=cur_pnt;
            if (!m_panel->IsDouble())
            {
              sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
              while (curObj)
              {
                curObj->InitTempMatrix()->Translate(m_trans_vector);
                curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
              }
            }
            m_panel->SetVector(m_trans_vector);
          }
          m_app->GetViewPort()->InvalidateViewPort();
        }
  }
}

void  TranslateCommand::LeftClick(unsigned int nFlags,int pX,int pY)
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
        if (m_active_axe!=TRANSLATION_X && m_active_axe!=TRANSLATION_Y && m_active_axe!=TRANSLATION_Z)
          return;

        SG_POINT beg_axis;
        beg_axis.x = 0.5*(m_selMinP.x+m_selMaxP.x);
        beg_axis.y = 0.5*(m_selMinP.y+m_selMaxP.y);
        beg_axis.z = 0.5*(m_selMinP.z+m_selMaxP.z);
        SG_VECTOR axe_dir;
        axe_dir.x= axe_dir.y = axe_dir.z =0.0;

        switch(m_active_axe)
        {
        case TRANSLATION_X:
          axe_dir.x = 1.0;
          if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,beg_axis,axe_dir,m_start_drag_point))
            ASSERT(0);
          break;
        case TRANSLATION_Y:
          axe_dir.y = 1.0;
          if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,beg_axis,axe_dir,m_start_drag_point))
            ASSERT(0);
          break;
        case TRANSLATION_Z:
          axe_dir.z = 1.0;
          if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,beg_axis,axe_dir,m_start_drag_point))
            ASSERT(0);
          break;
        default:
          ASSERT(0);
          break;
        }
        m_trans_vector.x = m_trans_vector.y = m_trans_vector.z=0.0;
        m_panel->SetVector(m_trans_vector);
  }
}

void  TranslateCommand::OnEnter()
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

    m_step++;
    m_app->GetCommandPanel()->SetActiveRadio(1);
    m_panel->SetFocus();
    m_message.LoadString(IDS_ENTER_TRANS_PARAMS);
    m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
      m_message);
    m_app->GetViewPort()->InvalidateViewPort();
  }
  else
  {
            m_panel->GetVector(m_trans_vector);
            sgGetScene()->StartUndoGroup();
            if (!m_panel->IsDouble())
            {
              sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
              while (curObj)
              {
                curObj->InitTempMatrix()->Translate(m_trans_vector);
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
                mtrx->Translate(m_trans_vector);
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
                  clone->SetName(nameClone);
                  newObjcts.push_back(clone);
                  curObj->DestroyTempMatrix();
                  clone->DestroyTempMatrix();
                  mtrx->Translate(m_trans_vector);
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
            //m_trans_vector.x = m_trans_vector.y = m_trans_vector.z=0.0;
            //m_panel->SetVector(m_trans_vector);
            m_app->GetViewPort()->InvalidateViewPort();
  }
}

unsigned int  TranslateCommand::GetItemsCount()
{
  return 0;
}

void         TranslateCommand::GetItem(unsigned int, CString&)
{
}

void     TranslateCommand::GetItemState(unsigned int, bool&, bool&)
{
}

HBITMAP   TranslateCommand::GetItemBitmap(unsigned int)
{
  return NULL;
}

void         TranslateCommand::Run(unsigned int)
{
}

static  void drawCone(GLUquadricObj *obj, float x, float y, float z,float gridSz)
{
  float color[4] = {x, y, z, 1.0f};

  glMaterialfv(GL_FRONT_AND_BACK, GL_DIFFUSE, color);

  glPushMatrix();
  glRotatef(90.0f * x - 90.0f * y, y, x, z);
  glTranslatef(0.0f, 0.0f, gridSz);
  gluCylinder(obj, 0.1*(double)gridSz, 0.0, 0.4*(double)gridSz, 10, 10);
  gluQuadricOrientation(obj, (GLenum) GLU_INSIDE);
  gluDisk(obj, 0.0, 0.1*(double)gridSz, 10, 1);
  gluQuadricOrientation(obj, (GLenum) GLU_OUTSIDE);
  glPopMatrix();
}


void TranslateCommand::drawTransArrows(bool sel)
{
  glPushMatrix();
  //glLoadIdentity();
  glTranslated(0.5*(m_selMinP.x+m_selMaxP.x),
    0.5*(m_selMinP.y+m_selMaxP.y),
    0.5*(m_selMinP.z+m_selMaxP.z));

  float    black[4] = { 0.0f, 0.0f, 0.0f, 1.0f };

  if (!sel)
  {
    glEnable(GL_CULL_FACE);
    glMaterialfv(GL_FRONT_AND_BACK, GL_EMISSION, black);
    glMaterialfv(GL_FRONT_AND_BACK, GL_SPECULAR, black);
  }

  GLUquadricObj   *obj = gluNewQuadric();

  float grid_sz = m_app->GetViewPort()->GetGridSize()*0.5f;
  glLoadName(TRANSLATION_X);
  glDisable(GL_LIGHTING);
  glBegin(GL_LINES);
    if (!sel)
      glColor3f(1.0f, 0.0f, 0.0f);
    glVertex3f(0.0f,0.0f,0.0f);
    glVertex3f(grid_sz,0.0f,0.0f);
  glEnd();
  if (!sel)
    glEnable(GL_LIGHTING);
  drawCone(obj, 1.0f, 0.0f, 0.0f,grid_sz);

  glLoadName(TRANSLATION_Y);
  glDisable(GL_LIGHTING);
  glBegin(GL_LINES);
    if (!sel)
      glColor3f(0.0f, 1.0f, 0.0f);
    glVertex3f(0.0f,0.0f,0.0f);
    glVertex3f(0.0f, grid_sz,0.0f);
  glEnd();
  if (!sel)
    glEnable(GL_LIGHTING);
  drawCone(obj, 0.0f, 1.0f, 0.0f,grid_sz);

  glLoadName(TRANSLATION_Z);
  glDisable(GL_LIGHTING);
  glBegin(GL_LINES);
    if (!sel)
      glColor3f(0.0f, 0.0f, 1.0f);
    glVertex3f(0.0f,0.0f,0.0f);
    glVertex3f(0.0f, 0.0f, grid_sz);
  glEnd();
  if (!sel)
    glEnable(GL_LIGHTING);
  drawCone(obj, 0.0f, 0.0f, 1.0f,grid_sz);

  gluDeleteQuadric(obj);

  if (!sel)
    glDisable(GL_CULL_FACE);
  glPopMatrix();

}
#include "..//Drawer.h"
void TranslateCommand::Draw()
{
  if (m_step==0)
    return ;

  glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

  glDisable(GL_TEXTURE_2D);
  glDisable(GL_DEPTH_TEST);
  glLineWidth(3);
  glEnable(GL_LINE_SMOOTH);

  drawTransArrows();

  if (m_panel->IsDouble())
  {
      glEnable(GL_DEPTH_TEST);
      sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
      while (curObj)
      {
        if (curObj->GetTempMatrix()==NULL)
          curObj->InitTempMatrix();
        int cc =m_panel->GetCopiesCount();
        for(int j=0;j<cc;j++)
        {
          curObj->GetTempMatrix()->Translate(m_trans_vector);
          Drawer::DrawObject(GL_RENDER,curObj,false);
        }
        curObj->DestroyTempMatrix();
        curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
      }
  }


  glPopAttrib();
}

void   TranslateCommand::LeftUp(int scrX,int scrY)
{
  if (m_step==0)
  {
  }
  else
  {
        if (m_active_axe!=TRANSLATION_X && m_active_axe!=TRANSLATION_Y && m_active_axe!=TRANSLATION_Z)
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
          sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
          newObjcts.reserve(sgGetScene()->GetSelectedObjectsList()->GetCount()*
            m_panel->GetCopiesCount());
          while (curObj)
          {
            mtrx->Identity();
            mtrx->Translate(m_trans_vector);
            int cc =m_panel->GetCopiesCount();
            for(int ii=0;ii<cc;ii++)
            {
              sgCObject* clone = curObj->Clone();
              clone->Select(false);
              clone->InitTempMatrix()->SetMatrix(mtrx);
              clone->ApplyTempMatrix();
              sgGetScene()->AttachObject(clone);
              m_app->CopyAttributes(*clone,*curObj);
              CString nameClone(curObj->GetName());
              numbStr.Format("%i",ii);
              nameClone += "__"+copyStr+"__"+numbStr;
              clone->SetName(nameClone);
              newObjcts.push_back(clone);
              curObj->DestroyTempMatrix();
              clone->DestroyTempMatrix();
              mtrx->Translate(m_trans_vector);
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
        m_trans_vector.x = m_trans_vector.y = m_trans_vector.z=0.0;
        m_panel->SetVector(m_trans_vector);
        m_app->GetViewPort()->InvalidateViewPort();
  }
}