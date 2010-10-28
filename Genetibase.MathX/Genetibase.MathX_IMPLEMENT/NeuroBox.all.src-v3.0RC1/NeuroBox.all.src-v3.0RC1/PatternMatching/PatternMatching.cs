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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using NeuroBox;
using NeuroBox.PatternMatching;
using NeuroBox.PatternMatching.Grid2D;

namespace NeuroBox.PatternMatchingDemo
{
	public class PatternMatching : System.Windows.Forms.UserControl
	{
		private NeuroBox.PatternMatchingDemo.OutputStatistics outputStat;
		private NeuroBox.PatternMatchingDemo.CheckboxGrid inputGrid;

		private System.Windows.Forms.GroupBox boxInput;
		private System.Windows.Forms.GroupBox boxOutput;
		private System.Windows.Forms.Button btnTrain;
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.Button btnShow;
		private System.Windows.Forms.ComboBox patternSelect;

		private System.ComponentModel.Container components = null;
		
		private Grid2DControler pm;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Button btnAutoTrain;
		private string[] outputTitles = null;

		public PatternMatching()
		{
			InitializeComponent();
		}

		public PatternMatching(int x, int y, int output)
		{
			InitializeComponent();

			ProgressBegin();

			pm = new Grid2DControler(x,y,output);

            BasicConfig bc = pm.BasicConfiguration;
            TrainingConfig tc = pm.TrainingConfiguration;
            Grid2DConfig gc = new Grid2DConfig(bc.Node);

            bc.ActivationType.Value = EActivationType.Symmetric;
            bc.BiasNeuronEnable.Value = true;
            bc.BiasNeuronOutput.Value = 0.9d;
            bc.DeadNeuronDecayEnabled.Value = false;
            bc.FlatspotEliminationEnable.Value = true;
            bc.FlatspotElimination.Value = 0.05d;
            bc.InitialSymmetryBreaking.Value = 0.2d;
            bc.LearningRate.Value = 0.3d;
            bc.ManhattanTrainingEnable.Value = false;
            bc.MomentumTermEnable.Value = false;
            bc.SymmetryPreventionEnable.Value = true;
            bc.SymmetryPrevention.Value = 0.05d;
            bc.WeightDecayEnable.Value = true;
            bc.WeightDecay.Value = 0.01d;

            bc.LowInput.Value = -3d;
            bc.HighInput.Value = 3d;
            bc.LowOutput.Value = -0.95d;
            bc.HighOutput.Value = 0.95d;

            tc.AutoTrainingAttempts.Value = 1;
            tc.AutoTrainingEpochs.Value = 400;
            tc.AutoTrainingPercentSuccessful.Value = 1.0d;
            tc.ShuffleEnable.Value = true;  // make it more robust for noise
            tc.ShuffleNoiseSigma.Value = 0.2d;
            tc.ShuffleSwapProbability.Value = 0.2d; // 0.2 = every 5ht pixel is swapped (20% probability) !!

            gc.All2AllEnable.Value = false;
            gc.HorizontalLinesEnable.Value = true;
            gc.VerticalLinesEnable.Value = true;
            gc.RingsEnable.Value = true;
            gc.LittleSquaresEnable.Value = true;

			pm.BuildNetwork(true);

            //pm.NetworkTrainer = new SimpleTrainer();
            //pm.NetworkTrainer = new OpenloopTrainer();
            //pm.NetworkTrainer = new ConditionalTrainer();
            pm.NetworkTrainer = new FeedbackTrainer();

			ProgressStatus(50);

			pm.Patterns.PatternAdded += pm_OnNewPatternAdded;
			pm.PatternSelectionChanged += pm_OnPatternSelectionChanged;
			pm.InputChanged += pm_OnInputChanged;
			pm.NetworkRebuilt += pm_OnNetworkRebuilt;

			outputStat.Init(pm);
			inputGrid.Init(pm);
			inputGrid.OnGridChanged += inputGrid_OnGridChanged;

			ProgressEnd();
		}

