using MobileMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace MobileMonitor.Login
{
    public class SessionContext
    {

        public void SetAuthenticationToken(string name, bool isPersistant, User userData)
        {
            HttpContext context = HttpContext.Current;

            string data = null;
            if (userData != null)
                data = new JavaScriptSerializer().Serialize(userData);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, name, DateTime.Now, DateTime.Now.AddYears(1), isPersistant, userData.UserID.ToString());

            string cookieData = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieData)
            {
                HttpOnly = true,
                Expires = ticket.Expiration
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
            context.Session["UserID"] = userData.UserID.ToString();
            context.Session["UserName"] = userData.UserName;
        }

        public int? GetUserID()
        {
            int? userData = 0;

            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                    userData = new JavaScriptSerializer().Deserialize(ticket.UserData, typeof(int)) as int?;
                }
            }
            catch (Exception ex)
            {
            }

            return userData;
        }
    }
}