using System;

namespace UseCaseMakerLibrary
{
	public class Requirement : IdentificableObject
	{
		#region Public Constants and Enumerators
		#endregion

		#region Class Members
		private String description = String.Empty;
		#endregion

		#region Constructors
		internal Requirement()
		{
		}
		#endregion

		#region Public Properties
		public String Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		public new String UniqueID
		{
			get
			{
				return base.UniqueID;
			}
			set
			{
				base.UniqueID = value;
			}
		}

		public new String Name
		{
			get
			{
				return base.ID.ToString();
			}
			set
			{
				base.Name = value;
			}
		}
		#endregion
	}
}
