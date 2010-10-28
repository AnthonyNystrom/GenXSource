/***
 * 
 *  ASMEX by RiskCare Ltd.
 * 
 * This source is copyright (C) 2002 RiskCare Ltd. All rights reserved.
 * 
 * Disclaimer:
 * This code is provided 'as is', with absolutely no warranty expressed or
 * implied.  Any use of this code is at your own risk.
 *   
 * You are hereby granted the right to redistribute this source unmodified
 * in its original archive. 
 * You are hereby granted the right to use this code, or code based on it,
 * provided that you acknowledge RiskCare Ltd somewhere in the documentation
 * of your application. 
 * You are hereby granted the right to distribute changes to this source, 
 * provided that:
 * 
 * 1 -- This copyright notice is retained unchanged 
 * 2 -- Your changes are clearly marked 
 * 
 * Enjoy!
 * 
 * --------------------------------------------------------------------
 * 
 * If you use this code or have comments on it, please mail me at 
 * support@jbrowse.com or ben.peterson@riskcare.com
 * 
 */

using System;
using System.Reflection;
using System.Resources;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using Genetibase.Debug.FileViewer;

namespace Genetibase.Debug
{

	class TypeNode : Genetibase.Debug.BaseNode
	{
		public TypeNode(){}
		public TypeNode(Type type) 
		{
			Text = Type2String(type, true);
			_data = type;
			_desc = Type2String(type, false);
			int icon = IconClass;
			if (type.IsEnum) icon = IconEnum;
			if (type.IsInterface) icon = IconIface;
			if (type.IsPrimitive) icon = IconStruct;
			if (type.IsNestedAssembly || type.IsNestedFamORAssem) icon += 1;
			else if (type.IsNestedPrivate) icon += 2;
			else if (type.IsNestedFamily || type.IsNestedFamANDAssem) icon += 3;
			ImageIndex = icon;
			SelectedImageIndex = icon;
		}

		public override string NodeLabel{get{return "Type";}}

		public override object Clone()
		{
			TypeNode tn = new TypeNode((Type)this._data);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			Type type = (Type)_data;

			int root;

			int i;

			if (type.IsInterface)
			{
				Nodes.Add(new RelationshipNode(type, RelationshipNode.Kind.Implementors));
			}
			else
			{
				Nodes.Add(new RelationshipNode(type, RelationshipNode.Kind.Parent));
				Nodes.Add(new RelationshipNode(type, RelationshipNode.Kind.Children));
			}

			ConstructorInfo[] ci = type.GetConstructors(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static);

			MethodInfo[] mi_raw = type.GetMethods(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static);

			PropertyInfo[] pi = type.GetProperties(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static);

			EventInfo[] ei = type.GetEvents(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static);

			FieldInfo[] fi = type.GetFields(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static);

			Type[] ii = type.GetInterfaces();

			Type[] ni = type.GetNestedTypes(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static);

			Attribute[] ats = Attribute.GetCustomAttributes(type);

			if (ci.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Constructors:"));
				for(i=0;i<ci.Length;++i)
				{
					Nodes[root].Nodes.Add(new ConsNode(ci[i]));
				}
			}


			//remove special methods
			ArrayList mi = new ArrayList();
			for(i=0;i<mi_raw.Length; ++i)
			{
				string methname = mi_raw[i].Name;

				if (methname.IndexOf("add_") != 0 && 
					methname.IndexOf("remove_") != 0 && 
					methname.IndexOf("get_") != 0 && 
					methname.IndexOf("set_") != 0 &&
					mi_raw[i].DeclaringType == type)
				{
					mi.Add(mi_raw[i]);
				}
			}

			if (mi.Count > 0)
			{
				root = Nodes.Add(new FolderNode("Methods:"));
				for(i=0;i<mi.Count;++i)
				{
					Nodes[root].Nodes.Add(new MethodNode((MethodInfo)mi[i]));
				}
			}

			if (pi.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Properties:"));
				for(i=0;i<pi.Length;++i)
				{
					if (pi[i].DeclaringType == type)
					{
						Nodes[root].Nodes.Add(new PropNode(pi[i]));
					}
				}
			}

			if (ei.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Events:"));
				for(i=0;i<ei.Length;++i)
				{
					if (ei[i].DeclaringType == type)
					{
						Nodes[root].Nodes.Add(new EventNode(ei[i]));
					}
				}
			}

			if (fi.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Fields:"));
				for(i=0;i<fi.Length;++i)
				{
					if (fi[i].DeclaringType == type)
					{
						Nodes[root].Nodes.Add(new FieldNode(fi[i]));
					}
				}
			}

			if (ii.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Interfaces:"));
				for(i=0;i<ii.Length;++i)
				{
					Nodes[root].Nodes.Add(new TypeNode(ii[i]));
				}
			}

			if (ats.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Attributes:"));
				for(i=0;i<ats.Length;++i)
				{
					Nodes[root].Nodes.Add(new ObjNode(ats[i], false));
				}
		
			}

			if (ni.Length > 0)
			{
				for(i=0;i<ni.Length;++i)
				{
					Nodes.Add(new TypeNode(ni[i]));
				}
			}
			

		}

