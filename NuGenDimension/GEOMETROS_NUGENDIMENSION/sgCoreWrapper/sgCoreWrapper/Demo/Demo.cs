using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using sgCoreWrapper.Objects;
using sgCoreWrapper.Structs;
using sgCoreWrapper;
using Demo.Scenes;

namespace Demo
{
	public partial class Demo : Form
	{
		public Demo()
		{
			InitializeComponent();

			msgCore.InitKernel();
			msg3DObject.AutoTriangulate(true, msgTriangulationTypeEnum.SG_DELAUNAY_TRIANGULATION);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			msgCore.FreeKernel(true);
		}

		private void boxesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Primitives.CreateBoxes();
			Invalidate();
		}

		private void spheresToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Primitives.CreateSpheres();
			Invalidate();
		}

		private void cylindersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Primitives.CreateCylinders();
			Invalidate();
		}

		private void conesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Primitives.CreateCones();
			Invalidate();
		}

		private void torsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Primitives.CreateTors();
			Invalidate();
		}

		private void ellipsoidsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Primitives.CreateEllipsoids();
			Invalidate();
		}

		private void bandsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Primitives.CreateBands();
			Invalidate();
		}

		private void intersectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Bool.CreateIntersection();
			Invalidate();
		}

		private void unionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Bool.CreateUnion();
			Invalidate();
		}

		private void subToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Bool.CreateSub();
			Invalidate();
		}

		private void intersectionContoursToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Bool.CreateIntersectionContours();
			Invalidate();
		}

		private void sectionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Bool.CreateSections();
			Invalidate();
		}

		private void rotationSurfaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Kinematic.CreateRotationSurface();
			Invalidate();
		}

		private void rotationBodyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Kinematic.CreateRotationBody();
			Invalidate();
		}

		private void extrudeSurfaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Kinematic.CreateExtrudeSurfaces();
			Invalidate();
		}

		private void extrudeBodyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Kinematic.CreateExtrudeBody();
			Invalidate();
		}

		private void spiralSurfaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Kinematic.CreateSpiralSurface();
			Invalidate();
		}

		private void spiralBodyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Kinematic.CreateSpiralBody();
			Invalidate();
		}

		private void pipeSurfaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Kinematic.CreatePipeSurface();
			Invalidate();
		}

		private void pipeBodyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Kinematic.CreatePipeBody();
			Invalidate();
		}

		private void meshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Surface.CreateMesh();
			Invalidate();
		}

		private void faceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Surface.CreateFace();
			Invalidate();
		}

		private void coonsFrom3CurvesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Surface.CreateCoonsFrom3Curves();
			Invalidate();
		}

		private void coonsFrom4CurvesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Surface.CreateCoonsFrom4Curves();
			Invalidate();
		}

		private void linearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Surface.CreateLinear();
			Invalidate();
		}

		private void fromClipsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Surface.CreateFromClips();
			Invalidate();
		}

		private void bodyFromClipsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Surface.CreateBodyFromClips();
			Invalidate();
		}
	}
}