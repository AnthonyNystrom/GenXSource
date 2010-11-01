using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace UseCaseMakerControls
{
	public delegate void MouseOverTokenEventHandler(object sender, MouseOverTokenEventArgs e);
	public delegate void SelectedChangeEventHandler(object sender, EventArgs e);
	public delegate void ItemTextChangedEventHandler(object sender, ItemTextChangedEventArgs e);
	public delegate void ItemClickEventHandler(object sender, MouseOverTokenEventArgs e);
	public delegate void ItemTextEnterEventHandler(object sender, ItemTextChangedEventArgs e);
	public delegate void ItemTextSelectionChangedEventHandler(object sender, ItemTextChangedEventArgs e);

	/// <summary>
	/// Descrizione di riepilogo per IndexedListItem.
	/// </summary>
	public class IndexedListItem
	{
		#region Private Enumerators and Constants
		#endregion

		#region Public Enumerators and Constants
		#endregion

		#region Class Members
		private Label								_index = new Label();
		private LinkEnabledRTB						_text = new LinkEnabledRTB();
		private bool								_selected = false;
		private object								_tag = null;
		private bool								_readOnly = false;

		public event MouseOverTokenEventHandler				MouseOverToken;
		public event SelectedChangeEventHandler				SelectedChange;
		public event ItemTextChangedEventHandler			ItemTextChanged;
		public event ItemClickEventHandler					ItemClick;
		public event ItemTextEnterEventHandler				ItemTextEnter;
		public event ItemTextSelectionChangedEventHandler	ItemTextSelectionChanged;
		#endregion

		#region Constructors
		public IndexedListItem()
		{
			_text.MouseOverToken += new MouseOverTokenEventHandler(_text_MouseOverToken);
			_text.ItemTextChanged += new ItemTextChangedEventHandler(_text_ItemTextChanged);
			_text.ItemClick += new ItemClickEventHandler(_text_ItemClick);
			_text.ItemTextEnter += new ItemTextEnterEventHandler(_text_ItemTextEnter);
			_text.ItemTextSelectionChanged += new ItemTextSelectionChangedEventHandler(_text_ItemTextSelectionChanged);
		}
		#endregion

		#region Events
		#endregion

		#region Public Properties
		[ 
		Category("Default"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true),
		Bindable(true)
		]
		public String Index
		{
			get
			{
				return this._index.Text;
			}
			set
			{
				this._index.Text = value;
			}
		}

		[ 
		Category("Default"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true),
		Bindable(true)
		]
		public String Text
		{
			get
			{
				return this._text.Text;
			}
			set
			{
				this._text.Text = value;
			}
		}

		[ 
		Category("Default"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true)
		]
		public object Tag
		{
			get
			{
				return this._tag;
			}
			set
			{
				this._tag = value;
			}
		}

		[ 
		Category("Default"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true)
		]
		public bool ReadOnly
		{
			get
			{
				return this._readOnly;
			}
			set
			{
				this._readOnly = value;
				this.ItemRichTextBox.ReadOnly = value;
			}
		}

		[ 
		Category("Default"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		NotifyParentProperty(true)
		]
		public Image IndexImage
		{
			get
			{
				return this.ItemLabel.Image;
			}
			set
			{
				this.ItemLabel.Image = value;
				this.ItemLabel.ImageAlign = ContentAlignment.BottomRight;
			}
		}

		[ 
		Browsable(false)
		]
		internal Label ItemLabel
		{
			get
			{
				return this._index;
			}
		}

		[ 
		Browsable(false)
		]
		internal LinkEnabledRTB ItemRichTextBox
		{
			get
			{
				return this._text;
			}
		}

		[ 
		Browsable(false)
		]
		public bool Selected
		{
			get
			{
				return this._selected;
			}
			set
			{
				this._selected = value;
				if(this._selected && SelectedChange != null)
				{
					SelectedChange(this.ItemLabel, new EventArgs());
				}
			}
		}
		#endregion

		#region Public Methods
		#endregion

		#region Protected Methods
		protected void _text_MouseOverToken(object sender, MouseOverTokenEventArgs e)
		{
			if(MouseOverToken != null)
			{
				MouseOverToken(sender,e);
			}
		}

		protected void _text_ItemTextChanged(object sender, ItemTextChangedEventArgs e)
		{
			if(ItemTextChanged != null)
			{
				ItemTextChanged(sender,e);
			}
		}

		protected void _text_ItemClick(object sender, MouseOverTokenEventArgs e)
		{
			if(ItemClick != null)
			{
				ItemClick(sender,e);
			}
		}

		protected void _text_ItemTextEnter(object sender, ItemTextChangedEventArgs e)
		{
			if(ItemTextEnter != null)
			{
				ItemTextEnter(sender,e);
			}
		}

		protected void _text_ItemTextSelectionChanged(object sender, ItemTextChangedEventArgs e)
		{
			if(ItemTextSelectionChanged != null)
			{
				ItemTextSelectionChanged(sender,e);
			}
		}
		#endregion

		#region Private Methods
		#endregion
	}
}
