/////////////////////////////////////////////////////////////////////////////////
// Paint.NET
// Copyright (C) Rick Brewster, Chris Crosetto, Dennis Dietrich, Tom Jackson, 
//               Michael Kelsey, Brandon Ortiz, Craig Taylor, Chris Trevino, 
//               and Luke Walker
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Genetibase.NuGenVisiCalc
{
    /// <summary>
    /// Provides methods and properties related to scanning and printing.
    /// </summary>
    /// <remarks>
    /// Originally adapted from http://www.codeproject.com/dotnet/wiascriptingdotnet.asp
    /// </remarks>
    internal static class ScanningAndPrinting
    {
        private const String wiaProxy32ExeName = "WiaProxy32.exe";

        private static Int32 CallWiaProxy32(String args, Boolean spinEvents)
        {
            String ourPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            String proxyPath = Path.Combine(ourPath, wiaProxy32ExeName);
            ProcessStartInfo psi = new ProcessStartInfo(proxyPath, args);

            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;

            Int32 exitCode = -1;

            try
            {
                Process process = Process.Start(psi);
				process.WaitForExit();

                // Can't just use process.WaitForExit() because then the VisiCalc UI
                // will not repaint and it'll look weird because of that.
                while (!process.HasExited)
                {
                    if (spinEvents)
                    {
                        Application.DoEvents();
                    }

                    Thread.Sleep(10);
                }

                exitCode = process.ExitCode;
                process.Dispose();
            }

            catch
            {
            }

            return exitCode;
        }

        /// <summary>
        /// Gets whether or not the scanning and printing features are available without
        /// taking into account whether a scanner or printer are actually connected.
        /// </summary>
        public static Boolean IsComponentAvailable
        {
            get
            {
                return 1 == CallWiaProxy32("IsComponentAvailable 1", false);
            }
        }

        /// <summary>
        /// Gets whether printing is possible. This does not take into account whether a printer
        /// is actually connected or available, just that it is possible to print (it is possible
        /// that the printing UI has a facility for adding or loading a new printer).
        /// </summary>
        public static Boolean CanPrint
        {
            get
            {
                return 1 == CallWiaProxy32("CanPrint 1", false);
            }
        }

        /// <summary>
        /// Gets whether scanning is possible. The user must have a scanner connect for this to return true.
        /// </summary>
        /// <remarks>
        /// This also covers image acquisition from, say, a camera.
        /// </remarks>
        public static Boolean CanScan
        {
            get
            {
                return 1 == CallWiaProxy32("CanScan 1", false);
            }
        }

        /// <summary>
        /// Presents a user interface for printing the given image.
        /// </summary>
        /// <param name="owner">The parent/owner control for the UI that will be presented for printing.</param>
        /// <param name="fileName">The name of a file containing a bitmap (.BMP) to print.</param>
        public static void Print(Control owner, String fileName)
        {
            if (!CanPrint)
            {
                throw new InvalidOperationException("Printing is not available");
            }

            String fileNameExt = Path.GetExtension(fileName);
            if (String.Compare(fileNameExt, ".bmp", StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                throw new ArgumentOutOfRangeException("fileName", fileName, "can only print .bmp files");
            }

            // Disable the entire UI, otherwise it's possible to close PDN while the
            // print wizard is active! And then it crashes.
            Form ownedForm = owner.FindForm();
            Boolean[] ownedFormsEnabled = null;

            if (ownedForm != null)
            {
                ownedFormsEnabled = new Boolean[ownedForm.OwnedForms.Length];

                for (Int32 i = 0; i < ownedForm.OwnedForms.Length; ++i)
                {
                    ownedFormsEnabled[i] = ownedForm.OwnedForms[i].Enabled;
                    ownedForm.OwnedForms[i].Enabled = false;
                }

                owner.FindForm().Enabled = false;
            } 
            
            CallWiaProxy32("Print \"" + fileName + "\"", true);

            if (ownedForm != null)
            {
                for (Int32 i = 0; i < ownedForm.OwnedForms.Length; ++i)
                {
                    ownedForm.OwnedForms[i].Enabled = ownedFormsEnabled[i];
                }

                owner.FindForm().Enabled = true;
            }

            owner.FindForm().Activate();
        }

        /// <summary>
        /// Presents a user interface for scanning.
        /// </summary>
        /// <param name="fileName">
        /// The filename of where to stored the scanned/acquired image. Only valid if the return value is ScanResult.Success.
        /// </param>
        /// <returns>The result of the scanning operation.</returns>
        public static ScanResult Scan(Control owner,  String fileName)
        {
            if (!CanScan)
            {
                throw new InvalidOperationException("Scanning is not available");
            }

            // Disable the entire UI, otherwise it's possible to close PDN while the
            // print wizard is active! And then it crashes.
            Form ownedForm = owner.FindForm();
            Boolean[] ownedFormsEnabled = null;

            if (ownedForm != null)
            {
                ownedFormsEnabled = new Boolean[ownedForm.OwnedForms.Length];

                for (Int32 i = 0; i < ownedForm.OwnedForms.Length; ++i)
                {
                    ownedFormsEnabled[i] = ownedForm.OwnedForms[i].Enabled;
                    ownedForm.OwnedForms[i].Enabled = false;
                }

                owner.FindForm().Enabled = false;
            } 
            
            // Do scanning
            Int32 retVal = CallWiaProxy32("Scan \"" + fileName + "\"", true);

            // Un-disable everything
            if (ownedForm != null)
            {
                for (Int32 i = 0; i < ownedForm.OwnedForms.Length; ++i)
                {
                    ownedForm.OwnedForms[i].Enabled = ownedFormsEnabled[i];
                }

                owner.FindForm().Enabled = true;
            }

            owner.FindForm().Activate();

            // Marshal the return code
            ScanResult result = (ScanResult)retVal;

            if (!Enum.IsDefined(typeof(ScanResult), result))
            {
                throw new ApplicationException("WiaProxy32 returned an error: " + retVal.ToString());
            }

            return result;
        }
    }
}
