#pragma once

#ifdef _MANAGED

#include "pdb.h"
#include "pdbInst.h"
#include "pdbInst.h"
#include "Interface.h"
 
using namespace System;
using namespace NuGenCbaseInterface;
using namespace System::Drawing;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace Microsoft::DirectX;
using namespace Microsoft::DirectX::Direct3D;

ref class CDotNetAtom;
ref class CDotNetResidue;
ref class CDotNetChain;
ref class CDotNetModel;
ref class CDotNetPDB;
ref class CDotNetProteinInsight;

class CSelectionDisplay;
void ForceRenderScene();
#define ManagedColor2COLORREF(_value) RGB(_value.R, _value.G, _value.B)
[Serializable]
public ref class CDotNetPropertyHelix: public IPropertyHelix
{
public:
	CDotNetPropertyHelix( CSelectionDisplay * pSelectionDisplay ) {m_pSelectionDisplay = pSelectionDisplay; }
	virtual void Init() {}
	CSelectionDisplay * m_pSelectionDisplay;

	
	property	bool		Show { virtual bool get(); virtual void set(bool); }
	
	property	bool		ShowTexture { virtual bool get(); virtual void set(bool); }
	
	property	String ^	TextureFilename { virtual String ^ get(); virtual void set(String ^); }
	
	property	int			TextureCoordU { virtual int get(); virtual void set(int); }
	
	property	int			TextureCoordV { virtual int get(); virtual void set(int); }
	
	property	System::Drawing::Color		Color { virtual System::Drawing::Color get(); virtual void set(System::Drawing::Color ); }
	
	property	System::Drawing::Size		Size { virtual System::Drawing::Size get(); virtual void set(System::Drawing::Size ); }
	
	property	IPropertyHelix::IFitting	Fitting { virtual IPropertyHelix::IFitting get(); virtual void set(IPropertyHelix::IFitting); }
	
	property	IPropertyHelix::IShape		Shape { virtual IPropertyHelix::IShape get(); virtual void set(IPropertyHelix::IShape); }
	
	property	bool		ShowCoilOnHelix { virtual bool get(); virtual void set(bool); }
};

[Serializable]
public ref class CDotNetPropertySheet: public IPropertySheet
{
public:
	CDotNetPropertySheet( CSelectionDisplay * pSelectionDisplay ) {m_pSelectionDisplay = pSelectionDisplay; }
	virtual void Init() {}
	CSelectionDisplay * m_pSelectionDisplay;

	
	property	bool		Show { virtual bool get(); virtual void set(bool); }
	
	property	bool		ShowTexture { virtual bool get(); virtual void set(bool); }
	
	property	String ^	TextureFilename { virtual String ^ get(); virtual void set(String ^); }
	
	property	int			TextureCoordU { virtual int get(); virtual void set(int); }
	
	property	int			TextureCoordV { virtual int get(); virtual void set(int); }
	
	property	System::Drawing::Color		Color { virtual System::Drawing::Color get(); virtual void set(System::Drawing::Color); }
	
	property	System::Drawing::Size		Size { virtual System::Drawing::Size get(); virtual void set(System::Drawing::Size); }
	
	property	IPropertySheet::IShape		Shape { virtual IPropertySheet::IShape get(); virtual void set(IPropertySheet::IShape); }
	
	property	bool		ShowCoilOnSheet { virtual bool get(); virtual void set(bool); }
};

[Serializable]
public ref class CDotNetPropertyCoil: public IPropertyCoil
{
public:
	CDotNetPropertyCoil( CSelectionDisplay * pSelectionDisplay ) {m_pSelectionDisplay = pSelectionDisplay; }
	virtual void Init() {}
	CSelectionDisplay * m_pSelectionDisplay;

	
	property	bool		Show { virtual bool get(); virtual void set(bool); }
	
	property	bool		ShowTexture { virtual bool get(); virtual void set(bool); }
	
	property	String ^	TextureFilename { virtual String ^ get(); virtual void set(String ^); }
	
	property	int			TextureCoordU { virtual int get(); virtual void set(int); }
	
	property	int			TextureCoordV { virtual int get(); virtual void set(int); }
	
	property	System::Drawing::Color		Color { virtual System::Drawing::Color get(); virtual void set(System::Drawing::Color); }
	
	property	System::Drawing::Size		Size { virtual System::Drawing::Size get(); virtual void set(System::Drawing::Size); }
	
	property	IPropertyCoil::IShape		Shape { virtual IPropertyCoil::IShape get(); virtual void set(IPropertyCoil::IShape); }
};



