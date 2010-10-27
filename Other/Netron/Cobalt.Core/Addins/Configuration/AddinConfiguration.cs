using System;

namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for ApplicationConfiguration.
	/// </summary>
	public class AddinPackageConfiguration
	{

		#region Fields
		private string mLocation;
		private bool mAutoLoad;
		#endregion

		#region Properties

		public bool AutoLoad
		{
			get{return mAutoLoad;}
			set{mAutoLoad = value;}
		}

		public string Location
		{
			get{return mLocation;}
			set{mLocation = value;}
		}

		#endregion

		#region Constructor
		public AddinPackageConfiguration()
		{		}

		public AddinPackageConfiguration(string location) : this()
		{
			this.mLocation = location;
		}

		public AddinPackageConfiguration(string location, bool autoload) : this(location)
		{
			this.mAutoLoad = autoload;
		}

		#endregion


	}
}
