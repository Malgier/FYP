using BLL.ViewModels;
using DAL;
using DomainModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MobileMonitor.Controllers
{
    [Authorize]
    public class ServerBackupsController : Controller
    {
        private StoredProcedureCalls sprocs = new StoredProcedureCalls();

        // GET: ServerBackups
        public ActionResult Index(int serverId)
        {
            ViewBag.LinkableId = serverId;
            var serverBackups = sprocs.ReturnServerBackups(serverId);
            List<string> sqlServerName = new List<string>();
            foreach (ServerBackup backup in serverBackups)
            {
                sqlServerName.Add(sprocs.ReturnSQLServer(backup.SQLBackupID).DatabaseName);
            }
            ViewBag.DatabaseNames = sqlServerName;
            return View(serverBackups.ToList());
        }

        // GET: ServerBackups/Create
        public ActionResult Create(int serverId)
        {
            ServerBackupVM vm = new ServerBackupVM(0, serverId);

            ViewBag.Frequency = vm.frequency;
            ViewBag.BackupType = vm.type;
            ViewBag.SQLBackup = vm.SQLServer;

            return View();
        }

        // POST: ServerBackups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServerBackup serverBackup)
        {
            if (ModelState.IsValid)
            {
                sprocs.MergeBackup(serverBackup);
                return RedirectToAction("Index", new { serverId = sprocs.ReturnSQLServer(serverBackup.SQLBackupID).Server_ServerID });
            }
            //ServerBackupVM vm = new ServerBackupVM(0, serverId);
            //ViewBag.Frequency = vm.frequency;
            //ViewBag.BackupType = vm.type;
            //ViewBag.SQLBackup = vm.SQLServer;

            return View(serverBackup);
        }

        // GET: ServerBackups/Edit/5
        public ActionResult Edit(int? id, int serverId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ServerBackupVM vm = new ServerBackupVM((int)id, serverId);
            ViewBag.Frequency = vm.frequency;
            ViewBag.BackupType = vm.type;
            ViewBag.SQLBackup = vm.SQLServer;

            return View(vm.backup);
        }

        // POST: ServerBackups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServerBackup serverBackup)
        {
            int serverId = sprocs.ReturnSQLServer(serverBackup.SQLBackupID).Server_ServerID;
            if (ModelState.IsValid)
            {
                sprocs.MergeBackup(serverBackup);
                return RedirectToAction("Index", new { serverId = serverId });
            }
            ServerBackupVM vm = new ServerBackupVM(serverBackup.BackupID, serverId);
            ViewBag.Frequency = vm.frequency;
            ViewBag.BackupType = vm.type;
            ViewBag.SQLBackup = vm.SQLServer;

            return View(serverBackup);
        }

        // GET: ServerBackups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServerBackup serverBackup = sprocs.ReturnServerBackup((int)id);
            if (serverBackup == null)
            {
                return HttpNotFound();
            }
            return View(serverBackup);
        }

        // POST: ServerBackups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(ServerBackup serverBackup)
        {
            sprocs.DeleteServerBackup(serverBackup.BackupID);
            return RedirectToAction("Index", new { serverId = sprocs.ReturnSQLServer(serverBackup.SQLBackupID).Server_ServerID });
        }
    }
}
