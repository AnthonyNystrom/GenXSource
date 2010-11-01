using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble
{
	public class NuGenUserString : NuGenTokenBase
	{
		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				name = value;
			}
		}

		public NuGenUserString(uint token, string name)
		{
			Token = token;
			Name = name;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}