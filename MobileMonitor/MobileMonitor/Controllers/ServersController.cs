using System.Collections.Generic;
using System.Web.Mvc;
using DAL;
using MobileMonitor.Login;
using DomainModel;
using System.Net;

namespace MobileMonitor.Controllers
{
    [Authorize]
    public class ServersController : Controller
    {
        private SessionContext userContext = new SessionContext();
        private StoredProcedureCalls sproc = new StoredProcedureCalls();

        // GET: Servers
        public ActionResult Index()
        {
            int userID = userContext.GetUserID().Value;
            List<int> serverList = sproc.ReturnUserServers(userID);

            return View(sproc.ReturnServerList(serverList));
        }

        // GET: Servers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Servers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Server server)
        {
            if (ModelState.IsValid)
            {
                int userID = userContext.GetUserID().Value;
                List<Server> serverList = sproc.ReturnServerID("", server.ServerUniqueCode);
                if(userID == 0 || serverList[0].ServerID == 0 || serverList.Count > 1)
                    return View(server);

                sproc.InsertUserServer(userID, serverList[0].ServerID);
                return RedirectToAction("Index");
            }

            return View(server);
        }

        // GET: ServerBackups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(sproc.ReturnServer((int)id));
        }

        // POST: ServerBackups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Server server)
        {
            if (ModelState.IsValid)
            {
                sproc.InsertServer(server);
                return RedirectToAction("Index");
            }
            return View(server);
        }

        // GET: Servers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Server server = sproc.ReturnServer((int)id);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        // POST: Servers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Server server)
        {
            sproc.DeleteServer(server.ServerID);
            return RedirectToAction("Index");
        }
    }
}
