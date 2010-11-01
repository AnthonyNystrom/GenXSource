#region Using directives

using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Security;

#endregion

namespace Genetibase.NuGenAnnotation
{
    using StringList = List<String>;
    using StringEnumerator = IEnumerator<String>;

    /// <summary>
    /// MRU manager - manages Most Recently Used Files list
    /// for Windows Form application.
    /// </summary>
    public class MruManager
    {
        #region Members

        // Event raised when user selects file from MRU list
        public event MruFileOpenEventHandler MruOpenEvent;

        private Form ownerForm;                 // owner form

        private ToolStripMenuItem menuItemMRU;  // Recent Files menu item
        private ToolStripMenuItem menuItemParent;        // Recent Files menu item parent

        private string registryPath;            // Registry path to keep MRU list

        private int maxNumberOfFiles = 10;      // maximum number of files in MRU list

        private int maxDisplayLength = 40;      // maximum length of file name for display

        private string currentDirectory;        // current directory

        private StringList mruList;              // MRU list (file names)

        private const string regEntryName = "file";  // entry name to keep MRU (file0, file1...)

        #endregion

        #region Windows API

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern bool PathCompactPathEx(
            StringBuilder pszOut,
            string pszPath,
            int cchMax,
            int reserved);

        #endregion

        #region Constructor

