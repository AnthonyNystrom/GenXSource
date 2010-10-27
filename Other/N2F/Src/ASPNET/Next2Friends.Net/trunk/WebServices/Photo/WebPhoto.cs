/* ------------------------------------------------
 * WebPhoto.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Next2Friends.WebServices.Photo
{
    public sealed class WebPhoto
    {
        public Int32 ID { get; set; }
        public String CreatedDT { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
    }
}