///////////////////////////////////////////////////////////////////////////////////////
[Serializable]
public ref class CDotNetProperty : public IProperty
{
public:
	CDotNetProperty(){}
	CDotNetProperty(CSelectionDisplay * pSelectionDisplay);
	virtual void Init();

	
	property String ^ Name { virtual String ^ get(); virtual void set(String ^ name); }
	
	property bool DisplaySideChain { virtual bool get(); virtual void set(bool display); }
	
	property IProperty::IColorScheme ColorScheme { virtual IProperty::IColorScheme get(); virtual void set(IProperty::IColorScheme colorScheme); }
	
	property List<System::Drawing::Color> ^ CustomizeColors { virtual List<System::Drawing::Color> ^ get(); virtual void set(List<System::Drawing::Color> ^); }

	
	property	IAnnotation ^		AnnotationVP{ virtual IAnnotation ^ get() { return m_annotationVP;} }
	
	property	IAnnotation ^		AnnotationAtom{ virtual IAnnotation ^ get() { return m_annotationAtom;} }
	
	property	IAnnotation ^		AnnotationResidue{ virtual IAnnotation ^ get() { return m_annotationResidue;} }

	
	property array <IClipping ^ > ^ Clippings { virtual array<IClipping ^> ^ get(){ return m_arrayClippings; } }

	
	property	IProperty::IShaderQuality	ShaderQuality { virtual IProperty::IShaderQuality get(); virtual void set(IProperty::IShaderQuality value); }
	
	property	IProperty::IGeometryQuality	GeometryQuality { virtual IProperty::IGeometryQuality get(); virtual void set(IProperty::IGeometryQuality value); }
	
	property	bool			ShowSelectionMark{ virtual bool get(); virtual void set(bool value); }

	
	property	bool			ShowIndicateSelectionMark{ virtual bool get(); virtual void set(bool value); }
	
	property	System::Drawing::Color			IndicateSelectionMarkColor{ virtual System::Drawing::Color get();  virtual void set(System::Drawing::Color _value); }

	
	property int IntensityAmbient { virtual int get(); virtual void set(int _value); }
	
	property int IntensityDiffuse { virtual int get(); virtual void set(int _value); }
	
	property int IntensitySpecular { virtual int get(); virtual void set(int _value); }

	
	property	bool  DisplayAxis { virtual bool get(); virtual void set(bool display); }
	
	property	int		AxisSize {  virtual int get(); virtual void set(int size); }


private:
	CSelectionDisplay * m_pSelectionDisplay;
	array <IClipping ^ > ^ m_arrayClippings;
	IAnnotation ^ m_annotationVP;
	IAnnotation ^ m_annotationAtom;
	IAnnotation ^ m_annotationResidue;
};

[Serializable]
public ref class CDotNetPropertyScene: public IPropertyScene 
{
private:
	CSelectionDisplay * m_pSelectionDisplay;
public:
	CDotNetPropertyScene(CSelectionDisplay * pSelectionDisplay)
	{
		this->m_pSelectionDisplay = pSelectionDisplay;
	}
	CDotNetPropertyScene() {  }
	virtual	 void Init() ;

	
	property	Color BackgroundColor { virtual Color get(); virtual void set(Color color); }
	
	property	bool  ShowBackgroundTexture { virtual bool get(); virtual void set(bool show); }
	
	property	String^ BackgroundTextureFilename { virtual String ^ get(); virtual void set(String ^ show); }

	
	property	IClipping ^		Clipping { virtual IClipping ^ get() { return m_clipping; } }
	
	property	array < ILight ^ > ^ Lights { virtual array < ILight ^ > ^ get() { return m_arrayLights; } }

	
	property	bool  DisplayAxis  { virtual bool get() ; virtual void set(bool display) ; }
	
	property	int		AxisSize {  virtual int get()  ; virtual void set(int size) ; }

	
	property	double	ClipPlaneNear { virtual double get(); virtual void set(double near); }
	
	property	double	ClipPlaneFar { virtual double get(); virtual void set(double far); }

	
	property	Vector3	CameraPosition { virtual Vector3 get(); virtual void set(Vector3 ); }

	
	property	IPropertyScene::ICameraType	CameraType { virtual IPropertyScene::ICameraType get(); virtual void set(IPropertyScene::ICameraType cameraType); }
	
	property	int	FOV { virtual int get(); virtual void set(int ); }
	
	property	int	SizeViewVol { virtual int get(); virtual void set(int ); }

	
	property	IPropertyScene::IShaderQuality	ShaderQuality { virtual IPropertyScene::IShaderQuality get() ; virtual void set(IPropertyScene::IShaderQuality value) ; }
	
	property	IPropertyScene::IGeometryQuality	GeometryQuality { virtual IPropertyScene::IGeometryQuality get() ; virtual void set(IPropertyScene::IGeometryQuality value) ; }
	
	property	IPropertyScene::IAntiAliasing		AntiAliasing	{ virtual IPropertyScene::IAntiAliasing get(); virtual void set(IPropertyScene::IAntiAliasing aa); }

	
	property	bool	 ShowSelectionMark{ virtual bool get() ; virtual void set(bool value) ; }

	
	property	bool	 EnableAO{ virtual bool get(); virtual void set(bool ); }
	
	property	int		 AORange{ virtual int get(); virtual void set(int ); }
	
	property	int		 AOSampling{ virtual int get(); virtual void set(int ); }
	
	property	int		 AOIntensity{ virtual int get(); virtual void set(int ); }
	
	property	IPropertyScene::IAOBlurType		AOBlurType { virtual IPropertyScene::IAOBlurType get(); virtual void set(IPropertyScene::IAOBlurType value); }
	
	property	bool	 AOFullSizeBuffer{ virtual bool get(); virtual void set(bool ); }

	
	property	bool  DepthOfField { virtual bool get(); virtual void set(bool dof); }
	
	property	Color FogColor { virtual Color get(); virtual void set(Color color); }
	
	property	int	  FogStart { virtual int get(); virtual void set(int start); }
	
	property	int	  FogEnd   { virtual int get(); virtual void set(int end); }

	virtual void CameraAnimation(IAtom ^ atom, float time);
	virtual void CameraAnimation(IResidue ^ residue, float time);
	virtual void CameraAnimation(Vector3 pos , float time);
	virtual void CameraAnimation(IVP ^ vp, float time);
	virtual void CameraAnimation();
private:
	IClipping ^				m_clipping;
	array < ILight ^ > ^	m_arrayLights;
};

