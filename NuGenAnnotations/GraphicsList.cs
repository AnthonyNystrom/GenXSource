using System;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Permissions;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;


namespace Genetibase.NuGenAnnotation
{
	/// <summary>
	/// List of graphic objects
	/// </summary>
	[Serializable]
	public class GraphicsList		//	 : ISerializable
	{
		private ArrayList graphicsList;

		private bool _isDirty;

		public bool Dirty
		{
			get
			{
				if (_isDirty == false)
				{
					foreach (DrawObject o in graphicsList)
					{
						if (o.Dirty == true)
						{
							_isDirty = true;
							break;
						}
					}
				}
				return _isDirty;
			}
			set
			{
				foreach (DrawObject o in graphicsList)
					o.Dirty = false;
				_isDirty = false;
			}
		}
	
		/// <summary>
		/// Returns IEnumerable object which may be used for enumeration
		/// of selected objects.
		/// 
		/// Note: returning IEnumerable<DrawObject> breaks CLS-compliance
		/// (assembly CLSCompliant = true is removed from AssemblyInfo.cs).
		/// To make this program CLS-compliant, replace 
		/// IEnumerable<DrawObject> with IEnumerable. This requires
		/// casting to object at runtime.
		/// </summary>
		/// <value></value>
		public IEnumerable<DrawObject> Selection
		{
			get
			{
				foreach (DrawObject o in graphicsList)
				{
					if (o.Selected)
					{
						yield return o;
					}
				}
			}
		}

		private const string entryCount = "ObjectCount";
		private const string entryType = "ObjectType";

		public GraphicsList()
		{
			graphicsList = new ArrayList();
		}

		public void LoadFromStream(SerializationInfo info, int orderNumber)
		{
			graphicsList = new ArrayList();

			int n = info.GetInt32(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryCount, orderNumber));

			string typeName;
			object drawObject;

