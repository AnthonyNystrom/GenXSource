using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble.ILCodes;
using Dile.UI;
using System.Xml.Serialization;

namespace Dile.Disassemble
{	
	public abstract class NuGenTokenBase
	{
		private uint token;
		[XmlIgnore()]
		public uint Token
		{
			get
			{
				return token;
			}
			set
			{
				token = value;
			}
		}

		private bool isMetadataRead = false;
		[XmlIgnore()]
		public bool IsMetadataRead
		{
			get
			{
				return isMetadataRead;
			}
			set
			{
				isMetadataRead = value;
			}
		}

		protected string name;
		[XmlIgnore()]
		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = NuGenHelperFunctions.QuoteName(value);
			}
		}

		public virtual SearchOptions ItemType
		{
			get
			{
				return SearchOptions.None;
			}
		}

		public void ReadMetadata()
		{
			if (!IsMetadataRead)
			{
				//HACK Warning: the boolean is set to true before the reading would really occur.
				IsMetadataRead = true;
				ReadMetadataInformation();
			}
		}

		protected virtual void ReadMetadataInformation()
		{
		}
	}
}