[Serializable]
public ref class CDotNetPropertyWireframe: public IPropertyWireframe 
{
public:
	CDotNetPropertyWireframe(){}
	CDotNetPropertyWireframe( CSelectionDisplay * pSelectionDisplay ) {m_pSelectionDisplay = pSelectionDisplay; }
	virtual	 void Init()  {}
	CSelectionDisplay * m_pSelectionDisplay;

	property IPropertyWireframe::IDisplayMode DisplayMode { virtual IPropertyWireframe::IDisplayMode get(); virtual void set(IPropertyWireframe::IDisplayMode _value); }

	property int LineWidth { virtual int get(); virtual void set(int _value); }

};

[Serializable]
public ref class CDotNetPropertyStick: public IPropertyStick 
{
public:
	CDotNetPropertyStick(){}
	CDotNetPropertyStick( CSelectionDisplay * pSelectionDisplay ) {m_pSelectionDisplay = pSelectionDisplay; }
	virtual	 void Init() {}
	CSelectionDisplay * m_pSelectionDisplay;

	
	property int SphereResolution { virtual int get(); virtual void set(int); }
	
	property int CylinderResolution{ virtual int get(); virtual void set(int); }
	
	property double	StickSize{ virtual double get(); virtual void set(double); }

};

[Serializable]
public ref class CDotNetPropertySpaceFill: public IPropertySpaceFill 
{
public:
	CDotNetPropertySpaceFill(){}
	CDotNetPropertySpaceFill( CSelectionDisplay * pSelectionDisplay ) {m_pSelectionDisplay = pSelectionDisplay; }
	virtual	 void Init()  {}
	CSelectionDisplay * m_pSelectionDisplay;
 
	property int	SphereResolution { virtual int get(); virtual void set(int); }
};

[Serializable]
public ref class CDotNetPropertyBallnStick: public IPropertyBallnStick,public CDotNetProperty
{
public:
	CDotNetPropertyBallnStick(){}
	CDotNetPropertyBallnStick( CSelectionDisplay * pSelectionDisplay ) {m_pSelectionDisplay = pSelectionDisplay; }
	virtual	 void Init() override {}
	CSelectionDisplay * m_pSelectionDisplay;

	
	property int SphereResolution { virtual int get(); virtual void set(int); }
	
	property int CylinderResolution { virtual int get(); virtual void set(int); }

	
	property double SphereRadius { virtual double get(); virtual void set(double); }
	
	property double CylinderSize { virtual double get(); virtual void set(double); }
};

[Serializable]
public ref class CDotNetPropertyRibbon: public IPropertyRibbon 
{
private:
	CDotNetPropertyHelix ^ m_propertyHelix;
	CDotNetPropertySheet ^ m_propertySheet;
	CDotNetPropertyCoil  ^ m_propertyCoil;
public:
	virtual void	SelectSugarInDNA();
	virtual void	SelectBackBoneInDNA();
	virtual void	SelectInnerAtomsInDNA();

	CDotNetPropertyRibbon(){}
	CDotNetPropertyRibbon( CSelectionDisplay * pSelectionDisplay ) {m_pSelectionDisplay = pSelectionDisplay; }
	virtual	 void Init() ;
	CSelectionDisplay * m_pSelectionDisplay;

	property int	CurveTension { virtual int get(); virtual void set(int); }
	property int	CurveResolution { virtual int get(); virtual void set(int); }

	property IPropertyHelix ^ Helix { virtual IPropertyHelix ^ get() { return m_propertyHelix; } }
	property IPropertySheet ^ Sheet { virtual IPropertySheet ^ get() { return m_propertySheet; } }
	 
	property IPropertyCoil ^ Coil { virtual IPropertyCoil ^ get() { return m_propertyCoil; } }
};

