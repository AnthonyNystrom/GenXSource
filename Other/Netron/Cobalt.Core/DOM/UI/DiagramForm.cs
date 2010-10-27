using System;
using System.IO;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Netron.Diagramming.Core;
using System.Windows.Forms;
using Netron.Neon.WinFormsUI;
namespace Netron.Cobalt
{
    public partial class DiagramForm : DockContent
    {

        private const string constSavingLocation = @"c:\temp\test.netron";
        public DiagramForm()
        {
            InitializeComponent();
            this.undoButton.Enabled = false;
            this.redoButton.Enabled = false;
        }

        /// <summary>
        /// Handles the Click event of the newDiagramButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void newDiagramButton_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure?", "New diagram", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                Application.Diagram.NewDocument();
        }

        #region Undo/redo tools

        /// <summary>
        /// Handles the Click event of the undoButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void undoButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.Undo();
        }

        /// <summary>
        /// Handles the Click event of the redoButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void redoButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.Redo();
        }


        #endregion

        #region Drawing tools
        /// <summary>
        /// Handles the Click event of the drawRectangleButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void drawRectangleButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("Rectangle Tool");
        }
        /// <summary>
        /// Handles the Click event of the drawEllipseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void drawEllipseButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("Ellipse Tool");
        }
        #region Diagram control handler links
        private void diagramControl1_OnShowSelectionProperties(object sender, SelectionEventArgs e)
        {
            Application.Tabs.Property.Show();
            Application.Property.SelectedObjects = e.SelectedObjects;
        }
        #endregion

        private void diagramControl1_OnEntityAdded(object sender, EntityEventArgs e)
        {
            string msg = string.Empty;
            if (e.Entity is IShape)
                msg = "New shape '" + e.Entity.EntityName + "' added.";
            else if (e.Entity is IConnection)
                msg = "New connection added.";

            if (msg.Length > 0)
                Application.Status.ShowStatusText(msg);
            if (e.Entity is ComplexRectangle)
            {
                ComplexRectangle shape = e.Entity as ComplexRectangle;

                try
                {
                    //shape.Services[typeof(IMouseListener)]= new MyPlugin(shape);
                }
                catch (ArgumentException exc)
                {
                    Application.Status.ShowStatusText(exc.Message);

                }


            }
            else if (e.Entity is ImageShape)
            {
                Bitmap bmp = GetSampleImage();
                if (bmp != null)
                    (e.Entity as ImageShape).Image = bmp;
            }
        }
        private Bitmap GetSampleImage()
        {

            Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Cobalt.Resources.Talking.gif");
            Bitmap bmp = null;
            if (stream != null)
            {
                bmp = Bitmap.FromStream(stream) as Bitmap;
                //if you close the stream here, serialization of the shape breaks down....one of the wonders of .Net...
                //stream.Close();
            }
            return bmp;
        }



        /// <summary>
        /// Handles the Click event of the connectionButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void connectionButton_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Not implemented yet.", "Hang on", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Diagram.ActivateTool("Connection Tool");
        }

        /// <summary>
        /// Handles the Click event of the connectorMoverButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void connectorMoverButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("Connector Mover Tool");
        }

        private void textToolButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("Text Tool");
        }
        #endregion

        #region Grouping tools
        /// <summary>
        /// Handles the Click event of the groupButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void groupButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("Group Tool");
        }

        /// <summary>
        /// Handles the Click event of the ungroupButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void ungroupButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("Ungroup Tool");
        }
        #endregion

        #region Z-ordering tools
        /// <summary>
        /// Handles the Click event of the sendToBackButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void sendToBackButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("SendToBack Tool");
        }

        /// <summary>
        /// Handles the Click event of the sendBackwardsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void sendBackwardsButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("SendBackwards Tool");
        }

        /// <summary>
        /// Handles the Click event of the sendForwardButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void sendForwardButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("SendForwards Tool");
        }

        /// <summary>
        /// Handles the Click event of the sendToFrontButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void sendToFrontButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("SendToFront Tool");
        }

        #endregion
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Properties.Settings.Default.LineStyleStripLocation = this.LineStyleStrip.Location;

            Properties.Settings.Default.DrawingStripLocation = this.DrawingStrip.Location;
            Properties.Settings.Default.ActionStripLocation = this.ActionsStrip.Location;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Updates the undo/redo buttons to reflect the history changes
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:Netron.Diagramming.Core.HistoryChangeEventArgs"/> instance containing the event data.</param>
        private void diagramControl1_OnHistoryChange(object sender, HistoryChangeEventArgs e)
        {
            if (e.UndoText.Length == 0)
            {
                this.undoButton.Enabled = false;
                Application.Menus.UndoMenu.Enabled = false;
            }
            else
            {
                this.undoButton.Enabled = true;
                Application.Menus.UndoMenu.Enabled = true;
                this.undoButton.ToolTipText = "Undo " + e.UndoText;
                Application.Menus.UndoMenu.Text = "Undo " + e.UndoText;
            }

            if (e.RedoText.Length == 0)
            {
                this.redoButton.Enabled = false;
                Application.Menus.RedoMenu.Enabled = false;
            }
            else
            {
                this.redoButton.Enabled = true;
                Application.Menus.RedoMenu.Enabled = true;
                this.redoButton.ToolTipText = "Redo " + e.RedoText;
                Application.Menus.RedoMenu.Text = "Redo " + e.RedoText;
            }
        }

        #region Toolbars actions

        /// <summary>
        /// View or hide the actions strip.
        /// </summary>
        private void ViewHideActionsStrip()
        {
            actionsToolStripMenuItem.Checked = !actionsToolStripMenuItem.Checked;
            ActionsStrip.Visible = actionsToolStripMenuItem.Checked;
        }

        /// <summary>
        /// View or hide the drawing strip.
        /// </summary>
        private void ViewHideDrawingStrip()
        {
            drawingToolStripMenuItem.Checked = !drawingToolStripMenuItem.Checked;
            DrawingStrip.Visible = drawingToolStripMenuItem.Checked;
        }
        /// <summary>
        /// Handles the Click event of the drawingButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void drawingButton_Click(object sender, EventArgs e)
        {
            ViewHideDrawingStrip();
        }

        #endregion

        #region Context menu of the toolbars
        /// <summary>
        /// Handles the Click event of the standardToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void standardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            standardToolStripMenuItem.Checked = !standardToolStripMenuItem.Checked;
            MainStrip.Visible = standardToolStripMenuItem.Checked;
        }
        /// <summary>
        /// Handles the Click event of the drawingToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void drawingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewHideDrawingStrip();
        }
       
        /// <summary>
        /// Handles the Click event of the actionsToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void actionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewHideActionsStrip();
        }
          #endregion

        private void propertiesButton_Click(object sender, EventArgs e)
        {
            Application.Tabs.Property.Show();
            Application.Property.SelectedObjects = Application.Diagram.SelectedItems.ToArray();
        }

        private void propertiesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowPropsForm();
        }

        private void ShowPropsForm()
        {
            using (DocumentProperties propfrom = new DocumentProperties(Application.Diagram))
            {
                propfrom.ShowDialog(this);
            }
        }
        /// <summary>
        /// Handles the Click event of the saveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveAs(constSavingLocation);

        }

        /// <summary>
        /// Handles the Click event of the openDiagramButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void openDiagramButton_Click(object sender, EventArgs e)
        {
            Open(constSavingLocation);
        }

        /// <summary>
        /// Handles the Click event of the printButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void printButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet.", "Hang on", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Handles the Click event of the cutButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void cutButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet.", "Hang on", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Handles the Click event of the copyButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void copyButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("Copy Tool");
        }

        /// <summary>
        /// Handles the Click event of the pasteButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void pasteButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("Paste Tool");
        }

        private void saveDiagramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            SaveFileDialog fileChooser = new SaveFileDialog();
            fileChooser.CheckFileExists = false;
            fileChooser.Filter = "Netron Graphs (*.netron)|*.netron";
            fileChooser.InitialDirectory = "\\c:";
            fileChooser.RestoreDirectory = true;
            fileChooser.Title = "Save diagram to binary file";
            DialogResult result = fileChooser.ShowDialog();
            string filename;

            if(result == DialogResult.Cancel)
                return;
            filename = fileChooser.FileName;
            if(filename == "" || filename == null)
                MessageBox.Show("Invalid file name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {

                if(mediator.GraphControl.SaveAs(filename))
                    MessageBox.Show("The diagram was saved in '" + filename + "'", "Save info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("The diagram was not saved.", "Save info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
             */
            SaveAs(constSavingLocation);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(constSavingLocation);
        }

        private void SaveAs(string path)
        {
            Application.Diagram.SaveAs(path);
            Application.Status.ShowStatusText("The diagram was saved to '" + Path.GetFileName(path) + "'.");
        }

        private void Open(string path)
        {
            Application.Diagram.Open(path);
            Application.Status.ShowStatusText("The diagram '" + Path.GetFileName(path) + "' is loaded.");
        }

        private void scribbleToolButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("Scribble Tool");
        }

        private void mutliLineToolButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("Multiline Tool");
        }

        private void polygonToolButton_Click(object sender, EventArgs e)
        {
            Application.Diagram.ActivateTool("Polygon Tool");
        }



        private void colorPickerToolButton_SelectedColorChanged(object sender, EventArgs e)
        {
            GradientPaintStyle paintStyle = new GradientPaintStyle(colorPickerToolButton.Color, Color.White, -135);
            Application.Diagram.ChangeStyle(paintStyle);
        }

        private void lineCapPicker_SelectedCapChanged(object sender, EventArgs e)
        {
            ChangePenStyle();
        }

        /// <summary>
        /// Changes the pen style of the selected connection.
        /// </summary>
        private void ChangePenStyle()
        {
            //instantiate the style
            LinePenStyle penStyle = new LinePenStyle();
            //fetch the selected cap
            LineCap cap;
            if ((cap = GetRightCap()) == LineCap.Custom)
            {
                penStyle.EndCap = LineCap.Custom;
                penStyle.CustomEndCap = LinePenStyle.GenerallizationCap;
            }
            else
                penStyle.EndCap = cap;
            if ((cap = GetLeftCap()) == LineCap.Custom)
            {
                penStyle.StartCap = LineCap.Custom;
                penStyle.CustomStartCap = LinePenStyle.GenerallizationCap;
            }
            else
                penStyle.StartCap = cap;
            //fetch the other properties
            penStyle.DashStyle = this.lineStylePicker.LineStyle;
            penStyle.Width = this.lineWidthPicker.LineWidth;
            penStyle.Color = this.lineColorPicker.Color;
            //call the method; it will change the pe style of the selected entities
            Application.Diagram.ChangeStyle(penStyle);


        }
        private LineCap GetLeftCap()
        {
            switch (this.lineCapPicker.LineCap)
            {
                case Netron.Neon.OfficePickers.ConnectionCap.NoCap:
                    return LineCap.NoAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.DiamondLeft:
                    return LineCap.DiamondAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.DiamondRight:
                    return LineCap.NoAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.DiamondBoth:
                    return LineCap.DiamondAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.ArrowLeft:
                    return LineCap.ArrowAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.ArrowRight:
                    return LineCap.NoAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.ArrowBoth:
                    return LineCap.ArrowAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.RoundLeft:
                    return LineCap.RoundAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.RoundRight:
                    return LineCap.NoAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.RoundBoth:
                    return LineCap.RoundAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.SquareLeft:
                    return LineCap.SquareAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.SquareRight:
                    return LineCap.NoAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.SquareBoth:
                    return LineCap.SquareAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.Generalization:
                    return LineCap.NoAnchor;
                default:
                    return LineCap.NoAnchor;
            }
        }
        private LineCap GetRightCap()
        {
            switch (this.lineCapPicker.LineCap)
            {
                case Netron.Neon.OfficePickers.ConnectionCap.NoCap:
                    return LineCap.NoAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.DiamondLeft:
                    return LineCap.NoAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.DiamondRight:
                    return LineCap.DiamondAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.DiamondBoth:
                    return LineCap.DiamondAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.ArrowLeft:
                    return LineCap.NoAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.ArrowRight:
                    return LineCap.ArrowAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.ArrowBoth:
                    return LineCap.ArrowAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.RoundLeft:
                    return LineCap.NoAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.RoundRight:
                    return LineCap.RoundAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.RoundBoth:
                    return LineCap.RoundAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.SquareLeft:
                    return LineCap.NoAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.SquareRight:
                    return LineCap.SquareAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.SquareBoth:
                    return LineCap.SquareAnchor;
                case Netron.Neon.OfficePickers.ConnectionCap.Generalization:
                    return LineCap.Custom;
                default:
                    return LineCap.NoAnchor;
            }
        }

        private void lineStylePicker_SelectedStyleChanged(object sender, EventArgs e)
        {
            ChangePenStyle();
        }

        private void lineWidthPicker_SelectedWidthChanged(object sender, EventArgs e)
        {
            ChangePenStyle();
        }

        private void lineColorPicker_SelectedColorChanged(object sender, EventArgs e)
        {
            ChangePenStyle();
        }

       

        /// <summary>
        /// Handles the Click event of the helpButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void helpButton_Click(object sender, EventArgs e)
        {
            Application.Tabs.WebBrowser.Show();
            Application.Tabs.WebBrowser.GotoForum();

        }

        private void RandomLayoutMenuItem_Click(object sender, EventArgs e)
        {
            this.DiagramControl.RunActivity("Random layout");
        }

        private void ForceDirectedLayoutMenuItem_Click(object sender, EventArgs e)
        {
            Application.Diagram.RunActivity("ForceDirected Layout");
        }

        private void RadialTreeLayoutMenuItem_Click(object sender, EventArgs e)
        {
            this.DiagramControl.RunActivity("Radial TreeLayout");
        }

        private void StandardTreeLayoutMenuItem_Click(object sender, EventArgs e)
        {
            this.DiagramControl.Layout(Netron.Diagramming.Core.LayoutType.ClassicTree);
            //this.DiagramControl.RunActivity("Standard TreeLayout");
        }

        private void BalloonLayoutMenuItem_Click(object sender, EventArgs e)
        {
            this.DiagramControl.RunActivity("Balloon TreeLayout");
        }

        private void FruchtermanRheingoldLayoutMenuItem_Click(object sender, EventArgs e)
        {
            this.DiagramControl.RunActivity("FruchtermanReingold Layout");
        }

        public void ChangeToolbarVisibility(DiagramMenuStrip toolbar, bool visible)
        {
            switch (toolbar)
            {
                case DiagramMenuStrip.All:
                    this.MainStrip.Visible = visible;
                    this.DrawingStrip.Visible = visible;
                    this.LineStyleStrip.Visible = visible;
                    this.ActionsStrip.Visible = visible;
                    this.AlignStrip.Visible = visible;
                    break;
                case DiagramMenuStrip.Drawing:
                    this.DrawingStrip.Visible = visible;
                    break;
                case DiagramMenuStrip.LineStyle:
                    this.LineStyleStrip.Visible = visible;
                    break;
                case DiagramMenuStrip.Actions:
                    this.ActionsStrip.Visible = visible;
                    break;
                case DiagramMenuStrip.Main:
                    this.MainStrip.Visible = visible;
                    break;
                case DiagramMenuStrip.Align:
                    this.AlignStrip.Visible = visible;
                    break;
                default:
                    break;
            }
        }

        private void alignLeftButton_Click(object sender, EventArgs e)
        {
            this.DiagramControl.ActivateTool("Align Left");
        }
    }

    public enum DiagramMenuStrip
    { 
        /// <summary>
        /// All the strips
        /// </summary>
        All,
        /// <summary>
        /// The drawing tools
        /// </summary>
        Drawing,
        /// <summary>
        /// The line style controls
        /// </summary>
        LineStyle,
        /// <summary>
        /// Grouping, layout, etc.
        /// </summary>
        Actions,
        /// <summary>
        /// The main toolbar.
        /// </summary>
        Main,
        /// <summary>
        /// Alignation of the shapes.
        /// </summary>
        Align
    }
}