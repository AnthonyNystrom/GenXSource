#include "stdafx.h"
#include "msgCore.h"
#include "Interfaces/mIObjectsList.h"
#include "Objects/msgGroup.h"
#include "Objects/2D/msgContour.h"
#include "Structs/msgUserDynamicDataStruct.h"
#include "Helpers/ObjectCreateHelper.h"

msgContour^ msg2DObject::GetEquidistantContour(double h1, double h2, bool toRound)
{
  return msgContour::InternalCreate(sg2DObject->GetEquidistantContour(h1, h2, toRound));
}

bool msgGroup::BreakGroup(array<msgObject^>^% objcts)
{
  int count = GetChildrenList()->GetCount();

  sgCObject** uobjcts = new sgCObject*[count];
  bool result = sgGroup->BreakGroup(uobjcts);
  if (result)
  {
    objcts = gcnew array<msgObject^>(count);
    for (int i = 0; i < count; i++)
    {
      objcts[i] = ObjectCreateHelper::CreateObject(uobjcts[i]);
    }
  }
  delete[] uobjcts;
  return result;
}

msgObject^ mIObjectsList::GetHead()
{
  sgCObject* head = _iObjectsList->GetHead();
  if (head)
  {
    return ObjectCreateHelper::CreateObject(head);
  }
  else
  {
    return nullptr;
  }
}

msgObject^ mIObjectsList::GetNext(msgObject^ current)
{
  sgCObject* next = _iObjectsList->GetNext(current->_sgCObject);
  if (next)
  {
    return ObjectCreateHelper::CreateObject(next);
  }
  else
  {
    return nullptr;
  }
}

msgObject^ mIObjectsList::GetTail()
{
  sgCObject* tail = _iObjectsList->GetTail();
  if (tail)
  {
    return ObjectCreateHelper::CreateObject(tail);
  }
  else
  {
    return nullptr;
  }
}

msgObject^ mIObjectsList::GetPrev(msgObject^ current)
{
  sgCObject* prev = _iObjectsList->GetPrev(current->_sgCObject);
  if (prev)
  {
    return ObjectCreateHelper::CreateObject(prev);
  }
  else
  {
    return nullptr;
  }
}

msgUserDynamicDataStruct::msgUserDynamicDataStruct()
{
  _msgUserDynamicDataStructHelper = new msgUserDynamicDataStructHelper(this);
}

msgUserDynamicDataStruct::!msgUserDynamicDataStruct()
{
  delete _msgUserDynamicDataStructHelper;
}

msgObject^ ObjectCreateHelper::CreateObject(sgCObject* sgCObject)
{
  switch (sgCObject->GetType())
  {
  case SG_OT_POINT:
    {
      return gcnew msgPoint((sgCPoint*)sgCObject);
    }
  case SG_OT_LINE:
    {
      return gcnew msgLine((sgCLine*)sgCObject);
    }
  case SG_OT_CIRCLE:
    {
      return gcnew msgCircle((sgCCircle*)sgCObject);
    }
  case SG_OT_ARC:
    {
      return gcnew msgArc((sgCArc*)sgCObject);
    }
  case SG_OT_SPLINE:
    {
      return gcnew msgSpline((sgCSpline*)sgCObject);
    }
  case SG_OT_CONTOUR:
    {
      return gcnew msgContour((sgCContour*)sgCObject);
    }
  case SG_OT_3D:
    {
      return gcnew msg3DObject((sgC3DObject*)sgCObject);
    }
  case SG_OT_GROUP:
    {
      return gcnew msgGroup((sgCGroup*)sgCObject);
    }
  case SG_OT_TEXT:
  case SG_OT_DIM:
  default:
    {
      return gcnew msgObject(sgCObject);
    }
  }
}