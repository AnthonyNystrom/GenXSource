using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class Friend
    {
        public static string GetFriendsXML(int MemberID)
        {
            StringWriter stringWriter = new StringWriter();

            XmlTextWriter writer = new XmlTextWriter(stringWriter);

            try
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("FriendsList");

                Friend[] Friends = Friend.GetAllFriendsByMemberIDWithJoin(MemberID);

                for (int i = 0; i < Friends.Length; i++)
                {
                    writer.WriteStartElement("Friend");
                    writer.WriteValue(Friends[i].Member.NickName);
                    writer.WriteEndElement();
                }

                //List<Member> Friends = Member.GetAllMember();

                //for (int i = 0; i < Friends.Count; i++)
                //{
                //    writer.WriteStartElement("Friend");
                //    writer.WriteValue(Friends[i].NickName);
                //    writer.WriteEndElement();
                //}

                writer.WriteEndElement();

                writer.WriteEndDocument();


            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
            }

            return stringWriter.ToString();
        }


    }
}
