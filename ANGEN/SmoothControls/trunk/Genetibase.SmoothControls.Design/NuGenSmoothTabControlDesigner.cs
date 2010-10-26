/* -----------------------------------------------
 * NuGenSmoothTabControlDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.Design;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace Genetibase.SmoothControls.Design
{
	internal sealed class NuGenSmoothTabControlDesigner : NuGenTabControlDesigner
	{
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection actionLists = new DesignerActionListCollection();
				Debug.Assert(_tabControl != null, "_tabControl != null");
				NuGenSmoothTabControlActionList actionList = new NuGenSmoothTabControlActionList(_tabControl);
				actionList.AutoShow = true;
				actionLists.Add(actionList);
				return actionLists;
			}
		}

		private NuGenTabControl _tabControl;

		public override void Initialize(IComponent component)
		{
			Debug.Assert(component != null, "component != null");
			_tabControl = (NuGenTabControl)component;
			base.Initialize(component);
		}
	}
}
