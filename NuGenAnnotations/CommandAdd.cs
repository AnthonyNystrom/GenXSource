using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.NuGenAnnotation
{
    /// <summary>
    /// Add new object command
    /// </summary>
    class CommandAdd : Command
    {
        DrawObject drawObject;

        // Create this command with DrawObject instance added to the list
        public CommandAdd(DrawObject drawObject) : base()
        {
            // Keep copy of added object
            this.drawObject = drawObject.Clone();
        }

		/// <summary>
		/// Undo last Add command
		/// </summary>
		/// <param name="list">Layers collection</param>
		public override void Undo(Layers list)
		{
			list[list.ActiveLayerIndex].Graphics.DeleteLastAddedObject();
		}
		/// <summary>
		/// Redo last Add command
		/// </summary>
		/// <param name="list">Layers collection</param>
		public override void Redo(Layers list)
		{
			list[list.ActiveLayerIndex].Graphics.UnselectAll();
			list[list.ActiveLayerIndex].Graphics.Add(drawObject);
		}
    }
}
