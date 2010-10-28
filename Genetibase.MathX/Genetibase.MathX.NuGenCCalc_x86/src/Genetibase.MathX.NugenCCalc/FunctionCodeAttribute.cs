using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NugenCCalc
{
    /// <summary><para>Specifies whether a assembly contain chart adapters.</para></summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class FunctionCodeAttribute : Attribute
    {
        private string _sharpHeader = "";
        private string _sharpFooter = "";

        private string _vbHeader = "";
        private string _vbFooter = "";

        /// <summary>
        /// Initializes a new instance of the FunctionCodeAttribute class.
		/// </summary>
        public FunctionCodeAttribute(string sharpHeader, string sharpFooter, string vbHeader, string vbFooter)
		{
            _sharpHeader = sharpHeader;
            _sharpFooter = sharpFooter;
            _vbHeader = vbHeader;
            _vbFooter = vbFooter;
		}

        public string SharpHeader
        {
            get { return _sharpHeader; }
        }

        public string SharpFooter
        {
            get { return _sharpFooter; }
        }

        public string VBHeader
        {
            get { return _vbHeader; }
        }

        public string VBFooter
        {
            get { return _vbFooter; }
        }
    }
}
