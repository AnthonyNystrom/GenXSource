/* ------------------------------------------------
 * AddBlogStepTwo.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Next2Friends.CrossPoster.Client.Logic;

namespace Next2Friends.CrossPoster.Client
{
    sealed partial class AddBlogStepTwo : WizardForm, IWizardStep
    {
        public AddBlogStepTwo()
        {
            InitializeComponent();
        }

        protected override void PreviewShow(BlogDescriptor blogDescriptor)
        {
            var url = "http://www.blogger.com/feeds/default/blogs";

            switch (blogDescriptor.BlogType)
            {
                case BlogType.WordPress:
                    url = "http://nickname.wordpress.com/xmlrpc.php";
                    break;
                case BlogType.LiveJournal:
                    url = "http://www.livejournal.com/interface/xmlrpc";
                    break;
            }

            _addressTextBox.Text = url;
        }

        private void InvokeAlert(String message)
        {
            MessageBox.Show(message, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void _backButton_Click(object sender, EventArgs e)
        {
            InvokeBack();
        }

        private void _finishButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_addressTextBox.Text))
            {
                InvokeAlert(VerificationMessages.InvalidBlogAddress);
                _addressTextBox.Focus();
                return;
            }

            if (String.IsNullOrEmpty(_usernameTextBox.Text))
            {
                InvokeAlert(VerificationMessages.InvalidUsername);
                _usernameTextBox.Focus();
                return;
            }

            if (String.IsNullOrEmpty(_passwordTextBox.Text))
            {
                InvokeAlert(VerificationMessages.InvalidPassword);
                _passwordTextBox.Focus();
                return;
            }

            if (String.IsNullOrEmpty(_blogNameTextBox.Text))
            {
                InvokeAlert(VerificationMessages.InvalidBlogName);
                _blogNameTextBox.Focus();
                return;
            }

            BlogDescriptor.Address = _addressTextBox.Text;
            BlogDescriptor.BlogName = _blogNameTextBox.Text;
            BlogDescriptor.Password = _passwordTextBox.Text;
            BlogDescriptor.Username = _usernameTextBox.Text;

            InvokeFinish();
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            InvokeCancel();
        }
    }
}
