using System;
using System.Collections;
using System.Reflection;

namespace Genetibase.UI.NuGenImageWorks.Undo
{
	/// <summary>
	/// Summary description for ModificationHistory.
	/// Maintains the stack of redo and undo logs
	/// Each entry in the undo stack represents a single property change
	/// </summary>
	[Serializable]
	public class ModificationHistory
	{
		protected Stack redoStack;
        protected Stack undoStack;
        protected object objParent;

		/// <summary>
		/// Constructor requires the reference of the parent
		/// object that will subscribe to the Undo management
		/// </summary>
		/// <param name="parent"></param>
		public ModificationHistory(object parent)
		{
			redoStack = new Stack();
			undoStack = new Stack();
			objParent = parent;
		}

		/// <summary>
		/// This method would be called by the parent class
		/// in order to store the value of a property prior
		/// to it being changed
		/// </summary>
		/// <param name="propName">Name of the property</param>
		/// <param name="propVal">The value prior to change</param>
		public void Store (string propName, object propVal)
		{
			PropertyValue pVal = new PropertyValue(propName, propVal);
			undoStack.Push(pVal);
		}

		/// <summary>
		/// Apply the undo information
		/// </summary>
		public void Undo()
		{
			if (undoStack.Count>0)
			{
				PropertyValue pVal = (PropertyValue)undoStack.Pop();
				Type compType = objParent.GetType();
				PropertyInfo pinfo = compType.GetProperty(pVal.PropertyName);

				//first get the existing value and push to redo stack
				object oldVal = pinfo.GetValue(objParent,null);
				PropertyValue pRedo = new PropertyValue(pVal.PropertyName, oldVal);
				redoStack.Push(pRedo);			
				// apply the value that was popped off the undo stack
				pinfo.SetValue(objParent,pVal.Value, null);
			}
		}
		public void Redo()
		{
			if (redoStack.Count>0)
			{
				PropertyValue pVal = (PropertyValue)redoStack.Pop();
				Type compType = objParent.GetType();
				PropertyInfo pinfo = compType.GetProperty(pVal.PropertyName);

				//first get the existing value and push to undo stack
				object oldVal = pinfo.GetValue(objParent,null);
				PropertyValue pUndo = new PropertyValue(pVal.PropertyName, oldVal);
				undoStack.Push(pUndo);			
				// apply the value that was popped off the redo stack
				pinfo.SetValue(objParent,pVal.Value, null);
			}
		}

		public bool CanUndo
		{
			get
			{
				return (undoStack.Count>0?true:false);
			}
		}
		public bool CanRedo
		{
			get
			{
				return (redoStack.Count>0?true:false);
			}
		}

        public void ClearHistory()
        {
            redoStack.Clear();
            undoStack.Clear();
        }
	}
}
