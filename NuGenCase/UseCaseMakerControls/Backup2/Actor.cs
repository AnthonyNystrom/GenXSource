using System;

namespace UseCaseMakerLibrary
{
	public class Actor : IdentificableObject
	{
		#region Class Members
		private CommonAttributes commonAttributes = new CommonAttributes();
		private Goals goals = new Goals();
		#endregion

		#region Constructors
		internal Actor()
			: base()
		{
		}

		internal Actor(String name, String prefix, Int32 id)
			: base(name,prefix,id)
		{
		}

		internal Actor(String name, String prefix, Int32 id, Package owner)
			: base(name,prefix,id,owner)
		{
		}
		#endregion

		#region Public Properties
		public CommonAttributes Attributes
		{
			get
			{
				return this.commonAttributes;
			}
		}
		#endregion

		#region Public Properties
		public Goals Goals
		{
			get
			{
				return this.goals;
			}
		}
		#endregion

		#region Public Methods
		#region Goals Handling
		public Int32 AddGoal()
		{
			Goal goal = new Goal();
			Int32 index = this.goals.Count;
			Int32 ret;

			if(index == 0)
			{
				goal.ID = 1;
			}
			else
			{
				goal.ID = ((Goal)this.goals[index - 1]).ID + 1;
			}

			ret = this.goals.Add(goal);

			return ret;
		}

		public void RemoveGoal(Goal goal)
		{
			foreach(Goal tmpGoal in this.goals)
			{
				if(tmpGoal.ID > goals.ID)
				{
					tmpGoal.ID -= 1;
				}
			}
			this.goals.Remove(goal);
		}

		public Goal FindGoalByUniqueID(String uniqueID)
		{
			Goal goal = null;

			foreach(Goal tmpGoal in this.goals)
			{
				if(tmpGoal.UniqueID == uniqueID)
				{
					goal = tmpGoal;
				}
			}

			return goal;
		}
		#endregion
		#endregion

		#region Static Members
		#endregion
	}
}
