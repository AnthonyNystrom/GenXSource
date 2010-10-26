/* -----------------------------------------------
 * IAdornmentSurface.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using Genetibase.Windows.Controls.Editor.Text.View;
using Genetibase.Windows.Controls.Editor.Text.Adornment;

namespace Genetibase.Windows.Controls.Editor.Text.AdornmentSurface
{
	/// <summary>
	/// </summary>
	public interface IAdornmentSurface
	{
		/// <summary>
		/// </summary>
		void AddAdornment(IAdornment adornment);
		
		/// <summary>
		/// </summary>
		Geometry GetAdornmentsGeometry();
		
		/// <summary>
		/// </summary>
		IList<SpaceNegotiation> GetSpaceNegotiations(ITextLine textLine);
		
		/// <summary>
		/// </summary>
		void RemoveAdornment(IAdornment adornment);

		/// <summary>
		/// </summary>
		Boolean CanNegotiateSurfaceSpace
		{
			get;
		}

		/// <summary>
		/// </summary>
		Panel SurfacePanel
		{
			get;
		}

		/// <summary>
		/// </summary>
		SurfacePosition SurfacePosition
		{
			get;
		}
	}
}
