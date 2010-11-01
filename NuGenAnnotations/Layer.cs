using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace Genetibase.NuGenAnnotation
{
	//[Serializable]
	/// <summary>
	/// Each <see cref="Layer"/> contains a list of Graphic Objects <see cref="GraphicsList"/>.
	/// Each <see cref="Layer"/> is contained in the <see cref="Layers"/> collection.
	/// Properties:
	///	LayerName		User-defined name of the Layer
	///	Graphics			Collection of <see cref="DrawObject"/>s in the Layer
	///	IsVisible			True if the <see cref="Layer"/> is visible
	///	IsActive			True if the <see cref="Layer"/> is the active <see cref="Layer"/> (only one <see cref="Layer"/> may be active at a time)
	///	Dirty				True if any object in the <see cref="Layer"/> is dirty
	/// </summary>
	public class Layer
	{
		private string _name;
		private bool _isDirty;
		private bool _visible;
		private bool _active;
		private GraphicsList _graphicsList;

		/// <summary>
		/// <see cref="Layer"/> Name (User-defined)
		/// </summary>
		public string LayerName
		{
			get { return _name; }
			set { _name = value; }
		}
		/// <summary>
		/// List of Graphic objects (derived from <see cref="DrawObject"/>) contained by this <see cref="Layer"/>
		/// </summary>
		public GraphicsList Graphics
		{
			get { return _graphicsList; }
			set { _graphicsList = value; }
		}
		/// <summary>
		/// Returns True if this <see cref="Layer"/> is visible, else False
		/// </summary>
		public bool IsVisible
		{
			get { return _visible; }
			set { _visible = value; }
		}
		/// <summary>
		/// Returns True if this is the active <see cref="Layer"/>, else False
		/// </summary>
		public bool IsActive
		{
			get { return _active; }
			set { _active = value; }
		}
		/// <summary>
		/// Dirty is True if any elements in the contained <see cref="GraphicsList"/> are dirty, else False
		/// </summary>
		public bool Dirty
		{
			get
			{
				if (_isDirty == false)
					_isDirty = _graphicsList.Dirty;
				return _isDirty;
			}
			set
			{
				_graphicsList.Dirty = false;
				_isDirty = false;
			}
		}
		private const string entryLayerName = "LayerName";
		private const string entryLayerVisible = "LayerVisible";
		private const string entryLayerActive = "LayerActive";
		private const string entryObjectType = "ObjectType";
		private const string entryGraphicsCount = "GraphicsCount";

		public void SaveToStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
		{
			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryLayerName, orderNumber),
				_name);

			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryLayerVisible, orderNumber),
				_visible);

			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryLayerActive, orderNumber),
				_active);

			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryGraphicsCount, orderNumber),
				_graphicsList.Count);

			for (int i = 0; i < _graphicsList.Count; i++)
			{
				object o = _graphicsList[i];
				info.AddValue(
					String.Format(CultureInfo.InvariantCulture,
						"{0}{1}-{2}",
						entryObjectType, orderNumber, i),
					((DrawObject)o).GetType().FullName);

				((DrawObject)o).SaveToStream(info, orderNumber, i);
			}
		}

		public void LoadFromStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
		{
			_graphicsList = new GraphicsList();

			_name = info.GetString(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryLayerName, orderNumber));

			_visible = info.GetBoolean(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryLayerVisible, orderNumber));

			_active = info.GetBoolean(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryLayerActive, orderNumber));

			int n = info.GetInt32(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryGraphicsCount, orderNumber));

			string typeName;
			object drawObject;

			for (int i = 0; i < n; i++)
			{
				typeName = info.GetString(
					String.Format(CultureInfo.InvariantCulture,
						"{0}{1}-{2}",
					entryObjectType, orderNumber, i));

				drawObject = Assembly.GetExecutingAssembly().CreateInstance(typeName);

				((DrawObject)drawObject).LoadFromStream(info, orderNumber, i);

				_graphicsList.Add((DrawObject)drawObject);
			}
		}

		internal void Draw(Graphics g)
		{
			_graphicsList.Draw(g);
		}
	}
}
