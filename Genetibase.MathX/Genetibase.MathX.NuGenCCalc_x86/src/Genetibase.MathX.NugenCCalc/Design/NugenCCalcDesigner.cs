using System;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace Genetibase.MathX.NugenCCalc.Design
{
	/// <summary>
	/// Designer for NugenMathX component
	/// </summary>
	public class NugenCCalcDesigner : ComponentDesigner
	{
		public NugenCCalcDesigner()
		{
			
		}

		/// <summary>
		/// This method provides an opportunity to perform processing when a designer is initialized.
		/// The component parameter is the component that the designer is associated with.
		/// </summary>
		/// <param name="component"></param>
		public override void Initialize(System.ComponentModel.IComponent component)
		{
			// Always call the base Initialize method in an override of this method.
			base.Initialize(component);
		}

		/// <summary>
		/// This method is invoked when the associated component is double-clicked.
		/// </summary>
		public override void DoDefaultAction()
		{
			ShowDesignerForm();
		}

		protected override void PreFilterProperties(System.Collections.IDictionary properties)
		{
			base.PreFilterProperties(properties);
			const string propName = "Function Type";
			properties[propName] = new FunctionTypePropertyDescriptor(this, 
				TypeDescriptor.CreateProperty(
				Component.GetType(),
				propName,
				typeof(FunctionType),
				new Attribute[] 
					{
						CategoryAttribute.Default,
						RefreshPropertiesAttribute.All
					}));
		}

		/// <summary>
		/// Gets the design-time verbs supported by the component that is associated with the designer.
		/// </summary>
		public override DesignerVerbCollection Verbs 
		{
			get
			{
				return new DesignerVerbCollection
					(
						new DesignerVerb[] 
							{
								new DesignerVerb(
								"NuGenCCalc Designer...", 
								new EventHandler(OnVerbClicked)),
								new DesignerVerb(
								"About", 
								new EventHandler(OnVerbClicked))
							}
					);
			}
		}

		/// <summary>
		/// Show designer form
		/// </summary>
		private void ShowDesignerForm()
		{
			if (this.Component is NugenCCalc2D)
			{
				NugenCCalcDesignerForm designerForm = new NugenCCalcDesignerForm((NugenCCalc2D)this.Component);
				designerForm.ShowDialog();
				TryRaiseComponentChanged();
			}
		}

		private void TryRaiseComponentChanged()
		{
			try
			{
				base.RaiseComponentChanging(null);
				base.RaiseComponentChanged(null, null, null);
			}
			catch (Exception)
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnVerbClicked(object sender, EventArgs e) 
		{
			switch(((DesignerVerb)sender).Text)
			{
				case "About":
					AboutForm about = new AboutForm();
					about.ShowDialog();
					break;
                case "NuGenCCalc Designer...":
					ShowDesignerForm();
					break;
			}
		}


	}
}
