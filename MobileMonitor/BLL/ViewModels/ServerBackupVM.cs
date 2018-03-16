using DAL;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BLL.ViewModels
{
    public class ServerBackupVM
    {
        private StoredProcedureCalls sprocs = new StoredProcedureCalls();
        public ServerBackup backup = new ServerBackup();
        public List<SQLBackupServer> list = new List<SQLBackupServer>();
        public List<SelectListItem> SQLServer = new List<SelectListItem>();
        public List<SelectListItem> frequency = new List<SelectListItem>();
        public List<SelectListItem> type = new List<SelectListItem>();

        public ServerBackupVM(int id, int serverId)
        {
            Setup(id, serverId);
        }

        private void Setup(int id, int serverId)
        {
            if(id != 0)
                backup = sprocs.ReturnServerBackup(id);

            list = sprocs.ReturnSQLServers(serverId);
            bool selectedSQL = false;

            SQLServer.Add(new SelectListItem() { Text = "----Select SQL Database----", Value = "----Select SQL Database----" });
            foreach (SQLBackupServer item in list)
            {
                if (item.SQLBackupID == backup.SQLBackupID)
                    selectedSQL = true;
                SQLServer.Add(new SelectListItem() { Text = item.DatabaseName, Value = item.SQLBackupID.ToString(), Selected = selectedSQL });
                selectedSQL = false;
            }
            if (id != 0)
            {
                if (backup.Frequency.Equals("Daily"))
                {
                    frequency.Add(new SelectListItem() { Text = "----Select Frequency----", Value = "----Select Frequency----" });
                    frequency.Add(new SelectListItem() { Text = "Daily", Value = "Daily", Selected = true });
                    frequency.Add(new SelectListItem() { Text = "Weekly", Value = "Weekly" });
                    frequency.Add(new SelectListItem() { Text = "Monthly", Value = "Monthly" });
                }
                else if (backup.Frequency.Equals("Weekly"))
                {
                    frequency.Add(new SelectListItem() { Text = "----Select Frequency----", Value = "----Select Frequency----" });
                    frequency.Add(new SelectListItem() { Text = "Daily", Value = "Daily" });
                    frequency.Add(new SelectListItem() { Text = "Weekly", Value = "Weekly", Selected = true });
                    frequency.Add(new SelectListItem() { Text = "Monthly", Value = "Monthly" });
                }
                else if (backup.Frequency.Equals("Monthly"))
                {
                    frequency.Add(new SelectListItem() { Text = "----Select Frequency----", Value = "----Select Frequency----" });
                    frequency.Add(new SelectListItem() { Text = "Daily", Value = "Daily" });
                    frequency.Add(new SelectListItem() { Text = "Weekly", Value = "Weekly" });
                    frequency.Add(new SelectListItem() { Text = "Monthly", Value = "Monthly", Selected = true });
                }

                if (backup.BackupType.Equals("Full Backup"))
                {
                    type.Add(new SelectListItem() { Text = "----Select Backup Type----", Value = "----Select Backup Type----" });
                    type.Add(new SelectListItem() { Text = "Full Backup", Value = "Full Backup", Selected = true });
                    type.Add(new SelectListItem() { Text = "Incremental Backup", Value = "Incremental Backup" });
                }
                else if (backup.BackupType.Equals("Full Backup"))
                {
                    type.Add(new SelectListItem() { Text = "----Select Backup Type----", Value = "----Select Backup Type----" });
                    type.Add(new SelectListItem() { Text = "Full Backup", Value = "Full Backup" });
                    type.Add(new SelectListItem() { Text = "Incremental Backup", Value = "Incremental Backup", Selected = true });
                }
            }
            else
            {
                frequency.Add(new SelectListItem() { Text = "----Select Frequency----", Value = "----Select Frequency----" });
                frequency.Add(new SelectListItem() { Text = "Daily", Value = "Daily" });
                frequency.Add(new SelectListItem() { Text = "Weekly", Value = "Weekly" });
                frequency.Add(new SelectListItem() { Text = "Monthly", Value = "Monthly" });

                type.Add(new SelectListItem() { Text = "----Select Backup Type----", Value = "----Select Backup Type----" });
                type.Add(new SelectListItem() { Text = "Full Backup", Value = "Full Backup" });
                type.Add(new SelectListItem() { Text = "Incremental Backup", Value = "Incremental Backup" });
            }
        }
    }
}
