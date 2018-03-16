using DAL;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileMonitor.Controllers
{
    [Authorize]
    public class ServerResultController : Controller
    {
        private StoredProcedureCalls sprocs = new StoredProcedureCalls();

        // GET: ServerResult
        public ActionResult Index(int backupID)
        {
            List<BackupResult> resultList = sprocs.ReturnBackupResult(backupID);
            if (resultList.Count != 0)
            {
                ViewBag.BackupName = sprocs.ReturnServerBackup(resultList[0].ServerBackup_BackupID).Name;
                return View(sprocs.ReturnBackupResult(backupID));
            }
            else
            {
                return View(new List<BackupResult>());
            }
        }

        // GET: ServerResult/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ServerResult/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
