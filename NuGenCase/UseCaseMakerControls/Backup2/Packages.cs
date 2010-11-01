using System;

namespace UseCaseMakerLibrary
{
	public class Packages : IdentificableObjectCollection
	{
		internal Packages(Package owner)
		{
			base.Owner = owner;
		}

		#region Public Properties
		#endregion

		public object FindElementByUniqueID(String uniqueID)
		{
			object element = null;

			if(this.UniqueID == uniqueID)
			{
				return this;
			}

			foreach(Package child in this)
			{
				if(child.UniqueID == uniqueID)
				{
					element = child;
					break;
				}
				if(child.Actors.UniqueID == uniqueID)
				{
					element = child.Actors;
					break;
				}
				element = child.Actors.FindByUniqueID(uniqueID);
				if(element != null)
				{
					break;
				}
				if(child.UseCases.UniqueID == uniqueID)
				{
					element = child.UseCases;
					break;
				}
				element = child.UseCases.FindByUniqueID(uniqueID);
				if(element != null)
				{
					break;
				}
				if(child.Requirements.UniqueID == uniqueID)
				{
					element = child.Requirements;
					break;
				}
				element = child.Requirements.FindByUniqueID(uniqueID);
				if(element != null)
				{
					break;
				}
				if(child.Packages.Count > 0)
				{
					element = child.Packages.FindElementByUniqueID(uniqueID);
					if(element != null)
					{
						break;
					}
				}
			}

			return element;
		}

		public object FindElementByName(String name)
		{
			object element = null;

			if(this.Name == name)
			{
				return this;
			}

			foreach(Package child in this)
			{
				if(child.Name == name)
				{
					element = child;
					break;
				}
				if(child.Actors.Name == name)
				{
					element = child.Actors;
					break;
				}
				element = child.Actors.FindByName(name);
				if(element != null)
				{
					break;
				}
				if(child.UseCases.Name == name)
				{
					element = child.UseCases;
					break;
				}
				element = child.UseCases.FindByName(name);
				if(element != null)
				{
					break;
				}
				if(child.Requirements.Name == name)
				{
					element = child.Requirements;
					break;
				}
				element = child.Requirements.FindByName(name);
				if(element != null)
				{
					break;
				}
				if(child.Packages.Count > 0)
				{
					element = child.Packages.FindElementByName(name);
					if(element != null)
					{
						break;
					}
				}
			}

			return element;
		}

		public object FindElementByPath(String path)
		{
			object element = null;

			if(this.Path == path)
			{
				return this;
			}

			foreach(Package child in this)
			{
				if(child.Path == path)
				{
					element = child;
					break;
				}
				if(child.Actors.Path == path)
				{
					element = child.Actors;
					break;
				}
				element = child.Actors.FindByPath(path);
				if(element != null)
				{
					break;
				}
				if(child.UseCases.Path == path)
				{
					element = child.UseCases;
					break;
				}
				element = child.UseCases.FindByPath(path);
				if(element != null)
				{
					break;
				}
				if(child.Requirements.Path == path)
				{
					element = child.Requirements;
					break;
				}
				element = child.Requirements.FindByPath(path);
				if(element != null)
				{
					return element;
				}
				if(child.Packages.Count > 0)
				{
					element = child.Packages.FindElementByPath(path);
					if(element != null)
					{
						break;
					}
				}
			}

			return element;
		}
	}
}
