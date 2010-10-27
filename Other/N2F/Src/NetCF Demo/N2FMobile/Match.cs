using System;

using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;

namespace goAbout.Engine
{
    /// <summary>
    /// Match object is a matched Member and therefore inherits from the Member object
    /// and contains only a minor additional Properties. 
    /// </summary>
	public class Match : Member
    {
        #region Properties

        private DateTime matchDT;
        private string macAddress;
        private bool confirmedByServer = false;
        private bool hasMyProfile = false;

        /// <summary>
        /// The Date & time of the Match
        /// </summary>
        public DateTime MatchDT
        {
            get { return matchDT; }
            set { matchDT = value; }
        }

        /// <summary>
        ///  The Bluetooth MAC address of the device
        /// </summary>
        public string MACAddress
        {
            get { return macAddress; }
            set { macAddress = value; }
        }

        /// <summary>
        /// True if the Match has been confirmed by the central server
        /// </summary>
        public bool ConfirmedByServer
        {
            get { return confirmedByServer; }
            set { confirmedByServer = value; }
        }

        /// <summary>
        /// The Match already has my profile
        /// </summary>
        public bool HasMyProfile
        {
            get { return hasMyProfile; }
            set { hasMyProfile = value; }
        }
        #endregion

        /// <summary>
        /// Create a new Match object
        /// </summary>
        /// <param name="name">The MemberID of the Match</param>
        public Match(string MemberID)
		{
            this.MemberID = MemberID;
			this.MatchDT=DateTime.Now;
		}

        /// <summary>
        /// returns the MemberID as a string
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return this.MemberID;
		}
	}
}
