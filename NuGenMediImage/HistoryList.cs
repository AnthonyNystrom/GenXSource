using System;
using System.Collections;

namespace Genetibase.NuGenMediImage
{
	/// <summary>
	/// Summary description for HistoryList.
	/// </summary>
	internal class HistoryList : ArrayList
	{
		public HistoryList():base()
		{			
		}

		public new History this[int idx]
		{
			get
			{
				return (History)base[ idx ];
			}
			set
			{
				this[ idx ] =  value;
			}
		}
	}
}
