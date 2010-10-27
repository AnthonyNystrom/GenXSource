using System;
using System.Text;
using Netron.Diagramming.Core;
namespace Netron.Cobalt
{
	/// <summary>
	/// The output channel is a buffer of string data
	/// </summary>
	public class OutputChannel
	{
		#region Events
		/// <summary>
		/// Notifies the outside that a new message has been posted in this 
		/// output channes
		/// </summary>
		public event EventHandler<ChannelEventArgs> OnNewOutput;
		/// <summary>
		/// Notifies the outside that the channel got cleared
		/// </summary>
		public event EventHandler<StringEventArgs> OnClear;
		#endregion

		#region Fields
		/// <summary>
		/// the name of the channel
		/// </summary>
		private string name = string.Empty;
		/// <summary>
		/// the actual string buffer
		/// </summary>
		private StringBuilder content;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the name of the output channel
		/// </summary>
		public string Name
		{
			get{return name;}			
		}
		/// <summary>
		/// Gets the content of the channel
		/// </summary>
		public string Content
		{
			get{return content.ToString();}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="name">the name of the channel</param>
		public OutputChannel(string name)
		{
			this.name = name;
			content = new StringBuilder();
		}

		public OutputChannel() : this("NewChannel")
		{			
		}
		#endregion

		#region Methods

		/// <summary>
		/// Write a message to the output
		/// </summary>
		/// <param name="message"></param>
		public void WriteLine(string message)
		{
			if(message==null) return;
			this.content.Append(Environment.NewLine);
			this.content.Append(message);
			RaiseOnNewMessage(message);
			
		
		}
		/// <summary>
		/// Write a line of message with a marked preamble
		/// </summary>
		/// <param name="message"></param>
		/// <param name="preamble"></param>
		public void WriteLine(string message, string preamble)
		{
			this.content.Append(Environment.NewLine);
			this.content.Append(preamble);
			this.content.Append("> ");
			this.content.Append(message);
			RaiseOnNewMessage(preamble + "> " + message);
			
		}

		/// <summary>
		/// Clears the content of this channel
		/// </summary>
		public void ClearAll()
		{
			this.content = new StringBuilder();
			RaiseOnClear();
		}
		/// <summary>
		/// Raises the OnClear event
		/// </summary>
		public void RaiseOnClear()
		{
			//tell the world
			if(OnClear!=null)
				OnClear(this, new StringEventArgs(this.name));
		}
		/// <summary>
		/// Raises the OnNewMessage event
		/// </summary>
		/// <param name="msg"></param>
		public void RaiseOnNewMessage(string msg)
		{
			if(this.OnNewOutput!=null)
				OnNewOutput(this, new ChannelEventArgs(this.Name ,msg));
		}

		#endregion
		

	}
}
