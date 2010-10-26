/* -----------------------------------------------
 * ISamplesManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SampleFramework
{
	internal interface ISamplesManager
	{
		/// <summary>
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="ISampleFolder"/></para>
		/// </param>
		/// <param name="treeView"></param>
		/// <param name="folderImageIndex"></param>
		/// <param name="expandedFolderImageIndex"></param>
		/// <param name="sampleImageIndex"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="treeView"/> is <see langword="null"/>.</para>
		/// </exception>
		void PopulateSampleTree(
			INuGenServiceProvider serviceProvider
			, NuGenTreeView treeView
			, int folderImageIndex
			, int expandedFolderImageIndex
			, int sampleImageIndex
		);
	}
}
