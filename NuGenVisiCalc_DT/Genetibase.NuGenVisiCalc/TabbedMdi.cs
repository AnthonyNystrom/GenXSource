/* -----------------------------------------------
 * TabbedMdi.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.Shared.Collections;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.SmoothControls;

namespace Genetibase.NuGenVisiCalc
{
    internal partial class TabbedMdi : NuGenSmoothTabControl
	{
		#region Events

		/*
		 * CanvasClosing
		 */

		private static readonly Object _canvasClosing = new Object();

		public event EventHandler<MdiTabCloseEventArgs> CanvasClosing
		{
			add
			{
				Events.AddHandler(_canvasClosing, value);
			}
			remove
			{
				Events.RemoveHandler(_canvasClosing, value);
			}
		}

		private void OnCanvasClosing(MdiTabCloseEventArgs e)
		{
			Initiator.InvokeEventHandlerT<MdiTabCloseEventArgs>(_canvasClosing, e);
		}

		/*
		 * StateChanged
		 */

		private static readonly Object _stateChanged = new Object();

		public event EventHandler<MdiStateEventArgs> StateChanged
		{
			add
			{
				Events.AddHandler(_stateChanged, value);
			}
			remove
			{
				Events.RemoveHandler(_stateChanged, value);
			}
		}

		private void OnStateChanged(MdiStateEventArgs e)
		{
			Initiator.InvokeEventHandlerT<MdiStateEventArgs>(_stateChanged, e);
		}

		#endregion

		#region Properties.Public

		public Canvas ActiveCanvas
		{
			get
			{
				if (SelectedTab != null)
				{
					foreach (Control ctrl in SelectedTab.Controls)
					{
						Canvas canvas = ctrl as Canvas;

						if (canvas != null)
						{
							return canvas;
						}
					}
				}

				return null;
			}
		}

		public Boolean IsEmpty
		{
			get
			{
				Boolean containsBlankPage = TabPages.Contains(BlankPage);
				Boolean notEnoughPages = TabPages.Count < 1;
				return notEnoughPages || containsBlankPage;
			}
		}

		#endregion

		#region Properties.Private

		/*
         * BlankPage
         */

        private BlankTabPage _blankPage;

        private BlankTabPage BlankPage
        {
            get
            {
                if (_blankPage == null || _blankPage.IsDisposed)
                {
                    _blankPage = new BlankTabPage();
                }

                return _blankPage;
            }
        }

        /*
         * ContentPageDictionary
         */

		private Dictionary<Control, NuGenTabPage> _contentPageDictionary;

        private Dictionary<Control, NuGenTabPage> ContentPageDictionary
        {
            get
            {
                if (_contentPageDictionary == null)
                {
                    _contentPageDictionary = new Dictionary<Control, NuGenTabPage>();
                }

                return _contentPageDictionary;
            }
        }

        #endregion

        #region Methods.Public

        /*
		 * AddTabPage
		 */

        /// <summary>
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <para>
        ///		<paramref name="pageContent"/> is <see langword="null"/>.
        /// </para>
        /// -or-
        /// <para>
        ///		<paramref name="text"/> is <see langword="null"/>.
        /// </para>
        /// </exception>
        public NuGenTabPage AddTabPage(Canvas canvas, String text, Image tabButtonImage)
        {
            if (canvas == null)
            {
                throw new ArgumentNullException("canvas");
            }

            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            NuGenTabPage tabPage = TabPages.Add(text, tabButtonImage);
            tabPage.Controls.Add(canvas);
			canvas.ParentTabPage = tabPage;
            return tabPage;
        }

		/*
		 * FindCanvasOnTabPage
		 */

		public Canvas FindCanvasOnTabPage(NuGenTabPage tabPage)
		{
			Canvas canvas = null;

			foreach (Control ctrl in tabPage.Controls)
			{
				if (ctrl is Canvas)
				{
					canvas = (Canvas)ctrl;
					break;
				}
			}

			return canvas;
		}

        #endregion

        #region Methods.Protected.Overriden

		/*
		 * OnTabCloseButtonClick
		 */

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Controls.NuGenTabbedMdi.TabCloseButtonClick"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTabCloseButtonClick(NuGenTabCancelEventArgs e)
		{
			base.OnTabCloseButtonClick(e);
			e.Cancel = (e.TabPage == BlankPage) ? true : false;

			if (!e.Cancel)
			{
				MdiTabCloseEventArgs tabCloseEventArgs = new MdiTabCloseEventArgs(FindCanvasOnTabPage(e.TabPage));
				OnCanvasClosing(tabCloseEventArgs);
				e.Cancel = tabCloseEventArgs.Cancel;
			}
		}

		/*
		 * OnTabPageAdded
		 */

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Controls.NuGenTabbedMdi.TabPageAdded"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTabPageAdded(NuGenCollectionEventArgs<NuGenTabPage> e)
		{
			base.OnTabPageAdded(e);

			if (TabPages.Count > 1 && TabPages.Contains(BlankPage))
			{
				TabPages.Remove(BlankPage);
			}

			OnStateChanged(new MdiStateEventArgs(IsEmpty));
		}

		/*
		 * OnTabPageRemoved
		 */

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Controls.NuGenTabbedMdi.TabPageRemoved"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTabPageRemoved(NuGenCollectionEventArgs<NuGenTabPage> e)
		{
			base.OnTabPageRemoved(e);

			if (TabPages.Count < 1)
			{
				TabPages.Add(BlankPage);
			}

			OnStateChanged(new MdiStateEventArgs(IsEmpty));
		}

		#endregion

		public TabbedMdi()
		{
			TabPages.Add(BlankPage);
		}
    }
}
