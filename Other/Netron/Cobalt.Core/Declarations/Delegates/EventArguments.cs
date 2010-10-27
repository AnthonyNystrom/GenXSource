using System;
using System.Collections.Generic;
using System.Text;

namespace Netron.Cobalt
{
    #region ChannelEventArgs
    /// <summary>
    /// Channel event argument
    /// </summary>
    public sealed class ChannelEventArgs : EventArgs
    {


        /// <summary>
        /// the ChannelName field
        /// </summary>
        private string mChannelName;
        /// <summary>
        /// Gets or sets the ChannelName
        /// </summary>
        public string ChannelName
        {
            get { return mChannelName; }
            set { mChannelName = value; }
        }

        /// <summary>
        /// the Message field
        /// </summary>
        private string mMessage;
        /// <summary>
        /// Gets or sets the Message
        /// </summary>
        public string Message
        {
            get { return mMessage; }
            set { mMessage = value; }
        }

        /// <summary>
        /// The empty argument.
        /// </summary>
        public static readonly new ChannelEventArgs Empty = new ChannelEventArgs();

        #region Constructor
        ///<summary>
        ///Default constructor
        ///</summary>
        public ChannelEventArgs(string  channelName, string message)
        {
            this.mMessage = message;
            this.mChannelName = channelName;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ChannelEventArgs"/> class.
        /// </summary>
        public ChannelEventArgs()
        {
        }
        #endregion

    }
    #endregion
}
