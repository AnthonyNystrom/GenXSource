/* -----------------------------------------------
 * Connector.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Editor.Text.Classification;

namespace Genetibase.Windows.Controls.Editor
{
	internal static class Connector
	{
		public interface IAssets
		{
			IClassificationFormatMapSelector ClassificationFormatMapSelector
			{
				get;
			}
		}

		private class AssetsImplementation : IAssets
		{
			public IClassificationFormatMapSelector ClassificationFormatMapSelector
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public AssetsImplementation()
			{
			}
		}

		public static IAssets Assets;

		static Connector()
		{
			Assets = new AssetsImplementation();
		}
	}
}
