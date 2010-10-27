using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace Netron.Cobalt
{
    public class AddinInfo
    {

        /// <summary>
        /// the Name field
        /// </summary>
        private string mFullName;
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string FullName
        {
            get { return mFullName; }
            set { mFullName = value; }
        }


        /// <summary>
        /// the ShortName field
        /// </summary>
        private string mShortName;
        /// <summary>
        /// Gets or sets the short name of the addin (which will be used e.g. in the URL of the documentation).
        /// </summary>
        public string ShortName
        {
            get { return mShortName; }
            set { mShortName = value; }
        }


        /// <summary>
        /// the Image field
        /// </summary>
        private string mImage;
        /// <summary>
        /// Gets or sets the Image
        /// </summary>
        public string Image
        {
            get { return mImage; }
            set { mImage = value; }
        }


        /// <summary>
        /// the Description field
        /// </summary>
        private string mDescription;
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }

    }
}
