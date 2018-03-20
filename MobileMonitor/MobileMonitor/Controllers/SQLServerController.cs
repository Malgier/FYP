using DAL;
using DomainModel;
using MobileMonitor.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MobileMonitor.Controllers
{
    [Authorize]
    public class SQLServerController : Controller
    {
        private StoredProcedureCalls sprocs = new StoredProcedureCalls();
        private PasswordEncryption encryption = new PasswordEncryption();

        // GET: SQLServer
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">ServerID</param>
        /// <returns></returns>
        public ActionResult Index(int id)
        {
            ViewBag.LinkableId = id;
            ViewBag.Server = sprocs.ReturnServer(id).ServerName;
            return View(sprocs.ReturnSQLServers(id));
        }

        // GET: ServerBackups/Create
        public ActionResult Create(int id)
        {
            SQLBackupServer sql = new SQLBackupServer() { Server_ServerID = id };
            return View(sql);
        }

        // POST: ServerBackups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SQLBackupServer sqlBackup)
        {
            if (ModelState.IsValid)
            {
                if (sqlBackup.Password != null)
                    sqlBackup.Password = encryption.Encrypt(sqlBackup.Password);

                sprocs.MergeSQLBackup(sqlBackup);
                return RedirectToAction("Index", new { id = sqlBackup.Server_ServerID });
            }

            return View(sqlBackup);
        }

        // GET: ServerBackups/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(sprocs.ReturnSQLServer((int)id));
        }

        // POST: ServerBackups/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SQLBackupServer sqlBackup)
        {
            if (ModelState.IsValid)
            {
                sprocs.MergeSQLBackup(sqlBackup);
                return RedirectToAction("Index", new { id = sqlBackup.Server_ServerID });
            }

            return View(sqlBackup);
        }

        // GET: SQLServers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SQLBackupServer server = sprocs.ReturnSQLServer((int)id);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        // POST: SQLServers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(SQLBackupServer server)
        {
            sprocs.DeleteSQLServer(server.SQLBackupID);
            return RedirectToAction("Index", new { id = server.Server_ServerID });
        }
    }
}