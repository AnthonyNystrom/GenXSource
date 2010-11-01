using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Reflection;

namespace UseCaseMakerControls
{
	/// <summary>
	/// Descrizione di riepilogo per UserControl1.
	/// </summary>
	[ Bindable(true) ]
	public class IndexedList : System.Windows.Forms.Panel
	{
		#region Private Constants And Enumerators
		private enum ColumnResizeStatus
		{
			None = 0,
			Resizing = 1,
			Released = 2
		}
		#endregion

		#region Class Members
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private IndexedListItemCollection items = null;
		private int rowHeight = 23;
		private int indexColumnWidth = 100;
		private int textColumnWidth = 100;
		private Color indexBackColor;
		private Color textBackColor;
		private Color readOnlyBackColor;
		private ColumnResizeStatus ColumnResizing = ColumnResizeStatus.None;
		private SepararatorCollection separators = new SepararatorCollection();
		private HighLightDescriptorCollection highLightDescriptors = new HighLightDescriptorCollection();
		private bool vscrollBarVisible = false;
		private int selectedIndex = -1;
		private object dataSource = null;
		private string indexDataField;
		private string textDataField;
		private string uniqueIDDataField;

		public event MouseOverTokenEventHandler				MouseOverToken;
		public event ItemTextChangedEventHandler			ItemTextChanged;
		public event SelectedChangeEventHandler				SelectedChanged;
		public event ItemClickEventHandler					ItemClick;
		public event ItemTextEnterEventHandler				ItemTextEnter;
		public event ItemTextSelectionChangedEventHandler	ItemTextSelectionChanged;
		#endregion

		#region Constructors
		public IndexedList()
		{
			// Chiamata richiesta da Progettazione form Windows.Forms.
			InitializeComponent();

			// TODO: aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitComponent.
			items = new IndexedListItemCollection(this);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ContainerControl,true);
			this.BackColor = System.Drawing.SystemColors.Window;
			this.AutoScroll = true;
			this.AutoScrollMinSize = new Size(5,5);
			this.AutoScrollMargin = new Size(0,0);

			this.indexBackColor = System.Drawing.SystemColors.Control;
			this.textBackColor = System.Drawing.SystemColors.Window;
			this.readOnlyBackColor = System.Drawing.SystemColors.ControlLightLight;
		}
		#endregion

		#region Public Properties
		[ 
		Category("Behavior"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true)
		]		
		public IndexedListItemCollection Items
		{
			get
			{
				return this.items;
			}
		}

		[ 
		Category("Layout"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true)
		]		
		public int RowHeight
		{
			get
			{
				return this.rowHeight;
			}
			set
			{
				this.rowHeight = value;
				// UpdateControlsLayout();
			}
		}

		[ 
		Category("Layout"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true)
		]		
		public int IndexColumnWidth
		{
			get
			{
				return this.indexColumnWidth;
			}
			set
			{
				this.indexColumnWidth = value;
				// UpdateControlsLayout();
			}
		}

		[ 
		Category("Layout"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true)
		]		
		public int TextColumnWidth
		{
			get
			{
				return this.textColumnWidth;
			}
			set
			{
				this.textColumnWidth = value;
				// UpdateControlsLayout();
			}
		}

		[ 
		Category("Appearance"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true)
		]		
		public Color IndexBackColor
		{
			get
			{
				return this.indexBackColor;
			}
			set
			{
				this.indexBackColor = value;
				UpdateControlsAppearance();
			}
		}

		[ 
		Category("Appearance"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true)
		]		
		public Color TextBackColor
		{
			get
			{
				return this.textBackColor;
			}
			set
			{
				this.textBackColor = value;
				UpdateControlsAppearance();
			}
		}

		[ 
		Category("Appearance"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true)
		]		
		public Color ReadOnlyBackColor
		{
			get
			{
				return this.readOnlyBackColor;
			}
			set
			{
				this.readOnlyBackColor = value;
				UpdateControlsAppearance();
			}
		}