[Serializable]
public ref class CDotNetPropertySurface: public IPropertySurface 
{
public:
	CDotNetPropertySurface(){}
	CDotNetPropertySurface( CSelectionDisplay * pSelectionDisplay ) 
	{m_pSelectionDisplay = pSelectionDisplay; }
	CSelectionDisplay * m_pSelectionDisplay;
	virtual	 void Init()  {}

	
	property	IPropertySurface::IDisplayMethod	DisplayMethod { virtual IPropertySurface::IDisplayMethod get(); virtual void set(IPropertySurface::IDisplayMethod); }
	
	property	int		Transparency { virtual int get(); virtual void set(int); }

	
	property	double	ProbeSphereRadius { virtual double get(); virtual void set(double); }
	
	property IPropertySurface::IQuality GeometryQuality { virtual IPropertySurface::IQuality get() ; virtual void set(IPropertySurface::IQuality) ; }
	
	property	IPropertySurface::IAlgorithm Algorithm{ virtual IPropertySurface::IAlgorithm get(); virtual void set(IPropertySurface::IAlgorithm); }

	
	property	bool				AddHETATM { virtual bool get(); virtual void set(bool); }

	
	property	bool				ShowCurvature { virtual bool get(); virtual void set(bool); }
	
	property	IPropertySurface::ICurvatureRingSize	CurvatureRingSize { virtual IPropertySurface::ICurvatureRingSize get(); virtual void set(IPropertySurface::ICurvatureRingSize); }

	
	property	IPropertySurface::IColorSmoothing	ColorSmoothing { virtual IPropertySurface::IColorSmoothing get(); virtual void set(IPropertySurface::IColorSmoothing); }
	
	property	bool				DepthSort { virtual bool get(); virtual void set(bool); }

	
	property	bool				UseInnerFaceColor { virtual bool get(); virtual void set(bool); }
	
	property	int					InnerFaceColorBlend { virtual int get(); virtual void set(int); }
	
	property	Color				InnerFaceColor { virtual Color get(); virtual void set(Color); }

	virtual		void		SelectSurfaceAtoms();
};
/////////////////////////////////////////////////////////
[Serializable]
public ref class CDotNetAtom : public IAtom
{
public:
	//	정상적인 atom 일 경우에..
	CDotNetAtom( CAtomInst * pAtom , CDotNetResidue ^ pResidue );
	//	HETATOM 일 경우에 Residue에 속하지 않는다.
	CDotNetAtom( CAtomInst * pAtom , CDotNetChain ^ pChain );

	virtual void Init();

	property	long	Num { virtual long get(){ return m_pAtomInst->GetAtom()->m_serial; } }
	property	String ^ Name { virtual String ^ get(); }
	property	String ^ ResidueName { virtual String ^ get(){ return gcnew String(m_pAtomInst->GetAtom()->m_residueName); } }
	property	float	Occupancy { virtual float get() { return m_pAtomInst->GetAtom()->m_occupancy; } virtual void set(float occ) { m_pAtomInst->GetAtom()->m_occupancy = occ; } }
	property	float	Temperature { virtual float get() { return m_pAtomInst->GetAtom()->m_temperature; } virtual void set(float temp) { m_pAtomInst->GetAtom()->m_temperature = temp; } }
	property	float	Hydropathy { virtual float get() { return m_pAtomInst->GetAtom()->m_hydropathy; } virtual void set(float hyd) { m_pAtomInst->GetAtom()->m_hydropathy = hyd; } }

	property	bool	SideChain { virtual bool get(){ return Convert::ToBoolean(m_pAtomInst->GetAtom()->m_bSideChain); } }

	property	IAtom::IType	Type { virtual IAtom::IType get() {return static_cast<IAtom::IType>(m_pAtomInst->GetAtom()->m_typeAtom); } }

	property	Vector3		Position {	virtual Vector3  get() {return Vector3(m_pAtomInst->GetAtom()->m_pos.x, m_pAtomInst->GetAtom()->m_pos.y, m_pAtomInst->GetAtom()->m_pos.z ); } 
										virtual void set(Vector3 pos) { m_pAtomInst->GetAtom()->m_pos.x = pos.X; m_pAtomInst->GetAtom()->m_pos.y = pos.Y; m_pAtomInst->GetAtom()->m_pos.z = pos.Z; } }
	property	Vector3		PositionTransformed { virtual Vector3 get(); }

	property	IResidue ^	ParentResidue { virtual IResidue ^ get(){ return safe_cast<IResidue ^>(m_pResidue); } }
	property	IChain ^	ParentChain { virtual IChain ^ get(){ return safe_cast<IChain ^>(m_pChain); } }
	property	IPDB ^		ParentPDB { virtual IPDB ^ get(){ return safe_cast<IPDB ^>(m_pPDB); } }

	virtual void	SetSelect(bool select, bool bNeedUpdate);
	property	bool		Select { virtual bool get(); virtual void set(bool bValue); }

	CAtomInst *			GetUnManagedAtom() { return m_pAtomInst; }

private:
	CDotNetResidue ^	m_pResidue;
	CDotNetChain ^		m_pChain;
	CDotNetPDB ^		m_pPDB;

	CAtomInst *			m_pAtomInst;
};

