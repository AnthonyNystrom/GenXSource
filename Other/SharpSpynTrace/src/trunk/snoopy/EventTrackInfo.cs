using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Genetibase.Debug
{
	/// <summary>
	/// Summary description for EventTrackInfo.
	/// </summary>
	public class EventTrackInfo : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public EventTrackInfo()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.treeView1 = new Genetibase.Debug.HighlightTrackTreeView();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.CausesValidation = false;
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.ImageIndex = -1;
			this.treeView1.Name = "treeView1";
			this.treeView1.SelectedImageIndex = -1;
			this.treeView1.Size = new System.Drawing.Size(600, 400);
			this.treeView1.TabIndex = 0;
			// 
			// EventTrackInfo
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.treeView1});
			this.Name = "EventTrackInfo";
			this.Size = new System.Drawing.Size(600, 400);
			this.ResumeLayout(false);

		}
		#endregion

		[Browsable(false)]
		public TreeView TreeView {
			get {
				return treeView1;
			}
		}

		object selectedObject;
		private Genetibase.Debug.HighlightTrackTreeView treeView1;
	
		[Browsable(false)]
		public object SelectedObject {
			set {
				selectedObject = value;
			}
			get {
				return selectedObject;
			}
		}

		ArrayList eventList;
		public ArrayList EventList {
			set {
				eventList = value;
			}
			get {
				return eventList;
			}
		}

		public override ContextMenu ContextMenu {
			set {
				treeView1.ContextMenu = value;
			}
		}
	}
}
