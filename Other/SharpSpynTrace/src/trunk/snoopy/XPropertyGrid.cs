/* ****************************************************************************
 *  RuntimeObjectEditor
 * 
 * Copyright (c) 2005 Corneliu I. Tusnea
 * 
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the author be held liable for any damages arising from 
 * the use of this software.
 * Permission to use, copy, modify, distribute and sell this software for any 
 * purpose is hereby granted without fee, provided that the above copyright 
 * notice appear in all copies and that both that copyright notice and this 
 * permission notice appear in supporting documentation.
 * 
 * Corneliu I. Tusnea (corneliutusnea@yahoo.com.au)
 * ****************************************************************************/


using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Debug
{
	/// <summary>
	/// Summary description for XPropertyGrid.
	/// </summary>
	public class NuGenPropertyGrid : PropertyGrid
	{
		public NuGenPropertyGrid()
		{
			InitializeComponent();

			this.SelectedObjectsChanged += new EventHandler(XPropertyGrid_SelectedObjectsChanged);
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
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
		#endregion
		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		protected override void WndProc(ref Message msg) 
		{
			base.WndProc(ref msg);
		}

		protected override void OnCreateControl()
		{
			ShowEventsButton(true);
			DrawFlatToolbar = true;
			CommandsVisibleIfAvailable = true;
			HelpVisible = true;

			CommandsVisibleIfAvailable = true;

			base.OnCreateControl ();

			// base.PropertyTabs.AddTabType(typeof(EventsTab)); // this does not work :(
			base.PropertyTabs.AddTabType(typeof(MethodsTab));
		}

		private void XPropertyGrid_SelectedObjectsChanged(object sender, EventArgs e)
		{
			if( SelectedObject != null )
			{
				Attribute[] attrsArray = new Attribute[1] {
					BrowsableAttribute.Yes
					/*,new DesignerAttribute( typeof(MethodDesigner), typeof(IDesigner) )*/ };
				this.BrowsableAttributes = new AttributeCollection(attrsArray);
			}
		}
	}
}
