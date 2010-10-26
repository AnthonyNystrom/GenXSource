using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.DirectX;
using NuGenSVisualLib.Recording;
using NuGenSVisualLib.Rendering.Chem.Structures;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Settings;
using Org.OpenScience.CDK.Interfaces;

namespace NuGenSVisualLib.Rendering.Chem
{
    /// <summary>
    /// Encapsulates how measurements are displayed
    /// </summary>
    public struct MeasurementSettings
    {
        public enum Quality
        {
            Standard        = 0,
            AntiAliased     = 1
        }

        public Quality TextQuality;
        public Quality LineQuality;

        public Color AngleClr;
        public Color DistanceClr;

        public bool AngleDashedLines;
        public bool DistanceDashedLines;

        public MeasurementSettings(Quality textQuality, Quality lineQuality,
                                   Color angleClr, Color distanceClr,
                                   bool angleDashedLines, bool distanceDashedLines)
        {
            TextQuality = textQuality;
            LineQuality = lineQuality;
            AngleClr = angleClr;
            DistanceClr = distanceClr;
            AngleDashedLines = angleDashedLines;
            DistanceDashedLines = distanceDashedLines;
        }
    }

    public delegate void RenderUpdateDelegate(double time);
    public delegate void SMILESUpdateDelegate(string rawSMILES);

    public enum ControlMode
    {
        ViewMovement,
        Selection,
        SelectionMovement,
        SelectionRotation
    }

    public interface IChemControl
    {
        ControlMode ControlMode { get; set; }

        void Init(HashTableSettings settings, ICommonDeviceInterface cdi);
        Color BackColor { get; set; }
        string Title { get; }
        void ApplySettings(CompleteOutputDescription outputDesc);

        CompleteOutputDescription OutputDescription { get; }

        void OpenEditShadingDialog();
        void OpenRecordingLoadingDialog();

        // data loading
        void LoadFile(string file);
        void LoadSMILES(string smiles, bool isSMARTS);
        void InsertSMILES(string smiles, bool isSMARTS);

        // recording
        void StartRecording(RecordingSettings settings);
        void StopRecording();
        void OpenRecording(string filename);

        int NumAtoms { get; }
        int NumBonds { get; }

        HashTableSettings Settings { get; }

        // selection
        ChemEntity TrySelectAtPoint(int x, int y);
        void SelectMode(bool toggleOn);
        void SelectAll();
        ChemEntity[] GetAllNearby(ChemEntity entity, float radius);
        ChemEntity[] GetAllNearby(Vector3 position, float radius);
        ChemEntity GetNearestTo(ChemEntity entity);
        ChemEntity GetNearestTo(Vector3 position);

        // measurements
        void MeasureDistance();
        void MeasureAngle();
        void MeasureDihedral();
        void ClearMesurements();
        void Setup(MeasurementSettings settings);

        // outline
        IChemObject GetRootNode();
        ChemEntity[] GetSelection();

        // display
        void TakeScreenshot(string file, ImageFormat format);
        Bitmap TakeScreenshotToBitmap();
        bool LowDetailMovement { get; set; }
        double LastRenderTime { get; }
        RenderUpdateDelegate OnRenderUpdate { get; set; }
        bool ShowMarkup { get; set; }
        bool ShowLayers { get; set; }

        // structure queries
        IChemObject[] QueryConnected(IChemObject obj);
        ChemEntity[] QueryConnectedEntities(ChemEntity entity);
        string MoleculeSMILESRawString { get; }
        SMILESUpdateDelegate OnSMILESUpdate { get; set; }

        // creation
        bool AutoBonding { get; set; }
        // TODO: Probably needs some sort of location suggestion feedback?
        AtomEntity NewAtom(string chemSymbol, Vector3 position);
        AtomEntity NewAtom(IAtom newAtom);
        void RemoveAtom(AtomEntity atom, bool queryUser);

        // editing
        void ChangeAtomElement(AtomEntity atom, string newChemSymbol);
    }
}