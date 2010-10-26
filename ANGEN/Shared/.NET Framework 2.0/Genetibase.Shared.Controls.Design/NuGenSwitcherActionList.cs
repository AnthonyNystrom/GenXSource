/* -----------------------------------------------
 * NuGenSwitcherActionList.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using ctrlProps = Genetibase.Shared.Controls.Properties;
using designProps = Genetibase.Shared.Controls.Design.Properties;

using Genetibase.Shared.Collections;

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
	public class NuGenSwitcherActionList : DesignerActionList
	{
		#region Methods.Public.Overridden

		/// <summary>
		/// Returns the collection of <see cref="T:System.ComponentModel.Design.DesignerActionItem"></see> objects contained in the list.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.ComponentModel.Design.DesignerActionItem"></see> array that contains the items in this list.
		/// </returns>
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection actionItems = new DesignerActionItemCollection();

			actionItems.Add(
				new DesignerActionMethodItem(
					this,
					"AddSwitchPage",
					designProps.Resources.ActionList_Switcher_AddSwitchPage
				)
			);
			actionItems.Add(
				new DesignerActionMethodItem(
					this,
					"RemoveSwitchPage",
					designProps.Resources.ActionList_Switcher_RemoveSwitchPage
				)
			);

			return actionItems;
		}

		#endregion

		#region Methods.Public

		/*
		 * AddSwitchPage
		 */

		/// <summary>
		/// </summary>
		public void AddSwitchPage()
		{
			this.CreateSwitchPage();
		}

		/*
		 * RemoveSwitchPage
		 */

		/// <summary>
		/// </summary>
		public void RemoveSwitchPage()
		{
			if (_switcher.SelectedSwitchPage != null)
			{
				this.DestroySwitchPage(_switcher.SelectedSwitchPage);
			}
		}

		#endregion

		#region Methods.Protected

		/// <summary>
		/// </summary>
		protected void CreateSwitchPage()
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
						transaction = host.CreateTransaction("NuGenSwitcher.AddSwitchPage");
					}
					catch (CheckoutException e)
					{
						if (e != CheckoutException.Canceled)
						{
							throw;
						}

						return;
					}

					NuGenSwitchPage switchPage = (NuGenSwitchPage)host.CreateComponent(typeof(NuGenSwitchPage));
					_switcher.SwitchPages.Add(switchPage);

					PropertyDescriptor textDescriptor = TypeDescriptor.GetProperties(switchPage)["Text"];

					if (textDescriptor != null)
					{
						textDescriptor.SetValue(switchPage, ctrlProps.Resources.BlankText);
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

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="switchPageToDestroy"/> is <see langword="null"/>.</para>
		/// </exception>
		protected void DestroySwitchPage(NuGenSwitchPage switchPageToDestroy)
		{
			if (switchPageToDestroy == null)
			{
				throw new ArgumentNullException("switchPageToDestroy");
			}

			IDesignerHost host = (IDesignerHost)this.GetService(typeof(IDesignerHost));

			if (host != null)
			{
				DesignerTransaction transaction = null;

				try
				{
					try
					{
						transaction = host.CreateTransaction("NuGenSwitcher.RemoveSwitchPage");
					}
					catch (CheckoutException e)
					{
						if (e != CheckoutException.Canceled)
						{
							throw;
						}

						return;
					}

					_switcher.SwitchPages.Remove(switchPageToDestroy);
					host.DestroyComponent(switchPageToDestroy);
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

		private void _switcher_SwitchPageRemoved(object sender, NuGenCollectionEventArgs<NuGenSwitchPage> e)
		{
			if (e.Item != null)
			{
				this.DestroySwitchPage(e.Item);
			}
		}

		#endregion

		private NuGenSwitcher _switcher;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSwitcherActionList"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="switcher"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSwitcherActionList(NuGenSwitcher switcher)
			: base(switcher)
		{
			if (switcher == null)
			{
				throw new ArgumentNullException("switcher");
			}

			_switcher = switcher;
			_switcher.SwitchPageRemoved += _switcher_SwitchPageRemoved;
		}
	}
}
