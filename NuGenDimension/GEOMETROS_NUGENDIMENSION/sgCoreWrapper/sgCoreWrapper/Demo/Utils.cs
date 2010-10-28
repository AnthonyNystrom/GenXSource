using System;
using System.Collections.Generic;
using System.Text;
using sgCoreWrapper.Objects;
using sgCoreWrapper.Structs;

namespace Demo
{
	public static class Utils
	{
		public static void AddFloorInScene(double size1, double size2, 
			double x_shift, double y_shift, double z_shift)
		{
			msgObject[] objects_buffer = new msgObject[4];
			objects_buffer[0] = msgLine.Create(size1/2.0, size2/2.0, 0.0, 
				-size1/2.0, size2/2.0, 0.0);
			objects_buffer[1] = msgLine.Create(-size1/2.0, size2/2.0, 0.0, 
				-size1/2.0, -size2/2.0, 0.0);
			objects_buffer[2] = msgLine.Create(-size1/2.0, -size2/2.0, 0.0, 
				size1/2.0, -size2/2.0, 0.0);
			objects_buffer[3] = msgLine.Create(size1/2.0, -size2/2.0, 0.0, 
				size1/2.0, size2/2.0, 0.0);

			msgContour cnt = msgContour.CreateContour(objects_buffer);
			msgObject floor = msgSurfaces.Face(cnt, null);
			msgObject.DeleteObject(cnt);

			msgScene.GetScene().AttachObject(floor);
			floor.SetAttribute(msgObjectAttrEnum.SG_OA_COLOR, 25);

			msgVectorStruct transV = new msgVectorStruct(x_shift, y_shift, z_shift);
			floor.InitTempMatrix().Translate(transV);
			floor.ApplyTempMatrix();
			floor.DestroyTempMatrix();
		}

		const double PiOver180 = 1.74532925199433E-002;
		const double PiUnder180 = 5.72957795130823E+001;

		public static double Radiansf(double Angle)
		{
			double r = Angle * PiOver180;
			return r;
		}

		public static double Degreesf(double Angle)
		{
			double d = Angle * PiUnder180;
			return d;
		}

		public static double Diff(double a, double b)
		{
			if (a >= 0 && b >= 0)
			{
				if (a > b)
					return a - b;
				else
					return b - a;
			}
			if (a < 0 && b < 0)
			{
				if (a > b)
					return b - a;
				else
					return a - b;
			}
			if (a >= 0 && b < 0)
				return a - b;
			else
				return b - a;
		}
	}
}
