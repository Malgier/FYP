using MobileMonitor.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DAL
{
    public class StoredProcedureCalls
    {
        string constring;

        public StoredProcedureCalls()
        {
            constring = "Server=tcp:servermonitorapplication.database.windows.net,1433;Initial Catalog=ServerMonitor;Persist Security Info=False;User ID=Malgier;Password=Darthvader456!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
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
                            DateAdded = (DateTime)row["DateAdded"]
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
                                DateAdded = (DateTime)row["DateAdded"]
                            });
                        }
                    }

                    cmd.Parameters.Clear();
                }

                return serverList;
            }
        }

        public User ReturnUser(string userName, string password)
        {
            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand("ReturnUser", con))
            {
                User user = new User();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter name = new SqlParameter("@UserName", SqlDbType.VarChar, 25);
                SqlParameter pass = new SqlParameter("@Password", SqlDbType.VarChar, 50);
                name.Value = userName;
                pass.Value = password;
                cmd.Parameters.Add(name);
                cmd.Parameters.Add(pass);

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
                            Server_ServerID = (int)row["Server_ServerID"],
                            Active = (bool)row["Active"]
                        });
                    }
                }

                return statusList;
            }
        }
    }
}