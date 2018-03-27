using DAL;
using DomainModel;
using MobileMonitor.Login;
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
        StoredProcedureCalls sprocs;
        PasswordEncryption encryption;

        public AccountController()
        {
            userApp = new UserApplication();
            context = new SessionContext();
            sprocs = new StoredProcedureCalls();
            encryption = new PasswordEncryption();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var authenticatedUser = userApp.GetByUsernameAndPassword(user);
            if (encryption.Decrypt(authenticatedUser.Password) == user.Password)
            {
                if (authenticatedUser != null)
                {
                    context.SetAuthenticationToken(authenticatedUser.UserID.ToString(), false, authenticatedUser);
                    return RedirectToAction("Index", "Servers");
                }
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
                List<string> usernames = sprocs.ReturnAllUsers();
                foreach (string name in usernames)
                    if (name.ToUpper().Equals(user.UserName.ToUpper()))
                        return View();

                user.Password = encryption.Encrypt(user.Password);
                sprocs.InsertUser(user);
                return RedirectToAction("Login");
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}