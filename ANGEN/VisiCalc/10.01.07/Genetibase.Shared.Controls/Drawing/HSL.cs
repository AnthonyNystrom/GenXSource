/* -----------------------------------------------
 * HSL.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.Drawing
{
	/// <summary>
	/// Represents a HSL-color.
	/// </summary>
	public struct HSL
	{
		#region Methods.Public

		/// <summary>
		/// Builds a <see cref="HSL"/> structure from the specified <see cref="Color"/> structure.
		/// </summary>
		public static HSL RGB2HSL(Color col)
		{
			float r = (float)col.R / 255f, g = (float)col.G / 255f, b = (float)col.B / 255f;
			float min = Math.Min(Math.Min(r, g), b);
			float max = Math.Max(Math.Max(r, g), b);
			float delta = max - min;

			HSL ret = new HSL(0, 0, 0);
			ret.l = (int)(50f * (max + min));

			if (delta == 0f)
			{
				ret.h = 0;
				ret.s = 0;
			}
			else
			{
				if (ret.l < 50)
					ret.s = (int)(100f * delta / (max + min));
				else
					ret.s = (int)(100f * delta / (2f - max - min));

				float del_R = (((max - r) / 6f) + (delta / 2f)) / delta;
				float del_G = (((max - g) / 6f) + (delta / 2f)) / delta;
				float del_B = (((max - b) / 6f) + (delta / 2f)) / delta;

				if (r == max)
					ret.h = (int)(360f * (del_B - del_G));
				else if (g == max)
					ret.h = (int)(360f * ((1f / 3f) + del_R - del_B));
				else if (b == max)
					ret.h = (int)(360f * ((2f / 3f) + del_G - del_R));

				if (ret.h < 0)
					ret.h += 360;
				if (ret.h > 360)
					ret.h -= 360;
			}
			return ret;
		}

		/// <summary>
		/// Builds a <see cref="Color"/> structure from the specified <see cref="HSL"/> structure.
		/// </summary>
		public static Color Hue2RGB(HSL hue)
		{
			float r, g, b, var_2, var_1;
			if (hue.s == 0)
			{
				r = g = b = (float)hue.l * 2.55f;
			}
			else
			{
				if (hue.l < 50)
					var_2 = (float)hue.l * (float)(100 + hue.s) / 10000f;
				else
					var_2 = ((float)(hue.l + hue.s) - (float)(hue.s * hue.l) / 100f) / 100f;

				var_1 = (float)hue.l / 50f - var_2;

				r = 255f * Hue_2_RGB(var_1, var_2, hue.h + 120);
				g = 255f * Hue_2_RGB(var_1, var_2, hue.h);
				b = 255f * Hue_2_RGB(var_1, var_2, hue.h - 120);
			}
			return Color.FromArgb((int)r, (int)g, (int)b);
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">Another object to compare to.</param>
		/// <returns>
		/// true if obj and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		public override bool Equals(object obj)
		{
			return obj is HSL && ((HSL)obj) == this;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Returns the fully qualified type name of this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> containing a fully qualified type name.
		/// </returns>
		public override string ToString()
		{
			return "HSL[" + h.ToString() + ";" + l.ToString() + ";" + s.ToString() + "]";
		}

		#endregion

		#region Methods.Private

		/// <summary>
		/// Interne Methode, nicht verwenden
		/// </summary>
		private static float Hue_2_RGB(float v1, float v2, int vH)
		{
			if (vH > 360)
				vH -= 360;
			else if (vH < 0)
				vH += 360;
			if (vH < 60)
				return v1 + (v2 - v1) * (float)vH / 60f;
			if (vH < 180)
				return v2;
			if (vH < 240)
				return v1 + (v2 - v1) * (240f - (float)vH) / 60f;
			return v1;
		}

		#endregion

		#region Operators

		/// <summary>
		/// </summary>
		public static bool operator ==(HSL v1, HSL v2)
		{
			return v1.h == v2.h && v1.l == v2.l && v1.s == v2.s;
		}

		/// <summary>
		/// </summary>
		public static bool operator !=(HSL v1, HSL v2)
		{
			return !(v1.h == v2.h && v1.l == v2.l && v1.s == v2.s);
		}

		#endregion

		/// <summary>
		/// </summary>
		public int h;

		/// <summary>
		/// </summary>
		public int s;

		/// <summary>
		/// </summary>
		public int l;

		/// <summary>
		/// Initializes a new instance of the <see cref="HSL"/> struct.
		/// </summary>
		/// <param name="h">Hue: Should be 0-360.</param>
		/// <param name="l">Luminance: Should be 0-100.</param>
		/// <param name="s">Saturation: Should be 0-100.</param>
		public HSL(int h, int l, int s)
		{
			this.h = h;
			this.l = l;
			this.s = s;
		}
	}
}
