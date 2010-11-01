/* -----------------------------------------------
 * NuGenTabbedMdi.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Design;
using Genetibase.Controls.Properties;
using Genetibase.Shared.Collections;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// </summary>
	[Designer(typeof(NuGenTabbedMdiDesigner))]
	[ToolboxItem(true)]
	public class NuGenTabbedMdi : NuGenTabControl
	{
		#region Properties.Public

		/*
		 * FlatStyle
		 */

		/// <summary>
		/// </summary>
		/// <value></value>
		[DefaultValue(FlatStyle.Flat)]
		public new FlatStyle FlatStyle
		{
			get
			{
				return base.FlatStyle;
			}
			set
			{
				base.FlatStyle = value;
			}
		}

		#endregion

		#region Properties.Protected

		/*
		 * BlankPage
		 */

		private NuGenTabPage _blankTabPage = null;

		/// <summary>
		/// </summary>
		protected NuGenTabPage BlankTabPage
		{
			get
			{
				if (
					_blankTabPage == null
					|| _blankTabPage.IsDisposed
					)
				{
					_blankTabPage = new NuGenBlankTabPage();
				}

				return _blankTabPage;
			}
		}
		
		/*
		 * ContentPageDictionary
		 */

		private Dictionary<Control, NuGenTabPage> _contentPageDictionary = null;

		/// <summary>
		/// </summary>
		protected Dictionary<Control, NuGenTabPage> ContentPageDictionary
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
		public NuGenTabPage AddTabPage(Control pageContent, string text, Image tabButtonImage)
		{
			if (pageContent == null)
			{
				throw new ArgumentNullException("pageContent");
			}

			if (text == null)
			{
				throw new ArgumentNullException("text");
			}

			NuGenTabPage tabPage = this.TabPages.Add(text, tabButtonImage);
			tabPage.Controls.Add(pageContent);
			return tabPage;
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
			e.Cancel = (e.TabPage == this.BlankTabPage)
				? true
				: false
				;
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

			if (
				this.TabPages.Count > 1
				&& this.TabPages.Contains(_blankTabPage)
				)
			{
				this.TabPages.Remove(_blankTabPage);
			}
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

			if (this.TabPages.Count < 1)
			{
				this.TabPages.Add(_blankTabPage);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabbedMdi"/> class.
		/// </summary>
		public NuGenTabbedMdi()
		{
			this.FlatStyle = FlatStyle.Flat;
			this.TabPages.Add(this.BlankTabPage);
		}

		#endregion
	}
}
