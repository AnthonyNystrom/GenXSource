/* ------------------------------------------------
 * BlogPropertiesForm.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Next2Friends.CrossPoster.Client.Controls;
using Next2Friends.CrossPoster.Client.Logic;

namespace Next2Friends.CrossPoster.Client
{
    sealed partial class BlogPropertiesForm : Form
    {
        public BlogPropertiesForm()
        {
            InitializeComponent();

            _blogList.Items.Add(new ImageComboItem("Blogger", 0) { Tag = BlogType.Blogger.ToString() });
        }

        public BlogDescriptor BlogDescriptor { get; private set; }

        public DialogResult ShowDialog(BlogDescriptor blogDescriptor)
        {
            if (blogDescriptor == null)
                throw new ArgumentNullException("blogDescriptor");
            BlogDescriptor = blogDescriptor;
            _blogAddressTextBox.Text = blogDescriptor.Address;
            _blogNameTextBox.Text = blogDescriptor.BlogName;
            _passwordTextBox.Text = blogDescriptor.Password;
            _usernameTextBox.Text = blogDescriptor.Username;
            switch (blogDescriptor.BlogType)
            {
                default:
                    _blogList.SelectedIndex = 0;
                    break;
            }
            return base.ShowDialog();
        }

        private void InvokeAlert(String message)
        {
            MessageBox.Show(message, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void _okButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_blogNameTextBox.Text))
            {
                InvokeAlert(VerificationMessages.InvalidBlogName);
                _blogNameTextBox.Focus();
                return;
            }
            else if (String.IsNullOrEmpty(_blogAddressTextBox.Text))
            {
                InvokeAlert(VerificationMessages.InvalidBlogAddress);
                _blogAddressTextBox.Focus();
                return;
            }
            else if (String.IsNullOrEmpty(_usernameTextBox.Text))
            {
                InvokeAlert(VerificationMessages.InvalidUsername);
                _usernameTextBox.Focus();
                return;
            }
            else if (String.IsNullOrEmpty(_passwordTextBox.Text))
            {
                InvokeAlert(VerificationMessages.InvalidPassword);
                _passwordTextBox.Focus();
                return;
            }

            BlogDescriptor.Address = _blogAddressTextBox.Text;
            BlogDescriptor.BlogName = _blogNameTextBox.Text;
            BlogDescriptor.BlogType = (BlogType)Enum.Parse(typeof(BlogType), Convert.ToString(_blogList.Items[_blogList.SelectedIndex]));
            BlogDescriptor.Password = _passwordTextBox.Text;
            BlogDescriptor.Username = _usernameTextBox.Text;
        }
    }
}
