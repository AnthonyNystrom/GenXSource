/* -----------------------------------------------
 * NuGenPinpointList.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using res = Genetibase.Shared.Controls.Properties.Resources;

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.PinpointInternals;
using Genetibase.Shared.Reflection;
using Genetibase.Shared.Windows;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("SelectedIndexChanged")]
	[DefaultProperty("Items")]
	[NuGenSRDescription("Description_PinpointList")]
	public class NuGenPinpointList : NuGenPinpointWindow
	{
		#region Properties.Data

		/*
		 * DataSource
		 */

		private object _dataSource;

		/// <summary>
		/// Gets or sets the data source for this <see cref="NuGenPinpointList"/>.
		/// </summary>
		/// <value>
		/// An object that implements the <see cref="IList"/> or <see cref="IListSource"/> interfaces,
		/// such as a <see cref="DataSet"/> or an <see cref="Array"/>. The default is <see langword="null"/>.
		/// </value>
		/// <exception cref="ArgumentException">
		/// <para>The assigned value does not implement the <see cref="IList"/> or <see cref="IListSource"/> interfaces.</para>
		/// </exception>
		[AttributeProvider(typeof(IListSource))]
		[Browsable(true)]
		[DefaultValue((string)null)]
		[NuGenSRCategory("Category_Data")]
		[NuGenSRDescription("Description_Pinpoint_DataSource")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public object DataSource
		{
			get
			{
				return _dataSource;
			}
			set
			{
				if (((value != null) && !(value is IList)) && !(value is IListSource))
				{
					throw new ArgumentException(res.Argument_DataSourceForComplexBinding);
				}

				if (_dataSource != value)
				{
					try
					{
						this.SetDataConnection(value, _displayMember, false);
					}
					catch
					{
						this.DisplayMember = "";
					}

					if (value == null)
					{
						this.DisplayMember = "";
						this.SelectedIndex = 0;
						this.Items.Clear();
					}
				}
			}
		}

		private static readonly object _dataSourceChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="DataSource"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Pinpoint_DataSourceChanged")]
		public event EventHandler DataSourceChanged
		{
			add
			{
				this.Events.AddHandler(_dataSourceChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dataSourceChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenPinpointList.DataSourceChanged"/> event.
		/// </summary>
		protected virtual void OnDataSourceChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_dataSourceChanged, e);
			this.RefreshItems();
		}

		/*
		 * DisplayMember
		 */

		private BindingMemberInfo _displayMember;

		/// <summary>
		/// Gets or sets the property to display for this <see cref="NuGenPinpointList"/>.
		/// </summary>
		[Browsable(true)]
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[NuGenSRCategory("Category_Data")]
		[NuGenSRDescription("Description_Pinpoint_DisplayMember")]
		[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string DisplayMember
		{
			get
			{
				return _displayMember.BindingMember;
			}
			set
			{
				BindingMemberInfo displayMember = _displayMember;

				try
				{
					this.SetDataConnection(_dataSource, new BindingMemberInfo(value), false);
				}
				catch
				{
					_displayMember = displayMember;
				}
			}
		}

		private static readonly object _displayMemberChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="DisplayMember"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Pinpoint_DisplayMemberChanged")]
		public event EventHandler DisplayMemberChanged
		{
			add
			{
				this.Events.AddHandler(_displayMemberChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_displayMemberChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenPinpointList.DisplayMemberChanged"/> event.
		/// </summary>
		protected virtual void OnDisplayMemberChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_displayMemberChanged, e);
			this.RefreshItems();
		}

		/*
		 * ValueMember
		 */

		private BindingMemberInfo _valueMember;

		/// <summary>
		/// Gets or sets the property to use as the actual value for the items in the <see cref="NuGenPinpointList"/>.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <para>The specified property cannot be found on the object specified by the <see cref="DataSource"/> property.</para>
		/// </exception>
		[Browsable(true)]
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[NuGenSRCategory("Category_Data")]
		[NuGenSRDescription("Description_Pinpoint_ValueMember")]
		public string ValueMember
		{
			get
			{
				return _valueMember.BindingMember;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}

				BindingMemberInfo newDisplayMember = new BindingMemberInfo(value);

				if (!newDisplayMember.Equals(_valueMember))
				{
					if (string.IsNullOrEmpty(this.DisplayMember))
					{
						this.SetDataConnection(this.DataSource, newDisplayMember, false);
					}
					if (((_dataManager != null) && (value != null)) && ((value.Length != 0) && !this.BindingMemberInfoInDataManager(newDisplayMember)))
					{
						throw new ArgumentException(res.Argument_WrongValueMember, "value");
					}

					_valueMember = newDisplayMember;
					this.OnValueMemberChanged(EventArgs.Empty);
					this.OnSelectedValueChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _valueMemberChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ValueMember"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Pinpoint_ValueMemberChanged")]
		public event EventHandler ValueMemberChanged
		{
			add
			{
				this.Events.AddHandler(_valueMemberChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_valueMemberChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenPinpointList.ValueMemberChanged"/> event.
		/// </summary>
		protected virtual void OnValueMemberChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_valueMemberChanged, e);
		}

		#endregion

		#region Properties.NonBrowsable

		/// <summary>
		/// Gets or sets the value of the member property specified by the <see cref="ValueMember"/> property.
		/// </summary>
		/// <value>
		/// An object containing the value of the member of the data source specified by the <see cref="ValueMember"/> property.
		/// </value>
		[Browsable(false)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedValue
		{
			get
			{
				if ((this.SelectedIndex != -1) && (_dataManager != null))
				{
					object item = _dataManager.List[this.SelectedIndex];
					return this.FilterItemOnProperty(item, _valueMember.BindingField);
				}

				return null;
			}
			set
			{
				if (_dataManager != null)
				{
					string bindingField = _valueMember.BindingField;

					if (string.IsNullOrEmpty(bindingField))
					{
						throw new InvalidOperationException(res.InvalidOperation_EmptyValueMember);
					}

					PropertyDescriptor property = _dataManager.GetItemProperties().Find(bindingField, true);
					NuGenInvoker invoker = new NuGenInvoker(_dataManager);
					int num = invoker.Methods["Find"].Invoke<int>(property, value, true);
					this.SelectedIndex = num;
				}
			}
		}

		private static readonly object _selectedValueChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SelectedValue"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Pinpoint_SelectedValueChanged")]
		public event EventHandler SelectedValueChanged
		{
			add
			{
				this.Events.AddHandler(_selectedValueChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectedValueChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Shared.Controls.NuGenPinpointList.SelectedValueChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnSelectedValueChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_selectedValueChanged, e);
		}

		#endregion

		#region Properties.Public.Overridden

		/// <summary>
		/// Indicates whether the size of the control is calculated automatically according to its content.
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// Returns the text representation of the specified item.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>
		/// If the <see cref="DisplayMember"/> property is not specified, the value returned by
		/// <see cref="GetItemText"/> is the value of the item's ToString method.
		/// Otherwise, the method returns the string value of the member specified in
		/// the <see cref="DisplayMember"/> property for the object specified in the item parameter.
		/// </returns>
		public override string GetItemText(object item)
		{
			if (item == null)
			{
				return string.Empty;
			}

			item = this.FilterItemOnProperty(item, _displayMember.BindingField);

			if (item == null)
			{
				return "";
			}

			return Convert.ToString(item, CultureInfo.CurrentCulture);
		}

		#endregion

		#region Methods.Protected

		/// <summary>
		/// Returns the current value of the <see cref="NuGenPinpointList"/> item, if it is a property of an object given the item and the property name.
		/// </summary>
		/// <param name="item">The object the <see cref="NuGenPinpointList"/> item is bound to.</param>
		/// <param name="field">The property name of the item the <see cref="NuGenPinpointList"/> is bound to.</param>
		/// <returns>The filtered object.</returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="field"/> is <see langword="null"/>.</para>
		/// </exception>
		protected object FilterItemOnProperty(object item, string field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}

			if ((item != null) && (field.Length > 0))
			{
				try
				{
					PropertyDescriptor descriptor;

					if (_dataManager != null)
					{
						descriptor = _dataManager.GetItemProperties().Find(field, true);
					}
					else
					{
						descriptor = TypeDescriptor.GetProperties(item).Find(field, true);
					}

					if (descriptor != null)
					{
						item = descriptor.GetValue(item);
					}
				}
				catch
				{
				}
			}

			return item;
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Shared.Controls.NuGenPinpointWindow.SelectedIndexChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			if (_dataSource != null && _dataManager != null)
			{
				_dataManager.Position = this.SelectedIndex;
			}

			base.OnSelectedIndexChanged(e);
		}

		#endregion

		#region Methods.Private

		private bool BindingMemberInfoInDataManager(BindingMemberInfo bindingMemberInfo)
		{
			if (_dataManager != null)
			{
				PropertyDescriptorCollection itemProperties = _dataManager.GetItemProperties();
				int count = itemProperties.Count;

				for (int i = 0; i < count; i++)
				{
					if (!typeof(IList).IsAssignableFrom(itemProperties[i].PropertyType) && itemProperties[i].Name.Equals(bindingMemberInfo.BindingField))
					{
						return true;
					}
				}

				for (int j = 0; j < count; j++)
				{
					if (!typeof(IList).IsAssignableFrom(itemProperties[j].PropertyType) && (string.Compare(itemProperties[j].Name, bindingMemberInfo.BindingField, true, CultureInfo.CurrentCulture) == 0))
					{
						return true;
					}
				}
			}
			return false;
		}

		private void RefreshItems()
		{
			this.Items.Clear();
			object[] destination = null;

			if (_dataManager != null && _dataManager.Count >= 0)
			{
				destination = new object[_dataManager.Count];

				for (int i = 0; i < destination.Length; i++)
				{
					destination[i] = _dataManager.List[i];
				}
			}

			if (destination != null)
			{
				this.Items.AddRange(destination);
			}

			if (_dataManager != null)
			{
				this.SelectedIndex = _dataManager.Position;
			}
			else
			{
				this.SelectedIndex = 0;
			}
		}

		private void SetDataConnection(
			object newDataSource
			, BindingMemberInfo newDisplayMember
			, bool force
			)
		{
			bool flag = _dataSource != newDataSource;
			bool flag2 = !_displayMember.Equals(newDisplayMember);

			if (!_inSetDataConnection)
			{
				try
				{
					if ((force || flag) || flag2)
					{
						_inSetDataConnection = true;
						IList list = (_dataManager != null) ? _dataManager.List : null;
						bool flag3 = _dataManager == null;
						this.UnwireDataSource();
						_dataSource = newDataSource;
						_displayMember = newDisplayMember;
						this.WireDataSource();

						if (_isDataSourceInitialized)
						{
							CurrencyManager manager = null;

							if (((newDataSource != null) && (this.BindingContext != null)) && (newDataSource != Convert.DBNull))
							{
								manager = (CurrencyManager)this.BindingContext[newDataSource, newDisplayMember.BindingPath];
							}

							if (_dataManager != manager)
							{
								if (_dataManager != null)
								{
									_dataManager.ItemChanged -= new ItemChangedEventHandler(this.DataManager_ItemChanged);
									_dataManager.PositionChanged -= new EventHandler(this.DataManager_PositionChanged);
								}

								_dataManager = manager;

								if (_dataManager != null)
								{
									_dataManager.ItemChanged += new ItemChangedEventHandler(this.DataManager_ItemChanged);
									_dataManager.PositionChanged += new EventHandler(this.DataManager_PositionChanged);
								}
							}

							if (((_dataManager != null) && (flag2 || flag)) && (((_displayMember.BindingMember != null) && (_displayMember.BindingMember.Length != 0)) && !this.BindingMemberInfoInDataManager(_displayMember)))
							{
								throw new ArgumentException(res.Argument_WrongDisplayMember, "newDisplayMember");
							}
							if (((_dataManager != null) && ((flag || flag2) || force)) && (flag2 || (force && ((list != _dataManager.List) || flag3))))
							{
								ConstructorInfo ci = typeof(ItemChangedEventArgs).GetConstructor(
									BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Instance
									, null
									, new Type[] { typeof(int) }
									, null
								);

								Debug.Assert(ci != null, "ci != null");

								this.DataManager_ItemChanged(
									_dataManager
									, (ItemChangedEventArgs)ci.Invoke(new object[] { -1 })
								);
							}
						}
					}

					if (flag)
					{
						this.OnDataSourceChanged(EventArgs.Empty);
					}

					if (flag2)
					{
						this.OnDisplayMemberChanged(EventArgs.Empty);
					}
				}
				finally
				{
					_inSetDataConnection = false;
				}
			}
		}

		private void UnwireDataSource()
		{
			if (_dataSource is IComponent)
			{
				((IComponent)_dataSource).Disposed -= new EventHandler(this.DataSourceDisposed);
			}

			ISupportInitializeNotification notification = _dataSource as ISupportInitializeNotification;

			if ((notification != null) && _isDataSourceInitEventHooked)
			{
				notification.Initialized -= new EventHandler(this.DataSourceInitialized);
				_isDataSourceInitEventHooked = false;
			}
		}

		private void WireDataSource()
		{
			if (_dataSource is IComponent)
			{
				((IComponent)_dataSource).Disposed += new EventHandler(this.DataSourceDisposed);
			}

			ISupportInitializeNotification notification = _dataSource as ISupportInitializeNotification;

			if ((notification != null) && !notification.IsInitialized)
			{
				notification.Initialized += new EventHandler(this.DataSourceInitialized);
				_isDataSourceInitEventHooked = true;
				_isDataSourceInitialized = false;
			}
			else
			{
				_isDataSourceInitialized = true;
			}
		}

		#endregion

		#region EventHandlers

		private void DataManager_ItemChanged(object sender, ItemChangedEventArgs e)
		{
			if (_dataManager != null)
			{
				if (e.Index == -1)
				{
					this.Items.Clear();
					this.Items.AddRange(_dataManager.List);
					this.SelectedIndex = _dataManager.Position;
				}
				else
				{
					this.Items[e.Index] = _dataManager.List[e.Index];
				}

				this.OnSelectedValueChanged(EventArgs.Empty);
			}
		}

		private void DataManager_PositionChanged(object sender, EventArgs e)
		{
			if (_dataManager != null)
			{
				this.SelectedIndex = _dataManager.Position;
			}
		}

		private void DataSourceInitialized(object sender, EventArgs e)
		{
			this.SetDataConnection(_dataSource, _displayMember, true);
		}

		private void DataSourceDisposed(object sender, EventArgs e)
		{
			this.SetDataConnection(null, new BindingMemberInfo(""), true);
		}

		#endregion

		private CurrencyManager _dataManager;
		private bool _isDataSourceInitialized;
		private bool _isDataSourceInitEventHooked;
		private bool _inSetDataConnection;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPinpointList"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenPinpointLayoutManager"/></para>
		/// 	<para><see cref="INuGenPinpointRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenPinpointList(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.AutoSize = false;
		}
	}
}
