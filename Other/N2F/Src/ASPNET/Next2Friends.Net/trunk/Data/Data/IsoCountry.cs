using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    /// <summary>
    /// The Countries & Country Codes
    /// </summary>
    public partial class ISOCountry
    {
        /// <summary>
        /// The constructer that takes an ISOCode
        /// </summary>
        /// <param name="ISOCode">The ISOCode of the country</param>
        public ISOCountry(string ISOCode)
        {
            db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetISOCountryByISOCode");
            db.AddInParameter(dbCommand, "ISOCode", DbType.String, ISOCode);

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {

                if (dr.Read())
                {
                    this._iSOCountryID = (int)dr["ISOCountryID"];
                    this._iSOCode = (string)dr["ISOCode"];
                    this._countryText = (string)dr["CountryText"];

                }
                else
                {
                    throw new ArgumentException(String.Format(Properties.Resources.Argument_NoISOCountry, ISOCode));
                }

                dr.Close();
            }
        }
    }
}


