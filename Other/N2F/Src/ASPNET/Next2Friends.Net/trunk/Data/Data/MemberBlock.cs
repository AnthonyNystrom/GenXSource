using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
namespace Next2Friends.Data
{
    public partial class MemberBlock
    {
        /// <summary>
        ///// obselete???????
        ///// </summary>
        ///// <param name="MemberID"></param>
        ///// <returns></returns>
        //public static MemberBlockItem[] GetMemberBTBlockList(string MemberID)
        //{
        //    Database db = DatabaseFactory.CreateDatabase();
        //    DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMemberBTBlockList");
        //    db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

        //    List<MemberBlock> BTBlockList = new List<MemberBlock>();
        //    List<MemberBlockItem> BTBlockItems = new List<MemberBlockItem>();

        //    using (IDataReader dr = db.ExecuteReader(dbCommand))
        //    {
        //        BTBlockList = MemberBlock.PopulateObject(dr);
        //        dr.Close();
        //    }

        //    for (int i = 0; i < BTBlockList.Count; i++)
        //    {
        //        //BTBlockItems.

        //        MemberBlockItem blockItem = new MemberBlockItem();
        //        blockItem.MemberID = BTBlockList[i].MemberID;
        //        blockItem.BlockMemberID = BTBlockList[i].BlockMemberID;
        //        blockItem.DTCreated = BTBlockList[i].DTCreated.Ticks.ToString();

        //        BTBlockItems.Add(blockItem);
        //    }

        //    return BTBlockItems.ToArray();
        //}




        /// <summary>
        /// Deletes a BTBlock item from the database
        /// </summary>
        /// <param name="MemberID">The owner of the block item</param>
        /// <param name="BTBlockItem">The BTBlock object</param>
        public static void DeleteBTBlockItems(string MemberID, MemberBlock[] BTBlockItems)
        {
            Database db = DatabaseFactory.CreateDatabase();

            for (int i = 0; i < BTBlockItems.Length; i++)
            {
                
                DbCommand dbCommand = db.GetStoredProcCommand("HG_DeleteBTBlockItem");
                db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
                db.AddInParameter(dbCommand, "BTBlockID", DbType.Int32, BTBlockItems[i].MemberBlockID);

                db.ExecuteNonQuery(dbCommand);
            }
        }

        /// <summary>
        /// Creates a BTBlock item int the database
        /// </summary>
        /// <param name="MemberID">The owner of the block item</param>
        /// <param name="BTBlockItem">The BTBlock object</param>
        public static void CreateBTBlockItems(int MemberID, MemberBlock[] BTBlockItems)
        {

            for (int i = 0; i < BTBlockItems.Length; i++)
            {
                BTBlockItems[i].MemberID = MemberID;
                BTBlockItems[i].Save();
            }
        }


        public void SaveWithCheck()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_SaveMemberBlockWithCheck");

            db.AddInParameter(dbCommand, "MemberBlockID", DbType.Int32, MemberBlockID);
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "BlockMemberID", DbType.Int32, BlockMemberID);
            db.AddInParameter(dbCommand, "DTCreated", DbType.DateTime, DTCreated);

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {

                // get the returned ID
                if (dr.Read())
                {
                    int ID = Int32.Parse(dr[0].ToString());
                    //if the ID is NOT zero then the query was an insert
                    if (ID != 0)
                        this.MemberBlockID = ID;
                }

                dr.Close();
            }

        }
    }



}