		public override MenuItem[] GetMenu()
		{
			MenuItem[] items = new MenuItem[3];

			items[0] = new MenuItem("Create Instance", new EventHandler(this.OnMenu));
			items[1] = new MenuItem("Show Form", new EventHandler(this.OnMenu2));
			items[2] = new MenuItem("Disassembly", new EventHandler(this.OnMenu3));

			return items;
		}

		public void OnMenu3(object sender, EventArgs e)
		{
			Type t = (Type)_data;

			string modName = t.Assembly.GetModules()[0].FullyQualifiedName;

			Process p = new Process();
            p.StartInfo.FileName = Application.StartupPath + "/ildasm.exe";
			p.StartInfo.Arguments = "/text /nobar \"" + modName + "\" /item=" + t.FullName;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			p.Start();

			string s  =p.StandardOutput.ReadToEnd();

			p.WaitForExit();
			p.Close();

			InfoDialog dlg = new InfoDialog("Disassembly", "Type " + t.FullName + " in " + modName, s);
			dlg.ShowDialog();
		}

		public void OnMenu2(object sender, EventArgs e)
		{
			Type type = (Type)_data;
			object o;

			try
			{
				Assembly asm = type.Assembly;
				o = asm.CreateInstance(type.FullName, true);

				((Form)o).Show();
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message, "Unable to create instance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
				return;
			}

			MessageBox.Show("Instance Created: " + o.ToString(), "Success!");
		}

		public void OnMenu(object sender, EventArgs e)
		{
			Type type = (Type)_data;
			object o;

			try
			{
				Assembly asm = type.Assembly;
				o = asm.CreateInstance(type.FullName, true);
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message, "Unable to create instance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
				return;
			}

			MessageBox.Show("Instance Created: " + o.ToString(), "Success!");
		}

		
	}

	class RelationshipNode : Genetibase.Debug.BaseNode
	{
		public enum Kind{Parent, Children, Implementors};

		Kind _kind;

		public RelationshipNode(){}
		public RelationshipNode(Type type, Kind kind) 
		{
			_kind = kind;
			Text = KindName;
			_data = type;
			_desc = KindName + " of " + type.FullName;
			int icon = IconClass;
			if (_kind == Kind.Parent) icon = IconParent;
			else icon = IconChild;
			ImageIndex = icon;
			SelectedImageIndex = icon;
		}

		public override string NodeLabel{get{return "Relationship";}}

		public override bool Uninteresting{get{return true;}}

		public override object Clone()
		{
			RelationshipNode tn = new RelationshipNode((Type)this._data, this._kind);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			Type type = (Type)_data;

			switch(_kind)
			{
				case Kind.Parent:
				{
					Type t = type.BaseType;
					if (t != null)
					{
						Nodes.Add(new TypeNode(t));
					}
					break;
				}
				case Kind.Children:
				{
					Assembly asm = type.Assembly;
					Type[] othertypes = asm.GetTypes();

					for(int i=0;i<othertypes.Length;++i)
					{
						if (type.Equals(othertypes[i].BaseType))
						{
							Nodes.Add(new TypeNode(othertypes[i]));
						}
					}
					break;
				}
				case Kind.Implementors:
				{
					Assembly asm = type.Assembly;
					Type[] othertypes = asm.GetTypes();

					for(int i=0;i<othertypes.Length;++i)
					{
						Type[] ifaces = othertypes[i].GetInterfaces();

						for(int j=0; j < ifaces.Length; ++j)
						{
							if (ifaces[j].Equals(type))
								Nodes.Add(new TypeNode(othertypes[i]));
						}
					}
					break;
				}
			}
		}

		public string KindName
		{
			get
			{
				switch(_kind)
				{
					case Kind.Parent: return "Parent";
					case Kind.Children: return "Children";
				}
				return "Implementors";
			}
		}
	}


	class NamespaceNode : Genetibase.Debug.BaseNode
	{
		public NamespaceNode(){}
		public NamespaceNode(Assembly asm, string ns)
		{
			Text = ns;
			_data = asm;
			_desc = "Namespace " + ns + " in assembly " + asm.FullName;
			SelectedImageIndex = IconNamespace;
			ImageIndex = IconNamespace;
		}

		//public override bool CanBeRoot{get{return true;}}

		public override object Clone()
		{
			NamespaceNode nn = new NamespaceNode((Assembly)this._data, Text);
			return nn;
		}

		public override bool Uninteresting{get{return true;}}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			int i;

			Assembly asm = (Assembly)_data;

