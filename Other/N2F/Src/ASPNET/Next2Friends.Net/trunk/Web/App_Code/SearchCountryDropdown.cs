﻿using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace Next2Friends.WebControls
{
    public enum SearchCountryDropdownType { Signup,Search}
    /// <summary>
    /// Summary description for CountryDropdown
    /// </summary>
    public class SearchCountryDropdown : DropDownList
    {

        public SearchCountryDropdown()
        {
            Populate();

            this.Width = new Unit(178);
            
        }

        private void Populate()
        {
            this.Items.Add(new ListItem("All Countries", "-1"));
            this.Items.Add(new ListItem("United States","US"));
            this.Items.Add(new ListItem("United Kingdom", "GB"));
            this.Items.Add(new ListItem("Afghanistan","AF"));
            this.Items.Add(new ListItem("Albania","AL"));
            this.Items.Add(new ListItem("Algeria","DZ"));
            this.Items.Add(new ListItem("American Samoa","AS"));
            this.Items.Add(new ListItem("Andorra","AD"));
            this.Items.Add(new ListItem("Angola","AO"));
            this.Items.Add(new ListItem("Anguilla","AI"));
            this.Items.Add(new ListItem("Antarctica","AQ"));
            this.Items.Add(new ListItem("Antigua And Barbuda","AG"));
            this.Items.Add(new ListItem("Argentina","AR"));
            this.Items.Add(new ListItem("Armenia","AM"));
            this.Items.Add(new ListItem("Aruba","AW"));
            this.Items.Add(new ListItem("Australia","AU"));
            this.Items.Add(new ListItem("Austria","AT"));
            this.Items.Add(new ListItem("Azerbaijan","AZ"));
            this.Items.Add(new ListItem("Bahamas","BS"));
            this.Items.Add(new ListItem("Bahrain","BH"));
            this.Items.Add(new ListItem("Bangladesh","BD"));
            this.Items.Add(new ListItem("Barbados","BB"));
            this.Items.Add(new ListItem("Belarus","BY"));
            this.Items.Add(new ListItem("Belgium","BE"));
            this.Items.Add(new ListItem("Belize","BZ"));
            this.Items.Add(new ListItem("Benin","BJ"));
            this.Items.Add(new ListItem("Bermuda","BM"));
            this.Items.Add(new ListItem("Bhutan","BT"));
            this.Items.Add(new ListItem("Bolivia","BO"));
            this.Items.Add(new ListItem("Bosnia And Herzegovina","BA"));
            this.Items.Add(new ListItem("Botswana","BW"));
            this.Items.Add(new ListItem("Bouvet Island","BV"));
            this.Items.Add(new ListItem("Brazil","BR"));
            this.Items.Add(new ListItem("British Indian Ocean Territory","IO"));
            this.Items.Add(new ListItem("Brunei Darussalam","BN"));
            this.Items.Add(new ListItem("Bulgaria","BG"));
            this.Items.Add(new ListItem("Burkina Faso","BF"));
            this.Items.Add(new ListItem("Burundi","BI"));
            this.Items.Add(new ListItem("Cambodia","KH"));
            this.Items.Add(new ListItem("Cameroon","CM"));
            this.Items.Add(new ListItem("Canada","CA"));
            this.Items.Add(new ListItem("Cape Verde","CV"));
            this.Items.Add(new ListItem("Cayman Islands","KY"));
            this.Items.Add(new ListItem("Central African Republic","CF"));
            this.Items.Add(new ListItem("Chad","TD"));
            this.Items.Add(new ListItem("Chile","CL"));
            this.Items.Add(new ListItem("China","CN"));
            this.Items.Add(new ListItem("Colombia","CO"));
            this.Items.Add(new ListItem("Comoros","KM"));
            this.Items.Add(new ListItem("Congo","CG"));
            this.Items.Add(new ListItem("Congo, The Democratic Republic Of The","CD"));
            this.Items.Add(new ListItem("Cook Islands","CK"));
            this.Items.Add(new ListItem("Costa Rica","CR"));
            this.Items.Add(new ListItem("Cote D'ivoire","CI"));
            this.Items.Add(new ListItem("Croatia","HR"));
            this.Items.Add(new ListItem("Cuba","CU"));
            this.Items.Add(new ListItem("Cyprus","CY"));
            this.Items.Add(new ListItem("Czech Republic","CZ"));
            this.Items.Add(new ListItem("Denmark","DK"));
            this.Items.Add(new ListItem("Djibouti","DJ"));
            this.Items.Add(new ListItem("Dominica","DM"));
            this.Items.Add(new ListItem("Dominican Republic","DO"));
            this.Items.Add(new ListItem("Ecuador","EC"));
            this.Items.Add(new ListItem("Egypt","EG"));
            this.Items.Add(new ListItem("El Salvador","SV"));
            this.Items.Add(new ListItem("Equatorial Guinea","GQ"));
            this.Items.Add(new ListItem("Eritrea","ER"));
            this.Items.Add(new ListItem("Estonia","EE"));
            this.Items.Add(new ListItem("Ethiopia","ET"));
            this.Items.Add(new ListItem("Falkland Islands (Malvinas)","FK"));
            this.Items.Add(new ListItem("Faroe Islands","FO"));
            this.Items.Add(new ListItem("Fiji","FJ"));
            this.Items.Add(new ListItem("Finland","FI"));
            this.Items.Add(new ListItem("France","FR"));
            this.Items.Add(new ListItem("French Guiana","GF"));
            this.Items.Add(new ListItem("French Polynesia","PF"));
            this.Items.Add(new ListItem("French Southern Territories","TF"));
            this.Items.Add(new ListItem("Gabon","GA"));
            this.Items.Add(new ListItem("Gambia","GM"));
            this.Items.Add(new ListItem("Georgia","GE"));
            this.Items.Add(new ListItem("Germany","DE"));
            this.Items.Add(new ListItem("Ghana","GH"));
            this.Items.Add(new ListItem("Gibraltar","GI"));
            this.Items.Add(new ListItem("Greece","GR"));
            this.Items.Add(new ListItem("Greenland","GL"));
            this.Items.Add(new ListItem("Grenada","GD"));
            this.Items.Add(new ListItem("Guadeloupe","GP"));
            this.Items.Add(new ListItem("Guam","GU"));
            this.Items.Add(new ListItem("Guatemala","GT"));
            this.Items.Add(new ListItem("Guinea","GN"));
            this.Items.Add(new ListItem("Guinea-bissau","GW"));
            this.Items.Add(new ListItem("Guyana","GY"));
            this.Items.Add(new ListItem("Haiti","HT"));
            this.Items.Add(new ListItem("Heard Island And Mcdonald Islands","HM"));
            this.Items.Add(new ListItem("Holy See (Vatican City State)","VA"));
            this.Items.Add(new ListItem("Honduras","HN"));
            this.Items.Add(new ListItem("Hong Kong","HK"));
            this.Items.Add(new ListItem("Hungary","HU"));
            this.Items.Add(new ListItem("Iceland","IS"));
            this.Items.Add(new ListItem("India","IN"));
            this.Items.Add(new ListItem("Indonesia","ID"));
            this.Items.Add(new ListItem("Iran, Islamic Republic Of","IR"));
            this.Items.Add(new ListItem("Iraq","IQ"));
            this.Items.Add(new ListItem("Ireland","IE"));
            this.Items.Add(new ListItem("Israel","IL"));
            this.Items.Add(new ListItem("Italy","IT"));
            this.Items.Add(new ListItem("Jamaica","JM"));
            this.Items.Add(new ListItem("Japan","JP"));
            this.Items.Add(new ListItem("Jordan","JO"));
            this.Items.Add(new ListItem("Kazakhstan","KZ"));
            this.Items.Add(new ListItem("Kenya","KE"));
            this.Items.Add(new ListItem("Kiribati","KI"));
            this.Items.Add(new ListItem("Korea, North","KP"));
            this.Items.Add(new ListItem("Korea, South","KR"));
            this.Items.Add(new ListItem("Kuwait","KW"));
            this.Items.Add(new ListItem("Kyrgyzstan","KG"));
            this.Items.Add(new ListItem("Lao People's Democratic Republic","LA"));
            this.Items.Add(new ListItem("Latvia","LV"));
            this.Items.Add(new ListItem("Lebanon","LB"));
            this.Items.Add(new ListItem("Lesotho","LS"));
            this.Items.Add(new ListItem("Liberia","LR"));
            this.Items.Add(new ListItem("Libyan Arab Jamahiriya","LY"));
            this.Items.Add(new ListItem("Liechtenstein","LI"));
            this.Items.Add(new ListItem("Lithuania","LT"));
            this.Items.Add(new ListItem("Luxembourg","LU"));
            this.Items.Add(new ListItem("Macao","MO"));
            this.Items.Add(new ListItem("Macedonia, The Former Yugoslav Republic Of","MK"));
            this.Items.Add(new ListItem("Madagascar","MG"));
            this.Items.Add(new ListItem("Malawi","MW"));
            this.Items.Add(new ListItem("Malaysia","MY"));
            this.Items.Add(new ListItem("Maldives","MV"));
            this.Items.Add(new ListItem("Mali","ML"));
            this.Items.Add(new ListItem("Malta","MT"));
            this.Items.Add(new ListItem("Marshall Islands","MH"));
            this.Items.Add(new ListItem("Martinique","MQ"));
            this.Items.Add(new ListItem("Mauritania","MR"));
            this.Items.Add(new ListItem("Mauritius","MU"));
            this.Items.Add(new ListItem("Mayotte","YT"));
            this.Items.Add(new ListItem("Mexico","MX"));
            this.Items.Add(new ListItem("Micronesia, Federated States Of","FM"));
            this.Items.Add(new ListItem("Moldova, Republic Of","MD"));
            this.Items.Add(new ListItem("Monaco","MC"));
            this.Items.Add(new ListItem("Mongolia","MN"));
            this.Items.Add(new ListItem("Montserrat","MS"));
            this.Items.Add(new ListItem("Morocco","MA"));
            this.Items.Add(new ListItem("Mozambique","MZ"));
            this.Items.Add(new ListItem("Myanmar","MM"));
            this.Items.Add(new ListItem("Namibia","NA"));
            this.Items.Add(new ListItem("Nauru","NR"));
            this.Items.Add(new ListItem("Nepal","NP"));
            this.Items.Add(new ListItem("Netherlands","NL"));
            this.Items.Add(new ListItem("Netherlands Antilles","AN"));
            this.Items.Add(new ListItem("New Caledonia","NC"));
            this.Items.Add(new ListItem("New Zealand","NZ"));
            this.Items.Add(new ListItem("Nicaragua","NI"));
            this.Items.Add(new ListItem("Niger","NE"));
            this.Items.Add(new ListItem("Nigeria","NG"));
            this.Items.Add(new ListItem("Niue","NU"));
            this.Items.Add(new ListItem("Norfolk Island","NF"));
            this.Items.Add(new ListItem("Northern Mariana Islands","MP"));
            this.Items.Add(new ListItem("Norway","NO"));
            this.Items.Add(new ListItem("Oman","OM"));
            this.Items.Add(new ListItem("Pakistan","PK"));
            this.Items.Add(new ListItem("Palau","PW"));
            this.Items.Add(new ListItem("Palestinian Territory, Occupied","PS"));
            this.Items.Add(new ListItem("Panama","PA"));
            this.Items.Add(new ListItem("Papua New Guinea","PG"));
            this.Items.Add(new ListItem("Paraguay","PY"));
            this.Items.Add(new ListItem("Peru","PE"));
            this.Items.Add(new ListItem("Philippines","PH"));
            this.Items.Add(new ListItem("Pitcairn","PN"));
            this.Items.Add(new ListItem("Poland","PL"));
            this.Items.Add(new ListItem("Portugal","PT"));
            this.Items.Add(new ListItem("Puerto Rico","PR"));
            this.Items.Add(new ListItem("Qatar","QA"));
            this.Items.Add(new ListItem("Reunion","RE"));
            this.Items.Add(new ListItem("Romania","RO"));
            this.Items.Add(new ListItem("Russian Federation","RU"));
            this.Items.Add(new ListItem("Rwanda","RW"));
            this.Items.Add(new ListItem("Saint Kitts And Nevis","KN"));
            this.Items.Add(new ListItem("Saint Lucia","LC"));
            this.Items.Add(new ListItem("Saint Vincent And The Grenadines","VC"));
            this.Items.Add(new ListItem("Samoa","WS"));
            this.Items.Add(new ListItem("San Marino","SM"));
            this.Items.Add(new ListItem("Sao Tome And Principe","ST"));
            this.Items.Add(new ListItem("Saudi Arabia","SA"));
            this.Items.Add(new ListItem("Senegal","SN"));
            this.Items.Add(new ListItem("Serbia And Montenegro","CS"));
            this.Items.Add(new ListItem("Seychelles","SC"));
            this.Items.Add(new ListItem("Sierra Leone","SL"));
            this.Items.Add(new ListItem("Singapore","SG"));
            this.Items.Add(new ListItem("Slovakia","SK"));
            this.Items.Add(new ListItem("Slovenia","SI"));
            this.Items.Add(new ListItem("Solomon Islands","SB"));
            this.Items.Add(new ListItem("Somalia","SO"));
            this.Items.Add(new ListItem("South Africa","ZA"));
            this.Items.Add(new ListItem("Spain","ES"));
            this.Items.Add(new ListItem("Sri Lanka","LK"));
            this.Items.Add(new ListItem("Sudan","SD"));
            this.Items.Add(new ListItem("Suriname","SR"));
            this.Items.Add(new ListItem("Swaziland","SZ"));
            this.Items.Add(new ListItem("Sweden","SE"));
            this.Items.Add(new ListItem("Switzerland","CH"));
            this.Items.Add(new ListItem("Syrian Arab Republic","SY"));
            this.Items.Add(new ListItem("Taiwan","TW"));
            this.Items.Add(new ListItem("Tajikistan","TJ"));
            this.Items.Add(new ListItem("Tanzania, United Republic Of","TZ"));
            this.Items.Add(new ListItem("Thailand","TH"));
            this.Items.Add(new ListItem("Timor-leste","TL"));
            this.Items.Add(new ListItem("Togo","TG"));
            this.Items.Add(new ListItem("Tokelau","TK"));
            this.Items.Add(new ListItem("Tonga","TO"));
            this.Items.Add(new ListItem("Trinidad And Tobago","TT"));
            this.Items.Add(new ListItem("Tunisia","TN"));
            this.Items.Add(new ListItem("Turkey","TR"));
            this.Items.Add(new ListItem("Turkmenistan","TM"));
            this.Items.Add(new ListItem("Turks And Caicos Islands","TC"));
            this.Items.Add(new ListItem("Tuvalu","TV"));
            this.Items.Add(new ListItem("Uganda","UG"));
            this.Items.Add(new ListItem("Ukraine","UA"));
            this.Items.Add(new ListItem("United Arab Emirates","AE"));
            this.Items.Add(new ListItem("United States Minor Outlying Islands","UM"));
            this.Items.Add(new ListItem("Uruguay","UY"));
            this.Items.Add(new ListItem("Uzbekistan","UZ"));
            this.Items.Add(new ListItem("Vanuatu","VU"));
            this.Items.Add(new ListItem("Venezuela","VE"));
            this.Items.Add(new ListItem("Viet Nam","VN"));
            this.Items.Add(new ListItem("Virgin Islands, British","VG"));
            this.Items.Add(new ListItem("Virgin Islands, U.s.","VI"));
            this.Items.Add(new ListItem("Yemen","YE"));
            this.Items.Add(new ListItem("Yugoslavia","YU"));
            this.Items.Add(new ListItem("Zambia","ZM"));
            this.Items.Add(new ListItem("Zimbabwe","ZW"));
        }
    }
}
