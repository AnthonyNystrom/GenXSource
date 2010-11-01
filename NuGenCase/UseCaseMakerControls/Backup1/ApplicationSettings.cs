using System;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace UseCaseMaker
{
	/// <summary>
	/// Application configuration handling class
	/// </summary>
	/// <remarks>
	/// This class handles the configuration file (.config) of the application.
	/// Modifications about the user preferences will be saved and reloaded at
	/// next application startup directly from this class,
	/// </remarks>
	public class ApplicationSettings
	{
		#region Public Constants and Enumerators
		private const string reportsFolderName = "Reports";
		private const string languagesFolderName = "Languages";
		private const string languageFileNamePrefix = "Localization_";
		#endregion

		#region Class Members
		private bool settingsChanged = false;
		private bool isMaximized = false;
		private int left = 0;
		private int top = 0;
		private int width = 630;
		private int height = 450;
		private int splitPosition = 130;
		private string modelFilePath;
		private string htmlFilesPath;
		private string uiLanguage = "EN-US";
		private string recentFile1;
		private string recentFile2;
		private string recentFile3;
		private string recentFile4;
		#endregion

		#region Constructors
		public ApplicationSettings()
		{
			//
			// TODO: aggiungere qui la logica del costruttore
			//
		}
		#endregion

		#region Public Properties
		public string ModelFilePath
		{
			get
			{
				return this.modelFilePath;
			}
			set
			{
				this.modelFilePath = value;
				this.settingsChanged = true;
			}
		}

		public string HtmlFilesPath
		{
			get
			{
				return this.htmlFilesPath;
			}
			set
			{
				this.htmlFilesPath = value;
				this.settingsChanged = true;
			}
		}

		public bool IsMaximized
		{
			get
			{
				return this.isMaximized;
			}
			set
			{
				this.isMaximized = value;
				this.settingsChanged = true;
			}
		}

		public int Left
		{
			get
			{
				return this.left;
			}
			set
			{
				this.left = value;
				this.settingsChanged = true;
			}
		}

		public int Top
		{
			get
			{
				return this.top;
			}
			set
			{
				this.top = value;
				this.settingsChanged = true;
			}
		}

		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
				this.settingsChanged = true;
			}
		}

		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
				this.settingsChanged = true;
			}
		}

		public int SplitPosition
		{
			get
			{
				return this.splitPosition;
			}
			set
			{
				this.splitPosition = value;
				this.settingsChanged = true;
			}
		}

		public string UILanguage
		{
			get
			{
				return this.uiLanguage;
			}
			set
			{
				this.uiLanguage = value;
				this.settingsChanged = true;
			}
		}

		public string RecentFile1
		{
			get
			{
				return this.recentFile1;
			}
			set
			{
				this.recentFile1 = value;
				this.settingsChanged = true;
			}
		}

		public string RecentFile2
		{
			get
			{
				return this.recentFile2;
			}
			set
			{
				this.recentFile2 = value;
				this.settingsChanged = true;
			}
		}

		public string RecentFile3
		{
			get
			{
				return this.recentFile3;
			}
			set
			{
				this.recentFile3 = value;
				this.settingsChanged = true;
			}
		}

		public string RecentFile4
		{
			get
			{
				return this.recentFile4;
			}
			set
			{
				this.recentFile4 = value;
				this.settingsChanged = true;
			}
		}

		public string LanguageFileNamePrefix
		{
			get
			{
				return languageFileNamePrefix;
			}
		}

		public string ReportsFilesPath
		{
			get
			{
#if DEBUG
				return Application.StartupPath.Replace("bin" + Path.DirectorySeparatorChar + "Debug",reportsFolderName);
#else
				return Application.StartupPath + Path.DirectorySeparatorChar + reportsFolderName;
#endif
			}
		}

		public string LanguagesFilePath
		{
			get
			{
#if DEBUG
				return Application.StartupPath.Replace("bin" + Path.DirectorySeparatorChar + "Debug",languagesFolderName);
#else
				return Application.StartupPath + Path.DirectorySeparatorChar + languagesFolderName;
#endif
			}
		}

		public string UILanguageFilePath
		{
			get
			{
#if DEBUG
				return Application.StartupPath.Replace("bin" + Path.DirectorySeparatorChar + "Debug",languagesFolderName) +
					Path.DirectorySeparatorChar +
					languageFileNamePrefix +
					this.uiLanguage.ToLower() + ".xml";
#else
				return Application.StartupPath +
					Path.DirectorySeparatorChar +
					languagesFolderName +
					Path.DirectorySeparatorChar +
					languageFileNamePrefix +
					this.uiLanguage.ToLower() + ".xml";
#endif
			}
		}
		#endregion

		#region Public Methods
		public bool SaveSettings()
		{
			if(this.settingsChanged)
			{
				StreamWriter myWriter = null;
				XmlSerializer mySerializer = null;
				try
				{
					// Create an XmlSerializer for the 
					// ApplicationSettings type.
					mySerializer = new XmlSerializer( 
						typeof(ApplicationSettings));
					myWriter = 
						new StreamWriter(Application.StartupPath +
						Path.DirectorySeparatorChar + 
						Application.ProductName +
						".config",false);
					// Serialize this instance of the ApplicationSettings 
					// class to the config file.
					mySerializer.Serialize(myWriter, this);
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
				finally
				{
					// If the FileStream is open, close it.
					if(myWriter != null)
					{
						myWriter.Close();
					}
				}
			}
			return this.settingsChanged;
		}

		public bool LoadSettings()
		{
			XmlSerializer mySerializer = null;
			FileStream myFileStream = null;
			bool fileExists = false;

			try
			{
				// Create an XmlSerializer for the ApplicationSettings type.
				mySerializer = new XmlSerializer(typeof(ApplicationSettings));
				FileInfo fi = new FileInfo(
					Application.StartupPath + 
					Path.DirectorySeparatorChar + 
					Application.ProductName + 
					".config");
				// If the config file exists, open it.
				if(fi.Exists)
				{
					myFileStream = fi.OpenRead();
					// Create a new instance of the ApplicationSettings by
					// deserializing the config file.
					ApplicationSettings myAppSettings = 
						(ApplicationSettings)mySerializer.Deserialize(myFileStream);
					// Assign the property values to this instance of 
					// the ApplicationSettings class.
					this.modelFilePath = myAppSettings.ModelFilePath;
					this.htmlFilesPath = myAppSettings.HtmlFilesPath;
					this.isMaximized = myAppSettings.IsMaximized;
					this.left = myAppSettings.Left;
					this.top = myAppSettings.Top;
					this.width = myAppSettings.Width;
					this.height = myAppSettings.Height;
					this.splitPosition = myAppSettings.SplitPosition;
					this.uiLanguage = myAppSettings.UILanguage;
					this.recentFile1 = myAppSettings.RecentFile1;
					this.recentFile2 = myAppSettings.RecentFile2;
					this.recentFile3 = myAppSettings.RecentFile3;
					this.recentFile4 = myAppSettings.RecentFile4;
					fileExists = true;
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				// If the FileStream is open, close it.
				if(myFileStream != null)
				{
					myFileStream.Close();
				}
			}

			if(this.modelFilePath == null)
			{
				// If myDirectory is not set, default
				// to the user's "My Documents" directory.
				this.modelFilePath = Environment.GetFolderPath(
					System.Environment.SpecialFolder.Personal);
				this.settingsChanged = true;
			}
			return fileExists;
		}

		public void AddToRecentFileList(string recentFilePath)
		{
			if(recentFilePath.CompareTo(this.recentFile1) == 0)
			{
				return;
			}
			if(recentFilePath.CompareTo(this.recentFile2) == 0)
			{
				return;
			}
			if(recentFilePath.CompareTo(this.recentFile3) == 0)
			{
				return;
			}
			if(recentFilePath.CompareTo(this.recentFile4) == 0)
			{
				return;
			}

			if(this.recentFile1 == null)
			{
				this.recentFile1 = recentFilePath;
				this.settingsChanged = true;
				return;
			}
			if(this.recentFile2 == null)
			{
				this.recentFile2 = recentFilePath;
				this.settingsChanged = true;
				return;
			}
			if(this.recentFile3 == null)
			{
				this.recentFile3 = recentFilePath;
				this.settingsChanged = true;
				return;
			}
			if(this.recentFile4 == null)
			{
				this.recentFile4 = recentFilePath;
				this.settingsChanged = true;
				return;
			}

			this.recentFile4 = this.recentFile3;
			this.recentFile3 = this.recentFile2;
			this.recentFile2 = this.recentFile1;
			this.recentFile1 = recentFilePath;
			this.settingsChanged = true;
		}
		#endregion
	}
}