[Serializable]
public ref class CDotNetResidue: public IResidue
{
public:
	CDotNetResidue(CResidueInst * pResidueInst, CDotNetChain ^ pChain);
	virtual void Init();

	property	String ^	Name { virtual String ^ get(){ return gcnew String(m_pResidueInst->GetResidue()->GetResidueName()); } }
	property	String ^	NameOneChar { virtual String ^ get(){ return gcnew String(m_pResidueInst->GetResidue()->m_residueNameOneChar); } }

	property	long		Num { virtual long get(){ return m_pResidueInst->GetResidue()->GetResidueNum(); } }

	property	IResidue::ISSType		SSType {virtual IResidue::ISSType  get(){ return static_cast<IResidue::ISSType>(m_pResidueInst->GetResidue()->GetSS()); } }
	property	IResidue::IHelixType  HelixType { virtual IResidue::IHelixType  get(){ return static_cast<IResidue::IHelixType>(m_pResidueInst->GetResidue()->GetTypeHelix()); } }

	property	IAtom ^ AtomN { virtual IAtom ^ get(){ return m_arrayAtomSpecial[MAINCHAIN_N]; } }
	property	IAtom ^	AtomCa{ virtual IAtom ^ get(){ return m_arrayAtomSpecial[MAINCHAIN_CA]; } }
	property	IAtom ^	AtomCb{ virtual IAtom ^ get(){ return m_arrayAtomSpecial[RESIDUE_CB]; } }
	property	IAtom ^	AtomC { virtual IAtom ^ get(){ return m_arrayAtomSpecial[MAINCHAIN_C]; } }
	property	IAtom ^	AtomO { virtual IAtom ^ get(){ return m_arrayAtomSpecial[MAINCHAIN_O]; } }

	property	bool		ExistMainChain { virtual bool get(){return Convert::ToBoolean(m_pResidueInst->GetResidue()->m_bExistMainChain); } }

	property	IChain ^	ParentChain { virtual IChain ^ get(){ return safe_cast<IChain ^>(m_pChain); } }
	property	IPDB ^		ParentPDB { virtual IPDB ^ get(){ return safe_cast<IPDB ^>(m_pPDB); } }

	property List<IAtom ^ > ^ Atoms { virtual List<IAtom ^> ^ get(){ return m_arrayAtoms; } }

	virtual void	SetSelect(bool select, bool bNeedUpdate);
	property	bool		Select { virtual bool get(); virtual void set(bool bValue); }

	CResidueInst * GetUnManagedResidue() { return m_pResidueInst; }

private:
	List<IAtom ^> ^ m_arrayAtoms;
	array <CDotNetAtom ^> ^ m_arrayAtomSpecial;

	CResidueInst *	m_pResidueInst;
	CDotNetChain ^ m_pChain;
	CDotNetPDB ^ m_pPDB;
};

[Serializable]
public ref class CDotNetChain: public IChain
{
public:
	CDotNetChain( CChainInst * pChain , CDotNetModel ^ pModel );
	virtual void Init();

	property String ^ ID { virtual String ^ get() { return gcnew String(CString(m_pChainInst->GetChain()->m_chainID)); } }
	property String ^ PDBID { virtual String ^ get() { return gcnew String(m_pChainInst->GetChain()->m_strPDBID); } }

	property IPDB ^ ParentPDB { virtual IPDB ^ get() { return safe_cast<IPDB ^>(m_pPDB); } }
	property IModel ^ ParentModel { virtual IModel ^ get() { return safe_cast<IModel ^>(m_pModel); } }

	property String ^ Sequence { virtual String ^ get() { return gcnew String(m_pChainInst->GetChain()->m_strSequenceData); } }

	virtual List<IResidue ^ > ^ GetResidues(String ^ residueName);
	virtual List<IAtom ^ > ^ GetAtoms(String ^ atomName);
	virtual List<IResidue ^ > ^ GetSSResidues(IResidue::ISSType type);

	property List<IAtom ^ > ^ Atoms { virtual List<IAtom ^> ^ get(){ return m_arrayAtoms; } }
	property List<IAtom ^ > ^ HETAtoms { virtual List<IAtom ^> ^ get(){ return m_arrayHETAtoms; } }
	property List<IResidue ^ > ^ Residues { virtual List<IResidue ^> ^ get(){ return m_arrayResidues; } }

	virtual void	SetSelect(bool select, bool bNeedUpdate);
	property	bool		Select { virtual bool get(); virtual void set(bool bValue); }

	property bool	IsDNA { virtual bool get() { return Convert::ToBoolean(m_pChainInst->GetChain()->m_bDNA); } }
private:
	CChainInst * m_pChainInst;
	CDotNetModel ^ m_pModel;
	CDotNetPDB ^ m_pPDB;

	List<IResidue ^> ^	m_arrayResidues;
	List<IAtom ^> ^		m_arrayAtoms;
	List<IAtom ^> ^		m_arrayHETAtoms;
};

