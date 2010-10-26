/* -----------------------------------------------
 * NuGenTreeViewServiceProvider.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.Collections;

using System;

namespace Genetibase.Shared.Controls.TreeViewInternals
{
	/// <summary>
	/// <para/>
	/// <see cref="INuGenTreeViewDragDropService"/><para/>
	/// <see cref="INuGenTreeViewSelectionService"/><para/>
	/// <see cref="INuGenTreeNodeSorter"/><para/>
	/// </summary>
	public class NuGenTreeViewServiceProvider : NuGenServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * DragService
		 */

		private INuGenTreeViewDragDropService dragService;

		/// <summary>
		/// </summary>
		protected virtual INuGenTreeViewDragDropService DragService
		{
			get
			{
				if (this.dragService == null)
				{
					this.dragService = new NuGenTreeViewDragDropService();
				}

				return this.dragService;
			}
		}

		/*
		 * SelectionService
		 */

		private INuGenTreeViewSelectionService selectionService;

		/// <summary>
		/// </summary>
		protected virtual INuGenTreeViewSelectionService SelectionService
		{
			get
			{
				if (this.selectionService == null)
				{
					this.selectionService = new NuGenTreeViewSelectionService();
				}

				return this.selectionService;
			}
		}

		/*
		 * Sorter
		 */

		private INuGenTreeNodeSorter sorter;

		/// <summary>
		/// </summary>
		protected virtual INuGenTreeNodeSorter Sorter
		{
			get
			{
				if (this.sorter == null)
				{
					this.sorter = new NuGenTreeNodeSorter();
				}

				return this.sorter;
			}
		}

		#endregion

		#region Methods.Protected.Virtual

		/*
		 * GetService
		 */

		/// <summary>
		/// </summary>
		/// <param name="serviceType"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="serviceType"/> is <see langword="null"/>.
		/// </exception>
		protected override object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}

			if (serviceType == typeof(INuGenTreeViewDragDropService))
			{
				return this.DragService;
			}
			else if (serviceType == typeof(INuGenTreeViewSelectionService))
			{
				return this.SelectionService;
			}
			else if (serviceType == typeof(INuGenTreeNodeSorter))
			{
				return this.Sorter;
			}

			return base.GetService(serviceType);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeViewServiceProvider"/> class.
		/// </summary>
		public NuGenTreeViewServiceProvider()
		{
		}
	}
}
