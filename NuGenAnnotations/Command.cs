using System;
using System.Collections.Generic;
using System.Text;

/// Undo-Redo code is written using the article:
/// http://www.codeproject.com/cs/design/commandpatterndemo.asp
//  The Command Pattern and MVC Architecture
//  By David Veeneman.

namespace Genetibase.NuGenAnnotation
{
    /// <summary>
    /// Base class for commands used for Undo - Redo
    /// </summary>
    public abstract class Command
    {
        // This function is used to make Undo operation.
        // It makes action opposite to the original command.
		//public abstract void Undo(GraphicsList list);
		public abstract void Undo(Layers list);
        // This command is used to make Redo operation.
        // It makes original command again.
		//public abstract void Redo(GraphicsList list);
		public abstract void Redo(Layers list);
        // Derived classes have members which contain enough information
        // to make Undo and Redo operations for every specific command.
    }
}
