/* -----------------------------------------------
 * NuGenDialogBlockDesigner.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.Properties;
using Genetibase.Shared;

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
	internal class NuGenDialogBlockDesigner : ParentControlDesigner
	{
		#region Declarations

		private NuGenDialogBlock _dialogBlock;
		private const int OFFSET = 5;

		#endregion

		#region Properties.Public

		/// <summary>
		/// Gets the selection rules that indicate the movement capabilities of a component.
		/// </summary>
		/// <value></value>
		public override SelectionRules SelectionRules
		{
			get
			{
				return base.SelectionRules & ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
			}
		}

		#endregion

		#region Methods.Public.Overridden

		/// <summary>
		/// Initializes the designer with the specified component.
		/// </summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent"/> to associate the designer with. This component must always be an instance of, or derive from, <see cref="T:System.Windows.Forms.Control"/> .</param>
		public override void Initialize(System.ComponentModel.IComponent component)
		{
			base.Initialize(component);
			_dialogBlock = (NuGenDialogBlock)component;
		}

		/// <summary>
		/// Called when the designer
		/// is initialized, so the designer can set default values for properties of the component.
		/// </summary>
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);

			IDesignerHost host = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			Debug.Assert(host != null, "host != null");

			IComponent hostComponent = host.RootComponent;
			Debug.Assert(hostComponent != null, "hostComponent != null");
			Debug.Assert(hostComponent is Form, "hostComponent is Form");

			Form hostForm = (Form)hostComponent;

			DesignerTransaction transaction = host.CreateTransaction("Host initialization");

			Button okButton = (Button)host.CreateComponent(
				typeof(Button),
				this.GetName(_dialogBlock, "okButton", 0)
				);

			Button cancelButton = (Button)host.CreateComponent(
				typeof(Button),
				this.GetName(_dialogBlock, "cancelButton", 0)
				);

			okButton.Text = Resources.Text_DialogBlock_okButton;
			cancelButton.Text = Resources.Text_DialogBlock_cancelButton;

			cancelButton.Left = _dialogBlock.Right - cancelButton.Width - OFFSET;
			okButton.Left = cancelButton.Left - okButton.Width - OFFSET;

			this.InitializeDialogButton(okButton);
			this.InitializeDialogButton(cancelButton);

			Debug.Assert(_dialogBlock != null, "this.dialogBlock != null");

			_dialogBlock.Controls.Add(okButton);
			_dialogBlock.Controls.Add(cancelButton);

			hostForm.AcceptButton = okButton;
			hostForm.CancelButton = cancelButton;

			transaction.Commit();
		}

		#endregion

		#region Methods.Private

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
		/// <param name="btn">Specifies the <see cref="T:Button"/> to initialize.</param>
		private void InitializeDialogButton(Button btn)
		{
			Debug.Assert(btn != null, "btn != null");
			
			if (btn != null)
			{
				btn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
				btn.FlatStyle = FlatStyle.System;
				btn.Top = (_dialogBlock.Height - btn.Height) / 2;

				PropertyDescriptor property = TypeDescriptor.GetProperties(btn)["Locked"];

				if (property != null) 
				{
					property.SetValue(btn, true);
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

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDialogBlockDesigner"/> class.
		/// </summary>
		public NuGenDialogBlockDesigner()
		{
		}
		
		#endregion
	}
}
