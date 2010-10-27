using System;
using System.Collections.Generic;
using System.Text;
using Netron.Neon.WinFormsUI;
using     System.Windows.Forms;
namespace Netron.Cobalt
{
    static partial class Application
    {
        public static class Menus
        {
            /// <summary>
            /// Gets the root menu.
            /// </summary>
            /// <value>The root menu.</value>
            public static MenuStrip RootMenu
            {
                get { return MainForm.RootMenuStrip; }
            }
            /// <summary>
            /// Gets the browser main menu.
            /// </summary>
            /// <value>The browser main menu.</value>
            public static ToolStripMenuItem BrowserMainMenu
            {
                get { return MainForm.BrowserMainMenu; }
            }

            /// <summary>
            /// Gets the edit main menu.
            /// </summary>
            /// <value>The edit main menu.</value>
            public static ToolStripMenuItem EditMainMenu
            {
                get { return MainForm.EditToolStripMenuItem; }
            }
            /// <summary>
            /// Gets the undo menu.
            /// </summary>
            /// <value>The undo menu.</value>
            public static ToolStripMenuItem UndoMenu
            {
                get { return MainForm.UndoToolStripMenuItem; }
            }
            /// <summary>
            /// Gets the redo menu.
            /// </summary>
            /// <value>The redo menu.</value>
            public static ToolStripMenuItem RedoMenu
            {
                get { return MainForm.RedoToolStripMenuItem; }
            }

            /// <summary>
            /// Gets the diagram main menu.
            /// </summary>
            /// <value>The diagram main menu.</value>
            public static ToolStripMenuItem DiagramMainMenu
            {
                get { return MainForm.DiagramMainMenuItem; }
            }
        }
    }
}
