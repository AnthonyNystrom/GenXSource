#include "stdafx.h"
#include "ProteinUIInterface.h"
#include "ProteinVista.h"
#include "Interface.h"
#include "Utils.h"
#include "ProteinVistaRenderer.h"
#include "PIProperty.h"
#include "PDBTreePane.h"
#include "ResiduePane.h"
#include "SelectionListPane.h"
using namespace System;
using namespace NuGenCbaseInterface;
using namespace System::Drawing;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace Microsoft::DirectX;
using namespace Microsoft::DirectX::Direct3D;
using namespace System::Runtime::InteropServices;
using namespace NGVChem ;

ProteinUIInterface::ProteinUIInterface()
{
	OnPropertyChange += gcnew PDBPropertChange(this,&ProteinUIInterface::propteryChange);
    pdbPropertyChangeMethod = gcnew NotifyPropertyChanged(this,&ProteinUIInterface::OnReceivedPropertyChange);
	GetMainActiveView()->mPropertyChanged=
		(void (*)())Marshal::GetFunctionPointerForDelegate(pdbPropertyChangeMethod).ToPointer();
}

void ProteinUIInterface::OnReceivedPropertyChange()
{
	CPIProperty^ propertyObj = GetMainActiveView()->GetProperty();
	OnPropertyChange(propertyObj);
}
 
System::Windows::Forms::ComboBox^ ProteinUIInterface::GetPDBCombox()
{
	System::Windows::Forms::ComboBox^ combox = (System::Windows::Forms::ComboBox^)System::Windows::Forms::ComboBox::FromHandle(::GetMainActiveView()->mComboxList->Handle);
	combox->DropDownStyle = System::Windows::Forms::ComboBoxStyle::DropDownList;
	return combox;
}
void ProteinUIInterface::SetOutputDelegate(OutputMethod^ outputMethod  )
{
	 if(outputMethod==nullptr)
		 return;
	 GetMainActiveView()->mOutputMethod=
	(void (*)(CString))Marshal::GetFunctionPointerForDelegate(outputMethod).ToPointer();
}
 
void ProteinUIInterface::SetHeaderInfoDelegate(GetHeaderInfoMethod^ getHeaderMethod )
{
	if(getHeaderMethod==nullptr)
		return;
	GetMainActiveView()->mGetHeaderInfoMethod=
		(void (*)(CString))Marshal::GetFunctionPointerForDelegate(getHeaderMethod).ToPointer();
}
void ProteinUIInterface::SetInitProgressDelegate(ProgressMethod^ initPrgMethod )
{
	if(initPrgMethod==nullptr)
		return;
	GetMainActiveView()->mInitProgressMethod=
		(void (*)(int))Marshal::GetFunctionPointerForDelegate(initPrgMethod).ToPointer();
}
void ProteinUIInterface::SetEndProgressDelegate(ProgressMethod^ endPrgMethod )
{
	if(endPrgMethod==nullptr)
		return;
	GetMainActiveView()->mEndProgressMethod=
		(void (*)(int))Marshal::GetFunctionPointerForDelegate(endPrgMethod).ToPointer();

}
void ProteinUIInterface::SetResetProgressDelegate(ProgressMethod^ resetPrgMethod )
{
	if(resetPrgMethod==nullptr)
		return;
	GetMainActiveView()->mResetProgressMethod=
		(void (*)(int))Marshal::GetFunctionPointerForDelegate(resetPrgMethod).ToPointer();
}
void ProteinUIInterface::SetSetProgressDelegate(CurrentProgressValue^ resetPrgMethod )
{
	if(resetPrgMethod==nullptr)
		return;
	GetMainActiveView()->mSetProgressMethod=
		(void (*)(int,int))Marshal::GetFunctionPointerForDelegate(resetPrgMethod).ToPointer();
}

void ProteinUIInterface::InitUI(IntPtr hInstance,IntPtr hwd)
{
	::GetMainApp()->m_HInstance =(HINSTANCE)hInstance.ToPointer();
	::GetMainApp()->m_CanvsHandle=((HWND)hwd.ToPointer());
	::GetMainApp()->InitInstance();
}
 
ProteinUIInterface::!ProteinUIInterface()
{
	
}
ProteinUIInterface::~ProteinUIInterface()
{
	
}
void ProteinUIInterface::CleanResource()
{
	try
	{
		::RemoveMainApp();
	}
	catch (CMemoryException& e)
	{
	}
	catch (CFileException& e)
	{
	}
	catch (CException& e)
	{
	}
}
 
