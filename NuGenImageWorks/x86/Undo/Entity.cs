using System;
using System.Collections;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Genetibase.UI.NuGenImageWorks.Undo
{
	public abstract class Entity 
	{
		protected ModificationHistory mHistory;
		protected bool mBeingUndone; // flag to denote whether the undo method has been called
		public Entity()
		{
			mHistory = new ModificationHistory(this);
			mBeingUndone = false;
            Program.fireEvent = true;
		}

		/// <summary>
		/// Add to the undo stack. Pass the Property name and the value
		/// from the Property Set method
		/// 
		/// This method delegates the call to the mHistory.Store method 
		/// IF the Undo flag is not set.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="Value"></param>
		protected  void AddHistory(string propertyName, object Value)
		{
			if (!mBeingUndone)
				mHistory.Store(propertyName, Value);
		}

		#region Modification History
		public bool CanUndo
		{
			get
			{
				return mHistory.CanUndo;
			}
		}
		public bool CanRedo
		{
			get
			{
				return mHistory.CanRedo;
			}
		}
		public void Redo()
		{
			if (mHistory.CanRedo)
			{
				mBeingUndone =true;
                Program.fireEvent = false;
				mHistory.Redo();
				mBeingUndone=false;
                Program.fireEvent = true;
			}
		}

		/// <summary>
		/// Set the undo flag, call the undo operation of the 
		/// ModificationHistory object and then reset the undo flag
		/// </summary>
		public void Undo()
		{
			if (mHistory.CanUndo)
			{
				mBeingUndone = true;
                Program.fireEvent = false;
				mHistory.Undo();
				mBeingUndone = false;
                Program.fireEvent = true;
			}
		}
		#endregion

	}
}
