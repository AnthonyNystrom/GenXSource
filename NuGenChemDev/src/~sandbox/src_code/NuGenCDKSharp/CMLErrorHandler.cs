/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
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
using Support;

namespace Org.OpenScience.CDK.IO.CML
{
    /// <summary> CDK's SAX2 ErrorHandler for giving feedback on XML errors in the CML document.
    /// Output is redirected to org.openscience.cdk.tools.LoggingTool.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// 
    /// </author>
    public class CMLErrorHandler : XmlSaxErrorHandler
    {
        //private LoggingTool logger;

        public bool reportErrors = true;
        public bool abortOnErrors = false;

        /// <summary> Constructor a SAX2 ErrorHandler that uses the cdk.tools.LoggingTool
        /// class to output errors and warnings to.
        /// 
        /// </summary>
        public CMLErrorHandler()
        {
            //logger = new LoggingTool(this);
            //logger.info("instantiated");
        }

        /// <summary> Internal procedure that outputs an SAXParseException with a significance level
        /// to the cdk.tools.LoggingTool logger.
        /// 
        /// </summary>
        /// <param name="level">    significance level
        /// </param>
        /// <param name="exception">Exception to output
        /// </param>
        //UPGRADE_TODO: Class 'org.xml.sax.SAXParseException' was converted to 'System.xml.XmlException' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
        private void print(System.String level, System.Xml.XmlException exception)
        {
            //if (level.Equals("warning"))
            //{
            //    logger.warn("** " + level + ": " + exception.Message);
            //    logger.warn("   URI  = " + exception.Source);
            //    logger.warn("   line = " + exception.LineNumber);
            //}
            //else
            //{
            //    logger.error("** " + level + ": " + exception.Message);
            //    logger.error("   URI  = " + exception.Source);
            //    logger.error("   line = " + exception.LineNumber);
            //}
        }

        // for recoverable errors, like validity problems

        /// <summary> Outputs a SAXParseException error to the logger.
        /// 
        /// </summary>
        /// <param name="exception">  Exception to output
        /// 
        /// </param>
        //UPGRADE_TODO: Class 'org.xml.sax.SAXParseException' was converted to 'System.xml.XmlException' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
        public virtual void error(System.Xml.XmlException exception)
        {
            if (reportErrors)
                print("error", exception);
            if (abortOnErrors)
                throw exception;
        }

        /// <summary> Outputs as fatal SAXParseException error to the logger.
        /// 
        /// </summary>
        /// <param name="exception">  Exception to output
        /// 
        /// </param>
        //UPGRADE_TODO: Class 'org.xml.sax.SAXParseException' was converted to 'System.xml.XmlException' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
        public virtual void fatalError(System.Xml.XmlException exception)
        {
            if (reportErrors)
                print("fatal", exception);
            if (abortOnErrors)
                throw exception;
        }

        /// <summary> Outputs a SAXParseException warning to the logger.
        /// 
        /// </summary>
        /// <param name="exception">  Exception to output
        /// 
        /// </param>
        //UPGRADE_TODO: Class 'org.xml.sax.SAXParseException' was converted to 'System.xml.XmlException' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
        public virtual void warning(System.Xml.XmlException exception)
        {
            if (reportErrors)
                print("warning", exception);
        }
    }
}