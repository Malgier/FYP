using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SystemTrayApp.Models;

namespace SystemTrayApp
{
    public class StoredProcedureCalls
    {
        string connectionString;

        public StoredProcedureCalls()
        {
            connectionString = "Server=tcp:servermonitorapplication.database.windows.net,1433;Initial Catalog=ServerMonitor;Persist Security Info=False;User ID=Malgier;Password=Darthvader456!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }
        /// <summary>
        /// Executes a Stored Procedure to insert the hardware data into the database
        /// </summary>
        /// <param name="cpuPercent">The CPU Usage as a String</param>
        /// <param name="ramUsage">The available RAM as a String</param>
        /// <param name="networkUsage">The current network use as a String</param>
        /// <param name="ServerID">The current server ID</param>
        public void InsertServerSpecs(string cpuPercent, string ramUsage, string networkUsage, int ServerID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("InsertSpec", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter cpu = new SqlParameter("@CpuUsage", SqlDbType.VarChar, 50);
            SqlParameter ram = new SqlParameter("@MemoryUsage", SqlDbType.VarChar, 50);
            SqlParameter net = new SqlParameter("@NetworkUsage", SqlDbType.VarChar, 50);
            SqlParameter serverID = new SqlParameter("@ServerID", SqlDbType.Int);
            cpu.Value = cpuPercent;
            ram.Value = ramUsage;
            net.Value = networkUsage;
            serverID.Value = ServerID;

            cmd.Parameters.Add(cpu);
            cmd.Parameters.Add(ram);
            cmd.Parameters.Add(net);
            cmd.Parameters.Add(serverID);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {

            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        public int InsertServer(string name, string code, int cpu, int net, int ram, int time)
        {
            int id = 0;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("InsertServer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter serverID = new SqlParameter("@serverID", SqlDbType.Int);
            SqlParameter serverName = new SqlParameter("@serverName", SqlDbType.VarChar, 50);
            SqlParameter serverCode = new SqlParameter("@serverCode", SqlDbType.VarChar, 20);
            SqlParameter cpuPoint = new SqlParameter("@CPUWarningPoint", SqlDbType.Int);
            SqlParameter networkPoint = new SqlParameter("@NetworkWarningPoint", SqlDbType.Int);
            SqlParameter ramPoint = new SqlParameter("@RAMWarningPoint", SqlDbType.Int);
            SqlParameter timeWarning = new SqlParameter("@TimeWarning", SqlDbType.Int);


            serverName.Value = name;
            serverCode.Value = code;
            cpuPoint.Value = cpu;
            networkPoint.Value = net;
            ramPoint.Value = ram;
            timeWarning.Value = time;
            serverID.Direction = ParameterDirection.ReturnValue;

            cmd.Parameters.Add(serverName);
            cmd.Parameters.Add(serverCode);
            cmd.Parameters.Add(serverID);
            cmd.Parameters.Add(cpuPoint);
            cmd.Parameters.Add(networkPoint);
            cmd.Parameters.Add(ramPoint);
            cmd.Parameters.Add(timeWarning);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                id = (int)serverID.Value;
            }
            catch (Exception e)
            {

            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }

            return id;
        }

        public void InsertBackupResult(DateTime performed, string result, string error, int backupID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("InsertBackupResult", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter datePerformed = new SqlParameter("@DatePerformed", SqlDbType.DateTime);
            SqlParameter backupResult = new SqlParameter("@Result", SqlDbType.VarChar, 50);
            SqlParameter backupError = new SqlParameter("@Error", SqlDbType.VarChar, 100);
            SqlParameter id = new SqlParameter("@BackupID", SqlDbType.Int);

            datePerformed.Value = performed;
            backupResult.Value = result;
            backupError.Value = error;
            id.Value = backupID;

            cmd.Parameters.Add(datePerformed);
            cmd.Parameters.Add(backupResult);
            cmd.Parameters.Add(backupError);
            cmd.Parameters.Add(id);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {

            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        public void MergeBackup(ServerBackup serverBackup)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("MergeBackup", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@BackupID", SqlDbType.Int);
                SqlParameter name = new SqlParameter("@Name", SqlDbType.VarChar, 25);
                SqlParameter scheduledTime = new SqlParameter("@ScheduledTime", SqlDbType.DateTime);
                SqlParameter frequency = new SqlParameter("@Frequency", SqlDbType.VarChar, 10);
                SqlParameter backupType = new SqlParameter("@BackupType", SqlDbType.VarChar, 25);
                SqlParameter expirationDate = new SqlParameter("@ExpirationDate", SqlDbType.Date);
                SqlParameter sqlBackupID = new SqlParameter("@SQLBackupID", SqlDbType.Int);

                id.Value = serverBackup.BackupID;
                name.Value = serverBackup.Name;
                scheduledTime.Value = serverBackup.ScheduledTime;
                frequency.Value = serverBackup.Frequency;
                backupType.Value = serverBackup.BackupType;
                expirationDate.Value = serverBackup.ExpirationDate;
                sqlBackupID.Value = serverBackup.SQLBackupID;

                cmd.Parameters.Add(id);
                cmd.Parameters.Add(name);
                cmd.Parameters.Add(scheduledTime);
                cmd.Parameters.Add(frequency);
                cmd.Parameters.Add(backupType);
                cmd.Parameters.Add(expirationDate);
                cmd.Parameters.Add(sqlBackupID);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {

                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }
        }

        public Server ReturnServer(string name)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("ReturnServer", con))
            {
                Server server = new Server();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter serverName = new SqlParameter("@ServerName", SqlDbType.VarChar, 50);
                serverName.Value = name;
                cmd.Parameters.Add(serverName);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        server = new Server()
                        {
                            ServerID = (int)row["ServerID"],
                            ServerName = (string)row["ServerName"],
                            DateAdded = (DateTime)row["DateAdded"],
                            CPUWarningPoint = (int)row["CPUWarningPoint"],
                            NetworkWarningPoint = (int)row["NetworkWarningPoint"],
                            RAMWarningPoint = (int)row["RAMWarningPoint"],
                            TimeWarning = (int)row["TimeWarning"]
                        };
                    }
                }

                return server;
            }
        }

        public int InsertServerWarning(string warning, DateTime start, DateTime end, int ServerID, int WarningID = 0)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("InsertWarning", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter warningID = new SqlParameter("@WarningID", SqlDbType.Int);
            SqlParameter warningCause = new SqlParameter("@WarningCause", SqlDbType.VarChar, 100);
            SqlParameter warningStart = new SqlParameter("@WarningStart", SqlDbType.DateTime);
            SqlParameter warningEnd = new SqlParameter("@WarningEnd", SqlDbType.DateTime);
            SqlParameter serverID = new SqlParameter("@ServerID", SqlDbType.Int);
            warningID.Value = WarningID;

            if (WarningID == 0)
                warningID.Direction = ParameterDirection.ReturnValue;

            warningCause.Value = warning;
            warningStart.Value = start;
            warningEnd.Value = end;
            serverID.Value = ServerID;

            cmd.Parameters.Add(warningID);
            cmd.Parameters.Add(warningCause);
            cmd.Parameters.Add(warningStart);
            cmd.Parameters.Add(warningEnd);
            cmd.Parameters.Add(serverID);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                WarningID = (int)warningID.Value;
            }
            catch (Exception e)
            {

            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }

            return WarningID;
        }

        public SQLBackupServer ReturnSQLServer(int serverId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("ReturnSQLServer", con))
            {
                SQLBackupServer sql = new SQLBackupServer();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@ServerID", SqlDbType.Int);
                id.Value = serverId;
                cmd.Parameters.Add(id);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count != 0)
                    {
                        DataRow row = dt.Rows[0];

                        string login = "";
                        if (!DBNull.Value.Equals(row["Login"]))
                            login = (string)row["Login"];

                        string password = "";
                        if (!DBNull.Value.Equals(row["Password"]))
                            password = (string)row["Password"];

                        sql = new SQLBackupServer()
                        {
                            SQLBackupID = (int)row["SQLBackupID"],
                            DatabaseName = (string)row["DatabaseName"],
                            Login = login,
                            Password = password,
                            Server_ServerID = (int)row["Server_ServerID"]
                        };
                    }
                }

                return sql;
            }
        }

        public List<ServerBackup> ReturnServerBackups(int serverID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("ReturnServerBackup", con))
            {
                List<ServerBackup> backupList = new List<ServerBackup>();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@ServerID", SqlDbType.Int);
                id.Value = serverID;
                cmd.Parameters.Add(id);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        backupList.Add(new ServerBackup()
                        {
                            BackupID = (int)row["BackupID"],
                            Name = (string)row["Name"],
                            ScheduledTime = (DateTime)row["ScheduledTime"],
                            Frequency = (string)row["Frequency"],
                            BackupType = (string)row["BackupType"],
                            ExpirationDate = (DateTime)row["ExpirationDate"],
                            SQLBackupID = (int)row["SQLBackupID"]
                        });
                    }
                }

                return backupList;
            }
        }
    }
}
