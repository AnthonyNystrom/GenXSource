using System;
using System.Collections.Generic;
using System.Text;

using Dile.UI;
using Dile.UI.Debug;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Dile.Configuration
{
	public class NuGenSettings
	{
		public event NoArgumentsDelegate RecentAssembliesChanged;
		public event NoArgumentsDelegate RecentProjectsChanged;
		public event NoArgumentsDelegate DisplayHexaNumbersChanged;
		public event NoArgumentsDelegate Changed;

		private const string ConfigurationFileName = "Dile.settings.xml";

		#region Singleton pattern
		private static readonly NuGenSettings instance = LoadConfiguration();
		public static NuGenSettings Instance
		{
			get
			{
				return instance;
			}
		}
		#endregion

		private static string configurationFilePath;
		private static string ConfigurationFilePath
		{
			get
			{
				return configurationFilePath;
			}
			set
			{
				configurationFilePath = value;
			}
		}

		public static NuGenSerializableFont DefaultFont
		{
			get
			{
				NuGenSerializableFont result = new NuGenSerializableFont();
				result.FamilyName = "Tahoma";
				result.FontStyle = FontStyle.Regular;
				result.GdiCharset = 1;
				result.GdiVerticalFont = false;
				result.GraphicsUnit = GraphicsUnit.World;
				result.Size = 11;

				return result;
			}
		}

		public static NuGenSerializableFont DefaultCodeEditorFont
		{
			get
			{
				NuGenSerializableFont result = new NuGenSerializableFont();
				result.FamilyName = "Courier New";
				result.FontStyle = FontStyle.Regular;
				result.GdiCharset = 0xee;
				result.GdiVerticalFont = false;
				result.GraphicsUnit = GraphicsUnit.Point;
				result.Size = 9;

				return result;
			}
		}

		private List<string> recentAssemblies = new List<string>();
		public List<string> RecentAssemblies
		{
			get
			{
				return recentAssemblies;
			}
			set
			{
				recentAssemblies = value;
			}
		}

		private List<string> recentProjects = new List<string>();
		public List<string> RecentProjects
		{
			get
			{
				return recentProjects;
			}
			set
			{
				recentProjects = value;
			}
		}

		private bool isLoadClassEnabled = true;
		public bool IsLoadClassEnabled
		{
			get
			{
				return isLoadClassEnabled;
			}
			set
			{
				isLoadClassEnabled = value;
			}
		}

		private bool warnUnloadedAssembly = true;
		public bool WarnUnloadedAssembly
		{
			get
			{
				return warnUnloadedAssembly;
			}
			set
			{
				warnUnloadedAssembly = value;
			}
		}

		private bool stopOnException = true;
		public bool StopOnException
		{
			get
			{
				return stopOnException;
			}
			set
			{
				stopOnException = value;
			}
		}

		private bool stopOnlyOnUnhandledException = false;
		public bool StopOnlyOnUnhandledException
		{
			get
			{
				return stopOnlyOnUnhandledException;
			}
			set
			{
				stopOnlyOnUnhandledException = value;
			}
		}

		private bool stopOnMdaNotification = true;
		public bool StopOnMdaNotification
		{
			get
			{
				return stopOnMdaNotification;
			}
			set
			{
				stopOnMdaNotification = value;
			}
		}

		private bool displayHexaNumbers = true;
		public bool DisplayHexaNumbers
		{
			get
			{
				return displayHexaNumbers;
			}
			set
			{
				bool valueChanged = (displayHexaNumbers != value);
				displayHexaNumbers = value;

				if (valueChanged && DisplayHexaNumbersChanged != null)
				{
					DisplayHexaNumbersChanged();
				}
			}
		}

		private bool detachOnQuit = true;
		public bool DetachOnQuit
		{
			get
			{
				return detachOnQuit;
			}
			set
			{
				detachOnQuit = value;
			}
		}

		private int maxRecentAssembliesCount = 10;
		public int MaxRecentAssembliesCount
		{
			get
			{
				return maxRecentAssembliesCount;
			}
			set
			{
				bool isValueDecreased = (value < maxRecentAssembliesCount);

				maxRecentAssembliesCount = value;

				if (isValueDecreased)
				{
					RecentAssemblies.RemoveRange(value, RecentAssemblies.Count - value);
					OnRecentAssembliesChanged();
				}
			}
		}

		private int maxRecentProjectsCount = 10;
		public int MaxRecentProjectsCount
		{
			get
			{
				return maxRecentProjectsCount;
			}
			set
			{
				bool isValueDecreased = (value < maxRecentProjectsCount);

				maxRecentProjectsCount = value;

				if (isValueDecreased)
				{
					RecentProjects.RemoveRange(value, RecentProjects.Count - value);
					OnRecentProjectsChanged();
				}
			}
		}

		private string defaultAssemblyDirectory = string.Empty;
		public string DefaultAssemblyDirectory
		{
			get
			{
				return defaultAssemblyDirectory;
			}
			set
			{
				defaultAssemblyDirectory = value;
			}
		}

		private string defaultProjectDirectory = string.Empty;
		public string DefaultProjectDirectory
		{
			get
			{
				return defaultProjectDirectory;
			}
			set
			{
				defaultProjectDirectory = value;
			}
		}

		private List<NuGenMenuFunctionShortcut> shortcuts = new List<NuGenMenuFunctionShortcut>();
		public List<NuGenMenuFunctionShortcut> Shortcuts
		{
			get
			{
				return shortcuts;
			}
			set
			{
				shortcuts = value;
			}
		}

		private List<NuGenPanelDisplayer> panels = new List<NuGenPanelDisplayer>();
		public List<NuGenPanelDisplayer> Panels
		{
			get
			{
				return panels;
			}
			set
			{
				panels = value;
			}
		}

		private NuGenCodeEditorFontSettings codeEditorFont;
		public NuGenCodeEditorFontSettings CodeEditorFont
		{
			get
			{
				return codeEditorFont;
			}
			set
			{
				codeEditorFont = value;
			}
		}

		private DebugEventType displayedDebugEvents = DebugEventType.AllSet;
		public DebugEventType DisplayedDebugEvents
		{
			get
			{
				return displayedDebugEvents;
			}
			set
			{
				displayedDebugEvents = value;
			}
		}

		private int funcEvalTimeout = 5;
		public int FuncEvalTimeout
		{
			get
			{
				return funcEvalTimeout;
			}
			set
			{
				funcEvalTimeout = value;
			}
		}

		private int funcEvalAbortTimeout = 30;
		public int FuncEvalAbortTimeout
		{
			get
			{
				return funcEvalAbortTimeout;
			}
			set
			{
				funcEvalAbortTimeout = value;
			}
		}

		private NuGenSettings()
		{
		}

		private static NuGenSettings LoadConfiguration()
		{
			NuGenSettings result = null;
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			ConfigurationFilePath = Path.Combine(Path.GetDirectoryName(executingAssembly.Location), ConfigurationFileName);

			if (File.Exists(ConfigurationFilePath))
			{
				FileStream fileStream = null;

				try
				{
					fileStream = new FileStream(ConfigurationFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
					XmlSerializer serializer = new XmlSerializer(typeof(NuGenSettings));
					result = (NuGenSettings)serializer.Deserialize(fileStream);
				}
				catch (Exception exception)
				{
					NuGenUIHandler.Instance.ShowException(exception);
					NuGenUIHandler.Instance.DisplayUserWarning("Unable to read the configuration file.");
					result = new NuGenSettings();
				}
				finally
				{
					if (fileStream != null)
					{
						fileStream.Close();
					}
				}
			}
			else
			{
				result = new NuGenSettings();
			}

			return result;
		}

		public static void SaveConfiguration()
		{
			FileStream fileStream = null;

			try
			{
				fileStream = new FileStream(ConfigurationFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
				XmlSerializer serializer = new XmlSerializer(typeof(NuGenSettings));
				serializer.Serialize(fileStream, Instance);
			}
			catch (Exception exception)
			{
				NuGenUIHandler.Instance.ShowException(exception);
				NuGenUIHandler.Instance.DisplayUserWarning("Unable to save the configuration file.");
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}

		private static void SaveShortcuts(MainMenu.MenuItemCollection menuItems)
		{
			foreach (MenuItem menuItem in menuItems)
			{
				NuGenBaseMenuInformation menuInformation = menuItem.Tag as NuGenBaseMenuInformation;

				if (menuInformation != null)
				{
					MenuFunction menuFunction = menuInformation.MenuFunction;
					NuGenMenuFunctionShortcut menuFunctionShortcut = new NuGenMenuFunctionShortcut();
					menuFunctionShortcut.MenuFunction = menuFunction;
					menuFunctionShortcut.Shortcut = menuItem.Shortcut;

					NuGenMenuFunctionShortcut existingShortcut = Instance.FindMenuFunctionShortcut(menuFunction);

					if (existingShortcut != null)
					{
						Instance.Shortcuts.Remove(existingShortcut);
					}

					Instance.Shortcuts.Add(menuFunctionShortcut);
				}

				if (menuItem.MenuItems != null && menuItem.MenuItems.Count > 0)
				{
					SaveShortcuts(menuItem.MenuItems);
				}
			}
		}

		public static void SaveConfiguration(MainMenu mainMenu)
		{
			SaveShortcuts(mainMenu.MenuItems);
			SaveConfiguration();
		}

		private void OnRecentAssembliesChanged()
		{
			if (RecentAssembliesChanged != null)
			{
				RecentAssembliesChanged();
			}
		}

		private void OnRecentProjectsChanged()
		{
			if (RecentProjectsChanged != null)
			{
				RecentProjectsChanged();
			}
		}

		private void OnChanged()
		{
			if (Changed != null)
			{
				Changed();
			}
		}

		public void SettingsUpdated()
		{
			OnChanged();
		}

		public void AddProject(string projectFilePath)
		{
			NuGenHelperFunctions.AddItemToList<string>(RecentProjects, projectFilePath, MaxRecentProjectsCount);
			SaveConfiguration();
			OnRecentProjectsChanged();
		}

		public void AddAssembly(string assemblyFilePath)
		{
			NuGenHelperFunctions.AddItemToList<string>(RecentAssemblies, assemblyFilePath, MaxRecentAssembliesCount);
			SaveConfiguration();
			OnRecentAssembliesChanged();
		}

		public void MoveAssemblyToFirst(string assemblyFilePath)
		{
			if (NuGenHelperFunctions.MoveItemInList<string>(RecentAssemblies, assemblyFilePath, 0))
			{
				SaveConfiguration();
				OnRecentAssembliesChanged();
			}
		}

		public void MoveProjectToFirst(string projectFilePath)
		{
			if (NuGenHelperFunctions.MoveItemInList<string>(RecentProjects, projectFilePath, 0))
			{
				SaveConfiguration();
				OnRecentProjectsChanged();
			}
		}

		public void AddAssemblies(string[] assemblyFilePaths)
		{
			NuGenHelperFunctions.AddItemsToList<string>(RecentAssemblies, assemblyFilePaths, MaxRecentAssembliesCount);
			SaveConfiguration();
			OnRecentAssembliesChanged();
		}

		private NuGenMenuFunctionShortcut FindMenuFunctionShortcut(MenuFunction menuFunction)
		{
			NuGenMenuFunctionShortcut result = null;
			int index = 0;

			while (result == null && index < Shortcuts.Count)
			{
				NuGenMenuFunctionShortcut menuFunctionShortcut = Shortcuts[index++];

				if (menuFunctionShortcut.MenuFunction == menuFunction)
				{
					result = menuFunctionShortcut;
				}
			}

			return result;
		}

		private void UpdateShortcuts(MainMenu.MenuItemCollection menuItems)
		{
			foreach (MenuItem menuItem in menuItems)
			{
				NuGenBaseMenuInformation menuInformation = menuItem.Tag as NuGenBaseMenuInformation;

				if (menuInformation != null)
				{
					MenuFunction menuFunction = menuInformation.MenuFunction;
					NuGenMenuFunctionShortcut menuFunctionShortcut = FindMenuFunctionShortcut(menuFunction);

					if (menuFunctionShortcut != null)
					{
						menuItem.Shortcut = menuFunctionShortcut.Shortcut;
					}
				}

				if (menuItem.MenuItems != null && menuItem.MenuItems.Count > 0)
				{
					UpdateShortcuts(menuItem.MenuItems);
				}
			}
		}

		public void UpdateShortcuts(MainMenu mainMenu)
		{
			UpdateShortcuts(mainMenu.MenuItems);
		}

		public bool DisplayDebugEvent(DebugEventType debugEventType)
		{
			return ((DisplayedDebugEvents & debugEventType) == debugEventType);
		}
	}
}