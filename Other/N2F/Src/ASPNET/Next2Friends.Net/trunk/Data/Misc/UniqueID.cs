using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

namespace Next2Friends.Misc
{
    public class UniqueID
    {
        /// <summary>
        /// A WebID is a 22 character string from a guid
        /// http://www.Next2Friends.com/v=?A73D8BA73EF8F7C670n7df344
        /// </summary>
        /// <returns></returns>
        public static string NewWebID()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(guid);

            return Convert.ToBase64String(bytes).Substring(0, 8);
            //return Convert.ToBase64String(bytes).Substring(0, 22);
        }

        /// <summary>
        /// A WebID is a 22 character string from a guid
        /// http://www.Next2Friends.com/v=?A73D8BA73EF8F7C670n7df344
        /// </summary>
        /// <returns></returns>
        public static bool IsWebID(string WebID)
        {
            string pattern = @"^[a-zA-Z0-9]+$";

            Regex reg = new Regex(pattern);

            return reg.IsMatch(WebID);
        }



        /// <summary>
        /// Creates an 8 byte encryption key for a device
        /// </summary>
        /// <returns></returns>
        public static string NewEncryptionKey()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(guid);

            Random rand = new Random();
            int StartIndex = rand.Next(0, 14);

            // create a random guid and then choose a random section of the 
            string EncKey = Convert.ToBase64String(bytes).Substring(StartIndex, 8);

            return EncKey;
        }

        /// <summary>
        /// A StrongID consists of 3 Guids combined (seperated by forward slash). The StrongID is used for referencing private objects on a public server to ensure
        /// that noone could protentially brute force random requests and guess
        /// http://www.Next2Friends.com/v=?A73D8BA73EF8F7CA73D8BA73EF8F7C/A73D8BA73EF8F7CA73D8BA73EF8F7C/A73D8BA73EF8F7CA73D8BA73EF8F7C
        /// </summary>
        /// <returns></returns>
        public static string NewStrongID()
        {
            

            string StrongID = Guid.NewGuid().ToString().Replace("-", string.Empty) + "/";
            StrongID += Guid.NewGuid().ToString().Replace("-", string.Empty) + "/";
            StrongID += Guid.NewGuid().ToString().Replace("-", string.Empty) + "/";

            return StrongID;
        }


    }
}
