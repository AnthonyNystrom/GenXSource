using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public enum MemberOrderBy {FirstName=1,LastName=2,NickName=3,Online=4 }

    public partial class Friend
    {
        public static Friend[] GetAllFriendsByMemberID(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("[HG_GetAllFriendsByMemberID]");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<Friend> friends = new List<Friend>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                friends = Friend.PopulateObject(dr);

                dr.Close();
            }

            return friends.ToArray();
        }

        public static bool IsFriend(int MemberID1,int MemberID2)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("[HG_IsFriend]");
            db.AddInParameter(dbCommand, "MemberID1", DbType.Int32, MemberID1);
            db.AddInParameter(dbCommand, "MemberID2", DbType.Int32, MemberID2);

            int rowCount = (int)db.ExecuteScalar(dbCommand);

            if (rowCount > 0)
                return true;
            
            return false;
        }

        public static int GetMutualFriendCount(Member Member1, Member Member2)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("[HG_GetMutualFriendCount]");
            db.AddInParameter(dbCommand, "MemberID1", DbType.Int32, Member1.MemberID);
            db.AddInParameter(dbCommand, "MemberID2", DbType.Int32, Member2.MemberID);

            int MutualFriendCount = 0;

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    MutualFriendCount = dr.GetInt32(0);
                }
                dr.Close();
            }

            return MutualFriendCount;
        }

        public static List<Member> GetMutualFriends(Member Member1, Member Member2)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("[HG_GetMutualFriends]");
            db.AddInParameter(dbCommand, "MemberID1", DbType.Int32, Member1.MemberID);
            db.AddInParameter(dbCommand, "MemberID2", DbType.Int32, Member2.MemberID);

            List<Member> MutualFriends = new List<Member>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                MutualFriends = Member.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            return MutualFriends;
        }


        public static string UnblockFriend(int MemberID, string BlockWebMemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("UnblockFriend");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "BlockWebMemberID", DbType.String, BlockWebMemberID);

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                dr.Close();
            }

            return BlockWebMemberID;
        }

        public static void AddFriend(int MemberID, int FriendMemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_AddFriend");
            db.AddInParameter(dbCommand, "@MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "@FriendMemberID", DbType.Int32, FriendMemberID);

            db.ExecuteNonQuery(dbCommand);
        }


        public static Friend[] GetAllFriendsByMemberIDWithJoin(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAllFriendsByMemberIDWithJoin");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<Friend> friends = new List<Friend>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                friends = Friend.PopulateObjectWithJoin(dr);

                dr.Close();
            }

            return friends.ToArray();
        }


        public static void UnFriend(Member Member, Member friend)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_UnFriend");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, Member.MemberID);
            db.AddInParameter(dbCommand, "FriendMemberID", DbType.Int32, friend.MemberID);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

    }
}