[Serializable]
public ref class CDotNetModel: public IModel
{
public:
	CDotNetModel(CModelInst * pModel , CDotNetPDB ^ pdb);
	virtual void Init();

	property long	Num { virtual long get() { return m_pModelInst->GetModel()->m_iModel; } }
	property IPDB ^ ParentPDB { virtual IPDB ^ get() {return safe_cast<IPDB ^>(m_pPDB);} } 
	property List <IChain ^> ^ Chains { virtual List <IChain ^> ^ get() { return m_arrayListChains; } }

	virtual IChain ^ GetChain(String ^ chainID);

	virtual void	SetSelect(bool select, bool bNeedUpdate);
	property	bool		Select { virtual bool get(); virtual void set(bool bValue); }

private:
	CModelInst * m_pModelInst;

	List<IChain ^> ^ m_arrayListChains;
	CDotNetPDB ^ m_pPDB;
};

[Serializable]
public ref class CDotNetPDB: public IPDB
{
public:
	CDotNetPDB(CPDBInst * pPDBInst, CDotNetProteinInsight ^ proteinVista );
	virtual void Init();

	property String ^ ID	{ virtual String ^ get() { return gcnew String(m_pPDBInst->GetPDB()->m_strPDBID); } }
	property String ^ Filename { virtual String ^ get() { return gcnew String(m_pPDBInst->GetPDB()->m_strFilename); } }

	property List <IChain ^> ^ Chains { virtual List <IChain ^> ^ get() { return m_arrayListChains; } }
	property List <IModel ^> ^ Models { virtual List <IModel ^> ^ get() { return m_arrayListModels; } }

	property bool	Active { virtual bool get(); virtual void set (bool); }

	//	
	virtual	 void	ShowBioUnit();
	property bool	IsBioUnit{ virtual bool get(); }
	property bool	IsBioUnitChild{ virtual bool get(); }
	property bool	IsBioUnitParent{ virtual bool get(); }
	property bool	AttatchBioUnit{ virtual bool get(); virtual void set (bool); }

	virtual	void	MoveCenter();
	virtual void	RotationAxis(Vector3 axis, float angle);
	virtual void	RotationX(float angle);
	virtual void	RotationY(float angle);
	virtual void	RotationZ(float angle);
	virtual	void	Move(float x, float y, float z);
	property	Microsoft::DirectX::Matrix TransformLocal {	virtual Microsoft::DirectX::Matrix get(); virtual void set(Microsoft::DirectX::Matrix _mat);	}

	//    
	virtual List<IChain ^> ^ GetChains(String ^ chainID);
	virtual IModel ^	GetModel(int modelNum);

	virtual void	SetSelect(bool select, bool bNeedUpdate);
	property	bool		Select { virtual bool get(); virtual void set(bool bValue); }

	CPDBInst * GetPDB() { return m_pPDBInst; }

	bool		m_bUsed;				//	사용된것 나타내는 flag. 내부적으로 사용.
private:

	CPDBInst *		m_pPDBInst;
	CDotNetProteinInsight ^ m_pDotNetProteinVista;

	List<IModel ^> ^ m_arrayListModels;
	List<IChain ^> ^ m_arrayListChains;
};

//
[Serializable]
public ref class CDotNetClipping: public IClipping
{
public:
	CDotNetClipping( CSelectionDisplay * pSelectionDisplay , int clipIndex );
	virtual void Init();

	property bool	Enable { virtual bool get(); virtual void set(bool enable); }
	property bool	Show { virtual bool get(); virtual void set(bool show); }
	property System::Drawing::Color	Color { virtual System::Drawing::Color get();  virtual void set(System::Drawing::Color color); }
	property int	Transparency { virtual int get(); virtual void set(int transparency); }
	property bool	Direction { virtual bool get(); virtual void set(bool direction); }
	property String ^ Equation { virtual String ^ get(); virtual void set(String ^ equ); }

private:
	CSelectionDisplay * m_pSelectionDisplay;
	int					m_clipIndex;
};

public ref class CDotNetAnnotation: public IAnnotation
{
public:
	CDotNetAnnotation( CSelectionDisplay * pSelectionDisplay , int index ){m_pSelectionDisplay = pSelectionDisplay; m_iAnnotation = index; }

	property	bool		Show { virtual bool get();	virtual void set(bool _value); }
	property	String ^	Text { virtual String ^ get(); virtual void set(String ^ _value); }
	property	String  ^		FontName { virtual String ^ get(); virtual void set(String ^ _value); }
	property	int				FontHeight { virtual int get(); virtual void set(int _value); }
	property	IAnnotation::IDisplayMethod		DisplayMethod { virtual IAnnotation::IDisplayMethod get(); virtual void set(IAnnotation::IDisplayMethod _value); }
	property	IAnnotation::ITextType	TextType { virtual IAnnotation::ITextType get(); virtual void set(IAnnotation::ITextType _value); }
	property	IAnnotation::IColorScheme ColorScheme { virtual IAnnotation::IColorScheme get(); virtual void set(IAnnotation::IColorScheme _value); }
	property	System::Drawing::Color		Color { virtual System::Drawing::Color get();  virtual void set(System::Drawing::Color _value); }
	property	int			RelativeXPos { virtual int get(); virtual void set(int _value); }
	property	int			RelativeYPos { virtual int get(); virtual void set(int _value); }
	property	int			RelativeZPos { virtual int get(); virtual void set(int _value); }
	property	int			TextXPos { virtual int get(); virtual void set(int _value); }
	property	int			TextYPos { virtual int get(); virtual void set(int _value); }
	property	int			DepthBias{ virtual int get(); virtual void set(int _value); }
	property	int			Transparency { virtual int get(); virtual void set(int _value); }

private:
	CSelectionDisplay * m_pSelectionDisplay;
	int					m_iAnnotation;		//	VP, ATOM, RESIDUE
};