		public void AddPattern(string title, int output, params bool[] input)
		{
			pm.Patterns.Add(title,output,input);
		}

		public void SetOutputTitles(params string[] titles)
		{
			this.outputTitles = titles;
			pm.SetOutputTitles(titles);
		}

		public Network NeuralNetwork
		{
			get {return pm.NeuralNetwork;}
		}

        public Grid2DControler Controler
        {
            get { return pm; }
        }

		public void RebuildNetwork()
		{
			pm.RebuildNetwork();
		}

		private void pm_OnInputChanged(object sender, EventArgs e)
		{
			Propagate();
		}

		private void pm_OnPatternSelectionChanged(object sender, PatternPositionEventArgs e)
		{
			patternSelect.SelectedIndex = e.Position;
		}

		private void pm_OnNewPatternAdded(object sender, PatternEventArgs e)
		{
			patternSelect.Items.Add(e.Pattern.Title);
		}

		private void inputGrid_OnGridChanged(object sender, EventArgs e)
		{
			pm.PushInput(inputGrid.BulkGet());
		}

		private void pm_OnNetworkRebuilt(object sender, EventArgs e)
		{
			pm.SetOutputTitles(outputTitles);
			outputStat.Init(pm);
		}

		public void Backpropagate()
		{
			pm.TrainCurrentNetwork();
		}

		public void Propagate()
		{
			pm.CalculateCurrentNetwork();
		}

		public void SelectAndActivatePattern(int pattern)
		{
			pm.SelectPattern(pattern);
		}

		public bool AutoTrain()
		{
			//btnAutoTrain.Text = "Abort";
			ProgressBegin();

			bool rsp = pm.AutoTrainNetwork(new Progress(ProgressStatus));
			pm.SelectPattern(0);

			//btnAutoTrain.Text = "Auto Train";
			ProgressEnd();

			return rsp;
		}

		public void ProgressBegin()
		{
			progressBar.Value = 0;
		}

		public void ProgressEnd()
		{
			progressBar.Value = 100;
		}

