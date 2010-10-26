using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Janus.Windows.ButtonBar;
using NuGenSVisualLib.Rendering.Chem;
using System.IO;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering.Devices;
using NuGenSVisualLib.Rendering;
using System.Threading;
using NuGenSVisualLib.Settings;
using System.Reflection;
using System.Xml;
using Microsoft.DirectX;
using NuGenSVisualLib.Rendering.Lighting;
using NuGenSVisualLib.Rendering.Effects;
using NuGenSVisualLib.Rendering.Chem.Materials;

namespace NuGenSVisualLib
{
    partial class MoleculeSchemeDlg : Form
    {
        #region Inner Structures
        /// <summary>
        /// Encapsulates a rendering scheme
        /// </summary>
        class Scheme
        {
            public Image previewImage;
            public bool available;

            public MoleculeSchemeSettings settings;
            public MoleculeRenderingScheme scheme;
            public RequirementsCompatibility compatibility;
            public MoleculeSchemeSUI sUI;
        }

        /// <summary>
        /// Encapsulates a lighting preset
        /// </summary>
        class LightingPreset
        {
            public Image previewImage;
            public bool available;

            public LightingSetup setup;
        }

        class Effect
        {
            public Image previewImage;
            public bool available;

            public RenderingEffectSettings settings;
            public RenderingEffect effect;
        }

        class ElementShadingModule
        {
            public Image previewImage;
            public MoleculeMaterialsModule materialModule;
        }
        #endregion

        Image previewBorder;
        Image previewNotAvailableAvailable, previewNotAvailableUnavailable;
        Image unavailableIcon;

        Scheme[] availableSchemes;
        Scheme currentScheme;

        Effect[] allEffects;
        Effect[] availableEffects;
        List<Effect> currentEffects;
        Effect[] requiredEffects;
        Effect currentEffect;

        LightingPreset[] availablePresets;
        LightingPreset currentPreset;
        NuGenSVisualLib.Rendering.Lighting.Light currentLight;

        ElementShadingModule[] allModuleTemplates;
        ElementShadingModule currentShadingModule;

        Device refDevice;
        OutputCaps outCaps;
        CompleteOutputDescription refcoDesc, coDesc;

        HashTableSettings settings;

        bool previewReady = false;

        Thread updateThread;
        bool wantSchemeUpdate;
        bool wantEffectUpdate;
        bool wantAtomUpdate, wantBondUpdate;

        EffectDetailsDlg efxDetailsDlg;

        string thislock = "";

        public MoleculeSchemeDlg()
        {
            InitializeComponent();
        }

        public MoleculeSchemeDlg(HashTableSettings settings, OutputCaps outCaps, Device device, CompleteOutputDescription coDesc)
        {
            InitializeComponent();

            currentEffects = new List<Effect>();

            this.settings = settings;

            LoadLocalResources();

            this.refDevice = device;
            this.outCaps = outCaps;
            this.refcoDesc = coDesc;
            this.coDesc = new CompleteOutputDescription(coDesc);

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenSVisualLib.LightingPresets.config"))
            {
                LoadLightingPresets(stream);
            }
            
            SetupLightingPreview();
            SetupEffectPreview();

            schemePreviewControl.OnNewPreview += new EventHandler(schemePreviewControl_OnNewPreview);
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenSVisualLib.MoleculeSchemes.config"))
            {
                if (stream != null)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(stream);

                    LoadSchemes(xml);
                    LoadEffects(xml);
                    LoadMaterials(xml);
                }
            }

            previewReady = true;

            updateThread = new Thread(this.UpdatePreviewsProcess);
            updateThread.Start();
        }

        public CompleteOutputDescription FinalCoDesc
        {
            get { return coDesc; }
        }

