/* ------------------------------------------------
 * WizardForm.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Next2Friends.CrossPoster.Client.Logic;

namespace Next2Friends.CrossPoster.Client
{
    class WizardForm : Form, IWizardStep
    {
        private static readonly Object _backEvent = new Object();
        public event EventHandler Back
        {
            add { Events.AddHandler(_backEvent, value); }
            remove { Events.RemoveHandler(_backEvent, value); }
        }
        protected void InvokeBack()
        {
            InvokeEvent(_backEvent);
        }

        private static readonly Object _cancelEvent = new Object();
        public event EventHandler Cancel
        {
            add { Events.AddHandler(_cancelEvent, value); }
            remove { Events.RemoveHandler(_cancelEvent, value); }
        }
        protected void InvokeCancel()
        {
            InvokeEvent(_cancelEvent);
        }

        private static readonly Object _finishEvent = new Object();
        public event EventHandler Finish
        {
            add { Events.AddHandler(_finishEvent, value); }
            remove { Events.RemoveHandler(_finishEvent, value); }
        }
        protected void InvokeFinish()
        {
            InvokeEvent(_finishEvent);
        }

        private static readonly Object _nextEvent = new Object();
        public event EventHandler Next
        {
            add { Events.AddHandler(_nextEvent, value); }
            remove { Events.RemoveHandler(_nextEvent, value); }
        }
        protected void InvokeNext()
        {
            InvokeEvent(_nextEvent);
        }

        private void InvokeEvent(Object key)
        {
            ((EventHandler)Events[key])(this, EventArgs.Empty);
        }

        public BlogDescriptor BlogDescriptor
        {
            get;
            set;
        }

        protected virtual void PreviewShow(BlogDescriptor blogDescriptor)
        {
        }

        void IWizardStep.Close()
        {
            base.Close();
        }

        void IWizardStep.Hide()
        {
            base.Hide();
        }

        void IWizardStep.Show(Form wizardOwner, BlogDescriptor blogDescriptor)
        {
            BlogDescriptor = blogDescriptor;
            PreviewShow(blogDescriptor);
            Show(wizardOwner);
        }
    }
}