		public void ProgressStatus(int status)
		{
			progressBar.Value = status;
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

		#region Vom Komponenten-Designer generierter Code
		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.boxInput = new System.Windows.Forms.GroupBox();
			this.inputGrid = new NeuroBox.PatternMatchingDemo.CheckboxGrid();
			this.outputStat = new NeuroBox.PatternMatchingDemo.OutputStatistics();
			this.boxOutput = new System.Windows.Forms.GroupBox();
			this.btnTrain = new System.Windows.Forms.Button();
			this.btnTest = new System.Windows.Forms.Button();
			this.btnShow = new System.Windows.Forms.Button();
			this.patternSelect = new System.Windows.Forms.ComboBox();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.btnAutoTrain = new System.Windows.Forms.Button();
			this.boxInput.SuspendLayout();
			this.boxOutput.SuspendLayout();
			this.SuspendLayout();
			// 
			// boxInput
			// 
			this.boxInput.Controls.Add(this.inputGrid);
			this.boxInput.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.boxInput.Location = new System.Drawing.Point(8, 8);
			this.boxInput.Name = "boxInput";
			this.boxInput.Size = new System.Drawing.Size(128, 168);
			this.boxInput.TabIndex = 2;
			this.boxInput.TabStop = false;
			this.boxInput.Text = "Input";
			// 
			// inputGrid
			// 
			this.inputGrid.AutoScroll = true;
			this.inputGrid.Location = new System.Drawing.Point(8, 16);
			this.inputGrid.Name = "inputGrid";
			this.inputGrid.Size = new System.Drawing.Size(112, 144);
			this.inputGrid.TabIndex = 0;
			// 
			// outputStat
			// 
			this.outputStat.AutoScroll = true;
			this.outputStat.AutoScrollMinSize = new System.Drawing.Size(250, 150);
			this.outputStat.Location = new System.Drawing.Point(8, 16);
			this.outputStat.Name = "outputStat";
			this.outputStat.Size = new System.Drawing.Size(275, 280);
			this.outputStat.TabIndex = 3;
			// 
			// boxOutput
			// 
			this.boxOutput.Controls.Add(this.outputStat);
			this.boxOutput.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.boxOutput.Location = new System.Drawing.Point(144, 8);
			this.boxOutput.Name = "boxOutput";
			this.boxOutput.Size = new System.Drawing.Size(296, 304);
			this.boxOutput.TabIndex = 4;
			this.boxOutput.TabStop = false;
			this.boxOutput.Text = "Output";
			// 
			// btnTrain
			// 
			this.btnTrain.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnTrain.Location = new System.Drawing.Point(8, 216);
			this.btnTrain.Name = "btnTrain";
			this.btnTrain.Size = new System.Drawing.Size(128, 23);
			this.btnTrain.TabIndex = 5;
			this.btnTrain.Text = "Train";
			this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
			// 
			// btnTest
			// 
			this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnTest.Location = new System.Drawing.Point(8, 280);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(128, 24);
			this.btnTest.TabIndex = 6;
			this.btnTest.Text = "Test";
			this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// btnShow
			// 
			this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnShow.Location = new System.Drawing.Point(8, 312);
			this.btnShow.Name = "btnShow";
			this.btnShow.Size = new System.Drawing.Size(128, 24);
			this.btnShow.TabIndex = 7;
			this.btnShow.Text = "Show Network";
			this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
			// 
			// patternSelect
			// 
			this.patternSelect.Location = new System.Drawing.Point(8, 184);
			this.patternSelect.Name = "patternSelect";
			this.patternSelect.Size = new System.Drawing.Size(128, 21);
			this.patternSelect.TabIndex = 8;
			this.patternSelect.SelectedIndexChanged += new System.EventHandler(this.patternSelect_SelectedIndexChanged);
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(144, 320);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(296, 16);
			this.progressBar.TabIndex = 9;
			// 
			// btnAutoTrain
			// 
			this.btnAutoTrain.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnAutoTrain.Location = new System.Drawing.Point(8, 248);
			this.btnAutoTrain.Name = "btnAutoTrain";
			this.btnAutoTrain.Size = new System.Drawing.Size(128, 23);
			this.btnAutoTrain.TabIndex = 5;
			this.btnAutoTrain.Text = "Auto Train";
			this.btnAutoTrain.Click += new System.EventHandler(this.btnAutoTrain_Click);
			// 
			// PatternMatching
			// 
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.patternSelect);
			this.Controls.Add(this.btnShow);
			this.Controls.Add(this.btnTest);
			this.Controls.Add(this.btnTrain);
			this.Controls.Add(this.boxOutput);
			this.Controls.Add(this.boxInput);
			this.Controls.Add(this.btnAutoTrain);
			this.Name = "PatternMatching";
			this.Size = new System.Drawing.Size(448, 344);
			this.boxInput.ResumeLayout(false);
			this.boxOutput.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnTrain_Click(object sender, System.EventArgs e)
		{
			Backpropagate();
			Propagate();
		}

		private void btnTest_Click(object sender, System.EventArgs e)
		{
			Propagate();
		}

		private void btnShow_Click(object sender, System.EventArgs e)
		{
			DesignForm form = new DesignForm();
			form.RenderNetwork(pm.NeuralNetwork);
			form.Show();
		}

		private void patternSelect_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			pm.SelectPattern(patternSelect.SelectedIndex);
		}

		private void btnAutoTrain_Click(object sender, System.EventArgs e)
		{
			if(AutoTrain())
				MessageBox.Show("successful");
			else
				MessageBox.Show("failed");
		}
	}
}
