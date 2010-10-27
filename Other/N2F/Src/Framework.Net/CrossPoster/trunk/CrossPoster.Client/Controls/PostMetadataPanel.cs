/* ------------------------------------------------
 * PostMetadataPanel.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;

namespace Next2Friends.CrossPoster.Client.Controls
{
    sealed partial class PostMetadataPanel : UserControl
    {
        /// <summary>
        /// Creates a new instance of the <code>PostMetadataPanel</code> class.
        /// </summary>
        public PostMetadataPanel()
        {
            InitializeComponent();
        }

        private String _date;

        public String Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    _dateLabel.Text = _date;
                }
            }
        }

        private String _sender;

        public String Sender
        {
            get
            {
                return _sender;
            }
            set
            {
                if (_sender != value)
                {
                    _sender = value;
                    _senderLabel.Text = _sender;
                }
            }
        }

        private String _subject;

        public String Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                if (_subject != value)
                {
                    _subject = value;
                    _subjectLabel.Text = _subject;
                }
            }
        }
    }
}
