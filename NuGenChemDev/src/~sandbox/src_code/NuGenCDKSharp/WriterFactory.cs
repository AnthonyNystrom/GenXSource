/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-09 21:32:32 +0200 (Tue, 09 May 2006) $  
* $Revision: 6204 $
*
* Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-devel@lists.sourceforge.net
*
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
* All we ask is that proper credit is given for our work, which includes
* - but is not limited to - adding the above copyright notice to the beginning
* of your source code files, and to any copyright notice that you may distribute
* with programs based on this work.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*
*/
using System;
using Org.OpenScience.CDK.IO.Formats;
using Support;
using System.Reflection;
using System.Runtime.Remoting;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Helper tool to create IChemObjectWriters.
    /// 
    /// </summary>
    /// <author>  Egon Willighagen <ewilligh@uni-koeln.de>
    /// </author>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    public class WriterFactory
    {
        private const System.String IO_FORMATS_LIST = "io-formats.set";

        //private LoggingTool //logger;

        private static System.Collections.IList formats = null;

        /// <summary> Constructs a ChemObjectIOInstantionTests.</summary>
        public WriterFactory()
        {
            //logger = new LoggingTool(this);
        }

        /// <summary> Finds IChemFormats that provide a container for serialization for the
        /// given features. The syntax of the integer is explained in the DataFeatures class.
        /// 
        /// </summary>
        /// <param name="features">the data features for which a IChemFormat is searched
        /// </param>
        /// <returns>          an array of IChemFormat's that can contain the given features
        /// 
        /// </returns>
        /// <seealso cref="org.openscience.cdk.tools.DataFeatures">
        /// </seealso>
        public virtual IChemFormat[] findChemFormats(int features)
        {
            if (formats == null)
                loadFormats();

            System.Collections.IEnumerator iter = formats.GetEnumerator();
            System.Collections.IList matches = new System.Collections.ArrayList();
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (iter.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                IChemFormat format = (IChemFormat)iter.Current;
                if ((format.SupportedDataFeatures & features) == features)
                    matches.Add(format);
            }

            return (IChemFormat[])SupportClass.ICollectionSupport.ToArray(matches, new IChemFormat[0]);
        }

        private void loadFormats()
        {
            if (formats == null)
            {
                formats = new System.Collections.ArrayList();
                try
                {
                    //logger.debug("Starting loading Formats...");
                    //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    //UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
                    //UPGRADE_ISSUE: Method 'java.lang.ClassLoader.getResourceAsStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassLoader'"
                    //UPGRADE_ISSUE: Method 'java.lang.Class.getClassLoader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetClassLoader'"
                    System.IO.StreamReader reader = new System.IO.StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenCDKSharp." + IO_FORMATS_LIST));//new System.IO.StreamReader(this.GetType().getClassLoader().getResourceAsStream(IO_FORMATS_LIST), System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(this.GetType().getClassLoader().getResourceAsStream(IO_FORMATS_LIST), System.Text.Encoding.Default).CurrentEncoding);
                    int formatCount = 0;
                    while (reader.Peek() != -1)
                    {
                        // load them one by one
                        System.String formatName = reader.ReadLine();
                        formatCount++;
                        try
                        {
                            //UPGRADE_ISSUE: Method 'java.lang.ClassLoader.loadClass' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassLoader'"
                            //UPGRADE_ISSUE: Method 'java.lang.Class.getClassLoader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetClassLoader'"
                            ObjectHandle handle = System.Activator.CreateInstance("NuGenCDKSharp", formatName);
                            IResourceFormat format = (IResourceFormat)handle.Unwrap();//System.Activator.CreateInstance(//this.GetType().getClassLoader().loadClass(formatName));
                            if (format is IChemFormat)
                            {
                                formats.Add(format);
                                //logger.info("Loaded IChemFormat: " + format.GetType().FullName);
                            }
                        }
                        //UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
                        catch (System.Exception exception)
                        {
                            //logger.error("Could not find this IResourceFormat: ", formatName);
                            //logger.debug(exception);
                        }
                        //catch (System.Exception exception)
                        //{
                        //    //logger.error("Could not load this IResourceFormat: ", formatName);
                        //    //logger.debug(exception);
                        //}
                    }
                    //logger.info("Number of loaded formats used in detection: ", formatCount);
                }
                catch (System.Exception exception)
                {
                    //logger.error("Could not load this io format list: ", IO_FORMATS_LIST);
                    //logger.debug(exception);
                }
            }
        }

        /// <summary> Creates a new IChemObjectWriter based on the given IChemFormat.</summary>
        public virtual IChemObjectWriter createWriter(IChemFormat format)
        {
            if (format != null)
            {
                System.String writerClassName = format.WriterClassName;
                if (writerClassName != null)
                {
                    try
                    {
                        // make a new instance of this class
                        //UPGRADE_ISSUE: Method 'java.lang.ClassLoader.loadClass' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassLoader'"
                        //UPGRADE_ISSUE: Method 'java.lang.Class.getClassLoader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetClassLoader'"
                        ObjectHandle handle = System.Activator.CreateInstance("NuGenCDKSharp", writerClassName);
                        return (IChemObjectWriter)handle.Unwrap();//System.Activator.CreateInstance(this.GetType().getClassLoader().loadClass(writerClassName));
                    }
                    //UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
                    catch (System.Exception exception)
                    {
                        //logger.error("Could not find this ChemObjectWriter: ", writerClassName);
                        //logger.debug(exception);
                    }
                    //catch (System.Exception exception)
                    //{
                    //    //logger.error("Could not create this ChemObjectWriter: ", writerClassName);
                    //    //logger.debug(exception);
                    //}
                }
                else
                {
                    //logger.warn("ChemFormat is recognized, but no writer is available.");
                }
            }
            else
            {
                //logger.warn("ChemFormat is not recognized.");
            }
            return null;
        }
    }
}