using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Controls;
using Dile.Debug;
using Dile.Disassemble;
using Dile.UI.Debug;
using System.Collections.Generic;


namespace Dile.UI
{
	public class NuGenProjectExplorer : NuGenBasePanel
	{
		private TreeView projectElements;
		private ContextMenuStrip projectMenu;
		private ToolStripMenuItem addAssemblyMenuItem;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripMenuItem projectPropertiesMenuItem;
		private ContextMenuStrip assemblyMenu;
		private ToolStripMenuItem removeAssemblyMenuItem;
		private ContextMenuStrip assemblyReferenceMenu;
		private ToolStripMenuItem openReferenceInProjectMenuItem;
		private ToolStripMenuItem setAsStartupAssemblyMenuItem;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem reloadAssemblyMenuItem;
		private ToolStripMenuItem reloadAllAssembliesMenuItem;
		private ToolStripMenuItem assemblyPathMenuItem;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem assemblyReferencePathMenuItem;
		private ToolStripSeparator toolStripMenuItem2;
        private ImageList imageList1;
		private System.ComponentModel.IContainer components = null;

		public NuGenProjectExplorer()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
            projectElements.ImageList = this.imageList1;
		}

        public delegate void CodeDisplayerAddedDelegate(NuGenCodeEditorForm displayer);
        private CodeDisplayerAddedDelegate codeEditorAdded;
        public CodeDisplayerAddedDelegate CodeEditorAdded
        {
            set
            {
                codeEditorAdded = value;
            }

            get
            {
                return codeEditorAdded;
            }
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NuGenProjectExplorer));
            this.projectElements = new System.Windows.Forms.TreeView();
            this.assemblyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.assemblyPathMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.setAsStartupAssemblyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.reloadAssemblyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAssemblyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addAssemblyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.reloadAllAssembliesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectPropertiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assemblyReferenceMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.assemblyReferencePathMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.openReferenceInProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.assemblyMenu.SuspendLayout();
            this.projectMenu.SuspendLayout();
            this.assemblyReferenceMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // projectElements
            // 
            this.projectElements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectElements.HideSelection = false;
            this.projectElements.Location = new System.Drawing.Point(0, 0);
            this.projectElements.Name = "projectElements";
            this.projectElements.ShowNodeToolTips = true;
            this.projectElements.Size = new System.Drawing.Size(292, 273);
            this.projectElements.Sorted = true;
            this.projectElements.TabIndex = 0;
            this.projectElements.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.projectElements_NodeMouseDoubleClick);
            this.projectElements.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.projectElements_BeforeExpand);
            this.projectElements.MouseDown += new System.Windows.Forms.MouseEventHandler(this.projectElements_MouseDown);
            // 
            // assemblyMenu
            // 
            this.assemblyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assemblyPathMenuItem,
            this.toolStripMenuItem1,
            this.setAsStartupAssemblyMenuItem,
            this.toolStripSeparator2,
            this.reloadAssemblyMenuItem,
            this.removeAssemblyMenuItem});
            this.assemblyMenu.Name = "assemblyMenu";
            this.assemblyMenu.Size = new System.Drawing.Size(197, 104);
            this.assemblyMenu.Opening += new System.ComponentModel.CancelEventHandler(this.assemblyMenu_Opening);
            // 
            // assemblyPathMenuItem
            // 
            this.assemblyPathMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
            this.assemblyPathMenuItem.Name = "assemblyPathMenuItem";
            this.assemblyPathMenuItem.Size = new System.Drawing.Size(196, 22);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(193, 6);
            // 
            // setAsStartupAssemblyMenuItem
            // 
            this.setAsStartupAssemblyMenuItem.Name = "setAsStartupAssemblyMenuItem";
            this.setAsStartupAssemblyMenuItem.Size = new System.Drawing.Size(196, 22);
            this.setAsStartupAssemblyMenuItem.Text = "Set as startup assembly";
            this.setAsStartupAssemblyMenuItem.Click += new System.EventHandler(this.setAsStartupAssemblyMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(193, 6);
            // 
            // reloadAssemblyMenuItem
            // 
            this.reloadAssemblyMenuItem.Name = "reloadAssemblyMenuItem";
            this.reloadAssemblyMenuItem.Size = new System.Drawing.Size(196, 22);
            this.reloadAssemblyMenuItem.Text = "Reload assembly";
            this.reloadAssemblyMenuItem.Click += new System.EventHandler(this.reloadAssemblyMenuItem_Click);
            // 
            // removeAssemblyMenuItem
            // 
            this.removeAssemblyMenuItem.Name = "removeAssemblyMenuItem";
            this.removeAssemblyMenuItem.Size = new System.Drawing.Size(196, 22);
            this.removeAssemblyMenuItem.Text = "Remove assembly";
            this.removeAssemblyMenuItem.Click += new System.EventHandler(this.removeAssemblyMenuItem_Click);
            // 
            // projectMenu
            // 
            this.projectMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAssemblyMenuItem,
            this.toolStripSeparator1,
            this.reloadAllAssembliesMenuItem,
            this.projectPropertiesMenuItem});
            this.projectMenu.Name = "projectMenu";
            this.projectMenu.Size = new System.Drawing.Size(186, 76);
            // 
            // addAssemblyMenuItem
            // 
            this.addAssemblyMenuItem.Name = "addAssemblyMenuItem";
            this.addAssemblyMenuItem.Size = new System.Drawing.Size(185, 22);
            this.addAssemblyMenuItem.Text = "Add assembly...";
            this.addAssemblyMenuItem.Click += new System.EventHandler(this.addAssemblyMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
            // 
            // reloadAllAssembliesMenuItem
            // 
            this.reloadAllAssembliesMenuItem.Name = "reloadAllAssembliesMenuItem";
            this.reloadAllAssembliesMenuItem.Size = new System.Drawing.Size(185, 22);
            this.reloadAllAssembliesMenuItem.Text = "Reload all assemblies";
            this.reloadAllAssembliesMenuItem.Click += new System.EventHandler(this.reloadAllAssembliesMenuItem_Click);
            // 
            // projectPropertiesMenuItem
            // 
            this.projectPropertiesMenuItem.Name = "projectPropertiesMenuItem";
            this.projectPropertiesMenuItem.Size = new System.Drawing.Size(185, 22);
            this.projectPropertiesMenuItem.Text = "Properties...";
            this.projectPropertiesMenuItem.Click += new System.EventHandler(this.projectPropertiesMenuItem_Click);
            // 
            // assemblyReferenceMenu
            // 
            this.assemblyReferenceMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assemblyReferencePathMenuItem,
            this.toolStripMenuItem2,
            this.openReferenceInProjectMenuItem});
            this.assemblyReferenceMenu.Name = "assemblyReferenceMenu";
            this.assemblyReferenceMenu.Size = new System.Drawing.Size(209, 54);
            this.assemblyReferenceMenu.Opening += new System.ComponentModel.CancelEventHandler(this.assemblyReferenceMenu_Opening);
            // 
            // assemblyReferencePathMenuItem
            // 
            this.assemblyReferencePathMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
            this.assemblyReferencePathMenuItem.Name = "assemblyReferencePathMenuItem";
            this.assemblyReferencePathMenuItem.Size = new System.Drawing.Size(208, 22);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(205, 6);
            // 
            // openReferenceInProjectMenuItem
            // 
            this.openReferenceInProjectMenuItem.Name = "openReferenceInProjectMenuItem";
            this.openReferenceInProjectMenuItem.Size = new System.Drawing.Size(208, 22);
            this.openReferenceInProjectMenuItem.Text = "Open reference in project";
            this.openReferenceInProjectMenuItem.Click += new System.EventHandler(this.openReferenceInProjectMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Project.png");
            this.imageList1.Images.SetKeyName(1, "Gear.png");
            this.imageList1.Images.SetKeyName(2, "Notepage.png");
            this.imageList1.Images.SetKeyName(3, "Cabinet.png");
            this.imageList1.Images.SetKeyName(4, "Book.png");
            this.imageList1.Images.SetKeyName(5, "Document.png");
            this.imageList1.Images.SetKeyName(6, "DatabaseTable.png");
            this.imageList1.Images.SetKeyName(7, "Execute.png");
            this.imageList1.Images.SetKeyName(8, "namespace.bmp");
            this.imageList1.Images.SetKeyName(9, "Table.png");
            this.imageList1.Images.SetKeyName(10, "pubfield.gif");
            this.imageList1.Images.SetKeyName(11, "class.bmp");
            this.imageList1.Images.SetKeyName(12, "pubmethod.gif");
            this.imageList1.Images.SetKeyName(13, "pubproperty.gif");
            // 
            // NuGenProjectExplorer
            // 
            this.Controls.Add(this.projectElements);
            this.Name = "NuGenProjectExplorer";
            this.Size = new System.Drawing.Size(292, 273);
            this.assemblyMenu.ResumeLayout(false);
            this.projectMenu.ResumeLayout(false);
            this.assemblyReferenceMenu.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private bool wordWrap = true;
		public bool WordWrap
		{
			get
			{
				return wordWrap;
			}
			set
			{
				wordWrap = value;
			}
		}

		public TreeView ProjectElements
		{
			get
			{
				return projectElements;
			}
		}

		private NoArgumentsDelegate addAssemblyDelegate;
		public NoArgumentsDelegate AddAssemblyDelegate
		{
			get
			{
				return addAssemblyDelegate;
			}
			set
			{
				addAssemblyDelegate = value;
			}
		}

		private StringArrayDelegate openAssemblyReferenceDelegate;
		public StringArrayDelegate OpenAssemblyReferenceDelegate
		{
			get
			{
				return openAssemblyReferenceDelegate;
			}
			set
			{
				openAssemblyReferenceDelegate = value;
			}
		}

		private List<NuGenCodeDisplayer> codeDisplayers = new List<NuGenCodeDisplayer>();

		protected override bool IsDebugPanel()
		{
			return false;
		}

		public void RemoveUnnecessaryAssemblies()
		{
			if (ProjectElements.Nodes.Count == 1)
			{
				ProjectElements.BeginUpdate();
				int index = 0;
				TreeNodeCollection assemblyNodes = ProjectElements.Nodes[0].Nodes;

				while (index < assemblyNodes.Count)
				{
					TreeNode assemblyNode = assemblyNodes[index];
					NuGenAssembly assembly = assemblyNode.Tag as NuGenAssembly;

					if (assembly != null && !NuGenProject.Instance.Assemblies.Contains(assembly))
					{
						assemblyNode.Remove();
					}
					else
					{
						index++;
					}
				}

				ProjectElements.EndUpdate();
			}
		}

		public void ShowProject()
		{
			Text = string.Format("Project Explorer - {0}", NuGenProject.Instance.Name);
			projectElements.BeginUpdate();
			projectElements.Nodes.Clear();

			TreeNode projectNode = new TreeNode(NuGenHelperFunctions.TruncateText(NuGenProject.Instance.Name));
			projectNode.ContextMenuStrip = projectMenu;
			projectNode.Tag = NuGenProject.Instance;

			foreach (NuGenAssembly assembly in NuGenProject.Instance.Assemblies)
			{
				ShowAssembly(projectNode, assembly);
			}

			projectElements.Nodes.Add(projectNode);
			projectElements.EndUpdate();
		}

		public void AddAssemblyToProject(NuGenAssembly assembly)
		{
			projectElements.BeginUpdate();

            if (projectElements.Nodes.Count == 0)
            {
                projectElements.Nodes.Add("New Project");
            }

			ShowAssembly(projectElements.Nodes[0], assembly);
			NuGenProject.Instance.Assemblies.Add(assembly);
			if (!assembly.IsInMemory)
			{
				NuGenProject.Instance.IsSaved = false;
			}

			projectElements.EndUpdate();

            ShowProject();
		}

		private void ShowAssembly(TreeNode projectNode, NuGenAssembly assembly)
		{
			if (assembly != null)
			{
				TreeNode assemblyNode = new TreeNode(NuGenHelperFunctions.TruncateText(assembly.FileName));
				assemblyNode.ContextMenuStrip = assemblyMenu;
                assemblyNode.ImageIndex = 1;

				if (NuGenProject.Instance.StartupAssembly == assembly)
				{
					assemblyNode.ForeColor = Color.Red;
					assemblyNode.NodeFont = new Font(ProjectElements.Font, FontStyle.Italic);
				}

				assemblyNode.Tag = assembly;
				projectNode.Nodes.Add(assemblyNode);

				if (assembly.DisplayInTree)
				{
					TreeNode assemblyDefinitionNode = new TreeNode(" definition");
					assemblyDefinitionNode.Tag = assembly;
                    assemblyDefinitionNode.ImageIndex = 2;
					assemblyNode.Nodes.Add(assemblyDefinitionNode);
				}

				if (assembly.AssemblyReferences != null)
				{
					TreeNode referencesNode = new TreeNode(" References");
					referencesNode.Tag = assembly;
                    referencesNode.ImageIndex = 3;
					assemblyNode.Nodes.Add(referencesNode);

					foreach (NuGenAssemblyReference reference in assembly.AssemblyReferences.Values)
					{
						TreeNode referenceNode = new TreeNode(NuGenHelperFunctions.TruncateText(reference.Name));
						referenceNode.ContextMenuStrip = assemblyReferenceMenu;
						referenceNode.Tag = reference;
                        referenceNode.ImageIndex = 1;
						referencesNode.Nodes.Add(referenceNode);
					}
				}

				if (assembly.ManifestResources != null)
				{
					TreeNode manifestResourcesNode = new TreeNode(" Manifest Resources");
                    manifestResourcesNode.ImageIndex = 4;
					assemblyNode.Nodes.Add(manifestResourcesNode);

					foreach (NuGenManifestResource manifestResource in assembly.ManifestResources)
					{
						TreeNode manifestResourceNode = new TreeNode(NuGenHelperFunctions.TruncateText(manifestResource.Name));
						manifestResourceNode.Tag = manifestResource;
                        manifestResourceNode.ImageIndex = 1;
						manifestResourcesNode.Nodes.Add(manifestResourceNode);
					}
				}

				if (assembly.Files != null)
				{
					TreeNode filesNode = new TreeNode(" Files");
                    filesNode.ImageIndex = 5;
					assemblyNode.Nodes.Add(filesNode);

					foreach (NuGenFile file in assembly.Files)
					{
						if (file.DisplayInTree)
						{
							TreeNode fileNode = new TreeNode(NuGenHelperFunctions.TruncateText(file.Name));
							fileNode.Tag = file;
                            fileNode.Tag = 5;
							filesNode.Nodes.Add(fileNode);
						}
					}
				}

				if (assembly.ModuleReferences != null)
				{
					TreeNode moduleReferencesNode = new TreeNode(" Module References");
                    moduleReferencesNode.ImageIndex = 6;
					assemblyNode.Nodes.Add(moduleReferencesNode);

					foreach (NuGenModuleReference moduleReference in assembly.ModuleReferences)
					{
						TreeNode moduleReferenceNode = new TreeNode(NuGenHelperFunctions.TruncateText(moduleReference.Name));
						moduleReferenceNode.Tag = moduleReference;
                        moduleReferenceNode.ImageIndex = 1;
                        moduleReferencesNode.Nodes.Add(moduleReferenceNode);
					}
				}

				TreeNode moduleScopeNode = new TreeNode(NuGenHelperFunctions.TruncateText(assembly.ModuleScope.Name));
                moduleScopeNode.ImageIndex = 7;
				assemblyNode.Nodes.Add(moduleScopeNode);

				TreeNode moduleScopeDefinitionNode = new TreeNode(" definition");
                moduleScopeDefinitionNode.ImageIndex = 2;
				moduleScopeDefinitionNode.Tag = assembly.ModuleScope;
				moduleScopeNode.Nodes.Add(moduleScopeDefinitionNode);
				Dictionary<string, TreeNode> namespaceNodes = new Dictionary<string, TreeNode>();

				foreach (NuGenTypeDefinition typeDefinition in assembly.ModuleScope.TypeDefinitions.Values)
				{
					CreateTypeDefinitionNode(namespaceNodes, moduleScopeNode, typeDefinition, true);
				}

				if (assembly.GlobalType.FieldDefinitions != null || assembly.GlobalType.MethodDefinitions != null || assembly.GlobalType.Properties != null)
				{
					CreateTypeDefinitionNode(namespaceNodes, moduleScopeNode, assembly.GlobalType, false).Text = " {global type}";
				}
			}
		}

		private TreeNode CreateTypeDefinitionNode(Dictionary<string, TreeNode> namespaceNodes, TreeNode moduleScopeNode, NuGenTypeDefinition typeDefinition, bool createDefinitionNode)
		{
			TreeNode result = new TreeNode(NuGenHelperFunctions.TruncateText(typeDefinition.FullName));
            result.ImageIndex = 11;
			string typeNamespace = typeDefinition.Namespace;

			if (typeNamespace.Length == 0)
			{
				typeNamespace = NuGenConstants.DefaultNamespaceName;
			}

			if (namespaceNodes.ContainsKey(typeNamespace))
			{
				namespaceNodes[typeNamespace].Nodes.Add(result);
			}
			else
			{
				TreeNode namespaceNode = new TreeNode(typeNamespace);
                namespaceNode.ImageIndex = 8;
				moduleScopeNode.Nodes.Add(namespaceNode);
				namespaceNodes[typeNamespace] = namespaceNode;

				namespaceNode.Nodes.Add(result);
			}

			if (createDefinitionNode)
			{
				TreeNode classNode = new TreeNode("definition");
                classNode.ImageIndex = 2;
				classNode.Tag = typeDefinition;
				result.Nodes.Add(classNode);
			}
			else
			{
				CreateTypeDefinitionSubnodes(typeDefinition, result);
				result.Tag = true;
			}

			return result;
		}

		private static void CreateTypeDefinitionSubnodes(NuGenTypeDefinition typeDefinition, TreeNode typeDefinitionNode)
		{
			NuGenAssembly assembly = typeDefinition.ModuleScope.Assembly;
			typeDefinition.LazyInitialize(assembly.AllTokens);
			Dictionary<uint, NuGenMethodDefinition> methodDefinitions = null;

			if (typeDefinition.MethodDefinitions != null)
			{
				methodDefinitions = new Dictionary<uint, NuGenMethodDefinition>(typeDefinition.MethodDefinitions);
			}

			if (typeDefinition.FieldDefinitions != null)
			{
				TreeNode fieldsNode = new TreeNode("Fields");
                fieldsNode.ImageIndex = 9;
				typeDefinitionNode.Nodes.Add(fieldsNode);

				foreach (NuGenFieldDefinition field in typeDefinition.FieldDefinitions.Values)
				{
					field.LazyInitialize(assembly.AllTokens);
					TreeNode fieldNode = new TreeNode(NuGenHelperFunctions.TruncateText(field.Name));
					fieldNode.Tag = field;
                    fieldNode.ImageIndex = 10;
					fieldsNode.Nodes.Add(fieldNode);
				}
			}

			if (typeDefinition.Properties != null)
			{
				TreeNode propertiesNode = new TreeNode("Properties");
                propertiesNode.ImageIndex = 9;
				typeDefinitionNode.Nodes.Add(propertiesNode);

				foreach (NuGenProperty property in typeDefinition.Properties.Values)
				{
					property.LazyInitialize(assembly.AllTokens);
					TreeNode propertyNode = new TreeNode(NuGenHelperFunctions.TruncateText(property.Name));
                    propertyNode.ImageIndex = 11;
					propertiesNode.Nodes.Add(propertyNode);                                            

					TreeNode definitionNode = new TreeNode(" definition");
					definitionNode.Tag = property;
                    definitionNode.ImageIndex = 2;
					propertyNode.Nodes.Add(definitionNode);

					if (methodDefinitions != null)
					{
						if (methodDefinitions.ContainsKey(property.GetterMethodToken))
						{
							NuGenMethodDefinition getterMethod = methodDefinitions[property.GetterMethodToken];
							getterMethod.LazyInitialize(assembly.AllTokens);

							TreeNode getterNode = new TreeNode(NuGenHelperFunctions.TruncateText(getterMethod.DisplayName));
                            getterNode.ImageIndex = 10;
							getterNode.Tag = getterMethod;
							propertyNode.Nodes.Add(getterNode);

							methodDefinitions.Remove(property.GetterMethodToken);
						}

						if (methodDefinitions.ContainsKey(property.SetterMethodToken))
						{
							NuGenMethodDefinition setterMethod = methodDefinitions[property.SetterMethodToken];
							setterMethod.LazyInitialize(assembly.AllTokens);

							TreeNode setterNode = new TreeNode(NuGenHelperFunctions.TruncateText(setterMethod.DisplayName));
                            setterNode.ImageIndex = 10;
							setterNode.Tag = setterMethod;
							propertyNode.Nodes.Add(setterNode);

							methodDefinitions.Remove(property.SetterMethodToken);
						}

						for (int index = 0; index < property.OtherMethodsCount; index++)
						{
							uint token = property.OtherMethods[index];

							if (methodDefinitions.ContainsKey(token))
							{
								NuGenMethodDefinition otherMethod = methodDefinitions[token];
								otherMethod.LazyInitialize(assembly.AllTokens);

								TreeNode otherNode = new TreeNode(NuGenHelperFunctions.TruncateText(otherMethod.DisplayName));
                                otherNode.ImageIndex = 10;
								otherNode.Tag = otherMethod;
								propertyNode.Nodes.Add(otherNode);

								methodDefinitions.Remove(token);
							}
						}
					}
				}
			}

			if (methodDefinitions != null && methodDefinitions.Count > 0)
			{
				TreeNode methodsNode = new TreeNode("Methods");
                methodsNode.ImageIndex = 9;
				typeDefinitionNode.Nodes.Add(methodsNode);

				foreach (NuGenMethodDefinition method in methodDefinitions.Values)
				{
					method.LazyInitialize(assembly.AllTokens);
					TreeNode methodNode = new TreeNode(NuGenHelperFunctions.TruncateText(method.DisplayName));
                    methodNode.ImageIndex = 12;
					methodNode.Tag = method;
					methodsNode.Nodes.Add(methodNode);
				}
			}
		}

		private void projectElements_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node.Tag is NuGenIMultiLine)
			{
				NuGenIMultiLine codeObject = (NuGenIMultiLine)e.Node.Tag;

				ShowCodeObject(codeObject, new NuGenCodeObjectDisplayOptions());
			}
		}

		public void UpdateBreakpoint(NuGenIMultiLine codeObject, NuGenBreakpointInformation breakpointInformation)
		{
			NuGenCodeDisplayer codeDisplayer = FindCodeDisplayer(codeObject);

			if (codeDisplayer != null)
			{
				codeDisplayer.UpdateBreakpoint(breakpointInformation);
			}
		}

		public void ShowCodeObject(NuGenIMultiLine codeObject, NuGenCodeObjectDisplayOptions options)
		{
			NuGenCodeDisplayer codeDisplayer = FindCodeDisplayer(codeObject);

			if (codeDisplayer == null)
			{
				NuGenCodeEditorForm codeEditorForm = new NuGenCodeEditorForm();
                codeEditorForm.ProjectExplorer = this;
                //PETETODO:  Why does this crash?
				//codeEditorForm.UpdateFont(Settings.Instance.CodeEditorFont.Font);
				codeEditorForm.SetWordWrap(WordWrap);

                NuGenCodeDisplayer displayer = new NuGenCodeDisplayer(codeEditorAdded, codeObject, codeEditorForm);
				codeDisplayers.Add(displayer);
				displayer.ShowCodeObject(options);
                displayer.Window.ProjectExplorer = this;
			}
			else
			{
				codeDisplayer.ShowCodeObject(options);
                codeDisplayer.Window.ProjectExplorer = this;
			}
		}

		private NuGenCodeDisplayer FindCodeDisplayer(NuGenIMultiLine codeObject)
		{
			NuGenCodeDisplayer result = null;			

            foreach(NuGenCodeDisplayer displayer in codeDisplayers)
            {
                if(displayer.CodeObject == codeObject)
                {
                    result = displayer;
                    break;
                }
            }

			return result;
		}

		private void removeAssemblyMenuItem_Click(object sender, EventArgs e)
		{
			if (projectElements.SelectedNode != null && projectElements.SelectedNode.Nodes.Count > 0 && projectElements.SelectedNode.Nodes[0].Tag is NuGenAssembly)
			{
				NuGenAssembly assembly = (NuGenAssembly)projectElements.SelectedNode.Nodes[0].Tag;

				NuGenProject.Instance.RemoveAssemblyRelatedBreakpoints(assembly);
				NuGenProject.Instance.Assemblies.Remove(assembly);
				NuGenProject.Instance.IsSaved = false;
				projectElements.Nodes.Remove(projectElements.SelectedNode);
			}
		}

		private void openReferenceInProjectMenuItem_Click(object sender, EventArgs e)
		{
			if (projectElements.SelectedNode != null && projectElements.SelectedNode.Tag is NuGenAssemblyReference)
			{
				NuGenAssemblyReference assemblyReference = (NuGenAssemblyReference)projectElements.SelectedNode.Tag;

				OpenAssemblyReferenceDelegate(new string[] { assemblyReference.FullPath });
			}
		}

		private void addAssemblyMenuItem_Click(object sender, EventArgs e)
		{
			AddAssemblyDelegate();
		}

		private void projectPropertiesMenuItem_Click(object sender, EventArgs e)
		{
			NuGenProjectProperties properties = new NuGenProjectProperties();

			if (properties.DisplaySettings() == DialogResult.OK)
			{
				projectElements.Nodes[0].Text = NuGenHelperFunctions.TruncateText(NuGenProject.Instance.Name);
				NuGenUIHandler.Instance.ShowDebuggerState(DebuggerState.DebuggeeStopped);
			}
		}

		private void setAsStartupAssemblyMenuItem_Click(object sender, EventArgs e)
		{
			if (projectElements.SelectedNode != null && projectElements.SelectedNode.Nodes.Count > 0 && projectElements.SelectedNode.Nodes[0].Tag is NuGenAssembly)
			{
				NuGenAssembly assembly = (NuGenAssembly)ProjectElements.SelectedNode.Nodes[0].Tag;

				foreach (TreeNode node in ProjectElements.Nodes[0].Nodes)
				{
					node.ForeColor = SystemColors.WindowText;
					node.NodeFont = ProjectElements.Font;
				}

				NuGenProject.Instance.StartupAssembly = assembly;
				NuGenProject.Instance.IsSaved = false;
				ProjectElements.SelectedNode.ForeColor = Color.Red;
				ProjectElements.SelectedNode.NodeFont = new Font(ProjectElements.Font, FontStyle.Italic);
				NuGenUIHandler.Instance.ShowDebuggerState(DebuggerState.DebuggeeStopped);
			}

            startupAssembly = projectElements.SelectedNode.Text;
		}

        private String startupAssembly;

        public String StartupAssembly
        {
            get
            {
                return startupAssembly;
            }
        }

		public void LocateTokenNode(NuGenTokenBase tokenObject)
		{
			ProjectElements.SelectedNode = NuGenTreeViewSearcher.LocateNode(ProjectElements.Nodes[0], tokenObject);			
		}

		private void reloadAssemblyMenuItem_Click(object sender, EventArgs e)
		{
			if (ProjectElements.SelectedNode != null && ProjectElements.SelectedNode.Nodes != null && ProjectElements.SelectedNode.Nodes.Count > 0)
			{
				TreeNode assemblyNode = ProjectElements.SelectedNode.Nodes[0];
				NuGenAssembly assembly = assemblyNode.Tag as NuGenAssembly;

				if (assembly != null)
				{
					ProjectElements.SelectedNode.Remove();
					NuGenProject.Instance.Assemblies.Remove(assembly);
					NuGenUIHandler.Instance.AddAssembly(new string[] { assembly.FullPath });
				}
			}
		}

		private void reloadAllAssembliesMenuItem_Click(object sender, EventArgs e)
		{
			if (NuGenProject.Instance.Assemblies != null && NuGenProject.Instance.Assemblies.Count > 0)
			{
				string[] assemblyFileNames = new string[NuGenProject.Instance.Assemblies.Count];

				for (int index = 0; index < NuGenProject.Instance.Assemblies.Count; index++)
				{
					assemblyFileNames[index] = NuGenProject.Instance.Assemblies[index].FullPath;
				}

				ProjectElements.Nodes[0].Nodes.Clear();
				NuGenProject.Instance.Assemblies.Clear();
				NuGenUIHandler.Instance.AddAssembly(assemblyFileNames);
			}
		}

		private void assemblyMenu_Opening(object sender, CancelEventArgs e)
		{
			string assemblyPath = string.Empty;

			if (ProjectElements.SelectedNode != null && ProjectElements.SelectedNode.Nodes != null && ProjectElements.SelectedNode.Nodes.Count > 0)
			{
				NuGenAssembly assembly = ProjectElements.SelectedNode.Nodes[0].Tag as NuGenAssembly;

				if (assembly != null)
				{
					if (assembly.IsInMemory)
					{
						assemblyPath = string.Empty;
					}
					else
					{
						assemblyPath = assembly.FullPath;
					}

					foreach (ToolStripItem menuItem in assemblyMenu.Items)
					{
						if (menuItem != removeAssemblyMenuItem)
						{
							menuItem.Visible = !assembly.IsInMemory;
						}
					}
				}
			}

			assemblyPathMenuItem.Text = assemblyPath;
		}

		private void assemblyReferenceMenu_Opening(object sender, CancelEventArgs e)
		{
			string assemblyReferencePath = string.Empty;

			if (ProjectElements.SelectedNode != null)
			{
				NuGenAssemblyReference assemblyReference = ProjectElements.SelectedNode.Tag as NuGenAssemblyReference;

				if (assemblyReference != null)
				{
					assemblyReferencePath = assemblyReference.FullPath;
				}
			}

			assemblyReferencePathMenuItem.Text = assemblyReferencePath;
		}

		private void projectElements_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node != null && e.Node.Tag == null && e.Node.Nodes != null && e.Node.Nodes.Count >= 1)
			{
				NuGenTypeDefinition typeDefinition = e.Node.Nodes[0].Tag as NuGenTypeDefinition;

				if (typeDefinition != null)
				{
					CreateTypeDefinitionSubnodes(typeDefinition, e.Node);
					e.Node.Tag = true;
				}
			}
		}

		private void projectElements_MouseDown(object sender, MouseEventArgs e)
		{
            TreeNode selected = ProjectElements.GetNodeAt(e.X, e.Y);

            if (selected != null)
            {
                selected.SelectedImageIndex = selected.ImageIndex;
                ProjectElements.SelectedNode = selected;
            }
		}

        public IEnumerable<NuGenCodeDisplayer> GetCodeDisplayers()
        {
            return codeDisplayers;
        }
    }
}