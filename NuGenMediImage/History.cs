using System;
using System.Collections;

namespace Genetibase.NuGenMediImage
{
	/// <summary>
	/// Summary description for History.
	/// </summary>
	internal class History
	{
		private string functionName = null;
		private ArrayList functionParam = new ArrayList();

		public string FunctionName
		{
			get
			{
				return functionName;
			}
			set
			{
				functionName = value;
			}
		}


		public ArrayList FunctionParameters
		{
			get
			{
				return functionParam;
			}
		}


		public object[] paramArray
		{
			get
			{
				object []param = null;

				if( this.FunctionParameters.Count > 0 )
				{
					param = new object[ this.FunctionParameters.Count ];

					for(int i=0; i < this.FunctionParameters.Count; i++ )
						param[i] = this.FunctionParameters[i];
				}

				return param;
			}
		}

		public History( String funcName)
		{
			this.functionName = funcName;
		}
	}
}
