using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MobileMonitor.Models;

namespace MobileMonitor.Controllers
{
    [Authorize]
    public class ServerBackupsController : Controller
    {
        private ServerMonitorEntities db = new ServerMonitorEntities();

        // GET: ServerBackups
        public ActionResult Index()
        {
            var serverBackups = db.ServerBackups.Include(s => s.Server);
            return View(serverBackups.ToList());
        }

        // GET: ServerBackups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServerBackup serverBackup = db.ServerBackups.Find(id);
            if (serverBackup == null)
            {
                return HttpNotFound();
            }
            return View(serverBackup);
        }

        // GET: ServerBackups/Create
        public ActionResult Create()
        {
            ViewBag.Server_ServerID = new SelectList(db.Servers, "ServerID", "ServerName");
            return View();
        }

        // POST: ServerBackups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BackupID,Name,ScheduledTime,Frequency,BackupType,Active,Server_ServerID")] ServerBackup serverBackup)
        {
            if (ModelState.IsValid)
            {
                db.ServerBackups.Add(serverBackup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Server_ServerID = new SelectList(db.Servers, "ServerID", "ServerName", serverBackup.Server_ServerID);
            return View(serverBackup);
        }

        // GET: ServerBackups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServerBackup serverBackup = db.ServerBackups.Find(id);
            if (serverBackup == null)
            {
                return HttpNotFound();
            }
            ViewBag.Server_ServerID = new SelectList(db.Servers, "ServerID", "ServerName", serverBackup.Server_ServerID);
            return View(serverBackup);
        }

        // POST: ServerBackups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BackupID,Name,ScheduledTime,Frequency,BackupType,Active,Server_ServerID")] ServerBackup serverBackup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serverBackup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Server_ServerID = new SelectList(db.Servers, "ServerID", "ServerName", serverBackup.Server_ServerID);
            return View(serverBackup);
        }

        // GET: ServerBackups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServerBackup serverBackup = db.ServerBackups.Find(id);
            if (serverBackup == null)
            {
                return HttpNotFound();
            }
            return View(serverBackup);
        }

        // POST: ServerBackups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServerBackup serverBackup = db.ServerBackups.Find(id);
            db.ServerBackups.Remove(serverBackup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