		[ 
		Category("Data"),
		RefreshProperties(RefreshProperties.Repaint),
		TypeConverter("System.Windows.Forms.Design.DataSourceConverter,System.Design")
		]
		public object DataSource
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				this.dataSource = value;
				this.DataBind();
			}
		}

		[ 
		Category("Data"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true),
		Bindable(true)
		]
		public string IndexDataField
		{
			get
			{
				return this.indexDataField;
			}
			set
			{
				this.indexDataField = value;
			}
		}

		[ 
		Category("Data"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true),
		Bindable(true)
		]
		public string TextDataField
		{
			get
			{
				return this.textDataField;
			}
			set
			{
				this.textDataField = value;
			}
		}

		[ 
		Category("Data"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true),
		Bindable(true)
		]
		public string UniqueIDDataField
		{
			get
			{
				return this.uniqueIDDataField;
			}
			set
			{
				this.uniqueIDDataField = value;
			}
		}

		[ Browsable(false) ]
		public SepararatorCollection Separators
		{
			get
			{
				return this.separators;
			}
			set
			{
				this.separators = value;
			}
		}

		[ Browsable(false) ]
		public HighLightDescriptorCollection HighlightDescriptors
		{
			get
			{
				return this.highLightDescriptors;
			}
			set
			{
				this.highLightDescriptors = value;
			}
		}

		[ Browsable(false) ]
		public int SelectedIndex
		{
			get
			{
				return this.selectedIndex;
			}
			set
			{
				if(value > items.Count - 1 || value < -1)
				{
					throw new IndexOutOfRangeException();
				}
				this.selectedIndex = value;
				if(value != -1)
				{
					this.items[value].Selected = true;
				}
				// this.UpdateControlsLayout();
			}
		}
		#endregion

		#region Internal Methods
		internal void UpdateControlsLayout()
		{
			IndexedListItem item;

			this.SuspendLayout();

			if(this.indexColumnWidth < 50)
			{
				this.indexColumnWidth = 50;
			}

			this.textColumnWidth = this.DisplayRectangle.Width - this.indexColumnWidth - 1;

			if(this.textColumnWidth < 50)
			{
				this.indexColumnWidth = this.DisplayRectangle.Width - 50;
			}

			this.selectedIndex = -1;
			for(int index = 0; index < this.items.Count; index++)
			{
				item = items[index];

				if(item.Selected == true)
				{
					this.selectedIndex = index;
				}

				item.ItemLabel.Left = 0 + this.AutoScrollPosition.X;
				item.ItemLabel.Top =
					this.DisplayRectangle.Top +
					((index == 0) ? (index * this.rowHeight) : (index * this.rowHeight) + (1 * index));
				item.ItemLabel.Width = this.indexColumnWidth;
				item.ItemLabel.Height = this.rowHeight;

				item.ItemRichTextBox.Left = this.indexColumnWidth + 3 + this.AutoScrollPosition.X;
				item.ItemRichTextBox.Top =
					this.DisplayRectangle.Top +
					((index == 0) ? (index * this.rowHeight) : (index * this.rowHeight) + (1 * index));
				item.ItemRichTextBox.Height = this.rowHeight;
				if(!this.VScroll)
				{
					this.vscrollBarVisible = false;
				}
				if((items.Count * (this.rowHeight + 1) > this.DisplayRectangle.Height) && !this.vscrollBarVisible)
				{
					this.vscrollBarVisible = true;
					item.ItemRichTextBox.Width = this.DisplayRectangle.Width - SystemInformation.VerticalScrollBarWidth - item.ItemLabel.Width - 4;
				}
				else
				{
					item.ItemRichTextBox.Width = this.DisplayRectangle.Width - item.ItemLabel.Width - 4;
				}
			}

			this.ResumeLayout();

			this.Invalidate();
			this.Refresh();
		}

		internal void UpdateControlsAppearance()
		{
			this.Refresh();
		}
		#endregion

		#region Events
		#endregion

		#region Private Methods
		private IEnumerable ResolveDataSource()
		{
			IListSource listSource;
			IList list;

			if(this.dataSource == null)
			{
				return null;
			}

			listSource = (this.dataSource as IListSource);
			if(listSource != null)
			{
				list = listSource.GetList();
				return list;
			}

			if((this.dataSource as IEnumerable) != null)
			{
				return (IEnumerable)this.dataSource;
			}

			return null;
		}
		#endregion

		#region Protected Methods
		/// <summary>
		/// Pulire le risorse in uso.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		public override void Refresh()
		{
			IndexedListItem item;

			Win32.SendMessage(this.Handle, Win32.WM_SETREDRAW, 0, (IntPtr)0);

			for(int index = 0; index < this.items.Count; index++)
			{
				item = items[index];

				if(!item.Selected)
				{
					item.ItemLabel.BackColor = this.indexBackColor;
				}
				if(item.ReadOnly)
				{
					item.ItemRichTextBox.BackColor = this.readOnlyBackColor;
				}
				else
				{
					item.ItemRichTextBox.BackColor = this.textBackColor;
				}
			}

			Win32.SendMessage(this.Handle, Win32.WM_SETREDRAW, 1, (IntPtr)0);
			this.Invalidate(this.DisplayRectangle,true);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.UpdateControlsLayout();
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			this.UpdateControlsLayout();
			base.OnLayout(levent);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rect;
			Pen pen;

			pen = new Pen(System.Drawing.SystemColors.ControlDark,1);
			foreach(IndexedListItem ili in this.items)
			{
				rect = ili.ItemLabel.Bounds;
				e.Graphics.DrawRectangle(pen,ili.ItemRichTextBox.Bounds);
				e.Graphics.DrawLine(
					pen,
					ili.ItemRichTextBox.Left - 1,
					ili.ItemRichTextBox.Top,
					ili.ItemRichTextBox.Left - 1,
					ili.ItemRichTextBox.Bottom);
				e.Graphics.DrawRectangle(pen,ili.ItemLabel.Bounds);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);

			Point mousePoint = new Point(e.X,e.Y);

			Rectangle rectSplitter = new Rectangle(
				this.indexColumnWidth + this.DisplayRectangle.Left - 5,
				this.ClientRectangle.Top,
				10,
				this.ClientRectangle.Height);

			if(rectSplitter.Contains(mousePoint) && this.items.Count > 0)
			{
				Cursor.Current = Cursors.VSplit;
				this.Capture = true;
				this.ColumnResizing = ColumnResizeStatus.Resizing;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);

			switch(this.ColumnResizing)
			{
				case ColumnResizeStatus.Resizing:
					Cursor.Current = Cursors.Default;
					this.Capture = false;
					this.ColumnResizing = ColumnResizeStatus.Released;
					break;
				default:
					break;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);

			Point mousePoint = new Point(e.X,e.Y);

			Rectangle rectSplitter = new Rectangle(
				this.indexColumnWidth + this.DisplayRectangle.Left - 5,
				this.ClientRectangle.Top,
				10,
				this.ClientRectangle.Height);

			if(rectSplitter.Contains(mousePoint) && this.items.Count > 0)
			{
				Cursor.Current = Cursors.VSplit;
			}

			switch(this.ColumnResizing)
			{
				case ColumnResizeStatus.Resizing:
					this.IndexColumnWidth = e.X;
					this.PerformLayout();
					break;
				case ColumnResizeStatus.Released:
					Cursor.Current = Cursors.Default;
					this.Capture = false;
					this.ColumnResizing = ColumnResizeStatus.None;
					break;
				case ColumnResizeStatus.None:
				default:
					break;
			}
		}
		#endregion

		#region Public Methods
		public void DataBind()
		{
			IEnumerable data = ResolveDataSource();
			if(data == null)
			{
				return;
			}

			if(this.IndexDataField == null
				|| this.TextDataField == null
				|| this.UniqueIDDataField == null)
			{
				return;
			}

			Win32.SendMessage(this.Handle,Win32.WM_SETREDRAW,0,(IntPtr)0);
			this.SuspendLayout();

			this.items.Clear();
			IEnumerator ie = data.GetEnumerator();
			while(ie.MoveNext())
			{
				IndexedListItem ili = new IndexedListItem();
				PropertyInfo indexDataField = ie.Current.GetType().GetProperty(this.IndexDataField);
				ili.Index = (string)indexDataField.GetValue(ie.Current,null);
				PropertyInfo textDataField = ie.Current.GetType().GetProperty(this.TextDataField);
				ili.Text = (string)textDataField.GetValue(ie.Current,null);
				PropertyInfo tagDataField = ie.Current.GetType().GetProperty(this.UniqueIDDataField);
				ili.Tag = (string)tagDataField.GetValue(ie.Current,null);
				this.items.Add(ili);
			}

			foreach(IndexedListItem ili in this.items)
			{
				ili.ItemRichTextBox.ParseNow();
			}

			this.ResumeLayout(true);
			Win32.SendMessage(this.Handle,Win32.WM_SETREDRAW,1,(IntPtr)0);

			this.Refresh();
		}

		public void SelectedChange(object sender)
		{
			int index = 0;
			foreach(IndexedListItem ili in this.items)
			{
				if(ili.ItemLabel.Name == ((Label)sender).Name)
				{
					ili.ItemLabel.BackColor = System.Drawing.SystemColors.Highlight;
					ili.ItemLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
					ili.ItemRichTextBox.Focus();
					this.selectedIndex = index;
					this.OnSelectedChanged(ili,new EventArgs());
					break;
				}
				index += 1;
			}

			this.Refresh();
		}

		public void OnItemLabelClick(object sender, EventArgs e) 
		{
			int index = 0;
			foreach(IndexedListItem ili in this.items)
			{
				if(ili.ItemLabel.Name == ((Label)sender).Name)
				{
					ili.Selected = true;
					ili.ItemRichTextBox.Focus();
					this.selectedIndex = index;
				}
				else
				{
					ili.Selected = false;
				}
				index += 1;
			}

			this.Refresh();
		}

		public void OnItemRichTextBoxClick(object sender, EventArgs e) 
		{
			foreach(IndexedListItem ili in this.items)
			{
				if(ili.ItemRichTextBox.Name == ((RichTextBox)sender).Name)
				{
					ili.Selected = true;
					ili.ItemLabel.BackColor = System.Drawing.SystemColors.Highlight;
					ili.ItemLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
				}
				else
				{
					ili.Selected = false;
					ili.ItemLabel.BackColor = this.indexBackColor;
					ili.ItemLabel.ForeColor = System.Drawing.SystemColors.WindowText;
				}
			}

			this.Refresh();
		}

		public void OnItemTextChanged(object sender, ItemTextChangedEventArgs e)
		{
			if(this.ItemTextChanged != null)
			{
				foreach(IndexedListItem ili in this.items)
				{
					if(ili.ItemRichTextBox.Name == ((RichTextBox)sender).Name)
					{
						this.ItemTextChanged(ili,e);
					}
				}
			}
		}

		public void OnMouseOverToken(object sender, MouseOverTokenEventArgs e)
		{
			if(this.MouseOverToken != null)
			{
				foreach(IndexedListItem ili in this.items)
				{
					if(ili.ItemRichTextBox.Name == ((RichTextBox)sender).Name)
					{
						this.MouseOverToken(ili,e);
					}
				}
			}
		}

		public void OnSelectedChanged(object sender, EventArgs e)
		{
			if(this.SelectedChanged != null)
			{
				this.SelectedChanged(sender,e);
			}
		}

		public void OnItemClick(object sender, MouseOverTokenEventArgs e)
		{
			if(this.ItemClick != null)
			{
				foreach(IndexedListItem ili in this.items)
				{
					if(ili.ItemRichTextBox.Name == ((RichTextBox)sender).Name)
					{
						this.ItemClick(ili,e);
					}
				}
			}
		}

		public void OnItemTextEnter(object sender, ItemTextChangedEventArgs e)
		{
			if(this.ItemTextEnter != null)
			{
				foreach(IndexedListItem ili in this.items)
				{
					if(ili.ItemRichTextBox.Name == ((RichTextBox)sender).Name)
					{
						this.ItemTextEnter(ili,e);
					}
				}
			}
		}

		public void OnItemTextSelectionChanged(object sender, ItemTextChangedEventArgs e)
		{
			foreach(IndexedListItem ili in this.items)
			{
				if(ili.ItemRichTextBox.Name == ((RichTextBox)sender).Name)
				{
					this.ItemTextSelectionChanged(ili,e);
				}
			}
		}
		#endregion

		#region Codice generato da Progettazione componenti
		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion
	}
}
