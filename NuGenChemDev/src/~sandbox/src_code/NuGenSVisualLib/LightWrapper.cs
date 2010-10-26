using System;
using System.Drawing;
using Microsoft.DirectX;

namespace NuGenSVisualLib.UI.Lighting
{
	/// <summary>
	/// Summary description for LightWrapper.
	/// </summary>
	public class LightWrapper : ICloneable
	{
		bool enabled = true;
		bool castSpecular = true;
		Color lightColor = Color.White;
		Vector3 direction;

		public LightWrapper()
		{
			direction = new Vector3(1.0f, 1.0f, 1.0f);
		}

		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
			}
		}

		public Color LightColor
		{
			get
			{
				return lightColor;
			}
			set
			{
				lightColor = value;
			}
		}

		public bool CastHighlights
		{
			get
			{
				return castSpecular;
			}
			set
			{
				castSpecular = value;
			}
		}

		public float DirectionX
		{
			get
			{
				return direction.X;
			}
			set
			{
				if (value > 1.0f)
					direction.X = 1.0f;
				else if (value < -1.0f)
					direction.X = -1.0f;
				else
					direction.X = value;
			}
		}

		public float DirectionY
		{
			get
			{
				return direction.Y;
			}
			set
			{
				if (value > 1.0f)
					direction.Y = 1.0f;
				else if (value < -1.0f)
					direction.Y = -1.0f;
				else
					direction.Y = value;
			}
		}

		public float DirectionZ
		{
			get
			{
				return direction.Z;
			}
			set
			{
				if (value > 1.0f)
					direction.Z = 1.0f;
				else if (value < -1.0f)
					direction.Z = -1.0f;
				else
					direction.Z = value;
			}
		}

		#region ICloneable Members

		public object Clone()
		{
			LightWrapper light = new LightWrapper();
			light.CastHighlights = this.CastHighlights;
			light.DirectionX = this.DirectionX;
			light.DirectionY = this.DirectionY;
			light.DirectionZ = this.DirectionZ;
			light.Enabled = this.Enabled;
			light.LightColor = this.LightColor;

			return light;
		}

		#endregion
	}
}