			Type[] types = asm.GetTypes();
			for(i=0;i<types.Length;++i)
			{
				if (types[i].IsNestedAssembly || types[i].IsNestedPublic || types[i].IsNestedPrivate)
				{
					//ignore it -- it will be shown under its nestee
				}
				else
				{
					if (types[i].Namespace == Text)
					{
						Nodes.Add(new TypeNode(types[i]));
					}

					if (types[i].Namespace == null && (Text == ""))
					{
						Nodes.Add(new TypeNode(types[i]));
					}
				}
			}

		}
	}


	class AsmNode : Genetibase.Debug.BaseNode
	{
		public AsmNode(){}
		public AsmNode(Assembly asm)
		{
			Text = Assembly2String(asm, true);
			_data = asm;
			_desc = Assembly2String(asm, false);
			SelectedImageIndex = IconAssembly;
			ImageIndex = IconAssembly;
		}

		public override bool CanBeRoot{get{return true;}}

		public override string NodeLabel{get{return "Assembly";}}

		public override object Clone()
		{
			AsmNode tn = new AsmNode((Assembly)this._data);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			int i, root;

			Assembly asm = (Assembly)_data;

			//we need to know all the type names so we can generate namespace nodes.

			Type[] types = asm.GetTypes();
			if (types.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Types:"));
				NamespaceMaker ns = new NamespaceMaker(types);
				for(i=0;i<ns.Count;++i)
				{
					Nodes[root].Nodes.Add(new NamespaceNode(asm, ns[i]));
				}
			}

			string[] mrns = asm.GetManifestResourceNames();
			if (mrns.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Resources:"));
				for(i=0;i<mrns.Length;++i)
				{
					Nodes[root].Nodes.Add(new ManResNode(new AManifestResource(asm.GetManifestResourceInfo(mrns[i]), asm, mrns[i])));
				}
			}


			Attribute[] ats = Attribute.GetCustomAttributes(asm);//.GetCustomAttributes(false);
			if (ats.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Attributes:"));
				for(i=0;i<ats.Length;++i)
				{
					//ats[i].
					Nodes[root].Nodes.Add(new ObjNode(ats[i], false));
				}
			}

			AssemblyName[] an = asm.GetReferencedAssemblies();
			if (an.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Referenced Assemblies:"));
				for(i=0;i<an.Length;++i)
				{
					Nodes[root].Nodes.Add(new AsmRefNode(an[i]));
				}
			}


			FileStream[] fs = asm.GetFiles(true);
			if (fs.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Files:"));	
				for(i=0;i<fs.Length;++i)
				{
					Nodes[root].Nodes.Add(new FileNode(new AFile(fs[i])));
					fs[i].Close();
				}
			}


			Module[] mi = asm.GetModules(true);
			if (mi.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Modules:"));
				for(i=0;i<mi.Length;++i)
				{
					Nodes[root].Nodes.Add(new ModuleNode(new AModule(mi[i])));
				}
			}

		}

		public override MenuItem[] GetMenu()
		{
			MenuItem[] items = new MenuItem[1];

			items[0] = new MenuItem("Run Assembly", new EventHandler(this.OnMenu));

			return items;
		}

		public void OnMenu(object sender, EventArgs e)
		{
			Assembly asm = (Assembly)_data;

			AppDomain dom = System.AppDomain.CreateDomain("hoopoe");

			int i;

			try
			{
				i = dom.ExecuteAssembly(asm.GetFiles()[0].Name);
			}
			catch(Exception ee)
			{
				MessageBox.Show("Exception: " + ee.Message, "Failure!");
				return;
			}

			MessageBox.Show("Assembly Executed: " + i.ToString(), "Success!");
		}
	}

	class AsmRefNode : Genetibase.Debug.BaseNode
	{
		public AsmRefNode(){}
		public AsmRefNode(AssemblyName an)
		{
			Text = AsmName2String(an, true);
			_data = an;
			_desc = AsmName2String(an, false);
			SelectedImageIndex = IconAssemblyRef;
			ImageIndex = IconAssemblyRef;
		}

		public override string NodeLabel{get{return "Assembly Reference";}}

		public override object Clone()
		{
			AsmRefNode tn = new AsmRefNode((AssemblyName)this._data);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			AssemblyName an = (AssemblyName)_data;

			Assembly asm = GetAssemblyByName(an);

			if (asm == null) return;

			AssemblyName[] ai = asm.GetReferencedAssemblies();

			for(int i=0; i < ai.Length; ++i)
			{
				Nodes.Add(new AsmRefNode(ai[i]));
			}
		}

		public override MenuItem[] GetMenu()
		{
			MenuItem[] items = new MenuItem[1];

			items[0] = new MenuItem("Open Assembly", new EventHandler(this.OnMenu));

			return items;
		}

		public void OnMenu(object sender, EventArgs e)
		{
			AssemblyName an = (AssemblyName)_data;

			AsmNode nod = new AsmNode(GetAssemblyByName(an));

			this.TreeView.Nodes.Add(nod);
		}

		//If we can't load an assembly directly, maybe it's in the path
		private Assembly GetAssemblyByName(AssemblyName an)
		{
			try
			{
				Assembly asm = Assembly.Load(an);
				return asm;
			}
			catch{}

			//may I please point out that my actual finished code does not contain things like this.
			//In real life, I would have found some elegant way to make services such as 'finding an assembly in a given path'
			//available to all objects that need them without having to cast up the GUI hierarchy.
			//Ironically, as this code is free there is a high chance that someone will notice my ugly kludge, whereas in the
			//code I'm PAID to write, I could easily get away with this kind of structure if I wanted to.
			//I'm sure there is some sort of commentary about the whole Open Source thing in there, but I don't know what it is.
			MainFrame fr = (MainFrame)this.TreeView.Parent.Parent.Parent;

			ArrayList arr = fr.GetPathAssemblies();

			for(int i=0;i<arr.Count; ++i)
			{
				AssemblyName a = ((Assembly)arr[i]).GetName(); //have to compare strings as Equals() does not work(?)

				if (a.ToString() == an.ToString())
				{
					return (Assembly)arr[i];
				}
			}

			return null;
		}

	}

	class ModuleNode : Genetibase.Debug.BaseNode
	{
		public ModuleNode(){}
		public ModuleNode(AModule m)
		{
			Text = m.ToString();
			_data = m;
			_desc = m.ToString();
			SelectedImageIndex = IconModule;
			ImageIndex = IconModule;
		}

		public override string NodeLabel{get{return "Module";}}

		public override bool CanBeRoot{get{return true;}}

		public override object Clone()
		{
			ModuleNode tn = new ModuleNode((AModule)this._data);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			AModule m = (AModule)_data;

			Genetibase.Debug.FileViewer.MModule mod = null;

			BinaryReader r;

			try
			{
				FileStream stream = new FileStream(m.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				r = new BinaryReader(stream);
			}
			catch//(Exception e)
			{
//				Nodes.Add(new ErrorNode("Can't open this file:  " + e.Message));
				return;
			}

			mod = new Genetibase.Debug.FileViewer.MModule(r);

			Assembly asm = m.Assembly;

			if (asm != null)
			{
				Nodes.Add(new AsmNode(asm));
			}

			//the purpose of this 'try' is to allow us to add as many nodes as the MModule might happen to contain
			try
			{
				int root = Nodes.Add(new FolderNode("Headers"));

				Nodes[root].Nodes.Add(new FileRegionNode(mod.ModHeaders.OSHeaders));

				if(mod.ModHeaders.COR20Header != null)
				{
					Nodes[root].Nodes.Add(new FileRegionNode(mod.ModHeaders.COR20Header));
					Nodes[root].Nodes.Add(new FileRegionNode(mod.ModHeaders.MetaDataHeaders));
					Nodes[root].Nodes.Add(new FileRegionNode(mod.ModHeaders.MetaDataTableHeader));
				}

				root = Nodes.Add(new FolderNode("Imports/Exports"));
				if (mod.ImportExport.ExportDirectoryTable != null)
					Nodes[root].Nodes.Add(new FileRegionNode(mod.ImportExport.ExportDirectoryTable));

				if (mod.ImportExport.Exports.Length > 0)
				{

					int n = Nodes[root].Nodes.Add(new FolderNode("Exports"));
					
					for(int i=0;i< mod.ImportExport.Exports.Length; ++i)
					{
						Nodes[root].Nodes[n].Nodes.Add(new ObjNode(mod.ImportExport.Exports[i], false));
					}
				}

				if (mod.ImportExport.ImportDirectoryEntries.Length > 0)
				{
					int n = Nodes[root].Nodes.Add(new FolderNode("Imports"));

					for(int i=0;i< mod.ImportExport.ImportDirectoryEntries.Length; ++i)
					{
						Nodes[root].Nodes[n].Nodes.Add(new FileRegionNode(mod.ImportExport.ImportDirectoryEntries[i]));
					}
				}


				root = Nodes.Add(new FolderNode("Relocations"));
				for(int i=0; i < mod.Relocations.Blocks.Length; ++i)	
				{
					Nodes[root].Nodes.Add(new FileRegionNode(mod.Relocations.Blocks[i]));
				}


				root = Nodes.Add(new FolderNode("Heaps"));
				Nodes[root].Nodes.Add(new MDHeapNode(mod.StringHeap));
				Nodes[root].Nodes.Add(new MDHeapNode(mod.BlobHeap));
				Nodes[root].Nodes.Add(new MDHeapNode(mod.GUIDHeap));
				if (mod.USHeap != null)
					Nodes[root].Nodes.Add(new MDHeapNode(mod.USHeap));

				root = Nodes.Add(new FolderNode("Tables"));
				for(int i=0; i< mod.MDTables.Tables.Length; ++i)
				{
					Nodes[root].Nodes.Add(new TableNode(mod.MDTables.Tables[i]));
				}
			}
			catch//(Exception e) 
			{
				Nodes.Add(new ErrorNode("Not all parts of this module could be read -- is it a valid .NET module?"));
			}

		}

		public override MenuItem[] GetMenu()
		{
			MenuItem[] items = new MenuItem[1];

			items[0] = new MenuItem("Run ILDASM", new EventHandler(this.OnMenu));

			return items;
		}

		public void OnMenu(object sender, EventArgs e)
		{
			Module m = (Module)_data;

			Process p = new Process();
			p.StartInfo.FileName = "ildasm.exe";
			p.StartInfo.Arguments = "/text /nobar " + m.FullyQualifiedName;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			p.Start();

			string s  =p.StandardOutput.ReadToEnd();

			p.WaitForExit();
			p.Close();

			InfoDialog dlg = new InfoDialog("Disassembly", "Module " + m.FullyQualifiedName, s);
			dlg.ShowDialog();
		}
	}



	class FileRegionNode : Genetibase.Debug.BaseNode
	{
		public FileRegionNode(){}
		public FileRegionNode(Region r)
		{
			//paranoia... we should never try to make a node with a null datum, but in case FileViewer
			//proves hard to handle...
			if (r == null)
			{
				_data = null;
				Text = _desc = "Region not found in module.";
				SelectedImageIndex = IconError;
				ImageIndex = IconError;
				return;
			}

			_data = r;
			Text = r.ToString();
			_desc = r.ToString();
			SelectedImageIndex = IconHeader;
			ImageIndex = IconHeader;
		}

		public override string NodeLabel{get{return "Binary Data In File";}}

		public override object Clone()
		{
			FileRegionNode tn = new FileRegionNode((Genetibase.Debug.FileViewer.Region)this._data);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			if (_data is OSHeaders)
			{
				OSHeaders h = (OSHeaders)_data;
				Nodes.Add(new FileRegionNode(h.DOSStub));
				Nodes.Add(new FileRegionNode(h.PEHeader));
				for(int i=0;i<h.SectionHeaders.Length;++i)
				{
					Nodes.Add(new FileRegionNode(h.SectionHeaders[i]));
				}
			}
			else if (_data is COR20Header)
			{
				COR20Header h = (COR20Header)_data;
				Nodes.Add(new FileRegionNode(h.MetaData));
				Nodes.Add(new FileRegionNode(h.Resources));
				Nodes.Add(new FileRegionNode(h.StrongNameSignature));
				Nodes.Add(new FileRegionNode(h.CodeManagerTable));
				Nodes.Add(new FileRegionNode(h.VTableFixups));
				Nodes.Add(new FileRegionNode(h.ExportAddressTableJumps));
			}
			else if (_data is MetaDataHeaders)
			{
				MetaDataHeaders h  = (MetaDataHeaders)_data;
				Nodes.Add(new FileRegionNode(h.StorageSigAndHeader));
				Nodes.Add(new FileRegionNode(h.BlobStreamHeader));
				Nodes.Add(new FileRegionNode(h.GUIDStreamHeader));
				Nodes.Add(new FileRegionNode(h.StringStreamHeader));
				if (h.USStreamHeader != null)
					Nodes.Add(new FileRegionNode(h.USStreamHeader));
				Nodes.Add(new FileRegionNode(h.TableStreamHeader));
			}
			else if (_data is PEHeader)
			{
				PEHeader h = (PEHeader)_data;
				for(int i=0; i < h.DataDirs.Length; ++i)
				{
					Nodes.Add(new FileRegionNode(h.DataDirs[i]));
				}
			}
			else if (_data is MetaDataTableHeader)
			{
				MetaDataTableHeader h = (MetaDataTableHeader)_data;
				for(int i=0; i < h.TableLengths.Length; ++i)
				{
					Nodes.Add(new ObjNode(h.TableLengths[i], false));
				}
			}
			else if (_data is RelocationBlock)
			{
				RelocationBlock rb = (RelocationBlock)_data;
				for(int i=0; i < rb.Relocations.Length; ++i)
				{
					Nodes.Add(new ObjNode(rb.Relocations[i], false));
				}	
			}
			else if (_data is ImportDirectoryEntry)
			{
				ImportDirectoryEntry rb = (ImportDirectoryEntry)_data;
				for(int i=0; i < rb.ImportLookupTable.Length; ++i)
				{
					Nodes.Add(new ObjNode(rb.ImportLookupTable[i], false));
				}	
			}

		}
	}


	class TableNode : Genetibase.Debug.BaseNode
	{
		bool _bRaw;

		public TableNode(){}
		public TableNode(Genetibase.Debug.FileViewer.Table t)
		{
			_bRaw = false;
			_data = t;
			Text = t.ToString();
			_desc = t.ToString();
			SelectedImageIndex = IconTable;
			ImageIndex = IconTable;
		}

		public override string NodeLabel{get{return "Metadata Table";}}

		public override object Clone()
		{
			TableNode tn = new TableNode((Genetibase.Debug.FileViewer.Table)this._data);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			Table t = (Table)_data;

			for(int i=0; i < t.Count; ++i)
			{
				Nodes.Add(new RowNode(t[i], i, _bRaw));
			}
		}

		public override MenuItem[] GetMenu()
		{
			MenuItem[] items = new MenuItem[1];

			if (_bRaw)
			{
				items[0] = new MenuItem("Show Processed Data", new EventHandler(this.OnMenu));
			}
			else
			{
				items[0] = new MenuItem("Show Raw Data", new EventHandler(this.OnMenu));
			}


			return items;
		}

		public void OnMenu(object sender, EventArgs e)
		{
			_bRaw = !_bRaw;
			
			TreeView.BeginUpdate();
			GenerateChildren();
			TreeView.EndUpdate();

		}
	}

	class RowNode : Genetibase.Debug.BaseNode
	{
		int _idx;
		bool _bRaw;
		public RowNode(){}
		public RowNode(Genetibase.Debug.FileViewer.Row r, int i, bool bRaw)
		{
			_idx = i;
			_bRaw = bRaw;
			_data = r;
			if (_bRaw)
			{
				Text = i.ToString("X8") + "        " + r.RawString();
				_desc = "Row " + i.ToString("X8") + ": " + r.RawString();
			}
			else
			{
				Text = i.ToString("D8") + "        " + r.CookedString();
				_desc = "Row " + i.ToString("X8") + " (" + i + ") : " + r.CookedString();
			}
			SelectedImageIndex = IconData;
			ImageIndex = IconData;
		}

		public override string NodeLabel{get{return "Metadata Table Row";}}

		public override object Clone()
		{
			RowNode tn = new RowNode((Genetibase.Debug.FileViewer.Row)this._data, _idx, _bRaw);
			return tn;
		}
	}

	class MDHeapNode : Genetibase.Debug.BaseNode
	{
		public MDHeapNode(){}
		public MDHeapNode(Genetibase.Debug.FileViewer.MDHeap h)
		{
			_data = h;
			Text = h.ToString();
			_desc = h.ToString() + " " + h.Count;
			SelectedImageIndex = IconHeader;
			ImageIndex = IconHeader;
		}

		public override string NodeLabel{get{return "Metadata Heap";}}

		public override object Clone()
		{
			MDHeapNode tn = new MDHeapNode((Genetibase.Debug.FileViewer.MDHeap)this._data);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			MDHeap mdh = (MDHeap)_data;

			for(int i=0;i<mdh.Count;++i)
			{
				Nodes.Add(new ObjNode(mdh[i], false));
			}
		}


	}

	class FileNode : Genetibase.Debug.BaseNode
	{
		public FileNode(){}
		public FileNode(AFile f)
		{
			Text = f.Name;
			_data = f;
			_desc = f.Name;
			SelectedImageIndex = IconManRes;
			ImageIndex = IconManRes;
		}

		public override string NodeLabel{get{return "File";}}

		public override object Clone()
		{
			FileNode tn = new FileNode((AFile)this._data);
			return tn;
		}
	}

	class ConsNode : Genetibase.Debug.BaseNode
	{
		public ConsNode(){}
		public ConsNode(ConstructorInfo ci)
		{
			Text = Method2String(ci, true);
			_data = ci;
			_desc = Method2String(ci, false);
			int icon = IconMethod;
			if (ci.IsPrivate) icon += 2;
			else if (ci.IsFamily) icon += 1;
			else if (!ci.IsPublic) icon += 3;
			SelectedImageIndex = icon;
			ImageIndex = icon;
		}

		public override string NodeLabel{get{return "Constructor";}}

		public override object Clone()
		{
			ConsNode tn = new ConsNode((ConstructorInfo)this._data);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			int i, root;

			ConstructorInfo ci = (ConstructorInfo)_data;


			ParameterInfo[] pi = ci.GetParameters();
			if (pi.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Parameters:"));
				for(i=0;i<pi.Length;++i)
				{
					Nodes[root].Nodes.Add(new ParamNode(pi[i]));
				}
			}


			Attribute[] ats = Attribute.GetCustomAttributes(ci);//.GetCustomAttributes(false);
			if (ats.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Attributes:"));
				for(i=0;i<ats.Length;++i)
				{
					//ats[i].
					Nodes[root].Nodes.Add(new ObjNode(ats[i], false));
				}
			}
		}

		public override MenuItem[] GetMenu()
		{
			MenuItem[] items = new MenuItem[1];

			items[0] = new MenuItem("Run ILDASM", new EventHandler(this.OnMenu));

			return items;
		}

		public void OnMenu(object sender, EventArgs e)
		{
			MethodBase m = (MethodBase)_data;

			string modName = m.DeclaringType.Assembly.GetModules()[0].FullyQualifiedName;
			string methName = m.DeclaringType.FullName + "::" + m.Name;

			Process p = new Process();
			p.StartInfo.FileName = "ildasm.exe";
			p.StartInfo.Arguments = "/text /nobar \"" + modName + "\" /item=" + methName;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			p.Start();

			string s  =p.StandardOutput.ReadToEnd();

			p.WaitForExit();
			p.Close();

			InfoDialog dlg = new InfoDialog("Disassembly", "Constructor " + methName + " in " + modName, s);
			dlg.ShowDialog();
		}
	}

	class MethodNode : Genetibase.Debug.BaseNode
	{
		public MethodNode(){}
		public MethodNode(MethodInfo mi)
		{
			Text = Method2String(mi, true);
			_data = mi;
			_desc = Method2String(mi, false);
			int icon = IconMethod;
			if (mi.IsPrivate) icon += 2;
			else if (mi.IsFamily) icon += 1;
			else if (!mi.IsPublic) icon += 3;
			if (mi.IsStatic) icon += 4;
			SelectedImageIndex = icon;
			ImageIndex = icon;
		}

		public override string NodeLabel{get{return "Method";}}

		public override string MatchingString{get{return ((MethodInfo)_data).Name;}}

		public override object Clone()
		{
			MethodNode tn = new MethodNode((MethodInfo)this._data);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			int i, root;

			MethodInfo mi = (MethodInfo)_data;

			if (mi.ReturnType != typeof(void))
			{
				root = Nodes.Add(new FolderNode("Return:"));
				Nodes[root].Nodes.Add(new TypeNode(mi.ReturnType));
			}

			ParameterInfo[] pi = mi.GetParameters();
			if (pi.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Parameters:"));
				for(i=0;i<pi.Length;++i)
				{
					Nodes[root].Nodes.Add(new ParamNode(pi[i]));
				}
			}


			Attribute[] ats = Attribute.GetCustomAttributes(mi);//.GetCustomAttributes(false);
			if (ats.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Attributes:"));
				for(i=0;i<ats.Length;++i)
				{
					//ats[i].
					Nodes[root].Nodes.Add(new ObjNode(ats[i], false));
				}
			}
		}

		public override MenuItem[] GetMenu()
		{
			MenuItem[] items = new MenuItem[1];

			items[0] = new MenuItem("Run ILDASM", new EventHandler(this.OnMenu));

			return items;
		}

		public void OnMenu(object sender, EventArgs e)
		{
			MethodBase m = (MethodBase)_data;

			string modName = m.DeclaringType.Assembly.GetModules()[0].FullyQualifiedName;
			string methName = m.DeclaringType.FullName + "::" + m.Name;

			Process p = new Process();
			p.StartInfo.FileName = "ildasm.exe";
			p.StartInfo.Arguments = "/text /nobar \"" + modName + "\" /item=" + methName;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			p.Start();

			string s  =p.StandardOutput.ReadToEnd();

			p.WaitForExit();
			p.Close();

			InfoDialog dlg = new InfoDialog("Disassembly", "Method " + methName + " in " + modName, s);
			dlg.ShowDialog();
		}
	}

	class ParamNode : Genetibase.Debug.BaseNode
	{
		public ParamNode(){}
		public ParamNode(ParameterInfo pi)
		{
			Text = Param2String(pi, true);
			_data = pi;
			_desc = Param2String(pi, false);
			SelectedImageIndex = IconParam;
			ImageIndex = IconParam;
		}

		public override string NodeLabel{get{return "Parameter";}}

		public override string MatchingString{get{return ((ParameterInfo)_data).Name;}}

		public override object Clone()
		{
			ParamNode tn = new ParamNode((ParameterInfo)this._data);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			int i, root;

			ParameterInfo pi = (ParameterInfo)_data;


			root = Nodes.Add(new FolderNode("Parameter Type:"));
			Nodes[root].Nodes.Add(new TypeNode(pi.ParameterType));

			Attribute[] ats = Attribute.GetCustomAttributes(pi);
			if (ats.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Attributes:"));
				for(i=0;i<ats.Length;++i)
				{
					Nodes[root].Nodes.Add(new ObjNode(ats[i], false));
				}
			}
		}
	}

	class PropNode : Genetibase.Debug.BaseNode
	{
		public PropNode(){}
		public PropNode(PropertyInfo pi)
		{
			Text = Property2String(pi, true);
			_data = pi;
			_desc = Property2String(pi, false);
			int icon = IconProp;
			if (pi.CanRead && !pi.CanWrite) icon = IconPropRO; 
			if (!pi.CanRead && pi.CanWrite) icon = IconPropWO; 
			SelectedImageIndex = icon;
			ImageIndex = icon;
		}

		public override string NodeLabel{get{return "Property";}}

		public override string MatchingString{get{return ((PropertyInfo)_data).Name;}}

		public override object Clone()
		{
			PropNode tn = new PropNode((PropertyInfo)this._data);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			int i, root;

			PropertyInfo pi = (PropertyInfo)_data;


			root = Nodes.Add(new FolderNode("Property Type:"));
			Nodes[root].Nodes.Add(new TypeNode(pi.PropertyType));

			Attribute[] ats = Attribute.GetCustomAttributes(pi);
			if (ats.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Attributes:"));
				for(i=0;i<ats.Length;++i)
				{
					//ats[i].
					Nodes[root].Nodes.Add(new ObjNode(ats[i], false));
				}
			}
		}
	}

	class EventNode : Genetibase.Debug.BaseNode
	{
		public EventNode(){}
		public EventNode(EventInfo ei)
		{
			Text = Event2String(ei, true);
			_data = ei;
			_desc = Event2String(ei, false);
			SelectedImageIndex = IconEvent;
			ImageIndex = IconEvent;
		}

		public override string NodeLabel{get{return "Event";}}

		public override string MatchingString{get{return ((EventInfo)_data).Name;}}

		public override object Clone()
		{
			EventNode tn = new EventNode((EventInfo)this._data);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			int i, root;

			EventInfo ei = (EventInfo)_data;

			root = Nodes.Add(new FolderNode("Event Type:"));
			Nodes[root].Nodes.Add(new TypeNode(ei.EventHandlerType));

			Attribute[] ats = Attribute.GetCustomAttributes(ei);
			if (ats.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Attributes:"));
				for(i=0;i<ats.Length;++i)
				{
					//ats[i].
					Nodes[root].Nodes.Add(new ObjNode(ats[i], false));
				}
			}
		}
	}

	class FieldNode : Genetibase.Debug.BaseNode
	{
		public FieldNode(){}
		public FieldNode(FieldInfo fi)
		{
			Text = Field2String(fi, true);
			_data = fi;
			_desc = Field2String(fi, false);
			int icon = IconField;
			if (fi.IsLiteral) icon = IconConst;
			else if (fi.IsStatic) icon += 4;
			if (fi.IsPrivate) icon += 2;
			else if (fi.IsFamily) icon += 1;
			else if (!fi.IsPublic) icon += 3;
			SelectedImageIndex = icon;
			ImageIndex = icon;
		}

		public override string NodeLabel{get{return "Field";}}

		public override object Clone()
		{
			FieldNode tn = new FieldNode((FieldInfo)this._data);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			int i, root;

			FieldInfo fi = (FieldInfo)_data;


			root = Nodes.Add(new FolderNode("Field Type:"));
			Nodes[root].Nodes.Add(new TypeNode(fi.FieldType));

			Attribute[] ats = Attribute.GetCustomAttributes(fi);
			if (ats.Length > 0)
			{
				root = Nodes.Add(new FolderNode("Attributes:"));
				for(i=0;i<ats.Length;++i)
				{
					//ats[i].
					Nodes[root].Nodes.Add(new ObjNode(ats[i], false));
				}
			}
		}
	}

	class ErrorNode : Genetibase.Debug.BaseNode
	{
		public ErrorNode(){}
		public ErrorNode(string s)
		{
			Text = s;
			_data = s;
			_desc = s;
			SelectedImageIndex = IconError;
			ImageIndex = IconError;
		}

		public override string NodeLabel{get{return "Error";}}

		public override object Clone()
		{
			ErrorNode tn = new ErrorNode((string)this._data);
			return tn;
		}
	}

	class ObjNode : Genetibase.Debug.BaseNode
	{
		bool _bExpandable;

		public ObjNode(){}
		public ObjNode(object o, bool bExpandable)
		{
			Text = Object2String(o, true);
			_data = o;
			_desc = Object2String(o, false);
			SelectedImageIndex = IconStruct;
			ImageIndex = IconStruct;
			_bExpandable = bExpandable;
		}

		public override string NodeLabel{get{return "Object Instance";}}

		public override object Clone()
		{
			ObjNode tn = new ObjNode(this._data, this._bExpandable);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			if (!_bExpandable) return;

			Type t = _data.GetType();
			PropertyInfo[] pia = t.GetProperties();

			for(int i=0; i<pia.Length;++i)
			{
				PropertyInfo pi = pia[i];
				MethodInfo mi = null;
				object ob;

				try
				{
					mi = t.GetMethod("get_" + pi.Name);
				}
				catch{}

				if (mi == null)
					continue;

				if (mi.IsStatic)
					continue;
				
				if (pi.Name == "Item")
				{
					for (int j=0; j < 10; ++j)
					{
						try
						{
							ob = pi.GetValue(_data, new object[1] {j});
						}
						catch//(Exception ee)
						{
							break;
						}
					
						Nodes.Add(new ObjNode(ob, true));
					}
				}
				else
				{
					try
					{
						ob = pi.GetValue(_data, new object[] {});
					}
					catch(Exception ee)
					{
						ob = ee;
					}

					Nodes.Add(new ObjNode(ob, true));
				}
			}

		}
	}

	class ManResNode : Genetibase.Debug.BaseNode
	{
		public ManResNode(){}
		public ManResNode(AManifestResource amr)
		{
			int icon;

			if (amr.IsResx)
			{
				icon = IconManResx;
			}
			else
			{
				icon = IconManRes;
			}

			Text = ManRes2String(amr, true);
			_data = amr;
			_desc = ManRes2String(amr, false);
			ImageIndex = icon;
			SelectedImageIndex = icon;
		}

		public override string NodeLabel{get{return "Manifest Resource";}}

		public override object Clone()
		{
			ManResNode tn = new ManResNode((AManifestResource)this._data);
			return tn;
		}

		public override void GenerateChildren()
		{
			base.GenerateChildren();

			AManifestResource amr = (AManifestResource)_data;

			if (!amr.IsResx)
				return;


			if (amr.Asm.GetManifestResourceStream(amr.Name) == null)
			{
				return;
			}

			ResourceReader  rl;

			try
			{
				rl = new ResourceReader(amr.Asm.GetManifestResourceStream(amr.Name));
			}
			catch//(Exception e)
			{
				return;
			}

			IDictionaryEnumerator en = rl.GetEnumerator();

			while(en.MoveNext())
			{
				Nodes.Add(new ResNode((string)en.Key, en.Value));
			}

		}

		public override MenuItem[] GetMenu()
		{
			MenuItem[] items = new MenuItem[1];

			items[0] = new MenuItem("Save To File", new EventHandler(this.OnMenu));

			return items;
		}

		public void OnMenu(object sender, EventArgs e)
		{
			AManifestResource amr = (AManifestResource)_data;

			try
			{
				Stream data = amr.Asm.GetManifestResourceStream(amr.Name);

				if (data == null)
				{
					MessageBox.Show("Unable to open resource", "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					return;
				}

				System.Windows.Forms.SaveFileDialog dlg = new SaveFileDialog();

				dlg.FileName = Text;

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					FileStream fs = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write, FileShare.None, 1024, false);

					byte[] bytes = new byte[data.Length];

					data.Read(bytes, 0, (int)data.Length);

					fs.Write(bytes, 0, (int)data.Length);
					fs.Close();

					data.Close();
				}
				else
				{
					return;
				}
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message, "Error saving resource", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
				return;
			}

			MessageBox.Show("Resource saved");
		}


	}

	class ResNode : Genetibase.Debug.BaseNode
	{
		public ResNode(){}
		public ResNode(string name, object o)
		{
			_data = o;
			_desc = name + " = [" + Object2String(o, false) + "]";
			Text = name;
			ImageIndex = IconRes;
			SelectedImageIndex = IconRes;
		}

		public override string NodeLabel{get{return "Resource Object From .RESX File";}}

		public override object Clone()
		{
			ResNode tn = new ResNode(Text, _data);
			return tn;
		}
	}

	class FolderNode : Genetibase.Debug.BaseNode
	{
		public FolderNode(){}
		public FolderNode(string s)
		{
			Text = s;
			_data = s;
			_desc = "Folder " + s;
			SelectedImageIndex = IconFolder;
			ImageIndex = IconFolder;
		}

		public override object Clone()
		{
			FolderNode tn = new FolderNode(Text);
			return tn;
		}

		public override bool Uninteresting{get{return true;}}

		public override void GenerateChildren()
		{
			_spinster = false;
		}

		public override void PrepareToGenerateChildren()
		{
		}
	}


}
