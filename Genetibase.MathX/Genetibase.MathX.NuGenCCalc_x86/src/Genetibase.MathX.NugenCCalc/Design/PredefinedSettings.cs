using System;
using System.IO;
using System.Xml.Serialization;
using System.Reflection ;

namespace Genetibase.MathX.NugenCCalc.Design
{
	/// <summary>
	/// Store predefined settings
	/// </summary>
	[Serializable()]
	public class PredefinedSettings
	{
        #region Private Instance Fields

		private static object lockFlag = new object();
		private static PredefinedSettings instance;

		private FunctionCollection _expressions = new FunctionCollection();
		private FunctionCollection _equations = new FunctionCollection();

        #endregion Private Instance Fields


		#region Public Static Method
		/// <summary>
		/// Load predefined settings from XML file
		/// </summary>
		public static void LoadPredefinedSettings()
		{
			InitDataFolder();
            InitSettings();

			try
			{
				//try load file from disk and deserialize him
				using( FileStream fs = 
						  new FileStream(DataFolder + "predefineItems.xml",FileMode.Open))
				{
					System.Xml.Serialization.XmlSerializer xs = 
						new System.Xml.Serialization.XmlSerializer(typeof(PredefinedSettings));
					instance = (PredefinedSettings)xs.Deserialize(fs);
				}
			}
			catch(FileNotFoundException)
			{
				instance = new PredefinedSettings();
				SavePredefinedSettings();
			}
			catch
			{
				instance = new PredefinedSettings();							
			}
		}



		/// <summary>
		/// Save predefined settings to XML file
		/// </summary>
		public static void SavePredefinedSettings()
		{
			//Create stream
			using(FileStream fs = 
					  new FileStream(DataFolder + "predefineItems.xml",FileMode.Create))
			{
				try
				{
					//Serialize it
					System.Xml.Serialization.XmlSerializer xs = 
						new System.Xml.Serialization.XmlSerializer(typeof(PredefinedSettings));
					xs.Serialize(fs, instance);
				}
				catch(Exception ex)
				{
					System.Windows.Forms.MessageBox.Show(ex.Message);
				}

			}
		}



		/// <summary>
        /// Set data folder for component as ApplicationData\\Genetibase\\NuGenCCalc\\
		/// </summary>
		private static void InitDataFolder()
		{
            _dataFolder = String.Format("{0}\\Genetibase\\NuGenCCalc\\",
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

			if (!Directory.Exists(_dataFolder))
				Directory.CreateDirectory(_dataFolder);
		}

        private static void InitSettings()
        {
            if (!File.Exists(DataFolder + "predefineItems.xml"))
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Genetibase.MathX.NugenCCalc.Design.predefineItems.xml");
                using (Stream output = new FileStream(DataFolder + "predefineItems.xml", FileMode.CreateNew )) 
                { 
                    byte[] buffer = new byte[32*1024]; 
                    int read; 
                    while ( (read=stream.Read(buffer, 0, buffer.Length)) > 0) 
                    { 
                        output.Write(buffer, 0, read); 
                    } 
                }
            }
        }


		#endregion


        #region Public Instance Constructors


		public PredefinedSettings()
		{
			InitDataFolder();
		}

        #endregion Public Instance Constructors


        #region Public Instance Properties

		//Thread-safe Singleton instance
		[XmlIgnore]
		public static PredefinedSettings Instance
		{
			get
			{
				lock(lockFlag) 
				{
					if(instance == null)
					{
						LoadPredefinedSettings();
					}
				}
				return instance;
			}
			set
			{
				instance=value;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public FunctionCollection Expressions
		{
			get
			{
				return _expressions;
			}
			set
			{
				if (_expressions != value)
				{
					_expressions = value;
				}
			}		
		}


		/// <summary>
		/// EquationCollection
		/// </summary>
		public FunctionCollection Equations
		{
			get
			{
				return _equations;
			}
			set
			{
				if (_equations != value)
				{
					_equations = value;
				}
			}		
		}

		#endregion Public Instance Properties

		private static string _dataFolder;

		/// <summary>
		/// Return data folder for Math X components
		/// </summary>
		public static string DataFolder
		{
			get
			{
				return _dataFolder;
			}
		}
	}
}
