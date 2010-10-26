/* -----------------------------------------------
 * SamplesManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SampleFramework
{
	internal sealed class SamplesManager : ISamplesManager
	{
		/// <summary>
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="ISampleFolder"/></para></param>
		/// <param name="treeView"></param>
		/// <param name="folderImageIndex"></param>
		/// <param name="expandedFolderImageIndex"></param>
		/// <param name="sampleImageIndex"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="treeView"/> is <see langword="null"/>.</para>
		/// </exception>
		public void PopulateSampleTree(
			INuGenServiceProvider serviceProvider
			, NuGenTreeView treeView
			, int folderImageIndex
			, int expandedFolderImageIndex
			, int sampleImageIndex
			)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			if (treeView == null)
			{
				throw new ArgumentNullException("treeView");
			}

			ISampleFolderDescriptor sampleFolder = serviceProvider.GetService<ISampleFolderDescriptor>();
			Debug.Assert(sampleFolder != null, "sampleFolder != null");

			DirectoryInfo sampleDirInfo = new DirectoryInfo(sampleFolder.Path);
			Debug.Write("Scanning \"");
			Debug.Write(sampleDirInfo.FullName);
			Debug.WriteLine("\" folder...");

			if (!sampleDirInfo.Exists)
			{
				Trace.Write("Sample directory does not exist.");
				return;
			}

			DirectoryInfo[] samples = sampleDirInfo.GetDirectories();
			Debug.WriteLine("Getting the list of sub-directories...");
			Debug.WriteLineIf(samples != null, "samples.Length = " + samples.Length.ToString());

			if (samples != null)
			{
				Debug.WriteLine("Looping through the sub-directories...");
				
				foreach (DirectoryInfo dirInfo in samples)
				{
					NuGenTreeNode treeNode = new NuGenTreeNode(dirInfo.Name, folderImageIndex, expandedFolderImageIndex);
					treeView.Nodes.Add(treeNode);

					Debug.Write("Examining \"");
					Debug.Write(dirInfo.FullName);
					Debug.WriteLine("\" sub-directory...");

					this.BuildSampleTreeNode(sampleFolder, dirInfo, treeNode, folderImageIndex, expandedFolderImageIndex, sampleImageIndex);
				}
			}
		}

		private void BuildSampleTreeNode(
			ISampleFolderDescriptor sampleFolderDescriptor
			, DirectoryInfo sampleDirInfo
			, NuGenTreeNode treeNode
			, int folderImageIndex
			, int expandedFolderImageIndex
			, int sampleImageIndex
			)
		{
			Debug.Assert(sampleFolderDescriptor != null, "sampleFolder != null");
			Debug.Assert(sampleDirInfo != null, "sampleDirInfo != null");
			Debug.Assert(treeNode != null, "treeNode != null");

			string samplePath = null;
			string descriptionPath = null;
			string screenShotPath = null;
			string exePath = null;
			string csProjectPath = null;
			string vbProjectPath = null;

			/*
			 * Algorithm:
			 * Get description if available.
			 * Get screen-shot if available.
			 * Get the list of folders.
			 * If CS folder exists, search for *.csproj file inside. And EXE in bin/Debug or bin/Release as well.
			 * If VB folder exists, search for *.vbproj file inside. And EXE in bin/Debug or bin/Release if CS project does not exist.
			 * Call this method recursively for other folders.
			 */

			FileInfo descriptionFileInfo = new FileInfo(Path.Combine(sampleDirInfo.FullName, sampleFolderDescriptor.DescriptionFileName));

			if (descriptionFileInfo.Exists)
			{
				descriptionPath = descriptionFileInfo.FullName;
			}

			Debug.WriteLine("descriptionPath = " + descriptionPath);

			FileInfo screenShotFileInfo = new FileInfo(Path.Combine(sampleDirInfo.FullName, sampleFolderDescriptor.ScreenshotFileName));

			if (screenShotFileInfo.Exists)
			{
				screenShotPath = screenShotFileInfo.FullName;
			}

			Debug.WriteLine("screenShotPath = " + screenShotPath);

			if (!string.IsNullOrEmpty(descriptionPath) && !string.IsNullOrEmpty(screenShotPath))
			{
				samplePath = sampleDirInfo.FullName;
			}

			Debug.WriteLine("samplePath = " + samplePath);

			DirectoryInfo[] subDirInfoCollection = sampleDirInfo.GetDirectories();
			Debug.WriteLine("Getting the list of sub-directories...");
			Debug.WriteLineIf(subDirInfoCollection != null, "subDirInfoCollection.Length = " + subDirInfoCollection.Length.ToString());

			if (subDirInfoCollection != null)
			{
				Debug.WriteLine("Looping through the sub-directories...");

				foreach (DirectoryInfo subDirInfo in subDirInfoCollection)
				{
					if (subDirInfo.Name == sampleFolderDescriptor.CSProjectFolderName)
					{
						Debug.WriteLine("CS project folder found.");
						csProjectPath = this.GetProjectPath(subDirInfo, sampleFolderDescriptor.CSProjectExtension);
						Debug.WriteLine("csProjectPath = " + csProjectPath);

						if (exePath == null)
						{
							exePath = this.FindExe(Path.GetDirectoryName(csProjectPath));
						}

						Debug.WriteLine("exePath = " + exePath);
					}
					else if (subDirInfo.Name == sampleFolderDescriptor.VBProjectFolderName)
					{
						Debug.WriteLine("VB project folder found.");
						vbProjectPath = this.GetProjectPath(subDirInfo, sampleFolderDescriptor.VBProjectExtension);
						Debug.WriteLine("vbProjectPath = " + vbProjectPath);

						if (exePath == null)
						{
							exePath = this.FindExe(Path.GetDirectoryName(vbProjectPath));
						}

						Debug.WriteLine("exePath = " + exePath);
					}
					else
					{
						NuGenTreeNode childNode = treeNode;

						if (this.ShouldCreateSubFolder(sampleFolderDescriptor, subDirInfo))
						{
							childNode = new NuGenTreeNode(subDirInfo.Name, folderImageIndex, expandedFolderImageIndex);
							treeNode.Nodes.Add(childNode);
						}

						this.BuildSampleTreeNode(sampleFolderDescriptor, subDirInfo, childNode, folderImageIndex, expandedFolderImageIndex, sampleImageIndex);
					}
				}
			}

			if (samplePath != null)
			{
				SampleDescriptor sampleDescriptor = new SampleDescriptor(
					samplePath
					, descriptionPath
					, screenShotPath
					, exePath
					, csProjectPath
					, vbProjectPath
				);

				Debug.WriteLine("Creating a node with a sample descriptor assigned to the Tag property...");
				NuGenTreeNode sampleNode = new NuGenTreeNode(sampleDirInfo.Name, sampleImageIndex, sampleImageIndex);
				sampleNode.Tag = sampleDescriptor;

				Debug.WriteLine("Adding the node to the parent node...");
				treeNode.Nodes.Add(sampleNode);
			}
		}

		private string GetProjectPath(DirectoryInfo dirInfo, string extension)
		{
			Debug.Assert(dirInfo != null, "dirInfo != null");
			Debug.Assert(!string.IsNullOrEmpty(extension), "!string.IsNullOrEmpty(extension)");

			FileInfo[] files = dirInfo.GetFiles(string.Format("*.{0}", extension), SearchOption.AllDirectories);

			if (files != null && files.Length > 0)
			{
				return files[0].FullName;
			}

			return null;
		}

		private string FindExe(string path)
		{
			DirectoryInfo binDirInfo = new DirectoryInfo(Path.Combine(path, "bin"));
			string exePath = null;

			if (binDirInfo.Exists)
			{
				exePath = this.FindExeInternal(Path.Combine(binDirInfo.FullName, "Release"));

				if (exePath == null)
				{
					exePath = this.FindExeInternal(Path.Combine(binDirInfo.FullName, "Debug"));
				}
			}

			return exePath;
		}

		private string FindExeInternal(string path)
		{
			DirectoryInfo dirInfo = new DirectoryInfo(path);

			if (dirInfo.Exists)
			{
				FileInfo[] exeFiles = dirInfo.GetFiles("*.exe", SearchOption.TopDirectoryOnly);

				foreach (FileInfo exeFile in exeFiles)
				{
					if (exeFile.Name.IndexOf("vshost") > -1)
					{
						continue;
					}
					else
					{
						return exeFile.FullName;
					}
				}
			}

			return null;
		}

		private bool ShouldCreateSubFolder(ISampleFolderDescriptor sampleFolderDescriptor, DirectoryInfo dirInfo)
		{
			Debug.Assert(sampleFolderDescriptor != null, "sampleFolderDescriptor != null");
			Debug.Assert(dirInfo != null, "dirInfo != null");

			DirectoryInfo[] subDirInfoCollection = dirInfo.GetDirectories();

			if (subDirInfoCollection != null && subDirInfoCollection.Length > 0)
			{
				foreach (DirectoryInfo subDirInfo in subDirInfoCollection)
				{
					if (subDirInfo.Name != sampleFolderDescriptor.CSProjectFolderName
						&& subDirInfo.Name != sampleFolderDescriptor.VBProjectFolderName)
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SamplesManager"/> class.
		/// </summary>
		public SamplesManager()
		{
		}
	}
}
