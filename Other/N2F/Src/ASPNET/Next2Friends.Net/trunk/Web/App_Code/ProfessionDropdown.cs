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
    /// Summary description for ProfessionDropdown
    /// </summary>
    public class ProfessionDropdown : DropDownList
    {
        public ProfessionDropdown()
        {
            Populate();
            this.Width = new Unit(178);            
        }

        private void Populate()
        {
            List<Profession> professions = Profession.GetAllProfession();

            
            IOrderedEnumerable<Profession> sortedProfessions = null;

            sortedProfessions = from P in professions orderby P.Name ascending select P;
            professions = sortedProfessions.ToList();

            foreach (Profession prof in professions)
            {
                this.Items.Add(new ListItem(prof.Name, prof.ProfessionID.ToString()));
            }
        }
    }
}
