using System;
using System.Windows.Forms;

namespace UseCaseMakerLibrary
{
	public class UserViewStatus
	{
		#region Class Members
		private TabPage currentPage = null;
		#endregion

		#region Constructors
		public UserViewStatus()
		{
		}
		#endregion

		#region Public Properties
		public TabPage CurrentTabPage
		{
			get
			{
				return this.currentPage;
			}
			set
			{
				this.currentPage = value;
			}
		}
		#endregion
	}
}
