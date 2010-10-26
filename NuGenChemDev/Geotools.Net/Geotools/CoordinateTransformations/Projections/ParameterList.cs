/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. 
 *
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */


#region Using
using System;
using System.Drawing;
using Geotools.CoordinateReferenceSystems;
#endregion

namespace Geotools.CoordinateTransformations
{
	/// <summary>
	/// Helper class that makes it easy to get named parameter from a ListDictionary. In particular this 
	/// class makes it easier to get a double as a named value in the ListDictionary.
	/// </summary>
	public class ParameterList : System.Collections.Specialized.ListDictionary
	{

		/// <summary>
		/// Gets an item from the collection and converts it to a double.
		/// </summary>
		/// <param name="key">The key of the item in the collection.</param>
		/// <param name="defaultValue">A default value if the item is not in the collection.</param>
		/// <returns>Double.</returns>
		public double GetDouble(string key, double defaultValue)
		{
			if (this.Contains(key))
			{
				return (double)this[key];
			}
			else
			{
				return defaultValue;
			}
		}
		/// <summary>
		/// Gets an item from the collection and converts it to a double.
		/// </summary>
		/// <param name="key">The key of the item in the collection.</param>
		/// <returns>Double</returns>
		/// <exception cref="ArgumentException">If the key does not exist or the value cannot be cast to a double.</exception>
		public double GetDouble(string key)
		{
			if (this.Contains(key))
			{
				try
				{
					return (double)this[key];
				}
				catch(Exception e)
				{
					throw new ArgumentException(String.Format("key {0} has an invalid entry.",key),e);
				}
			}
			else
			{
				throw new ArgumentException(String.Format("The key with a value of '{0}' is not in the list.",key));
			}
		}

	}
}
