/*  $RCSfile$
*  $Author: rajarshi $
*  $Date: 2006-06-10 17:05:07 +0200 (Sat, 10 Jun 2006) $
*  $Revision: 6415 $
*
*  Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
*  All we ask is that proper credit is given for our work, which includes
*  - but is not limited to - adding the above copyright notice to the beginning
*  of your source code files, and to any copyright notice that you may distribute
*  with programs based on this work.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public License
*  along with this program; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*
*/
using System;
using Org.OpenScience.CDK.IO.Formats;
using Org.OpenScience.CDK.IO.Setting;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using System.IO;
using Org.Jmol.Adapter.Smarter;
using Org.OpenScience.CDK.Libio.Jmol;
using Org.Jmol.Api;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Reads a molecule from an JME file using Jmol's JME reader.
    /// 
    /// </summary>
    /// <author>  Egon Willighagen
    /// </author>
    /// <author>  Miguel Howard
    /// </author>
    /// <cdk.module>  io-jmol </cdk.module>
    /// <cdk.created>  2004-05-18 </cdk.created>
    /// <cdk.keyword>  file format, JME </cdk.keyword>
    /// <cdk.builddepends>  jmolIO.jar </cdk.builddepends>
    /// <cdk.builddepends>  jmolApis.jar </cdk.builddepends>
    /// <cdk.depends>  jmolIO.jar </cdk.depends>
    /// <cdk.depends>  jmolApis.jar </cdk.depends>
    public class JMEReader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new JMEFormat();
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

        public JMEReader()
            : this((StreamReader)null)
        {
        }

        public JMEReader(System.IO.Stream in_Renamed)
            : this(new System.IO.StreamReader(in_Renamed, System.Text.Encoding.Default))
        {
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public JMEReader(System.IO.StreamReader input)
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

        /// <summary> Read a Molecule from a JME file.
        /// 
        /// </summary>
        /// <returns> The Molecule that was read from the MDL file.
        /// </returns>
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