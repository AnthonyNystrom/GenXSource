/* -----------------------------------------------
 * NuGenSmoothNavigationBarDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.Design;

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace Genetibase.SmoothControls.Design
{
	internal sealed class NuGenSmoothNavigationBarDesigner : NuGenNavigationBarDesigner
	{
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection actionLists = new DesignerActionListCollection();
				Debug.Assert(_navigationBar != null, "_navigationBar != null");
				NuGenSmoothNavigationBarActionList actionList = new NuGenSmoothNavigationBarActionList(_navigationBar);
				actionList.AutoShow = true;
				actionLists.Add(actionList);
				return actionLists;
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			properties.Remove("BackColor");
			properties.Remove("BorderStyle");
			base.PreFilterProperties(properties);
		}

		private NuGenNavigationBar _navigationBar;

		public override void Initialize(IComponent component)
		{
			Debug.Assert(component != null, "component != null");
			_navigationBar = (NuGenNavigationBar)component;
			base.Initialize(component);
		}
	}
}
