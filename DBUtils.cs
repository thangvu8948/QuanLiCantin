using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCantin
{
    class DBUtils
    {
        public static SqlConnection GetDBConnection()
        {
            string datasource = @"DESKTOP-S11RR64\SQLEXPRESS";

            string database = "qlct";

            return DBSQLServerUtils.GetDBConnection(datasource, database);
        }
    }
}
