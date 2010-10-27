using System;
using System.Collections.Generic;
using System.Text;

namespace Next2Friends.WebServices.Twitter
{
    /// <summary>
    /// The various actions used at Twitter. Not all actions works on all object types.
    /// For more information about the actions types and the supported functions Check the 
    /// Twitter documentation at: http://groups.google.com/group/twitter-development-talk/web/api-documentation.
    /// </summary>
    internal enum ActionType
    {
        Public_Timeline,
        User_Timeline,
        Friends_Timeline,
        Friends,
        Followers,
        Update,
        Account_Settings,
        Featured,
        Show,
    }
}