        private void UpdatePreviewsProcess()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        // Wait for any other changes
                        Thread.Sleep(1000);
                        bool updateScheme = false;
                        bool updateEffect = false;
                        lock (thislock)
                        {
                            updateScheme = wantSchemeUpdate;
                            wantSchemeUpdate = false;
                            // generate scheme changes
                            if (schemePreviewControl.CODesc != null)
                                schemePreviewControl.CODesc.CompareAgainst(coDesc, true);
                            else
                                updateScheme = false;

                            updateEffect = wantEffectUpdate;
                        }
                        lock (updateThread)
                        {
                            if (updateScheme)
                                schemePreviewControl.UpdateScheme(wantAtomUpdate, wantBondUpdate, false);
                            if (updateEffect)
                                effectPreviewControl1.UpdateView();
                        }
                        if (!updateScheme)
                            Thread.Sleep(Timeout.Infinite);
                    }
                    catch (ThreadInterruptedException) { }
                }
            }
            catch (ThreadAbortException) { }
        }

        #region Loading Code

        private void LoadSchemes(XmlDocument xml)
        {
            XmlNodeList schemeNodes = xml.SelectNodes("/configuration/moleculeSchemeSettings/scheme");
            if (schemeNodes != null && schemeNodes.Count > 0)
            {
                List<MoleculeSchemeSettings> schemes = new List<MoleculeSchemeSettings>();
                for (int scheme = 0; scheme < schemeNodes.Count; scheme++)
                {
                    Type type = Assembly.GetExecutingAssembly().GetType(schemeNodes[scheme].InnerText, false);
                    if (type != null)
                        schemes.Add((MoleculeSchemeSettings)Activator.CreateInstance(type));
                }
                AddSchemes(schemes.ToArray());
            }
        }

        private void LoadEffects(XmlDocument xml)
        {
            XmlNodeList effectNodes = xml.SelectNodes("/configuration/moleculeEffectSettings/effect");
            if (effectNodes != null && effectNodes.Count > 0)
            {
                List<RenderingEffectSettings> abEffects = new List<RenderingEffectSettings>();
                List<RenderingEffectSettings> uabEffects = new List<RenderingEffectSettings>();
                for (int effect = 0; effect < effectNodes.Count; effect++)
                {
                    Type type = Assembly.GetExecutingAssembly().GetType(effectNodes[effect].InnerText, false);
                    if (type != null)
                    {
                        if (((XmlElement)effectNodes[effect]).GetAttribute("available") == "false")
                            uabEffects.Add((RenderingEffectSettings)Activator.CreateInstance(type));
                        else
                            abEffects.Add((RenderingEffectSettings)Activator.CreateInstance(type));
                    }
                }
                AddEffects(abEffects.ToArray(), uabEffects.ToArray());
            }
        }

        private void LoadLightingPresets(Stream presetsStream)
        {
            if (presetsStream != null)
            {
                string base_path = (string)settings["Base.Path"];

                LightingSetup[] setups = LightingSetup.FromXml(presetsStream);
                availablePresets = new LightingPreset[setups.Length];

                for (int i = 0; i < setups.Length; i++)
                {
                    availablePresets[i] = new LightingPreset();
                    availablePresets[i].setup = setups[i];
                    availablePresets[i].available = true;

                    string previewImgPath = base_path + @"Media\UI\previews\lp-" + setups[i].name + ".jpg";
                    if (File.Exists(previewImgPath))
                        availablePresets[i].previewImage = WriteNameText(ApplyBorder(Image.FromFile(previewImgPath), true), availablePresets[i].setup.name, false);
                    else
                        availablePresets[i].previewImage = WriteNameText(previewNotAvailableAvailable, availablePresets[i].setup.name, true);

                    // add to list
                    ButtonBarItem button = new ButtonBarItem();
                    button.Image = availablePresets[i].previewImage;
                    button.ToolTipText = availablePresets[i].setup.name;
                    button.Tag = availablePresets[i];

                    uiLightingList.Groups[0].Items.Add(button);
                }
            }
        }

        private void SetupLightingPreview()
        {
            OutputSettings outSettings = new OutputSettings(0, true, DeviceType.Hardware, Format.Unknown, MultiSampleType.None, DepthFormat.D16, CreateFlags.HardwareVertexProcessing);
            lightPreviewControl2.OutSettings = outSettings;
            lightPreviewControl2.OnNewPreview += new EventHandler(lightPreviewControl2_OnNewPreview);

            /*lightingSetup = lightPreviewControl2.Lighting = new LightingSetup();
            DirectionalLight light = new DirectionalLight();
            light.Clr = Color.Red;
            light.Direction = new Vector3(-1, 0, 0);
            light.Direction.Normalize();
            light.Enabled = true;
            lightingSetup.lights.Add(light);

            directionalLightControl1.SetData(light);

            lightingSchemeLabel.Text = lightingSetup.name;*/
        }

        private void LoadLocalResources()
        {
            string base_path = (string)settings["Base.Path"];
            previewBorder = Bitmap.FromFile(base_path + @"Media\UI\PreviewBorder.png");
            unavailableIcon = Bitmap.FromFile(base_path + @"Media\UI\UnavailableIcon.png");
            Image noPreview = Bitmap.FromFile(base_path + @"Media\UI\SchemePreviewDefault.png");
            previewNotAvailableAvailable = ApplyBorder(new Bitmap(noPreview), true);
            previewNotAvailableUnavailable = ApplyBorder(noPreview, false);
        }

        #endregion

        #region Scheme Manipulation

        private void AddSchemes(MoleculeSchemeSettings[] schemes)
        {
            string base_path = (string)settings["Base.Path"];

            availableSchemes = new Scheme[schemes.Length];
            for (int scheme=0; scheme < schemes.Length; scheme++)
            {
                // create scheme instance
                availableSchemes[scheme] = new Scheme();
                availableSchemes[scheme].settings = schemes[scheme];
                availableSchemes[scheme].scheme = schemes[scheme].GetScheme(refDevice);
                availableSchemes[scheme].sUI = schemes[scheme].GetSUI();
                if (availableSchemes[scheme].sUI != null)
                    availableSchemes[scheme].sUI.SetChangeEvent(thislock, new EventHandler<SchemeSUIChangeHandler>(OnSettingsChanged));

                // compare requirements to adapter capabilities
                availableSchemes[scheme].compatibility = outCaps.CheckCapabilities(availableSchemes[scheme].scheme.DeviceRequirements);
                bool available = availableSchemes[scheme].available = (availableSchemes[scheme].compatibility.NumFails == 0);

                // look for preview image
                string previewImgPath = base_path + @"Media\UI\previews\schemes\" + availableSchemes[scheme].scheme.Name + ".jpg";
                if (File.Exists(previewImgPath))
                    availableSchemes[scheme].previewImage = WriteNameText(ApplyBorder(Image.FromFile(previewImgPath), available), availableSchemes[scheme].scheme.Name, false);
                else if (available)
                    availableSchemes[scheme].previewImage = WriteNameText(previewNotAvailableAvailable, availableSchemes[scheme].scheme.Name, true);
                else
                    availableSchemes[scheme].previewImage = WriteNameText(previewNotAvailableUnavailable, availableSchemes[scheme].scheme.Name, true);

                // add to list
                ButtonBarItem button = new ButtonBarItem();
                button.Image = availableSchemes[scheme].previewImage;
                button.ToolTipText = availableSchemes[scheme].scheme.Name;

                uiSchemesList.Groups[0].Items.Add(button);
            }
        }

        private void TryChangeScheme(Scheme scheme)
        {
            // fill in requirements
            FillRequirements(scheme);

            label13.Text = scheme.scheme.Name;

            if (scheme.available)
            {
                DoRemoveScheme();
                DoChangeScheme(scheme);
            }
            else
            {
                // clear settings
                schemePreviewControl.Enabled = false;
            }
        }

        private void DoRemoveScheme()
        {
            if (currentScheme != null)
            {
                // remove SUI control
                if (currentScheme.sUI != null)
                {
                    panel2.Controls.Remove(((Control)currentScheme.sUI).Parent);
                    ((Control)currentScheme.sUI).Parent.Dispose();
                    ((Control)currentScheme.sUI).Dispose();
                }
                currentScheme = null;

                pictureBox4.SizeMode = pictureBox5.SizeMode = PictureBoxSizeMode.CenterImage;
                pictureBox4.Image = pictureBox5.Image = Properties.Resources.redcross_32;
            }
        }

        private void DoChangeScheme(Scheme scheme)
        {
            // setup settings
            lock (thislock)
            {
                currentScheme = null;

                //coDesc.AtomShadingDesc.MoleculeMaterials = refcoDesc.AtomShadingDesc.MoleculeMaterials;
                //coDesc.BondShadingDesc.MoleculeMaterials = refcoDesc.BondShadingDesc.MoleculeMaterials;

                // set common values
                if (scheme.scheme.HandlesAtoms)
                {
                    uiAtomsGSetGroup.Enabled = true;

                    uiAtomLODControl.SetupValues(scheme.settings.AtomLODRange, scheme.settings.AtomLOD);
                    if (!(uiAtomDDraw.Enabled = scheme.scheme.HandlesBonds))
                        coDesc.AtomShadingDesc.Draw = false;
                    uiAtomDDraw.Checked = coDesc.AtomShadingDesc.Draw;
                    uiAtomFillModeList.SelectedIndex = (int)coDesc.AtomShadingDesc.FillMode;
                    uiAtomSymbolsDraw.Checked = coDesc.AtomShadingDesc.SymbolText;
                    uiAtomSymbolsBlend.Checked = coDesc.AtomShadingDesc.BlendSymbolText;
                }
                else
                {
                    uiAtomsGSetGroup.Enabled = false;
                }

                if (scheme.scheme.HandlesBonds)
                {
                    uiBondsGSetGroup.Enabled = true;

                    uiBondSpacingList.SelectedIndex = (int)coDesc.BondShadingDesc.Spacing;
                    uiBondEndTypeList.SelectedIndex = (int)coDesc.BondShadingDesc.EndType;
                    uiBondLODControl.SetupValues(scheme.settings.BondLODRange, scheme.settings.BondLOD);
                    if (!(uiBondDDraw.Enabled = scheme.scheme.HandlesAtoms))
                        coDesc.BondShadingDesc.Draw = false;
                    uiBondDDraw.Checked = coDesc.BondShadingDesc.Draw;
                }
                else
                {
                    uiBondsGSetGroup.Enabled = false;
                }

                // add control
                if (scheme.sUI != null/* || ((scheme.sUI = scheme.settings.GetSUI()) != null)*/)
                {
                    //scheme.sUI.SetChangeEvent(new EventHandler(OnSettingsChanged));
                    scheme.sUI.UpdateValues();
                    Control control = (Control)scheme.sUI;
                    GroupBox gBox = new GroupBox();
                    gBox.MaximumSize = gBox.MinimumSize = new Size(panel2.Width, control.Height + 20);
                    control.Dock = DockStyle.Fill;
                    gBox.Text = scheme.scheme.Name;
                    gBox.Controls.Add(control);
                    gBox.Dock = DockStyle.Top;
                    panel2.Controls.Add(gBox);
                }

                // TODO: need to get these from elsewhere!
                OutputSettings outSettings = new OutputSettings(outCaps.Adapter, true, DeviceType.Hardware, Format.Unknown, outCaps.AntiAliasing.MaxSupported, DepthFormat.D16, CreateFlags.HardwareVertexProcessing);

                schemePreviewControl.Enabled = true;
                schemePreviewControl.OutSettings = outSettings;
                coDesc.SchemeSettings = scheme.settings;
                schemePreviewControl.SetScheme(scheme.scheme, coDesc);

                currentScheme = scheme;

                // update effects tab
                SetRequiredEffects(scheme.settings.GetRequiredEffects());
            }
        }

        private void FillRequirements(Scheme scheme)
        {
            if (scheme == null)
            {
                requirementGague1.Enabled = false;
                requirementGague2.Enabled = false;
            }
            else
            {
                RequirementsCompatibility rComp = scheme.compatibility;
                OutputRequirements oReqs = scheme.scheme.DeviceRequirements;

                if (oReqs.VertexShader != null)
                {
                    requirementGague1.MinReqValue = oReqs.VertexShader.Major;
                    requirementGague1.MaxReqValue = oReqs.VertexShader.Major;
                    requirementGague1.ActualValue = outCaps.VertexShaderVersion.Major;
                    requirementGague1.Enabled = true;
                    requirementGague1.UpdateLayout();
                }
                else
                    requirementGague1.Enabled = false;

                if (oReqs.PixelShader != null)
                {
                    requirementGague2.MaxReqValue = oReqs.PixelShader.Major;
                    requirementGague2.MaxReqValue = oReqs.PixelShader.Major;
                    requirementGague2.ActualValue = outCaps.FragmentShaderVersion.Major;
                    requirementGague2.Enabled = true;
                    requirementGague2.UpdateLayout();
                }
                else
                    requirementGague2.Enabled = false;
            }
        }

        private void RequestSchemeUpdate(bool updateAtoms, bool updateBonds)
        {
            // inform local thread
            lock (thislock)
            {
                wantSchemeUpdate = true;
                if (updateAtoms)
                    wantAtomUpdate = true;
                if (updateBonds)
                    wantBondUpdate = true;
            }
            if (Monitor.TryEnter(updateThread, 50))
            {
                if (updateThread.ThreadState != ThreadState.Running)
                    updateThread.Interrupt();
                Monitor.Exit(updateThread);
            }
        }

        #endregion

        #region Effect Manipulation

        private void AddEffects(RenderingEffectSettings[] abEffects, RenderingEffectSettings[] uabEffects)
        {
            string base_path = (string)settings["Base.Path"];

            availableEffects = new Effect[abEffects.Length];
            allEffects = new Effect[abEffects.Length + uabEffects.Length];
            for (int effect = 0; effect < abEffects.Length; effect++)
            {
                // create scheme instance
                allEffects[effect] = availableEffects[effect] = new Effect();
                availableEffects[effect].settings = abEffects[effect];
                availableEffects[effect].effect = abEffects[effect].GetEffect(refDevice);
                //availableEffects[effect].sUI = effects[effect].GetSUI();
                //if (availableEffects[effect].sUI != null)
                //    availableEffects[effect].sUI.SetChangeEvent(new EventHandler(OnSettingsChanged));

                // compare requirements to adapter capabilities
                //availableEffects[effect].compatibility = outCaps.CheckCapabilities(availableSchemes[effect].scheme.DeviceRequirements);
                bool available = availableSchemes[effect].available = true;// (availableSchemes[effect].compatibility.NumFails == 0);
                // NOTE: ^ availablity is not per effect but context sensitive also
                
                // look for preview image
                string previewImgPath = base_path + @"Media\UI\previews\effects\" + availableEffects[effect].effect.Name + ".jpg";
                if (File.Exists(previewImgPath))
                    availableEffects[effect].previewImage = WriteNameText(ApplyBorder(Image.FromFile(previewImgPath), available), availableEffects[effect].effect.Name, false);
                else if (available)
                    availableEffects[effect].previewImage = WriteNameText(previewNotAvailableAvailable, availableEffects[effect].effect.Name, true);
                else
                    availableEffects[effect].previewImage = WriteNameText(previewNotAvailableUnavailable, availableEffects[effect].effect.Name, true);

                // add to list
                ButtonBarItem button = new ButtonBarItem();
                button.Image = availableEffects[effect].previewImage;
                button.ToolTipText = availableEffects[effect].effect.Name;
                button.Tag = availableEffects[effect];

                uiEffectsList.Groups[0].Items.Add(button);
            }

            for (int effect = 0; effect < uabEffects.Length; effect++)
            {
                // create scheme instance
                allEffects[effect + abEffects.Length] = new Effect();
                allEffects[effect].settings = uabEffects[effect];
                allEffects[effect].effect = uabEffects[effect].GetEffect(refDevice);

                // compare requirements to adapter capabilities
                bool available = allEffects[effect].available = true;
                // NOTE: ^ availablity is not per effect but context sensitive also

                // look for preview image
                string previewImgPath = base_path + @"Media\UI\previews\effects\" + allEffects[effect].effect.Name + ".jpg";
                if (File.Exists(previewImgPath))
                    allEffects[effect].previewImage = WriteNameText(ApplyBorder(Image.FromFile(previewImgPath), available), allEffects[effect].effect.Name, false);
                else if (available)
                    allEffects[effect].previewImage = WriteNameText(previewNotAvailableAvailable, allEffects[effect].effect.Name, true);
                else
                    allEffects[effect].previewImage = WriteNameText(previewNotAvailableUnavailable, allEffects[effect].effect.Name, true);
            }
        }

        private bool AddEffectToCurrent(Effect effect)
        {
            if (requiredEffects.Length > 0)
                return false;
            // check all current for clashes
            /*foreach (Effect efx in currentEffects)
            {
                if (efx.effect.EfxType == effect.effect.EfxType)
                {
                    return false;
                }
            }*/
            currentEffects.Clear();

            // check all required effects for compatibility

            uiCurrentEffectsList.Groups[0].Items.Clear();

            ButtonBarItem button = new ButtonBarItem();
            button.Image = effect.previewImage;
            button.ToolTipText = effect.effect.Name;
            button.Tag = effect;
            uiCurrentEffectsList.Groups[0].Items.Add(button);

            currentEffects.Add(effect);

            // update preview
            effectPreviewControl1.SetEffect(effect.settings);

            return true;
        }

        private void SetRequiredEffects(RenderingEffectSettings[] effects)
        {
            if (effects != null)
            {
                // find effects locally
                requiredEffects = new Effect[effects.Length];
                for (int i = 0; i < effects.Length; i++)
                {
                    requiredEffects[i] = new Effect();
                    requiredEffects[i].settings = effects[i];
                    foreach (Effect effect in allEffects)
                    {
                        if (effect.settings.GetType() == effects[i].GetType())
                        {
                            // copy over
                            requiredEffects[i].previewImage = effect.previewImage;
                            requiredEffects[i].effect = effect.effect;
                            break;
                        }
                    }
                }

                // add to control list
                uiEffectsReqList.Groups[0].Items.Clear();
                foreach (Effect effect in requiredEffects)
                {
                    ButtonBarItem item = new ButtonBarItem();
                    item.Image = effect.previewImage;
                    item.ToolTipText = effect.effect.Name;
                    item.Tag = effect;
                    uiEffectsReqList.Groups[0].Items.Add(item);
                }

                // check through current effects for conflicts to remove
                if (currentEffects != null)
                {
                    foreach (Effect effect in currentEffects)
                    {
                    }
                }
            }
        }

        private void SetupEffectPreview()
        {
            OutputSettings outSettings = new OutputSettings(outCaps.Adapter, true, DeviceType.Hardware,
                                                            Format.Unknown, outCaps.AntiAliasing.MaxSupported,
                                                            DepthFormat.D16, CreateFlags.HardwareVertexProcessing);

            effectPreviewControl1.CoDesc = coDesc;
            effectPreviewControl1.Enabled = true;
            effectPreviewControl1.OutSettings = outSettings;

            uiEffectLOD.OnLODValueChanged += new EventHandler(uiEffectLOD_OnLODValueChanged);

            effectPreviewControl1.OnNewPreview += new EventHandler(effectPreviewControl1_OnNewPreview);
        }

        private void DoChangeEffect(Effect effect)
        {
            currentEffect = effect;

            uiEffectPropGroup.Enabled = (effect != null && effect.available);
            if (effect != null)
            {
                // load reqs
                OutputRequirements[] oReqs = effect.effect.GetDeviceRequirements();
                OutputRequirements oReq = effect.effect.GetDeviceRequirements(effect.settings);
                int reqMaxIdx = oReqs.Length - 1;

                uiEfxVSReqs.MinReqValue = oReqs[0].VertexShader.Major;
                uiEfxVSReqs.MaxReqValue = oReqs[reqMaxIdx].VertexShader.Major;
                uiEfxVSReqs.ActualValue = oReq.VertexShader.Major;
                uiEfxVSReqs.UpdateLayout();

                uiEfxPSReqs.MinReqValue = oReqs[0].PixelShader.Major;
                uiEfxPSReqs.MaxReqValue = oReqs[reqMaxIdx].PixelShader.Major;
                uiEfxPSReqs.ActualValue = oReq.PixelShader.Major;
                uiEfxPSReqs.UpdateLayout();

                if (effect.available)
                {
                    // load settings
                    uiEffectPropGroup.Enabled = true;
                    uiEffectLOD.SetupValues(effect.effect.LODRange, effect.effect.LOD);
                }
            }
        }

        private void RequestEffectUpdate()
        {
            // inform local thread
            lock (thislock)
            {
                wantEffectUpdate = true;
            }
            if (Monitor.TryEnter(updateThread, 50))
            {
                if (updateThread.ThreadState != ThreadState.Running)
                    updateThread.Interrupt();
                Monitor.Exit(updateThread);
            }
        }
        #endregion

        #region Image Manipulation

        private Image WriteNameText(Image image, string name, bool cloneImg)
        {
            Image outImg;
            if (cloneImg)
                outImg = new Bitmap(image);
            else
                outImg = image;

            Graphics g = Graphics.FromImage(outImg);
            System.Drawing.Font font = new System.Drawing.Font("Verdana", 8);
            RectangleF rect = new RectangleF(0, 0, (float)outImg.Width, (float)outImg.Height - 5);
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Far;
            g.DrawString(name, font, new SolidBrush(Color.DarkGray), rect, strFormat);

            return outImg;
        }

        private Image ApplyBorder(Image image, bool available)
        {
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(previewBorder, 0, 0, 72, 72);
            if (!available)
                g.DrawImage(unavailableIcon, 19, 19, 35, 35);
            g.Flush();

            return image;
        }

        #endregion

        #region LightingPresets Manipulation

        private void ChangeLight(NuGenSVisualLib.Rendering.Lighting.Light light)
        {
            if (light != null)
            {
                currentLight = light;
                uiLightEnabled.Checked = light.Enabled;
                uiColorButton1.SelectedColor = light.Clr;
                uiLightCastShadows.Checked = light.CastShadows;

                // load correct control
                if (light is DirectionalLight)
                {
                    DirectionalLightControl control = new DirectionalLightControl();
                    control.Dock = DockStyle.Fill;
                    control.SetData((DirectionalLight)light);
                    control.OnValueUpdate += new EventHandler(control_OnValueUpdate);
                    uiLightingPropPanel.Controls.Add(control);

                    uiLightingPropGroup.Enabled = true;
                    currentLight = light;
                }
                else
                {
                    currentLight = null;
                    foreach (Control control in uiLightingPropPanel.Controls)
                    {
                        control.Dispose();
                    }
                    uiLightingPropPanel.Controls.Clear();
                    uiLightingPropGroup.Enabled = false;
                }
            }
            else
            {
                currentLight = null;
                foreach (Control control in uiLightingPropPanel.Controls)
                {
                    control.Dispose();
                }
                uiLightingPropPanel.Controls.Clear();
                uiLightingPropGroup.Enabled = false;
            }
        }

        private void TryChangePreset(LightingPreset preset)
        {
            // remove any current preset
            if (currentPreset != null)
            {
                // remove custom controls
                foreach (Control control in uiLightingPropPanel.Controls)
                {
                    control.Dispose();
                }
                uiLightingPropPanel.Controls.Clear();
                // clear lights
                lightsListBox.Items.Clear();
                lightPreviewControl2.Lighting = null;

                currentPreset = null;
                ChangeLight(null);
            }

            currentPreset = preset;
            lightingSchemeLabel.Text = preset.setup.name;
            lightsListBox.Items.AddRange(preset.setup.lights.ToArray());
            lightPreviewControl2.Lighting = preset.setup;
            uiLightingPropGroup.Enabled = false;
        }

        #endregion

        #region Materials Manipulation

        private void LoadMaterials(XmlDocument xml)
        {
            // load templates
            XmlNodeList moduleNodes = xml.SelectNodes("/configuration/elementsShading/templates/module");
            if (moduleNodes != null && moduleNodes.Count > 0)
            {
                List<MoleculeMaterialsModule> modules = new List<MoleculeMaterialsModule>();
                for (int module = 0; module < moduleNodes.Count; module++)
                {
                    Type type = Assembly.GetExecutingAssembly().GetType(moduleNodes[module].InnerText, false);
                    if (type != null)
                        modules.Add((MoleculeMaterialsModule)Activator.CreateInstance(type));
                }
                AddMaterialTemplates(modules.ToArray());
            }
            // TODO: load presets
        }

        private void AddMaterialTemplates(MoleculeMaterialsModule[] modules)
        {
            string base_path = (string)settings["Base.Path"];

            allModuleTemplates = new ElementShadingModule[modules.Length];
            for (int module = 0; module < modules.Length; module++)
            {
                modules[module].LoadModuleSettings(HashTableSettings.Instance);

                allModuleTemplates[module] = new ElementShadingModule();
                allModuleTemplates[module].materialModule = modules[module];
                
                // look for preview image
                string previewImgPath = base_path + @"Media\UI\previews\materials\" + allModuleTemplates[module].materialModule.Name + ".jpg";
                if (File.Exists(previewImgPath))
                    allModuleTemplates[module].previewImage = WriteNameText(ApplyBorder(Image.FromFile(previewImgPath), true), allModuleTemplates[module].materialModule.Name, false);
                else
                    allModuleTemplates[module].previewImage = WriteNameText(previewNotAvailableAvailable, allModuleTemplates[module].materialModule.Name, true);

                // add to list
                ButtonBarItem button = new ButtonBarItem();
                button.Image = allModuleTemplates[module].previewImage;
                button.ToolTipText = allModuleTemplates[module].materialModule.Name;
                button.Tag = module;

                uiElementShadingList.Groups[1].Items.Add(button);
            }
        }

        private void DoChangeElementModule(ElementShadingModule elementShadingModule)
        {
            currentShadingModule = elementShadingModule;
            uiShadingSeriesList.Items.Clear();
            uiShadingElementList.Items.Clear();

            if (elementShadingModule == null)
            {
                uiShadingSeriesGroup.Enabled = false;
                uiShadingElementsGroup.Enabled = false;
                uiShadingSeriesList.Enabled = false;
                uiShadingElementList.Enabled = false;
                return;
            }
            uiShadingSeriesGroup.Enabled = true;
            uiShadingElementsGroup.Enabled = true;
            uiShadingSeriesList.Enabled = true;
            uiShadingElementList.Enabled = true;

            // fill lists
            foreach (KeyValuePair<string, IMoleculeMaterial> pair in elementShadingModule.materialModule.series)
            {
                uiShadingSeriesList.Items.Add(pair.Key);
            }
            foreach (KeyValuePair<string, MoleculeMaterialTemplate> pair in elementShadingModule.materialModule.elements)
            {
                uiShadingElementList.Items.Add(pair.Key);
            }

            periodicTableControl1.SetMaterialsModule(elementShadingModule.materialModule);
        }
        #endregion

        #region Scheme Events

        void schemePreviewControl_OnNewPreview(object sender, EventArgs e)
        {
            // put texture onto side image preview
            Texture tex = (Texture)sender;
            GraphicsStream stream = SurfaceLoader.SaveToStream(ImageFileFormat.Bmp, tex.GetSurfaceLevel(0));

            Image img = Bitmap.FromStream(stream);
            pictureBox4.SizeMode = pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.Image = img;
            //pictureBox5.Image = img;

            tex.Dispose();
        }

        private void uiAtomFillModeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentScheme == null)
                return;
            lock (thislock)
            {
                coDesc.AtomShadingDesc.FillMode = (FillMode)uiAtomFillModeList.SelectedIndex + 1;
            }
            RequestSchemeUpdate(true, false);
        }

        private void uiAtomDDraw_CheckedChanged(object sender, EventArgs e)
        {
            if (currentScheme == null)
                return;
            lock (thislock)
            {
                coDesc.AtomShadingDesc.Draw = !uiAtomDDraw.Checked;
            }
            RequestSchemeUpdate(true, false);
        }

        private void uiAtomSymbolsDraw_CheckedChanged(object sender, EventArgs e)
        {
            if (currentScheme == null)
                return;
            lock (thislock)
            {
                coDesc.AtomShadingDesc.SymbolText = uiAtomSymbolsDraw.Checked;
            }
            RequestSchemeUpdate(true, false);
        }

        private void uiAtomSymbolsBlend_CheckedChanged(object sender, EventArgs e)
        {
            if (currentScheme == null)
                return;
            lock (thislock)
            {
                coDesc.AtomShadingDesc.BlendSymbolText = uiAtomSymbolsBlend.Checked;
            }
            RequestSchemeUpdate(true, false);
        }

        private void uiBondSpacingList_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (currentScheme == null)
                return;
            lock (thislock)
            {
                coDesc.BondShadingDesc.Spacing = (BondShadingDesc.BondSpacings)uiBondSpacingList.SelectedIndex;
            }
            RequestSchemeUpdate(false, true);
        }

        private void uiBondEndTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentScheme == null)
                return;
            lock (thislock)
            {
                coDesc.BondShadingDesc.EndType = (BondShadingDesc.BondEndTypes)uiBondEndTypeList.SelectedIndex;
            }
            RequestSchemeUpdate(false, true);
        }

        private void uiBondLODControl_OnLODValueChanged(object sender, EventArgs e)
        {
            if (currentScheme == null)
                return;
            lock (thislock)
            {
                coDesc.SchemeSettings.BondLOD = uiBondLODControl.CurrentLODValue;
            }
            RequestSchemeUpdate(false, true);
        }

        private void uiBondDDraw_CheckedChanged(object sender, EventArgs e)
        {
            if (currentScheme == null)
                return;
            lock (thislock)
            {
                coDesc.BondShadingDesc.Draw = !uiBondDDraw.Checked;
            }
            RequestSchemeUpdate(false, true);
        }

        private void OnSettingsChanged(object sender, SchemeSUIChangeHandler e)
        {
            RequestSchemeUpdate(true, true);
        }

        private void uiAtomLODControl_OnLODValueChanged(object sender, EventArgs e)
        {
            if (currentScheme == null)
                return;
            lock (thislock)
            {
                coDesc.SchemeSettings.AtomLOD = uiAtomLODControl.CurrentLODValue;
            }
            RequestSchemeUpdate(true, false);
        }

        private void buttonBar1_ItemSelected(object sender, EventArgs e)
        {
            if (uiSchemesList.SelectedItem.Index >= 0)
                TryChangeScheme(availableSchemes[uiSchemesList.SelectedItem.Index]);
            else
                TryChangeScheme(null);
        }
        #endregion

        #region Effect Events
        
        private void uiEffectsList_ItemSelected(object sender, EventArgs e)
        {
            // show requirements
        }

        private void effectsListMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (uiEffectsList.SelectedItem == null)
                e.Cancel = true;
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // display effect details dialog
            if (efxDetailsDlg == null)
                efxDetailsDlg = new EffectDetailsDlg();
            efxDetailsDlg.SetEffect(((Effect)uiEffectsList.SelectedItem.Tag).effect);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEffectToCurrent((Effect)uiEffectsList.SelectedItem.Tag);
        }

        private void uiCurrentEffectsList_ItemSelected(object sender, EventArgs e)
        {
            DoChangeEffect((Effect)uiCurrentEffectsList.SelectedItem.Tag);
        }

        void uiEffectLOD_OnLODValueChanged(object sender, EventArgs e)
        {
            if (currentEffect == null)
                return;
            lock (thislock)
            {
                effectPreviewControl1.Effect.LOD = uiEffectLOD.CurrentLODValue;
            }
            RequestEffectUpdate();
        }

        void effectPreviewControl1_OnNewPreview(object sender, EventArgs e)
        {
            // put texture onto side image preview
            Texture tex = (Texture)sender;
            GraphicsStream stream = SurfaceLoader.SaveToStream(ImageFileFormat.Bmp, tex.GetSurfaceLevel(0));

            Image img = Bitmap.FromStream(stream);
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.Image = img;

            tex.Dispose();
        }
        #endregion

        #region Light Events

        void lightPreviewControl2_OnNewPreview(object sender, EventArgs e)
        {
            // put texture onto side image preview
            Texture tex = (Texture)sender;
            GraphicsStream stream = SurfaceLoader.SaveToStream(ImageFileFormat.Bmp, tex.GetSurfaceLevel(0));

            Image img = Bitmap.FromStream(stream);
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.Image = img;

            tex.Dispose();
        }

        private void uiLightEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (currentLight != null)
            {
                currentLight.Enabled = uiLightEnabled.Checked;
                lightPreviewControl2.QueueUpdate(true);
            }
        }

        private void uiLightCastShadows_CheckedChanged(object sender, EventArgs e)
        {
            if (currentLight != null)
            {
                currentLight.CastShadows = uiLightCastShadows.Checked;
                lightPreviewControl2.QueueUpdate(true);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeLight((NuGenSVisualLib.Rendering.Lighting.Light)lightsListBox.SelectedItem);
        }

        void control_OnValueUpdate(object sender, EventArgs e)
        {
            // queue preview update
            lightPreviewControl2.QueueUpdate(true);
        }

        private void uiLightingList_ItemSelected(object sender, EventArgs e)
        {
            //if (uiLightingList.SelectedGroup.Index == 0)
            //{
            // select a lighting setup preset
            if (uiLightingList.SelectedItem != null)
                TryChangePreset((LightingPreset)uiLightingList.SelectedItem.Tag);
            else
                TryChangePreset(null);
            //}
        }

        private void uiColorButton1_SelectedColorChanged(object sender, EventArgs e)
        {
            if (currentLight != null)
            {
                currentLight.Clr = uiColorButton1.SelectedColor;
                lightPreviewControl2.QueueUpdate(true);
            }
        }
        #endregion

        #region Material Events

        private void uiElementShadingList_ItemSelected(object sender, EventArgs e)
        {
            if (uiElementShadingList.SelectedGroup.Index == 0)
            {
            }
            else if (uiElementShadingList.SelectedGroup.Index == 1)
            {
                DoChangeElementModule(allModuleTemplates[(int)uiElementShadingList.SelectedItem.Tag]);
            }
        }

        private void periodicTableControl1_OnElementSelect(object sender, EventArgs e)
        {

        }

        private void uiShadingSeriesList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void uiShadingElementList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uiShadingElementList.SelectedIndex >= 0)
            {
                uiElementClrBtn.SelectedColor = periodicTableControl1.MaterialsModule.ResolveBySymbol(periodicTableControl1.SelectedElement.Symbol).BySymbol.BaseColor;
            }
        }
        #endregion

        #region Events

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //coDesc.BondShadingDesc.EndType = (BondShadingDesc.BondEndTypes)uiBondEndTypeList.SelectedIndex;
            //if (previewReady)
            //    schemePreviewControl1.UpdateBonds();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
        #endregion
    }
}