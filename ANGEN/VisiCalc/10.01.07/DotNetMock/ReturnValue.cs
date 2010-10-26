
namespace DotNetMock
{
	using System.Collections;
	using System;
	
	/// <summary>
	/// Represent a queue of expected Return Values.
	/// </summary>
	/// <remarks/>
	public class ReturnValue
	{
		private bool repeatFinal;
		private bool hasReturnValues;
		private Queue returnValues = new Queue();
		private string name = "";
		
		/// <summary>
		/// Default constructor.  Sets name to "Unamed ReturnValue"
		/// </summary>
		public ReturnValue() {
			 this.name = "Unnamed ReturnValue";
		}
		/// <summary>
		/// Constructor.  Sets the name of this Return Value to the given name.
		/// </summary>
		/// <param name="newName">Name of this Return Value</param>
		public ReturnValue(string newName) : base()
		{
			this.name = newName;
		}
		/// <summary>
		/// Gets/Sets if the Return Value should repeat the last value of the the group
		/// </summary>
		public bool RepeatFinalValue
		{			
			set
			{
				this.repeatFinal = value;
			}
			get
			{
				return this.repeatFinal;
			}
		}
		/// <summary>
		/// Gets/Sets if this group has any return values
		/// </summary>
		public bool HasValues
		{
		        set
		        {
		                this.hasReturnValues = value;
		        }
		        get
		        {
		                return this.hasReturnValues;
		        }
		}
		/// <summary>
		/// Adds a value to the group of return values
		/// </summary>
		/// <param name="nextReturnValue">Object to add</param>
		public void Add(object nextReturnValue)
		{
			HasValues = true;
			this.returnValues.Enqueue(nextReturnValue);
		}
		/// <summary>
		/// Adds a collection of values to the group of return values
		/// </summary>
		/// <param name="values">Collection to add</param>
		public void AddCollection(ICollection values)
		{
			foreach (object value in values)
			{
				Add(value);
			}
		}
		/// <summary>
		/// Pop's the next value off the queue of return values.
		/// </summary>
		/// <returns>Next return value</returns>
		public object Next()
		{
			if (returnValues.Count == 0)
			{
				throw new AssertionException(this.name + " has run out of return values");
			}
			
			object returnVal = null;
			
			if (returnValues.Count == 1 && RepeatFinalValue)
			{
				returnVal = returnValues.Peek();
			}
			else
			{
				returnVal = returnValues.Dequeue();	
			}
			
			Exception returnException = null;
			if ((returnException = (returnVal as Exception)) != null) throw returnException;
			
			return returnVal;
		}
	}
}