			for (int i = 0; i < n; i++)
			{
				typeName = info.GetString(
					String.Format(CultureInfo.InvariantCulture,
						"{0}{1}",
					entryType, i));

				drawObject = Assembly.GetExecutingAssembly().CreateInstance(
					typeName);

				((DrawObject)drawObject).LoadFromStream(info, orderNumber, i);

				graphicsList.Add(drawObject);
			}
		}
		/// <summary>
		/// Save GraphicsList to the stream
		/// </summary>
		/// <param name="info"></param>
		/// <param name="orderNumber"></param>
		public void SaveToStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
		{
			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryCount, orderNumber),
				graphicsList.Count);

			int i = 0;
			foreach (DrawObject o in graphicsList)
			{
				info.AddValue(
					String.Format(CultureInfo.InvariantCulture,
						"{0}{1}",
						entryType, i),
					o.GetType().FullName);

				o.SaveToStream(info, orderNumber, i);
				i++;
			}
		}

		public void Draw(Graphics g)
		{
			int n = graphicsList.Count;
			DrawObject o;

			// Enumerate list in reverse order
			// to get first object on the top
			graphicsList.Sort();
			for (int i = n - 1; i >= 0; i--)
			{
				o = (DrawObject)graphicsList[i];
				// Only draw objects that are visible
				if(o.IntersectsWith(Rectangle.Round(g.ClipBounds)))
					o.Draw(g);

				if (o.Selected == true)
					o.DrawTracker(g);
			}
		}

		/// <summary>
		/// Clear all objects in the list
		/// </summary>
		/// <returns>
		/// true if at least one object is deleted
		/// </returns>
		public bool Clear()
		{
			bool result = (graphicsList.Count > 0);
			graphicsList.Clear();
			// Set dirty flag based on result. Result is true only if at least one item was cleared and since the list is empty, there can be nothing dirty.
			if (result == true)
				_isDirty = false;
			return result;
		}

		/// <summary>
		/// Count and this [nIndex] allow to read all graphics objects
		/// from GraphicsList in the loop.
		/// </summary>
		public int Count
		{
			get
			{
				return graphicsList.Count;
			}
		}

		public DrawObject this[int index]
		{
			get
			{
				if (index < 0 || index >= graphicsList.Count)
					return null;

				return (DrawObject)graphicsList[index];
			}
		}

		/// <summary>
		/// SelectedCount and GetSelectedObject allow to read
		/// selected objects in the loop
		/// </summary>
		public int SelectionCount
		{
			get
			{
				int n = 0;

				foreach (DrawObject o in graphicsList)
				{
					if (o.Selected)
						n++;
				}

				return n;
			}
		}

		public DrawObject GetSelectedObject(int index)
		{
			int n = -1;

			foreach (DrawObject o in graphicsList)
			{
				if (o.Selected)
				{
					n++;

					if (n == index)
						return o;
				}
			}

			return null;
		}

		public void Add(DrawObject obj)
		{
			graphicsList.Sort();
			foreach(DrawObject o in graphicsList)
				o.ZOrder++;

			graphicsList.Insert(0, obj);
		}

		public void SelectInRectangle(Rectangle rectangle)
		{
			UnselectAll();

			foreach (DrawObject o in graphicsList)
			{
				if (o.IntersectsWith(rectangle))
					o.Selected = true;
			}

		}

		public void UnselectAll()
		{
			foreach (DrawObject o in graphicsList)
			{
				o.Selected = false;
			}
		}

		public void SelectAll()
		{
			foreach (DrawObject o in graphicsList)
			{
				o.Selected = true;
			}
		}

		/// <summary>
		/// Delete selected items
		/// </summary>
		/// <returns>
		/// true if at least one object is deleted
		/// </returns>
		public bool DeleteSelection()
		{
			bool result = false;

			int n = graphicsList.Count;

			for (int i = n - 1; i >= 0; i--)
			{
				if (((DrawObject)graphicsList[i]).Selected)
				{
					graphicsList.RemoveAt(i);
					result = true;
				}
			}
			if (result == true)
				_isDirty = true;
			return result;
		}

		/// <summary>
		/// Delete last added object from the list
		/// (used for Undo operation).
		/// </summary>
		public void DeleteLastAddedObject()
		{
			if (graphicsList.Count > 0)
			{
				graphicsList.RemoveAt(0);
			}
		}
		/// <summary>
		/// Replace object in specified place.
		/// Used for Undo.
		/// </summary>
		public void Replace(int index, DrawObject obj)
		{
			if (index >= 0 && index < graphicsList.Count)
			{
				graphicsList.RemoveAt(index);
				graphicsList.Insert(index, obj);
			}
		}
		/// <summary>
		/// Remove object by index.
		/// Used for Undo.
		/// </summary>
		public void RemoveAt(int index)
		{
			graphicsList.RemoveAt(index);
		}

		/// <summary>
		/// Move selected items to front (beginning of the list)
		/// </summary>
		/// <returns>
		/// true if at least one object is moved
		/// </returns>
		public bool MoveSelectionToFront()
		{
			int n;
			int i;
			ArrayList tempList;

			tempList = new ArrayList();
			n = graphicsList.Count;

			// Read source list in reverse order, add every selected item
			// to temporary list and remove it from source list
			for (i = n - 1; i >= 0; i--)
			{
				if (((DrawObject)graphicsList[i]).Selected)
				{
					tempList.Add(graphicsList[i]);
					graphicsList.RemoveAt(i);
				}
			}

			// Read temporary list in direct order and insert every item
			// to the beginning of the source list
			n = tempList.Count;

			for (i = 0; i < n; i++)
			{
				graphicsList.Insert(0, tempList[i]);
			}
			if (n > 0)
				_isDirty = true;
			return (n > 0);
		}

		/// <summary>
		/// Move selected items to back (end of the list)
		/// </summary>
		/// <returns>
		/// true if at least one object is moved
		/// </returns>
		public bool MoveSelectionToBack()
		{
			int n;
			int i;
			ArrayList tempList;

			tempList = new ArrayList();
			n = graphicsList.Count;

			// Read source list in reverse order, add every selected item
			// to temporary list and remove it from source list
			for (i = n - 1; i >= 0; i--)
			{
				if (((DrawObject)graphicsList[i]).Selected)
				{
					tempList.Add(graphicsList[i]);
					graphicsList.RemoveAt(i);
				}
			}

			// Read temporary list in reverse order and add every item
			// to the end of the source list
			n = tempList.Count;

			for (i = n - 1; i >= 0; i--)
			{
				graphicsList.Add(tempList[i]);
			}
			if (n > 0)
				_isDirty = true;
			return (n > 0);
		}

		/// <summary>
		/// Get properties from selected objects and fill GraphicsProperties instance
		/// </summary>
		/// <returns></returns>
		private GraphicsProperties GetProperties()
		{
			GraphicsProperties properties = new GraphicsProperties();

			//int n = SelectionCount;

			//if (n < 1)
			//    return properties;

			//DrawObject o = GetSelectedObject(0);

			//int firstColor = o.Color.ToArgb();
			//int firstPenWidth = o.PenWidth;

			//bool allColorsAreEqual = true;
			//bool allWidthAreEqual = true;

			//for (int i = 1; i < n; i++)
			//{
			//    if (GetSelectedObject(i).Color.ToArgb() != firstColor)
			//        allColorsAreEqual = false;

			//    if (GetSelectedObject(i).PenWidth != firstPenWidth)
			//        allWidthAreEqual = false;
			//}

			//if (allColorsAreEqual)
			//{
			//    properties.ColorDefined = true;
			//    properties.Color = Color.FromArgb(firstColor);
			//}

			//if (allWidthAreEqual)
			//{
			//    properties.PenWidthDefined = true;
			//    properties.PenWidth = firstPenWidth;
			//}

			return properties;
		}

		/// <summary>
		/// Apply properties for all selected objects
		/// </summary>
		private void ApplyProperties(GraphicsProperties properties)
		{
			//foreach (DrawObject o in graphicsList)
			//{
			//    if (o.Selected)
			//    {
			//        if (properties.ColorDefined)
			//        {
			//            o.Color = properties.Color;
			//            DrawObject.LastUsedColor = properties.Color;
			//        }

			//        if (properties.PenWidthDefined)
			//        {
			//            o.PenWidth = properties.PenWidth;
			//            DrawObject.LastUsedPenWidth = properties.PenWidth;
			//        }
			//    }
			//}
		}

		/// <summary>
		/// Show Properties dialog. Return true if list is changed
		/// </summary>
		/// <param name="parent"></param>
		/// <returns></returns>
		public bool ShowPropertiesDialog(IWin32Window parent)
		{
			if (SelectionCount < 1)
				return false;

			GraphicsProperties properties = GetProperties();
			PropertiesDialog dlg = new PropertiesDialog();
			dlg.Properties = properties;

			if (dlg.ShowDialog(parent) != DialogResult.OK)
				return false;

			ApplyProperties(properties);

			return true;
		}
	}
}
