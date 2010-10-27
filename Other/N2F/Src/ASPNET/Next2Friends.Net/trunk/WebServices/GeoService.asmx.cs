/* ------------------------------------------------
 * GeoService.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Services;

using Next2Friends.WebServices.GeoMessage;
using Next2Friends.WebServices.GeoMessage.Data;
using Next2Friends.WebServices.GeoMessage.Databases;
using Next2Friends.WebServices.GeoMessage.Tables;
using Next2Friends.WebServices.Properties;

using tables = Next2Friends.WebServices.GeoMessage.Tables;

namespace Next2Friends.WebServices
{
    /// <summary>
    /// Provides functionality to set geographically dependent messages.
    /// </summary>
    [WebService(
        Description = "Provides functionality to set geographically dependent messages.",
        Name = "GeoMessage",
        Namespace = "http://www.next2friends.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public sealed class GeoService : WebService
    {
        /// <summary>
        /// Adds the message that will be sent to my friends and/or to myself.
        /// </summary>
        /// <param name="webMemberID">Specifies the user identifier.</param>
        /// <param name="password">Specifies the user password.</param>
        /// <param name="message">Specifies the message to add.</param>
        /// <param name="base64PhotoString">Specifies the image to associate with this message. Can be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">
        /// If the specified <c>webMemberID</c> is <c>null</c>, or if the specified <c>password</c> is <c>null</c>, or
        /// if the specified <c>message</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the value of the <c>CurrentLocation</c> property on <c>message</c> instance is <c>null</c>, or
        /// if the value of the <c>CurrentLocation.QualifiedCoordinates</c> property on <c>message</c> instance is <c>null</c>, or
        /// if the value of the <c>Receivers</c> property on <c>message</c> instance is <c>null</c>, or
        /// if the user with the specified <c>webMemberID</c> and <c>password</c> does not exist.
        /// </exception>
        [WebMethod(
            Description = "Adds the message that will be sent to my friends and/or to myself.<br/>" +
            "<br/><b>Params:</b>" +
            "<br/><tt>webMemberID</tt> - Specifies the user identifier." +
            "<br/><tt>password</tt> - Specifies the user password." +
            "<br/><tt>message</tt> - Specifies the message to add." +
            "<br/><ul>" +
            "<li><tt>MessageType</tt> - The following values are allowed and can be combined:<br/>1 - for the message that will be sent when the target enters the area.<br/>2 - for the message that will be sent when the target leaves the area.</li>" +
            "<li><tt>MeasureUnits</tt> - Measure units for <tt>Spot</tt>. The following values are allowed:<br/>1 - Feets<br/>2 - Meters</li>" +
            "<li><tt>Spot</tt> - Radius in feets or meters to indicate the area the message will be valid within.</li>" +
            "<li><tt>ShouldRepeat</tt> - Indicates whether the message will be repeated or initiated only once.</li>" +
            "<li><tt>RepeatTimes</tt> - Number of times the message will be repeated. Ignored if <tt>ShouldRepeat</tt> is <tt>false</tt>. Specify <tt>0</tt> to always repeat this message.</li>" +
            "<li><tt>CurrentLocation</tt> - Current user location. <a href=\"http://www.forum.nokia.com/document/Java_ME_Developers_Library_v2/GUID-4AEC8DAF-DDCC-4A30-B820-23F2BA60EA52/javax/microedition/location/Location.html\">More info</a>." +
            "<ul>" +
            "<li><tt>AddressInfo</tt> - Current address associated data. <a href=\"http://www.forum.nokia.com/document/Java_ME_Developers_Library_v2/GUID-4AEC8DAF-DDCC-4A30-B820-23F2BA60EA52/javax/microedition/location/AddressInfo.html\">More info</a>.</li>" +
            "<li><tt>Course</tt> - Course made good in degrees relative to true north. The value is always in the range [0.0,360.0) degrees.</li>" +
            "<li><tt>ExtraInfo</tt> - Extra information about the location.</li>" +
            "<li><tt>LocationMethod</tt> - Information about the location method used.</li>" +
            "<li><tt>QualifiedCoordinates</tt> - Coordinates of this location and their accuracy. <a href=\"http://www.forum.nokia.com/document/Java_ME_Developers_Library_v2/GUID-4AEC8DAF-DDCC-4A30-B820-23F2BA60EA52/javax/microedition/location/QualifiedCoordinates.html\">More info</a>.</li>" +
            "<li><tt>Speed</tt> - Current ground speed in meters per second (m/s) at the time of measurement. The speed is always a non-negative value. Note that unlike the coordinates, speed does not have an associated accuracy because the methods used to determine the speed typically are not able to indicate the accuracy.</li>" +
            "<li><tt>TimeStamp</tt> - Time stamp at which the data was collected. This timestamp should represent the point in time when the measurements were made and should be in ticks that are standard in the Microsoft .NET Framework.</li>" +
            "<li><tt>IsValid</tt> - Value indicating whether this Location instance represents a valid location with coordinates or an invalid one where all the data, especially the latitude and longitude coordinates, may not be present. A valid Location object contains valid coordinates whereas an invalid Location object may not contain valid coordinates but may contain <c>ExtraInfo</c> to provide information on why it was not possible to provide a valid Location object.</li>" +
            "</ul></li>" +
            "<li><tt>Receivers</tt> - Specifies the list of members that will receive this message.</li>" +
            "<li><tt>MessageText</tt> - Specifies the text for this message.</li>" +
            "<br/></ul>" +
            "<br/><tt>base64PhotoString</tt> - Specifies the image to associate with this message. Can be <tt>null</tt>." +
            "<br/><br/><b>Throws:</b>" +
            "<br/><tt>ArgumentNullException</tt> - If the specified <tt>webMemberID</tt> is <tt>null</tt>, or if the specified <tt>password</tt> is <tt>null</tt>, or if the specified <tt>message</tt> is <tt>null</tt>." +
            "<br/><tt>ArgumentException</tt> - If the value of the <tt>CurrentLocation</tt> property on <tt>message</tt> instance is <tt>null</tt>, or if the value of the <tt>CurrentLocation.QualifiedCoordinates</tt> property on <tt>message</tt> instance is <tt>null</tt>, or if the value of the <tt>Receivers</tt> property on <tt>message</tt> instance is <tt>null</tt>, or if the user with the specified <tt>webMemberID</tt> and <tt>password</tt> does not exist.")]
        public void AddMessage(
            String webMemberID,
            String password,
            Message message,
            String base64PhotoString
            )
        {
            if (webMemberID == null)
                throw new ArgumentNullException("webMemberID");
            if (password == null)
                throw new ArgumentNullException("password");
            if (message == null)
                throw new ArgumentNullException("message");

            if (message.CurrentLocation == null)
                throw new ArgumentException(Resources.Argument_CurrentLocationIsNull);
            if (message.CurrentLocation.QualifiedCoordinates == null)
                throw new ArgumentException(Resources.Argument_QualifiedCoordinatesIsNull);
            if (message.Receivers == null)
                throw new ArgumentException(Resources.Argument_ReceiversIsNull);

            GeoMessageQualifiedCoordinates coordinates = CreateQualifiedCoordinatesRecord(message.CurrentLocation.QualifiedCoordinates);
            GeoMessageAddressInfo addressInfo = CreateAddressInfoRecord(message.CurrentLocation.AddressInfo);

            SqlConnection connection = null;
            SqlTransaction transaction = null;

            try
            {
                connection = N2FDatabase.OpenConnection();
                transaction = connection.BeginTransaction();

                Int32 memberID = GetMemberID(webMemberID, password, connection, transaction);

                Int32? qualifiedCoordinatesID = null;
                Int32? addressInfoID = null;
                Int32? locationID = null;
                Int32? messageID = null;

                qualifiedCoordinatesID = SqlQueryBuilder.Insert<GeoMessageQualifiedCoordinates>(coordinates, connection, transaction);
                if (!qualifiedCoordinatesID.HasValue)
                    throw new InvalidOperationException(String.Format(Resources.InvalidOperation_InsertRecord, "GeoMessageQualifiedCoordinates"));

                if (addressInfo != null)
                    addressInfoID = SqlQueryBuilder.Insert<GeoMessageAddressInfo>(addressInfo, connection, transaction);

                GeoMessageLocation location = CreateLocationRecord(message.CurrentLocation);
                location.AddressInfoID = addressInfoID;
                location.QualifiedCoordinatesID = qualifiedCoordinatesID.Value;

                locationID = SqlQueryBuilder.Insert<GeoMessageLocation>(location, connection, transaction);
                if (!locationID.HasValue)
                    throw new InvalidOperationException(String.Format(Resources.InvalidOperation_InsertRecord, "GeoMessageLocation"));

                tables.GeoMessage messageTable = CreateGeoMessageRecord(message);
                messageTable.LocationID = locationID.Value;

                messageID = SqlQueryBuilder.Insert<tables.GeoMessage>(messageTable, connection, transaction);
                if (!messageID.HasValue)
                    throw new InvalidOperationException(String.Format(Resources.InvalidOperation_InsertRecord, "GeoMessage"));

                foreach (Member member in message.Receivers)
                {
                    try
                    {
                        var messageMemberInfo = new GeoMessageMemberInfo()
                        {
                            MessageID = messageID.Value,
                            MessageCreatorID = memberID,
                            MessageReceiverID = GetFriendID(member.ID, connection, transaction)
                        };

                        if (!SqlQueryBuilder.Insert<GeoMessageMemberInfo>(messageMemberInfo, connection, transaction).HasValue)
                            throw new InvalidOperationException(String.Format(Resources.InvalidOperation_InsertRecord, "GeoMessageMemberInfo"));
                    }
                    catch (ArgumentException e)
                    {
                        Debug.WriteLine(e.Message, "GetFriendID");
                        Debug.WriteLine(e.StackTrace, "GetFriendID");
                        continue;
                    }
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                if (transaction != null)
                    transaction.Rollback();

                Debug.WriteLine(e.Message, "AddMessage");
                Debug.WriteLine(e.StackTrace, "AddMessage");
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        /// <summary>
        /// Edits the messages that is already set. For example a user may change the friend the message is for. Can only edit messages that are created by the member that is intending to edit the message.
        /// </summary>
        /// <param name="webMemberID">Specifies the user identifier.</param>
        /// <param name="password">Specifies the user password.</param>
        /// <param name="messageID">Specifies the identifier of the message to edit.</param>
        /// <param name="message">Specifies the edited message.</param>
        /// <param name="base64PhotoString">Specifies the image to associate with this message. Can be <c>null</c>.</param>
        /// <exception cref="ArgumentException">
        /// If the identifier of the message creator does not match <c>webMemberID</c> that is the identifier of the user that is intending to edit the message, or
        /// if the value of the <c>CurrentLocation</c> property on <c>message</c> instance is <c>null</c>, or
        /// if the value of the <c>CurrentLocation.QualifiedCoordinates</c> property on <c>message</c> instance is <c>null</c>, or
        /// if the value of the <c>Receivers</c> property on <c>message</c> instance is <c>null</c>, or
        /// if the user with the specified <c>webMemberID</c> and <c>password</c> does not exist.
        /// </exception>
        [WebMethod(Description = "Edits the messages that is already set. For example a user may change the friend the message is for.<br/>" +
            "<br/><b>Params:</b>" +
            "<br/><tt>webMemberID</tt> - Specifies the user identifier." +
            "<br/><tt>password</tt> - Specifies the user password" +
            "<br/><tt>messageID</tt> - Specifies the identifier of the message to edit." +
            "<br/><tt>message</tt> - Specifies the message to add." +
            "<br/><ul>" +
            "<li><tt>MessageType</tt> - The following values are allowed and can be combined:<br/>1 - for the message that will be sent when the target enters the area.<br/>2 - for the message that will be sent when the target leaves the area.</li>" +
            "<li><tt>MeasureUnits</tt> - Measure units for <tt>Spot</tt>. The following values are allowed:<br/>1 - Feets<br/>2 - Meters</li>" +
            "<li><tt>Spot</tt> - Radius in feets or meters to indicate the area the message will be valid within.</li>" +
            "<li><tt>ShouldRepeat</tt> - Indicates whether the message will be repeated or initiated only once.</li>" +
            "<li><tt>RepeatTimes</tt> - Number of times the message will be repeated. Ignored if <tt>ShouldRepeat</tt> is <tt>false</tt>. Specify <tt>0</tt> to always repeat this message.</li>" +
            "<li><tt>CurrentLocation</tt> - Current user location. <a href=\"http://www.forum.nokia.com/document/Java_ME_Developers_Library_v2/GUID-4AEC8DAF-DDCC-4A30-B820-23F2BA60EA52/javax/microedition/location/Location.html\">More info</a>." +
            "<ul>" +
            "<li><tt>AddressInfo</tt> - Current address associated data. <a href=\"http://www.forum.nokia.com/document/Java_ME_Developers_Library_v2/GUID-4AEC8DAF-DDCC-4A30-B820-23F2BA60EA52/javax/microedition/location/AddressInfo.html\">More info</a>.</li>" +
            "<li><tt>Course</tt> - Course made good in degrees relative to true north. The value is always in the range [0.0,360.0) degrees.</li>" +
            "<li><tt>ExtraInfo</tt> - Extra information about the location.</li>" +
            "<li><tt>LocationMethod</tt> - Information about the location method used.</li>" +
            "<li><tt>QualifiedCoordinates</tt> - Coordinates of this location and their accuracy. <a href=\"http://www.forum.nokia.com/document/Java_ME_Developers_Library_v2/GUID-4AEC8DAF-DDCC-4A30-B820-23F2BA60EA52/javax/microedition/location/QualifiedCoordinates.html\">More info</a>.</li>" +
            "<li><tt>Speed</tt> - Current ground speed in meters per second (m/s) at the time of measurement. The speed is always a non-negative value. Note that unlike the coordinates, speed does not have an associated accuracy because the methods used to determine the speed typically are not able to indicate the accuracy.</li>" +
            "<li><tt>TimeStamp</tt> - Time stamp at which the data was collected. This timestamp should represent the point in time when the measurements were made and should be in ticks that are standard in the Microsoft .NET Framework.</li>" +
            "<li><tt>IsValid</tt> - Value indicating whether this Location instance represents a valid location with coordinates or an invalid one where all the data, especially the latitude and longitude coordinates, may not be present. A valid Location object contains valid coordinates whereas an invalid Location object may not contain valid coordinates but may contain <c>ExtraInfo</c> to provide information on why it was not possible to provide a valid Location object.</li>" +
            "</ul></li>" +
            "<li><tt>Receivers</tt> - Specifies the list of friends this message is for.</li>" +
            "<li><tt>MessageText</tt> - Specifies the text for this message.</li>" +
            "</ul>" +
            "<br/><tt>base64PhotoString</tt> - Specifies the image to associate with this message. Can be <tt>null</tt>." +
            "<br/><br/><b>Throws:</b>" +
            "<br/><tt>ArgumentException</tt> - If the identifier of the message creator does not match <tt>webMemberID</tt> that is the identifier of the user that is intending to edit the message." +
            "<br/><tt>ArgumentNullException</tt> - If the specified <tt>webMemberID</tt> is <tt>null</tt>, or if the specified <tt>password</tt> is <tt>null</tt>, or if the specified <tt>messageID</tt> is <tt>null</tt>, or if the specified <tt>message</tt> is <tt>null</tt>." +
            "<br/><tt>ArgumentException</tt> - If the value of the <tt>CurrentLocation</tt> property on <tt>message</tt> instance is <tt>null</tt>, or if the value of the <tt>CurrentLocation.QualifiedCoordinates</tt> property on <tt>message</tt> instance is <tt>null</tt>, or if the value of the <tt>Receivers</tt> property on <tt>message</tt> instance is <tt>null</tt>, or if the user with the specified <tt>webMemberID</tt> and <tt>password</tt> does not exist.")]
        public void EditMessage(
            String webMemberID,
            String password,
            Int32 messageID,
            Message message,
            String base64PhotoString
            )
        {
            if (webMemberID == null)
                throw new ArgumentNullException("webMemberID");
            if (password == null)
                throw new ArgumentNullException("password");

            if (message.CurrentLocation == null)
                throw new ArgumentException(Resources.Argument_CurrentLocationIsNull);
            if (message.CurrentLocation.QualifiedCoordinates == null)
                throw new ArgumentException(Resources.Argument_QualifiedCoordinatesIsNull);
            if (message.Receivers == null)
                throw new ArgumentException(Resources.Argument_ReceiversIsNull);

            GeoMessageQualifiedCoordinates coordinates = CreateQualifiedCoordinatesRecord(message.CurrentLocation.QualifiedCoordinates);
            GeoMessageAddressInfo addressInfo = CreateAddressInfoRecord(message.CurrentLocation.AddressInfo);

            SqlConnection connection = null;
            SqlTransaction transaction = null;

            try
            {
                connection = N2FDatabase.OpenConnection();
                transaction = connection.BeginTransaction();

                Int32 memberID = GetMemberID(webMemberID, password, connection, transaction);
                if (!CheckMessageExists(messageID, connection, transaction))
                    throw new ArgumentException(Resources.Argument_MessageNotExist);
                if (!CheckMessageCreator(memberID, messageID, connection, transaction))
                    throw new ArgumentException(Resources.Argument_BadMessageEditor);

                Int32 locationID = GetLocationID(messageID, connection, transaction);
                coordinates.ID = GetQualifiedCoordinatesID(locationID, connection, transaction);
                SqlQueryBuilder.Update<GeoMessageQualifiedCoordinates>(coordinates, connection, transaction);

                if (addressInfo != null)
                {
                    Int32? addressInfoIDRaw = GetAddressInfoID(coordinates.ID, connection, transaction);
                    addressInfo.ID = addressInfoIDRaw.Value;
                    SqlQueryBuilder.Update<GeoMessageAddressInfo>(addressInfo, connection, transaction);
                }

                GeoMessageLocation location = CreateLocationRecord(message.CurrentLocation);
                location.ID = locationID;
                SqlQueryBuilder.Update<GeoMessageLocation>(location, connection, transaction);

                tables.GeoMessage messageTable = CreateGeoMessageRecord(message);
                messageTable.LocationID = locationID;
                SqlQueryBuilder.Update<tables.GeoMessage>(messageTable, connection, transaction);

                RemoveMessageReceivers(messageID, connection, transaction);

                foreach (Member member in message.Receivers)
                {
                    try
                    {
                        var messageMemberInfo = new GeoMessageMemberInfo()
                        {
                            MessageID = messageID,
                            MessageCreatorID = memberID,
                            MessageReceiverID = GetFriendID(member.ID, connection, transaction)
                        };

                        if (!SqlQueryBuilder.Insert<GeoMessageMemberInfo>(messageMemberInfo, connection, transaction).HasValue)
                            throw new InvalidOperationException(String.Format(Resources.InvalidOperation_InsertRecord, "GeoMessageMemberInfo"));
                    }
                    catch (ArgumentException e)
                    {
                        Debug.WriteLine(e.Message, "GetFriendID");
                        Debug.WriteLine(e.StackTrace, "GetFriendID");
                        continue;
                    }
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                if (transaction != null)
                    transaction.Rollback();

                Debug.WriteLine(e.Message, "EditMessage");
                Debug.WriteLine(e.StackTrace, "EditMessage");
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        /// <summary>
        /// Removes the specified message. Useful if the message is out-of-date for example. Can remove only messages that are created by the member that is intending to remove the message.
        /// </summary>
        /// <param name="webMemberID">Specifies the user identifier.</param>
        /// <param name="password">Specifies the user password.</param>
        /// <param name="messageID">Specifies the identifier of the message to remove.</param>
        /// <exception cref="ArgumentException">
        /// If the identifier of the message creator does not match <c>webMemberID</c> that is the identifier of the user that is intending to remove the message, or
        /// if the message with the specified <c>messageID</c> does not exist, or
        /// if the user with the specified <c>webMemberID</c> and <c>password</c> does not exist.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If the specified <c>webMemberID</c> is <c>null</c>, or
        /// if the specified <c>password</c> is <c>null</c>, or
        /// if the specified <c>messageID</c> is <c>null</c>.
        /// </exception>
        [WebMethod(
            Description = "Removes the specified message. Useful if the message is out-of-date for example.<br/>" +
            "<br/><b>Params:</b>" +
            "<br/><tt>webMemberID</tt> - Specifies the user identifier." +
            "<br/><tt>password</tt> - Specifies the user password." +
            "<br/><tt>messageID</tt> - Specifies the identifier of the message to remove." +
            "<br/><br/><b>Throws:</b>" +
            "<br/><tt>ArgumentException</tt> - If the identifier of the message creator does not match <tt>webMemberID</tt> that is the identifier of the user that is intending to remove the message, or if the message with the specified <tt>messageID</tt> does not exist, or if the user with the specified <tt>webMemberID</tt> and <tt>password</tt> does not exist." +
            "<br/><tt>ArgumentNullException</tt> - If the specified <tt>webMemberID</tt> is <tt>null</tt>, or if the specified <tt>password</tt> is <tt>null</tt>.")]
        public void RemoveMessage(
            String webMemberID,
            String password,
            Int32 messageID
            )
        {
            if (webMemberID == null)
                throw new ArgumentNullException("webMemberID");
            if (password == null)
                throw new ArgumentNullException("password");

            SqlConnection connection = null;
            SqlTransaction transaction = null;

            try
            {
                connection = N2FDatabase.OpenConnection();
                transaction = connection.BeginTransaction();

                Int32 memberID = GetMemberID(webMemberID, password, connection, transaction);
                if (!CheckMessageExists(messageID, connection, transaction))
                    throw new ArgumentException(Resources.Argument_MessageNotExist);
                if (!CheckMessageCreator(memberID, messageID, connection, transaction))
                    throw new ArgumentException(Resources.Argument_BadMessageRemover);

                SqlCommand cmd = new SqlCommand("delete from GeoMessageMemberInfo where MessageID = @messageID", connection, transaction);
                cmd.Parameters.AddWithValue("@messageID", messageID);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "select LocationID from GeoMessage where GeoMessageID = @messageID";
                Int32 locationID = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText = "delete from GeoMessage where GeoMessageID = @messageID";
                cmd.ExecuteNonQuery();

                Int32? addressInfoID = null;
                Int32 qualifiedCoordinatesID = 0;

                cmd = new SqlCommand("select AddressInfoID, QualifiedCoordinatesID from GeoMessageLocation where GeoMessageLocationID = @locationID", connection, transaction);
                cmd.Parameters.AddWithValue("@locationID", locationID);
                SqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        Object addressInfoIDRaw = reader[0];
                        if (!(addressInfoIDRaw is System.DBNull))
                            addressInfoID = Convert.ToInt32(addressInfoIDRaw);
                        qualifiedCoordinatesID = (Int32)reader[1];
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                cmd.CommandText = "delete from GeoMessageLocation where GeoMessageLocationID = @locationID";
                cmd.ExecuteNonQuery();

                if (addressInfoID.HasValue)
                {
                    cmd = new SqlCommand("delete from GeoMessageAddressInfo where GeoMessageAddressInfoID = @addressInfoID", connection, transaction);
                    cmd.Parameters.AddWithValue("@addressInfoID", addressInfoID.Value);
                    cmd.ExecuteNonQuery();
                }

                cmd = new SqlCommand("delete from GeoMessageQualifiedCoordinates where GeoMessageQualifiedCoordinatesID = @qualifiedCoordinatesID", connection, transaction);
                cmd.Parameters.AddWithValue("@qualifiedCoordinatesID", qualifiedCoordinatesID);
                cmd.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception e)
            {
                if (transaction != null)
                    transaction.Rollback();

                Debug.WriteLine(e.Message, "RemoveMessage");
                Debug.WriteLine(e.StackTrace, "RemoveMessage");
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        /// <summary>
        /// Retrieves a list of messages I receive from my friends or myself.
        /// </summary>
        /// <param name="webMemberID">Specifies the user identifier.</param>
        /// <param name="password">Specifies the user password.</param>
        /// <param name="lastMessageID">Specifies the identifier of the latest message. All messages that are newer will be retrieved.
        /// If <c>null</c> all available messages will be retrieved.</param>
        /// <returns>An array of descriptors to contain brief message identifiers, associated text, friend identifiers,
        /// and human-readable names for each friend identifier. Returns <c>null</c> if there are no messages available.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <c>webMemberID</c> is <c>null</c>, or if the specified <c>password</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the user with the specified <c>webMemberID</c> and <c>password</c> does not exist.
        /// </exception>
        [WebMethod(
            Description = "Retrieves a list of messages I receive from my friends or myself.<br/>" +
            "<br/><b>Params:</b>" +
            "<br/><tt>webMemberID</tt> - Specifies the user identifier." +
            "<br/><tt>password</tt> - Specifies the user password." +
            "<br/><tt>lastMessageID</tt> - Specifies the identifier of the latest message. All messages that are newer will be retrieved. If <tt>null</tt> all available messages will be retrieved." +
            "<br/><br/><b>Returns:</b> " +
            "An array of descriptors to contain brief message identifiers, associated text, friend identifiers, and human-readable names for each friend identifier." +
            "Returns <tt>null</tt> if there are no messages available." +
            "<br/><br/><b>Throws:</b>" +
            "<br/><tt>ArgumentNullException</tt> - If the specified <tt>webMemberID</tt> is <tt>null</tt>, or if the specified <tt>password</tt> is <tt>null</tt>." +
            "<br/><tt>ArgumentException</tt> - If the user with the specified <tt>webMemberID</tt> and <tt>password</tt> does not exist.")]
        public BriefMessage[] GetMessagesIReceive(
            String webMemberID,
            String password,
            Int32 lastMessageID
            )
        {
            return GetMessages(webMemberID, password, lastMessageID, GetMessagesIReceiveIDs);
        }

        /// <summary>
        /// Retrieves a list of messages I created using the <c>AddMessage</c> method.
        /// </summary>
        /// <param name="webMemberID">Specifies the user identifier.</param>
        /// <param name="password">Specifies the user password.</param>
        /// <param name="lastMessageID">Specifies the identifier of the latest message. All messages that are newer will be retrieved.
        /// If <c>null</c> all available messages will be retrieved.</param>
        /// <returns>An array of descriptors to contain brief message identifiers, associated text, friend identifiers,
        /// and human-readable names for each friend identifier. Returns <c>null</c> if there are no messages available.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <c>webMemberID</c> is <c>null</c>, or if the specified <c>password</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the user with the specified <c>webMemberID</c> and <c>password</c> does not exist.
        /// </exception>
        [WebMethod(
            Description = "Retrieves a list of messages I created using the <tt>AddMessage</tt> method.<br/>" +
            "<br/><b>Params:</b>" +
            "<br/><tt>webMemberID</tt> - Specifies the user identifier." +
            "<br/><tt>webPassword</tt> - Specifies the user password." +
            "<br/><tt>lastMessageID</tt> - Specifies the identifier of the latest message. All messages that are newer will be retrieved. If <tt>null</tt> all available messages will be retrieved." +
            "<br/><br/><b>Returns:</b> " +
            "An array of descriptors to contain brief message identifiers, associated text, friend identifiers, and human-readable names for each friend identifier." +
            "Returns <tt>null</tt> if there are no messages available." +
            "<br/><br/><b>Throws:</b>" +
            "<br/><tt>ArgumentNullException</tt> - If the specified <tt>webMemberID</tt> is <tt>null</tt>, or if the specified <tt>password</tt> is <tt>null</tt>." +
            "<br/><tt>ArgumentException</tt> - If the user with the specified <tt>webMemberID</tt> and <tt>password</tt> does not exist.")]
        public BriefMessage[] GetMyMessages(
            String webMemberID,
            String password,
            Int32 lastMessageID
            )
        {
            return GetMessages(webMemberID, password, lastMessageID, GetMyMessagesIDs);
        }

        /// <summary>
        /// Returns the value indicating there are new messages I will receive.
        /// Client will check it periodically and notify the user if there are new messages.
        /// </summary>
        /// <param name="webMemberID">Specifies the user identifier.</param>
        /// <param name="password">Specifies the user password.</param>
        /// <param name="lastMessageID">Specifies the identifier of the latest message that was successfully retrieved by the client-side. Will check if there are messages newer than the specified one.</param>
        /// <returns>The value indicating whether there are new messages I will receive.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <c>webMemberID</c> is <c>null</c>, or if the specified <c>password</c> is <c>null</c>.
        /// </exception>
        [WebMethod(Description = "Returns the value indicating there are new messages I will receive. Client will check it periodically and notify the user if there are new messages.<br/>" +
            "<br/><b>Params:</b>" +
            "<br/><tt>webMemberID</tt> - Specifies the user identifier." +
            "<br/><tt>password</tt> - Specifies the user password." +
            "<br/><tt>lastMessageID</tt> - Specifies the identifier of the latest message that was successfully retrieved by the client-side. Will check if there are messages newer than the specified one." +
            "<br/><br/><b>Returns: </b> The value indicating whether there are new messages I will receive." +
            "<br/><br/><b>Throws:</b>" +
            "<br/><tt>ArgumentNullException</tt> - If the specified <tt>webMemberID</tt> is <tt>null</tt>, or if the specified <tt>password</tt> is <tt>null</tt>.")]
        public Boolean HasNewMessagesIReceive(
            String webMemberID,
            String password,
            Int32 lastMessageID
            )
        {
            if (webMemberID == null)
                throw new ArgumentNullException("webMemberID");
            if (password == null)
                throw new ArgumentNullException("password");

            SqlConnection connection = null;
            SqlTransaction transaction = null;
            Boolean result = false;

            try
            {
                connection = N2FDatabase.OpenConnection();
                transaction = connection.BeginTransaction();

                Int32 memberID = GetMemberID(webMemberID, password, connection, transaction);
                SqlCommand cmd = new SqlCommand(
                    "select count(MessageReceiverID) from GeoMessageMemberInfo where MessageReceiverID = @memberID and MessageID > @lastMessageID",
                    connection,
                    transaction);
                cmd.Parameters.AddWithValue("@memberID", memberID);
                cmd.Parameters.AddWithValue("@lastMessageID", lastMessageID);
                result = Convert.ToInt32(cmd.ExecuteScalar()) > 0;

                transaction.Commit();
            }
            catch (Exception e)
            {
                if (transaction != null)
                    transaction.Rollback();

                Debug.WriteLine(e.Message, "HasNewMessagesIReceive");
                Debug.WriteLine(e.StackTrace, "HasNewMessagesIReceive");
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }

            return result;
        }

        /// <summary>
        /// Retrieves message details to load wizard with data for future editing.
        /// </summary>
        /// <param name="webMemberID">Specifies the user identifier.</param>
        /// <param name="password">Specifies the user password.</param>
        /// <param name="messageID">Specifies the identifier of the message to get details for.</param>
        /// <returns>Message details.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <c>webMemberID</c> is <c>null</c>, or if the specified <c>password</c> is <c>null</c>, or if the specified <c>messageID</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the user with the specified <c>webMemberID</c> and <c>password</c> does not exist.
        /// </exception>
        [WebMethod(
            Description = "Retrieves message details to load wizard with data for future editing.<br/>" +
            "<br/><b>Params:</b>" +
            "<br/><tt>webMemberID</tt> - Specifies the user identifier." +
            "<br/><tt>password</tt> - Specifies the user password." +
            "<br/><tt>messageID</tt> - Specifies the identifier of the message to get details for." +
            "<br/><br/><b>Returns:</b> Message details." +
            "<br/><br/><b>Throws:</b>" +
            "<br/><tt>ArgumentNullException</tt> - If the specified <tt>webMemberID</tt> is <tt>null</tt>, or if the specified <tt>password</tt> is <tt>null</tt>, or if the specified <tt>messageID</tt> is <tt>null</tt>.")]
        public Message GetMessageDetails(
            String webMemberID,
            String password,
            Int32 messageID
            )
        {
            if (webMemberID == null)
                throw new ArgumentNullException("webMemberID");
            if (password == null)
                throw new ArgumentNullException("password");

            SqlConnection connection = null;
            SqlTransaction transaction = null;
            Message msg = null;

            try
            {
                connection = N2FDatabase.OpenConnection();
                transaction = connection.BeginTransaction();

                Int32 memberID = GetMemberID(webMemberID, password, connection, transaction);
                tables.GeoMessage messageTable = GetGeoMessage(messageID, connection, transaction);
                GeoMessageLocation locationTable = GetGeoMessageLocation(messageTable.LocationID, connection, transaction);

                Int32? addressInfoID = locationTable.AddressInfoID;
                GeoMessageAddressInfo addressInfoTable = null;
                if (addressInfoID.HasValue)
                    addressInfoTable = GetGeoMessageAddressInfo(addressInfoID.Value, connection, transaction);

                GeoMessageQualifiedCoordinates qualifiedCoordinatesTable = GetGeoMessageQualifiedCoordinates(
                    locationTable.QualifiedCoordinatesID,
                    connection,
                    transaction);

                AddressInfo addressInfo = null;
                if (addressInfoTable != null)
                    addressInfo = CreateAddressInfoInstance(addressInfoTable);

                QualifiedCoordinates qualifiedCoordinates = CreateQualifiedCoordinatesInstance(qualifiedCoordinatesTable);
                Location location = CreateLocationInstance(locationTable);
                location.AddressInfo = addressInfo;
                location.QualifiedCoordinates = qualifiedCoordinates;

                msg = CreateMessageInstance(messageTable);
                msg.CurrentLocation = location;
                msg.Receivers = GetMessageReceivers(messageID, connection, transaction);

                transaction.Commit();
            }
            catch (Exception e)
            {
                if (transaction != null)
                    transaction.Rollback();

                Debug.WriteLine(e.Message, "GetMessageDetails");
                Debug.WriteLine(e.StackTrace, "GetMessageDetails");
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }

            return msg;
        }

        /// <summary>
        /// Returns the image (if any) that is attached to the message with the specified identifier.
        /// </summary>
        /// <param name="webMemberID">Specifies the user identifier.</param>
        /// <param name="password">Specifies the user password.</param>
        /// <param name="messageID">Specifies the identifier of the message to retrieve the attached image for.</param>
        /// <returns>Attached image. Returns <c>null</c> if no image is attached to the specified message.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <c>webMemberID</c> is <c>null</c>, or if the specified <c>password</c> is <c>null</c>, or if the specified <c>messageID</c> is <c>null</c>.
        /// </exception>
        [WebMethod(Description = "Returns the image (if any) that is attached to the message with the specified identifier.<br/>" +
            "<br/><b>Params:</b>" +
            "<br/><tt>webMemberID</tt> - Specifies the user identifier." +
            "<br/><tt>password</tt> - Specifies the user password." +
            "<br/><tt>messageID</tt> - Specifies the identifier of the message to retrieve the attached image for." +
            "<br/><br/><b>Returns:</b> Attached image. Returns <tt>null</tt> if no image is attached to the specified message." +
            "<br/><br/><b>Throws:</b>" +
            "<br/><tt>ArgumentNullException</tt> - If the specified <tt>webMemberID</tt> is <tt>null</tt>, or if the specified <tt>password</tt> is <tt>null</tt>, or if the specified <tt>messageID</tt> is <tt>null</tt>.")]
        public String GetAttachedImage(
            String webMemberID,
            String password,
            String messageID
            )
        {
            if (webMemberID == null)
                throw new ArgumentNullException("webMemberID");
            if (password == null)
                throw new ArgumentNullException("password");
            if (messageID == null)
                throw new ArgumentNullException("messageID");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a list of friends for the current user.
        /// </summary>
        /// <param name="webMemberID">Specifies the user identifier.</param>
        /// <param name="password">Specifies the user password.</param>
        /// <returns>An array of friends. Returns an empty array if there are no friends available.</returns>
        /// <exception cref="ArgumentException">If the member with the specified <c>webMemberID</c> and <c>password</c> does not exist.</exception>
        /// <exception cref="ArgumentNullException">
        /// If the specified <c>webMemberID</c> is <c>null</c>, or if the specified <c>password</c> is <c>null</c>.
        /// </exception>
        [WebMethod(
            Description = "Retrieves a list of friends for the current user.<br/>" +
            "<br/><b>Params:</b>" +
            "<br/><tt>webMemberID</tt> - Specifies the user identifier." +
            "<br/><tt>password</tt> - Specifies the user password." +
            "<br/><br/><b>Returns:</b> " +
            "An array of friends. Returns an empty array if there are no friends available." +
            "<br/><br/><b>Throws:</b>" +
            "<br/><tt>ArgumentException</tt> - If the member with the specified <tt>webMemberID</tt> and <tt>password</tt> does not exist." +
            "<br/><tt>ArgumentNullException</tt> - If the specified <tt>webMemberID</tt> is <tt>null</tt>, or if the specified <tt>password</tt> is <tt>null</tt>.")]
        public Member[] GetFriends(
            String webMemberID,
            String password
            )
        {
            if (webMemberID == null)
                throw new ArgumentNullException("webMemberID");
            if (password == null)
                throw new ArgumentNullException("password");

            IList<Member> friends = new List<Member>();

            using (var connection = N2FDatabase.OpenConnection())
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "HG_GetGeoMessageFriends";
                cmd.Parameters.AddWithValue("@webMemberID", webMemberID);
                cmd.Parameters.AddWithValue("@password", password);
                SqlParameter returnValueParam = cmd.Parameters.Add("return_value", SqlDbType.Int);
                returnValueParam.Direction = ParameterDirection.ReturnValue;

                SqlDataReader reader = cmd.ExecuteReader();

                if (returnValueParam.Value<Int32>() == -1)
                    throw new ArgumentException(Resources.Argument_BadCredentials);

                try
                {
                    while (reader.Read())
                        friends.Add(
                            new Member(
                                (String)reader[0] /* WebMemberID */,
                                (String)reader[1] /* NickName */));
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            Member[] result = new Member[friends.Count];
            friends.CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// Unsubscribes the specified user from the message with the specified <c>messageID</c>. Can be safely called several times.
        /// If the user exists and the message with the specified <c>messageID</c> exists, no exceptions will be thrown if the user
        /// is already unsubscribed from the message or was never subscribed to.
        /// </summary>
        /// <param name="webMemberID"></param>
        /// <param name="password"></param>
        /// <param name="messageID"></param>
        [WebMethod(
            Description = "Unsubscribes the specified user from the message with the specified <tt>messageID</tt>. Can be safely called several times. If the user exists and the message with the specified <tt>memberID</tt> exists, no exceptions will be thrown if the user is already unsubscribed from the message or was never subscribed to.<br/>" +
            "<br/><b>Params:</b>" +
            "<br/><tt>webMemberID</tt> - Specifies the user identifier." +
            "<br/><tt>password</tt> - Specifies the user password." +
            "<br/><tt>messageID</tt> - Specifies the identifier of the message to remove." +
            "<br/><br/><b>Throws:</b>" +
            "<br/><tt>ArgumentException</tt> - If the message with the specified <tt>messageID</tt> does not exist, or if the user with the specified <tt>webMemberID</tt> and <tt>password</tt> does not exist." +
            "<br/><tt>ArgumentNullException</tt> - If the specified <tt>webMemberID</tt> is <tt>null</tt>, or if the specified <tt>password</tt> is <tt>null</tt>.")]
        public void Unsubscribe(
            String webMemberID,
            String password,
            Int32 messageID
            )
        {
            if (webMemberID == null)
                throw new ArgumentNullException("webMemberID");
            if (password == null)
                throw new ArgumentNullException("password");

            SqlConnection connection = null;
            SqlTransaction transaction = null;

            try
            {
                connection = N2FDatabase.OpenConnection();
                transaction = connection.BeginTransaction();

                Int32 memberID = GetMemberID(webMemberID, password, connection, transaction);
                if (!CheckMessageExists(messageID, connection, transaction))
                    throw new ArgumentException(Resources.Argument_MessageNotExist);

                SqlCommand cmd = new SqlCommand("delete from GeoMessageMemberInfo where MessageID = @messageID and MessageReceiverID = @memberID", connection, transaction);
                cmd.Parameters.AddWithValue("@messageID", messageID);
                cmd.Parameters.AddWithValue("@memberID", memberID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                if (transaction != null)
                    transaction.Rollback();

                Debug.WriteLine(e.Message, "Unsubscribe");
                Debug.WriteLine(e.StackTrace, "Unsubscribe");
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        private static Boolean CheckMessageCreator(Int32 memberID, Int32 messageID, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand cmd = new SqlCommand("select distinct MessageCreatorID from GeoMessageMemberInfo where MessageID = @messageID", connection, transaction);
            cmd.Parameters.AddWithValue("@messageID", messageID);
            Object creatorIDRaw = cmd.ExecuteScalar();
            if (creatorIDRaw == null || Convert.ToInt32(creatorIDRaw) != memberID)
                return false;
            return true;
        }

        private static Boolean CheckMessageExists(Int32 messageID, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand cmd = new SqlCommand("select count(GeoMessageID) from GeoMessage where GeoMessageID = @messageID", connection, transaction);
            cmd.Parameters.AddWithValue("@messageID", messageID);
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }

        private static AddressInfo CreateAddressInfoInstance(GeoMessageAddressInfo table)
        {
            return new AddressInfo()
            {
                BuildingFloor = table.BuildingFloor,
                BuildingName = table.BuildingName,
                BuildingRoom = table.BuildingRoom,
                BuildingZone = table.BuildingZone,
                City = table.City,
                Country = table.Country,
                CountryCode = table.CountryCode,
                County = table.County,
                Crossing1 = table.Crossing1,
                Crossing2 = table.Crossing2,
                District = table.District,
                Extension = table.Extension,
                PhoneNumber = table.PhoneNumber,
                PostalCode = table.PostalCode,
                State = table.State,
                Street = table.Street,
                Url = table.Url
            };
        }

        private static GeoMessageAddressInfo CreateAddressInfoRecord(AddressInfo addressInfo)
        {
            if (addressInfo == null)
                return null;

            return new GeoMessageAddressInfo()
            {
                BuildingFloor = addressInfo.BuildingFloor,
                BuildingName = addressInfo.BuildingName,
                BuildingRoom = addressInfo.BuildingRoom,
                BuildingZone = addressInfo.BuildingZone,
                City = addressInfo.City,
                Country = addressInfo.Country,
                CountryCode = addressInfo.CountryCode,
                County = addressInfo.County,
                Crossing1 = addressInfo.Crossing1,
                Crossing2 = addressInfo.Crossing2,
                District = addressInfo.District,
                Extension = addressInfo.Extension,
                PhoneNumber = addressInfo.PhoneNumber,
                PostalCode = addressInfo.PostalCode,
                State = addressInfo.State,
                Street = addressInfo.Street,
                Url = addressInfo.Url
            };
        }

        private static Location CreateLocationInstance(GeoMessageLocation table)
        {
            return new Location()
            {
                Course = table.Course,
                ExtraInfo = table.ExtraInfo,
                IsValid = table.IsValid,
                LocationMethod = table.LocationMethod,
                Speed = table.Speed,
                TimeStamp = table.TimeStamp.Value.Ticks
            };
        }

        private static GeoMessageLocation CreateLocationRecord(Location location)
        {
            return new GeoMessageLocation()
            {
                Course = location.Course,
                ExtraInfo = location.ExtraInfo,
                IsValid = location.IsValid,
                LocationMethod = location.LocationMethod,
                Speed = location.Speed,
                TimeStamp = new DateTime(location.TimeStamp)
            };
        }

        private static Message CreateMessageInstance(tables.GeoMessage table)
        {
            var msg = new Message()
            {
                MessageText = table.MessageText,
                RepeatTimes = table.RepeatTimes,
                ShouldRepeat = table.ShouldRepeat,
                Spot = table.Spot
            };

            switch (table.MeasureUnits)
            {
                case 1:
                    msg.MeasureUnits = MeasureUnits.Feets;
                    break;
                case 2:
                    msg.MeasureUnits = MeasureUnits.Meters;
                    break;
                default:
                    throw new ArgumentException(Resources.Argument_BadMeasureUnits);
            }

            Int32 messageTypeRaw = table.MessageType;

            if ((messageTypeRaw & MessageType.EntersTheArea) != MessageType.None)
                msg.MessageType |= MessageType.EntersTheArea;
            if ((messageTypeRaw & MessageType.LeavesTheArea) != MessageType.None)
                msg.MessageType |= MessageType.EntersTheArea;

            if (msg.MessageType == MessageType.None)
                throw new ArgumentException(Resources.Argument_BadMessageType);

            return msg;
        }

        private static tables.GeoMessage CreateGeoMessageRecord(Message msg)
        {
            return new tables.GeoMessage()
            {
                MeasureUnits = msg.MeasureUnits,
                MessageText = msg.MessageText,
                MessageType = msg.MessageType,
                RepeatTimes = msg.RepeatTimes,
                ShouldRepeat = msg.ShouldRepeat,
                Spot = msg.Spot
            };
        }

        private static QualifiedCoordinates CreateQualifiedCoordinatesInstance(GeoMessageQualifiedCoordinates table)
        {
            return new QualifiedCoordinates()
            {
                Altitude = table.Altitude,
                HorizontalAccuracy = table.HorizontalAccuracy,
                Latitude = table.Latitude,
                Longitude = table.Longitude,
                VerticalAccuracy = table.VerticalAccuracy
            };
        }

        private static GeoMessageQualifiedCoordinates CreateQualifiedCoordinatesRecord(QualifiedCoordinates coordinates)
        {
            return new GeoMessageQualifiedCoordinates()
            {
                Altitude = coordinates.Altitude,
                Latitude = coordinates.Latitude,
                Longitude = coordinates.Longitude,
                HorizontalAccuracy = coordinates.HorizontalAccuracy,
                VerticalAccuracy = coordinates.VerticalAccuracy
            };
        }

        private static Int32 GetMemberID(String webMemberID, String password, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "HG_GetGeoMessageMemberID";
            cmd.Parameters.AddWithValue("@webMemberID", webMemberID);
            cmd.Parameters.AddWithValue("@password", password);
            SqlParameter memberIDOutput = cmd.Parameters.Add("@memberID", SqlDbType.Int);
            memberIDOutput.Direction = ParameterDirection.Output;
            SqlParameter returnValueParam = cmd.Parameters.Add("return_value", SqlDbType.Int);
            returnValueParam.Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();

            if (returnValueParam.Value<Int32>() == -1)
                throw new ArgumentException(Resources.Argument_BadCredentials);

            return cmd.Parameters["@memberID"].Value<Int32>();
        }

        /// <summary>
        /// </summary>
        /// <param name="webMemberID"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If the specified <c>webMemberID</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If the user with the specified <c>webMemberID</c> does not exist.</exception>
        private static Int32 GetFriendID(String webMemberID, SqlConnection connection, SqlTransaction transaction)
        {
            if (webMemberID == null)
                throw new ArgumentNullException("webMemberID");

            var cmd = new SqlCommand("HG_GetGeoMessageFriendID", connection, transaction);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@webMemberID", webMemberID);
            SqlParameter friendIDOutput = cmd.Parameters.Add("@friendID", SqlDbType.Int);
            friendIDOutput.Direction = ParameterDirection.Output;
            SqlParameter returnValueParam = cmd.Parameters.Add("return_value", SqlDbType.Int);
            returnValueParam.Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();

            if (returnValueParam.Value<Int32>() == -1)
                throw new ArgumentException(String.Format(Resources.Argument_MemberNotExist, webMemberID));

            return cmd.Parameters["@friendID"].Value<Int32>();
        }

        private static GeoMessageAddressInfo GetGeoMessageAddressInfo(Int32 addressInfoID, SqlConnection connection, SqlTransaction transaction)
        {
            var addressInfo = new GeoMessageAddressInfo();

            SqlCommand cmd = new SqlCommand(
                "select Extension, Street, PostalCode, City, County, State, Country, CountryCode, District, BuildingName, BuildingFloor, BuildingRoom, BuildingZone, Crossing1, Crossing2, Url, PhoneNumber from GeoMessageAddressInfo where GeoMessageAddressInfoID = @addressInfoID",
                connection,
                transaction);
            cmd.Parameters.AddWithValue("@addressInfoID", addressInfoID);
            SqlDataReader reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    String extension = Convert.ToString(reader[0]);
                    String street = Convert.ToString(reader[1]);
                    String postalCode = Convert.ToString(reader[2]);
                    String city = Convert.ToString(reader[3]);
                    String county = Convert.ToString(reader[4]);
                    String state = Convert.ToString(reader[5]);
                    String country = Convert.ToString(reader[6]);
                    String countryCode = Convert.ToString(reader[7]);
                    String district = Convert.ToString(reader[8]);
                    String buildingName = Convert.ToString(reader[9]);
                    String buildingFloor = Convert.ToString(reader[10]);
                    String buildingRoom = Convert.ToString(reader[11]);
                    String buildingZone = Convert.ToString(reader[12]);
                    String crossing1 = Convert.ToString(reader[13]);
                    String crossing2 = Convert.ToString(reader[14]);
                    String url = Convert.ToString(reader[15]);
                    String phoneNumber = Convert.ToString(reader[16]);

                    addressInfo.BuildingFloor = buildingFloor;
                    addressInfo.BuildingName = buildingName;
                    addressInfo.BuildingRoom = buildingRoom;
                    addressInfo.BuildingZone = buildingZone;
                    addressInfo.City = city;
                    addressInfo.Country = country;
                    addressInfo.CountryCode = countryCode;
                    addressInfo.County = county;
                    addressInfo.Crossing1 = crossing1;
                    addressInfo.Crossing2 = crossing2;
                    addressInfo.District = district;
                    addressInfo.Extension = extension;
                    addressInfo.ID = addressInfoID;
                    addressInfo.PhoneNumber = phoneNumber;
                    addressInfo.PostalCode = postalCode;
                    addressInfo.State = state;
                    addressInfo.Street = street;
                    addressInfo.Url = url;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return addressInfo;
        }

        private static GeoMessageLocation GetGeoMessageLocation(Int32 locationID, SqlConnection connection, SqlTransaction transaction)
        {
            var location = new GeoMessageLocation();

            SqlCommand cmd = new SqlCommand(
                "select AddressInfoID, Course, LocationMethod, QualifiedCoordinatesID, Speed, TimeStamp, IsValid, ExtraInfo from GeoMessageLocation where GeoMessageLocationID = @locationID",
                connection,
                transaction);
            cmd.Parameters.AddWithValue("@locationID", locationID);
            SqlDataReader reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Int32? addressInfo = null;
                    Object addressInfoRaw = reader[0];
                    if (!(addressInfoRaw is DBNull))
                        addressInfo = Convert.ToInt32(addressInfoRaw);

                    Single course = Single.NaN;
                    Object courseRaw = reader[1];
                    if (!(courseRaw is DBNull))
                        course = Convert.ToSingle(courseRaw);

                    Int32 locationMethod = Convert.ToInt32(reader[2]);
                    Int32 qualifiedCoordinatesID = Convert.ToInt32(reader[3]);

                    Single speed = Single.NaN;
                    Object speedRaw = reader[4];
                    if (!(speedRaw is DBNull))
                        speed = Convert.ToSingle(speedRaw);

                    DateTime timeStamp = Convert.ToDateTime(reader[5]);
                    Boolean isValid = Convert.ToBoolean(reader[6]);
                    String extraInfo = Convert.ToString(reader[7]);

                    location.AddressInfoID = addressInfo;
                    location.Course = course;
                    location.ExtraInfo = extraInfo;
                    location.ID = locationID;
                    location.IsValid = isValid;
                    location.LocationMethod = locationMethod;
                    location.QualifiedCoordinatesID = qualifiedCoordinatesID;
                    location.Speed = speed;
                    location.TimeStamp = timeStamp;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return location;
        }

        private static GeoMessageQualifiedCoordinates GetGeoMessageQualifiedCoordinates(Int32 qualifiedCoordinatesID, SqlConnection connection, SqlTransaction transaction)
        {
            var result = new GeoMessageQualifiedCoordinates();

            SqlCommand cmd = new SqlCommand(
                "select Latitude, Longitude, Altitude, HorizontalAccuracy, VerticalAccuracy from GeoMessageQualifiedCoordinates where GeoMessageQualifiedCoordinatesID = @qualifiedCoordinatesID",
                connection,
                transaction);
            cmd.Parameters.AddWithValue("@qualifiedCoordinatesID", qualifiedCoordinatesID);
            SqlDataReader reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Double latitude = Convert.ToDouble(reader[0]);
                    Double longitude = Convert.ToDouble(reader[1]);

                    Single altitude = Single.NaN;
                    Object altitudeRaw = reader[2];
                    if (!(altitudeRaw is DBNull))
                        altitude = Convert.ToSingle(altitudeRaw);

                    Single horizontalAccuracy = Single.NaN;
                    Object horizontalAccuracyRaw = reader[3];
                    if (!(horizontalAccuracyRaw is DBNull))
                        horizontalAccuracy = Convert.ToSingle(horizontalAccuracyRaw);

                    Single verticalAccuracy = Single.NaN;
                    Object verticalAccuracyRaw = reader[4];
                    if (!(verticalAccuracyRaw is DBNull))
                        verticalAccuracy = Convert.ToSingle(verticalAccuracyRaw);

                    result.Altitude = altitude;
                    result.HorizontalAccuracy = horizontalAccuracy;
                    result.ID = qualifiedCoordinatesID;
                    result.Latitude = latitude;
                    result.Longitude = longitude;
                    result.VerticalAccuracy = verticalAccuracy;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return result;
        }

        private static tables.GeoMessage GetGeoMessage(Int32 messageID, SqlConnection connection, SqlTransaction transaction)
        {
            var result = new tables.GeoMessage();

            SqlCommand cmd = new SqlCommand(
                "select MessageType, MeasureUnits, Spot, ShouldRepeat, RepeatTimes, LocationID, MessageText, DTCreated from GeoMessage where GeoMessageID = @messageID",
                connection,
                transaction);
            cmd.Parameters.AddWithValue("@messageID", messageID);
            SqlDataReader reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Int32 messageType = Convert.ToInt32(reader[0]);
                    Int32 measureUnits = Convert.ToInt32(reader[1]);
                    Single spot = Convert.ToSingle(reader[2]);
                    Boolean shouldRepeat = Convert.ToBoolean(reader[3]);
                    Int32 repeatTimes = Convert.ToInt32(reader[4]);
                    Int32 locationID = Convert.ToInt32(reader[5]);
                    String messageText = Convert.ToString(reader[6]);
                    DateTime dtCreated = Convert.ToDateTime(reader[7]);

                    result.DTCreated = dtCreated;
                    result.ID = messageID;
                    result.LocationID = locationID;
                    result.MeasureUnits = measureUnits;
                    result.MessageText = messageText;
                    result.MessageType = messageType;
                    result.RepeatTimes = repeatTimes;
                    result.ShouldRepeat = shouldRepeat;
                    result.Spot = spot;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return result;
        }

        private static IList<Int32> GetMessageIDs(SqlCommand command)
        {
            IList<Int32> result = new List<Int32>();
            SqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                    result.Add(Convert.ToInt32(reader[0]));
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return result;
        }

        private static IList<Int32> GetMessagesIReceiveIDs(Int32 memberID, Int32 lastMessageID, SqlConnection connection, SqlTransaction transaction)
        {
            var cmd = new SqlCommand(
                "select MessageID from GeoMessageMemberInfo where MessageReceiverID = @memberID and MessageID > @lastMessageID",
                connection,
                transaction);
            cmd.Parameters.AddWithValue("@memberID", memberID);
            cmd.Parameters.AddWithValue("@lastMessageID", lastMessageID);
            return GetMessageIDs(cmd);
        }

        private static IList<Int32> GetMyMessagesIDs(Int32 memberID, Int32 lastMessageID, SqlConnection connection, SqlTransaction transaction)
        {
            var cmd = new SqlCommand(
                "select distinct MessageID from GeoMessageMemberInfo where MessageCreatorID = @memberID and MessageID > @lastMessageID",
                connection,
                transaction);
            cmd.Parameters.AddWithValue("@memberID", memberID);
            cmd.Parameters.AddWithValue("@lastMessageID", lastMessageID);
            return GetMessageIDs(cmd);
        }

        private static String GetMessageText(Int32 messageID, SqlConnection connection, SqlTransaction transaction)
        {
            var command = new SqlCommand("select MessageText from GeoMessage where GeoMessageID = @messageID", connection, transaction);
            command.Parameters.AddWithValue("@messageID", messageID);
            Object messageText = command.ExecuteScalar();
            return Convert.ToString(messageText);
        }

        private static Member[] GetMessageReceivers(Int32 messageID, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand cmd = new SqlCommand("HG_GetGeoMessageReceivers", connection, transaction);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@messageID", messageID);

            IList<Member> receivers = new List<Member>();
            SqlDataReader reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                    receivers.Add(
                        new Member(
                            (String)reader[0] /* WebMemberID */,
                            (String)reader[1] /* NickName */));
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            Member[] result = new Member[receivers.Count];
            receivers.CopyTo(result, 0);
            return result;
        }

        private delegate IList<Int32> MessageIDsRetriever(Int32 memberID, Int32 lastMessageID, SqlConnection connection, SqlTransaction transaction);

        private static BriefMessage[] GetMessages(String webMemberID, String password, Int32 lastMessageID, MessageIDsRetriever messageIDsRetriever)
        {
            if (webMemberID == null)
                throw new ArgumentNullException("webMemberID");
            if (password == null)
                throw new ArgumentNullException("password");

            SqlConnection connection = null;
            SqlTransaction transaction = null;
            BriefMessage[] result = null;

            try
            {
                connection = N2FDatabase.OpenConnection();
                transaction = connection.BeginTransaction();

                Int32 memberID = GetMemberID(webMemberID, password, connection, transaction);
                IList<Int32> messageIDList = messageIDsRetriever(memberID, lastMessageID, connection, transaction);
                result = new BriefMessage[messageIDList.Count];

                for (var i = 0; i < messageIDList.Count; i++)
                {
                    Int32 currentMessageID = messageIDList[i];
                    String messageText = GetMessageText(currentMessageID, connection, transaction);
                    Member[] receivers = GetMessageReceivers(currentMessageID, connection, transaction);
                    result[i] = new BriefMessage() { MessageID = currentMessageID, MessageText = messageText, Receivers = receivers };
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                if (transaction != null)
                    transaction.Rollback();

                Debug.WriteLine(e.Message, "GetMyMessages");
                Debug.WriteLine(e.StackTrace, "GetMyMessages");
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }

            return result;
        }

        private static Int32? GetAddressInfoID(Int32 locationID, SqlConnection connection, SqlTransaction transaction)
        {
            Int32? addressInfoID = null;
            SqlCommand cmd = new SqlCommand("select AddressInfoID from GeoMessageLocation where GeoMessageLocationID = @locationID", connection, transaction);
            cmd.Parameters.AddWithValue("@locationID", locationID);
            Object addressInfoIDRaw = cmd.ExecuteScalar();
            if (addressInfoIDRaw != null && !(addressInfoIDRaw is DBNull))
                addressInfoID = Convert.ToInt32(addressInfoIDRaw);
            return addressInfoID;
        }

        private static Int32 GetQualifiedCoordinatesID(Int32 locationID, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand cmd = new SqlCommand("select QualifiedCoordinatesID from GeoMessageLocation where GeoMessageLocationID = @locationID", connection, transaction);
            cmd.Parameters.AddWithValue("@locationID", locationID);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static Int32 GetLocationID(Int32 messageID, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand cmd = new SqlCommand(
                "select LocationID from GeoMessage where GeoMessageID = @messageID",
                connection,
                transaction);
            cmd.Parameters.AddWithValue("@messageID", messageID);
            Object locationIDRaw = cmd.ExecuteScalar();
            return Convert.ToInt32(locationIDRaw);
        }

        private static void RemoveMessageReceivers(Int32 messageID, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand cmd = new SqlCommand("delete from GeoMessageMemberInfo where MessageID = @messageID", connection, transaction);
            cmd.Parameters.AddWithValue("@messageID", messageID);
            cmd.ExecuteNonQuery();
        }
    }
}
