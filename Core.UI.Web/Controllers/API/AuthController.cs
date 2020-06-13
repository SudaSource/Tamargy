using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core.Data.Models;
using Core.Data.Types;
using Core.Lib.Utilites;
using Core.UI.Web.App_Start.Resources;

namespace Core.UI.Web.Controllers.API
{
    public class AuthController : _Controller
    {

        public async Task<JsonResult> Login(string name, string pass, bool rememberMe)
        {
            HttpCookie userInfoCookies;
            var us = await _sdk.UsersManager.Login(name);
            try
            {
                if (us != null)
                {
                    Session["user"] = us;
                    if (us.SetNewPassword)
                    {
                        return Json(new { state = true, url = RedirectToAction("CreateNewPassword", "Home") },
                            JsonRequestBehavior.AllowGet);
                    }

                    var passConfirm = await _sdk.UsersManager.ConfiremPassword(us, pass);
                    if (passConfirm)
                    {
                        if (rememberMe)
                        {
                            userInfoCookies = new HttpCookie("User")
                            {
                                ["UserName"] = us.Name,
                                ["UserId"] = us.Id.ToString(),
                                Expires = DateTime.Now.AddDays(5)
                            };
                            Response.Cookies.Add(userInfoCookies);
                        }
                        else
                        {
                            userInfoCookies = new HttpCookie("User")
                            {
                                ["UserName"] = us.Name,
                                ["UserId"] = us.Id.ToString(),
                            };
                            Response.Cookies.Add(userInfoCookies);
                        }
                        return Json(new { state = true, url = RedirectToAction("Search", "Home") },
                            JsonRequestBehavior.AllowGet);
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { state = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { state = false, msg = us }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SetNewPassword(string pass, string cpass)
        {
            var us = Session["user"] as User;
            if (us != null && (pass == cpass))
            {
                us.Password = pass;
                var state = await _sdk.UsersManager.SetPassword(us);
                if (state)
                {
                    HttpCookie userInfoCookies = new HttpCookie("User")
                    {
                        ["UserName"] = us.Name,
                        ["UserId"] = us.Id.ToString(),
                        Expires = DateTime.Now.AddDays(5)
                    };
                    Response.Cookies.Add(userInfoCookies);
                    return Json(new { state = true, url = RedirectToAction("Search", "Home") },
                        JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { state = false, msg = Msg.errLogin, url = RedirectToAction("Index", "Home") },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SaveProfileImage()
        {
            try
            {
                byte[] image = null;
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    HttpPostedFile file = System.Web.HttpContext.Current.Request.Files["file"];
                    image = EncryptionManager.ConvertToBytes(file);
                }

                bool state = await _sdk.UsersManager.UpdateImage(image, GetUserId());
                return Json(state, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<JsonResult> ChangeProfilePass(string Pass,string newPass,string cPass)
        {
            try
            {
               
                bool state = await _sdk.UsersManager.UpdatePass(Pass, newPass, GetUserId());
                return Json(state, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateProfile(string name, string phone, string email, Gender gender)
        {
            try
            {

                bool state = await _sdk.UsersManager.UpdateProfile(name,phone,email,gender,GetUserId());
                return Json(state, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Register(string name, string email, string phone, string pass,Gender gender,Role role)
        {
            try
            {
                bool state = await _sdk.UsersManager.Register(name, phone, email, pass,gender, role);
                if (state)
                {
                    return await Login(email, pass, true);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception e)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }

}