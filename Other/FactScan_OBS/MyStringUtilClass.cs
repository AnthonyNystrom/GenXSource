namespace FacScan
{
    using System;

    public class MyStringUtilClass
    {
        public string dup(string str, int n)
        {
            string text1 = "";
            for (int num1 = 0; num1 < n; num1++)
            {
                text1 = text1 + str;
            }
            return text1;
        }

    }
}

