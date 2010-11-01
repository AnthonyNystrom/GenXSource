using System;

namespace UseCaseMakerLibrary
{
	public interface IIdentificableObject
	{
		String UniqueID
		{
			get;
		}

		Package Owner
		{
			get;
			set;
		}

		String Name
		{
			get;
			set;
		}

		Int32 ID
		{
			get;
			set;
		}

		String Prefix
		{
			get;
			set;
		}

		String Path
		{
			get;
		}

		String ElementID
		{
			get;
		}
	}
}
