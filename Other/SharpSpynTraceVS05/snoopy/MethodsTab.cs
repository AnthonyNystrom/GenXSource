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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Genetibase.Debug
{
	/// <summary>
	/// MethodsTab.
	/// </summary>
	internal class MethodsTab : System.Windows.Forms.Design.PropertyTab
	{
		public MethodsTab()
		{
			InitializeComponent();
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

		public override Bitmap Bitmap
		{
			get
			{
				return (Bitmap)Bitmap.FromStream ( GetType().Assembly.GetManifestResourceStream("Genetibase.Debug.Resources.Methods.bmp" ));
			}
		}

		public override object[] Components
		{
			get
			{
				return base.Components;
			}
			set
			{
				base.Components = value;
			}
		}


		public override string TabName
		{
			get
			{
				return "Methods";
			}
		}

		public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
		{
			if ( component != null )
			{
				return Genetibase.Debug.MethodUtils.GetMethodProperties(component);
			}
			return null;
		}
	}
}
