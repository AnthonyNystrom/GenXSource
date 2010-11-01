using System;
using System.Collections;

namespace UseCaseMakerLibrary
{
	/// <summary>
	/// Descrizione di riepilogo per GenericSort.
	/// </summary>
	public class PropertySorter : IComparer
	{
		String sortPropertyName;
		String sortOrder;

		public PropertySorter(string sortPropertyName, string sortOrder) 
		{
			this.sortPropertyName = sortPropertyName;
			this.sortOrder = sortOrder;
		}

		public int Compare(object x, object y)
		{
			IComparable ic1 = (IComparable)x.GetType().GetProperty(sortPropertyName).GetValue(x,null);
			IComparable ic2 = (IComparable)y.GetType().GetProperty(sortPropertyName).GetValue(y,null);

			if(sortOrder != null && sortOrder.ToUpper().Equals("ASC"))
			{
				return ic1.CompareTo(ic2);
			}
			else
			{
				return ic2.CompareTo(ic1);
			}
		}
	}
}
