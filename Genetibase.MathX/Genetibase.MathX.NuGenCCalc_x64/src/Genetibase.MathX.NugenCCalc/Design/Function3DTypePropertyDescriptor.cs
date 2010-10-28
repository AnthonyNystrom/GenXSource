using System;
using System.ComponentModel;
using Genetibase.MathX.NugenCCalc;


namespace Genetibase.MathX.NugenCCalc.Design
{
    public class Function3DTypePropertyDescriptor : PropertyDescriptor
	{
		NugenCCalc3DDesigner _CompDes;
		public Function3DTypePropertyDescriptor(NugenCCalc3DDesigner CompDes, 
			PropertyDescriptor PropDesc) : base(PropDesc)
		{
			_CompDes = CompDes;
		}
		public override bool ShouldSerializeValue(object component){ return false; }
		public override bool DesignTimeOnly {get { return true; } }
		public override Type PropertyType{ get{ return typeof(FunctionType3D); } }
		public override Type ComponentType{ get { return typeof(NugenCCalc3D); } }
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
			if (((NugenCCalc3D)component).FunctionParameters is Parametric3DParameters)
			{
				return FunctionType3D.Parametric;
			}
			if (((NugenCCalc3D)component).FunctionParameters is Explicit3DParameters)
			{
				return FunctionType3D.Explicit;
			}
			if (((NugenCCalc3D)component).FunctionParameters is Implicit3DParameters)
			{
				return FunctionType3D.Implicit;
			}
			if (((NugenCCalc3D)component).FunctionParameters is ParametricSurfaceParameters)
			{
				return FunctionType3D.ParametricSurface;
			}
			return null;
		}

		public override void SetValue(object component, object value) 
		{
			switch ((FunctionType3D)value)
			{
				case FunctionType3D.Explicit:
					((NugenCCalc3D)component).FunctionParameters = new Explicit3DParameters();
					break;
				case FunctionType3D.Implicit:
					((NugenCCalc3D)component).FunctionParameters = new Implicit3DParameters();
					break;
				case FunctionType3D.Parametric:
					((NugenCCalc3D)component).FunctionParameters = new Parametric3DParameters();
					break;
				case FunctionType3D.ParametricSurface:
					((NugenCCalc3D)component).FunctionParameters = new ParametricSurfaceParameters();
					break;
			}
		}
	};
}
