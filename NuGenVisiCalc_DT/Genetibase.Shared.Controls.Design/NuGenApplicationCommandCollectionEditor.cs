using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;

namespace Genetibase.Shared.Controls.Design
{
    /// <summary>
    /// Provides a user interface that can edit a <see cref="NuGenApplicationCommandCollection"/> at design time.
    /// </summary>
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    internal sealed class NuGenApplicationCommandCollectionEditor : CollectionEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionEditor"/> class using the specified collection type.
        /// </summary>
        /// <param name="type">The type of the collection for this editor to edit.</param>
        public NuGenApplicationCommandCollectionEditor(Type type)
            : base(type)
        {
        }

        /// <summary>
        /// Creates a new instance of the specified collection item type.
        /// </summary>
        /// <param name="itemType">The type of item to create.</param>
        /// <returns>A new instance of the specified object.</returns>
        protected override object CreateInstance(Type itemType)
        {
            NuGenApplicationCommand applicationCommand = base.CreateInstance(itemType) as NuGenApplicationCommand;
            NuGenCommandManagerBase commandManager = Context.Instance as NuGenCommandManagerBase;
            applicationCommand.CommandManager = commandManager;

            return applicationCommand;
        }
    }
}