        public MruManager()
        {
            mruList = new StringList();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Maximum length of displayed file name in menu (default is 40).
        /// 
        /// Set this property to change default value (optional).
        /// </summary>
        public int MaxDisplayNameLength
        {
            set
            {
                maxDisplayLength = value;

                if (maxDisplayLength < 10)
                    maxDisplayLength = 10;
            }

            get
            {
                return maxDisplayLength;
            }
        }

        /// <summary>
        /// Maximum length of MRU list (default is 10).
        /// 
        /// Set this property to change default value (optional).
        /// </summary>
        public int MaxMruLength
        {
            set
            {
                maxNumberOfFiles = value;

                if (maxNumberOfFiles < 1)
                    maxNumberOfFiles = 1;

                if (mruList.Count > maxNumberOfFiles)
                    mruList.RemoveRange(maxNumberOfFiles - 1, mruList.Count - maxNumberOfFiles);
            }

            get
            {
                return maxNumberOfFiles;
            }
        }

        /// <summary>
        /// Set current directory.
        /// 
        /// Default value is program current directory which is set when
        /// Initialize function is called.
        /// 
        /// Set this property to change default value (optional)
        /// after call to Initialize.
        /// </summary>
        public string CurrentDir
        {
            set
            {
                currentDirectory = value;
            }

            get
            {
                return currentDirectory;
            }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Initialization. Call this function in form Load handler.
        /// </summary>
        /// <param name="owner">Owner form</param>
        /// <param name="mruItem">Recent Files menu item</param>
        /// <param name="regPath">Registry Path to keep MRU list</param>
        public void Initialize(Form owner, ToolStripMenuItem mruItem, ToolStripMenuItem mruItemParent, string regPath)
        {
            // keep reference to owner form
            ownerForm = owner;

            // keep reference to MRU menu item
            menuItemMRU = mruItem;

            // keep reference to MRU menu item parent
            menuItemParent = mruItemParent;


            // keep Registry path adding MRU key to it
            registryPath = regPath;
            if (registryPath.EndsWith("\\"))
                registryPath += "MRU";
            else
                registryPath += "\\MRU";


            // keep current directory in the time of initialization
            currentDirectory = Directory.GetCurrentDirectory();

            // subscribe to MRU parent Popup event
            menuItemParent.DropDownOpening += new EventHandler(this.OnMRUParentPopup);

            // subscribe to owner form Closing event
            ownerForm.Closing += OnOwnerClosing;

            // load MRU list from Registry
            LoadMRU();
        }

        /// <summary>
        /// Add file name to MRU list.
        /// Call this function when file is opened successfully.
        /// If file already exists in the list, it is moved to the first place.
        /// </summary>
        /// <param name="file">File Name</param>
        public void Add(string file)
        {
            Remove(file);

            // if array has maximum length, remove last element
            if (mruList.Count == maxNumberOfFiles)
                mruList.RemoveAt(maxNumberOfFiles - 1);

            // add new file name to the start of array
            mruList.Insert(0, file);
        }

        /// <summary>
        /// Remove file name from MRU list.
        /// Call this function when File - Open operation failed.
        /// </summary>
        /// <param name="file">File Name</param>
        public void Remove(string file)
        {
            int i = 0;

            StringEnumerator myEnumerator = mruList.GetEnumerator();

            while (myEnumerator.MoveNext())
            {
                if ((string)myEnumerator.Current == file)
                {
                    mruList.RemoveAt(i);
                    return;
                }

                i++;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Update MRU list when MRU menu item parent is opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMRUParentPopup(object sender, EventArgs e)
        {
            // remove all childs
            //if (menuItemMRU.IsParent)
            //{
            menuItemMRU.DropDownItems.Clear();
            //}

            // Disable menu item if MRU list is empty
            if (mruList.Count == 0)
            {
                menuItemMRU.Enabled = false;
                return;
            }

            // enable menu item and add child items
            menuItemMRU.Enabled = true;

            ToolStripMenuItem item;

            StringEnumerator myEnumerator = mruList.GetEnumerator();
            int i = 0;

            while (myEnumerator.MoveNext())
            {
                item = new ToolStripMenuItem();
                item.Text = GetDisplayName((string)myEnumerator.Current);
                item.Tag = i++;

                // subscribe to item's Click event
                item.Click += OnMRUClicked;

                menuItemMRU.DropDownItems.Add(item);
            }
        }

        /// <summary>
        /// MRU menu item is clicked - call owner's OpenMRUFile function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMRUClicked(object sender, EventArgs e)
        {
            string s;

            // cast sender object to MenuItem
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            if (item != null)
            {
                // Get file name from list using item index
                s = (string)mruList[(int)item.Tag];

                // Raise event to owner and pass file name.
                // Owner should handle this event and open file.
                if (s.Length > 0)
                {
                    if (MruOpenEvent != null)
                    {
                        MruOpenEvent(this, new MruFileOpenEventArgs(s));
                    }
                }
            }
        }

        /// <summary>
        /// Save MRU list in Registry when owner form is closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOwnerClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int i, n;

            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);

                if (key != null)
                {
                    n = mruList.Count;

                    for (i = 0; i < maxNumberOfFiles; i++)
                    {
                        key.DeleteValue(regEntryName +
                            i.ToString(CultureInfo.InvariantCulture), false);
                    }

                    for (i = 0; i < n; i++)
                    {
                        key.SetValue(regEntryName +
                            i.ToString(CultureInfo.InvariantCulture), mruList[i]);
                    }
                }

            }
            catch (ArgumentNullException ex) { HandleWriteError(ex); }
            catch (SecurityException ex) { HandleWriteError(ex); }
            catch (ArgumentException ex) { HandleWriteError(ex); }
            catch (ObjectDisposedException ex) { HandleWriteError(ex); }
            catch (UnauthorizedAccessException ex) { HandleWriteError(ex); }
        }


        #endregion

        #region Private Functions

        /// <summary>
        /// Load MRU list from Registry.
        /// Called from Initialize.
        /// </summary>
        private void LoadMRU()
        {
            string sKey, s;

            try
            {
                mruList.Clear();

                RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath);

                if (key != null)
                {
                    for (int i = 0; i < maxNumberOfFiles; i++)
                    {
                        sKey = regEntryName + i.ToString(CultureInfo.InvariantCulture);

                        s = (string)key.GetValue(sKey, "");

                        if (s.Length == 0)
                            break;

                        mruList.Add(s);
                    }
                }
            }
            catch (ArgumentNullException ex) { HandleReadError(ex); }
            catch (SecurityException ex) { HandleReadError(ex); }
            catch (ArgumentException ex) { HandleReadError(ex); }
            catch (ObjectDisposedException ex) { HandleReadError(ex); }
            catch (UnauthorizedAccessException ex) { HandleReadError(ex); }
        }

