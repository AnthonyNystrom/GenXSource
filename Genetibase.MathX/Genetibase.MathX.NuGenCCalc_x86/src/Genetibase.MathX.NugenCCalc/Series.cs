//using System;
//using System.ComponentModel;
//using System.Drawing;

//namespace Genetibase.MathX.NugenCCalc
//{
//    /// <summary>
//    /// Store settings of series for 2D charts
//    /// </summary>
//    /// <example>
//    /// 	<code lang="CS" description="The following example set new series for NugenCCalcComponent">
//    /// NugenCCalcComponent ccalc = new NugenCCalcComponent();
//    /// ccalc.Series = new Series("newSeries", Color.Red);
//    /// </code>
//    /// </example>
//    [Serializable()]
//    public class Series
//    {
//        private int _index;
//        private string _label;
//        private Color _color;

//        /// <summary>
//        /// Gets or sets the index of the series in the chart
//        /// </summary>
//        [Browsable(false)]
//        public int Index
//        {
//            get
//            {
//                return _index;
//            }
//            set
//            {
//                if (_index != value)
//                {
//                    _index = value;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets or sets the name of the series displayed by the chart
//        /// </summary>
//        [DefaultValue("newSeries")]
//        public string Label
//        {
//            get
//            {
//                return _label;
//            }
//            set
//            {
//                if (this._label != value)
//                {
//                    this._label = value;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets or sets the color of the series displayed by the chart
//        /// </summary>
//        [TypeConverter(typeof(ColorConverter))]
//        public Color Color
//        {
//            get
//            {
//                return _color;
//            }
//            set
//            {
//                if (this._color != value)
//                {
//                    this._color = value;
//                }
//            }
//        }

//        /// <summary>
//        /// Initializes a new instance of the Series class on the specified index, label and color. 
//        /// </summary>
//        /// <param name="index">index of series</param>
//        /// <param name="label">label of series</param>
//        /// <param name="color">color of series</param>
//        public Series(int index, string label, Color color)
//        {
//            _index = index;
//            _label = label;
//            _color = color;
//        }

//        /// <summary>
//        /// Initializes a new instance of the Series class on the specified label and color. 
//        /// </summary>
//        /// <param name="label">label of series</param>
//        /// <param name="color">color of series</param>
//        public Series(string label, Color color)
//        {
//            _index = -1;
//            _label = label;
//            _color = color;
//        }

//        /// <summary>
//        /// Initializes a new instance of the Series class on the specified label.
//        /// </summary>
//        /// <param name="label">label of series</param>
//        public Series(string label)
//        {
//            _index = -1;
//            _label = label;
//            _color = Color.Black;
//        }


//        /// <summary>
//        /// Overridden. Returns label for current series
//        /// </summary>
//        public override string ToString()
//        {
//            return _label;
//        }

//    }
//}
