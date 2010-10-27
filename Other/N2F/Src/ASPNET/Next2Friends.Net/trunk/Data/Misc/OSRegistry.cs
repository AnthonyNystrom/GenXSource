using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.ComponentModel;

namespace Next2Friends.Misc
{
    public class OSRegistry
    {

        /// <summary>
        /// gets the photo path from the registry
        /// </summary>
        /// <returns></returns>
        public static string GetPhotoDirectory()
        {
            string RegKeyName = @"SOFTWARE\Next2Friends";

            using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegKeyName))
            {
                return (string)rk.GetValue("PhotoDirectory");
            }

            throw new Win32Exception(String.Format(Next2Friends.Data.Properties.Resources.Win32_CannotFindOrAccessRegKey, "PhotoDirectory"));
        }


        /// <summary>
        /// gets the video path from the registry
        /// </summary>
        /// <returns></returns>
        public static string GetVideoDirectory()
        {
            string RegKeyName = @"SOFTWARE\Next2Friends";

            using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegKeyName))
            {
                return (string)rk.GetValue("VideoDirectory");
            }

            throw new Win32Exception(String.Format(Next2Friends.Data.Properties.Resources.Win32_CannotFindOrAccessRegKey, "VideoDirectory"));

        }

        /// <summary>
        /// gets the Save path for the user directory
        /// </summary>
        /// <returns></returns>
        public static string GetDiskUserDirectory()
        {
            return @"\\www\user\";
        }

        /// <summary>
        /// gets the Root web path from the registry
        /// </summary>
        /// <returns></returns>
        public static string GetWebRootDirectory()
        {
            string RegKeyName = @"SOFTWARE\Next2Friends";

            using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegKeyName))
            {
                return (string)rk.GetValue("WebRootDirectory");
            }

            throw new Win32Exception(String.Format(Next2Friends.Data.Properties.Resources.Win32_CannotFindOrAccessRegKey, "WebRootDirectory"));

        }
    }
}
