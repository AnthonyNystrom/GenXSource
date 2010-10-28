using System;

namespace Genetibase.MathX.NugenCCalc.Design
{
	/// <summary>
	/// Contains a string event argument
	/// </summary>
	public class StringArgs : EventArgs
	{
		private string _data;

		public StringArgs( string data )
		{
			_data = data;
		}
		
		public string Data
		{
			get
			{
				return _data;
			}
		}

	}
}
