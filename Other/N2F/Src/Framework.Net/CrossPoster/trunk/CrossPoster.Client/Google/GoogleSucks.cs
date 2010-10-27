/* ------------------------------------------------
 * GoogleSucks.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Next2Friends.CrossPoster.Client.Google
{
    /// <summary>
    /// Provides workarounds for Google privacy policy.
    /// </summary>
    static class GoogleSucks
    {
        private static Random _random;

        static GoogleSucks()
        {
            _random = new Random(DateTime.Now.Millisecond);
        }

        /// <summary>
        /// Returns a random application name to pass it to GData API Service constructors.
        /// </summary>
        public static String GetApplicationName()
        {
            return String.Format(
                "{0}-{1}-{2}",
                GetRandomName(_random),
                GetRandomName(_random),
                GetRandomAppVersion(_random));
        }

        private static String GetRandomName(Random random)
        {
            return GetRandomText(random, 64, 97, 122);
        }

        private static String GetRandomAppVersion(Random random)
        {
            return GetRandomText(random, 9, 48, 57);
        }

        private static String GetRandomText(Random random, Int32 length, Int32 minChar, Int32 maxChar)
        {
            var nameLength = random.Next(1, length);
            var name = new StringBuilder(nameLength);

            for (var i = 0; i < nameLength; i++)
                name.Append(Convert.ToChar(_random.Next(minChar, maxChar)));

            return name.ToString();
        }
    }
}
