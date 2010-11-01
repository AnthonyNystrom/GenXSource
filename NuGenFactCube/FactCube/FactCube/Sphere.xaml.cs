using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Genetibase.FactCube
{
    /// <summary>
    /// Interaction logic for Sphere.xaml
    /// </summary>

    public partial class Sphere : System.Windows.Controls.Canvas
    {
        public static DependencyProperty BrushProperty = DependencyProperty.Register(
            "Brush",
            typeof( Brush ),
            typeof( Sphere ) );

        /// <summary>
        /// Gets or sets CubeBrush value
        /// </summary>
        public Brush Brush
        {
            get { return GetValue( BrushProperty ) as Brush; }
            set { SetValue( BrushProperty, value ); }
        }

        public Sphere()
        {
            InitializeComponent();
        }
    }
}