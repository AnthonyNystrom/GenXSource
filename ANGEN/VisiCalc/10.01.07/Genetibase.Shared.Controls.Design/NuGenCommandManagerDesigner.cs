using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
    /// <summary>
    /// Provides basic design-time functionality for the <see cref="NuGenCommandManager"/> class. 
    /// </summary>
    public class NuGenCommandManagerDesigner : System.ComponentModel.Design.ComponentDesigner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenCommandManagerDesigner"/> class. 
        /// </summary>
        public NuGenCommandManagerDesigner()
        {
        }

        /// <summary>
        /// Prepares the designer to view, edit, and design the specified component.
        /// </summary>
        /// <param name="component">The component for this designer.</param>
        public override void Initialize(System.ComponentModel.IComponent component)
        {
            // Always call the base Initialize method in an override of this method.
            base.Initialize(component);
        }

        /// <summary>
        /// Creates a method signature in the source code file for the default event
        /// on the component and navigates the user's cursor to that location.
        /// This method is invoked when the associated component is double-clicked.
        /// </summary>
        public override void DoDefaultAction()
        {
            NuGenEditorServiceContext.EditValue(this, this.Component, "ApplicationCommands");
        }

        /// <summary>
        /// Gets the design-time verbs supported by the component that is associated with the designer.
        /// </summary>
        public override System.ComponentModel.Design.DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerb DesignerVerbEditCommands = new DesignerVerb("Edit Application Commands...", new EventHandler(this.onVerbEditApplicationCommands));
                DesignerVerb DesignerVerbEditContextMenus = new DesignerVerb("Edit Context Menus...", new EventHandler(this.onVerbEditContextMenus));
                DesignerVerbCollection designerVerbCollection = new DesignerVerbCollection();
                designerVerbCollection.Add(DesignerVerbEditCommands);
                designerVerbCollection.Add(DesignerVerbEditContextMenus);
                return designerVerbCollection;
            }
        }

        /// <summary>
        /// Called when the EditApplicationCommands verb is activated.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains no event data.</param>
        private void onVerbEditApplicationCommands(object sender, EventArgs e)
        {
            NuGenEditorServiceContext.EditValue(this, this.Component, "ApplicationCommands");
        }

        // Event handling method for the designer verb
        /// <summary>
        /// Called when the EditContextMenus verb is activated.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains no event data.</param>
        private void onVerbEditContextMenus(object sender, EventArgs e)
        {
            NuGenEditorServiceContext.EditValue(this, this.Component, "ContextMenus");
        }
    }
}
