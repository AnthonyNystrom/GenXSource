#include "stdafx.h"
#include "PIProperty.h"

#include "DotNetInterface.h"
String ^ Clipping::Equation::get()
{ 
	CString strEqu;
	if ( m_pSelectionDisplay != NULL )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			CString strEqu;
			if(index ==0)
			{
			strEqu.Format ("%.3f,%.3f,%.3f,%.3f", 
				   pPropertyCommon->m_clipPlaneEquation1.a, 
				   pPropertyCommon->m_clipPlaneEquation1.b,
				   pPropertyCommon->m_clipPlaneEquation1.c,
				   pPropertyCommon->m_clipPlaneEquation1.d );
			}else
			{
				strEqu.Format ("%.3f,%.3f,%.3f,%.3f", 
				   pPropertyCommon->m_clipPlaneEquation2.a, 
				   pPropertyCommon->m_clipPlaneEquation2.b,
				   pPropertyCommon->m_clipPlaneEquation2.c,
				   pPropertyCommon->m_clipPlaneEquation2.d );
			}
			return gcnew String(strEqu);
		}
	}
    return "";
	 
}
void  Clipping::Equation::set(String ^ equ)
{ 
	CString strEqu;
	strEqu =MStrToCString(equ);

	if ( m_pSelectionDisplay != NULL )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if(index ==0)
		{
			m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_PLANE1_EQUATION,strEqu);
		}
		else
		{
			m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_PLANE2_EQUATION,strEqu);
		}
	}
	else
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
		{
			GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_PLANE0_EQUATION,strEqu,NULL);
		}
	}
	ForceRenderScene(); 
}
 
 