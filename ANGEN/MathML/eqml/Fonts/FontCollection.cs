using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace Fonts
{
    public class FontCollection
    {
        public FontCollection()
        {
            this.fonts_ = null;
            this.fonts_ = new PrivateFontCollection();
            this.hash_ = new Hashtable();
        }

        ~FontCollection()
        {
            try
            {
                this.hash_.Clear();
                this.hash_ = null;
                try
                {
                    IEnumerator enumerator = this.fonts_.Families.GetEnumerator();
                    while (enumerator.Current != null)
                    {
                        ((FontFamily) enumerator.Current).Dispose();
                        enumerator.MoveNext();
                    }
                    this.fonts_.Dispose();
                }
                catch
                {
                }
                this.fonts_ = null;
            }
            catch
            {
                return;
            }
        }

        public void Put(string sFontName, byte[] fontdata)
        {
            IntPtr intPtr = Marshal.AllocHGlobal((int) (Marshal.SizeOf(typeof(byte)) * fontdata.Length));
            Marshal.Copy(fontdata, 0, intPtr, fontdata.Length);
            this.fonts_.AddMemoryFont(intPtr, fontdata.Length);
            Marshal.FreeHGlobal(intPtr);
            try
            {
                for (int i = 0; i < this.fonts_.Families.Length; i++)
                {
                    if (this.fonts_.Families[i].Name == sFontName)
                    {
                        this.hash_.Add(sFontName, this.fonts_.Families[i]);
                    }
                }
            }
            catch
            {
            }
        }

        public FontFamily Get(string sFontName)
        {
            FontFamily family = null;
            try
            {
                object o = this.hash_[sFontName];
                if (o != null)
                {
                    family = (FontFamily) o;
                }
            }
            catch
            {
            }
            return family;
        }


        private PrivateFontCollection fonts_;
        private Hashtable hash_;
    }
}