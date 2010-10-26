using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Org.OpenScience.CDK.Config;
using Org.OpenScience.CDK;
using NuGenSVisualLib.Rendering;
using System.Threading;

namespace NuGenSVisualLib
{
    class ChemSymbolTextures
    {
        private static ChemSymbolTextures instance;
        private static Device device;
        private Dictionary<string, Texture> symbolTextures;

        public ChemSymbolTextures()
        {
            symbolTextures = new Dictionary<string,Texture>();
            if (device != null)
            {
                /*LoadingResourcesDlg dlg = new LoadingResourcesDlg();
                dlg.Text = "Loading Symbol Textures";

                Thread thread = new Thread(new ParameterizedThreadStart(this.LoadingProcess));
                dlg.ProgressProcess = thread;
                dlg.ShowDialog();*/
            }
        }

        private void LoadingProcess(object param)
        {
            LoadingResourcesDlg dlg = (LoadingResourcesDlg)param;
            try
            {
                // load symbols for elements
                ElementPTFactory elements = ElementPTFactory.Instance;
                System.Drawing.Font font = new System.Drawing.Font("Tahoma", 20);
                float onePer = 100.0f / elements.Size;
                float prog = 0;
                foreach (PeriodicTableElement element in elements)
                {
                    Texture tex = TextTexture.DrawTextToTexture(element.Symbol,
                                                font, device, 64, 64).Texture;

                    symbolTextures[element.Symbol] = tex;

                    prog += onePer;
                    dlg.Progress = (int)prog;
                }

                dlg.TryClose();
            }
            catch { }
        }

        public static ChemSymbolTextures Instance
        {
            get
            {
                if (instance == null)
                    instance = new ChemSymbolTextures();
                return instance;
            }
        }

        public Texture this[string symbol]
        {
            get
            {
                Texture tex = null;
                if (symbolTextures.TryGetValue(symbol, out tex))
                    return tex;
                // try load
                ElementPTFactory elements = ElementPTFactory.Instance;
                PeriodicTableElement element = elements.getElement(symbol);
                if (element != null)
                {
                    System.Drawing.Font font = new System.Drawing.Font("Tahoma", 20);
                    tex = TextTexture.DrawTextToTexture(element.Symbol,
                                                        font, device, 64, 64).Texture;

                    return symbolTextures[element.Symbol] = tex;
                }
                return null;
            }
        }

        public static Device GraphicsDevice
        {
            get { return device; }
            set { device = value; }
        }
    }
}
