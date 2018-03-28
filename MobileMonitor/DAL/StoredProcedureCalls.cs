using DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class StoredProcedureCalls
    {
        string constring;

        public StoredProcedureCalls()
        {
            constring = "Server=tcp:servermonitorapplication.database.windows.net,1433;Initial Catalog=ServerMonitor;Persist Security Info=False;User ID=Malgier;Password=Darthvader456!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }

        public List<BackupResult> ReturnBackupResult(int backupID, int resultID = 0)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnResult", con))
            {
                List<BackupResult> resultList = new List<BackupResult>();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@BackupID", SqlDbType.Int);
                SqlParameter primaryID = new SqlParameter("@ResultID", SqlDbType.Int);
                id.Value = backupID;
                primaryID.Value = resultID;
                cmd.Parameters.Add(id);
                cmd.Parameters.Add(primaryID);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        string error = "";
                        if (!DBNull.Value.Equals(row["Error"]))
                            error = (string)row["Error"];

                        resultList.Add(new BackupResult()
                        {
                            ResultID = (int)row["ResultID"],
                            DatePerformed = (DateTime)row["DatePerformed"],
                            Result = (string)row["Result"],
                            Error = error,
                            ServerBackup_BackupID = (int)row["ServerBackup_BackupID"]
                        });
                    }
                }

                return resultList;
            }
        }

        public List<int> ReturnUserServers(int userID)
        {
            List<int> serverIDList = new List<int>();
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnServerByUserID", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@UserID", SqlDbType.Int);
                id.Value = userID;
                cmd.Parameters.Add(id);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        serverIDList.Add((int)row["Server_ServerID"]);
                    }
                }
                return serverIDList;
            }
        }

        public List<int> ReturnUsersByServer(int serverID)
        {
            List<int> serverIDList = new List<int>();
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnUsersByServer", con))
            {
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
                        serverIDList.Add((int)row["User_UserID"]);
                    }
                }
                return serverIDList;
            }
        }

        public List<SQLBackupServer> ReturnSQLServers(int id)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnSQLServer", con))
            {
                List<SQLBackupServer> serverList = new List<SQLBackupServer>();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter serverID = new SqlParameter("@ServerID", SqlDbType.Int);
                serverID.Value = id;
                cmd.Parameters.Add(serverID);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        string login = "";
                        if (! DBNull.Value.Equals(row["Login"]))
                            login = (string)row["Login"];

                        string password = "";
                        if (! DBNull.Value.Equals(row["Password"]))
                            password = (string)row["Password"];

                        serverList.Add(new SQLBackupServer()
                        {
                            SQLBackupID = (int)row["SQLBackupID"],
                            DatabaseName = (string)row["DatabaseName"],
                            Login = login,
                            Password = password,
                            Server_ServerID = (int)row["Server_ServerID"]
                        });
                    }
                }

                return serverList;
            }
        }

        public void MergeBackup(ServerBackup serverBackup)
        {
            using (SqlConnection con = new SqlConnection(constring))
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

        public void MergeSQLBackup(SQLBackupServer sqlBackup)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("MergeSQLBackupServer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@SQLBackupID", SqlDbType.Int);
                SqlParameter name = new SqlParameter("@DatabaseName", SqlDbType.VarChar, 50);
                SqlParameter login = new SqlParameter("@Login", SqlDbType.DateTime);
                SqlParameter password = new SqlParameter("@Password", SqlDbType.VarChar, 10);
                SqlParameter serverId = new SqlParameter("@ServerID", SqlDbType.VarChar, 25);

                id.Value = sqlBackup.SQLBackupID;
                name.Value = sqlBackup.DatabaseName;
                login.Value = sqlBackup.Login;
                password.Value = sqlBackup.Password;
                serverId.Value = sqlBackup.Server_ServerID;

                cmd.Parameters.Add(id);
                cmd.Parameters.Add(name);
                cmd.Parameters.Add(login);
                cmd.Parameters.Add(password);
                cmd.Parameters.Add(serverId);

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

        public void InsertServer(Server server)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("InsertServer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter serverID = new SqlParameter("@serverID", SqlDbType.Int);
            SqlParameter serverName = new SqlParameter("@serverName", SqlDbType.VarChar, 50);
            SqlParameter serverDate = new SqlParameter("@DateAdded", SqlDbType.Date);
            SqlParameter serverCode = new SqlParameter("@serverCode", SqlDbType.VarChar, 20);
            SqlParameter cpuPoint = new SqlParameter("@CPUWarningPoint", SqlDbType.Int);
            SqlParameter networkPoint = new SqlParameter("@NetworkWarningPoint", SqlDbType.Int);
            SqlParameter ramPoint = new SqlParameter("@RAMWarningPoint", SqlDbType.Int);
            SqlParameter timeWarning = new SqlParameter("@TimeWarning", SqlDbType.Int);

            serverID.Value = server.ServerID;
            serverName.Value = server.ServerName;
            serverCode.Value = server.ServerUniqueCode;
            serverDate.Value = server.DateAdded;
            cpuPoint.Value = server.CPUWarningPoint;
            networkPoint.Value = server.NetworkWarningPoint;
            ramPoint.Value = server.RAMWarningPoint;
            timeWarning.Value = server.TimeWarning;

            cmd.Parameters.Add(serverID);
            cmd.Parameters.Add(serverName);
            cmd.Parameters.Add(serverCode);
            cmd.Parameters.Add(serverDate);
            cmd.Parameters.Add(cpuPoint);
            cmd.Parameters.Add(networkPoint);
            cmd.Parameters.Add(ramPoint);
            cmd.Parameters.Add(timeWarning);

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

        public void InsertUser(User user)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("InsertUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter username = new SqlParameter("@UserName", SqlDbType.VarChar, 25);
                SqlParameter email = new SqlParameter("@Email", SqlDbType.VarChar, 50);
                SqlParameter password = new SqlParameter("@Password", SqlDbType.VarChar, 100);
                username.Value = user.UserName;
                email.Value = user.Email;
                password.Value = user.Password;
                cmd.Parameters.Add(username);
                cmd.Parameters.Add(email);
                cmd.Parameters.Add(password);

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

        public void InsertUserServer(int userID, int serverID)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("InsertUserServer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter user = new SqlParameter("@UserID", SqlDbType.Int);
                SqlParameter server = new SqlParameter("@ServerID", SqlDbType.Int);
                user.Value = userID;
                server.Value = serverID;
                cmd.Parameters.Add(user);
                cmd.Parameters.Add(server);

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

        public List<Server> ReturnServerID(string serverName, string serverCode)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnServer", con))
            {
                List<Server> serverList = new List<Server>();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter name = new SqlParameter("@ServerName", SqlDbType.VarChar, 50);
                SqlParameter code = new SqlParameter("@ServerCode", SqlDbType.VarChar, 20);
                name.Value = serverName;
                code.Value = serverCode;
                cmd.Parameters.Add(name);
                cmd.Parameters.Add(code);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        serverList.Add(new Server()
                        {
                            ServerID = (int)row["ServerID"],
                            ServerName = (string)row["ServerName"],
                            DateAdded = (DateTime)row["DateAdded"],
                            CPUWarningPoint = (int)row["CPUWarningPoint"],
                            NetworkWarningPoint = (int)row["NetworkWarningPoint"],
                            RAMWarningPoint = (int)row["RAMWarningPoint"],
                            TimeWarning = (int)row["TimeWarning"]
                        });
                    }
                }

                return serverList;
            }
        }

        public List<Server> ReturnServerList(List<int> serverIDs)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnServer", con))
            {
                List<Server> serverList = new List<Server>();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@ServerID", SqlDbType.Int);
                foreach (int serverId in serverIDs)
                {
                    id.Value = serverId;
                    cmd.Parameters.Add(id);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        foreach (DataRow row in dt.Rows)
                        {
                            serverList.Add(new Server()
                            {
                                ServerID = (int)row["ServerID"],
                                ServerName = (string)row["ServerName"],
                                DateAdded = (DateTime)row["DateAdded"],
                                CPUWarningPoint = (int)row["CPUWarningPoint"],
                                NetworkWarningPoint = (int)row["NetworkWarningPoint"],
                                RAMWarningPoint = (int)row["RAMWarningPoint"],
                                TimeWarning = (int)row["TimeWarning"]
                            });
                        }
                    }

                    cmd.Parameters.Clear();
                }

                return serverList;
            }
        }

        public Server ReturnServer(int id)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnServer", con))
            {
                Server server = new Server();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter serverID = new SqlParameter("@ServerID", SqlDbType.Int);
                serverID.Value = id;
                cmd.Parameters.Add(serverID);

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
                            ServerUniqueCode = (string)row["ServerUniqueCode"],
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

        public User ReturnUser(string userName = "", int userId = 0)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnUser", con))
            {
                User user = new User();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@UserID", SqlDbType.Int);
                SqlParameter name = new SqlParameter("@UserName", SqlDbType.VarChar, 25);
                id.Value = userId;
                name.Value = userName;
                cmd.Parameters.Add(id);
                cmd.Parameters.Add(name);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    DataRow row = dt.Rows[0];

                    user = new User()
                    {
                        UserID = (int)row["UserID"],
                        UserName = (string)row["UserName"],
                        Email = (string)row["Email"],
                        Password = (string)row["Password"],
                        Active = (bool)row["Active"]
                    };
                }

                return user;
            }
        }

        public List<string> ReturnAllUsers()
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnAllUsers", con))
            {
                List<string> userList = new List<string>();
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        userList.Add((string)row["Username"]);
                    }
                }

                return userList;
            }
        }

        public List<ServerStatu> ReturnStatus(int id)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnStatus", con))
            {
                List<ServerStatu> statusList = new List<ServerStatu>();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter serverId = new SqlParameter("@ServerID", SqlDbType.Int);
                serverId.Value = id;
                cmd.Parameters.Add(serverId);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        statusList.Add(new ServerStatu()
                        {
                            StatusID = (int)row["StatusID"],
                            CPUUsage = (string)row["CPUUsage"],
                            MemoryAvailble = (string)row["MemoryAvailble"],
                            NetworkUsage = (string)row["NetworkUsage"],
                            DateOfStatus = (DateTime)row["DateOfStatus"],
                            Server_ServerID = (int)row["Server_ServerID"],
                            Active = (bool)row["Active"]
                        });
                    }
                }

                return statusList;
            }
        }

        public List<ServerBackup> ReturnServerBackups(int serverID)
        {
            using (SqlConnection con = new SqlConnection(constring))
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

        public ServerWarning ReturnWarning(int id, DateTime currentDate)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnWarning", con))
            {
                ServerWarning warning = new ServerWarning();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter serverId = new SqlParameter("@ServerID", SqlDbType.Int);
                SqlParameter date = new SqlParameter("@Date", SqlDbType.DateTime);
                serverId.Value = id;
                date.Value = currentDate;
                cmd.Parameters.Add(serverId);
                cmd.Parameters.Add(date);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    DataRow row = dt.Rows[0];

                    warning = new ServerWarning()
                    {
                        WarningID = (int)row["WarningID"],
                        WarningCause = (string)row["WarningCause"],
                        TimeWarningStart = (DateTime)row["TimeWarningStart"],
                        TimeWarningEnd = (DateTime)row["TimeWarningEnd"],
                        Server_ServerID = (int)row["Server_ServerID"]
                    };
                }

                return warning;
            }
        }

        public List<ServerWarning> ReturnWarnings(int serverId)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnServerWarnings", con))
            {
                List<ServerWarning> warning = new List<ServerWarning>();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@ServerID", SqlDbType.Int);
                id.Value = serverId;
                cmd.Parameters.Add(id);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        warning.Add(new ServerWarning()
                        {
                            WarningID = (int)row["WarningID"],
                            WarningCause = (string)row["WarningCause"],
                            TimeWarningStart = (DateTime)row["TimeWarningStart"],
                            TimeWarningEnd = (DateTime)row["TimeWarningEnd"],
                            Server_ServerID = (int)row["Server_ServerID"]
                        });
                    }
                }

                return warning;
            }
        }


        public SQLBackupServer ReturnSQLServer(int sqlId)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnSQLServer", con))
            {
                SQLBackupServer sql = new SQLBackupServer();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@SQLBackupID", SqlDbType.Int);
                id.Value = sqlId;
                cmd.Parameters.Add(id);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
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

                return sql;
            }
        }

        public ServerBackup ReturnServerBackup(int backupId)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnServerBackup", con))
            {
                ServerBackup backup = new ServerBackup();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@BackupID", SqlDbType.Int);
                id.Value = backupId;
                cmd.Parameters.Add(id);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    DataRow row = dt.Rows[0];

                    backup = new ServerBackup()
                    {
                        BackupID = (int)row["BackupID"],
                        Name = (string)row["Name"],
                        ScheduledTime = (DateTime)row["ScheduledTime"],
                        Frequency = (string)row["Frequency"],
                        BackupType = (string)row["BackupType"],
                        ExpirationDate = (DateTime)row["ExpirationDate"],
                        SQLBackupID = (int)row["SQLBackupID"]
                    };
                }

                return backup;
            }
        }

        public void DeleteServerBackup(int backupID)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("DeleteBackup", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@BackupID", SqlDbType.Int);
                id.Value = backupID;
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
        }

        public void DeleteServer(int serverID)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("DeleteServer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@ServerID", SqlDbType.Int);
                id.Value = serverID;
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
        }

        public void DeleteBackupResult(int resultID)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("DeleteBackupResult", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@ResultID", SqlDbType.Int);
                id.Value = resultID;
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
        }

        public void DeleteSQLServer(int sqlServerID)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("DeleteSQLServer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("@SQLServerID", SqlDbType.Int);
                id.Value = sqlServerID;
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
        }
    }
}