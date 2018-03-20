using DAL;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        // GET: ServerResults/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BackupResult result = sprocs.ReturnBackupResult(0, (int)id).SingleOrDefault();
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: ServerResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(BackupResult result)
        {
            sprocs.DeleteBackupResult(result.ResultID);
            return RedirectToAction("Index", new { backupID = result.ServerBackup_BackupID });
        }
    }
}