//
[Serializable]
public ref class CDotNetSelection : public IVP
{
public:
	CDotNetSelection(CSelectionDisplay * pSelectionDisplay, CDotNetProteinInsight ^ proteinVista);
	virtual void Init();

	property	IProperty ^ Property { virtual IProperty	^ get() { return m_property; } }
	property	IPropertyWireframe	^ PropertyWireframe { virtual IPropertyWireframe ^ get() { return m_propertyWireframe; } }
	property	IPropertyStick		^ PropertyStick { virtual IPropertyStick ^ get() { return m_propertyStick; } }
	property	IPropertySpaceFill	^ PropertySpaceFill { virtual IPropertySpaceFill ^ get() { return m_propertySpaceFill; } }
	property	IPropertyBallnStick	^ PropertyBallnStick { virtual IPropertyBallnStick ^ get() { return m_propertyBallnStick; } }
	property	IPropertyRibbon		^ PropertyRibbon { virtual IPropertyRibbon ^ get() { return m_propertyRibbon; } }
	property	IPropertySurface	^ PropertySurface { virtual IPropertySurface ^ get() { return m_propertySurface; } }

	property	IProteinInsight::IDisplayStyle	DisplayStyle { virtual IProteinInsight::IDisplayStyle  get(); virtual void set(IProteinInsight::IDisplayStyle  ); }
	property	String ^ PDBID { virtual String ^ get(); }
	property	String ^ Filename { virtual String ^ get(); }
	property	bool Show { virtual bool get(); virtual void set(bool show); }
	property	String ^ Name { virtual String ^ get(); virtual void set(String ^ name); }

	property	bool Select { virtual bool get(); virtual void set(bool select); }

	virtual	void MoveCenter();
	virtual void RotationAxis(Vector3 axis, float angle);
	virtual void RotationX(float angle);
	virtual void RotationY(float angle);
	virtual void RotationZ(float angle);
	virtual	void Move(float x, float y, float z);
	virtual void ViewAll(float time);
	property	Microsoft::DirectX::Matrix TransformLocal { virtual Microsoft::DirectX::Matrix get(); virtual void set( Microsoft::DirectX::Matrix ); }

	//
	CSelectionDisplay * m_pSelectionDisplay;

	property IPDB ^ PDB { virtual IPDB ^ get(); }
	property List<IAtom ^ > ^ Atoms { virtual List<IAtom ^> ^ get(){ return m_arrayAtoms; } }

	//    
	//    selection update 시에, 기존값을 유지하기 위해서 내부적으로 사용.
	//    
	bool		m_bSelectionUsed;

private:
	CDotNetProperty	^			m_property;
	CDotNetPropertyWireframe ^	m_propertyWireframe;
	CDotNetPropertyStick ^		m_propertyStick;
	CDotNetPropertySpaceFill ^	m_propertySpaceFill;
	CDotNetPropertyBallnStick ^	m_propertyBallnStick;
	CDotNetPropertyRibbon ^		m_propertyRibbon;
	CDotNetPropertySurface ^	m_propertySurface;

	List <IAtom ^> ^	m_arrayAtoms;

	CDotNetProteinInsight ^		m_pDotNetProteinVista;
};

//
[Serializable]
public ref class CDotNetLight: public ILight
{
public:
	CDotNetLight(int indexLight) { m_indexLight = indexLight; }
	virtual void Init() {} 

	property	bool  Enable { virtual bool get(); virtual void set(bool light); }
	property	bool  Show { virtual bool get(); virtual void set(bool light); }
	property	System::Drawing::Color Color { virtual System::Drawing::Color get(); virtual void set(System::Drawing::Color color); }
	property	int	  Intensity { virtual int get(); virtual void set(int intensity); }
	property	Vector3 Position {  virtual Vector3 get(); virtual void set(Vector3 pos ); }

private:
	int			m_indexLight;
};

//
[Serializable]
public ref class CDotNetUtility: public IUtility
{
public:
	CDotNetUtility();
	virtual void Init(){}

	virtual System::Drawing::Color	GetGradientColor (int iStep, int nTotalStep);
	virtual System::Drawing::Color	GetGradientColor (System::Drawing::Color color1, System::Drawing::Color	color2, int iStep, int nTotalStep);

	virtual void OutputMsg(String ^ msg);
	virtual void OutputMsgInStatusBar(String ^ msg);
	virtual void SetProgressInStatusBar(int progress);
	virtual Microsoft::DirectX::Direct3D::Device ^ GetDirect3DDevice9();
};

