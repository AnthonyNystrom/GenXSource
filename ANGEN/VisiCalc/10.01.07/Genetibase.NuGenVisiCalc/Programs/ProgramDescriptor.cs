/* -----------------------------------------------
 * ProgramDescriptor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Shared.Reflection;

namespace Genetibase.NuGenVisiCalc.Programs
{
	internal sealed class ProgramDescriptor : IDescriptor
	{
		private String _programName;

		public String ProgramName
		{
			get
			{
				return _programName;
			}
		}

		private Type _programType;

		public Type ProgramType
		{
			get
			{
				return _programType;
			}
		}

		public NodeBase CreateNode(Canvas canvas)
		{
			return (NodeBase)NuGenActivator.CreateObject(ProgramType, new Object[] { canvas.SelectedProgram });
		}

		public override String ToString()
		{
			return ProgramName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProgramDescriptor"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="programType"/> is <see langword="null"/>.</para>
		/// <para><paramref name="programName"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="programName"/> is an empty string.</para>
		/// </exception>
		public ProgramDescriptor(Type programType, String programName)
		{
			if (programType == null)
			{
				throw new ArgumentNullException("programType");
			}

			if (String.IsNullOrEmpty(programName))
			{
				throw new ArgumentNullException("programName");
			}

			_programType = programType;
			_programName = programName;
		}
	}
}
