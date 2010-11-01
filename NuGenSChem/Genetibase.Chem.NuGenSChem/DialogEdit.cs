using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace Genetibase.Chem.NuGenSChem
{
	
	/*
	A dialog box which allows tabulated editing of molecular data, as an alternative to doing all editing graphically.*/
	
	[Serializable]
	public class DialogEdit:System.Windows.Forms.Form
	{
        public DialogEdit(Control Parent, Molecule Mol, List<int> SelIdx)
		{
            // super(Parent, "Edit Molecule", true)

            mol = Mol.Clone();
            aselidx = SelIdx;
            //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
            bselidx = new List<int>();
            for (int n = 1; n <= mol.NumBonds(); n++)
                if (aselidx.IndexOf(mol.BondFrom(n)) >= 0 && aselidx.IndexOf(mol.BondTo(n)) >= 0)
                    bselidx.Add(n);

            //UPGRADE_ISSUE: Method 'javax.swing.JDialog.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJDialogsetLayout_javaawtLayoutManager'"
            //UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
            /*
            setLayout(new BorderLayout());*/

            atoms = new AnonymousClassJTable(this, CompileAtomData(), new System.String[] { "#", "El", "X", "Y", "Charge", "Unpaired", "HExplicit" });
            bonds = new AnonymousClassJTable1(this, CompileBondData(), new System.String[] { "#", "From", "To", "Order", "Type" });

            //UPGRADE_TODO: Method 'javax.swing.table.TableColumn.setCellEditor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
            //UPGRADE_TODO: The equivalent in .NET for method 'javax.swing.table.TableColumnModel.getColumn' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            // ((System.Data.DataTable)atoms.DataSource).Columns[0].setCellEditor(null);
            System.Windows.Forms.ComboBox bondTypes = new System.Windows.Forms.ComboBox();
            for (int n = 0; n < BOND_TYPES.Length; n++)
                bondTypes.Items.Add(BOND_TYPES[n]);
            //UPGRADE_TODO: Method 'javax.swing.table.TableColumn.setCellEditor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
            //UPGRADE_TODO: The equivalent in .NET for method 'javax.swing.table.TableColumnModel.getColumn' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            //UPGRADE_ISSUE: Constructor 'javax.swing.DefaultCellEditor.DefaultCellEditor' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingDefaultCellEditor'"
            // TODO: Have to see this in action before I can tel what they need to do. 
            // ((System.Data.DataTable)bonds.DataSource).Columns[4].setCellEditor(new DefaultCellEditor(bondTypes));

            System.Windows.Forms.Panel tabAtoms = new System.Windows.Forms.Panel(), tabBonds = new System.Windows.Forms.Panel();
            //UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
            //UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
            /*
            tabAtoms.setLayout(new BorderLayout());*/
            //UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
            //UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
            /*
            tabBonds.setLayout(new BorderLayout());*/

            //UPGRADE_ISSUE: Method 'javax.swing.JTable.setPreferredScrollableViewportSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTablesetPreferredScrollableViewportSize_javaawtDimension'"
            
            // TODO: atoms.setPreferredScrollableViewportSize(new System.Drawing.Size(350, 200));
            //UPGRADE_ISSUE: Method 'javax.swing.JTable.setPreferredScrollableViewportSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTablesetPreferredScrollableViewportSize_javaawtDimension'"
            // TODO: bonds.setPreferredScrollableViewportSize(new System.Drawing.Size(350, 200));

            //UPGRADE_TODO: Constructor 'javax.swing.JScrollPane.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollPaneJScrollPane_javaawtComponent'"
            System.Windows.Forms.ScrollableControl temp_scrollablecontrol2;
            temp_scrollablecontrol2 = new System.Windows.Forms.ScrollableControl();
            temp_scrollablecontrol2.AutoScroll = true;
            temp_scrollablecontrol2.Controls.Add(atoms);
            //UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
            System.Windows.Forms.Control temp_Control;
            temp_Control = temp_scrollablecontrol2;
            tabAtoms.Controls.Add(temp_Control);
            //UPGRADE_TODO: Constructor 'javax.swing.JScrollPane.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollPaneJScrollPane_javaawtComponent'"
            System.Windows.Forms.ScrollableControl temp_scrollablecontrol4;
            temp_scrollablecontrol4 = new System.Windows.Forms.ScrollableControl();
            temp_scrollablecontrol4.AutoScroll = true;
            temp_scrollablecontrol4.Controls.Add(bonds);
            //UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
            System.Windows.Forms.Control temp_Control2;
            temp_Control2 = temp_scrollablecontrol4;
            tabBonds.Controls.Add(temp_Control2);

            tabs = new System.Windows.Forms.TabControl();
            //UPGRADE_TODO: Method 'javax.swing.JTabbedPane.addTab' was converted to 'SupportClass.TabControlSupport.AddTab' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJTabbedPaneaddTab_javalangString_javaawtComponent'"
            SupportClass.TabControlSupport.AddTab(tabs, "Atoms", tabAtoms);
            //UPGRADE_TODO: Method 'javax.swing.JTabbedPane.addTab' was converted to 'SupportClass.TabControlSupport.AddTab' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJTabbedPaneaddTab_javalangString_javaawtComponent'"
            SupportClass.TabControlSupport.AddTab(tabs, "Bonds", tabBonds);
            //UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
            Controls.Add(tabs);
            tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            tabs.BringToFront();

            System.Windows.Forms.Panel buttons = new System.Windows.Forms.Panel();
            //UPGRADE_TODO: Constructor 'java.awt.FlowLayout.FlowLayout' was converted to 'System.Object[]' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFlowLayoutFlowLayout_int'"
            buttons.Tag = new System.Object[] { (int)System.Drawing.ContentAlignment.TopRight, 5, 5 };
            buttons.Layout += new System.Windows.Forms.LayoutEventHandler(SupportClass.FlowLayoutResize);
            accept = SupportClass.ButtonSupport.CreateStandardButton("Accept");
            accept.Click += new System.EventHandler(this.actionPerformed);
            SupportClass.CommandManager.CheckCommand(accept);
            reject = SupportClass.ButtonSupport.CreateStandardButton("Reject");
            reject.Click += new System.EventHandler(this.actionPerformed);
            SupportClass.CommandManager.CheckCommand(reject);
            //UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
            buttons.Controls.Add(accept);
            //UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
            buttons.Controls.Add(reject);
            //UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
            Controls.Add(buttons);
            buttons.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttons.SendToBack();

            // TODO: What does pack do? 
            // pack();
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassJTable' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		//UPGRADE_TODO: Class 'javax.swing.JTable' was converted to 'System.Windows.Forms.DataGrid' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
		[Serializable]
		private class AnonymousClassJTable:System.Windows.Forms.DataGrid
		{
			private void  InitBlock(DialogEdit enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DialogEdit enclosingInstance;
			public DialogEdit Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_ISSUE: Constructor 'javax.swing.JTable.JTable' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000'"
			internal AnonymousClassJTable(DialogEdit enclosingInstance, System.Object[][] Param1, System.Object[] Param2):base()
			{
                // super(Param1, Param2); 
				InitBlock(enclosingInstance);
			}
			//UPGRADE_NOTE: The equivalent of method 'javax.swing.JTable.isCellEditable' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public bool isCellEditable(int row, int column)
			{
				return column > 0;
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassJTable1' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		//UPGRADE_TODO: Class 'javax.swing.JTable' was converted to 'System.Windows.Forms.DataGrid' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
		[Serializable]
		private class AnonymousClassJTable1:System.Windows.Forms.DataGrid
		{
			private void  InitBlock(DialogEdit enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DialogEdit enclosingInstance;
			public DialogEdit Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_ISSUE: Constructor 'javax.swing.JTable.JTable' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000'"
			internal AnonymousClassJTable1(DialogEdit enclosingInstance, System.Object[][] Param1, System.Object[] Param2)
			{
                // super(Param1, Param2)
				InitBlock(enclosingInstance);
			}
			//UPGRADE_NOTE: The equivalent of method 'javax.swing.JTable.isCellEditable' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public bool isCellEditable(int row, int column)
			{
				return column > 2;
			}
		}

		internal Molecule mol, retMol = null;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		List<int> aselidx, bselidx;
		
		internal System.Windows.Forms.TabControl tabs;
        internal Button accept, reject;
		//UPGRADE_TODO: Class 'javax.swing.JTable' was converted to 'System.Windows.Forms.DataGrid' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
		internal System.Windows.Forms.DataGrid atoms, bonds;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'BOND_TYPES'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] BOND_TYPES = new System.String[]{"Normal", "Inclined", "Declined", "Unknown"};
		

		
		public virtual Molecule exec()
		{
			//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
			//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
			Visible = true;
			return retMol;
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
		{
			if (event_sender == accept)
			{
				if (!ReadData())
					return ;
				retMol = mol;
				//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
				//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
				Visible = false;
			}
			if (event_sender == reject)
			{
				//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
				//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
				Visible = false;
			}
		}
		
		internal virtual System.Object[][] CompileAtomData()
		{
			System.Object[][] data = new System.Object[aselidx.Count][];
			
			//UPGRADE_ISSUE: Class 'java.text.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
			//UPGRADE_ISSUE: Constructor 'java.text.DecimalFormat.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
			// DecimalFormat fmt = new DecimalFormat("0.0000");
            string fmtStr = "N4"; 

            for (int n = 0; n < aselidx.Count; n++)
			{
				int i = aselidx[n];
				System.Object[] da = new System.Object[7];
				da[0] = (System.Int32) i;
				da[1] = new System.Text.StringBuilder(mol.AtomElement(i)).ToString();
				da[2] = mol.AtomX(i).ToString(fmtStr);
                da[3] = mol.AtomY(i).ToString(fmtStr);
				da[4] = System.Convert.ToString(mol.AtomCharge(i));
				da[5] = System.Convert.ToString(mol.AtomUnpaired(i));
				da[6] = mol.AtomHExplicit(i) == Molecule.HEXPLICIT_UNKNOWN?"?":System.Convert.ToString(mol.AtomHExplicit(i));
				data[n] = da;
			}
			
			return data;
		}
		
		internal virtual System.Object[][] CompileBondData()
		{
			System.Object[][] data = new System.Object[bselidx.Count][];
			
			for (int n = 0; n < bselidx.Count; n++)
			{
				int i = bselidx[n];
				System.Object[] db = new System.Object[5];
				db[0] = (System.Int32) i;
				db[1] = (System.Int32) mol.BondFrom(i);
				db[2] = (System.Int32) mol.BondTo(i);
				db[3] = System.Convert.ToString(mol.BondOrder(i));
				db[4] = new System.Text.StringBuilder(BOND_TYPES[mol.BondType(i)]).ToString();
				data[n] = db;
			}
			
			return data;
		}
		
		internal virtual bool ReadData()
		{
			for (int n = 0; n < ((System.Data.DataTable) atoms.DataSource).Rows.Count; n++)
			{
				int i = (System.Int32) ((System.Data.DataTable) atoms.DataSource).Rows[n][0];
				mol.SetAtomElement(i, (System.String) ((System.Data.DataTable) atoms.DataSource).Rows[n][1]);
				mol.SetAtomPos(i, SafeDouble((System.String) ((System.Data.DataTable) atoms.DataSource).Rows[n][2]), SafeDouble((System.String) ((System.Data.DataTable) atoms.DataSource).Rows[n][3]));
				
				mol.SetAtomCharge(i, SafeInt((System.String) ((System.Data.DataTable) atoms.DataSource).Rows[n][4]));
				mol.SetAtomUnpaired(i, SafeInt((System.String) ((System.Data.DataTable) atoms.DataSource).Rows[n][5]));
				System.String hyStr = (System.String) ((System.Data.DataTable) atoms.DataSource).Rows[n][6];
				int hy = SafeInt(hyStr);
				mol.SetAtomHExplicit(i, String.CompareOrdinal(hyStr, "0") == 0?0:(hy > 0?hy:Molecule.HEXPLICIT_UNKNOWN));
			}
			for (int n = 0; n < ((System.Data.DataTable) bonds.DataSource).Rows.Count; n++)
			{
				int i = (System.Int32) ((System.Data.DataTable) bonds.DataSource).Rows[n][0];
				mol.SetBondOrder(i, System.Int32.Parse((System.String) ((System.Data.DataTable) bonds.DataSource).Rows[n][3]));
				int type;
				for (int j = BOND_TYPES.Length - 1; j >= 0; j--)
					if (String.CompareOrdinal(BOND_TYPES[j], (System.String) ((System.Data.DataTable) bonds.DataSource).Rows[n][4]) == 0)
					{
						mol.SetBondType(i, j); break;
					}
			}
			return true;
		}
		
		internal virtual int SafeInt(System.String Str)
		{
			try
			{
				return System.Int32.Parse(Str);
			}
			catch (System.FormatException)
			{
				return 0;
			}
		}
		internal virtual double SafeDouble(System.String Str)
		{
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				return System.Double.Parse(Str);
			}
			catch (System.FormatException)
			{
				return 0;
			}
		}
	}
}