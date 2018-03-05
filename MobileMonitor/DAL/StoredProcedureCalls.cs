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
        SqlConnection con;
        SqlDataReader reader;

        public StoredProcedureCalls()
        {
            con = new SqlConnection("data source=.\\SQLEXPRESS;initial catalog=ServerMonitor;integrated security=True;");
        }

        public List<int> ReturnUserServers(int userID)
        {
            List<int> serverIDList = new List<int>();
            SqlCommand cmd = new SqlCommand("ReturnServerByUserID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter id = new SqlParameter("@UserID", SqlDbType.Int);
            id.Value = userID;
            cmd.Parameters.Add(id);

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                int i = 0;

                while (reader.Read())
                {
                    serverIDList.Add(reader.GetInt32(i));
                    i++;
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
            return serverIDList;
        }

        public List<Server> ReturnServer(List<int> serverIDs)
        {
            SqlCommand cmd = new SqlCommand("ReturnServerByUserID", con);
            List<string> serverList = new List<string>();
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter id = new SqlParameter("@ServerID", SqlDbType.Int);
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataRow row = dt.Rows[0];

                return employee = new EmployeeVM()
                {
                    EmployeeNumber = (int)row["Number"],
                    EmployeeForename = (string)row["Forename"],
                    EmployeeSurname = (string)row["Surname"],
                    EmployeeDateofBirth = (DateTime)row["DoB"],
                    Department = (int)row["DepartmentID"]
                };
            }

            return serverList;
        }
    }
}