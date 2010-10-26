/* -----------------------------------------------
 * NuGenDialogBlockDesigner.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.Properties;
using Genetibase.Shared;
using Genetibase.Shared.Controls;

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.ApplicationBlocks.Design
{
	/// <summary>
	/// Provides additional design-time funcionality for the <see cref="NuGenDialogBlock"/>.
	/// </summary>
	public abstract class NuGenDialogBlockDesigner : ParentControlDesigner
	{
		/// <summary>
		/// Gets the selection rules that indicate the movement capabilities of a component.
		/// </summary>
		/// <value></value>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.Design.SelectionRules"></see> values.</returns>
		public override SelectionRules SelectionRules
		{
			get
			{
				return base.SelectionRules & ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
			}
		}

		/// <summary>
		/// Initializes the designer with the specified component.
		/// </summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent"></see> to associate with the designer.</param>
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			_dialogBlock = (NuGenDialogBlock)component;
		}

		/// <summary>
		/// </summary>
		protected void CreateButtons<TButton>()
			where TButton : NuGenButton, new()
		{
			IDesignerHost host = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			Debug.Assert(host != null, "host != null");

			Control hostCtrl = host.RootComponent as Control;
			Debug.Assert(hostCtrl != null, "hostCtrl != null");

			DesignerTransaction transaction = host.CreateTransaction("DialogBlock initialization");

			TButton okButton = (TButton)host.CreateComponent(typeof(TButton), this.GetName(_dialogBlock, "okButton", 0));
			TButton cancelButton = (TButton)host.CreateComponent(typeof(TButton), this.GetName(_dialogBlock, "cancelButton", 0));

			okButton.Text = Resources.Text_DialogBlock_okButton;
			cancelButton.Text = Resources.Text_DialogBlock_cancelButton;

			cancelButton.Left = _dialogBlock.Right - cancelButton.Width - OFFSET;
			okButton.Left = cancelButton.Left - okButton.Width - OFFSET;

			this.InitializeDialogButton(okButton);
			this.InitializeDialogButton(cancelButton);

			Debug.Assert(_dialogBlock != null, "this.dialogBlock != null");

			_dialogBlock.Controls.Add(okButton);
			_dialogBlock.Controls.Add(cancelButton);

			Form hostForm = hostCtrl as Form;

			if (hostForm != null)
			{
				hostForm.AcceptButton = okButton;
				hostForm.CancelButton = cancelButton;
			}

			transaction.Commit();
		}

		/// <summary>
		/// Retrieves a component name according to the specified default component name and the 
		/// specified postfix.
		/// </summary>
		/// <param name="name">Specifies the default component name.</param>
		/// <param name="postfix">Specifies the postfix for the component name.</param>
		/// <returns></returns>
		private string GetCurrentComponentName(string name, int postfix)
		{
			return string.Format("{0}{1}", name, postfix > 0 ? postfix.ToString() : "");
		}

		/// <summary>
		/// Retrieves a qualified name within the specified container.
		/// </summary>
		/// <param name="container">Specifies the container that should contain only unique names.</param>
		/// <param name="defaultName">Specifies the default name for a component.</param>
		/// <param name="defaultCount">Specifies the default postfix. If zero no postfix is appended.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="container"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="defaultName"/> is <see langword="null"/>.
		/// </exception>
		private string GetName(Control container, string defaultName, int defaultCount)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}

			if (defaultName == null)
			{
				throw new ArgumentNullException("defaultName");
			}

			string name = this.GetCurrentComponentName(defaultName, defaultCount);

			foreach (Component component in container.Container.Components)
			{
				if (this.NameAlreadyExists(component, name))
				{
					name = this.GetName(container, defaultName, ++defaultCount);
				}
			}

			return name;
		}

		/// <summary>
		/// Initializes the specified <see cref="T:Button"/>.
		/// </summary>
		private void InitializeDialogButton(Control buttonToInitialize)
		{
			Debug.Assert(buttonToInitialize != null, "buttonToInitialize != null");

			if (buttonToInitialize != null)
			{
				buttonToInitialize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
				buttonToInitialize.Top = (_dialogBlock.Height - buttonToInitialize.Height) / 2;

				PropertyDescriptor property = TypeDescriptor.GetProperties(buttonToInitialize)["Locked"];

				if (property != null)
				{
					property.SetValue(buttonToInitialize, true);
				}
			}
		}

		/// <summary>
		/// Indicates whether the name of the specified component is equal to the specified name.
		/// </summary>
		/// <param name="component">Specifies the component to retrieve the name from.</param>
		/// <param name="name">Specifies the name to compare with.</param>
		/// <returns><see langword="true"/> if the name of the specified component is equal to the 
		/// specified name; otherwise, <see langword="false"/>.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="component"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="name"/> is <see langword="null"/>.
		/// </exception>
		private bool NameAlreadyExists(IComponent component, string name)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}

			if (name == null)
			{
				throw new ArgumentNullException("name");
			}

			return (string)TypeDescriptor.GetProperties(component)["Name"].GetValue(component) == name ? true : false;
		}

		private NuGenDialogBlock _dialogBlock;
		private const int OFFSET = 5;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDialogBlockDesigner"/> class.
		/// </summary>
		protected NuGenDialogBlockDesigner()
		{
		}
	}
}
