/* -----------------------------------------------
 * ProgramNode.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Genetibase.NuGenVisiCalc.Types;

namespace Genetibase.NuGenVisiCalc.Programs
{
	[Serializable]
	[Program("Program")]
	internal sealed class ProgramNode : NodeBase
	{
		private Program _subProgram;

		[Browsable(false)]
		public Program SubProgram
		{
			get
			{
				return _subProgram;
			}
			set
			{
				_subProgram = value;
			}
		}

		public override Object GetData()
		{
			return GetData(0);
		}

		public override Object GetData(Int32 index)
		{
			for (Int32 i = 0; i < _subProgram.Nodes.Count; i++)
			{
				ProgramOutput programOutput = _subProgram.Nodes[i] as ProgramOutput;

				if (programOutput != null)
				{
					if (programOutput.OutputIndex == index)
					{
						return programOutput.GetData();
					}
				}
			}

			return null;
		}

		public override Object GetData(Int32 index, params Object[] options)
		{
			return GetData(index);
		}

		public override Object GetData(params Object[] options)
		{
			return GetData(0);
		}

		public ProgramNode(Program parentProgram)
		{
			_subProgram = new Program(parentProgram);
			_subProgram.ParentNode = this;

			Name = "SubProgram";
			CreateInputs(1);
			SetInput(0, "On/Off", false);
		}
	}
}