void ProteinUIInterface::OpenPDBFile()
{
	GetMainApp()->OnFileOpen();
}
void ProteinUIInterface::OpenPDBFile(String^ fileName)
{
	CString cFileName = MStrToCString(fileName);
	::GetMainApp()->OpenDocumentFile(cFileName);
}
void ProteinUIInterface::WndProcess(System::Windows::Forms::Message% m)
{
	if (GetMainActiveView())
	{
		GetMainActiveView()->DefWindowProc(m.Msg, (WPARAM)m.WParam.ToPointer(), (LPARAM)m.LParam.ToPointer());
	}
}
void ProteinUIInterface::OnPaint()
{
	GetMainActiveView()->OnPaint();
}
void ProteinUIInterface::OnSizeChange()
{
	//GetMainActiveView()->OnSizeChange();
}
void ProteinUIInterface::OnSize(int width,int heigh)
{
}

void ProteinUIInterface::AddPDB()
{
   GetMainActiveView()->OnAddPdb();
}
void ProteinUIInterface::AddPDB(String^ fileName)
{
	CString cFileName = MStrToCString(fileName);
	::GetMainActiveView()->AddPDB(cFileName);
}
void ProteinUIInterface::ClosePDB( )
{
	GetMainActiveView()->OnClosePdb();
}
void ProteinUIInterface::OpenWorkspace( )
{
	GetMainApp()->OnOpenWorkspace();
}
void ProteinUIInterface::OpenWorkspace( String^ fileName)
{
	GetMainApp()->OpenWorkspaceFile(MStrToCString(fileName));
}
void ProteinUIInterface::OnSaveAsWorkspace()
{
	GetMainApp()->OnSaveAsWorkspace();
}
void ProteinUIInterface::OnSaveWorkspace()
{
	GetMainApp()->OnSaveWorkspace();
}
void ProteinUIInterface::OnSaveWorkspace(String^ fileName)
{
	GetMainApp()->SaveWorkspaceFile(MStrToCString(fileName));
}
///////////////////////////////////////////////////////////////
void ProteinUIInterface::OnButtonBall()
{
	GetMainActiveView()->OnButtonBall();
}
void ProteinUIInterface::OnButtonBallStick()
{
	GetMainActiveView()->OnButtonBallStick();
}
void ProteinUIInterface::OnButtonDotsurface()
{
	GetMainActiveView()->OnButtonDotsurface();
}
void ProteinUIInterface::OnButtonDotsurfaceWithResolution(UINT id)
{
	GetMainActiveView()->OnButtonDotsurfaceWithResolution(id);
}
void ProteinUIInterface::OnButtonRibbon()
{
	GetMainActiveView()->OnButtonRibbon();
}
void ProteinUIInterface::OnButtonStick()
{
	GetMainActiveView()->OnButtonStick();
}
void ProteinUIInterface::OnButtonWireframe()
{
    GetMainActiveView()->OnButtonWireframe();
	  
}
void ProteinUIInterface::OnButtonNextFrame()
{
	 GetMainActiveView()->OnButtonNextFrame();
}
void ProteinUIInterface::OnButtonGoFirst()
{
    GetMainActiveView()->OnButtonGoFirst();
}
void ProteinUIInterface::OnButtonGoLast()
{
	GetMainActiveView()->OnButtonGoLast();
}
void ProteinUIInterface::OnButtonPlay()
{
	GetMainActiveView()->OnButtonPlay();
}
void ProteinUIInterface::OnButtonPrevFrame()
{
   GetMainActiveView()->OnButtonPrevFrame();
}
void ProteinUIInterface::OnButtonStop()
{
   GetMainActiveView()->OnButtonStop();
}
void ProteinUIInterface::OnButtonPlayFast()
{
	GetMainActiveView()->OnButtonPlayFast();
}
void ProteinUIInterface::OnButtonPlaySlow()
{
	GetMainActiveView()->OnButtonPlaySlow();
}
void ProteinUIInterface::OnAddPdb()
{
   GetMainActiveView()->OnAddPdb();
}
void ProteinUIInterface::OnClosePdb()
{
	GetMainActiveView()->OnClosePdb();
}
void ProteinUIInterface::OnCenterMolecule()
{
	GetMainActiveView()->OnCenterMolecule();
}
void ProteinUIInterface::OnViewAll()
{
	GetMainActiveView()->OnViewAll();
}
void ProteinUIInterface::OnViewAllDisplayParams()
{
	GetMainActiveView()->OnViewAllDisplayParams();
}
void ProteinUIInterface::OnNextSelectionList()
{
	//GetMainActiveView()->OnNextSelectionList();
}
void ProteinUIInterface::OnPrevSelectionList()
{
	//GetMainActiveView()->OnPrevSelectionList();
}
void ProteinUIInterface::OnNextActivePDB()
{
	GetMainActiveView()->OnNextActivePDB();
}
void ProteinUIInterface::OnPrevActivePDB()
{
	GetMainActiveView()->OnPrevActivePDB();
}
void ProteinUIInterface::OnDisplayBioUnit()
{
	GetMainActiveView()->OnDisplayBioUnit();
}
void ProteinUIInterface::OnFlagMoleculeSelectionCenter()
{
	GetMainActiveView()->OnFlagMoleculeSelectionCenter();
}
 
