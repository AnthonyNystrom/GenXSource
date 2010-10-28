#region Copyright 2001-2006 Christoph Daniel Rüegg, Tobias Finazzi [GNU Public License]
/*
A Pattern Matching Demonstration using NeuroBox Neural Network Library
Copyright (c) 2001-2006, Christoph Daniel Rueegg, Tobias Finazzi.
http://cdrnet.net/. All rights reserved.

Using Netron Library, Copyright Francois Vanderseypen, Lutz Roeder
http://netron.sourceforge.net

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
*/
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using NeuroBox;

namespace NeuroBox.PatternMatchingDemo
{
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.ComponentModel.Container components = null;
		private NeuroBox.PatternMatchingDemo.PatternMatching patternMatching;
		private System.Windows.Forms.MenuItem menuAbout;
		private System.Windows.Forms.MenuItem menuNetwork;
		private System.Windows.Forms.MenuItem menuPropagate;
		private System.Windows.Forms.MenuItem menuBackpropagate;
		private System.Windows.Forms.MenuItem menuAutoTrain;
		private System.Windows.Forms.MenuItem menuAutoTrainPrimitive;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuReset;
		private System.Windows.Forms.MenuItem menuConfig;
		private System.Windows.Forms.MenuItem menuInfo;

		public MainForm()
		{
			InitializeComponent();

			//CHARACTER Matching Training:
			/*patternMatching = new PatternMatching(7,7,26);
			patternMatching.Location = new Point(0,0);
			Controls.Add(patternMatching);
            
			patternMatching.AddPattern("Ziffer A [0]", 0, false, false, true,  true,  false, false, false,    false, false, true,  true,  false, false, false,   false, true,  false, false, true,  false, false,   false, true,  false, false, true,  false, false,   false, true,  true,  true,  true,  false, false,   true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false);
			patternMatching.AddPattern("Ziffer B [0]", 1, true,  true,  true,  true,  true,  false, false,    true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  true,  true,  true,  true,  false, false,   true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  true,  true,  true,  true,  false, false);
			patternMatching.AddPattern("Ziffer C [0]", 2, false, true,  true,  true,  true,  false, false,    true,  false, false, false, false, true,  false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, true,  false,   false, true,  true,  true,  true,  false, false);
			patternMatching.AddPattern("Ziffer D [0]", 3, true,  true,  true,  true,  true,  false, false,    true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  true,  true,  true,  true,  false, false);
			patternMatching.AddPattern("Ziffer E [0]", 4, true,  true,  true,  true,  true,  false, false,    true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  true,  true,  true,  false, false, false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  true,  true,  true,  true,  false, false);
			patternMatching.AddPattern("Ziffer F [0]", 5, true,  true,  true,  true,  true,  false, false,    true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  true,  true,  true,  false, false, false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false);
			patternMatching.AddPattern("Ziffer G [0]", 6, false, true,  true,  true,  true,  false, false,    true,  false, false, false, false, true,  false,   true,  false, false, false, false, false, false,   true,  false, false, true,  true,  true,  false,   true,  false, false, false, false, true,  false,   true,  false, false, false, true,  true,  false,   false, true,  true,  true,  false, true,  false);
			patternMatching.AddPattern("Ziffer H [0]", 7, true,  false, false, false, false, true,  false,    true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  true,  true,  true,  true,  true,  false,   true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false);
			patternMatching.AddPattern("Ziffer I [0]", 8, true,  false, false, false, false, false, false,    true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false);
			patternMatching.AddPattern("Ziffer J [0]", 9, false, false, false, true,  false, false, false,    false, false, false, true,  false, false, false,   false, false, false, true,  false, false, false,   false, false, false, true,  false, false, false,   true,  false, false, true,  false, false, false,   true,  false, false, true,  false, false, false,   false, true,  true,  false, false, false, false);
			patternMatching.AddPattern("Ziffer K [0]",10, true,  false, false, false, true,  false, false,    true,  false, false, true,  false, false, false,   true,  false, true,  false, false, false, false,   true,  true,  false, false, false, false, false,   true,  false, true,  false, false, false, false,   true,  false, false, true,  false, false, false,   true,  false, false, false, true,  false, false);
			patternMatching.AddPattern("Ziffer L [0]",11, true,  false, false, false, false, false, false,    true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  true,  true,  true,  false, false, false);
			patternMatching.AddPattern("Ziffer M [0]",12, true,  true,  false, false, false, true,  true,     true,  true,  false, false, false, true,  true,    true,  false, true,  false, true,  false, true,    true,  false, true,  false, true,  false, true,    true,  false, false, true,  false, false, true,    true,  false, false, false, false, false, true,    true,  false, false, false, false, false, true);
			patternMatching.AddPattern("Ziffer N [0]",13, true,  false, false, false, false, false, true,     true,  true,  false, false, false, false, true,    true,  false, true,  false, false, false, true,    true,  false, false, true,  false, false, true,    true,  false, false, false, true,  false, true,    true,  false, false, false, false, true,  true,    true,  false, false, false, false, false, true);
			patternMatching.AddPattern("Ziffer O [0]",14, false, true,  true,  true,  true,  false, false,    true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   false, true,  true,  true,  true,  false, false);
			patternMatching.AddPattern("Ziffer P [0]",15, true,  true,  true,  true,  true,  false, false,    true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  true,  true,  true,  true,  false, false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  false, false, false, false, false, false);
			patternMatching.AddPattern("Ziffer Q [0]",16, false, true,  true,  true,  true,  false, false,    true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  false, false, true,  false, true,  false,   true,  false, false, false, true,  true,  false,   false, true,  true,  true,  true,  false, true);
			patternMatching.AddPattern("Ziffer R [0]",17, true,  true,  true,  true,  true,  false, false,    true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  true,  true,  true,  true,  false, false,   true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false,   true,  false, false, false, false, true,  false);
			patternMatching.AddPattern("Ziffer S [0]",18, false, true,  true,  true,  false, false, false,    true,  false, false, false, true,  false, false,   true,  false, false, false, false, false, false,   false, true,  true,  true,  false, false, false,   false, false, false, false, true,  false, false,   true,  false, false, false, true,  false, false,   false, true,  true,  true,  false, false, false);
			patternMatching.AddPattern("Ziffer T [0]",19, true,  true,  true,  true,  true,  false, false,    false, false, true,  false, false, false, false,   false, false, true,  false, false, false, false,   false, false, true,  false, false, false, false,   false, false, true,  false, false, false, false,   false, false, true,  false, false, false, false,   false, false, true,  false, false, false, false);
			patternMatching.AddPattern("Ziffer U [0]",20, true,  false, false, false, true,  false, false,    true,  false, false, false, true,  false, false,   true,  false, false, false, true,  false, false,   true,  false, false, false, true,  false, false,   true,  false, false, false, true,  false, false,   true,  false, false, false, true,  false, false,   false, true,  true,  true,  false, false, false);
			patternMatching.AddPattern("Ziffer V [0]",21, true,  false, false, false, true,  false, false,    true,  false, false, false, true,  false, false,   false, true,  false, true,  false, false, false,   false, true,  false, true,  false, false, false,   false, true,  false, true,  false, false, false,   false, false, true,  false, false, false, false,   false, false, true,  false, false, false, false);
			patternMatching.AddPattern("Ziffer W [0]",22, true,  false, false, false, false, false, true,     true,  false, false, false, false, false, true,    false, true,  false, false, false, true,  false,   false, true,  false, true,  false, true,  false,   false, true,  false, true,  false, true,  false,   false, false, true,  false, true,  false, false,   false, false, true,  false, true,  false, false);
			patternMatching.AddPattern("Ziffer X [0]",23, true,  false, false, false, false, false, true,     false, true,  false, false, false, true,  false,   false, false, true,  false, true,  false, false,   false, false, false, true,  false, false, false,   false, false, true,  false, true,  false, false,   false, true,  false, false, false, true,  false,   true,  false, false, false, false, false, true);
			patternMatching.AddPattern("Ziffer Y [0]",24, true,  false, false, false, false, false, true,     false, true,  false, false, false, true,  false,   false, false, true,  false, true,  false, false,   false, false, false, true,  false, false, false,   false, false, false, true,  false, false, false,   false, false, false, true,  false, false, false,   false, false, false, true,  false, false, false);
			patternMatching.AddPattern("Ziffer Z [0]",25, true,  true,  true,  true,  true,  false, false,    false, false, false, false, true,  false, false,   false, false, false, true,  false, false, false,   false, false, true,  false, false, false, false,   false, true,  false, false, false, false, false,   true,  false, false, false, false, false, false,   true,  true,  true,  true,  true,  false, false);

			patternMatching.SetOutputTitles("A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z");
			patternMatching.SelectAndActivatePattern(0);*/

			//DIGIT Matching Training:
			patternMatching = new PatternMatching(7,9,10);
			patternMatching.Location = new Point(0,0);
			Controls.Add(patternMatching);

			patternMatching.AddPattern("Ziffer 0 [0]", 0, false, false, true, true, true, false, false,  false, true, false, false, false, true, false,  true, false, false, false, false, false, true,  true, false, false, false, false, false, true,  true, false, false, false, false, false, true,  true, false, false, false, false, false, true,  true, false, false, false, false, false, true,  false, true, false, false, false, true, false,  false, false, true, true, true, false, false);
			patternMatching.AddPattern("Ziffer 1 [0]", 1, false, false, false, true, false, false, false,  false, false, true, true, false, false, false,  false, true, false, true, false, false, false,  true, false, false, true, false, false, false,  false, false, false, true, false, false, false,  false, false, false, true, false, false, false,  false, false, false, true, false, false, false,  false, false, false, true, false, false, false,  false, false, false, true, false, false, false);
			patternMatching.AddPattern("Ziffer 2 [0]", 2, false, true, true, true, true, true, false,  true, false, false, false, false, false, true,  true, false, false, false, false, false, true,  false, false, false, false, false, true, false,  false, false, false, false, true, false, false,  false, false, false, true, false, false, false,  false, false, true, false, false, false, false,  false, true, false, false, false, false, false,  true, true, true, true, true, true, true);
			patternMatching.AddPattern("Ziffer 3 [0]", 3, false, true, true, true, true, true, false,  true, false, false, false, false, false, true,  false, false, false, false, false, false, true,  false, false, false, false, false, false, true,  false, true, true, true, true, true, true,  false, false, false, false, false, false, true,  false, false, false, false, false, false, true,  true, false, false, false, false, false, true,  false, true, true, true, true, true, false);
			patternMatching.AddPattern("Ziffer 4 [0]", 4, false, false, false, false, true, false, false,  false, false, false, true, true, false, false,  false, false, true, false, true, false, false,  false, true, false, false, true, false, false,  true, false, false, false, true, false, false,  true, true, true, true, true, true, true,  false, false, false, false, true, false, false,  false, false, false, false, true, false, false,  false, false, false, false, true, false, false);
			patternMatching.AddPattern("Ziffer 5 [0]", 5, true, true, true, true, true, true, true,  true, false, false, false, false, false, false,  true, false, false, false, false, false, false,  true, false, false, false, false, false, false,  false, true, true, true, true, true, false,  false, false, false, false, false, false, true,  false, false, false, false, false, false, true,  true, false, false, false, false, false, true,  false, true, true, true, true, true, false);
			patternMatching.AddPattern("Ziffer 6 [0]", 6, false, true, true, true, true, true, true,  true, false, false, false, false, false, false,  true, false, false, false, false, false, false,  true, false, false, false, false, false, false,  true, true, true, true, true, true, false,  true, false, false, false, false, false, true,  true, false, false, false, false, false, true,  true, false, false, false, false, false, true,  false, true, true, true, true, true, false);
			patternMatching.AddPattern("Ziffer 7 [0]", 7, true, true, true, true, true, true, true, false, false, false, false, false, false, true, false, false, false, false, false, true, false, false, false, false, false, true, false, false, false, false, false, true, false, false, false, false, false, false, true, false, false, false, false, false, false, true, false, false, false, false, false, false, true, false, false, false, false, false, false, true, false, false, false);
			patternMatching.AddPattern("Ziffer 8 [0]", 8, false, true, true, true, true, true, false,  true, false, false, false, false, false, true,  true, false, false, false, false, false, true,  true, false, false, false, false, false, true,  false, true, true, true, true, true, false,  true, false, false, false, false, false, true,  true, false, false, false, false, false, true,  true, false, false, false, false, false, true,  false, true, true, true, true, true, false);
			patternMatching.AddPattern("Ziffer 9 [0]", 9, false, true, true, true, true, true, false, true, false, false, false, false, false, true, true, false, false, false, false, false, true, true, false, false, false, false, false, true, false, true, true, true, true, true, true, false, false, false, false, false, false, true, false, false, false, false, false, false, true, true, false, false, false, false, false, true, false, true, true, true, true, true, false);
			
            patternMatching.SetOutputTitles("0","1","2","3","4","5","6","7","8","9");
			patternMatching.SelectAndActivatePattern(0);

			//XOR Training:
			/*patternMatching = new PatternMatching(2,1,2);
			patternMatching.Location = new Point(0,0);
			Controls.Add(patternMatching);
			patternMatching.AddPattern("A1 B0",0,true,false);
			patternMatching.AddPattern("A0 B1",0,false,true);
			patternMatching.AddPattern("A0 B0",1,false,false);
			patternMatching.AddPattern("A1 B1",1,true,true);
			patternMatching.SetOutputTitles("TRUE","FALSE");
			patternMatching.SelectAndActivatePattern(0);*/

		}

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

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuNetwork = new System.Windows.Forms.MenuItem();
			this.menuReset = new System.Windows.Forms.MenuItem();
			this.menuConfig = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuPropagate = new System.Windows.Forms.MenuItem();
			this.menuBackpropagate = new System.Windows.Forms.MenuItem();
			this.menuAutoTrain = new System.Windows.Forms.MenuItem();
			this.menuAutoTrainPrimitive = new System.Windows.Forms.MenuItem();
			this.menuAbout = new System.Windows.Forms.MenuItem();
			this.menuInfo = new System.Windows.Forms.MenuItem();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuNetwork,
																					  this.menuAbout});
			// 
			// menuNetwork
			// 
			this.menuNetwork.Index = 0;
			this.menuNetwork.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.menuReset,
																						this.menuConfig,
																						this.menuItem1,
																						this.menuPropagate,
																						this.menuBackpropagate,
																						this.menuAutoTrain});
			this.menuNetwork.Text = "Network";
			// 
			// menuReset
			// 
			this.menuReset.Index = 0;
			this.menuReset.Text = "Reset";
			this.menuReset.Click += new System.EventHandler(this.menuReset_Click);
			// 
			// menuConfig
			// 
			this.menuConfig.Index = 1;
			this.menuConfig.Text = "Configuration";
			this.menuConfig.Click += new System.EventHandler(this.menuConfig_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 2;
			this.menuItem1.Text = "-";
			// 
			// menuPropagate
			// 
			this.menuPropagate.Index = 3;
			this.menuPropagate.Text = "Propagate";
			this.menuPropagate.Click += new System.EventHandler(this.menuPropagate_Click);
			// 
			// menuBackpropagate
			// 
			this.menuBackpropagate.Index = 4;
			this.menuBackpropagate.Text = "BackPropagate";
			this.menuBackpropagate.Click += new System.EventHandler(this.menuBackpropagate_Click);
			// 
			// menuAutoTrain
			// 
			this.menuAutoTrain.Index = 5;
			this.menuAutoTrain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.menuAutoTrainPrimitive});
			this.menuAutoTrain.Text = "Auto Train";
			// 
			// menuAutoTrainPrimitive
			// 
			this.menuAutoTrainPrimitive.Index = 0;
			this.menuAutoTrainPrimitive.Text = "Primitive";
			this.menuAutoTrainPrimitive.Click += new System.EventHandler(this.menuAutoTrainPrimitive_Click);
			// 
			// menuAbout
			// 
			this.menuAbout.Index = 1;
			this.menuAbout.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuInfo});
			this.menuAbout.Text = "About";
			// 
			// menuInfo
			// 
			this.menuInfo.Index = 0;
			this.menuInfo.Text = "Info";
			this.menuInfo.Click += new System.EventHandler(this.menuInfo_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(448, 344);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Menu = this.mainMenu1;
			this.Name = "MainForm";
			this.Text = "Pattern Matching Demo - NeuroBox Neural Network Library";

		}
		#endregion

		private void menuPropagate_Click(object sender, System.EventArgs e)
		{
			patternMatching.Propagate();
		}

		private void menuBackpropagate_Click(object sender, System.EventArgs e)
		{
			patternMatching.Backpropagate();
		}

		private void menuAutoTrainPrimitive_Click(object sender, System.EventArgs e)
		{
			if(patternMatching.AutoTrain())
				MessageBox.Show("successful");
			else
				MessageBox.Show("failed");
		}

		private void menuReset_Click(object sender, System.EventArgs e)
		{
			patternMatching.RebuildNetwork();
		}

		private void menuConfig_Click(object sender, System.EventArgs e)
		{
			/*ConfigurationEditor ce = new ConfigurationEditor();
			ce.BindToConfiguration(patternMatching.NeuralNetwork.Node);
			ce.Show();*/
		}

		private void menuInfo_Click(object sender, System.EventArgs e)
		{
			AboutInfo ai = new AboutInfo();
			ai.ShowDialog(this);
		}

	}
}
