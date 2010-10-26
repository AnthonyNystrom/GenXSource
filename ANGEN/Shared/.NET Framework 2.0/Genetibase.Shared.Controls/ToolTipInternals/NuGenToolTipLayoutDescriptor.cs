/* -----------------------------------------------
 * NuGenToolTipLayoutDescriptor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace Genetibase.Shared.Controls.ToolTipInternals
{
	/// <summary>
	/// </summary>
	public struct NuGenToolTipLayoutDescriptor
	{
		/// <summary>
		/// </summary>
		public Rectangle BevelBounds;

		/// <summary>
		/// </summary>
		public Rectangle HeaderBounds;

		/// <summary>
		/// </summary>
		public Rectangle ImageBounds;

		/// <summary>
		/// </summary>
		public Rectangle TextBounds;

		/// <summary>
		/// </summary>
		public Rectangle RemarksHeaderBounds;

		/// <summary>
		/// </summary>
		public Rectangle RemarksImageBounds;

		/// <summary>
		/// </summary>
		public Rectangle RemarksBounds;

		/// <summary>
		/// </summary>
		public Size TooltipSize;

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">Another object to compare to.</param>
		/// <returns>
		/// true if obj and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj is NuGenToolTipLayoutDescriptor)
			{
				NuGenToolTipLayoutDescriptor compared = (NuGenToolTipLayoutDescriptor)obj;

				if (
					compared.BevelBounds == this.BevelBounds
					&& compared.HeaderBounds == this.HeaderBounds
					&& compared.ImageBounds == this.ImageBounds
					&& compared.RemarksBounds == this.RemarksBounds
					&& compared.RemarksHeaderBounds == this.RemarksHeaderBounds
					&& compared.RemarksImageBounds == this.RemarksImageBounds
					&& compared.TextBounds == this.TextBounds
					)
				{
					return compared.TooltipSize == this.TooltipSize;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override int GetHashCode()
		{
			int bevelBoundsHash = this.BevelBounds.GetHashCode();
			int headerBoundsHash = this.HeaderBounds.GetHashCode();
			int imageBoundsHash = this.ImageBounds.GetHashCode();
			int remarksBoundsHash = this.RemarksBounds.GetHashCode();
			int remarksHeaderBoundsHash = this.RemarksHeaderBounds.GetHashCode();
			int remarksImageBoundsHash = this.RemarksImageBounds.GetHashCode();
			int textBoundsHash = this.TextBounds.GetHashCode();
			int tooltipSizeHash = this.TooltipSize.GetHashCode();

			return 0
				| (bevelBoundsHash >> 28) & 0xf
				| ((headerBoundsHash >> 28) & 0xf) << 4
				| ((imageBoundsHash >> 28) & 0xf) << 8
				| ((remarksBoundsHash >> 28) & 0xf) << 12
				| ((remarksHeaderBoundsHash >> 28) & 0xf) << 16
				| ((remarksImageBoundsHash >> 28) & 0xf) << 20
				| ((textBoundsHash >> 28) & 0xf) << 24
				| ((tooltipSizeHash >> 28) & 0xf) << 28
				;
		}

		/// <summary>
		/// </summary>
		public static bool operator ==(NuGenToolTipLayoutDescriptor left, NuGenToolTipLayoutDescriptor right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// </summary>
		public static bool operator !=(NuGenToolTipLayoutDescriptor left, NuGenToolTipLayoutDescriptor right)
		{
			return !left.Equals(right);
		}
	}
}
