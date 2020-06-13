using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core.UI.Web.Controllers.API;
using Core.UI.Web.Utilites;

namespace Core.UI.Web.Controllers
{
    public class HomeController : _Controller
    {
        public ActionResult Search()
        {
            return View();
        }

        public ActionResult Home(string search)
        {
            ViewBag.Q = search;
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateNewPassword()
        {
            return View();
        }

        public ActionResult ChangeLanguage(string lang, string url)
        {
            new LanguageMang().SetLanguage(lang);
            return Redirect(url);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            HttpCookie userInfoCookies = new HttpCookie("User") { Expires = DateTime.Now.AddDays(-1) };
            Response.Cookies.Add(userInfoCookies);
            return RedirectToAction("Index");

        }

        public ActionResult Register()
        {
            return View();
        }

        public async Task<ActionResult> Profile()
        {
            return View(await GetUser());
        }

    }
}