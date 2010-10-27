using System;
using System.Collections.Generic;
using System.Text;

namespace Netron.Cobalt
{
	static partial class Application
	{
        public static class Addins
        {
            private static List<IAddin> mExtensions = new List<IAddin>();

            public static List<IAddin> Extensions
            {
                get { return mExtensions; }
            }

            public static int FindAddinIndex(string addinName)
            {
                
                IAddin found = mExtensions.Find(
                    delegate(IAddin addin)
                    {
                        if (addin.Info.FullName.Equals(addinName, StringComparison.CurrentCultureIgnoreCase))
                            return true;
                        else if (addin.Info.ShortName.Equals(addinName, StringComparison.CurrentCultureIgnoreCase))
                            return true;
                        else
                            return false;

                    }
                    );
                if (found != null)
                    return mExtensions.IndexOf(found);
                else
                    return -1;
            }
        }
	}
}
