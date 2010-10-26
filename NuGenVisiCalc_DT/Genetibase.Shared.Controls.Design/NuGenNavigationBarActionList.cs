/* -----------------------------------------------
 * NuGenNavigationBarActionList.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using designProps = Genetibase.Shared.Controls.Design.Properties;
using ctrlProps = Genetibase.Shared.Controls.Properties;

using Genetibase.Shared.Collections;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Design;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// </summary>
	public abstract class NuGenNavigationBarActionList : DesignerActionList
	{
		#region Methods.Public.Overridden

		/*
		 * GetSortedActionItems
		 */

		/// <summary>
		/// Returns the collection of <see cref="T:System.ComponentModel.Design.DesignerActionItem"></see> objects contained in the list.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.ComponentModel.Design.DesignerActionItem"></see> array that contains the items in this list.
		/// </returns>
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection collection = new DesignerActionItemCollection();

			collection.Add(new DesignerActionMethodItem(
				this,
				"AddNavigationPane",
				designProps.Resources.ActionList_NavigationBar_AddNavigationPane)
			);

			collection.Add(new DesignerActionMethodItem(
				this,
				"RemoveNavigationPane",
				designProps.Resources.ActionList_NavigationBar_RemoveNavigationPane)
			);

			return collection;

		}

		#endregion

		#region Methods.Public.Virtual

		/*
		 * AddNavigationPane
		 */

		/// <summary>
		/// </summary>
		public abstract void AddNavigationPane();

		/*
		 * RemoveNavigationPane
		 */

		/// <summary>
		/// </summary>
		public virtual void RemoveNavigationPane()
		{
			NuGenNavigationPane selectedNavigationPane = _navigationBar.SelectedNavigationPane;

			if (selectedNavigationPane != null)
			{
				this.DestroyNavigationPane(selectedNavigationPane);
			}
		}

		#endregion

		#region Methods.Protected

		/*
		 * CreateNavigationPane
		 */

		/// <summary>
		/// </summary>
		protected void CreateNavigationPane<TPane>() where TPane : NuGenNavigationPane
		{
			IDesignerHost host = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			Debug.Assert(host != null, "host != null");

			if (host != null)
			{
				DesignerTransaction transaction = null;

				try
				{
					try
					{
						transaction = host.CreateTransaction("NuGenNavigationBar.AddNavigationPane");
					}
					catch (CheckoutException e)
					{
						if (e != CheckoutException.Canceled)
						{
							throw;
						}

						return;
					}

					TPane navigationPane = (TPane)host.CreateComponent(typeof(TPane));
					_navigationBar.NavigationPanes.Add(navigationPane);

					PropertyDescriptor textDescriptor = TypeDescriptor.GetProperties(navigationPane)["Text"];

					if (textDescriptor != null)
					{
						textDescriptor.SetValue(navigationPane, ctrlProps.Resources.BlankText);
					}
				}
				finally
				{
					if (transaction != null)
					{
						transaction.Commit();
					}
				}
			}
		}

		/*
		 * DestroyNavigationPane
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="navigationPaneToDestroy"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		protected void DestroyNavigationPane(NuGenNavigationPane navigationPaneToDestroy)
		{
			if (navigationPaneToDestroy == null)
			{
				throw new ArgumentNullException("navigationPaneToDestroy");
			}

			IDesignerHost host = (IDesignerHost)this.GetService(typeof(IDesignerHost));

			if (host != null)
			{
				DesignerTransaction transaction = null;

				try
				{
					try
					{
						transaction = host.CreateTransaction("NuGenNavigationBar.RemoveNavigationPane");
					}
					catch (CheckoutException e)
					{
						if (e != CheckoutException.Canceled)
						{
							throw;
						}

						return;
					}

					_navigationBar.NavigationPanes.Remove(navigationPaneToDestroy);
					host.DestroyComponent(navigationPaneToDestroy);
				}
				finally
				{
					if (transaction != null)
					{
						transaction.Commit();
					}
				}
			}
		}

		#endregion

		#region EventHandlers

		private void _navigationBar_NavigationPaneRemoved(object sender, NuGenCollectionEventArgs<NuGenNavigationPane> e)
		{
			NuGenNavigationPane navigationPaneToRemove = e.Item;

			if (navigationPaneToRemove != null)
			{
				this.DestroyNavigationPane(navigationPaneToRemove);
			}
		}

		#endregion

		private NuGenNavigationBar _navigationBar;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenNavigationBarActionList"/> class.
		/// </summary>
		protected NuGenNavigationBarActionList(NuGenNavigationBar navigationBar)
			: base(navigationBar)
		{
			_navigationBar = navigationBar;
			_navigationBar.NavigationPaneRemoved += _navigationBar_NavigationPaneRemoved;
		}
	}
}
