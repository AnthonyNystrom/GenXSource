/* ------------------------------------------------
 * ResourceProcessor.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using data = Next2Friends.Data;
using System.Drawing;
using Next2Friends.Misc;

namespace Next2Friends.WebServices.Utils
{
    internal static class ResourceProcessor
    {
        public static String GetThumbnailBase64String(String thumbnailSavePath)
        {
            var fullPath = String.Concat(
                            OSRegistry.GetDiskUserDirectory(),
                            thumbnailSavePath);
            return data.Photo.ImageToBase64String(new Bitmap(fullPath));
        }
    }
}
