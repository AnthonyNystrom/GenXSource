/* -----------------------------------------------
 * NuGenTaskServiceProvider.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Text;

using System;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// <para/>
	/// <see cref="INuGenTreeViewDragDropService"/><para/>
	/// <see cref="INuGenTreeViewSelectionService"/><para/>
	/// <see cref="INuGenTreeNodeSorter"/><para/>
	/// <see cref="INuGenImageListService"/><para/>
	/// <see cref="INuGenTaskXmlService"/><para/>
	/// <see cref="INuGenStringProcessor"/><para/>
	/// </summary>
	public class NuGenTaskServiceProvider : NuGenTreeViewServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * ImageService
		 */

		private INuGenImageListService imageListService = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenImageListService ImageListService
		{
			get
			{
				if (this.imageListService == null)
				{
					this.imageListService = new NuGenImageListService();
				}

				return this.imageListService;
			}
		}

		/*
		 * StringProcessor
		 */

		private INuGenStringProcessor stringProcessor = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenStringProcessor StringProcessor
		{
			get
			{
				if (this.stringProcessor == null)
				{
					this.stringProcessor = new NuGenStringProcessor();
				}

				return this.stringProcessor;
			}
		}

		/*
		 * XmlService
		 */

		private INuGenTaskXmlService xmlService = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenTaskXmlService XmlService
		{
			get
			{
				if (this.xmlService == null)
				{
					this.xmlService = new NuGenTaskXmlService();
				}

				return this.xmlService;
			}
		}

		#endregion

		#region Properties.Protected.Overriden

		private INuGenTreeViewDragDropService dragService = null;

		/// <summary>
		/// </summary>
		protected override INuGenTreeViewDragDropService DragService
		{
			get
			{
				if (this.dragService == null)
				{
					this.dragService = new NuGenTaskDragDropService();
				}

				return this.dragService;
			}
		}

		#endregion

		#region Methods.Protected.Overriden

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

			if (serviceType == typeof(INuGenImageListService))
			{
				return this.ImageListService;
			}

			if (serviceType == typeof(INuGenStringProcessor))
			{
				return this.StringProcessor;
			}

			if (serviceType == typeof(INuGenTaskXmlService))
			{
				return this.XmlService;
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskServiceProvider"/> class.
		/// </summary>
		public NuGenTaskServiceProvider()
		{

		}

		#endregion
	}
}
