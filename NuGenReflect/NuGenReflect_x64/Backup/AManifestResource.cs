
/***
 * 
 *  ASMEX by RiskCare Ltd.
 * 
 * This source is copyright (C) 2002 RiskCare Ltd. All rights reserved.
 * 
 * Disclaimer:
 * This code is provided 'as is', with absolutely no warranty expressed or
 * implied.  Any use of this code is at your own risk.
 *   
 * You are hereby granted the right to redistribute this source unmodified
 * in its original archive. 
 * You are hereby granted the right to use this code, or code based on it,
 * provided that you acknowledge RiskCare Ltd somewhere in the documentation
 * of your application. 
 * You are hereby granted the right to distribute changes to this source, 
 * provided that:
 * 
 * 1 -- This copyright notice is retained unchanged 
 * 2 -- Your changes are clearly marked 
 * 
 * Enjoy!
 * 
 * --------------------------------------------------------------------
 * 
 * If you use this code or have comments on it, please mail me at 
 * support@jbrowse.com or ben.peterson@riskcare.com
 * 
 */



using System;
using System.Reflection;
using System.IO;

namespace Genetibase.Debug
{
	/// <summary>
	/// A node data item kept in ManResNodes.  Contains the info needed to identify a manifest resource
	/// </summary>
	internal class AManifestResource
	{
		string _name;
		ManifestResourceInfo _mri;
		Assembly _asm;


		public AManifestResource(ManifestResourceInfo mri, Assembly asm, string name)
		{
			_mri = mri;
			_name = name;
			_asm = asm;
		}

		public string FileName
		{
			get{return _mri.FileName;}
		}

		public string ReferencedAssembly
		{
			get{return _mri.ReferencedAssembly.GetName().Name;}
		}

		public string ResourceLocation
		{
			get{return _mri.ResourceLocation.ToString();}
		}

		public Assembly Asm
		{
			get{return _asm;}
		}

		public string Name
		{
			get{return _name;}
		}

		public bool IsResx
		{
			get
			{
				return _name.EndsWith(".resources");
			}
		}
	}

	/// <summary>
	/// A node data item used by ModuleNodes.  Contains the information needed to specify a module
	/// </summary>
	internal class AModule
	{
		string _file;


		public AModule(string file)
		{
			
			_file = file;
		}

		public AModule(Module mod)
		{
			
			_file = mod.FullyQualifiedName;
		}

		public Assembly Assembly
		{
			get
			{
				try
				{
					return Assembly.LoadFrom(_file);
				}
				catch//(Exception e)
				{
					return null;
				}

			}
		}

		public string FileName
		{
			get
			{
				return _file;
			}
		}

		public override string ToString()
		{
			return _file;
		}





	}

	/// <summary>
	/// A node data item used by FileNodes.  Contains the info needed to specify a file
	/// </summary>
	internal class AFile
	{
		string _file;


		public AFile(string file)
		{
			_file = file;
		}

		public AFile(FileStream f)
		{
			
			_file = f.Name;
			f.Close();
		}


		public bool Exists{get{return File.Exists(_file);}}
		public FileAttributes Attributes{get{return File.GetAttributes(_file);}}
		public DateTime TimeCreated{get{return File.GetCreationTime(_file);}}
		public DateTime TimeLastAccessed{get{return File.GetLastAccessTime(_file);}}
		public DateTime TimeLastModified{get{return File.GetLastWriteTime(_file);}}

		public string Name{get{return _file;}}

		public override string ToString(){return _file;}

	}
}
