/* ------------------------------------------------
 * MainForm.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

using Next2Friends.CrossPoster.Client.Properties;
using Next2Friends.CrossPoster.Client.Logic;

using gClient = Google.GData.Client;
using System.Globalization;
using System.Diagnostics;
using Next2Friends.CrossPoster.Client.Google;
using Next2Friends.CrossPoster.Client.Engines;

namespace Next2Friends.CrossPoster.Client
{
    sealed partial class MainForm : Form
    {
        private readonly String _blogListStoragePath;
        private AddBlogWizard _wizard;
        private BlogListModel _blogListModel;
        private IDictionary<BlogDescriptor, TreeNode> _descriptor2NodeMap;
        private IDictionary<BlogDescriptor, ToolStripItem> _descriptor2ItemMap;

        public MainForm()
        {
            _descriptor2NodeMap = new Dictionary<BlogDescriptor, TreeNode>();
            _descriptor2ItemMap = new Dictionary<BlogDescriptor, ToolStripItem>();

            _blogListStoragePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Genetibase\CrossPoster\BlogList.xml");

            InitializeComponent();

            _blogList.AfterSelect += _blogList_AfterSelect;
            _blogList.GotFocus += _blogList_FocusChanged;
            _blogList.LostFocus += _blogList_FocusChanged;

            _wizard = new AddBlogWizard();
            _wizard.Finished += _wizard_Finished;

            _blogListModel = new BlogListModel();

            _blogListModel.ItemAdded += _blogListModel_BlogAdded;
            _blogListModel.ItemRemoved += _blogListModel_BlogRemoved;
            _blogListModel.ItemAdded += _blogListModel_BlogCountChanged;
            _blogListModel.ItemRemoved += _blogListModel_BlogCountChanged;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _blogListModel.SaveToXml(_blogListStoragePath);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (File.Exists(_blogListStoragePath))
                _blogListModel.LoadFromXml(_blogListStoragePath);
        }

        private void UpdateItemRelatedButtonState()
        {
            _composeButton.Enabled = _removeBlogItem.Enabled = _propertiesItem.Enabled = _blogListModel.ItemCount > 0 && _blogList.SelectedNode != null;
        }

        private Int32 GetBlogListNodeImageIndex(BlogType blogType)
        {
            switch (blogType)
            {
                case BlogType.WordPress:
                    return 1;
                case BlogType.LiveJournal:
                    return 2;
            }

            return 0;
        }

        private void RefreshBlogEntries(BlogDescriptor blogDescriptor)
        {
            var blogEntries = BlogEngineFactory.GetEngine(blogDescriptor.BlogType).GetBlogEntries(blogDescriptor);
            _blogEntries.BeginUpdate();
            _blogEntries.Items.Clear();
            foreach (BlogEntryDescriptor entry in blogEntries)
                _blogEntries.Items.Add(
                    new ListViewItem(
                        new[] {
                            entry.Subject,
                            entry.Sender,
                            String.Format(CultureInfo.CurrentUICulture, "{0} {1}", entry.Date.ToShortDateString(), entry.Date.ToShortTimeString())
                        }) { Tag = entry.Content });
            _blogEntries.EndUpdate();
        }

        private void _blogListModel_BlogAdded(Object sender, ListEventArgs<BlogDescriptor> e)
        {
            var imageIndex = GetBlogListNodeImageIndex(e.Item.BlogType);
            var node = new TreeNode()
            {
                ImageIndex = imageIndex,
                SelectedImageIndex = imageIndex,
                Tag = e.Item,
                Text = e.Item.BlogName
            };

            _descriptor2NodeMap.Add(e.Item, node);
            _blogList.Nodes.Add(node);

            var item = new ToolStripMenuItem()
            {
                Image = _blogListImages.Images[GetBlogListNodeImageIndex(e.Item.BlogType)],
                Tag = e.Item,
                Text = e.Item.BlogName
            };

            _descriptor2ItemMap.Add(e.Item, item);
            _downloadButton.DropDownItems.Add(item);
        }

        private void _blogListModel_BlogRemoved(Object sender, ListEventArgs<BlogDescriptor> e)
        {
            var node = _descriptor2NodeMap[e.Item];
            _descriptor2NodeMap.Remove(e.Item);
            _blogList.Nodes.Remove(node);

            var item = _descriptor2ItemMap[e.Item];
            _descriptor2ItemMap.Remove(e.Item);
            _downloadButton.DropDownItems.Remove(item);

            UpdateItemRelatedButtonState();
        }

        private void _blogListModel_BlogCountChanged(Object sender, ListEventArgs<BlogDescriptor> e)
        {
            UpdateItemRelatedButtonState();
        }

        private void _blogList_AfterSelect(Object sender, TreeViewEventArgs e)
        {
            UpdateItemRelatedButtonState();
        }

        private void _blogList_FocusChanged(Object sender, EventArgs e)
        {
            UpdateItemRelatedButtonState();
        }

        private void _wizard_Finished(Object sender, WizardFinishedEventArgs e)
        {
            if (e.Cancelled)
                return;
            _blogListModel.AddItem(e.BlogDescriptor);
        }

        private void _addBlogItem_Click(Object sender, EventArgs e)
        {
            _wizard.AddStep(new AddBlogStepOne());
            _wizard.AddStep(new AddBlogStepTwo());
            _wizard.Start(new BlogDescriptor());
        }

        private void _removeBlogItem_Click(Object sender, EventArgs e)
        {
            if (_blogList.SelectedNode != null)
                _blogListModel.RemoveBlog((BlogDescriptor)_blogList.SelectedNode.Tag);
        }

        private void _propertiesItem_Click(Object sender, EventArgs e)
        {
            using (var propsForm = new BlogPropertiesForm())
            {
                if (propsForm.ShowDialog((BlogDescriptor)_blogList.SelectedNode.Tag) == DialogResult.OK)
                {
                }
            }
        }

        private void _composeButton_Click(Object sender, EventArgs e)
        {
            if (_blogList.SelectedNode != null)
            {
                BlogDescriptor blogDescriptor = (BlogDescriptor)_blogList.SelectedNode.Tag;

                using (var composeForm = new ComposeForm())
                {
                    if (composeForm.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(composeForm.Message))
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        BlogEngineFactory.GetEngine(blogDescriptor.BlogType).PublishNewEntry(blogDescriptor, composeForm.Title, composeForm.Message);
                        RefreshBlogEntries(blogDescriptor);
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void _downloadButton_ButtonClick(Object sender, EventArgs e)
        {
            if (_blogList.SelectedNode != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                RefreshBlogEntries((BlogDescriptor)_blogList.SelectedNode.Tag);
                Cursor.Current = Cursors.Default;
            }
        }

        private void _blogEntries_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (_blogEntries.SelectedItems != null && _blogEntries.SelectedItems.Count > 0)
            {
                var selectedItem = _blogEntries.SelectedItems[0];
                _postContentPanel.Content = (String)selectedItem.Tag;
                _postContentPanel.Subject = selectedItem.SubItems[0].Text;
                _postContentPanel.Sender = selectedItem.SubItems[1].Text;
                _postContentPanel.Date = selectedItem.SubItems[2].Text;
            }
        }
    }
}
