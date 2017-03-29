using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UserWebApplication.Models
{
    public static class SessionInfo    {
        public static bool IsLoggedIn
        {
            get
            {
                if (HttpContext.Current.Session["Users"] != null)
                    return true;
                else
                    return false;
            }
        }
      
        public static UserModel CurrentUserSession
        {
            get
            {
                return (UserModel)HttpContext.Current.Session["Users"];
            }
            set
            {
                HttpContext.Current.Session["Users"] = value;
            }
        }
    }
}
