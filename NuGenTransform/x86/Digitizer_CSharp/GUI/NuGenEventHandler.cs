using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.NuGenTransform
{
    public interface NuGenEventHandler
    {
        #region Event Handlers

        #region File Menu
        void Import_Click(object sender, System.EventArgs args);
        void Open_Click(object sender, System.EventArgs args);
        void Close_Click(object sender, System.EventArgs args);

        void Save_Click(object sender, System.EventArgs args);
        void SaveAs_Click(object sender, System.EventArgs args);
        void Export_Click(object sender, System.EventArgs args);
        void ExportAs_Click(object sender, System.EventArgs args);

        void Exit_Click(object sender, System.EventArgs args);

        void Print_Click(object sender, System.EventArgs args);

        #endregion

        #region EditMenu

        void Cut_Click(object sender, System.EventArgs args);
        void Copy_Click(object sender, System.EventArgs args);
        void Paste_Click(object sender, System.EventArgs args);
        void PasteAsNew_Click(object sender, System.EventArgs args);

        #endregion

        #region View Menu

        void AxesPointsView_Click(object sender, System.EventArgs args);
        void ScaleBarView_Click(object sender, System.EventArgs args);
        void CurvePointsView_Click(object sender, System.EventArgs args);
        void MeasurePointsView_Click(object sender, System.EventArgs args);
        void AllPointsView_Click(object sender, System.EventArgs args);

        void NoBackground_Click(object sender, System.EventArgs args);
        void OriginalBackground_Click(object sender, System.EventArgs args);

        void ProcessedImage_Click(object sender, System.EventArgs args);

        void GridlinesDisplay_Click(object sender, System.EventArgs args);

        void CurveGeometryInfo_Click(object sender, System.EventArgs args);
        void MeasureGeometryInfo_Click(object sender, System.EventArgs args);

        #endregion

        #region Digitize Menu

        void Select_Click(object sender, System.EventArgs args);

        void AxisPoint_Click(object sender, System.EventArgs args);
        void ScaleBar_Click(object sender, System.EventArgs args);

        void CurvePoint_Click(object sender, System.EventArgs args);
        void SegmentFill_Click(object sender, System.EventArgs args);
        void PointMatch_Click(object sender, System.EventArgs args);

        void MeasurePoint_Click(object sender, System.EventArgs args);

        #endregion

        #region Settings Menu

        void Coordinates_Click(object sender, System.EventArgs args);

        void Axes_Settings_Click(object sender, System.EventArgs args);
        void ScaleBar_Settings_Click(object sender, System.EventArgs args);
        void Curves_Settings_Click(object sender, System.EventArgs args);
        void Segments_Settings_Click(object sender, System.EventArgs args);
        void PointMatch_Settings_Click(object sender, System.EventArgs args);
        void Measures_Settings_Click(object sender, System.EventArgs args);

        void Discretize_Settings_Click(object sender, System.EventArgs args);
        void GridRemoval_Settings_Click(object sender, System.EventArgs args);

        void GridDisplay_Settings_Click(object sender, System.EventArgs args);

        void Export_Settings_Click(object sender, System.EventArgs args);

        #endregion

        #region Window Menu
        void Cascade_Click(object sender, System.EventArgs args);
        void Tile_Click(object sender, System.EventArgs args);
        #endregion

        void View_Activated(object sender, System.EventArgs args);

        void Show_Coordinates(int x, int y);

        #endregion

        bool ProcessDialogKey(System.Windows.Forms.Keys keyData);

        void Refresh();
    }
}
