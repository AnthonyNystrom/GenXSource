using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Next2Friends.Misc;

namespace Next2Friends.Data
{
    public enum BannerType { VideoInjection,PageBanner,EmailBanner,MobileBanner}

    public partial class Banner
    {
        public string BannerURL
        {
            get { return this.FileLocation; }
        }



        public static Banner GetBannerByWebBannerID(string WebBannerID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetBannerByWebBannerID");

            db.AddInParameter(dbCommand, "WebBannerID", DbType.String, WebBannerID);

            Banner banner = new Banner();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                banner = Banner.PopulateSingleObject(dr);

                dr.Close();
            }
            return banner;
        }

        public static Banner GetNextBanner(BannerType bannerType, string URL)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNextBanner");

            db.AddInParameter(dbCommand, "BannerType", DbType.Int32, bannerType);
            db.AddInParameter(dbCommand, "URL", DbType.String, URL);

            Banner banner = new Banner();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                banner = Banner.PopulateSingleObject(dr);

                dr.Close();
            }
            return banner;
        }

        public static Banner PopulateSingleObject(IDataReader dr)
        {
            List<Banner> banner = Banner.PopulateObject(dr);

            if (banner.Count > 0)
            {
                return banner[0];
            }
            else
            {
                return new Banner();
            }
        }

        public string ToHTML()
        {
            StringBuilder sbHTML = new StringBuilder();

            string[] parameters = new string[3];

            parameters[0] = "/ad/" + this.WebBannerID;
            parameters[1] = this.BannerURL;
            parameters[2] = "alterntive text";

            sbHTML.AppendFormat(@"<a href='{0}' style='border:0px'><img src='{1}' alt='{2}'></a>", parameters);

            return sbHTML.ToString();
        }
    }

}