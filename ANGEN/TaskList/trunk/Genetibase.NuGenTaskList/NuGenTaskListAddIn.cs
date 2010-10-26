/* -----------------------------------------------
 * NuGenTaskListAddIn.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using EnvDTE;
using EnvDTE80;
using Extensibility;

using Genetibase.NuGenTaskList.Properties;
using Genetibase.Shared;
using Genetibase.Shared.Extensibility;

using Microsoft.VisualStudio.CommandBars;

using stdole;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// Extended multilevel Task List for Microsoft Visual Studio 2005.
	/// </summary>
	public class NuGenTaskListAddIn : IDTExtensibility2, IDTCommandTarget
	{
		#region Declarations.Fields

		private DTE2 _applicationObject = null;
		private AddIn _addInInstance = null;
		private Window _addInUI = null;
		private SolutionEvents _solution = null;
		private NuGenTaskListUI _taskListUI = null;
		private string _taskListIdentifier = "";
		private CommandBarControl _taskListMenuItem = null;

		#endregion

		#region Declarations.Trace

		private static readonly TraceSwitch _TraceSwitch = new TraceSwitch(typeof(NuGenTaskListAddIn).Name, typeof(NuGenTaskListAddIn).FullName);
		private static readonly bool _ErrorEnabled = _TraceSwitch.TraceError;

		#endregion

		#region Methods.Public

		/*
		 * OnConnection
		 */

		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			if (application == null)
			{
				throw new ArgumentNullException("application");
			}

			if (addInInst == null)
			{
				throw new ArgumentNullException("addInInst");
			}

			if (application is DTE2)
			{
				_applicationObject = (DTE2)application;
			}
			else
			{
				throw new ArgumentException(Resources.Argument_InvalidApplicationType);
			}

			if (addInInst is AddIn)
			{
				_addInInstance = (AddIn)addInInst;
			}
			else
			{
				throw new ArgumentException(Resources.Argument_InvalidAddInInstanceType);
			}

			_taskListIdentifier = NuGenExtensibility.BuildCommandIdentifier(this.GetType(), Resources.Menu_TaskList_Identifier);

			Events applicationObjectEvents = _applicationObject.Events;
			Debug.Assert(applicationObjectEvents != null, "applicationObjectEvents != null");

			_solution = applicationObjectEvents.SolutionEvents;
			Debug.Assert(_solution != null, "_Solution != null");

			_solution.BeforeClosing += _Solution_BeforeClosing;
			_solution.Opened += _Solution_Opened;

			switch (connectMode)
			{
				case ext_ConnectMode.ext_cm_UISetup:
				{
					try
					{
						NuGenExtensibility.RemoveAddInCommands(this.GetType(), _applicationObject);
					}
					catch (Exception e)
					{
						if (_ErrorEnabled)
						{
							Trace.WriteLine("Error occured while removing add-in commands: " + e.Message);
						}
					}

					try
					{
						NuGenExtensibility.CreateAddInCommand(
							_applicationObject,
							_addInInstance,
							Resources.Menu_TaskList_Identifier,
							Resources.Menu_TaskList_Name,
							Resources.Menu_TaskList_Description,
							439
						);
					}
					catch (Exception e)
					{
						if (_ErrorEnabled)
						{
							Trace.WriteLine("Error occured while creating add-in commands: " + e.Message);
						}
					}

					break;
				}
				case ext_ConnectMode.ext_cm_AfterStartup:
				case ext_ConnectMode.ext_cm_Startup:
				{
					CommandBar menuBar = ((CommandBars)_applicationObject.CommandBars)["MenuBar"];
					Debug.Assert(menuBar != null, "menuBar != null");

					CommandBarPopup menuPopup = (CommandBarPopup)menuBar.Controls[this.GetFullMenuName(Resources.Menu_ParentMenu_Name)];
					Debug.Assert(menuPopup != null, "menuPopup != null");

					CommandBar menuCommandBar = menuPopup.CommandBar;
					Debug.Assert(menuCommandBar != null, "menuCommandBar != null");

					Command taskListCommand = null;

					try
					{
						taskListCommand = _applicationObject.Commands.Item(_taskListIdentifier, -1);
					}
					catch (ArgumentException)
					{
						NuGenExtensibility.RemoveAddInCommands(this.GetType(), _applicationObject);
						taskListCommand = NuGenExtensibility.CreateAddInCommand(
							_applicationObject,
							_addInInstance,
							Resources.Menu_TaskList_Identifier,
							Resources.Menu_TaskList_Name,
							Resources.Menu_TaskList_Description,
							439
						);
					}

					Debug.Assert(taskListCommand != null, "taskListCommand != null");

					try
					{
						_taskListMenuItem = NuGenExtensibility.CreateAddInMenu(
							taskListCommand,
							menuCommandBar,
							1
						);
					}
					catch (Exception e)
					{
						Trace.WriteLineIf(
							_ErrorEnabled,
							"Error occured while creating menu item.: " + e.Message
						);
					}

					try
					{
						_addInUI = this.CreateToolWindow(
							_addInInstance,
							typeof(NuGenTaskListUI),
							Resources.ToolWindow_Caption
						);
					}
					catch (Exception e)
					{
						Trace.WriteLineIf(
							_ErrorEnabled,
							"Error occured while creating tool window: " + e.Message
						);
					}

					break;
				}
			}
		}

		/*
		 * OnDisconnection
		 */

		/// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
		/// <param term="disconnectMode">Describes how the Add-in is being unloaded.</param>
		/// <param term="custom">Array of parameters that are host application specific.</param>
		/// <seealso class="IDTExtensibility2" />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
			if (_taskListUI != null && !_taskListUI.IsDisposed)
			{
				_taskListUI.Dispose();
			}

			if (_solution != null)
			{
				_solution.BeforeClosing -= _Solution_BeforeClosing;
				_solution.Opened -= _Solution_Opened;
			}

			if (_taskListMenuItem != null)
			{
				try
				{
					_taskListMenuItem.Delete(Type.Missing);
				}
				catch (Exception e)
				{
					Trace.WriteLineIf(
						_ErrorEnabled,
						"Error occured while deleting menu item: " + e.Message
						);
				}
			}
		}

		/*
		 * OnAddInsUpdate
		 */

		/// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
		/// <param term="custom">Array of parameters that are host application specific.</param>
		/// <seealso class="IDTExtensibility2" />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		/*
		 * OnStartupComplete
		 */

		/// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
		/// <param term="custom">Array of parameters that are host application specific.</param>
		/// <seealso class="IDTExtensibility2" />
		public void OnStartupComplete(ref Array custom)
		{
		}

		/*
		 * OnBeginShutdown
		 */

		/// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
		/// <param term="custom">Array of parameters that are host application specific.</param>
		/// <seealso class="IDTExtensibility2" />
		public void OnBeginShutdown(ref Array custom)
		{
			if (_taskListUI != null && !_taskListUI.IsDisposed)
			{
				_taskListUI.Dispose();
			}
		}

		/*
		 * Exec
		 */

		/// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
		/// <param term="commandName">The name of the command to execute.</param>
		/// <param term="executeOption">Describes how the command should be run.</param>
		/// <param term="varIn">Parameters passed from the caller to the command handler.</param>
		/// <param term="varOut">Parameters passed from the command handler to the caller.</param>
		/// <param term="handled">Informs the caller if the command was handled or not.</param>
		/// <seealso class="Exec" />
		public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
		{
			handled = false;

			if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
			{
				if (commandName == _taskListIdentifier)
				{
					if (_addInUI != null)
					{
						_addInUI.Visible = true;
					}

					handled = true;
				}
			}
		}

		/*
		 * QueryStatus
		 */

		/// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
		/// <param term="commandName">The name of the command to determine state for.</param>
		/// <param term="neededText">Text that is needed for the command.</param>
		/// <param term="status">The state of the command in the user interface.</param>
		/// <param term="commandText">Text requested by the neededText parameter.</param>
		/// <seealso class="Exec" />
		public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
		{
			if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
			{
				if (commandName == _taskListIdentifier)
				{
					status = vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
				}
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * CreateToolWindow
		 */

		private Window CreateToolWindow(AddIn addIn, Type toolWindowType, string toolWindowCaption)
		{
			Debug.Assert(addIn != null, "addIn != null");
			Debug.Assert(toolWindowType != null, "toolWindowType != null");

			Window toolWindow = null;

			object controlObject = null;
			Windows2 toolWindows = (Windows2)_applicationObject.Windows;

			toolWindow = toolWindows.CreateToolWindow2(
				addIn,
				Assembly.GetExecutingAssembly().Location,
				toolWindowType.FullName,
				toolWindowCaption,
				"{7ED46EA6-8E0D-4116-ABFE-0EEED142BF1E}",
				ref controlObject
			);

			Debug.Assert(toolWindow != null, "toolWindow != null");
			Debug.Assert(controlObject is NuGenTaskListUI, "controlObject is NuGenTaskListUI");

			_taskListUI = (NuGenTaskListUI)controlObject;
			_taskListUI.DTE = _applicationObject;
			_taskListUI.Load += _TaskListUI_Load;

			toolWindow.SetTabPicture(Resources.ToolWindow_TaskList.GetHbitmap());

			/* It is necessary to set the value of the Visible property to true; otherwise,
			 * exceptions are thrown.
			 */
			toolWindow.Visible = true;

			toolWindow.Width = 250;
			toolWindow.Height = 200;

			return toolWindow;
		}

		/*
		 * GetFullMenuName
		 */

		private string GetFullMenuName(string menuName)
		{
			string fullMenuName = "";

			try
			{
				ResourceManager resourceManager = new ResourceManager(
					"Genetibase.NuGenTaskList.CommandBar",
					Assembly.GetExecutingAssembly()
				);

				CultureInfo cultureInfo = new CultureInfo(_applicationObject.LocaleID);
				string resourceName = string.Concat(
					cultureInfo.TwoLetterISOLanguageName,
					menuName
					);

				fullMenuName = resourceManager.GetString(resourceName);
			}
			catch
			{
				fullMenuName = menuName;
			}

			return fullMenuName;
		}

		/*
		 * LoadTaskList
		 */

		private void LoadTaskList()
		{
			string solutionPath = _applicationObject.Solution.FullName;

			if (!string.IsNullOrEmpty(solutionPath))
			{
				string taskListPath = NuGenExtensibility.BuildSlnAssociatedPath(
					solutionPath,
					Resources.File_Extension
				);

				Debug.Assert(_taskListUI != null, "_TaskListUI != null");

				if (_taskListUI != null)
				{
					_taskListUI.Restore(taskListPath);
				}
			}
		}

		#endregion

		#region EventHandlers.Solution

		private void _Solution_BeforeClosing()
		{
			string solutionPath = _applicationObject.Solution.FullName;

			if (!string.IsNullOrEmpty(solutionPath))
			{
				Debug.Assert(_taskListUI != null, "_TaskListUI != null");

				if (_taskListUI != null)
				{
					_taskListUI.Save(
						NuGenExtensibility.BuildSlnAssociatedPath(
							solutionPath,
							Resources.File_Extension
						)
					);
				}
			}
		}

		private void _Solution_Opened()
		{
			this.LoadTaskList();
		}

		#endregion

		#region EventHandlers.TaskListUI

		private void _TaskListUI_Load(object sender, EventArgs e)
		{
			this.LoadTaskList();
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskListAddIn"/> class.
		/// </summary>
		public NuGenTaskListAddIn()
		{
		}

		#endregion
	}
}
