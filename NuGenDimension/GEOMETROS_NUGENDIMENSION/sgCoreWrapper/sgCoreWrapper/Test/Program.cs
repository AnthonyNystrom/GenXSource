using System;
using System.Collections.Generic;
using System.Text;
using sgCoreWrapper.Objects;
using sgCoreWrapper.Structs;
using sgCoreWrapper;

namespace Test
{
	class Program
	{
		static void Main(string[] args)
		{
			msgCore.InitKernel();
			msg3DObject.AutoTriangulate(true, msgTriangulationTypeEnum.SG_DELAUNAY_TRIANGULATION);

			msgCircleStruct circleStruct = new msgCircleStruct();
			msgPointStruct p1 = new msgPointStruct();
			p1.x = 0;
			p1.y = 0;
			p1.z = 0;

			msgPointStruct p2 = new msgPointStruct();
			p2.x = 1;
			p2.y = 1;
			p2.z = 1;

			msgPointStruct p3 = new msgPointStruct();
			p3.x = 0;
			p3.y = 0;
			p3.z = 1;
			circleStruct.FromThreePoints(p1, p2, p3);

			msgCircle circle = msgCircle.Create(circleStruct);

			msgCore.FreeKernel();
		}
	}
}
