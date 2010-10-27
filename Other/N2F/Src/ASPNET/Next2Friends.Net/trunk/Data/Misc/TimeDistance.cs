using System;
using System.Collections.Generic;
using System.Text;

namespace Next2Friends.Data
{
    public class TimeDistance
    {

        public static string ShortDateTime(DateTime dateTime)
        {
            if (dateTime.Date.ToShortDateString() == DateTime.Now.ToShortDateString())
            {
                return dateTime.ToShortTimeString();
            }
            else
            {
                return dateTime.ToString("MMM d");
            }
        }


        /// <summary>
        /// Returns the age in years from
        /// </summary>
        /// <param name="birthDate"></param>
        /// <returns></returns>
        public static int GetAgeYears(DateTime birthDate)
        {
            // cache the current time
            DateTime now = DateTime.Today; 

            // get the difference in years
            int years = now.Year - birthDate.Year;

            // subtract another year if we're before the birth day in the current year
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
            {
                --years;
            }

            return years;
        }


        /// <summary>
        /// The amount of time ago that something happened.. what? hmmmmmm
        /// </summary>
        public static string TimeAgo(DateTime TimeAgo)
        {
            string FormatedTime = "";

            TimeSpan ts = DateTime.Now - TimeAgo;
            string Plural;

            if (ts.Days > 0)
            {
                Plural = (ts.Days != 1) ? "s" : string.Empty;
                FormatedTime = ts.Days + " day" + Plural + " ago";
            }
            else if (ts.Hours > 0)
            {
                Plural = (ts.Hours != 1) ? "s" : string.Empty;
                FormatedTime = ts.Hours + " hour" + Plural + " ago";
            }
            else if (ts.Minutes > 0)
            {
                Plural = (ts.Minutes != 1) ? "s" : string.Empty;
                FormatedTime = ts.Minutes + " minute" + Plural + " ago";
            }
            else
            {
                Plural = (ts.Seconds != 1) ? "s" : string.Empty;
                FormatedTime = ts.Seconds + " second" + Plural + " ago";
            }

            //if (ts.Minutes == 0)
            //{
            //    Plural = (ts.Seconds!=1)? "s" : string.Empty;  
            //    FormatedTime = ts.Seconds + " second"+Plural+" ago";
            //}
            //else if (ts.Hours == 0)
            //{
            //    Plural = (ts.Minutes != 1) ? "s" : string.Empty;
            //    FormatedTime = ts.Minutes + " minute" + Plural + " ago";
            //}
            //else if (ts.Days == 0)
            //{
            //    Plural = (ts.Hours != 1) ? "s" : string.Empty;
            //    FormatedTime = ts.Hours + " hour" + Plural + " ago";
            //}
            //else if (ts.Days > 0)
            //{
            //    Plural = (ts.Days != 1) ? "s" : string.Empty;
            //    FormatedTime = ts.Days + " day" + Plural + " ago";
            //}

            return FormatedTime;
        }
    }
}
