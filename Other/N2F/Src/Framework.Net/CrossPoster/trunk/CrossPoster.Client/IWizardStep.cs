/* ------------------------------------------------
 * IWizardStep.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Next2Friends.CrossPoster.Client.Logic;

namespace Next2Friends.CrossPoster.Client
{
    interface IWizardStep
    {
        BlogDescriptor BlogDescriptor { get; set; }
        event EventHandler Back;
        event EventHandler Cancel;
        event EventHandler Finish;
        event EventHandler Next;
        void Close();
        void Hide();
        void Show(Form wizardOwner, BlogDescriptor descriptor);
    }
}
