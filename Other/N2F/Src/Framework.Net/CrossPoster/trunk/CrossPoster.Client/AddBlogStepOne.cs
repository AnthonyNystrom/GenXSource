/* ------------------------------------------------
 * AddBlogStepOne.cs
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
    sealed partial class AddBlogStepOne : WizardForm, IWizardStep
    {
        public AddBlogStepOne()
        {
            InitializeComponent();
        }

        private void _nextButton_Click(Object sender, EventArgs e)
        {
            BlogDescriptor.BlogType = (BlogType)Enum.Parse(typeof(BlogType), Convert.ToString(_blogList.SelectedNode.Tag));
            InvokeNext();
        }

        private void _cancelButton_Click(Object sender, EventArgs e)
        {
            InvokeCancel();
        }
    }
}
