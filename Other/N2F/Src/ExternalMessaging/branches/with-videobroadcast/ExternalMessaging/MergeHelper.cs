using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace ExternalMessaging
{
    class MergeHelper
    {
        public static string GenericMerge(string inputString, DataColumnCollection columns,DataRow row)
        {
            StringBuilder stringBuilder = new StringBuilder(inputString);

            foreach (DataColumn column in columns)
            {
                if (column.ColumnName.ToLower() == "title")
                {
                    stringBuilder.Replace("<#" + column.ColumnName + "#>", Utility.FormatStringForURL(row[column].ToString()));
                }
                stringBuilder.Replace("<#" + column.ColumnName + "#>", row[column].ToString());
                stringBuilder.Replace("<#Member" + column.ColumnName + "#>", row[column].ToString());

                stringBuilder.Replace("<#" + ReplaceFirstInstance(column.ColumnName,"Member") + "#>", row[column].ToString());
                stringBuilder.Replace("<#" + ReplaceFirstInstance(column.ColumnName, "VideoResourceFile") + "#>", row[column].ToString());
                stringBuilder.Replace("<#" + ReplaceFirstInstance(column.ColumnName, "PhotoResourceFile") + "#>", row[column].ToString());
            }

            return stringBuilder.ToString();
        }

        public static string ReplaceFirstInstance(string input, string search)
        {
            int idx = input.IndexOf(search);

            try
            {
                if (idx >= 0)
                {
                    return input.Substring(idx + search.Length);
                }
            }
            catch { }

            return input;
        }

        public static string MergeMemberInfo(string inputString, Member memberInfo)
        {
            StringBuilder stringBuilder = new StringBuilder(inputString);
            stringBuilder.Replace("<#DOB#>", memberInfo.DOB.ToShortDateString());
            stringBuilder.Replace("<#Email#>", memberInfo.Email);
            stringBuilder.Replace("<#FirstName#>", memberInfo.FirstName);            
            stringBuilder.Replace("<#LastName#>", memberInfo.LastName);
            stringBuilder.Replace("<#NickName#>", memberInfo.NickName);
            stringBuilder.Replace("<#ZipPostcode#>", memberInfo.ZipPostcode);
            stringBuilder.Replace("<#Password#>", memberInfo.Password);
            stringBuilder.Replace("<#WebMemberID#>", memberInfo.WebMemberID);

            return stringBuilder.ToString();
        }

        public static string MergeOtherMemberInfo(string inputString, Member memberInfo)
        {
            StringBuilder stringBuilder = new StringBuilder(inputString);
            stringBuilder.Replace("<#OtherDOB#>", memberInfo.DOB.ToShortDateString());
            stringBuilder.Replace("<#OtherEmail#>", memberInfo.Email);
            stringBuilder.Replace("<#OtherFirstName#>", memberInfo.FirstName);
            stringBuilder.Replace("<#OtherLastName#>", memberInfo.LastName);
            stringBuilder.Replace("<#OtherNickName#>", memberInfo.NickName);
            stringBuilder.Replace("<#OtherZipPostcode#>", memberInfo.ZipPostcode);

            return stringBuilder.ToString();
        }

        public static string MergeContentType(string inputString, ContentType contentType)
        {
            StringBuilder stringBuilder = new StringBuilder(inputString);
            stringBuilder.Replace("<#ContentType#>", contentType.ToString());
            return stringBuilder.ToString();
        }

        public static string MergeReferral(string inputString, int Number, string referralString)
        {
            StringBuilder stringBuilder = new StringBuilder(inputString);
            stringBuilder.Replace("<#Referral" + Number.ToString() + "#>", referralString);
            return stringBuilder.ToString();
        }

        public static string MergeBanner(string body,string type,SqlConnection conn)
        {
            return body.Replace("<#Banner#",Utility.GetBanner(type,conn));
        }
    }
}
