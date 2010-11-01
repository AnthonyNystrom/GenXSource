/*
 * Basic Message Class - RFC2822.
 * Revision No  Date        Notes
 *  01          12 Feb 06   Initial development. Very basic! A.Neillans
 * 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Mail {
  public class Message {
      protected string mFromAddress;
      protected string mSenderAddress;
      protected string mToAddress;
      protected string mCCAddress;
      protected string mSubject;

      protected string mContentType;
      protected string mMessageID;
      protected string mOrganisation;
      protected DateTime mDateTime;

      protected ArrayList mHeaders;
      protected ArrayList mBody;

      public Message()
      {
          // Constructor
          mBody = new ArrayList();
          mHeaders = new ArrayList();
      }

      ~Message()
      {
          // Destructor
          mHeaders = null;
          mBody = null;
      }

      // TODO : Note, we should really have a EMailAddress class that constructs the 
      // email address, like we did in Delphi Genetibase.Network. However, for speed, I haven't bothered
      // implementing it like that.

      public void ClearHeaders()
      {
          mCCAddress = "";
          mDateTime = DateTime.Now;
          mFromAddress = "";
          mOrganisation = "";
          mSubject = "";
          mContentType = "";
          mSenderAddress = "";
          mMessageID = "";
      }

      public void GenerateHeaders()
      {
          mHeaders.Clear();
          mHeaders.Add("Organization: " + mOrganisation);
          mHeaders.Add("Subject: " + mSubject);
          mHeaders.Add("Envelope To: " + mToAddress);
          mHeaders.Add("Date: " + mDateTime.ToUniversalTime().ToString());
//          mHeaders.Add("Priority: ");
          mHeaders.Add("Message-ID: " + mMessageID);
      }
      
      #region From Address Property
      public string FromAddress
      {
          get { return mFromAddress; }
          set { mFromAddress = value; }
      }
      #endregion

      #region To Address Property
      public string ToAddress
      {
          get { return mToAddress; }
          set { mToAddress = value; }
      }
      #endregion

      #region CC Address Property
      public string CCAddress
      {
          get { return mCCAddress; }
          set { mCCAddress = value; }
      }
      #endregion

      #region Sender Property
      public string SenderAddress
      {
          get { return mSenderAddress; }
          set { mSenderAddress = value; }
      }
      #endregion

      #region Organisation Property
      public string Organisation
      {
          get { return mOrganisation; }
          set { mOrganisation = value; }
      }
      #endregion

      #region Message ID Property
      public string MessageID
      {
          get { return mMessageID; }
          set { mMessageID = value; }
      }
      #endregion

      #region Content Type Property
      public string ContentType
      {
          get { return mContentType; }
          set { mContentType = value; }
      }
      #endregion

      #region Date Property
      public DateTime Date
      {
          get { return mDateTime; }
          set { mDateTime = value; }
      }
      #endregion

      #region Subject Property
      public string Subject
      {
          get { return mSubject; }
          set { mSubject = value; }
      }
      #endregion

      #region Headers Property
      public ArrayList Headers
      {
          get { return mHeaders; }
          set { mHeaders = value; }
      }
      #endregion

      #region Body Property
      public ArrayList Body
      {
          get { return mBody; }
          set { mBody = value; }
      }
      #endregion


  }
}
