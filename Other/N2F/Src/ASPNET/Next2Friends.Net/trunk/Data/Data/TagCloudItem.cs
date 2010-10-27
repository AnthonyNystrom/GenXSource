using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public class TagCloudItem
    {
        public string Tag { get; set; }
        public int Count { get; set; }

        /// <summary>
        /// Returns the most popular Video tags 
        /// </summary>
        public static List<TagCloudItem> GetVideoTagCloud()
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetVideoTagCloud");

            List<TagCloudItem> Cloud = new List<TagCloudItem>();

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                while(dr.Read())
                {
                    TagCloudItem item = new TagCloudItem();
                    item.Count = (int)dr[0];
                    item.Tag = (string)dr[1];
                    Cloud.Add(item);
                }

                dr.Close();
            }

            return Cloud;      
        }

        public static string GenerateTagCloud()
        {
            List<TagCloudItem> Tags = GetVideoTagCloud();

            string HTML = string.Empty;


            if (Tags.Count > 1)
            {
                int Steps = 5;
                int High = Tags[0].Count;
                int Low = Tags[0].Count;
                int Stage = (High - Low) / Steps;

                // get the highest and lowest
                for (int i = 0; i < Tags.Count; i++)
                {
                    if (Tags[i].Count < Low)
                    {
                        Low = Tags[i].Count;
                    }

                    if (Tags[i].Count > High)
                    {
                        High = Tags[i].Count;
                    }
                }

                int FontSize = 0;
                //get the range
                for (int i = 0; i < Tags.Count; i++)
                {
                    if (Tags[i].Count <= Low + Stage)
                    {
                        FontSize = 110;
                    }
                    else if (Tags[i].Count >= Low + Stage && Tags[i].Count < Low + (Stage * 2))
                    {
                        FontSize = 120;
                    }
                    else if (Tags[i].Count >= Low + (Stage * 2) && Tags[i].Count < Low + (Stage * 3))
                    {
                        FontSize = 130;
                    }
                    else if (Tags[i].Count >= Low + (Stage * 3) && Tags[i].Count < Low + (Stage * 4))
                    {
                        FontSize = 140;
                    }
                    else
                    {
                        FontSize = 150;
                    }

                    HTML += "<a href='/video/?g="+Tags[i].Tag+"' style='font-size:" + FontSize + "%'>" + Tags[i].Tag + "</a> ";
                }
            }

            return HTML;
        }
    }    
}
