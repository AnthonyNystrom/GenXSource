/* -----------------------------------------------
 * NuGenTreeViewDragDropService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// Provides service methods for drag-n-drop operations.
	/// </summary>
	public class NuGenTreeViewDragDropService : INuGenTreeViewDragDropService
	{
		#region Properties.Public

		/*
		 * EdgeSensivity
		 */

		private NuGenInt32 edgeSensivity = new NuGenInt32(0, int.MaxValue);

		/// <summary>
		/// Gets or sets the offset that is used to determine the drop position: after the node, before the
		/// node, or inside the node.
		/// </summary>
		/// <exception cref="T:System.ArgumentException"><paramref name="value"/> should not be negative.</exception>
		public int EdgeSensivity
		{
			get
			{
				return this.edgeSensivity.Value;
			}
			set
			{
				try
				{
					this.edgeSensivity.Value = value;
				}
				catch (ArgumentException e)
				{
					throw e;
				}
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * GetDropPosition
		 */

		/// <summary>
		/// Determines the drop position: after the node, before the node, or inside the node. If this
		/// operation fails to determine the drop position, <see cref="T:NuGenDropPosition.Nowhere"/> is
		/// returned.
		/// </summary>
		/// <param name="targetTreeNode"></param>
		/// <param name="cursorPosition">Client coordinates.</param>
		/// <returns></returns>
		public NuGenDropPosition GetDropPosition(NuGenTreeNode targetTreeNode, Point cursorPosition)
		{
			if (targetTreeNode == null)
			{
				return NuGenDropPosition.Nowhere;
			}

			Rectangle targetBounds = targetTreeNode.Bounds;

			if (
				(cursorPosition.Y >= targetBounds.Top + this.EdgeSensivity)
				&& (cursorPosition.Y <= targetBounds.Bottom - this.EdgeSensivity)
				)
			{
				return NuGenDropPosition.Inside;
			}
			else if (
				(cursorPosition.Y >= targetBounds.Top)
				&& (cursorPosition.Y <= targetBounds.Top + this.EdgeSensivity)
				)
			{
				return NuGenDropPosition.Before;
			}
			else if (
				(cursorPosition.Y >= targetBounds.Bottom - this.EdgeSensivity)
				&& (cursorPosition.Y <= targetBounds.Bottom)
				)
			{
				return NuGenDropPosition.After;
			}

			return NuGenDropPosition.Nowhere;
		}

		#endregion

		#region Methods.Public.Virtual

		/*
		 * DoDrop
		 */

		/// <summary>
		/// Invokes service specific nodes merge operation.
		/// </summary>
		public virtual void DoDrop(
			NuGenTreeNode targetTreeNode,
			List<NuGenTreeNode> selectedNodes,
			NuGenDropPosition dropPosition
			)
		{
			return;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeViewDragDropService"/> class.
		/// </summary>
		public NuGenTreeViewDragDropService()
			: this(2)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeViewDragDropService"/> class.
		/// </summary>
		/// <param name="edgeSensivity">Specifies the offset that is used to determine the drop position:
		/// after the node, before the node, or inside the node.</param>
		/// <exception cref="T:System.ArgumentException"><paramref name="edgeSensivity"/> should not be
		/// negative.</exception>
		public NuGenTreeViewDragDropService(int edgeSensivity)
		{
			try
			{
				this.EdgeSensivity = edgeSensivity;
			}
			catch (ArgumentException e)
			{
				throw e;
			}
		}

		#endregion
	}
}
