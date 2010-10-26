using System.IO;
using System.Reflection;

namespace Fonts
{
    public class FontsProvider
    {
        public FontCollection LoadAll()
        {
            FontCollection r = null;
            r = new FontCollection();

            Load("ESSTIXOne", "ESSTIX1_.TTF", r);
            Load("ESSTIXTwo", "ESSTIX2_.TTF", r);
            Load("ESSTIXThree", "ESSTIX3_.TTF", r);
            Load("ESSTIXFour", "ESSTIX4_.TTF", r);
            Load("ESSTIXFive", "ESSTIX5_.TTF", r);
            Load("ESSTIXSix", "ESSTIX6_.TTF", r);
            Load("ESSTIXSeven", "ESSTIX7_.TTF", r);
            Load("ESSTIXEight", "ESSTIX8_.TTF", r);
            Load("ESSTIXNine", "ESSTIX9_.TTF", r);
            Load("ESSTIXTen", "ESSTIX10.TTF", r);
            Load("ESSTIXEleven", "ESSTIX11.TTF", r);
            Load("ESSTIXTwelve", "ESSTIX12.TTF", r);
            Load("ESSTIXThirteen", "ESSTIX13.TTF", r);
            Load("ESSTIXFourteen", "ESSTIX14.TTF", r);
            Load("ESSTIXFifteen", "ESSTIX15.TTF", r);
            Load("ESSTIXSixteen", "ESSTIX16.TTF", r);
            Load("ESSTIXSeventeen", "ESSTIX17.TTF", r);

            return r;
        }

        private void Load(string sFontName, string sFontFileName, FontCollection FontCollection)
        {
            try
            {
                Stream stream = Facade.ResourceLoader.GetStream("Binary", sFontFileName);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int) stream.Length);
                FontCollection.Put(sFontName, buffer);
                buffer = null;
                stream.Close();
            }
            catch
            {
            }
        }
    }
}