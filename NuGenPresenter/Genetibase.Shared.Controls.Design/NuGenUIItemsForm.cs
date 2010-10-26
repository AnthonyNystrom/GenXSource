using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Genetibase.Shared.Controls.Design
{
    /// <summary>
    /// Form for editing item collection at design-time.
    /// </summary>
    internal sealed partial class NuGenUIItemsForm : Form
    {
        private NuGenCommandManagerBase UISwitchboard;
        private Collection<object> associatedItems;
        private Collection<object> availableItems;
        private ListView dragSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenUIItemsForm"/> class.
        /// </summary>
        /// <param name="applicationCommand">The command to edit.</param>
        /// <param name="associatedItems">Items associated with the command.</param>
        /// <param name="availableItems">Itemas available for association with the command.</param>
        public NuGenUIItemsForm(NuGenApplicationCommand applicationCommand, Collection<object> associatedItems, Collection<object> availableItems)
        {
            UISwitchboard = applicationCommand.CommandManager;

            this.associatedItems = associatedItems;
            this.availableItems = availableItems;

            InitializeComponent();

            // Set command information
            labelCommandName.Text = applicationCommand.ApplicationCommandName;

            // Fill listview with associated items
            foreach (object item in associatedItems)
            {
                string name = UISwitchboard.UIItemAdapter.GetUIItemName(item);
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = name;
                listViewItem.Tag = item;
                listViewAssociatedItems.Items.Add(listViewItem);
            }

            // Fill listview with available items
            foreach (object item in availableItems)
            {
                if (!associatedItems.Contains(item))
                {
                    string name = UISwitchboard.UIItemAdapter.GetUIItemName(item);
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Text = name;
                    listViewItem.Tag = item;
                    listViewAvailableItems.Items.Add(listViewItem);
                }
            }

            columnHeaderAvailableItems.Width = listViewAvailableItems.ClientSize.Width;
            columnHeaderAssociatedItems.Width = listViewAssociatedItems.ClientSize.Width;
        }

        /// <summary>
        /// Called when an available item is double clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewAvailableItems_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem listViewItem = listViewAvailableItems.SelectedItems[0];
            listViewAvailableItems.Items.Remove(listViewItem);
            listViewAssociatedItems.Items.Add(listViewItem);

            availableItems.Remove(listViewItem.Tag);
            associatedItems.Add(listViewItem.Tag);
        }

        /// <summary>
        /// Called when an associated item is double clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewAssociatedItems_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem listViewItem = listViewAssociatedItems.SelectedItems[0];
            listViewAssociatedItems.Items.Remove(listViewItem);
            listViewAvailableItems.Items.Add(listViewItem);

            associatedItems.Remove(listViewItem.Tag);
            availableItems.Add(listViewItem.Tag);
        }

        /// <summary>
        /// Called when the available items listview is resized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewAvailableItems_Resize(object sender, EventArgs e)
        {
            columnHeaderAvailableItems.Width = listViewAvailableItems.ClientSize.Width;
        }

        /// <summary>
        /// Called when the associated items listview is resized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewAssociatedItems_Resize(object sender, EventArgs e)
        {
            columnHeaderAssociatedItems.Width = listViewAssociatedItems.ClientSize.Width;
        }

        /// <summary>
        /// Called when a drag operation is stared.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ListView listView = sender as ListView;
                List<ListViewItem> listViewItems = new List<ListViewItem>();
                foreach (ListViewItem listViewItem in listView.SelectedItems)
                {
                    listViewItems.Add(listViewItem.Clone() as ListViewItem);
                }
                dragSource = listView;
                if (DoDragDrop(listViewItems, DragDropEffects.Move) == DragDropEffects.Move)
                {
                    foreach (ListViewItem listViewItem in listView.SelectedItems)
                    {
                        listView.Items.Remove(listViewItem);
                        if (listView == listViewAvailableItems)
                        {
                            availableItems.Remove(listViewItem.Tag);
                        }
                        else
                        {
                            associatedItems.Remove(listViewItem.Tag);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called when a drag operation enters the listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_DragEnter(object sender, DragEventArgs e)
        {
            ListView listView = sender as ListView;
            if ((e.Data.GetDataPresent(typeof(List<ListViewItem>)) && (dragSource != listView)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Called when a drag operation is moving over the listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_DragOver(object sender, DragEventArgs e)
        {
            // Do nothing
        }

        /// <summary>
        /// Called when a drag operation leaves the listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_DragLeave(object sender, EventArgs e)
        {
            // Do nothing
        }

        /// <summary>
        /// Called when a drag operation is dropped on a listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_DragDrop(object sender, DragEventArgs e)
        {
            ListView listView = sender as ListView;
            List<ListViewItem> listViewItems = e.Data.GetData(typeof(List<ListViewItem>)) as List<ListViewItem>;
            if (listViewItems != null)
            {
                foreach (ListViewItem listViewItem in listViewItems)
                {
                    listView.Items.Add(listViewItem);
                    if (listView == listViewAvailableItems)
                    {
                        availableItems.Add(listViewItem.Tag);
                    }
                    else
                    {
                        associatedItems.Add(listViewItem.Tag);
                    }
                }
            }
        }
    }
}