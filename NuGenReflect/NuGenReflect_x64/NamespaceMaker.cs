/***
 * 
 *  ASMEX by RiskCare Ltd.
 * 
 * This source is copyright (C) 2002 RiskCare Ltd. All rights reserved.
 * 
 * Disclaimer:
 * This code is provided 'as is', with absolutely no warranty expressed or
 * implied.  Any use of this code is at your own risk.
 *   
 * You are hereby granted the right to redistribute this source unmodified
 * in its original archive. 
 * You are hereby granted the right to use this code, or code based on it,
 * provided that you acknowledge RiskCare Ltd somewhere in the documentation
 * of your application. 
 * You are hereby granted the right to distribute changes to this source, 
 * provided that:
 * 
 * 1 -- This copyright notice is retained unchanged 
 * 2 -- Your changes are clearly marked 
 * 
 * Enjoy!
 * 
 * --------------------------------------------------------------------
 * 
 * If you use this code or have comments on it, please mail me at 
 * support@jbrowse.com or ben.peterson@riskcare.com
 * 
 */

using System;
using System.Collections;
using System.Reflection;


namespace Genetibase.Debug
{

	/// <summary>
	/// Given a list of type names, this class generates a list of all the unique namespaces the types come from
	/// </summary>
	internal class NamespaceMaker
	{
		SortedList _names;

		internal NamespaceMaker(Type[] types)
		{
			_names = new SortedList();
			Assimilate(types);								  
		}

		public int Count{get{return _names.Count;}}

		public string this[int idx]
		{
			get
			{
				return (string)_names.GetKey(idx);
			}
		}

		private void Assimilate(Type[] types)
		{
			for(int i=0; i < types.Length; ++i)
			{
				if (types[i].IsNestedAssembly || types[i].IsNestedFamily || types[i].IsNestedPrivate || types[i].IsNestedPublic)
				{
					//nested ones can't have their own namespace, right?  So presumably I can ignore them.
				}
				else
				{
					AddName(types[i].Namespace);
				}
			}
		}

		private void AddName(string s)
		{
			if (s == null)
			{
				s = "";
			}

			_names[s] = "aru";
		}

	}
}
