/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-02 11:17:35 +0200 (Tue, 02 May 2006) $
* $Revision: 6123 $
*
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-devel@lists.sourceforge.net
*
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Org.OpenScience.CDK.Interfaces;
using System.Reflection;
using Org.OpenScience.CDK.Config.AtomTypes;

namespace Org.OpenScience.CDK.Config
{
    /// <summary> AtomType resource that reads the atom type configuration from an XML file.
    /// The format is required to be in the STMML format {@cdk.cite PMR2002}; examples
    /// can be found in CVS in the src/org/openscience/cdk/config/data directory.
    /// 
    /// </summary>
    /// <cdk.module>  core </cdk.module>
    public class CDKBasedAtomTypeConfigurator : IAtomTypeConfigurator
    {
        virtual public System.IO.Stream InputStream
        {
            set
            {
                this.ins = value;
            }

        }

        private System.String configFile = "structgen_atomtypes.xml";
        private System.IO.Stream ins = null;

        //private LoggingTool //logger;

        public CDKBasedAtomTypeConfigurator()
        {
            //logger = new LoggingTool(this);
        }

        /// <summary> Reads the atom types from the CDK based atom type list.
        /// 
        /// </summary>
        /// <param name="builder">IChemObjectBuilder used to construct the IAtomType's.
        /// </param>
        /// <throws>         IOException when a problem occured with reading from the InputStream </throws>
        /// <returns>        A Vector with read IAtomType's.
        /// </returns>
        public virtual System.Collections.ArrayList readAtomTypes(IChemObjectBuilder builder)
        {
            System.Collections.ArrayList atomTypes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(0));
            if (ins == null)
            {
                try
                {
                    ins = Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenCDKSharp." + configFile);
                }
                catch (System.Exception exc)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    //logger.error(exc.Message);
                    //logger.debug(exc);
                    throw new System.IO.IOException("There was a problem getting a stream for " + configFile + " with getClass.getClassLoader.getResourceAsStream");
                }
                if (ins == null)
                {
                    try
                    {
                        //UPGRADE_ISSUE: Method 'java.lang.Class.getResourceAsStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetResourceAsStream_javalangString'"
                        ins = Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenCDKSharp." + configFile);
                    }
                    catch (System.Exception exc)
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        //logger.error(exc.Message);
                        //logger.debug(exc);
                        throw new System.IO.IOException("There was a problem getting a stream for " + configFile + " with getClass.getResourceAsStream");
                    }
                }
            }
            if (ins == null)
                throw new System.IO.IOException("There was a problem getting an input stream");
            AtomTypeReader reader = new AtomTypeReader(new System.IO.StreamReader(ins, System.Text.Encoding.Default));
            atomTypes = reader.readAtomTypes(builder);
            for (int f = 0; f < atomTypes.Count; f++)
            {
                System.Object object_Renamed = atomTypes[f];
                if (object_Renamed == null)
                {
                    System.Console.Out.WriteLine("Expecting an object but found null!");
                    if (!(object_Renamed is IAtomType))
                    {
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        System.Console.Out.WriteLine("Expecting cdk.AtomType class, but got: " + object_Renamed.GetType().FullName);
                    }
                }
            }
            return atomTypes;
        }
    }
}