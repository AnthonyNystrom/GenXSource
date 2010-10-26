/* -----------------------------------------------
 * NuGenUISnap.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// A window that sticks to other windows of the same type when moved or resized.
	/// You get a nice way of organizing multiple top-level windows.
	/// Quite similar to WinAmp 2.x style of sticking windows.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenUISnap), "Resources.NuGenIcon.png")]
	[DefaultProperty("StickGap")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenUISnapDesigner")]
	[NuGenSRDescription("Description_UISnap")]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenUISnap : Component
	{
		private Form _hostForm;

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public Form HostForm
		{
			get
			{
				return _hostForm;
			}
			set
			{
				if (_hostForm != value)
				{
					if (_wndManager != null && !_wndManager.IsDisposed)
					{
						_wndManager.Dispose();
					}

					_hostForm = value;

					if (!this.DesignMode)
					{
						if (_hostForm != null)
						{
							_wndManager = new WndManager(_hostForm, this);
						}
					}
				}
			}
		}

		#region Properties.Behavior

		private int _stickGap = 20;

		/// <summary>
		/// Gets or sets the distance in pixels between two forms or a form and the screen where the sticking should start.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(20)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_UISnap_StickGap")]
		public int StickGap
		{
			get
			{
				return _stickGap;
			}
			set
			{
				_stickGap = value;
			}
		}

		private bool _stickOnResize = true;

		/// <summary>
		/// Gets or sets the value indicating whether the host form can stick while resizing.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_UISnap_StickOnResize")]
		public bool StickOnResize
		{
			get
			{
				return _stickOnResize;
			}
			set
			{
				_stickOnResize = value;
			}
		}

		private bool _stickOnMove = true;

		/// <summary>
		/// Gets or sets the value indicating whether the host form can stick while moving.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_UISnap_StickOnMove")]
		public bool StickOnMove
		{
			get
			{
				return _stickOnMove;
			}
			set
			{
				_stickOnMove = value;
			}
		}

		private bool _stickToScreen = true;

		/// <summary>
		/// Gets or sets the value indicating whether the host form can stick to screen margins.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_UISnap_StickToScreen")]
		public bool StickToScreen
		{
			get
			{
				return _stickToScreen;
			}
			set
			{
				_stickToScreen = value;
			}
		}

		private bool _stickToOther = true;

		/// <summary>
		/// Gets or sets the value indicating whether the host form can stick to other stick windows.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_UISnap_StickToOther")]
		public bool StickToOther
		{
			get
			{
				return _stickToOther;
			}
			set
			{
				_stickToOther = value;
			}
		}

		#endregion

		/// <summary>
		/// Register a new form as an external reference form.
		/// All Sticky windows will try to stick to the external references.
		/// Use this to register your MainFrame so the child windows try to stick to it, when your MainFrame is NOT a sticky window.
		/// </summary>
		/// <param name="externalForm">External window that will be used as reference.</param>
		public static void RegisterExternalReferenceForm(Form externalForm)
		{
			_snaps.Add(externalForm);
		}

		/// <summary>
		/// Unregister a form from the external references.
		/// <see cref="RegisterExternalReferenceForm"/>
		/// </summary>
		/// <param name="externalForm">External window that will was used as reference.</param>
		public static void UnregisterExternalReferenceForm(Form externalForm)
		{
			_snaps.Remove(externalForm);
		}

		private WndManager _wndManager;
		private static ArrayList _snaps = new ArrayList();

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenUISnap"/> class.
		/// </summary>
		public NuGenUISnap()
		{
		}
	}
}
