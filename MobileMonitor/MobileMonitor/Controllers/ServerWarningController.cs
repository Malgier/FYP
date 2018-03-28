using DAL;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileMonitor.Controllers
{
    public class ServerWarningController : Controller
    {
        // GET: ServerWarning
        public ActionResult Index(int serverID)
        {
            StoredProcedureCalls sprocs = new StoredProcedureCalls();
            List<ServerWarning> warnings = sprocs.ReturnWarnings(serverID);
            ViewBag.Server = sprocs.ReturnServer(serverID).ServerName;
            return View(warnings);
        }
    }
}