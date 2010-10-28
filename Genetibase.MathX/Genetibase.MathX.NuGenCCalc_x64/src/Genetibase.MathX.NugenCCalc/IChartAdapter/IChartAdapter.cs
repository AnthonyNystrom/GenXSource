//using System;
//using System.Drawing;

//namespace Genetibase.MathX.NugenCCalc.IChartAdapter
//{
//    /// <summary>
//    /// Base interface for IChartAdapter2D and IChartAdapter3D. Provide methods for
//    /// validate controls.
//    /// </summary>
//    public interface IChartAdapter 
//    {
//        /// <summary>Determines whether the chart is supported.</summary>
//        /// <returns><para><strong>true</strong> is chart supported</para></returns>
//        /// <param name="obj">Control</param>
//        bool Validate(object obj);
//        /// <summary>Set new chart control for current ChartAdapter</summary>
//        /// <param name="chartControl">Chart control</param>
//        void SetChartControl(object chartControl);
//        /// <summary>Return chart control fro this adapter</summary>
//        /// <value>Chart control for current adapter</value>
//        object ChartControl{get;}

//        /// <summary>Occurs when size of chart is changed</summary>
//        event EventHandler SizeChanged;
//        /// <summary>Occurs when scope of chart is changed</summary>
//        event EventHandler ScopeChanged;
//    }
//}
