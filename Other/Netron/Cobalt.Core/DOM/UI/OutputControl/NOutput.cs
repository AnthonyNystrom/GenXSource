using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Netron.Diagramming.Core;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
namespace Netron.Cobalt
{
	/// <summary>
	/// Simple scrollable textbox for line-by-line output
	/// </summary>
	public class NOutput : UserControl
    {
        #region Events
        /// <summary>
        /// Occurs when a new message is outputted
        /// </summary>
        public event EventHandler<StringEventArgs>  OnNewOutputMessage;
        /// <summary>
        /// Occurs when a channel is removed
        /// </summary>
		public event EventHandler<StringEventArgs> OnChannelRemoved;
        /// <summary>
        /// Occurs when a channel is added
        /// </summary>
		public event EventHandler<StringEventArgs> OnChannelAdded;
        /// <summary>
        /// Occurs when the control is shown
        /// </summary>
		public event EventHandler OnShow;

        #endregion

        #region Fields

		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem mnuClearAll;
		private System.Windows.Forms.Panel lowerPanel;
		private System.Windows.Forms.Label label;
		

		private OutputChannel currentChannel;
		private string current;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel upperPanel;
		private System.Windows.Forms.TextBox OutputText;
		private System.Windows.Forms.ComboBox comboBox;
        private OutputChannelCollection channels = null;
		#endregion

		#region Properties

		public bool ShowBottomPanel
		{
			get{return lowerPanel.Visible ;}
			set{lowerPanel.Visible = value;}
		}

		public string Label
		{
			get{return label.Text;}
			set{label.Text  = value;}
		}

		public Image Image
		{
			get{return this.pictureBox.Image;}
			set{this.pictureBox.Image = value;}
		}


        public CollectionBase<OutputChannel> Channels
		{
			get{return channels;}			
		}


		/// <summary>
		/// Gets or sets the current channel
		/// </summary>
		public string Current
		{
			get{return current;}
			set{this.SelectChannel(value);}
		}

		public OutputChannel CurrentChannel
		{
			get
			{
                return channels[current];

			}
		}

		

		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public NOutput()
		{			
			InitializeComponent();
			//add the default channel
            channels = new OutputChannelCollection();
			AddChannel("Default");
		}
		#endregion

		#region Methods

		/// <summary>
		/// Adds a new channel and sets this new one to the current
		/// </summary>
		/// <param name="channelName">the name of the new channel</param>
		public void AddChannel(string channelName)
		{
			if(channels[channelName]==null)
			{
				OutputChannel channel = new OutputChannel(channelName);
				channels.Add(channel);
				comboBox.Items.Add(channelName);
				currentChannel = channels[channelName];
				current = channelName;
				comboBox.SelectedIndex = comboBox.Items.Count-1; //the last added
                channel.OnNewOutput += new EventHandler<ChannelEventArgs>(channel_OnNewOutput);
                channel.OnClear += new EventHandler<StringEventArgs>(channel_OnClear);
				if(OnChannelAdded!=null)
					OnChannelAdded(this, new StringEventArgs(channelName));
			}

		}

        /// <summary>
        /// Handles the OnClear event of the channel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:Netron.Diagramming.Core.StringEventArgs"/> instance containing the event data.</param>
        void channel_OnClear(object sender, StringEventArgs e)
        {
            //only clear if the current is the channel that sent the event
            if (e.Data == current)
                this.OutputText.Text = "";
        }

        /// <summary>
        /// Handles the OnNewOutput event of the channel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:Netron.Cobalt.ChannelEventArgs"/> instance containing the event data.</param>
        void channel_OnNewOutput(object sender, ChannelEventArgs e)
        {
            if (current == e.ChannelName)
            {
                this.OutputText.Text += Environment.NewLine + e.Message;
                this.OutputText.SelectionStart = this.OutputText.Text.Length;
                this.OutputText.ScrollToCaret();
                if (this.OnNewOutputMessage != null)
                    OnNewOutputMessage(this, new StringEventArgs(e.Message));
            }
        }

		public void RemoveChannel(string channelName)
		{
			if(channelName!="Default")
			{
				comboBox.Items.Remove(channelName);
				channels.Remove(channelName);
				if(OnChannelRemoved!=null)
					OnChannelRemoved(this, new StringEventArgs(channelName));
				if(current==channelName)
					SelectChannel("Default");
			}
		}
		public void SelectChannel(string name)
		{
			if(channels[name]!=null)
			{
				currentChannel = channels[name];
				current = name;
				comboBox.SelectedItem = name;
			}

		}

