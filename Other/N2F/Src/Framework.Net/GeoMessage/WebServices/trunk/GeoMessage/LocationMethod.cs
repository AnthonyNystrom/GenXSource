/* ------------------------------------------------
 * LocationMethod.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;

namespace Next2Friends.GeoMessage
{
    /// <summary>
    /// Defines possible location method types.
    /// </summary>
    [Flags]
    public enum LocationMethod
    {
        /// <summary>
        /// Location method is assisted by the other party (Terminal assisted for Network based, Network assisted for terminal based).
        /// </summary>
        MTA_ASSISTED = 262144,

        /// <summary>
        /// Location method is unassisted. This bit and <c>MTA_ASSISTED</c> bit MUST NOT both be set.
        /// Only one of these bits may be set or neither to indicate that the assistance information is not known.
        /// </summary>
        MTA_UNASSISTED = 524288,

        /// <summary>
        /// Location method Angle of Arrival for cellular / terrestrial RF system.
        /// </summary>
        MTE_ANGLEOFARRIVAL = 32,

        /// <summary>
        /// Location method Cell-ID for cellular (in GSM, this is the same as CGI, Cell Global Identity).
        /// </summary>
        MTE_CELLID = 8,

        /// <summary>
        /// Location method using satellites (for example, Global Positioning System (GPS)).
        /// </summary>
        MTE_SATELLITE = 1,

        /// <summary>
        /// Location method Short-range positioning system (for example, Bluetooth LP).
        /// </summary>
        MTE_SHORTRANGE = 16,

        /// <summary>
        /// Location method Time Difference for cellular / terrestrial RF system (for example, Enhanced Observed Time Difference (E-OTD) for GSM).
        /// </summary>
        MTE_TIMEDIFFERENCE = 2,

        /// <summary>
        /// Location method Time of Arrival (TOA) for cellular / terrestrial RF system.
        /// </summary>
        MTE_TIMEOFARRIVAL = 4,

        /// <summary>
        /// Location method is of type network based. This means that the final location result is calculated in the network.
        /// This bit and <c>MTY_TERMINALBASED</c> bit MUST NOT both be set. Only one of these bits may be set or neither to indicate
        /// that it is not known where the result is calculated.
        /// </summary>
        MTY_NETWORKBASED = 131072,

        /// <summary>
        /// Location method is of type terminal based. This means that the final location result is calculated in the terminal.
        /// </summary>
        MTY_TERMINALBASED = 65536
    }
}