[Serializable]
public ref class CDotNetMovie: public IMovie
{
public:
	CDotNetMovie();
	virtual void Init(){}
	virtual void BeginMovie(String ^ filename, int width, int height, int fps );
	virtual void Caption(String ^ strCaption, int frame, IMovie::ICaptionPosition pos, String ^ fontFamily, int fontHeight );
	virtual void Capture(int frame);
	virtual void EndMovie();
	virtual void CancelMovie();
	virtual void CaptureCameraAnimation(IAtom ^ atom, int frame);
	virtual void CaptureCameraAnimation(IResidue ^ residue, int frame);
	virtual void CaptureCameraAnimation(Vector3 pos , int frame);
	virtual void CaptureCameraAnimation(IVP ^ vp, int frame);

private:
	List < String ^ >	m_listImageFilename;
	List < int >		m_listImageFrame;		//	filename이 지속되는 frame 수

	String ^	m_movieFilename;
	int			m_width;
	int			m_height;
	int			m_fps;
};

//
[Serializable]
public ref class CDotNetProteinInsight : public IProteinInsight
{
public:
	CDotNetProteinInsight();
	!CDotNetProteinInsight();
	~CDotNetProteinInsight();

	void	WorkspaceInit();
	void	WorkspaceDestroy();

	virtual void Init();
	
	property List <IPDB ^> ^ PDBs { virtual List<IPDB ^> ^ get() { return m_arrayPDB; } }

	virtual void UpdateSelections();
	property List <IVP ^> ^ VPs { virtual List<IVP ^> ^ get(); }


	//
	virtual bool Open(String ^filename);		//	filname, pdb, ent, piw, pdbid 4 개 가능
	virtual bool AddPDB(String ^filename);
	virtual bool ClosePDB (IPDB ^ pdb);
	virtual bool CloseWorkspace();
	virtual bool SaveWorkspace(String ^ filename);

	property String ^ ScriptDir { virtual String ^ get(); }
	property String ^ ScriptPath { virtual String ^ get(); }			//	dir + filename + .cs
	property String ^ PlugInDir { virtual String ^ get(); }	
	property String ^ PlugInPath { virtual String ^ get(); }				//	dir + filename + .dll
	property String ^ ProteinInsightDir { virtual String ^ get(); }	
	property String ^ PDBDir { virtual String ^ get(); }	
	property String ^ WorkspaceDir { virtual String ^ get(); }	
	property String ^ MovieDir { virtual String ^ get(); }	

	virtual void SetSelect(bool select, bool bNeedUpdate);
	virtual void SetSelect ( IProteinInsight::ISelectType type );
	virtual void UpdateSelect();

	virtual IVP ^ AddVP(IProteinInsight::IDisplayStyle displayStyle);

	virtual IVP ^ AddVP(IPDB ^ pdb, IProteinInsight::IDisplayStyle displayStyle);
	virtual IVP ^ AddVP(List<IPDB ^> ^ pdbs, IProteinInsight::IDisplayStyle displayStyle);

	virtual IVP ^ AddVP(IModel ^ model, IProteinInsight::IDisplayStyle displayStyle);
	virtual IVP ^ AddVP(List<IModel ^> ^  models, IProteinInsight::IDisplayStyle displayStyle);

	virtual IVP ^ AddVP(IChain ^ chain, IProteinInsight::IDisplayStyle displayStyle);
	virtual IVP ^ AddVP(List<IChain ^> ^ chains, IProteinInsight::IDisplayStyle displayStyle);

	virtual IVP ^ AddVP(IResidue ^ residue, IProteinInsight::IDisplayStyle displayStyle);
	virtual IVP ^ AddVP(List<IResidue ^> ^ residues, IProteinInsight::IDisplayStyle displayStyle);

	virtual IVP ^ AddVP(IAtom ^ atom, IProteinInsight::IDisplayStyle displayStyle);
	virtual IVP ^ AddVP(List<IAtom ^> ^ atoms, IProteinInsight::IDisplayStyle displayStyle);

	virtual void VPSubtrctVP(IVP ^, IVP^);
	virtual void VPUnionVP(IVP ^, IVP^);
	virtual void VPIntersectVP(IVP ^, IVP^);

	virtual void DeleteVP(IVP ^ selection);

	virtual void Idle(double millisecond);

	virtual void SaveImage(String ^ filename, int width, int height, IProteinInsight::IImageFormat format);

	property IPropertyScene ^ Property { virtual IPropertyScene ^ get() { return m_pPropertyScene; } }
	property IMovie ^ Movie { virtual IMovie ^ get() { return m_pMovie; } }
	property IUtility ^ Utility { virtual IUtility ^ get() { return m_pUtilty; } }

	virtual void ViewAll();

private:
	List <IPDB ^> ^		m_arrayPDB;
	List <IVP ^> ^		m_arrayVP;
	CDotNetPropertyScene ^ m_pPropertyScene;
	CDotNetMovie ^ m_pMovie;
	CDotNetUtility ^ m_pUtilty;
};

#endif
