using System;

namespace DotNetMock
{

	/// <summary>
	/// This represents a Null Object.
	/// </summary>
	/// <remarks/>
	public class NullObject
	{
		private string _name;

		/// <summary>
		/// Default Constructor.  Sets the name of this object to "null"
		/// </summary>
		public NullObject() {
			_name = "null";
		}
		/// <summary>
		/// Default Constructor for NullObject.  Set the name for this Object
		/// </summary>
		/// <param name="name">Name of this Object</param>
		public NullObject(string name) 
		{
			_name = name;
		}
		/// <summary>
		/// Overrides Object.ToString().
		/// </summary>
		/// <returns>Object Name.</returns>
		public override string ToString()
		{
			return _name;
		}
		/// <summary>
		/// Determines if the given object is Null object.
		/// </summary>
		/// <param name="other">Object to compare.</param>
		/// <returns>True/False.</returns>
		public override bool Equals(Object other)
		{
			return (other is NullObject);
		}
		/// <summary>
		/// Returns unique hash code for the object.
		/// </summary>
		/// <returns>Objects HashCode.</returns>
		public override int GetHashCode()
		{
			return _name.GetHashCode();
		}
		/// <summary>
		/// Gets/Sets the name of this Null Object
		/// </summary>
		public string Name 
		{
			get 
			{
				return _name;
			}
			set 
			{
				_name = value;
			}
		}
	}	
}
