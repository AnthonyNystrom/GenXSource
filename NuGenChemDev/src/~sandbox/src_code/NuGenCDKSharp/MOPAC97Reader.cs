/* $RCSfile$
* $Author: rajarshi $
* $Date: 2006-06-10 17:12:48 +0200 (Sat, 10 Jun 2006) $
* $Revision: 6416 $
*
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-devel@lists.sourceforge.net
*
* This library is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public
* License as published by the Free Software Foundation; either
* version 2.1 of the License, or (at your option) any later version.
*
*  This library is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
* Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public
* License along with this library; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Org.OpenScience.CDK.IO.Formats;
using Org.OpenScience.CDK.IO.Setting;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using Org.Jmol.Adapter.Smarter;
using Org.OpenScience.CDK.Libio.Jmol;
using Org.Jmol.Api;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Reader for MOPAC 93, 97 and 2002 files. Only tested for MOPAC 93 files.
    /// It uses Jmol IO classes.
    /// 
    /// </summary>
    /// <cdk.module>  io-jmol </cdk.module>
    /// <cdk.keyword>  file format, MOPAC </cdk.keyword>
    /// <cdk.builddepends>  jmolIO.jar </cdk.builddepends>
    /// <cdk.builddepends>  jmolApis.jar </cdk.builddepends>
    /// <cdk.depends>  jmolIO.jar </cdk.depends>
    /// <cdk.depends>  jmolApis.jar </cdk.depends>
    public class MOPAC97Reader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new MOPAC97Format();
            }

        }
        override public IOSetting[] IOSettings
        {
            get
            {
                return new IOSetting[0];
            }

        }

        internal System.IO.StreamReader input = null;

        //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
        //UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
        public MOPAC97Reader(System.IO.Stream in_Renamed)
            : this(new System.IO.StreamReader(new System.IO.StreamReader(in_Renamed, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(in_Renamed, System.Text.Encoding.Default).CurrentEncoding))
        {
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public MOPAC97Reader(System.IO.StreamReader input)
        {
            if (input is System.IO.StreamReader)
            {
                this.input = (System.IO.StreamReader)input;
            }
            else
            {
                //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                this.input = new System.IO.StreamReader(input.BaseStream, input.CurrentEncoding);
            }
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public override void setReader(System.IO.StreamReader input)
        {
            if (input is System.IO.StreamReader)
            {
                this.input = (System.IO.StreamReader)input;
            }
            else
            {
                //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                this.input = new System.IO.StreamReader(input.BaseStream, input.CurrentEncoding);
            }
        }

        public override void setReader(System.IO.Stream input)
        {
            setReader(new System.IO.StreamReader(input, System.Text.Encoding.Default));
        }

        public override bool accepts(System.Type classObject)
        {
            System.Type[] interfaces = classObject.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (typeof(IMolecule).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IMolecule)
            {
                return readMolecule((IMolecule)object_Renamed);
            }
            else
            {
                throw new CDKException("Only supported are Molecule.");
            }
        }

        private IMolecule readMolecule(IMolecule molecule)
        {
            JmolAdapter adapter = new SmarterJmolAdapter();
            // note that it actually let's the adapter detect the format!
            System.Object model = adapter.openBufferedReader("", input);
            molecule.add(new Convertor(molecule.Builder).convert(model));
            return molecule;
        }

        public override void close()
        {
            input.Close();
        }
    }
}