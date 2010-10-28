using System;
using System.ComponentModel;
using Genetibase.MathX.NugenCCalc;

namespace Genetibase.MathX.NugenCCalc.Design
{
	public class FunctionTypePropertyDescriptor : PropertyDescriptor
	{
		NugenCCalcDesigner _compDes;
		public FunctionTypePropertyDescriptor(NugenCCalcDesigner CompDes, 
			PropertyDescriptor PropDesc) : base(PropDesc)
		{
			_compDes = CompDes;
		}
		public override bool ShouldSerializeValue(object component){ return false; }
		public override bool DesignTimeOnly {get { return false; } }
		public override Type PropertyType{ get{ return typeof(FunctionType); } }
		public override Type ComponentType{ get { return typeof(NugenCCalc2D); } }
		public override bool IsReadOnly { get{ return false; } }

		public override string Category{get{return "Source properties";}}


		public override bool CanResetValue(object component)
		{

			return false;
		}

		public override void ResetValue(object component) 
		{

		}

		public override object GetValue(object component) 
		{
			if (((NugenCCalc2D)component).FunctionParameters is Parametric2DParameters)
			{
				return FunctionType.Parametric;
			}
			if (((NugenCCalc2D)component).FunctionParameters is Explicit2DParameters)
			{
				return FunctionType.Explicit;
			}
			if (((NugenCCalc2D)component).FunctionParameters is Implicit2DParameters)
			{
				return FunctionType.Implicit;
			}
			return null;
		}

		public override void SetValue(object component, object value) 
		{
			switch ((FunctionType)value)
			{
				case FunctionType.Explicit:
					((NugenCCalc2D)component).FunctionParameters = new Explicit2DParameters();
					break;
				case FunctionType.Implicit:
					((NugenCCalc2D)component).FunctionParameters = new Implicit2DParameters();
					break;
				case FunctionType.Parametric:
					((NugenCCalc2D)component).FunctionParameters = new Parametric2DParameters();
					break;
			}
		}
	};
}


