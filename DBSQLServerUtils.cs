using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCantin
{
    class DBSQLServerUtils
    {
        public static SqlConnection
            GetDBConnection(string datasource, string database/*, string username, string password*/)
        {
            //
            // Data Source=TRAN-VMWARE\SQLEXPRESS;Initial Catalog=simplehr;Persist Security Info=True;User ID=sa;Password=12345
            //
            string connString = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Integrated Security=SSPI;";

            SqlConnection conn = new SqlConnection(connString);

            return conn;
        }
    }
}
