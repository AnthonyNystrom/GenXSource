/* ------------------------------------------------
 * PostContentPanel.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Next2Friends.CrossPoster.Client.Controls
{
    sealed partial class PostContentPanel : UserControl
    {
        public PostContentPanel()
        {
            InitializeComponent();
        }

        private String _content;

        public String Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (_content != value)
                {
                    _content = value;
                    _webBrowser.DocumentText = _content;
                }
            }
        }

        public String Date
        {
            get { return _postMetadataPanel.Date; }
            set { _postMetadataPanel.Date = value; }
        }

        public String Sender
        {
            get { return _postMetadataPanel.Sender; }
            set { _postMetadataPanel.Sender = value; }
        }

        public String Subject
        {
            get { return _postMetadataPanel.Subject; }
            set { _postMetadataPanel.Subject = value; }
        }
    }
}
