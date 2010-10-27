using System;
using System.Collections.Generic;
using System.Text;

namespace Next2Friends.Data
{
    public partial class Member
    {
        public int AgeYears 
        {
            get 
            {
                return TimeDistance.GetAgeYears(this.DOB);
            }
        }


        public string CountryName 
        {
            get 
            {
                return new ISOCountry(this.ISOCountry).CountryText;
            }
        }

        public MemberProfile GetProfile()
        {
            return this.MemberProfile[0];
        }

    }
}
