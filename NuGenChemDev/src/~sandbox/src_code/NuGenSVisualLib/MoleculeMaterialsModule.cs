using System;
using System.Collections.Generic;
using System.Text;
using Org.OpenScience.CDK.Config;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Drawing;
using NuGenSVisualLib.Settings;
using System.Xml.Serialization;
using Org.OpenScience.CDK;
using Microsoft.DirectX;
using System.Drawing.Drawing2D;

namespace NuGenSVisualLib.Rendering.Chem.Materials
{
    public interface IMoleculeMaterial
    {
        Color BaseColor { get; }
    }

    public class AtomMaterial : IMoleculeMaterial
    {
        Color baseColor;

        public AtomMaterial(Color baseColor)
        {
            this.baseColor = baseColor;
        }

        #region IMoleculeMaterial Members

        public Color BaseColor
        {
            get { return baseColor; }
        }

        #endregion
    }

    public interface IMoleculeMaterialTemplate
    {
        IMoleculeMaterial BySymbol { get; }
        IMoleculeMaterial BySerie { get; }
    }

    public interface IMoleculeMaterialLookup
    {
        IMoleculeMaterial GetBySeries(string serie);
        //IMoleculeMaterial GetBySymbol(string symbol);

        IMoleculeMaterialTemplate ResolveBySymbol(string symbol);
    }

    public class MoleculeMaterialTemplate : IMoleculeMaterialTemplate
    {
        IMoleculeMaterial symbolMat;
        IMoleculeMaterial serieMat;

        public MoleculeMaterialTemplate(IMoleculeMaterial symbolMat, IMoleculeMaterial serieMat)
        {
            this.symbolMat = symbolMat;
            this.serieMat = serieMat;
        }

        #region IMoleculeMaterialTemplate Members

        public IMoleculeMaterial BySymbol
        {
            get { if (symbolMat != null) return symbolMat; return serieMat; }
            set { symbolMat = value; }
        }

        public IMoleculeMaterial BySerie
        {
            get { return serieMat; }
            set { serieMat = value; }
        }

        #endregion
    }

    abstract class MoleculeMaterialsModule : ISettingsModule, IMoleculeMaterialLookup
    {
        public static string[] serieNames = new string[] { "Nonmetals", "Noble Gasses", "Alkali Metals", "Alkali Earth Metals",
                                                           "Metalloids", "Halogens" , "Metals", "Transition metals",
                                                           "Lanthanides", "Actinides", "Actinides" };

        string name;
        internal Dictionary<string, IMoleculeMaterial> series;
        internal Dictionary<string, MoleculeMaterialTemplate> elements;

        public MoleculeMaterialsModule(string name)
        {
            this.name = name;
            series = new Dictionary<string, IMoleculeMaterial>();
            elements = new Dictionary<string, MoleculeMaterialTemplate>();
        }

        #region ISettingsModule Members

        public abstract void LoadModuleSettings(HashTableSettings settings);

        [XmlAttribute(AttributeName="Name", DataType="string")]
        public string Name
        {
            get { return name; }
        }

        #endregion

        #region IMoleculeMaterialLookup Members

        public IMoleculeMaterialTemplate ResolveBySymbol(string symbol)
        {
            MoleculeMaterialTemplate temp = null;
            elements.TryGetValue(symbol, out temp);
            return temp;
        }

        public IMoleculeMaterial GetBySeries(string serie)
        {
            IMoleculeMaterial mat = null;
            series.TryGetValue(serie, out mat);
            return mat;
        }

        #endregion

        public Bitmap DrawPTToBitmap(int maxWidth, int maxHeight, bool keepAspect,
                                     bool showStates, bool showNaturalOccurance,
                                     bool showSeries, bool showRowColumnHeadings,
                                     out int width, out int height,
                                     out int elWidth, out int elHeight,
                                     out Size tableOrigin)
        {
            // NOTE: Table is 18x10 + 1x1 for row/columm headers
            // calc size needed & position
            int cellsOnX = showRowColumnHeadings ? 19 : 18;
            int cellsOnY = showRowColumnHeadings ? 8 : 7;

            float ratio = ((float)cellsOnX / (float)cellsOnY) * 1.25f;

            width = maxWidth;
            height = maxHeight;
            if (keepAspect)
            {
                if (maxHeight * ratio > maxWidth)
                    height = (int)(maxWidth / ratio);
                else if (maxWidth / ratio > maxHeight)
                    width = (int)(maxHeight * ratio);
            }

            elWidth = width / cellsOnX;
            elHeight = height / cellsOnY;

            tableOrigin = new Size(maxWidth - width, maxHeight - height);
            if (tableOrigin.Width > 0)
                tableOrigin.Width /= 2;
            if (tableOrigin.Height > 0)
                tableOrigin.Height /= 2;
            if (showRowColumnHeadings)
            {
                tableOrigin.Width += elWidth;
                tableOrigin.Height += elHeight;
            }


            // draw table
            return DrawPTToBitmap(elWidth, elHeight, showStates, showNaturalOccurance,
                                  showSeries, showRowColumnHeadings);
        }

        public Bitmap DrawPTToBitmap(int cellWidth, int cellHeight,
                                     bool showStates, bool showNaturalOccurance,
                                     bool showSeries, bool showRowColumnHeadings)
        {
            Bitmap bitmap = new Bitmap(cellWidth * 19, cellHeight * 8);
            DrawPT(bitmap, cellWidth, cellHeight, showStates, showNaturalOccurance, showSeries, showRowColumnHeadings);
            return bitmap;
        }