        /// <summary>
        /// Handle error from OnOwnerClosing function
        /// </summary>
        /// <param name="ex"></param>
        private void HandleReadError(Exception ex)
        {
            Trace.WriteLine("Loading MRU from Registry failed: " + ex.Message);
        }

        /// <summary>
        /// Handle error from OnOwnerClosing function
        /// </summary>
        /// <param name="ex"></param>
        private void HandleWriteError(Exception ex)
        {
            Trace.WriteLine("Saving MRU to Registry failed: " + ex.Message);
        }


        /// <summary>
        /// Get display file name from full name.
        /// </summary>
        /// <param name="fullName">Full file name</param>
        /// <returns>Short display name</returns>
        private string GetDisplayName(string fullName)
        {
            // if file is in current directory, show only file name
            FileInfo fileInfo = new FileInfo(fullName);

            if (fileInfo.DirectoryName == currentDirectory)
                return GetShortDisplayName(fileInfo.Name, maxDisplayLength);

            return GetShortDisplayName(fullName, maxDisplayLength);
        }

        /// <summary>
        /// Truncate a path to fit within a certain number of characters 
        /// by replacing path components with ellipses.
        /// 
        /// This solution is provided by CodeProject and GotDotNet C# expert
        /// Richard Deeming.
        /// 
        /// </summary>
        /// <param name="longName">Long file name</param>
        /// <param name="maxLen">Maximum length</param>
        /// <returns>Truncated file name</returns>
        private string GetShortDisplayName(string longName, int maxLen)
        {
            StringBuilder pszOut = new StringBuilder(maxLen + maxLen + 2);  // for safety

            if (PathCompactPathEx(pszOut, longName, maxLen, 0))
            {
                return pszOut.ToString();
            }
            else
            {
                return longName;
            }
        }

        #endregion

    }

    public delegate void MruFileOpenEventHandler(object sender, MruFileOpenEventArgs e);

    public class MruFileOpenEventArgs : System.EventArgs
    {
        private string fileName;

        public MruFileOpenEventArgs(string fileName)
        {
            this.fileName = fileName;
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
        }
    }
}

#region Using

/*******************************************************************************

NOTE: This class works with new Visual Studio 2005 MenuStrip class.
If owner form has old-style MainMenu, use previous MruManager version.

Using:

1) Add menu item Recent Files (or any name you want) to main application menu.
   This item is used by MruManager as popup menu for MRU list.

2) Write function which opens file selected from MRU:

     private void mruManager_MruOpenEvent(object sender, MruFileOpenEventArgs e)
     {
         // open file e.FileName
     }

3) Add MruManager member to the form class and initialize it:

     private MruManager mruManager;

   Initialize it in the form initialization code:
   
         mruManager = new MruManager();
         mruManager.Initialize(
             this,                              // owner form
             mnuFileMRU,                        // Recent Files menu item (ToolStripMenuItem class)
             mruItemParent,                     // Recent Files menu item parent (ToolStripMenuItem class)
             "Software\\MyCompany\\MyProgram"); // Registry path to keep MRU list

        mruManager.MruOpenEvent += this.mruManager_MruOpenEvent;

        // Optional. Call these functions to change default values:

        mruManager.CurrentDir = ".....";           // default is current directory
        mruManager.MaxMruLength = ...;             // default is 10
        mruMamager.MaxDisplayNameLength = ...;     // default is 40

     NOTES:
     - If Registry path is, for example, "Software\MyCompany\MyProgram",
       MRU list is kept in
       HKEY_CURRENT_USER\Software\MyCompany\MyProgram\MRU Registry entry.

     - CurrentDir is used to show file names in the menu. If file is in
       this directory, only file name is shown.

4) Call MruManager Add and Remove functions when necessary:

       mruManager.Add(fileName);          // when file is successfully opened

       mruManager.Remove(fileName);       // when Open File operation failed

*******************************************************************************/

// Implementation details:
//
// MruManager loads MRU list from Registry in Initialize function.
// List is saved in Registry when owner form is closed.
//
// MRU list in the menu is updated when parent menu is poped-up.
//
// Owner form OnMRUFileOpen function is called when user selects file
// from MRU list.

#endregion
