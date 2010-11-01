using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;

namespace Dile.Disassemble
{
	public class NuGenExceptionClause
	{
		private CorExceptionFlag flags;
		public CorExceptionFlag Flags
		{
			get
			{
				return flags;
			}
			set
			{
				flags = value;
			}
		}

		private uint tryOffset;
		public uint TryOffset
		{
			get
			{
				return tryOffset;
			}
			set
			{
				tryOffset = value;
			}
		}

		private uint tryLength;
		public uint TryLength
		{
			get
			{
				return tryLength;
			}
			set
			{
				tryLength = value;
			}
		}

		private uint handlerOffset;
		public uint HandlerOffset
		{
			get
			{
				return handlerOffset;
			}
			set
			{
				handlerOffset = value;
			}
		}

		private uint handlerLength;
		public uint HandlerLength
		{
			get
			{
				return handlerLength;
			}
			set
			{
				handlerLength = value;
			}
		}

		private uint classToken;
		public uint ClassToken
		{
			get
			{
				return classToken;
			}
			set
			{
				classToken = value;
			}
		}

		private uint filterOffset;
		public uint FilterOffset
		{
			get
			{
				return filterOffset;
			}
			set
			{
				filterOffset = value;
			}
		}

		public NuGenExceptionClause()
		{
		}
	}
}