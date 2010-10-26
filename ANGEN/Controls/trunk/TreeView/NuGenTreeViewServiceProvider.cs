/* -----------------------------------------------
 * NuGenTreeViewServiceProvider.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Collections;

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using System;

namespace Genetibase.Controls
{
	/// <summary>
	/// <para/>
	/// <see cref="T:INuGenTreeViewDragDropService"/><para/>
	/// <see cref="T:INuGenTreeViewSelectionService"/><para/>
	/// <see cref="T:INuGenTreeNodeSorter"/><para/>
	/// </summary>
	public class NuGenTreeViewServiceProvider : INuGenServiceProvider
	{
		#region INuGenServiceProvider Members

		/*
		 * GetService
		 */

		/// <summary>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public T GetService<T>() where T : class
		{
			return (T)this.GetService(typeof(T));	
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * DragService
		 */

		private INuGenTreeViewDragDropService dragService = null;

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

		private INuGenTreeViewSelectionService selectionService = null;

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

		private INuGenTreeNodeSorter sorter = null;

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
		protected virtual object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}

			if (serviceType == typeof(INuGenTreeViewDragDropService))
			{
				return this.DragService;
			}

			if (serviceType == typeof(INuGenTreeViewSelectionService))
			{
				return this.SelectionService;
			}

			if (serviceType == typeof(INuGenTreeNodeSorter))
			{
				return this.Sorter;
			}

			return null;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeViewServiceProvider"/> class.
		/// </summary>
		public NuGenTreeViewServiceProvider()
		{
		}

		#endregion
	}
}
