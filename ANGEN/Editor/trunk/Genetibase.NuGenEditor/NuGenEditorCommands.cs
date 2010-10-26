/* -----------------------------------------------
 * NuGenEditorCommands.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Genetibase.Shared;

namespace Genetibase.Windows.Controls
{
    /// <summary>
    /// Provides a standard set of <see cref="NuGenEditor"/> related commands.
    /// </summary>
    public static partial class NuGenEditorCommands
    {
        private static RoutedUICommand _splitContainer;

        /// <summary>
        /// Gets the value that represents the Split Container command.
        /// </summary>
        public static RoutedUICommand SplitContainer
        {
            get
            {
                return EnsureCommand(ref _splitContainer, "SplitContainer");
            }
        }

        private static RoutedUICommand EnsureCommand(ref RoutedUICommand command, String commandPropertyName)
        {
            lock (_synchronize)
            {
                if (command == null)
                {
                    command = new RoutedUICommand(commandPropertyName, commandPropertyName, typeof(NuGenEditorCommands));
                }
            }

            return command;
        }

        private static Object _synchronize = new Object();
    }
}
