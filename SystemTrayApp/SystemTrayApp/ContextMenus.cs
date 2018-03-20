using System;
using System.Diagnostics;
using System.Windows.Forms;
using SystemTrayApp.Properties;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Threading;
using System.Net.NetworkInformation;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Net.Http;
using SystemTrayApp.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace SystemTrayApp
{
    /// <summary>
    /// 
    /// </summary>
    class ContextMenus
    {
        int totalHitsCPU = 0;
        int totalHitsNetwork = 0;
        string serverName = null;
        Models.Server server = new Models.Server();
        Thread threadMonitor;
        Thread threadBackup;
        NotifyIcon icon;
        StoredProcedureCalls sproc;

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ContextMenuStrip</returns>
        public ContextMenuStrip Create()
        {
            // Add the default menu options.
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep;
            serverName = Environment.MachineName;
            icon = new NotifyIcon();
            sproc = new StoredProcedureCalls();
            RegisterServer();
            threadMonitor = new Thread(() => Monitoring(server.ServerID));
            threadBackup = new Thread(() => TrackBackup(server.ServerID));
            threadBackup.Start();

            // Start Monitor.
            item = new ToolStripMenuItem();
            item.Text = "Start Monitor";
            item.Click += new EventHandler(Monitor_Click);
            item.Image = Resources.Explorer;
            menu.Items.Add(item);

            // Stop Monitor.
            item = new ToolStripMenuItem();
            item.Text = "Stop Monitor";
            item.Click += new EventHandler(Stop_Monitor_Click);
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Exit.
            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += new EventHandler(Exit_Click);
            item.Image = Resources.Exit;
            menu.Items.Add(item);

            return menu;
        }

        /// <summary>
        /// Handles the Click event of the Explorer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Monitor_Click(object sender, EventArgs e)
        {
            threadMonitor = new Thread(() => Monitoring(server.ServerID));
            threadMonitor.Start();

            icon.Icon = SystemIcons.Exclamation;
            icon.BalloonTipText = "Monitor Started";
            icon.BalloonTipIcon = ToolTipIcon.Info;
            icon.Visible = true;
            icon.ShowBalloonTip(3000);
        }

        /// <summary>
        /// Handles the Click event of the About control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Stop_Monitor_Click(object sender, EventArgs e)
        {
            threadMonitor.Abort();

            icon.Icon = SystemIcons.Exclamation;
            icon.BalloonTipText = "Monitor Stopped";
            icon.BalloonTipIcon = ToolTipIcon.Info;
            icon.Visible = true;
            icon.ShowBalloonTip(3000);
        }

        /// <summary>
        /// Processes a menu item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Exit_Click(object sender, EventArgs e)
        {
            // Quit without further ado.
            Application.Exit();
        }

        public void RegisterServer()
        {
            server = sproc.ReturnServer(serverName);
            if (server.ServerID == 0)
            {
                Guid g = Guid.NewGuid();
                string serverUniqueCode = Convert.ToBase64String(g.ToByteArray());
                serverUniqueCode = serverUniqueCode.Replace("=", "");
                serverUniqueCode = serverUniqueCode.Replace("+", "");

                //Numbers given are the defult
                server.ServerID = sproc.InsertServer(serverName, serverUniqueCode, 90, 90, 500, 60);

                icon.Icon = SystemIcons.Exclamation;
                icon.BalloonTipText = "Use this code to register your server on the mobile app: " + serverUniqueCode;
                icon.BalloonTipIcon = ToolTipIcon.Info;
                icon.Visible = true;
                icon.ShowBalloonTip(3000);
            }
        }


        /// <summary>
        /// The main monitoring code
        /// </summary>
        /// <param name="serverID">ID of the current server</param>
        public void Monitoring(int serverID)
        {
            string warning = "";
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            int warningID = 0;
            int stop = 0;
            int ticksForOneMinute = (int)(server.TimeWarning / 5);
            Client client = new Client();

            while (stop != 1)
            {
                float cpuPercent = GetCPUCounter();
                float networkUsage = GetBandWidth();
                float ramAvailable = GetAvailableRAM();

                #region CPU Alert Check
                if (cpuPercent >= server.CPUWarningPoint)
                {
                    totalHitsCPU = totalHitsCPU + 1;
                    if (totalHitsCPU == ticksForOneMinute)
                    {
                        warning = "ALERT! CPU over " + server.CPUWarningPoint + "% usage for " + server.TimeWarning + " seconds";
                        startTime = DateTime.Now;
                        warningID = sproc.InsertServerWarning(warning, startTime, new DateTime(), serverID);
                        client.GetClient("http://localhost:2021/", "api/Push");
                    }
                }
                //Check for the end of the warning
                else if (totalHitsCPU >= ticksForOneMinute)
                {
                    endTime = DateTime.Now;

                    sproc.InsertServerWarning(warning, startTime, endTime, serverID, warningID);
                    totalHitsCPU = 0;
                }
                else
                {
                    totalHitsCPU = 0;
                }
                #endregion

                #region RAM Alert Usage
                if (ramAvailable >= server.RAMWarningPoint)
                {
                    totalHitsNetwork = totalHitsNetwork + 1;
                    if (totalHitsNetwork == ticksForOneMinute)
                    {
                        warning = "ALERT! RAM under " + server.RAMWarningPoint + "MB Avaialble for " + server.TimeWarning + " seconds";
                        startTime = DateTime.Now;
                        warningID = sproc.InsertServerWarning(warning, startTime, new DateTime(), serverID);
                        client.GetClient("http://localhost:2021/", "api/Push");
                    }
                }
                //Check for the end of the warning
                else if (totalHitsNetwork >= ticksForOneMinute)
                {
                    endTime = DateTime.Now;

                    sproc.InsertServerWarning(warning, startTime, endTime, serverID, warningID);
                    totalHitsNetwork = 0;
                }
                else
                {
                    totalHitsNetwork = 0;
                }
                #endregion

                #region Network Alert Usage
                if (networkUsage >= server.NetworkWarningPoint)
                {
                    totalHitsNetwork = totalHitsNetwork + 1;
                    if (totalHitsNetwork == ticksForOneMinute)
                    {
                        warning = "ALERT! Network over " + server.NetworkWarningPoint + "% usage for " + server.TimeWarning + " seconds";
                        startTime = DateTime.Now;
                        warningID = sproc.InsertServerWarning(warning, startTime, new DateTime(), serverID);
                        client.GetClient("http://localhost:2021/", "api/Push");
                    }
                }
                //Check for the end of the warning
                else if (totalHitsNetwork >= ticksForOneMinute)
                {
                    endTime = DateTime.Now;

                    sproc.InsertServerWarning(warning, startTime, endTime, serverID, warningID);
                    totalHitsNetwork = 0;
                }
                else
                {
                    totalHitsNetwork = 0;
                }
                #endregion

                sproc.InsertServerSpecs(cpuPercent.ToString() + "%", GetAvailableRAM() + "MB Available", networkUsage.ToString(), serverID);
            }
        }

        /// <summary>
        /// Gets the current CPU Usage Value
        /// </summary>
        /// <returns>The CPU Usage as a float</returns>
        public float GetCPUCounter()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            // will always start at 0
            float firstValue = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(5000);
            // now matches task manager reading
            float secondValue = cpuCounter.NextValue();

            return secondValue;
        }

        /// <summary>
        /// Gets the current available RAM
        /// </summary>
        /// <returns>Returns the RAM value</returns>
        public float GetAvailableRAM()
        {
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            return ramCounter.NextValue();
        }

        /// <summary>
        /// Gets the Network Usage
        /// </summary>
        /// <returns>Returns Network Bandwidth</returns>
        public float GetBandWidth()
        {
            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
            String[] instanceName = category.GetInstanceNames();

            const int numberOfIterations = 10;

            PerformanceCounter bandwidthCounter =
             new PerformanceCounter("Network Interface", "Current Bandwidth", instanceName[0]);

            float bandwidth = bandwidthCounter.NextValue();

            PerformanceCounter dataSentCounter =
             new PerformanceCounter("Network Interface", "Bytes Sent/sec", instanceName[0]);
            PerformanceCounter dataReceivedCounter =
             new PerformanceCounter("Network Interface", "Bytes Received/sec", instanceName[0]);

            float sendSum = 0;
            float receiveSum = 0;

            for (int index = 0; index < numberOfIterations; index++)
            {
                sendSum += dataSentCounter.NextValue();
                receiveSum += dataReceivedCounter.NextValue();
            }

            float dataSent = sendSum;
            float dataReceived = receiveSum;

            float utilization = (8 * (dataSent + dataReceived)) / (bandwidth * numberOfIterations) * 100;

            return utilization;
        }

        public void TrackBackup(int id)
        {
            int stop = 0;
            while (stop != 1)
            {
                SQLBackupServer sqlServer = sproc.ReturnSQLServer(id);
                List<ServerBackup> backupList = sproc.ReturnServerBackups(id);
                foreach (ServerBackup backup in backupList)
                {
                    int i = 0;
                    if (backup.ScheduledTime.ToString("dd/MM/yyy hh:mm") == DateTime.Now.ToString("dd/MM/yyy hh:mm"))
                    {
                        PerformBackup(sqlServer.DatabaseName, sqlServer.Login, sqlServer.Password, backupList[i].Name, backupList[i].BackupType, backupList[i].ExpirationDate, backupList[i].BackupID);
                        if (backup.Frequency == "Daily")
                        {
                            backup.ScheduledTime = backup.ScheduledTime.AddDays(1);
                            backup.ExpirationDate = backup.ExpirationDate.AddDays(1);
                        }
                        else if (backup.Frequency == "Weekly")
                        {
                            backup.ScheduledTime = backup.ScheduledTime.AddDays(7);
                            backup.ExpirationDate = backup.ExpirationDate.AddDays(7);
                        }
                        else if (backup.Frequency == "Monthly")
                        {
                            backup.ScheduledTime = backup.ScheduledTime.AddMonths(1);
                            backup.ExpirationDate = backup.ExpirationDate.AddMonths(1);
                        }
                        sproc.MergeBackup(backup);
                    }
                    i++;
                }
                Thread.Sleep(60000);
            }
        }

        public void PerformBackup(string serverName, string username, string password, string name, string type, DateTime expiration, int backupID)
        {
            string result = "";
            string error = "";

            try
            {
                Microsoft.SqlServer.Management.Smo.Server backupServer = new Microsoft.SqlServer.Management.Smo.Server(serverName);
                ServerConnection conContext = new ServerConnection();
                if (username != "")
                {
                    conContext = backupServer.ConnectionContext;
                    conContext.LoginSecure = false;
                    conContext.Login = username;
                    conContext.Password = Decrypt(password);
                    backupServer = new Microsoft.SqlServer.Management.Smo.Server(conContext);
                }
                else
                {
                    backupServer.ConnectionContext.LoginSecure = true;
                    backupServer.ConnectionContext.Connect();
                }

                Backup bkdb = new Backup();
                bkdb.Action = BackupActionType.Database;
                bkdb.Database = name;
                bkdb.Devices.AddDevice(@"D:\" + name + ".bak", DeviceType.File);
                bkdb.BackupSetName = name + "database backup";
                bkdb.BackupSetDescription = name + " - " + type;
                bkdb.ExpirationDate = expiration;
                bkdb.Initialize = false;
                if (type.Equals("Full Backup"))
                    bkdb.Incremental = false;
                else
                    bkdb.Incremental = true;

                bkdb.SqlBackup(backupServer);
                result = "Backup Successful";
            }
            catch (Exception e)
            {
                error = e.Message;
                result = "Backup Failed";
            }
            finally
            {
                sproc.InsertBackupResult(DateTime.Now, result, error, backupID);
            }
        }

        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}