        public void DrawPT(Bitmap bitmap, int cellWidth, int cellHeight,
                           bool showStates, bool showNaturalOccurance,
                           bool showSeries, bool showRowColumnHeadings)
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            Pen solidCellBorder = new Pen(Color.Black, 1f);
            Pen dashedCellBorder = new Pen(Color.Black, 1f);
            dashedCellBorder.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            Pen dottedCellBorder = new Pen(Color.Black, 1f);
            dottedCellBorder.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            Font symbolFont = new Font("Tohoma", 7);
            SolidBrush symbolBrush = new SolidBrush(Color.White);
            SolidBrush symbolBrush2 = new SolidBrush(Color.Black);

            int xOffset = 0, yOffset = 0;
            if (showRowColumnHeadings)
            {
                // draw row / column headers
                xOffset = cellWidth;
                yOffset = cellHeight;

                for (int i = 1; i <= 18; i++)
                {
                    int x = i * cellWidth;
                    int y = 3;
                    SizeF sz = g.MeasureString(i.ToString(), symbolFont);
                    g.DrawString(i.ToString(), symbolFont, symbolBrush2, (float)x + ((float)cellWidth / 2) - (sz.Width / 2), y);
                }
                for (int i = 1; i <= 7; i++)
                {
                    int x = 3;
                    int y = i * cellHeight;
                    SizeF sz = g.MeasureString(i.ToString(), symbolFont);
                    g.DrawString(i.ToString(), symbolFont, symbolBrush2, x, (float)y + ((float)cellHeight / 2) - (sz.Height / 2));
                }
            }

            foreach (PeriodicTableElement element in ElementPTFactory.Instance)
            {
                if (element.Group.Length > 0)
                {
                    int group = int.Parse(element.Group);
                    int period = int.Parse(element.Period);

                    int x = xOffset + (group - 1) * cellWidth;
                    int y = yOffset + (period - 1) * cellHeight;

                    // fill
                    IMoleculeMaterial elMat = null, seriesMat = null;
                    if (elements.ContainsKey(element.Symbol))
                    {
                        elMat = elements[element.Symbol].BySymbol;
                        seriesMat = elements[element.Symbol].BySerie;
                    }
                    if (series.ContainsKey(element.ChemicalSerie))
                        seriesMat = series[element.ChemicalSerie];

                    using (LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, cellHeight / 2), seriesMat.BaseColor, Color.White))
                    {
                        g.FillRectangle(brush, x, y + (cellHeight / 2), cellWidth - 1, (cellHeight / 2) - 1);
                    }
                    using (SolidBrush brush = new SolidBrush(seriesMat.BaseColor))
                    {
                        g.FillRectangle(brush, x, y, cellWidth - 1, (cellHeight / 2) + 1);
                    }
                    if (elMat != null)
                    {
                        using (SolidBrush brush = new SolidBrush(elMat.BaseColor))
                        {
                            g.FillRectangle(brush, x + 4, y + 4, cellWidth - 9, cellHeight - 9);
                        }
                    }

                    // border
                    if (element.Phase == "Gas")
                        g.DrawRectangle(dottedCellBorder, x, y, cellWidth - 2, cellHeight - 2);
                    else if (element.Phase == "Liquid")
                        g.DrawRectangle(dashedCellBorder, x, y, cellWidth - 2, cellHeight - 2);
                    else
                        g.DrawRectangle(solidCellBorder, x, y, cellWidth - 2, cellHeight - 2);

                    // symbol
                    SizeF symbolSz = g.MeasureString(element.Symbol, symbolFont);
                    g.DrawString(element.Symbol, symbolFont, symbolBrush2,
                                 x + ((float)cellWidth / 2) - (symbolSz.Width / 2) + 1,
                                 y + ((float)cellHeight / 2) - (symbolSz.Height / 2) + 1);
                    g.DrawString(element.Symbol, symbolFont, symbolBrush,
                                 x + ((float)cellWidth / 2) - (symbolSz.Width / 2),
                                 y + ((float)cellHeight / 2) - (symbolSz.Height / 2));
                }
            }
            g.Flush();
            g.Dispose();
        }
    }

    class MoleculeDefaultMaterials : MoleculeMaterialsModule
    {
        public MoleculeDefaultMaterials()
            : base("Default")
        { }

        public override void LoadModuleSettings(HashTableSettings settings)
        {
            //settings["Materials.Molecules.IMoleculeMaterialLookup"] = this;

            // load molecule settings from xml resource
            ColorConverter cc = new ColorConverter();
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenSVisualLib.Molecule.config");
            XmlDocument doc = new XmlDocument();
            doc.Load(str);
            
            // load series settings
            XmlNodeList series = doc.SelectNodes("configuration/chemicalSeries/chemicalSerie");
            foreach (XmlNode serie in series)
            {
                string id = serie.Attributes["id"].InnerText;
                Color baseColor = (Color)cc.ConvertFromString(serie.SelectSingleNode("color").Attributes["desc"].InnerText);
                
                this.series[id] = new AtomMaterial(baseColor);
            }

            // load symbols
            XmlNodeList symbols = doc.SelectNodes("configuration/chemicalSymbols/symbol");
            ElementPTFactory ptElements = ElementPTFactory.Instance;
            foreach (XmlNode symbol in symbols)
            {
                string id = symbol.Attributes["id"].InnerText;
                Color baseColor = (Color)cc.ConvertFromString(symbol.SelectSingleNode("color").Attributes["desc"].InnerText);

                PeriodicTableElement element = ptElements.getElement(id);

                IMoleculeMaterial serie = null;
                this.series.TryGetValue(element.ChemicalSerie, out serie);

                elements[id] = new MoleculeMaterialTemplate(new AtomMaterial(baseColor), serie);
            }
        }
    }
}
