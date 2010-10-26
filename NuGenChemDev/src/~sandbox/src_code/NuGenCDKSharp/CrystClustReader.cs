/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-06 10:55:35 +0200 (Tue, 06 Jun 2006) $
* $Revision: 6346 $
*
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
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
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;
using javax.vecmath;
using Org.OpenScience.CDK.Math;
using Org.OpenScience.CDK.Geometry;

namespace Org.OpenScience.CDK.IO
{
    /// <cdk.module>  extra </cdk.module>
    public class CrystClustReader : DefaultChemObjectReader
    {
        override public IResourceFormat Format
        {
            get
            {
                return new CrystClustFormat();
            }

        }

        private System.IO.StreamReader input;
        //private LoggingTool //logger;

        public CrystClustReader()
        {
            //logger = new LoggingTool(this);
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public CrystClustReader(System.IO.StreamReader input)
            : this()
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

        public CrystClustReader(System.IO.Stream input)
            : this(new System.IO.StreamReader(input, System.Text.Encoding.Default))
        {
        }

        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public override void setReader(System.IO.StreamReader reader)
        {
            if (input is System.IO.StreamReader)
            {
                this.input = (System.IO.StreamReader)reader;
            }
            else
            {
                //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                this.input = new System.IO.StreamReader(reader.BaseStream, reader.CurrentEncoding);
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
                if (typeof(IChemFile).Equals(interfaces[i]))
                    return true;
            }
            return false;
        }

        public override IChemObject read(IChemObject object_Renamed)
        {
            if (object_Renamed is IChemFile)
            {
                IChemFile cf = readChemFile((IChemFile)object_Renamed);
                return cf;
            }
            else
            {
                throw new CDKException("Only supported is reading of ChemFile.");
            }
        }

        private IChemFile readChemFile(IChemFile file)
        {
            IChemSequence seq = file.Builder.newChemSequence();
            IChemModel model = file.Builder.newChemModel();
            ICrystal crystal = null;

            int lineNumber = 0;
            Vector3d a, b, c;

            try
            {
                System.String line = input.ReadLine();
                while (input.Peek() != -1 && line != null)
                {
                    //logger.debug((lineNumber++) + ": ", line);
                    if (line.StartsWith("frame:"))
                    {
                        //logger.debug("found new frame");
                        model = file.Builder.newChemModel();
                        crystal = file.Builder.newCrystal();

                        // assume the file format is correct

                        //logger.debug("reading spacegroup");
                        line = input.ReadLine();
                        //logger.debug((lineNumber++) + ": ", line);
                        crystal.SpaceGroup = line;

                        //logger.debug("reading unit cell axes");
                        Vector3d axis = new Vector3d();
                        //logger.debug("parsing A: ");
                        line = input.ReadLine();
                        //logger.debug((lineNumber++) + ": ", line);
                        axis.x = FortranFormat.atof(line);
                        line = input.ReadLine();
                        //logger.debug((lineNumber++) + ": ", line);
                        axis.y = FortranFormat.atof(line);
                        line = input.ReadLine();
                        //logger.debug((lineNumber++) + ": ", line);
                        axis.z = FortranFormat.atof(line);
                        crystal.A = axis;
                        axis = new Vector3d();
                        //logger.debug("parsing B: ");
                        line = input.ReadLine();
                        //logger.debug((lineNumber++) + ": ", line);
                        axis.x = FortranFormat.atof(line);
                        line = input.ReadLine();
                        //logger.debug((lineNumber++) + ": ", line);
                        axis.y = FortranFormat.atof(line);
                        line = input.ReadLine();
                        //logger.debug((lineNumber++) + ": ", line);
                        axis.z = FortranFormat.atof(line);
                        crystal.B = axis;
                        axis = new Vector3d();
                        //logger.debug("parsing C: ");
                        line = input.ReadLine();
                        //logger.debug((lineNumber++) + ": ", line);
                        axis.x = FortranFormat.atof(line);
                        line = input.ReadLine();
                        //logger.debug((lineNumber++) + ": ", line);
                        axis.y = FortranFormat.atof(line);
                        line = input.ReadLine();
                        //logger.debug((lineNumber++) + ": ", line);
                        axis.z = FortranFormat.atof(line);
                        crystal.C = axis;
                        //logger.debug("Crystal: ", crystal);
                        a = crystal.A;
                        b = crystal.B;
                        c = crystal.C;

                        //logger.debug("Reading number of atoms");
                        line = input.ReadLine();
                        //logger.debug((lineNumber++) + ": ", line);
                        int atomsToRead = System.Int32.Parse(line);

                        //logger.debug("Reading no molecules in assym unit cell");
                        line = input.ReadLine();
                        //logger.debug((lineNumber++) + ": ", line);
                        int Z = System.Int32.Parse(line);
                        crystal.Z = Z;

                        System.String symbol;
                        double charge;
                        Point3d cart;
                        for (int i = 1; i <= atomsToRead; i++)
                        {
                            cart = new Point3d();
                            line = input.ReadLine();
                            //logger.debug((lineNumber++) + ": ", line);
                            symbol = line.Substring(0, (line.IndexOf(":")) - (0));
                            charge = System.Double.Parse(line.Substring(line.IndexOf(":") + 1));
                            line = input.ReadLine();
                            //logger.debug((lineNumber++) + ": ", line);
                            cart.x = System.Double.Parse(line); // x
                            line = input.ReadLine();
                            //logger.debug((lineNumber++) + ": ", line);
                            cart.y = System.Double.Parse(line); // y
                            line = input.ReadLine();
                            //logger.debug((lineNumber++) + ": ", line);
                            cart.z = System.Double.Parse(line); // z
                            IAtom atom = file.Builder.newAtom(symbol);
                            atom.setCharge(charge);
                            // convert cartesian coords to fractional
                            Point3d frac = CrystalGeometryTools.cartesianToFractional(a, b, c, cart);
                            atom.setFractionalPoint3d(frac);
                            crystal.addAtom(atom);
                            //logger.debug("Added atom: ", atom);
                        }

                        model.Crystal = crystal;
                        seq.addChemModel(model);
                    }
                    else
                    {
                        //logger.debug("Format seems broken. Skipping these lines:");
                        while (!line.StartsWith("frame:") && input.Peek() != -1 && line != null)
                        {
                            line = input.ReadLine();
                            //logger.debug(lineNumber++ + ": ", line);
                        }
                        //logger.debug("Ok, resynched: found new frame");
                    }
                }
                file.addChemSequence(seq);
            }
            catch (System.Exception exception)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                System.String message = "Error while parsing CrystClust file: " + exception.Message;
                //logger.error(message);
                //logger.debug(exception);
                throw new CDKException(message, exception);
            }
            return file;
        }

        public override void close()
        {
            input.Close();
        }
    }
}