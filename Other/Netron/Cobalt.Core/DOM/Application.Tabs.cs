using System;
using System.Collections.Generic;
using System.Text;

namespace Netron.Cobalt
{
    static partial class Application
    {
        public static partial class Tabs
        {
            #region Fields
            /// <summary>
            /// the Cobalt.IDE shell
            /// </summary>
            private static ShellForm mShellForm;
            /// <summary>
            /// the OutputForm field
            /// </summary>
            private static OutputForm mOutputForm;
             /// <summary>
            /// the PropertyForm field
            /// </summary>
            private static PropertyForm mPropertyForm;
             /// <summary>
            /// the Diagram field
            /// </summary>
            private static DiagramForm mDiagram;
            /// <summary>
            /// the Shapes field
            /// </summary>
            private static ShapesForm mShapes;
            /// <summary>
            /// the WebBrowser field
            /// </summary>
            private static BrowserForm mWebBrowser;
            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets the Cobalt shell form.
            /// </summary>
            /// <value>The shell.</value>
            public static ShellForm Shell
            {
                get {
                    if (mShellForm == null)
                        CreateShellForm();
                    return mShellForm;
                }
                set { mShellForm = value; }
            }
            /// <summary>
            /// Gets or sets the output form.
            /// </summary>
            /// <value>The output form.</value>
            public static OutputForm Output
            {
                get {
                    if (mOutputForm == null)
                        CreateOutputForm();
                    return mOutputForm;
                }
                set { mOutputForm = value; }
            }
                /// <summary>
            /// Gets or sets the PropertyForm
            /// </summary>
            public static PropertyForm Property
            {
                get {
                    if (mPropertyForm == null)
                        CreateProperty();
                    return mPropertyForm; }
                set { mPropertyForm = value; }
            }

           
            /// <summary>
            /// Gets or sets the Diagram
            /// </summary>
            public static DiagramForm Diagram
            {
                get {
                    if (mDiagram == null)
                        CreateDiagramCanvas();
                    return mDiagram; }
                set { mDiagram = value; }
            }


            
            /// <summary>
            /// Gets or sets the Shapes
            /// </summary>
            public static ShapesForm Shapes
            {
                get {
                    if (mShapes == null)
                        CreateShapesForm();
                    return mShapes; }
                set { mShapes = value; }
            }


            
            /// <summary>
            /// Gets or sets the WebBrowser
            /// </summary>
            public static BrowserForm WebBrowser
            {
                get
                {
                    if (mWebBrowser == null)
                        CreateWebBrowser();
                    return mWebBrowser;
                }
                set { mWebBrowser = value; }
            }
            #endregion

        

        }
    }
}
