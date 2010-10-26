/////////////////////////////////////////////////////////////////////////////////
// Paint.NET
// Copyright (C) Rick Brewster, Chris Crosetto, Dennis Dietrich, Tom Jackson, 
//               Michael Kelsey, Brandon Ortiz, Craig Taylor, Chris Trevino, 
//               and Luke Walker
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.
/////////////////////////////////////////////////////////////////////////////////

using System;

namespace Genetibase.NuGenVisiCalc
{
    /// <summary>
    /// Defines the possible results when scanning.
    /// </summary>
    public enum ScanResult
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        Success = 1,

        /// <summary>
        /// The user cancelled the operation.
        /// </summary>
        UserCancelled = 2,

        /// <summary>
        /// The device was busy or otherwise inaccessible.
        /// </summary>
        DeviceBusy = 3
    }
}
