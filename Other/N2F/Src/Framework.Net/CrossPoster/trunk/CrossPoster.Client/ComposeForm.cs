/* ------------------------------------------------
 * ComposeForm.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Next2Friends.CrossPoster.Client
{
    sealed partial class ComposeForm : Form
    {
        /// <summary>
        /// Creates a new instance of the <code>ComposeForm</code> class.
        /// </summary>
        public ComposeForm()
        {
            InitializeComponent();
        }

        public String Message
        {
            get { return _messageTextBox.Text; }
            set { _messageTextBox.Text = value; }
        }

        public String Title
        {
            get { return _titleTextBox.Text; }
            set { _titleTextBox.Text = value; }
        }

        private void _inputTextBox_TextChanged(object sender, EventArgs e)
        {
            _publishButton.Enabled = !String.IsNullOrEmpty(_titleTextBox.Text) && !String.IsNullOrEmpty(_messageTextBox.Text);
        }
    }
}
