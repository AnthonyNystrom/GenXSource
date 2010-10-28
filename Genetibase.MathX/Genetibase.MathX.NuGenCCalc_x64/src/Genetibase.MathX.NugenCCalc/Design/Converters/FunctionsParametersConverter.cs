using System;
using System.ComponentModel;
using Genetibase.MathX.NugenCCalc;

namespace Genetibase.MathX.NugenCCalc.Design.Converters
{
	/// <summary>
	/// Summary description for FunctionsParametersConverter.
	/// </summary>
	public class FunctionsParametersConverter : ExpandableObjectConverter
	{
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection baseProps = 
				TypeDescriptor.GetProperties( value, attributes, true); 

			FunctionParameters parameters = value as FunctionParameters;
			PropertyDescriptorCollection propCollection = null;
			PropertyDescriptor[] resultPropCollection = null;


			if (parameters != null)
			{
				propCollection = TypeDescriptor.GetProperties(parameters.GetType(),new Attribute[]{new BrowsableAttribute(true)});
				int index = 0;
				switch (parameters.SourceType)
				{
					case SourceType.Equation:
						resultPropCollection = (PropertyDescriptor[])Array.CreateInstance(typeof(PropertyDescriptor), propCollection.Count - 2);

						foreach(PropertyDescriptor propertyDescriptor in propCollection)
						{
							if (propertyDescriptor.DisplayName != "Code" && propertyDescriptor.DisplayName != "CodeLanguage")
							{
								resultPropCollection.SetValue(propertyDescriptor, index);
								index++;
							}
						}
						break;
					case SourceType.CodeExpression:
						int formulaCount = 0;
						foreach(PropertyDescriptor propertyDescriptor in propCollection)
						{
							if (propertyDescriptor.DisplayName.StartsWith("Formula"))
							{
								formulaCount++;
							}
						}


						resultPropCollection = (PropertyDescriptor[])Array.CreateInstance(typeof(PropertyDescriptor), propCollection.Count - formulaCount);

						foreach(PropertyDescriptor propertyDescriptor in propCollection)
						{
							if (!propertyDescriptor.DisplayName.StartsWith("Formula"))
							{
								resultPropCollection.SetValue(propertyDescriptor, index);
								index++;
							}
						}
						break;
				}
			}

			return new PropertyDescriptorCollection(resultPropCollection);
		}

	}
}
