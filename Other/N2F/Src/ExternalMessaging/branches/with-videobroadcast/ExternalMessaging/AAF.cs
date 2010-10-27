using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ExternalMessaging
{
    public class AskAFriend
    {
        public static DataSet GetAskAFriendInformation(int AskAFriendID,SqlConnection conn)
        {   
            SqlCommand command = new SqlCommand("AG_GetAskAFriendByAskAFriendID", conn);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter("@AskAFriendID", AskAFriendID);
            param.DbType = DbType.Int32;
            param.Direction = ParameterDirection.Input;

            command.Parameters.Add(param);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            return ds;
        }
    }
}
