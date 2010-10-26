/////////////////////////////////////////////////////////////////////////////////
// Paint.NET
// Copyright (C) Rick Brewster, Chris Crosetto, Dennis Dietrich, Tom Jackson, 
//               Michael Kelsey, Brandon Ortiz, Craig Taylor, Chris Trevino, 
//               and Luke Walker
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.
/////////////////////////////////////////////////////////////////////////////////

// NOTE: This exe MUST be built to target the x86 CPU platform, NOT x64 or 'Any'.

using System;
using System.Collections.Generic;
using System.Text;
using Genetibase.NuGenVisiCalc;

namespace WiaProxy32
{
    class WiaProxy32
    {
        const String helpText = @"WiaProxy32 for NuGenVisiCalc

Purpose:
NuGenVisiCalc uses the Windows Image Acquisition Automation Layer, which is
contained in wiaaut.dll. This DLL requires Windows XP SP1 or newer. However,
try as we might we could not find a 64-bit version of this DLL. So, in order
to get printing and scanning for 64-bit Paint.NET, we have implemented this
out-of-process proxy for wiaaut.dll that implements the functionality
required by the PaintDotNet.SystemLayer.ScanningAndPrinting class.

Command line:
    wiaproxy32 [IsComponentAvailable 1 | 
                CanPrint 1 | 
                CanScan 1 | 
                Print <filename> | 
                Scan <filename>]

IsComponentAvailable, CanPrint, CanScan
    These implement the properties for the ScanningAndPrinting class. Return
    codes are -1 for error, 0 for false and 1 for true. For these, you must
    specify a second parameter. In the syntax above, it is listed as '1'.

Print <filename>
    Presents the printing user interface. The image to be printed is
    <filename>. Return code is -1 for error, or 0.

Scan <filename>
    Presents the scanning user interface. The is scanned or acquired and put
    into the given filename. Return codes are -1 for error, or one of the
    values in the ScanResult enumeration.";

        static Int32 Main(String[] args)
        {
            Int32 exitCode;

            try
            {
                exitCode = MainImpl(args);
            }

            catch
            {
                exitCode = -1;
            }

            return exitCode;
        }

        static Int32 MainImpl(String[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine(helpText);
                return 0;
            }

            Console.WriteLine(args[0] + " " + args[1]);

            Int32 exitCode;

            switch (args[0])
            {
                case "IsComponentAvailable":
                    exitCode = IsComponentAvailable ? 1 : 0;
                    break;

                case "CanPrint":
                    exitCode = CanPrint ? 1 : 0;
                    break;

                case "CanScan":
                    exitCode = CanScan ? 1 : 0;
                    break;

                case "Print":
                    Print(args[1]);
                    exitCode = 0;
                    break;

                case "Scan":
                    exitCode = (Int32)Scan(args[1]);
                    break;

                default:
                    exitCode = -1;
                    break;
            }

            return exitCode;
        }

        private static Boolean IsComponentAvailable
        {
            get
            {
                return IsWia2Available();
            }
        }

        private static Boolean CanPrint
        {
            get
            {
                return IsWia2Available();
            }
        }

        private static Boolean CanScan
        {
            get
            {
                if (IsWia2Available())
                {
                    WIA.DeviceManagerClass dmc = new WIA.DeviceManagerClass();

                    if (dmc.DeviceInfos.Count > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private static void Print(String fileName)
        {
            WIA.VectorClass vector = new WIA.VectorClass();
            Object tempName_o = (Object)fileName;
            vector.Add(ref tempName_o, 0);
            Object vector_o = (Object)vector;
            WIA.CommonDialogClass cdc = new WIA.CommonDialogClass();            
            cdc.ShowPhotoPrintingWizard(ref vector_o);
        }

        private static ScanResult Scan(String fileName)
        {
            ScanResult result;

            WIA.CommonDialogClass cdc = new WIA.CommonDialogClass();
            WIA.ImageFile imageFile = null;

            try
            {
                imageFile = cdc.ShowAcquireImage(WIA.WiaDeviceType.UnspecifiedDeviceType,
                                                 WIA.WiaImageIntent.UnspecifiedIntent,
                                                 WIA.WiaImageBias.MaximizeQuality,
                                                 "{00000000-0000-0000-0000-000000000000}",
                                                 true,
                                                 true,
                                                 false);
            }

            catch (System.Runtime.InteropServices.COMException)
            {
                result = ScanResult.DeviceBusy;
                imageFile = null;
            }

            if (imageFile != null)
            {
                imageFile.SaveFile(fileName);
                result = ScanResult.Success;
            }
            else
            {
                result = ScanResult.UserCancelled;
            }

            return result;
        }

        // Have to split this in to two functions because the WIA DLL is resolved
        // at the time a function is entered that depends on it (IsWia2AvailableImpl).
        // This way we can avoid a runtime error and maintain the semantics of
        // IsWia2Available().

        private static Boolean IsWia2Available()
        {
            try
            {
                return IsWia2AvailableImpl();
            }

            catch
            {
                return false;
            }
        }

        private static Boolean IsWia2AvailableImpl()
        {
            try
            {
                WIA.DeviceManagerClass dmc = new WIA.DeviceManagerClass();
                return true;
            }

            catch
            {
                return false;
            }
        }
    }
}
