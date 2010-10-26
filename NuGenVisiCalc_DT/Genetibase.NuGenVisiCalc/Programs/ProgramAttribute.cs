/* -----------------------------------------------
 * ProgramAttribute.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc.Programs
{
	/// <summary>
	/// Apply this attribute to the class that is intended to act like a program.
	/// This class should be derived from the <see cref="NodeBase"/> class (directly or indirectly).
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	internal sealed class ProgramAttribute : Attribute
	{
		private String _programName;

		public String ProgramName
		{
			get
			{
				return _programName;
			}
			set
			{
				_programName = value;
			}
		}

		public ProgramAttribute(String programName)
		{
			if (String.IsNullOrEmpty(programName))
			{
				throw new ArgumentNullException("programName");
			}

			_programName = programName;
		}	
	}
}
