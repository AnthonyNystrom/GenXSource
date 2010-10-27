using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Next2Friends.Misc;
using System.Drawing;
using System.IO;

namespace Next2Friends.WebServices.Fix
{
    public static class ResourceService
    {
        public static String GetNickname(String path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            var slashIndex = path.IndexOf('/');
            return path.Substring(0, slashIndex);
        }

        public static Image GetMediumImage(String path, String fileName)
        {
            return Image.FromFile(Path.Combine(GetMediumImagePath(path), fileName));
        }

        public static String GetMediumImagePath(String thumbnailPath)
        {
            if (String.IsNullOrEmpty(thumbnailPath))
                throw new ArgumentNullException("thumbnailPath");

            return String.Concat(
                OSRegistry.GetDiskUserDirectory()
                , GetNickname(thumbnailPath)
                , @"\pmed\");
        }

        public static Image GetThumbnailImage(String path, String fileName)
        {
            return Image.FromFile(Path.Combine(GetThumbnailImageFullPath(path), fileName));
        }

        public static String GetThumbnailImageFullPath(String thumbnailPath)
        {
            if (String.IsNullOrEmpty(thumbnailPath))
                throw new ArgumentNullException("thumbnailPath");

            return String.Concat(
                OSRegistry.GetDiskUserDirectory()
                , GetNickname(thumbnailPath)
                , @"\pthmb\");
        }


    }
}
