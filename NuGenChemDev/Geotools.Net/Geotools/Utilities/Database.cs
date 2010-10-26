/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. 
 *
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */


#region Using
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
#endregion

namespace Geotools.Utilities
{
	/// <summary>
	/// Useful functions for reading a database.
	/// </summary>
	internal class Database
	{
		#region Methods
		
		/// <summary>
		/// Returns a data reader with results of a query.
		/// </summary>
		/// <remarks>
		/// The connection that is passed in must not be open. The behaviour of the IDataReader when the 
		/// last record is read is to close the connection. The connection cannot be re-used until the IDataReader
		/// is closed.
		/// </remarks>
		/// <param name="databaseConnection">The database connection to use when executing the query.</param>
		/// <param name="sql">The SQL to execute.</param>
		/// <returns>IDataReader with the results of a query.</returns>
		internal static IDataReader ExecuteQuery(IDbConnection databaseConnection, string sql) 
		{
			if (databaseConnection==null)
			{
				throw new ArgumentNullException("databaseConnection");
			}
			if (sql==null)
			{
				throw new ArgumentNullException(sql);
			}

			IDataReader reader = null;
			if (databaseConnection is SqlConnection)
			{
				SqlConnection connection = (SqlConnection)databaseConnection;
				SqlCommand command = new SqlCommand(sql,connection);
				connection.Open();
				reader = command.ExecuteReader(CommandBehavior.CloseConnection);
			}
			else if (databaseConnection is OleDbConnection)
			{
				OleDbConnection connection = (OleDbConnection)databaseConnection;
				OleDbCommand command = new OleDbCommand(sql,connection);
				connection.Open();
				reader = command.ExecuteReader(CommandBehavior.CloseConnection);
			}
			else
			{
				throw new ArithmeticException("Connection must be a SqlConnection or a OleDbConnection");
			}
			return reader;
			

		}
		#endregion

		#region Private Helper Methods
		/// <summary>
		/// Ensures that there are not more remaining records.
		/// </summary>
		/// <remarks>
		/// This function is used when you are expecting a one record to be returned 
		/// using a IDataReader. Once you have read the first record, this function is called. It
		/// will check to see if there are any more records. If there are more records then an 
		/// error message is thrown showing the 'code' and the 'table'. The code usually corresponds
		/// to a primary key used in the where clause of an SQL statement.
		/// </remarks>
		/// <param name="reader">The datareader to use.</param>
		/// <param name="code">The code to display in the error message.</param>
		/// <param name="table">The name of the table.</param>
		internal static void CheckOneRow(IDataReader reader, string code, string table)
		{
			if (reader==null)
			{
				throw new ArgumentNullException("reader");
			}
			// perform a read. It should return false if the last record record has already been read.
			if ( reader.Read() )
			{
				throw new InvalidOperationException(String.Format("Database had more than one record for code = {0} in {1}",code, table));
			}
			// close the reader. This will close the database connection if the command behaviour is set to CommandBehavior.CloseConnection. 
			reader.Close();
		}
		#endregion
	}
}
