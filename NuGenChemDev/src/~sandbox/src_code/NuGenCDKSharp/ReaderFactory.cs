/* $RCSfile$
* $Author: egonw $
* $Date: 2006-06-23 13:03:45 +0200 (Fri, 23 Jun 2006) $
* $Revision: 6501 $
*
* Copyright (C) 2001-2003  Jmol Project
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-devel@lists.sourceforge.net
*
*  This library is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public
*  License as published by the Free Software Foundation; either
*  version 2.1 of the License, or (at your option) any later version.
*
*  This library is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
*  Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Org.OpenScience.CDK.IO.Formats;
using System.Reflection;
using System.IO;
using Support;
using System.Runtime.Remoting;
using System.Collections.Generic;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> A factory for creating ChemObjectReaders. The type of reader
    /// created is determined from the content of the input. Formats
    /// of GZiped files can be detected too.
    /// 
    /// A typical example is:
    /// <pre>
    /// StringReader stringReader = "&lt;molecule/>";
    /// ChemObjectReader reader = new ReaderFactory().createReader(stringReader);
    /// </pre>
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>   Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    /// <author>   Bradley A. Smith <bradley@baysmith.com>
    /// </author>
    public class ReaderFactory
    {
        virtual public List<IChemFormatMatcher> Formats
        {
            get
            {
                return formats;
            }

        }

        private const System.String IO_FORMATS_LIST = "io-formats.set";

        private int headerLength;
        //private LoggingTool logger;

        private static List<IChemFormatMatcher> formats = null;

        /// <summary> Constructs a ReaderFactory which tries to detect the format in the
        /// first 65536 chars.
        /// </summary>
        public ReaderFactory()
            : this(65536)
        {
        }

        /// <summary> Constructs a ReaderFactory which tries to detect the format in the
        /// first given number of chars.
        /// 
        /// </summary>
        /// <param name="headerLength">length of the header in number of chars
        /// </param>
        public ReaderFactory(int headerLength)
        {
            //logger = new LoggingTool(this);
            this.headerLength = headerLength;
            loadReaders();
        }

        /// <summary> Registers a format for detection.</summary>
        public virtual void registerFormat(IChemFormatMatcher format)
        {
            formats.Add(format);
        }

        private void loadReaders()
        {
            if (formats == null)
            {
                formats = new List<IChemFormatMatcher>();
                    //System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
                try
                {
                    //logger.debug("Starting loading Readers...");
                    Stream stm = Assembly.GetExecutingAssembly().GetManifestResourceStream("NuGenCDKSharp." + IO_FORMATS_LIST);
                    System.IO.StreamReader reader = new System.IO.StreamReader(stm);

                    int formatCount = 0;
                    while (reader.Peek() != -1)
                    {
                        // load them one by one
                        System.String formatName = reader.ReadLine();
                        formatCount++;
                        try
                        {
                            ObjectHandle handle = System.Activator.CreateInstance("NuGenCDKSharp", formatName);
                            IChemFormatMatcher format = (IChemFormatMatcher)handle.Unwrap();
                            formats.Add(format);
                            //logger.info("Loaded IO format: " + format.GetType().FullName);
                        }
                        catch (System.Exception exception)
                        {
                            //logger.error("Could not find this ChemObjectReader: ", formatName);
                            //logger.debug(exception);
                        }
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

        /// <summary> Creates a String of the Class name of the <code>IChemObject</code> reader
        /// for this file format. The input is read line-by-line
        /// until a line containing an identifying string is
        /// found.
        /// 
        /// <p>The ReaderFactory detects more formats than the CDK
        /// has Readers for.
        /// 
        /// <p>This method is not able to detect the format of gziped files.
        /// Use <code>guessFormat(InputStream)</code> instead for such files.
        /// 
        /// </summary>
        /// <throws>  IOException  if an I/O error occurs </throws>
        /// <throws>  IllegalArgumentException if the input is null </throws>
        /// <summary> 
        /// </summary>
        /// <seealso cref="guessFormat(InputStream)">
        /// </seealso>
        public virtual IChemFormat guessFormat(StreamReader input)
        {
            if (input == null)
            {
                throw new System.ArgumentException("input cannot be null");
            }

            // make a copy of the header
            char[] header = new char[this.headerLength];
            //UPGRADE_ISSUE: Method 'java.io.BufferedReader.markSupported' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000'"
            //if (!input.markSupported())
            //{
            //    //logger.error("Mark not supported");
            //    throw new System.ArgumentException("input must support mark");
            //}
            //int pos = this.headerLength;//input.mark(this.headerLength);
            input.Read(header, 0, this.headerLength);
            input.BaseStream.Seek(0, SeekOrigin.Begin);

            //UPGRADE_ISSUE: Constructor 'java.io.BufferedReader.BufferedReader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioBufferedReaderBufferedReader_javaioReader'"
            System.IO.StringReader buffer = new StringReader(new System.String(header));

            /* Search file for a line containing an identifying keyword */
            System.String line = null;
            int lineNumber = 1;
            while ((line = buffer.ReadLine()) != null)
            {
                //logger.debug(lineNumber + ": ", line);
                for (int i = 0; i < formats.Count; i++)
                {
                    IChemFormatMatcher cfMatcher = (IChemFormatMatcher)formats[i];
                    if (cfMatcher.matches(lineNumber, line))
                    {
                        //logger.info("Detected format: ", cfMatcher.FormatName);
                        return cfMatcher;
                    }
                }
                lineNumber++;
            }

            //logger.warn("Now comes the tricky and more difficult ones....");
            //UPGRADE_ISSUE: Constructor 'java.io.BufferedReader.BufferedReader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioBufferedReaderBufferedReader_javaioReader'"
            buffer = new StringReader(new System.String(header));

            line = buffer.ReadLine();
            // is it a XYZ file?
            SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(line.Trim());
            try
            {
                int tokenCount = tokenizer.Count;
                if (tokenCount == 1)
                {
                    System.Int32.Parse(tokenizer.NextToken());
                    // if not failed, then it is a XYZ file
                    return null;// new org.openscience.cdk.io.formats.XYZFormat();
                }
                else if (tokenCount == 2)
                {
                    System.Int32.Parse(tokenizer.NextToken());
                    if ("Bohr".ToUpper().Equals(tokenizer.NextToken().ToUpper()))
                    {
                        return null;// new org.openscience.cdk.io.formats.XYZFormat();
                    }
                }
            }
            catch (System.FormatException exception)
            {
                //logger.info("No, it's not a XYZ file");
            }

            //logger.warn("File format undetermined");
            return null;
        }

        public virtual IChemFormat guessFormat(System.IO.Stream input)
        {
            System.IO.BufferedStream bistream = new System.IO.BufferedStream(input, 8192);
            System.IO.Stream istreamToRead = bistream; // if gzip test fails, then take default
            SupportClass.BufferedStreamManager.manager.MarkPosition(5, bistream);
            int countRead = 0;
            try
            {
                sbyte[] abMagic = new sbyte[4];
                countRead = SupportClass.ReadInput(bistream, abMagic, 0, 4);
                bistream.Position = SupportClass.BufferedStreamManager.manager.ResetMark(bistream);
                if (countRead == 4)
                {
                    if (abMagic[0] == (sbyte)0x1F && abMagic[1] == (sbyte)SupportClass.Identity(0x8B))
                    {
                        //UPGRADE_ISSUE: Constructor 'java.util.zip.GZIPInputStream.GZIPInputStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipGZIPInputStream'"
                        istreamToRead = null;// new GZIPInputStream(bistream);
                    }
                }
            }
            catch (System.IO.IOException exception)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.error(exception.Message);
                //logger.debug(exception);
            }
            //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            //UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
            return guessFormat(new System.IO.StreamReader(new System.IO.StreamReader(istreamToRead, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(istreamToRead, System.Text.Encoding.Default).CurrentEncoding));
        }

        public virtual IChemFormat guessFormatByExtension(string filename)
        {
            try
            {
                string ext = Path.GetExtension(filename).ToLower().Substring(1);

                foreach (IChemFormatMatcher format in formats)
                {
                    foreach (string nameExtension in format.NameExtensions)
                    {
                        if (nameExtension == ext)
                            return format;
                    }
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Detects the format of the Reader input, and if known, it will return
        /// a CDK Reader to read the format, or null when the reader is not
        /// implemented.
        /// </summary>
        /// <returns> null if CDK does not contain a reader for the detected format.
        /// 
        /// </returns>
        /// <seealso cref="createReader(Reader)">
        /// </seealso>
        public virtual IChemObjectReader createReader(System.IO.Stream input)
        {
            System.IO.BufferedStream bistream = new System.IO.BufferedStream(input, 8192);
            System.IO.Stream istreamToRead = bistream; // if gzip test fails, then take default
            SupportClass.BufferedStreamManager.manager.MarkPosition(5, bistream);
            int countRead = 0;
            try
            {
                sbyte[] abMagic = new sbyte[4];
                countRead = SupportClass.ReadInput(bistream, abMagic, 0, 4);
                bistream.Position = SupportClass.BufferedStreamManager.manager.ResetMark(bistream);
                if (countRead == 4)
                {
                    if (abMagic[0] == (sbyte)0x1F && abMagic[1] == (sbyte)SupportClass.Identity(0x8B))
                    {
                        //UPGRADE_ISSUE: Constructor 'java.util.zip.GZIPInputStream.GZIPInputStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipGZIPInputStream'"
                        istreamToRead = null;// new GZIPInputStream(bistream);
                    }
                }
            }
            catch (System.IO.IOException exception)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.error(exception.Message);
                //logger.debug(exception);
            }
            return createReader(new System.IO.StreamReader(istreamToRead, System.Text.Encoding.Default));
        }

        /// <summary> Creates a new IChemObjectReader based on the given IChemFormat.
        /// 
        /// </summary>
        /// <seealso cref="createReader(InputStream)">
        /// </seealso>
        public virtual IChemObjectReader createReader(IChemFormat format)
        {
            if (format != null)
            {
                System.String readerClassName = format.ReaderClassName;
                if (readerClassName != null)
                {
                    try
                    {
                        // make a new instance of this class
                        ObjectHandle handle = System.Activator.CreateInstance("NuGenCDKSharp", readerClassName);
                        return (IChemObjectReader)handle.Unwrap();
                    }
                    catch (System.Exception exception)
                    {
                        //logger.error("Could not find this ChemObjectReader: ", readerClassName);
                        //logger.debug(exception);
                    }
                }
                else
                {
                    // TODO: HAndle no reader available
                    //logger.warn("ChemFormat is recognized, but no reader is available.");
                }
            }
            else
            {
                //logger.warn("ChemFormat is not recognized.");
            }
            return null;
        }

        /// <summary> Detects the format of the Reader input, and if known, it will return
        /// a CDK Reader to read the format. This method is not able to detect the 
        /// format of gziped files. Use createReader(InputStream) instead for such 
        /// files.
        /// 
        /// </summary>
        /// <seealso cref="createReader(InputStream)">
        /// </seealso>
        //UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        public virtual IChemObjectReader createReader(System.IO.StreamReader input)
        {
            if (!(input is System.IO.StreamReader))
            {
                //UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                input = new System.IO.StreamReader(input.BaseStream, input.CurrentEncoding);
            }
            IChemFormat chemFormat = guessFormat((System.IO.StreamReader)input);
            IChemObjectReader coReader = createReader(chemFormat);
            try
            {
                coReader.setReader(input);
            }
            catch (System.Exception exception)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                //logger.error("Could not set the Reader source: ", exception.Message);
                //logger.debug(exception);
            }
            return coReader;
        }

        public virtual IChemObjectReader createReader(string filename, StreamReader input)
        {
            IChemFormat chemFormat = guessFormatByExtension(filename);
            IChemObjectReader coReader = createReader(chemFormat);

            try
            {
                coReader.setReader(input);
            }
            catch { }
            return coReader;
        }

        public IChemFormatMatcher[] GetFormats()
        {
            return formats.ToArray();
        }
    }
}