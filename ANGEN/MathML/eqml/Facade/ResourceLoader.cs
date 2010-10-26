using System.IO;
using System.Reflection;
using System.IO.Compression;

namespace Facade
{
    public class ResourceLoader
    {
        public static Stream GetStream(string appNamespace, string resourceName)
        {
            try
            {
                return FromGzip(Assembly.GetExecutingAssembly().GetManifestResourceStream(appNamespace + "." + resourceName));
            }
            catch
            {
                return null;
            }
        }

        public static Stream FromGzip(Stream baseStream)
        {
            GZipStream stream = new GZipStream(baseStream, CompressionMode.Decompress);
            int totalCount = 0;

            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[1000];

            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, 1000);
                if (bytesRead == 0)
                {
                    break;
                }
                ms.Write(buffer, 0, bytesRead);
                totalCount += bytesRead;
            }

            ms.Position = 0;
            return ms;
        }
    }
}