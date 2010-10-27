using System;
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
using Next2Friends.Data;

namespace Next2Friends.WebControls
{
    /// <summary>
    /// Summary description for HobbyDropdown
    /// </summary>
    public class HobbyDropdown : DropDownList
    {
        public HobbyDropdown()
        {
            Populate();
            this.Width = new Unit(178);            
        }

        private void Populate()
        {
            List<Hobby> hobbies = Hobby.GetAllHobby();


            IOrderedEnumerable<Hobby> sortedHobbies = null;

            sortedHobbies = from P in hobbies orderby P.Name ascending select P;
            hobbies = sortedHobbies.ToList();

            foreach (Hobby hob in hobbies)
            {
                this.Items.Add(new ListItem(hob.Name, hob.HobbyID.ToString()));
            }
        }
    }
}
