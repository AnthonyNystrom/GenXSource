using System;
using System.Collections.Generic;
using System.Text;
using Netron.GraphLib;
using Netron.GraphLib.UI;
using GraphSynth.Representation;
using System.Drawing;

namespace GraphSynth
{
    public class graphLayout
    {
        /* One can create 3 custom Layout algorithms to alter the graph
         * appearance. Here I am showing two and leaving the third one 
         * commented out. These names will appear in the GraphLayout menu
         * drop down and are accessible by the posted shortcuts as well.
         * ALSO, you can set a layout to be the default for whenever a 
         * graph window is 'entered'. These names won't be shown in the
         * settings but you can choose one of these as Custom1, Custom2, 
         * Custom3 in the settings dialog. */
        public void diagonalLayout(GraphControl gc, designGraph graph)
        {
            /* in these functions, you need to set the properties of the
             * displayShape of the node and arc, and not their properties
             * directly. */
            Random rnd = new Random();
            int i = 1;
            foreach (node a in graph.nodes)
            {
                a.displayShape.X = 10 * i;
                a.displayShape.Y = 10 * i;
                a.displayShape.ShapeColor = Color.FromArgb((17 * i) % 255, 255 - i, (100 * i) % 255);
                /* in addition to X, Y, and ShapeColor, you can also change
                 * the Z-Order (a.displayShape.ZOrder which is like "order"
                 * in PPT, MS Word, or Photoshop), Width, Height, Show text 
                 * (i.e. ShowLabel), outline (Pen and PenWidth), and font. */
                i++;
            }
            foreach (arc a in graph.arcs)
            {
                a.displayShape.LineColor = Color.Navy;
                //a.displayShape.LineEnd = ConnectionEnd.LeftOpenArrow;
                a.displayShape.LinePath = "Bezier"; // could also be "Default", which is straightline
                                                    // or "Rectangular" but that doesn't seem to work
                                                    // sometimes.
                a.displayShape.LineStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                a.displayShape.LineWeight = ConnectionWeight.Fat;
                a.displayShape.ZOrder = 0;
                a.displayShape.BoxedLabel = false;
                /* here are the basic properties one can change for an arc. */
            }
        }

        public void onlyXLayout(GraphControl gc, designGraph graph)
        {
            int i = 1;
            foreach (node a in graph.nodes)
            {
                a.displayShape.X = 10 * i;
                a.displayShape.Y = 10;
                i++;
            }
        }

        //public void thirdLayout(GraphControl gc, designGraph graph)
        //{
        //}
    }
}
