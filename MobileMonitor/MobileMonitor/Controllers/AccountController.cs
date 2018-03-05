using MobileMonitor.Login;
using MobileMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MobileMonitor.Controllers
{
    public class AccountController : Controller
    {
        UserApplication userApp;
        SessionContext context;
        private ServerMonitorEntities db;

        public AccountController()
        {
            userApp = new UserApplication();
            context = new SessionContext();
            db = new ServerMonitorEntities();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            var authenticatedUser = userApp.GetByUsernameAndPassword(user);
            if (authenticatedUser != null)
            {
                context.SetAuthenticationToken(authenticatedUser.UserID.ToString(), false, authenticatedUser);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.Active = true;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session["UserID"] = "";
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}