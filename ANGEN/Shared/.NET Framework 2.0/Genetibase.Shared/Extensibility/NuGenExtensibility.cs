/* -----------------------------------------------
 * NuGenExtensibility.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using EnvDTE;
using EnvDTE80;
using Extensibility;

using Genetibase.WinApi;

using Microsoft.VisualStudio.CommandBars;

using stdole;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace Genetibase.Shared.Extensibility
{
	/// <remarks>
	/// Provides functionality to work with DTE model.
	/// </remarks>
	[CLSCompliant(false)]
	public static class NuGenExtensibility
	{
		#region Declarations.Trace

		private static readonly TraceSwitch _traceSwitch = new TraceSwitch(typeof(NuGenExtensibility).Name, typeof(NuGenExtensibility).FullName);
		private static readonly bool _errorEnabled = _traceSwitch.TraceError;

		#endregion

		#region Methods.Public.Static

		/*
		 * BuildCommandIdentifier
		 */

		/// <summary>
		/// Builds a valid command name to use along with DTE model.
		/// </summary>
		/// <param name="addInType"></param>
		/// <param name="baseCommandIdentifier">Specifies the command identifier used for initialization.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="addInType"/> is <see langword="null"/>.
		/// </exception>
		public static string BuildCommandIdentifier(Type addInType, string baseCommandIdentifier)
		{
			if (addInType == null)
			{
				throw new ArgumentNullException("addInType");
			}

			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", addInType.FullName, baseCommandIdentifier);
		}

		/*
		 * BuildSlnAssociatedPath
		 */

		/// <summary>
		/// <example>
		/// <para>Solution path: C:\SolutionFolder\Solution.sln.</para>
		/// <para>If it is necessary to retrieve the path for a file containing some solution associated data, use
		/// this method to get the following:</para>
		/// <para>Associated file path: C:\SolutionFolder\Solution.ext.</para>
		/// <para>ext - is the associated file extension you path as a second parameter.</para>
		/// </example>
		/// </summary>
		/// 
		/// <param name="solutionPath"></param>
		/// <param name="associatedFileExtension">Can be <see langword="null"/> or an empty string.</param>
		/// 
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="solutionPath"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="solutionPath"/> is an empty string.
		/// </para>
		/// </exception>
		/// <exception cref="PathTooLongException"></exception>
		public static string BuildSlnAssociatedPath(string solutionPath, string associatedFileExtension)
		{
			if (string.IsNullOrEmpty(solutionPath))
			{
				throw new ArgumentNullException("solutionPath");
			}

			return Path.Combine(
				Path.GetDirectoryName(solutionPath),
				string.Format(
					CultureInfo.InvariantCulture,
					"{0}{1}",
					Path.GetFileNameWithoutExtension(solutionPath),
					string.IsNullOrEmpty(associatedFileExtension)
						? ""
						: "." + associatedFileExtension
				)
			);
		}

		/*
		 * CreateAddInCommand
		 */

		/// <summary>
		/// </summary>
		/// <param name="applicationObject"></param>
		/// <param name="addInInstance"></param>
		/// <param name="commandIdentifier"></param>
		/// <param name="commandName"></param>
		/// <param name="commandDescription"></param>
		/// <param name="commandBmpId"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="applicationObject"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="addInInstance"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public static Command CreateAddInCommand(
			DTE2 applicationObject,
			AddIn addInInstance,
			string commandIdentifier,
			string commandName,
			string commandDescription,
			int commandBmpId
			)
		{
			if (applicationObject == null)
			{
				throw new ArgumentNullException("applicationObject");
			}

			if (addInInstance == null)
			{
				throw new ArgumentNullException("addInInstance");
			}

			object[] contextGUIDs = new object[] { };

			Commands2 commands = (Commands2)applicationObject.Commands;
			Debug.Assert(commands != null, "commands != null");

			return commands.AddNamedCommand2(
			   addInInstance,
			   commandIdentifier,
			   commandName,
			   commandDescription,
			   true,
			   commandBmpId,
			   ref contextGUIDs,
			   (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
			   (int)vsCommandStyle.vsCommandStylePictAndText,
			   vsCommandControlType.vsCommandControlTypeButton
		   );
		}

		/*
		 * CreateAddInMenu
		 */

		/// <summary>
		/// </summary>
		/// <param name="menuCommand"></param>
		/// <param name="commandBarToAddMenuTo"></param>
		/// <param name="menuPosition"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="menuCommand"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="commandBarToAddMenuTo"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public static CommandBarControl CreateAddInMenu(
			Command menuCommand,
			CommandBar commandBarToAddMenuTo,
			int menuPosition
			)
		{
			if (menuCommand == null)
			{
				throw new ArgumentNullException("menuCommand");
			}

			if (commandBarToAddMenuTo == null)
			{
				throw new ArgumentNullException("commandBarToAddMenuTo");
			}

			CommandBarControl menuItem = null;

			try
			{
				menuItem = (CommandBarControl)menuCommand.AddControl(commandBarToAddMenuTo, menuPosition);
			}
			catch (Exception e)
			{
				if (_errorEnabled)
				{
					Trace.TraceError(e.Message);
				}

				throw;
			}
			finally
			{
				if (menuItem != null)
				{
					menuItem.Visible = true;
				}
			}

			return menuItem;
		}

		/*
		 * RemoveAddInCommands
		 */

		/// <summary>
		/// </summary>
		/// <param name="addInType"></param>
		/// <param name="applicationObject"></param>
		/// <exception cref="ArgumentNullException">
		///	<para>
		///		<paramref name="addInType"/> is <see langword="null"/>.
		/// </para>
		///	-or-
		///	<para>
		///		<paramref name="applicationObject"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public static void RemoveAddInCommands(Type addInType, DTE2 applicationObject)
		{
			if (addInType == null)
			{
				throw new ArgumentNullException("addInType");
			}

			if (applicationObject == null)
			{
				throw new ArgumentNullException("applicationObject");
			}

			List<Command> commandsToRemove = new List<Command>();

			string addInTypeString = addInType.ToString();

			foreach (Command command in (Commands2)applicationObject.Commands)
			{
				if (command.Name.StartsWith(addInTypeString))
				{
					commandsToRemove.Add(command);
				}
			}

			try
			{
				List<Command>.Enumerator enumerator = commandsToRemove.GetEnumerator();

				try
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.Delete();
					}
				}
				finally
				{
					enumerator.Dispose();
				}
			}
			catch (COMException e)
			{
				if (_errorEnabled)
				{
					Trace.TraceError(e.Message);
				}
			}
		}

		#endregion
	}
}
