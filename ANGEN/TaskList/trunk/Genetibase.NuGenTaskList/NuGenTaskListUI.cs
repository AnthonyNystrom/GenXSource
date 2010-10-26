/* -----------------------------------------------
 * NuGenTaskListUI.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using EnvDTE;
using EnvDTE80;

using Genetibase.Controls;
using Genetibase.NuGenTaskList.Properties;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Timers;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// Represents the add-in's tool window.
	/// </summary>
	public partial class NuGenTaskListUI : UserControl
	{
		#region Properties.Internal

		/*
		 * DTE
		 */

		private DTE2 _DTE = null;

		/// <summary>
		/// </summary>
		internal DTE2 DTE
		{
			get
			{
				return _DTE;
			}
			set
			{
				if (_DTE != value)
				{
					_DTE = value;

					if (_DTE != null)
					{
						_TaskList = (TaskList)_DTE.Windows.Item(Constants.vsWindowKindTaskList).Object;
						this.TaskListRefresher.Start();
					}
				}
			}
		}

		#endregion

		#region Properties.Protected

		/*
		 * TaskList
		 */

		private TaskList _TaskList = null;

		/// <summary>
		/// </summary>
		protected TaskList TaskList
		{
			get
			{
				return _TaskList;
			}
		}

		/*
		 * TaskListRefresher
		 */

		private Timer _TaskListRefresher = null;

		/// <summary>
		/// </summary>
		protected Timer TaskListRefresher
		{
			/* Since standard TaskList does not notifies about task modification and removal through
			 * TaskListEvents events, timer is used to request TaskList state.
			 */

			get
			{
				if (_TaskListRefresher == null)
				{
					_TaskListRefresher = new Timer(_Components);
					_TaskListRefresher.Interval = 1000;
					_TaskListRefresher.Enabled = false;

					_TaskListRefresher.Tick += _TaskListRefresher_Tick;
				}

				return _TaskListRefresher;
			}
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * CompletedComparer
		 */

		private IComparer<NuGenTaskTreeNodeBase> _CompletedComparer = null;

		/// <summary>
		/// </summary>
		protected virtual IComparer<NuGenTaskTreeNodeBase> CompletedComparer
		{
			get
			{
				if (_CompletedComparer == null)
				{
					_CompletedComparer = new NuGenCompletedTaskTreeNodeComparer();
				}

				return _CompletedComparer;
			}
		}

		/*
		 * DescriptionComparer
		 */

		private IComparer<NuGenTaskTreeNodeBase> _DescriptionComparer = null;

		/// <summary>
		/// </summary>
		protected virtual IComparer<NuGenTaskTreeNodeBase> DescriptionComparer
		{
			get
			{
				if (_DescriptionComparer == null)
				{
					_DescriptionComparer = new NuGenAZTaskTreeNodeComparer();
				}

				return _DescriptionComparer;
			}
		}

		/*
		 * PriorityComparer
		 */

		private IComparer<NuGenTaskTreeNodeBase> _PriorityComparer = null;

		/// <summary>
		/// </summary>
		protected virtual IComparer<NuGenTaskTreeNodeBase> PriorityComparer
		{
			get
			{
				if (_PriorityComparer == null)
				{
					_PriorityComparer = new NuGenPriorityTaskTreeNodeComparer();
				}

				return _PriorityComparer;
			}
		}
		
		#endregion

		#region Methods.Public

		/*
		 * Restore
		 */

		/// <summary>
		/// </summary>
		/// <param name="path"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="path"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		/// The file specified by the <paramref name="path"/> cannot be found.
		/// </exception>
		public void Restore(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}

			try
			{
				_TaskTreeView.Load(path);
			}
			catch (FileNotFoundException)
			{
				_TaskTreeView.Clear();
			}
			catch (Exception)
			{
				_TaskTreeView.Nodes.Clear();
			}
		}

		/*
		 * Save
		 */

		/// <summary>
		/// </summary>
		/// <param name="path"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="path"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="path"/> is an empty string.
		/// </exception>
		public void Save(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}

			try
			{
				_TaskTreeView.Save(path);
			}
			catch (DirectoryNotFoundException)
			{
				try
				{
					Directory.CreateDirectory(Path.GetDirectoryName(path));
					_TaskTreeView.Save(path);
				}
				catch
				{
				}
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.UserControl.Load"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.CheckOutTaskList();
		}

		#endregion

		#region Methods.Private

		private void CheckOutTaskList()
		{
			Debug.Assert(this.TaskList != null, "this.TaskList != null");

			_TaskTreeView.SuspendLayout();
			_TaskTreeView.ClearCodeTasks();

			foreach (TaskItem taskItem in this.TaskList.TaskItems)
			{
				if (taskItem.Category == Resources.TaskListCategory_Comments)
				{
					_TaskTreeView.AddCodeTask(taskItem);
				}
			}

			_TaskTreeView.ResumeLayout();
		}

		#endregion

		#region EventHandlers.Sort

		private void _CompletedMenuItem_Click(object sender, EventArgs e)
		{
			Debug.Assert(this.CompletedComparer != null, "this.CompletedComparer != null");
			_TaskTreeView.Sort<NuGenTaskTreeNodeBase>(this.CompletedComparer);
		}

		private void _DescriptionMenuItem_Click(object sender, EventArgs e)
		{
			Debug.Assert(this.DescriptionComparer != null, "this.DescriptionComparer != null");
			_TaskTreeView.Sort<NuGenTaskTreeNodeBase>(this.DescriptionComparer);
		}

		private void _PriorityMenuItem_Click(object sender, EventArgs e)
		{
			Debug.Assert(this.PriorityComparer != null, "this.PriorityComparer != null");
			_TaskTreeView.Sort<NuGenTaskTreeNodeBase>(this.PriorityComparer);
		}

		#endregion

		#region EventHandlers.Task

		private void _NewFolderMenuItem_Click(object sender, EventArgs e)
		{
			_TaskTreeView.AddFolder(Resources.NewFolder_DefaultText);
		}

		private void _NewTaskMenuItem_Click(object sender, EventArgs e)
		{
			_TaskTreeView.AddTask(Resources.NewTask_DefaultText);
		}

		#endregion

		#region EventHandlers.TaskListRefresher

		private void _TaskListRefresher_Tick(object sender, EventArgs e)
		{
			/*
			 * Do NOT refresh Comments folder if user is operating the task tree view.
			 * Otherwise, the tree looses focus.
			 */

			if (!_TaskTreeView.Focused)
			{
				this.CheckOutTaskList();
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskListUI"/> class.
		/// </summary>
		public NuGenTaskListUI()
		{
			this.InitializeComponent();

			INuGenTimer timer = new NuGenTimer();
			timer.Interval = 500;

			NuGenDEHService service = new NuGenDEHService(timer);
			service.AddClient(_TaskEditBox);
			service.AddClient(_TaskTreeView);
		}

		#endregion
	}
}
