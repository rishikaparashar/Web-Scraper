/*
This class will be used as a helper for DB connections.
*/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace WebScraperModularized.helpers
{
    public class DBConnectionHelper
    {
        public static IDbConnection getConnection()
        {
            MyConfigurationHelper myConfigurationManager = new MyConfigurationHelper();
            return new SqlConnection(myConfigurationManager.getDBConnectionString());
        }
    }
}