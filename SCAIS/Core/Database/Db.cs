using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SCAIS.Core.Database
{
    public static class Db
    {
        public static string ConnStr
        {
            get { return ConfigurationManager.ConnectionStrings["SCAIS_DB"].ConnectionString; }
        }

        public static DataTable Query(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(ConnStr))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
