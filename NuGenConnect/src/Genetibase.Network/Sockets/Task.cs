using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets {
	public delegate void TaskExceptionEvent(Task task, Exception exception);
	/// <summary>
	/// The base class for tasks, executable by a <see cref="Scheduler"/>.
	/// </summary>
	public abstract class Task {
		/// <summary>
		/// The data associated with this <see cref="Task"/> instance.
		/// </summary>
		protected object mData;
		/// <summary>
		/// The <see cref="Yarn"/> instance associated with this <see cref="Task"/> instance.
		/// </summary>
		protected Yarn mYarn = null;

		/// <summary>
		/// Occurs before the method <see cref="Run"/> is called.
		/// </summary>
		public event EventHandler OnBeforeRun;

		/// <summary>
		/// Occurs after the method <see cref="Run"/> is called.
		/// </summary>
		public event EventHandler OnAfterRun;

		/// <summary>
		/// Gets called after the method <see cref="Run"/> is called.
		/// </summary>
		protected virtual void AfterRun() {
			if (OnAfterRun != null) {
				OnAfterRun(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Gets called before the method <see cref="Run"/> is called.
		/// </summary>
		protected virtual void BeforeRun() {
			if (OnBeforeRun != null) {
				OnBeforeRun(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Executes this task.
		/// </summary>
		/// <returns>
		///		<see langword="true"/> if this <see cref="Task"/> instance
		/// needs to be ran again, otherwise <see langword="false"/>.
		/// </returns>
		protected abstract bool Run();

		/// <summary>
		/// Calls <see cref="AfterRun"/>.
		/// </summary>
		public void DoAfterRun() {
			AfterRun();
		}

		/// <summary>
		/// Calls <see cref="BeforeRun"/>.
		/// </summary>
		public void DoBeforeRun() {
			BeforeRun();
		}

		/// <summary>
		/// Gets called when an exception occurs during this task run.
		/// </summary>
		/// <param name="exception">The exception.</param>
		protected abstract void DoException(Exception exception);

		/// <summary>
		/// Executes this task.
		/// </summary>
		/// <returns>
		///		<see langword="true"/> if this <see cref="Task"/> instance
		/// needs to be ran again, otherwise <see langword="false"/>.
		/// </returns>
		public bool DoRun() {
			try {
				return Run();
			} catch (Exception E) {
				DoException(E);
			}
			return false;
		}

		/// <summary>
		/// Gets the data associated with this <see cref="Task"/> instance.
		/// </summary>
		public object Data {
			get {
				return mData;
			}
		}

		/// <summary>
		/// Gets or sets the yarn associated with this <see cref="Task"/> instance.
		/// </summary>
		/// <value>The yarn.</value>
		public Yarn Yarn {
			get {
				return mYarn;
			}
			set {
				if (value != mYarn) {
					if (mYarn != null) {
						throw new Exception("Can only set the Yarn once!");
					}
					mYarn = value;
				}
			}
		}
	}
}