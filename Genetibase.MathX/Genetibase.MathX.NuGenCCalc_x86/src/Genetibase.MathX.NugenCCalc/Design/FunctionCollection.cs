using System;
using System.Xml.Serialization;
using System.Collections;
using Genetibase.MathX.NugenCCalc;

namespace Genetibase.MathX.NugenCCalc.Design
{
	/// <summary>
	/// Summary description for SourceCollection.
	/// </summary>
	[Serializable()]
	public class FunctionCollection : CollectionBase
	{
		public event EventHandler OnChange;
		public event ParametersChangeHandler OnItemChange;

        #region Public Instance Constructors


		/// <summary>
		/// Initializes a new instance of the FunctionCollection class.
		/// </summary>
		public FunctionCollection()
		{

		}

        #endregion Public Instance Constructors

        #region Public Instance Properties
		/// <summary>
		/// Gets or sets the element at the specified index
		/// </summary>
		public FunctionParameters this[ int index ]  
		{
			get  
			{
				return((FunctionParameters) List[index] );
			}
			set  
			{
				List[index] = value;
				Change(this, new EventArgs());
			}
		}

		[XmlIgnore()]
		public FunctionCollection Functions2D
		{
			get  
			{
				FunctionCollection functions2D = new FunctionCollection();
				foreach(FunctionParameters function in List)
				{
					if (function is Function2DParameters)
						functions2D.Add(function);
				}
				return functions2D;
			}
		}

		[XmlIgnore()]
		public FunctionCollection Functions3D
		{
			get  
			{
				FunctionCollection functions3D = new FunctionCollection();
				foreach(FunctionParameters function in List)
				{
					if (function is Function3DParameters)
						functions3D.Add(function);
				}
				return functions3D;
			}
		}

        #endregion Public Instance Properties

		#region Public Instance Methods


		public int Add(FunctionParameters value)  
		{
			int retValue = List.Add( value );
			value.Changed +=new ParametersChangeHandler(value_Changed);
			Change(this, new EventArgs());
			return retValue;
		}


		public int IndexOf( FunctionParameters value )  
		{
			return( List.IndexOf( value ) );
		}


		public void Insert( int index, FunctionParameters value )  
		{
			List.Insert( index, value );
			value.Changed+=new ParametersChangeHandler(value_Changed);
			Change(this, new EventArgs());
		}



		public void Remove( FunctionParameters value )  
		{
			List.Remove( value );
			Change(this, new EventArgs());
		}


		public bool Contains( FunctionParameters value )  
		{
			return( List.Contains( value ) );
		}


		private void Change( object sender, EventArgs e)
		{
			if( null != OnChange )
			{
				OnChange( sender, e);
			}
		}

		private void value_Changed(FunctionParameters parameters, EventArgs e)
		{
			if( null != OnItemChange )
			{
				OnItemChange( parameters, e);
			}
		}
		#endregion


	}
}
