/* -----------------------------------------------
 * ToolboxForm.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using VisiTypes = Genetibase.NuGenVisiCalc.Types;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Genetibase.NuGenVisiCalc.Properties;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;
using Genetibase.NuGenVisiCalc.Operators;
using Genetibase.NuGenVisiCalc.Params;
using Genetibase.NuGenVisiCalc.Programs;

namespace Genetibase.NuGenVisiCalc
{
	[System.ComponentModel.DesignerCategory("Form")]
	internal sealed partial class ToolboxForm : FloatingToolForm
	{
		#region Properties.Services

		private OperatorsCache _operatorsCache;

		private OperatorsCache OperatorsCache
		{
			get
			{
				if (_operatorsCache == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_operatorsCache = this.ServiceProvider.GetService<OperatorsCache>();

					if (_operatorsCache == null)
					{
						throw new NuGenServiceNotFoundException<OperatorsCache>();
					}
				}

				return _operatorsCache;
			}
		}

		private ParamsCache _paramsCache;

		private ParamsCache ParamsCache
		{
			get
			{
				if (_paramsCache == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_paramsCache = this.ServiceProvider.GetService<ParamsCache>();

					if (_paramsCache == null)
					{
						throw new NuGenServiceNotFoundException<ParamsCache>();
					}
				}

				return _paramsCache;
			}
		}

		private ProgramsCache _programsCache;

		private ProgramsCache ProgramsCache
		{
			get
			{
				if (_programsCache == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_programsCache = this.ServiceProvider.GetService<ProgramsCache>();

					if (_programsCache == null)
					{
						throw new NuGenServiceNotFoundException<ProgramsCache>();
					}
				}

				return _programsCache;
			}
		}

		private TypesCache _typesCache;

		private TypesCache TypesCache
		{
			get
			{
				if (_typesCache == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_typesCache = this.ServiceProvider.GetService<TypesCache>();

					if (_typesCache == null)
					{
						throw new NuGenServiceNotFoundException<TypesCache>();
					}
				}

				return _typesCache;
			}
		}

		private INuGenServiceProvider _serviceProvider;

		private INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		private INuGenWindowStateTracker _windowStateTracker;

		private INuGenWindowStateTracker WindowStateTracker
		{
			get
			{
				if (_windowStateTracker == null)
				{
					Debug.Assert(ServiceProvider != null, "ServiceProvider != null");
					_windowStateTracker = ServiceProvider.GetService<INuGenWindowStateTracker>();

					if (_windowStateTracker == null)
					{
						throw new NuGenServiceNotFoundException<INuGenWindowStateTracker>();
					}
				}

				return _windowStateTracker;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			Settings.Default.ToolboxForm_Location = Location;
			Settings.Default.ToolboxForm_Size = Size;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Location = Settings.Default.ToolboxForm_Location;
			Size = Settings.Default.ToolboxForm_Size;

			LoadPrograms();
			LoadParams();
			LoadTypes();
			LoadOperators();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			WindowStateTracker.SetWindowState(this);
		}

		#endregion

		private void LoadOperators()
		{
			Dictionary<String, OperatorDescriptor> operators = OperatorsCache.Operators;
			OperatorDescriptor[] operatorsToSort = new OperatorDescriptor[operators.Values.Count];
			operators.Values.CopyTo(operatorsToSort, 0);

			Array.Sort<OperatorDescriptor>(operatorsToSort, new OperatorDescriptorComparer());

			foreach (OperatorDescriptor descriptor in operatorsToSort)
			{
				AddOperator(descriptor);
			}

			_operatorNode.Expand();
		}

		private void LoadParams()
		{
			IList<ParamDescriptor> parameters = ParamsCache.Params;
			ParamDescriptor[] parametersToSort = new ParamDescriptor[parameters.Count];
			parameters.CopyTo(parametersToSort, 0);

			Array.Sort<ParamDescriptor>(parametersToSort, new ParamDescriptorComparer());

			foreach (ParamDescriptor descriptor in parametersToSort)
			{
				AddParam(descriptor);
			}
		}

		private void LoadPrograms()
		{
			IList<ProgramDescriptor> programs = ProgramsCache.Programs;
			ProgramDescriptor[] programsToSort = new ProgramDescriptor[programs.Count];
			programs.CopyTo(programsToSort, 0);

			Array.Sort<ProgramDescriptor>(programsToSort, new ProgramDescriptorComparer());

			foreach (ProgramDescriptor descriptor in programsToSort)
			{
				AddProgram(descriptor);
			}
		}

		private void LoadTypes()
		{
			IList<Genetibase.NuGenVisiCalc.Types.TypeDescriptor> types = TypesCache.Types;
			VisiTypes.TypeDescriptor[] typesToSort = new VisiTypes.TypeDescriptor[types.Count];
			types.CopyTo(typesToSort, 0);

			Array.Sort<VisiTypes.TypeDescriptor>(typesToSort, new VisiTypes.TypeDescriptorComparer());

			foreach (VisiTypes.TypeDescriptor descriptor in typesToSort)
			{
				AddType(descriptor);
			}
		}

		private void AddOperator(OperatorDescriptor operatorDescriptor)
		{
			TreeNode node = new TreeNode(operatorDescriptor.ToString());
			node.Tag = operatorDescriptor;
			_operatorNode.Nodes.Add(node);
		}

		private void AddParam(ParamDescriptor paramDescriptor)
		{
			TreeNode node = new TreeNode(paramDescriptor.ToString());
			node.Tag = paramDescriptor;
			_paramNode.Nodes.Add(node);
		}

		private void AddProgram(ProgramDescriptor programDescriptor)
		{
			TreeNode node = new TreeNode(programDescriptor.ToString());
			node.Tag = programDescriptor;
			_programNode.Nodes.Add(node);
		}

		private void AddType(VisiTypes.TypeDescriptor typeDescriptor)
		{
			TreeNode node = new TreeNode(typeDescriptor.ToString());
			node.Tag = typeDescriptor;
			_typeNode.Nodes.Add(node);
		}

		private void _opTreeView_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				TreeNode treeNode = _opTreeView.SelectedNode;

				if (treeNode != null && treeNode.Tag != null)
				{
					_opTreeView.DoDragDrop(treeNode.Tag, DragDropEffects.Copy);
				}
			}
		}

		private TreeNode _paramNode;
		private TreeNode _typeNode;
		private TreeNode _operatorNode;
		private TreeNode _programNode;

		public ToolboxForm()
		{
			InitializeComponent();
		}

		public ToolboxForm(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;
			InitializeComponent();
			SetStyle(ControlStyles.Opaque, true);

			_operatorNode = new TreeNode(Resources.Text_Operators);
			_paramNode = new TreeNode(Resources.Text_Params);
			_typeNode = new TreeNode(Resources.Text_Types);
			_programNode = new TreeNode(Resources.Text_Programs);

			_opTreeView.Nodes.AddRange(new TreeNode[] { _paramNode, _operatorNode, _typeNode, _programNode });
		}
	}
}