		public void RaiseShow()
		{
			if(OnShow!=null)
				OnShow(this,EventArgs.Empty);
		}

		
		/// <summary>
		/// Windows designer initialization
		/// </summary>
		private void InitializeComponent()
		{
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.mnuClearAll = new System.Windows.Forms.MenuItem();
			this.lowerPanel = new System.Windows.Forms.Panel();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.label = new System.Windows.Forms.Label();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.upperPanel = new System.Windows.Forms.Panel();
			this.OutputText = new System.Windows.Forms.TextBox();
			this.comboBox = new System.Windows.Forms.ComboBox();
			this.lowerPanel.SuspendLayout();
			this.upperPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.mnuClearAll});
			// 
			// mnuClearAll
			// 
			this.mnuClearAll.Index = 0;
			this.mnuClearAll.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
			this.mnuClearAll.Text = "Clear All";
			this.mnuClearAll.Click += new System.EventHandler(this.mnuClearAll_Click);
			// 
			// lowerPanel
			// 
			this.lowerPanel.Controls.Add(this.pictureBox);
			this.lowerPanel.Controls.Add(this.label);
			this.lowerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lowerPanel.Location = new System.Drawing.Point(0, 424);
			this.lowerPanel.Name = "lowerPanel";
			this.lowerPanel.Size = new System.Drawing.Size(256, 64);
			this.lowerPanel.TabIndex = 2;
			// 
			// pictureBox
			// 
			this.pictureBox.Location = new System.Drawing.Point(8, 8);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(56, 50);
			this.pictureBox.TabIndex = 1;
			this.pictureBox.TabStop = false;
			// 
			// label
			// 
			this.label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label.Location = new System.Drawing.Point(72, 8);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(176, 43);
			this.label.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 421);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(256, 3);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// upperPanel
			// 
			this.upperPanel.Controls.Add(this.OutputText);
			this.upperPanel.Controls.Add(this.comboBox);
			this.upperPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.upperPanel.Location = new System.Drawing.Point(0, 0);
			this.upperPanel.Name = "upperPanel";
			this.upperPanel.Size = new System.Drawing.Size(256, 421);
			this.upperPanel.TabIndex = 4;
			// 
			// OutputText
			// 
			this.OutputText.BackColor = System.Drawing.Color.WhiteSmoke;
			this.OutputText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.OutputText.ContextMenu = this.contextMenu1;
			this.OutputText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OutputText.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.OutputText.Location = new System.Drawing.Point(0, 21);
			this.OutputText.Multiline = true;
			this.OutputText.Name = "OutputText";
			this.OutputText.ReadOnly = true;
			this.OutputText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.OutputText.Size = new System.Drawing.Size(256, 400);
			this.OutputText.TabIndex = 6;
			this.OutputText.Text = "";
			// 
			// comboBox
			// 
			this.comboBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox.Location = new System.Drawing.Point(0, 0);
			this.comboBox.Name = "comboBox";
			this.comboBox.Size = new System.Drawing.Size(256, 21);
			this.comboBox.TabIndex = 3;
			this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
			// 
			// NOutput
			// 
			this.BackColor = System.Drawing.Color.LightSteelBlue;
			this.Controls.Add(this.upperPanel);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.lowerPanel);
			this.Name = "NOutput";
			this.Size = new System.Drawing.Size(256, 488);
			this.lowerPanel.ResumeLayout(false);
			this.upperPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		private void mnuClearAll_Click(object sender, System.EventArgs e)
		{
			channels[current].ClearAll();
		}

		private void comboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(this.comboBox.SelectedIndex>-1)
			{
				SelectChannel((string) comboBox.SelectedItem);	
				this.OutputText.Text = currentChannel.Content;
				this.OutputText.SelectionStart=this.OutputText.Text.Length;
				this.OutputText.ScrollToCaret();

			}
		}


		

		#endregion

	
		
		/// <summary>
		/// Clears the default channel. If you want to clear another channel, use the Channels property.
		/// </summary>
		public void ClearAll()
		{
			channels[0].ClearAll();
		}

		/// <summary>
		/// Writes a message to the default channel
		/// </summary>
		/// <param name="message"></param>
		public void WriteLine(string message)
		{
			channels[0].WriteLine(message);
		}
		

		/// <summary>
		/// Writes a messages to the given channes
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="message"></param>
		public void WriteLine(string channel, string message)
		{
			try
			{
				channels[channel].WriteLine(message);
			}
			catch
			{
				//assuming that the it's better to direct the wanted output
				//to the default channel than nowhere at all, we return the 'default' channel.
				//At least, the NOutput control is supposed to add the default channel before anything else...
				if(channels.Count>0)				
					channels[0].WriteLine(message);
			}
		}

	



		public void SetImage(OutputPicture picture)
		{
			switch(picture)
			{
				case OutputPicture.None:
					this.pictureBox.Image = null;
					break;
				case OutputPicture.Exclamation: case OutputPicture.Info: case OutputPicture.Question:
					this.pictureBox.Image = GetImage(picture);
					break;
			}
		}
		private Bitmap GetImage(OutputPicture picture)
		{
			Bitmap bmp=null;
			string name = string.Empty;
			try
			{
				switch(picture)
				{
					case OutputPicture.None:						
						return null;
					case OutputPicture.Exclamation:
						name = "Exclamation.gif";
						break;
					case OutputPicture.Info:
						name = "Info.gif";
						break;
					case OutputPicture.Question:
						name = "Question.gif";
						break;
				
				}
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Neon.UI.Output." + name);
					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}
			return bmp;

			
		}
	}

    /// <summary>
    /// Customized <see cref="CollectionBase"/>
    /// </summary>
    class OutputChannelCollection : CollectionBase<OutputChannel>
    {
        public OutputChannel this[string name]
        {
            get {
                return this.innerList.Find(
                    delegate(OutputChannel channel)
                   {
                       if (channel.Name == name)
                           return true;
                       return false;
                   });
            }            
        }
        public void Remove(string channelName)
        {
            for (int k = 0; k < this.innerList.Count; k++)
            {
                if ((this.innerList[k] as OutputChannel).Name == channelName)
                    this.innerList.RemoveAt(k);

            }
        }
    }

    public enum OutputPicture
    {
        Question,
        Info,
        Exclamation,
        None
    }
	
	
}
