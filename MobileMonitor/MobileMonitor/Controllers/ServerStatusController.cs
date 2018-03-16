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

        [WebMethod]
        public string RetrieveCpuUsage(int id)
        {
            List<float> cpuList = new List<float>();
            List<ServerStatu> statusList = sproc.ReturnStatus(id).TakeLast(60).ToList();
            foreach (ServerStatu status in statusList)
            {
                string cpu = status.CPUUsage.Replace("%", string.Empty);
                cpuList.Add(float.Parse(cpu));
            }

            var json = JsonConvert.SerializeObject(cpuList);
            return json;
        }

        [WebMethod]
        public string RetrieveRAMAvailable(int id)
        {
            List<float> RAMlist = new List<float>();
            List<ServerStatu> statusList = sproc.ReturnStatus(id).TakeLast(60).ToList();
            foreach (ServerStatu status in statusList)
            {
                string ram = status.MemoryAvailble.Replace("MB Available", string.Empty);
                RAMlist.Add(float.Parse(ram));
            }

            var json = JsonConvert.SerializeObject(RAMlist);
            return json;
        }

        [WebMethod]
        public string RetrieveNetworkAvailable(int id)
        {
            List<float> networkList = new List<float>();
            List<ServerStatu> statusList = sproc.ReturnStatus(id).TakeLast(60).ToList();
            foreach (ServerStatu status in statusList)
            {
                string network = status.NetworkUsage.Replace("%", string.Empty);
                networkList.Add(float.Parse(network));
            }

            var json = JsonConvert.SerializeObject(networkList);
            return json;
        }
    }
}
