using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using System.Reflection.Emit;

namespace Dile.Controls
{
	public class NuGenOpCodeItem : IComparable
	{
		private OpCode opCode;
		public OpCode OpCode
		{
			get
			{
				return opCode;
			}

			set
			{
				opCode = value;
			}
		}

		public short OpCodeValue
		{
			get
			{
				return OpCode.Value;
			}
			set
			{
				OpCode = NuGenOpCodeGroups.OpCodesByValue[value];
			}
		}

		private string description;
		public string Description
		{
			get
			{
				return description;
			}

			set
			{
				description = value;
			}
		}

		public NuGenOpCodeItem()
		{	
		}

		public NuGenOpCodeItem(OpCode opCode, string description)
		{
			OpCode = opCode;
			Description = description;
		}

		public override string ToString()
		{
			return OpCode.Name;
		}

		public int CompareTo(object obj)
		{
			int result = 0;

			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (obj is NuGenOpCodeItem)
			{
				result = OpCode.Name.CompareTo(((NuGenOpCodeItem)obj).OpCode.Name);
			}
			else
			{
				throw new ArgumentException("Not supported type.", "obj");
			}

			return result;
		}
	}
}