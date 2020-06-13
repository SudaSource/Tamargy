using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core.Data.Models;
using Core.Lib.SDK;
using Core.UI.Web.Utilites;

namespace Core.UI.Web.Controllers.API
{
    public class _Controller : Controller
    {
        public SDK _sdk { get; set; }

        public _Controller()
        {
            _sdk = new SDK();
        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            try
            {
                string lang = null;
                HttpCookie langCookie = Request.Cookies["culture"];
                if (langCookie != null)
                {
                    lang = langCookie.Value;
                }
                else
                {
                    var userLanguage = Request.UserLanguages;
                    var userLang = userLanguage != null ? userLanguage[0] : "";
                    if (userLang != "")
                    {
                        lang = userLang;
                    }
                    else
                    {
                        lang = LanguageMang.GetDefaultLanguage();
                    }
                }
                new LanguageMang().SetLanguage(lang);
                return base.BeginExecuteCore(callback, state);
            }
            catch (Exception e)
            {
                return base.BeginExecuteCore(callback, state);
            }
        }


        protected async Task<User> GetUser()
        {
            try
            {
                User user = null;
                HttpCookie userCookie = Request.Cookies["User"];
                if (userCookie != null)
                {
                    user = await _sdk.UsersManager.GetSingleUser(long.Parse(userCookie["UserId"]));
                }
                if (Session["user"] is User && user == null)
                {
                    user = Session["user"] as User;
                }

                return user;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        protected long GetUserId()
        {
            try
            {
                HttpCookie userCookie = Request.Cookies["User"];
                return (userCookie != null) ? long.Parse(userCookie["UserId"]) : 0;
            }
            catch (Exception e)
            {
                return 0;
            }

        }
    }
}