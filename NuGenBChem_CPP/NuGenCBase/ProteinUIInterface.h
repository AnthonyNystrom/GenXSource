#pragma once
using namespace System;
using namespace NuGenCbaseInterface;
using namespace System::Drawing;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace Microsoft::DirectX;
using namespace Microsoft::DirectX::Direct3D;
using namespace System::Runtime::InteropServices;

ref class CPIProperty;
namespace NGVChem 
{
	[System::Runtime::InteropServices::UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate void OutputMethod(String^ logInf);
	[System::Runtime::InteropServices::UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate void GetHeaderInfoMethod(String^ headerInf);
	[System::Runtime::InteropServices::UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate void ProgressMethod(int value);
	[System::Runtime::InteropServices::UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate void CurrentProgressValue(int curValue,int totalValue);

	[System::Runtime::InteropServices::UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate void NotifyPropertyChanged();

	public delegate void PDBPropertChange(CPIProperty^ propertyEntity);
	public enum class  PropertyType
	{
		CPIPropertyScene,
		CPIPropertyWireframe,
		CPIPropertyStick,
		CPIPropertySpaceFill,
		CPIPropertyBallnStick,
		CPIPropertyRibbon,
		CPIPropertySurface
	};
	public ref class ProteinUIInterface
	{
	private:
		void propteryChange(CPIProperty^ propertyEntity){};
	    NotifyPropertyChanged^ pdbPropertyChangeMethod;
	public:
		ProteinUIInterface();
		~ProteinUIInterface();
		!ProteinUIInterface();
		void CleanResource();
	    event PDBPropertChange^ OnPropertyChange;
		void OnReceivedPropertyChange();
	public:
		void InitUI(IntPtr hInstance,IntPtr hwd);
		void OpenPDBFile();
		void OpenPDBFile(String^ fileName);
		void OpenPDBFile(array<String^>^ files);
		void AddPDB(array<String^>^ files);
		void AddPDB();
		void AddPDB(String^ fileName);
		void ClosePDB();
		void OpenWorkspace();
		void OpenWorkspace(String^ fileName);
		void OnSaveAsWorkspace();
		void OnSaveWorkspace();
		void OnSaveWorkspace(String^ fileName);
		void OnPaint();
		void RenderProteinVistaRenderer();
		void OnSizeChange();

		void OnSize(int width,int heigh);
		void WndProcess(System::Windows::Forms::Message % m);
		
		System::Windows::Forms::ComboBox^ GetPDBCombox();

		void SetOutputDelegate(OutputMethod^ outputMethod );
		void SetHeaderInfoDelegate(GetHeaderInfoMethod^ getHeaderMethod );
		void SetInitProgressDelegate(ProgressMethod^ initPrgMethod );
		void SetEndProgressDelegate(ProgressMethod^ endPrgMethod );
		void SetResetProgressDelegate(ProgressMethod^ resetPrgMethod );
		void SetSetProgressDelegate(CurrentProgressValue^ resetPrgMethod );

		void CreateResiduesPanel(IntPtr hwnd);
		void ResiduesSizeChange(int cx, int cy);

		void CreateSelectPanel(IntPtr hwnd);
		void SelectPanelSizeChange(int cx, int cy);

		void CreatePDBTreePanel(IntPtr hwnd);
		void PDBTreePanelSizeChange(int cx, int cy);
  
		CPIProperty^ GetPDBProperty();
		void SetPropertyValue(Object^ entity,String^ propertyName,Object^ value);
		void ShowLog(bool bShow);
	public:
		void OnButtonBall();
		void OnButtonBallStick();	
		void OnButtonDotsurface();	
		void OnButtonDotsurfaceWithResolution(UINT id);
		void OnButtonRibbon();
		void OnButtonStick();
		void OnButtonWireframe();
		void OnButtonNextFrame();
		void OnButtonGoFirst();
		void OnButtonGoLast();
		void OnButtonPlay();
		void OnButtonPrevFrame();
		void OnButtonStop();
		void OnButtonPlayFast();
		void OnButtonPlaySlow();
		void OnAddPdb();
		void OnClosePdb();
		void OnCenterMolecule();
		void OnViewAll();
		void OnViewAllDisplayParams();
		void OnNextSelectionList();
		void OnPrevSelectionList();
		void OnNextActivePDB();
		void OnPrevActivePDB();
		void OnDisplayBioUnit();
		void OnFlagMoleculeSelectionCenter();
		void OnDisplayBioUnitStyle(int mode);
		void OnDisplayBioUnitSurface(int quality);
		void OnAttatchBiounit();
		void OnSurfaceGenAlgorithmMQ();
		void OnSurfaceGenAlgorithmMSMS();
		void OnSurfaceBiounitGenAlgorithmMQ();
		void OnSurfaceBiounitGenAlgorithmMSMS();
	public:
		void OnFileScreenshot();
		void MakeMovie();
	public:
		void OnActivePDB();
		void OnUnion();
		void OnIntersect();
	    void OnSubtract();
		void OnOperatResult();
		void OnSaveSelection();
		void OnDisplayResiduesSelected();
	};
}