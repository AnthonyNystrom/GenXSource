using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
namespace Netron.Cobalt
{
    public interface IAddin
    {
        /// <summary>
        /// Loads the addin into Cobalt
        /// </summary>
        void Load();
        /// <summary>
        /// Unloads the addin; here the developer is supposed to clean up all the stuff that's been added and not 
        /// to hog up the memory.
        /// </summary>
        void Unload();
        /// <summary>
        /// Gets the metadata of the addin
        /// </summary>
        AddinInfo Info { get;}

        Stream GetDocument(string address);
    }

   
}