void ProteinUIInterface::OnAttatchBiounit()
{
	GetMainActiveView()->OnAttatchBiounit();
}

void ProteinUIInterface::OnSurfaceGenAlgorithmMQ()
{
	GetMainActiveView()->OnSurfaceGenAlgorithmMQ();
}
void ProteinUIInterface::OnSurfaceGenAlgorithmMSMS()
{
	GetMainActiveView()->OnSurfaceGenAlgorithmMSMS();
}
void ProteinUIInterface::OnSurfaceBiounitGenAlgorithmMQ()
{
	GetMainActiveView()->OnSurfaceBiounitGenAlgorithmMQ();
}
void ProteinUIInterface::OnSurfaceBiounitGenAlgorithmMSMS()
{
	GetMainActiveView()->OnSurfaceBiounitGenAlgorithmMSMS();
}

void  ProteinUIInterface::OnDisplayBioUnitStyle(int mode)
{
    GetMainActiveView()->OnDisplayBioUnitStyle(mode);
}
void  ProteinUIInterface::OnDisplayBioUnitSurface(int quality)
{
    GetMainActiveView()->OnDisplayBioUnitSurface(quality);
}


void ProteinUIInterface::CreateResiduesPanel(IntPtr hwnd)
{
     GetMainActiveView()->CreateResiduePanel((HWND)hwnd.ToPointer());
}
void ProteinUIInterface::ResiduesSizeChange(int cx,int cy)
{
	GetMainActiveView()->ResiduesSizeChange(cx,cy);
}

void ProteinUIInterface::CreateSelectPanel(IntPtr hwnd)
{
    GetMainActiveView()->CreateSelectPanel((HWND)hwnd.ToPointer());
}
void ProteinUIInterface::SelectPanelSizeChange(int cx, int cy)
{
    GetMainActiveView()->SelectPanelSizeChange(cx,cy);
}

void ProteinUIInterface::CreatePDBTreePanel(IntPtr hwnd)
{
	GetMainActiveView()->CreatePDBTreePanel((HWND)hwnd.ToPointer());
}
void ProteinUIInterface::PDBTreePanelSizeChange(int cx, int cy)
{
	GetMainActiveView()->PDBTreePanelSizeChange(cx,cy);
}
 
CPIProperty^ ProteinUIInterface::GetPDBProperty()
{
   return GetMainActiveView()->GetProperty();
}
 
void ProteinUIInterface::SetPropertyValue(Object^ entity,String^ propertyName,Object^ value)
{
	Type^ lastTypeInfo = entity->GetType();  
	if(lastTypeInfo ==nullptr)
		return;
	System::Reflection::PropertyInfo^ fieldType = lastTypeInfo->GetProperty(propertyName,System::Reflection::BindingFlags::Instance|System::Reflection::BindingFlags::Public );
	if(fieldType ==nullptr) 
		return;
	fieldType->SetValue(entity,value,nullptr);
}
 
 void ProteinUIInterface::OnFileScreenshot()
 {
	 GetMainActiveView()->OnFileScreenshot();
 }
 void ProteinUIInterface::MakeMovie()
 {
	 //GetMainApp()->OnMakeMovie();
 }

 void ProteinUIInterface::ShowLog(bool bShow)
 {
	 GetMainActiveView()->m_bShowLog=bShow? TRUE:FALSE;
	 GetMainActiveView()->OnPaint();
 }

///////////////////////////////////////////////////////////////////////////
 void ProteinUIInterface::OnActivePDB()
 {
	 GetMainActiveView()->OnActivePDB();
 }
 void ProteinUIInterface::OnUnion()
 {
	  GetMainActiveView()->GetSelectPanel()->UnionPDB();
 }
 void ProteinUIInterface::OnIntersect()
 {
	 GetMainActiveView()->GetSelectPanel()->IntersectPDB();
 }
 void ProteinUIInterface::OnSubtract()
 {
	 GetMainActiveView()->GetSelectPanel()->SubtractPDB();
 }
 void ProteinUIInterface::OnOperatResult()
 {
	 GetMainActiveView()->GetSelectPanel()->OperateResult();
 }
 void ProteinUIInterface::OnSaveSelection()
 {
	  GetMainActiveView()->GetCPDBTreePane()->OnSaveSelection();
 }
 void ProteinUIInterface::OnDisplayResiduesSelected()
 {
	 GetMainActiveView()->GetResiduePanel()->DisplaySelected();
 }

 void ProteinUIInterface::RenderProteinVistaRenderer()
 {
	  GetMainActiveView()->RenderProteinVistaRenderer();
 }


 void ProteinUIInterface::OpenPDBFile(array<String^>^ files)
 {
	 GetMainApp()->OpenPDBFiles(files);
 }
 void ProteinUIInterface::AddPDB(array<String^>^ files)
 {
	 GetMainActiveView()->OnAddPdb(files);
 }