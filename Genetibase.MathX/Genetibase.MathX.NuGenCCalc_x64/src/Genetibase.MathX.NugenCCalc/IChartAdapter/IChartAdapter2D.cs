//using System;
//using System.Drawing;

//using Genetibase.MathX.Core;
 
//namespace Genetibase.MathX.NugenCCalc.IChartAdapter
//{
//    /// <summary>
//    /// Instances of classes that implement this interface provide methods for work with
//    /// 2D charts.
//    /// </summary>
//    public interface IChartAdapter2D : IChartAdapter
//    {

//        /// <summary>Set chart axes on given values</summary>
//        /// <param name="minX">Min value of X axes</param>
//        /// <param name="maxX">Max value of X axes</param>
//        /// <param name="minY">Min value of Y axes</param>
//        /// <param name="maxY">Max value of Y axes</param>
//        void SetAxes(double minX, double maxX, double minY, double maxY);
//        /// <summary>Delete all series from chart.</summary>
//        void ClearChartSeries();

//        /// <summary>Return all series names from chart</summary>
//        /// <returns>String array that contains all series name</returns>
//        string[] GetSeriesNames();
//        /// <summary>Return all series from chart</summary>
//        /// <returns>Series array that contains all series from chart</returns>
//        Series[] GetSeries();

//        /// <summary>Plot array of double values on chart with given series name</summary>
//        /// <param name="values">Array of double values</param>
//        /// <param name="seriesName">Series name for plot on chart</param>
//        int Plot(double[] values, string seriesName);
//        /// <summary>Plot array of Point2D on chart with given series name</summary>
//        /// <param name="points">Array of Point2D</param>
//        /// <param name="seriesName">Series name for plot on chart</param>
//        int Plot(Point2D[] points, string seriesName);
//        /// <summary>Plot array of double values on chart with given series index</summary>
//        /// <param name="values">Array of double values</param>
//        /// <param name="seriesIndex">Series index for plot on chart</param>
//        void Plot(double[] values, int seriesIndex);
//        /// <summary>Plot array of Point2D on chart with given series index</summary>
//        /// <param name="points">Array of Point2D</param>
//        /// <param name="seriesIndex">Series index for plot on chart</param>
//        void Plot(Point2D[] points, int seriesIndex);
//        /// <summary>Plot array of double values on chart with given series</summary>
//        /// <param name="values">Array of double values</param>
//        /// <param name="series">Series for plot on chart</param>
//        void Plot(double[] values, Series series);
//        /// <summary>Plot array of Point2D on chart with given series</summary>
//        /// <param name="points">Array of Point2D</param>
//        /// <param name="series">Series for plot on chart</param>
//        void Plot(Point2D[] points, Series series);

//        /// <summary>Plot array of Point2D on polar chart with given series name</summary>
//        /// <param name="points">Array of Point2D</param>
//        /// <param name="seriesName">Series name for plot on chart</param>
//        int PlotPolar(Point2D[] points, string seriesName);
//        /// <summary>Plot array of Point2D on polar chart with given series index</summary>
//        /// <param name="points">Array of Point2D</param>
//        /// <param name="seriesIndex">Series index for plot on chart</param>
//        void PlotPolar(Point2D[] points, int seriesIndex);
//        /// <summary>Plot array of Point2D on polar chart with given series</summary>
//        /// <param name="points">Array of Point2D</param>
//        /// <param name="series">Series for plot on chart</param>
//        void PlotPolar(Point2D[] points, Series series);

//        /// <summary>Plot array of double values on polar chart with given series name</summary>
//        /// <param name="values">Array of double values</param>
//        /// <param name="seriesName">Series name for plot on chart</param>
//        int PlotPolar(double[] values, string seriesName);
//        /// <summary>Plot array of double values on polar chart with given series index</summary>
//        /// <param name="values">Array of double values</param>
//        /// <param name="seriesIndex">Series index for plot on chart</param>
//        void PlotPolar(double[] values, int seriesIndex);
//        /// <summary>Plot array of double values on polar chart with given series</summary>
//        /// <param name="values">Array of double values</param>
//        /// <param name="series">Series for plot on chart</param>
//        void PlotPolar(double[] values, Series series);

//        /// <summary>Return area size of chart</summary>
//        Size PlotAreaSize{get;}

//        /// <summary>Return min value of X axes of chart</summary>
//        double MinX{get;}
//        /// <summary>Return max value of X axes of chart</summary>
//        double MaxX{get;}
//        /// <summary>Return min value of Y axes of chart</summary>
//        double MinY{get;}
//        /// <summary>Return max value of Y axes of chart</summary>
//        double MaxY{get;}

//    }
//}
