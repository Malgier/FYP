using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using MoreLinq;
using DAL;
using DomainModel;

namespace MobileMonitor.Controllers
{
    [Authorize]
    public class ServerStatusController : Controller
    {
        private StoredProcedureCalls sproc = new StoredProcedureCalls();

        // GET: ServerStatus
        public ActionResult Index(int id)
        {
            ViewBag.LinkableId = id;
            return View(id);
        }

        public ActionResult StatusHistory(int id)
        {
            ViewBag.LinkableId = id;
            return View();
        }

        [WebMethod]
        public string RetrieveStatus(int id)
        {
            List<ServerStatu> statusList = sproc.ReturnStatus(id).Where(x => x.DateOfStatus.Date == DateTime.Now.Date).TakeLast(60).ToList();
            foreach (ServerStatu status in statusList)
            {
                status.CPUUsage = status.CPUUsage.Replace("%", string.Empty);
                status.MemoryAvailble = status.MemoryAvailble.Replace("MB Available", string.Empty);
                status.NetworkUsage = status.NetworkUsage.Replace("%", string.Empty);
            }

            var json = JsonConvert.SerializeObject(statusList);
            return json;
        }

        [WebMethod]
        public string RetrievePreivousDayStatus(int id)
        {
            List<ServerStatu> statusList = sproc.ReturnStatus(id).Where(x => x.DateOfStatus.Date == DateTime.Now.Date.AddDays(-1)).ToList();
            foreach (ServerStatu status in statusList)
            {
                status.CPUUsage = status.CPUUsage.Replace("%", string.Empty);
                status.MemoryAvailble = status.MemoryAvailble.Replace("MB Available", string.Empty);
                status.NetworkUsage = status.NetworkUsage.Replace("%", string.Empty);
            }

            var json = JsonConvert.SerializeObject(statusList);
            return json;
        }
    }
}
