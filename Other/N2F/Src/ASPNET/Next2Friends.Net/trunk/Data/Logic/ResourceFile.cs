using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Next2Friends.Misc;

namespace Next2Friends.Data
{

    public partial class ResourceFile
    {
        public static void CreateUserDirectories(Member member)
        {
            string root = OSRegistry.GetDiskUserDirectory() + member.NickName;

            Directory.CreateDirectory(root);
            Directory.CreateDirectory(root + @"\plrge");
            Directory.CreateDirectory(root + @"\pmed");
            Directory.CreateDirectory(root + @"\pthmb");

            Directory.CreateDirectory(root + @"\video");
            Directory.CreateDirectory(root + @"\vthmb");

            Directory.CreateDirectory(root + @"\aaflrge");
            Directory.CreateDirectory(root + @"\aafthmb");

            Directory.CreateDirectory(root + @"\nslrge");
            Directory.CreateDirectory(root + @"\nsmed");
            Directory.CreateDirectory(root + @"\nsthmb");

            Directory.CreateDirectory(root + @"\advrimg");
            Directory.CreateDirectory(root + @"\banner");
        }

        public string GetDiskPath()
        {
            return @"\\www\user\" + this.FileName.Replace("/",@"\");
        }

    }
}
