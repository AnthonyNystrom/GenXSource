using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Metadata.Signature
{
	public class NuGenFunctionPointerSignatureItem : NuGenBaseSignatureItem
	{
		private string definition = null;
		public string Definition
		{
			get
			{
				return definition;
			}
			private set
			{
				definition = value;
			}
		}

		private NuGenMethodRefSignatureReader signatureReader;
		public NuGenMethodRefSignatureReader SignatureReader
		{
			get
			{
				return signatureReader;
			}
			private set
			{
				signatureReader = value;
			}
		}

		public NuGenFunctionPointerSignatureItem(NuGenMethodRefSignatureReader signatureReader)
		{
			SignatureReader = signatureReader;
		}

		public override string ToString()
		{
			if (Definition == null)
			{
				Definition = "method " + SignatureReader.GetDefinition(null, null, true);
			}

			return Definition;
		}
	}
}