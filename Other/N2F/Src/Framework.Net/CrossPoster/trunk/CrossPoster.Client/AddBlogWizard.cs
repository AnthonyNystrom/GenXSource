/* ------------------------------------------------
 * AddBlogWizard.cs
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
    sealed class AddBlogWizard
    {
        private IList<IWizardStep> _steps;
        private Int32 _currentStepIndex;
        private Form _wizardOwner;

        /// <summary>
        /// Creates a new instance of the <code>AddBlogWizard</code> class.
        /// </summary>
        public AddBlogWizard()
            : this(null)
        {
        }

        /// <summary>
        /// Creates a new instance of the <code>AddBlogWizard</code> class.
        /// </summary>
        /// <param name="wizardOwner"></param>
        public AddBlogWizard(Form wizardOwner)
        {
            _steps = new List<IWizardStep>();
            _wizardOwner = wizardOwner;
        }

        public event EventHandler<WizardFinishedEventArgs> Finished;
        private void InvokeFinished(WizardFinishedEventArgs e)
        {
            if (Finished != null)
                Finished(this, e);
        }

        public void AddStep(IWizardStep step)
        {
            if (step == null)
                throw new ArgumentNullException("step");
            _steps.Add(step);

            step.Back += WizardStep_Back;
            step.Cancel += WizardStep_Cancel;
            step.Finish += WizardStep_Finish;
            step.Next += WizardStep_Next;
        }

        /// <summary>
        /// </summary>
        /// <param name="blogDescriptor"></param>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>blogDescriptor</code> is <code>null</code>.
        /// </exception>
        public void Start(BlogDescriptor blogDescriptor)
        {
            if (blogDescriptor == null)
                throw new ArgumentNullException("blogDescriptor");
            if (_steps.Count == 0)
                return;
            _currentStepIndex = 0;
            _steps[_currentStepIndex].Show(_wizardOwner, blogDescriptor);
        }

        private delegate Int32 StepIndexChanger(Int32 currentStepIndex);

        private void ChangeStep(IWizardStep currentStep, Predicate<Int32> canChange, StepIndexChanger stepIndexChanger)
        {
            if (canChange(_currentStepIndex))
            {
                currentStep.Hide();
                _currentStepIndex = stepIndexChanger(_currentStepIndex);
                _steps[_currentStepIndex].Show(_wizardOwner, currentStep.BlogDescriptor);
            }
        }

        private void CleanUp()
        {
            foreach (var step in _steps)
            {
                step.Back -= WizardStep_Back;
                step.Cancel -= WizardStep_Cancel;
                step.Finish -= WizardStep_Finish;
                step.Next -= WizardStep_Next;
                step.Close();
            }

            _steps.Clear();
        }

        private void WizardStep_Back(Object sender, EventArgs e)
        {
            ChangeStep((IWizardStep)sender, i => i > 0, i => i - 1);
        }

        private void WizardStep_Cancel(Object sender, EventArgs e)
        {
            InvokeFinished(new WizardFinishedEventArgs(true, ((IWizardStep)sender).BlogDescriptor));
            CleanUp();
        }

        private void WizardStep_Finish(Object sender, EventArgs e)
        {
            InvokeFinished(new WizardFinishedEventArgs(false, ((IWizardStep)sender).BlogDescriptor));
            CleanUp();
        }

        private void WizardStep_Next(Object sender, EventArgs e)
        {
            ChangeStep((IWizardStep)sender, i => i < _steps.Count - 1, i => i + 1);
        }
    }
}
