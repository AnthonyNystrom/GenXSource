using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using sgCoreWrapper.Structs;
using sgCoreWrapper.Objects;
using sgCoreWrapper.Interfaces;

namespace Demo
{
	public partial class DemoOpenGLControl : OpenGLControl
	{
		public DemoOpenGLControl()
		{
			InitializeComponent();
		}

		private Camera _camera = new Camera();
		public Camera Camera
		{
			get { return _camera; }
		}

		public void FitToScene()
		{
			msgPointStruct a1 = new msgPointStruct();
			msgPointStruct a2 = new msgPointStruct();

			msgScene.GetScene().GetGabarits(a1, a2);
			_camera.FitBounds(a1.x, a1.y, a1.z, a2.x, a2.y, a2.z);
		}

		private void DemoOpenGLControl_Paint(object sender, PaintEventArgs e)
		{
			ActivateContext();

			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

			// Set the matrix mode to MODELVIEW and load
			// the identity matrix
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();

			glTranslated(_fTransX, _fTransY, _fTransZ);
			glRotated(_fAngleX, 1, 0, 0);
			glRotated(_fAngleY, 0, 1, 0);
			glRotated(_fAngleZ, 0, 0, 1);

//			Camera.PositionCamera();

			// Start rendering...
			DrawScene();

			glFlush();
			glFinish();

			SwapBuffers();
			DeactivateContext();
		}

		protected override void OnInitScene()
		{
			base.OnInitScene();

			float[] diffuseProperties = { 1.0f, 1.0f, 1.0f, 1.0f };
			float[] specularProperties = { 1.0f, 1.0f, 1.0f, 1.0f };

			glLightfv(GL_LIGHT0, GL_DIFFUSE, diffuseProperties);
			glLightfv(GL_LIGHT0, GL_SPECULAR, specularProperties);
			glLightModeli(GL_LIGHT_MODEL_TWO_SIDE, 1);

			glClearColor((float)BackColor.R / 255, (float)BackColor.G / 255, (float)BackColor.B / 255, (float)BackColor.A / 255);

			glHint(GL_LINE_SMOOTH_HINT, GL_FASTEST);

			glEnable(GL_TEXTURE_2D);
			glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
			glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);

			glEnable(GL_LIGHTING);
			glEnable(GL_LIGHT0);

			glEnable(GL_BLEND);
			glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

			glEnable(GL_DEPTH_TEST);
		}

		private void DrawScene()
		{
			msgScene scene = msgScene.GetScene();
			mIObjectsList objList = scene.GetObjectsList();
			msgObject curObj = objList.GetHead();
			while (curObj != null)
			{
				Painter.DrawObject(Painter.RenderingMode.GL_RENDER, curObj, false, false);

				curObj = objList.GetNext(curObj);
			}
		}

		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseDown(e);

			_ptCursorPos.X = e.X;
			_ptCursorPos.Y = e.Y;
		}

		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (MouseButtons != MouseButtons.Left)
				return;

			float _fShiftDY = (float)(e.Y - _ptCursorPos.Y) / 4f;
			float _fShiftDX = (float)(e.X - _ptCursorPos.X) / 4f;

			float a = Math.Abs(_fAngleX);
			if (90f < a && a < 270f)
			{
				_fShiftDX = -_fShiftDX;
			}

			_fAngleX += _fShiftDY;
			_fAngleY += _fShiftDX;

			_ptCursorPos.X = e.X;
			_ptCursorPos.Y = e.Y;

			Invalidate();
		}

		private Point _ptCursorPos = new Point();
		private float _fTransX = 0, _fTransY = 0, _fTransZ = -16;
		private float _fAngleX = 30;
		private float _fAngleY = 30;
		private float _fAngleZ = 30;
	}
}
