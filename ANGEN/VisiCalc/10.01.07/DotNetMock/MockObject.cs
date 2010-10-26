using System;

namespace DotNetMock
{
	/// <summary>
	/// Base Mock Object.  All custom Mock Objects can extend this object.
	/// </summary>
	[Serializable()]
	public class MockObject : MarshalByRefObject, IMockObject
	{
		/// <summary>
		/// Field to hold the name of this MockObject
		/// </summary>
		private string _name = null;
		
		/// <summary>
		/// Flag to indicate if this object has been verified
		/// </summary>
		private bool _verified = false;

		/// <summary>
		/// Constructor.  Sets the name of this MockObject
		/// </summary>
		/// <param name="name">Name of this MockObject</param>
		public MockObject(string name) {
			_name = name;
		}
		/// <summary>
		/// Default Constructor.  Sets this MockObject's name to "MockObject"
		/// </summary>
		public MockObject() 
		{
			_name = "MockObject";
		}
		/// <summary>
		/// Returns true / false if this object has been verified or not.
		/// </summary>
		public bool IsVerified 
		{
			get { return _verified; }
		}

		/// <summary>
		/// Throws NotImplementedException.
		/// </summary>
		/// <param name="methodName">Method that isn't implemented.</param>
		public void NotImplemented(string methodName) 
		{
			throw new NotImplementedException(methodName + " not currently implemented");
		}
		/// <summary>
		/// Verifies object.
		/// </summary>
		public virtual void Verify()
		{
			Verifier.Verify(this);
			_verified = true;
		}
		/// <summary>
		/// Gets/Sets the Name of the Mock Object
		/// </summary>
		public string MockName 
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
